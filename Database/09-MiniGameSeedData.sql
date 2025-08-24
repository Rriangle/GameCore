-- =============================================
-- GameCore 小遊戲系統種子資料
-- 建立完整的冒險遊戲測試資料，包含多樣化的遊戲記錄、勝負統計、關卡進度等
-- 嚴格按照規格要求生成符合Asia/Taipei時區和每日限制的遊戲資料
-- =============================================

USE GameCore;
GO

PRINT '開始插入小遊戲系統種子資料...';

-- 清除現有的小遊戲記錄 (重新生成)
PRINT '清除現有小遊戲記錄...';
DELETE FROM MiniGame;

-- 重置自增ID
DBCC CHECKIDENT ('MiniGame', RESEED, 0);

PRINT '開始生成小遊戲記錄...';

-- 為前50名使用者建立遊戲記錄
DECLARE @UserId INT = 1;
DECLARE @MaxUserId INT = (SELECT MIN(50, MAX(User_ID)) FROM Users);
DECLARE @GameCount INT = 0;

WHILE @UserId <= @MaxUserId
BEGIN
    PRINT '為使用者 ' + CAST(@UserId AS VARCHAR) + ' 建立遊戲記錄...';
    
    -- 每個使用者隨機生成3-15場遊戲記錄 (跨越多天)
    DECLARE @UserGameCount INT = 3 + (ABS(CHECKSUM(NEWID())) % 13); -- 3-15場
    DECLARE @GameIndex INT = 0;
    
    -- 從30天前開始生成遊戲記錄
    DECLARE @StartDate DATETIME2 = DATEADD(DAY, -30, GETUTCDATE());
    
    WHILE @GameIndex < @UserGameCount
    BEGIN
        -- 隨機選擇遊戲日期 (最近30天內)
        DECLARE @DaysFromStart INT = ABS(CHECKSUM(NEWID())) % 30;
        DECLARE @GameDate DATETIME2 = DATEADD(DAY, @DaysFromStart, @StartDate);
        
        -- 隨機選擇當天的時間 (Asia/Taipei 9:00-22:00)
        DECLARE @HourOffset INT = 9 + (ABS(CHECKSUM(NEWID())) % 14); -- 9-22點
        DECLARE @MinuteOffset INT = ABS(CHECKSUM(NEWID())) % 60;
        
        -- 轉換為UTC時間 (Asia/Taipei UTC+8)
        DECLARE @GameStartTime DATETIME2 = DATEADD(HOUR, @HourOffset - 8, 
                                                  DATEADD(MINUTE, @MinuteOffset, @GameDate));
        
        -- 確保不超過當前時間
        IF @GameStartTime > GETUTCDATE()
            SET @GameStartTime = DATEADD(HOUR, -1, GETUTCDATE());
        
        -- 計算使用者當前等級 (基於之前的勝利記錄)
        DECLARE @CurrentLevel INT = 1;
        SELECT @CurrentLevel = ISNULL(MAX(Level), 0) + 1
        FROM MiniGame 
        WHERE UserID = @UserId 
        AND Result = 1 
        AND StartTime < @GameStartTime
        AND NOT Aborted = 1;
        
        -- 限制最高等級
        IF @CurrentLevel > 20 SET @CurrentLevel = 20;
        IF @CurrentLevel < 1 SET @CurrentLevel = 1;
        
        -- 根據關卡設定怪物數和速度
        DECLARE @MonsterCount INT, @SpeedMultiplier DECIMAL(3,2);
        SELECT 
            @MonsterCount = CASE 
                WHEN @CurrentLevel = 1 THEN 6
                WHEN @CurrentLevel = 2 THEN 8  
                WHEN @CurrentLevel = 3 THEN 10
                ELSE 6 + (@CurrentLevel * 2) -- 動態增加
            END,
            @SpeedMultiplier = CASE 
                WHEN @CurrentLevel = 1 THEN 1.0
                WHEN @CurrentLevel = 2 THEN 1.2
                WHEN @CurrentLevel = 3 THEN 1.5
                ELSE 1.0 + (@CurrentLevel * 0.1) -- 動態增加
            END;
        
        -- 隨機決定遊戲結果 (考慮關卡難度)
        DECLARE @VictoryChance INT = CASE 
            WHEN @CurrentLevel <= 3 THEN 70  -- 前3關較容易
            WHEN @CurrentLevel <= 10 THEN 60 -- 中等關卡
            ELSE 45 -- 高等關卡較困難
        END;
        
        DECLARE @RandomResult INT = ABS(CHECKSUM(NEWID())) % 100;
        DECLARE @IsVictory BIT = CASE WHEN @RandomResult < @VictoryChance THEN 1 ELSE 0 END;
        
        -- 小概率產生中斷遊戲 (5%)
        DECLARE @IsAborted BIT = CASE WHEN (ABS(CHECKSUM(NEWID())) % 100) < 5 THEN 1 ELSE 0 END;
        
        -- 如果中斷則無勝負結果
        IF @IsAborted = 1 SET @IsVictory = NULL;
        
        -- 計算遊戲時長 (30秒到10分鐘)
        DECLARE @DurationSeconds INT = 30 + (ABS(CHECKSUM(NEWID())) % 570); -- 30-600秒
        DECLARE @DurationMinutes DECIMAL(5,2) = CAST(@DurationSeconds AS DECIMAL(5,2)) / 60.0;
        DECLARE @GameEndTime DATETIME2 = DATEADD(SECOND, @DurationSeconds, @GameStartTime);
        
        -- 計算擊敗怪物數 (根據結果)
        DECLARE @MonstersDefeated INT = CASE 
            WHEN @IsAborted = 1 THEN ABS(CHECKSUM(NEWID())) % (@MonsterCount / 2) -- 中斷時較少
            WHEN @IsVictory = 1 THEN @MonsterCount -- 勝利時全部擊敗
            ELSE (ABS(CHECKSUM(NEWID())) % @MonsterCount) -- 失敗時部分擊敗
        END;
        
        -- 計算最終分數 (基於擊敗怪物數和時間)
        DECLARE @FinalScore INT = (@MonstersDefeated * 100) + 
                                  CASE WHEN @DurationSeconds < 120 THEN 200 -- 速度獎勵
                                       WHEN @DurationSeconds < 300 THEN 100
                                       ELSE 50 END;
        
        -- 如果中斷，分數減半
        IF @IsAborted = 1 SET @FinalScore = @FinalScore / 2;
        
        -- 計算獎勵 (只有勝利才有獎勵)
        DECLARE @ExpGained INT = 0, @PointsGained INT = 0;
        IF @IsVictory = 1 AND @IsAborted = 0
        BEGIN
            SET @ExpGained = @CurrentLevel * 100;
            SET @PointsGained = @CurrentLevel * 10;
        END
        
        -- 插入遊戲記錄
        INSERT INTO MiniGame (
            UserID, Level, MonsterCount, SpeedMultiplier,
            StartTime, EndTime, DurationMinutes,
            Result, Aborted, MonstersDefeated, FinalScore,
            ExpGained, PointsChanged
        )
        VALUES (
            @UserId, @CurrentLevel, @MonsterCount, @SpeedMultiplier,
            @GameStartTime, @GameEndTime, @DurationMinutes,
            @IsVictory, @IsAborted, @MonstersDefeated, @FinalScore,
            @ExpGained, @PointsGained
        );
        
        SET @GameCount = @GameCount + 1;
        SET @GameIndex = @GameIndex + 1;
        
        -- 模擬每日限制：同一天最多3場非中斷遊戲
        DECLARE @TodayGameCount INT;
        SELECT @TodayGameCount = COUNT(*)
        FROM MiniGame 
        WHERE UserID = @UserId 
        AND CAST(@GameStartTime AS DATE) = CAST(StartTime AS DATE)
        AND Aborted = 0;
        
        -- 如果當天已達3場，跳到下一天
        IF @TodayGameCount >= 3
        BEGIN
            SET @StartDate = DATEADD(DAY, 1, @StartDate);
        END
    END
    
    SET @UserId = @UserId + 1;
END

PRINT '基本遊戲記錄生成完成！總計: ' + CAST(@GameCount AS VARCHAR) + ' 場遊戲';

-- 生成特殊情況的遊戲記錄
PRINT '生成特殊遊戲記錄...';

-- VIP使用者的高等級遊戲記錄 (ID 1-5)
DECLARE @VipUserId INT = 1;
WHILE @VipUserId <= 5
BEGIN
    -- 為VIP用戶生成一些高等級勝利記錄
    DECLARE @HighLevelCount INT = 0;
    WHILE @HighLevelCount < 5
    BEGIN
        DECLARE @HighLevel INT = 15 + @HighLevelCount;
        DECLARE @VipGameTime DATETIME2 = DATEADD(DAY, -@HighLevelCount, GETUTCDATE());
        
        INSERT INTO MiniGame (
            UserID, Level, MonsterCount, SpeedMultiplier,
            StartTime, EndTime, DurationMinutes,
            Result, Aborted, MonstersDefeated, FinalScore,
            ExpGained, PointsChanged
        )
        VALUES (
            @VipUserId, @HighLevel, 6 + (@HighLevel * 2), 1.0 + (@HighLevel * 0.1),
            @VipGameTime, DATEADD(MINUTE, 8, @VipGameTime), 8.0,
            1, 0, 6 + (@HighLevel * 2), @HighLevel * 200,
            @HighLevel * 100, @HighLevel * 10
        );
        
        SET @HighLevelCount = @HighLevelCount + 1;
    END
    
    SET @VipUserId = @VipUserId + 1;
END

-- 生成今日的遊戲記錄 (測試每日限制)
PRINT '生成今日遊戲記錄...';

DECLARE @TodayUserId INT = 10;
WHILE @TodayUserId <= 15
BEGIN
    -- 為這些使用者生成今日的遊戲記錄 (1-3場)
    DECLARE @TodayGameIndex INT = 0;
    DECLARE @TodayGameLimit INT = 1 + (ABS(CHECKSUM(NEWID())) % 3); -- 1-3場
    
    WHILE @TodayGameIndex < @TodayGameLimit
    BEGIN
        DECLARE @TodayGameTime DATETIME2 = DATEADD(HOUR, @TodayGameIndex * 2 - 8, -- 轉UTC
                                                  DATEADD(HOUR, 10 + @TodayGameIndex, 
                                                  CAST(GETUTCDATE() AS DATE))); -- Asia/Taipei 10點開始
        
        INSERT INTO MiniGame (
            UserID, Level, MonsterCount, SpeedMultiplier,
            StartTime, EndTime, DurationMinutes,
            Result, Aborted, MonstersDefeated, FinalScore,
            ExpGained, PointsChanged
        )
        VALUES (
            @TodayUserId, 
            1 + @TodayGameIndex, -- 遞增關卡
            6 + (@TodayGameIndex * 2), 
            1.0 + (@TodayGameIndex * 0.2),
            @TodayGameTime, 
            DATEADD(MINUTE, 5, @TodayGameTime), 
            5.0,
            1, -- 都設為勝利
            0, 
            6 + (@TodayGameIndex * 2), 
            (1 + @TodayGameIndex) * 150,
            (1 + @TodayGameIndex) * 100, 
            (1 + @TodayGameIndex) * 10
        );
        
        SET @TodayGameIndex = @TodayGameIndex + 1;
    END
    
    SET @TodayUserId = @TodayUserId + 1;
END

-- 生成一些中斷的遊戲記錄
PRINT '生成中斷遊戲記錄...';

INSERT INTO MiniGame (
    UserID, Level, MonsterCount, SpeedMultiplier,
    StartTime, EndTime, DurationMinutes,
    Result, Aborted, MonstersDefeated, FinalScore,
    ExpGained, PointsChanged
)
SELECT TOP 20
    UserID,
    1 + (ABS(CHECKSUM(NEWID())) % 5), -- 關卡1-5
    6,
    1.0,
    DATEADD(DAY, -(ABS(CHECKSUM(NEWID())) % 10), GETUTCDATE()), -- 最近10天
    DATEADD(MINUTE, 2, DATEADD(DAY, -(ABS(CHECKSUM(NEWID())) % 10), GETUTCDATE())),
    2.0,
    NULL, -- 中斷無勝負
    1,    -- 標記為中斷
    ABS(CHECKSUM(NEWID())) % 3, -- 少量擊敗
    50,   -- 低分
    0,    -- 無經驗獎勵
    0     -- 無點數獎勵
FROM Users
WHERE User_ID <= 30;

PRINT '特殊遊戲記錄生成完成！';

-- 統計報告
PRINT '=== 小遊戲系統種子資料統計報告 ===';

-- 總遊戲數量
DECLARE @TotalGames INT = (SELECT COUNT(*) FROM MiniGame);
PRINT '總遊戲場次: ' + CAST(@TotalGames AS VARCHAR);

-- 遊戲結果分布
PRINT '遊戲結果分布:';
SELECT 
    CASE 
        WHEN Result = 1 THEN '勝利'
        WHEN Result = 0 THEN '失敗'
        WHEN Aborted = 1 THEN '中斷'
        ELSE '異常'
    END as 遊戲結果,
    COUNT(*) as 場次,
    CAST(COUNT(*) * 100.0 / @TotalGames AS DECIMAL(5,2)) as 佔比
FROM MiniGame
GROUP BY 
    CASE 
        WHEN Result = 1 THEN '勝利'
        WHEN Result = 0 THEN '失敗'
        WHEN Aborted = 1 THEN '中斷'
        ELSE '異常'
    END
ORDER BY 場次 DESC;

-- 關卡分布
PRINT '關卡分布:';
SELECT 
    CASE 
        WHEN Level BETWEEN 1 AND 3 THEN '初級(1-3)'
        WHEN Level BETWEEN 4 AND 10 THEN '中級(4-10)'
        WHEN Level BETWEEN 11 AND 20 THEN '高級(11-20)'
        ELSE '超級(20+)'
    END as 關卡範圍,
    COUNT(*) as 遊戲次數,
    CAST(COUNT(*) * 100.0 / @TotalGames AS DECIMAL(5,2)) as 佔比
FROM MiniGame
GROUP BY 
    CASE 
        WHEN Level BETWEEN 1 AND 3 THEN '初級(1-3)'
        WHEN Level BETWEEN 4 AND 10 THEN '中級(4-10)'
        WHEN Level BETWEEN 11 AND 20 THEN '高級(11-20)'
        ELSE '超級(20+)'
    END
ORDER BY 關卡範圍;

-- 玩家遊戲統計
PRINT '玩家遊戲參與統計:';
SELECT 
    CASE 
        WHEN game_count = 1 THEN '新手(1場)'
        WHEN game_count BETWEEN 2 AND 5 THEN '初級(2-5場)'
        WHEN game_count BETWEEN 6 AND 15 THEN '中級(6-15場)'
        ELSE '資深(15+場)'
    END as 玩家類型,
    COUNT(*) as 玩家數量
FROM (
    SELECT UserID, COUNT(*) as game_count
    FROM MiniGame
    WHERE Aborted = 0 -- 排除中斷
    GROUP BY UserID
) player_stats
GROUP BY 
    CASE 
        WHEN game_count = 1 THEN '新手(1場)'
        WHEN game_count BETWEEN 2 AND 5 THEN '初級(2-5場)'
        WHEN game_count BETWEEN 6 AND 15 THEN '中級(6-15場)'
        ELSE '資深(15+場)'
    END
ORDER BY 玩家數量 DESC;

-- 獎勵統計
PRINT '獎勵發放統計:';
SELECT 
    SUM(ExpGained) as 總經驗發放,
    SUM(PointsChanged) as 總點數發放,
    AVG(ExpGained) as 平均單場經驗,
    AVG(PointsChanged) as 平均單場點數
FROM MiniGame
WHERE Result = 1 AND Aborted = 0; -- 只計算勝利場次

-- 遊戲時長統計
PRINT '遊戲時長統計:';
SELECT 
    CAST(AVG(DurationMinutes) AS DECIMAL(5,2)) as 平均時長_分鐘,
    CAST(MIN(DurationMinutes) AS DECIMAL(5,2)) as 最短時長_分鐘,
    CAST(MAX(DurationMinutes) AS DECIMAL(5,2)) as 最長時長_分鐘
FROM MiniGame
WHERE Aborted = 0;

-- 今日遊戲統計
PRINT '今日遊戲統計:';
DECLARE @TodayStart DATETIME2 = CAST(GETUTCDATE() AS DATE);
DECLARE @TodayEnd DATETIME2 = DATEADD(DAY, 1, @TodayStart);

SELECT 
    COUNT(*) as 今日總場次,
    COUNT(CASE WHEN Result = 1 THEN 1 END) as 今日勝利場次,
    COUNT(DISTINCT UserID) as 今日活躍玩家數
FROM MiniGame
WHERE StartTime >= @TodayStart AND StartTime < @TodayEnd;

-- 高分榜 TOP 10
PRINT '最高分遊戲 TOP 10:';
SELECT TOP 10
    u.User_name as 玩家名稱,
    mg.Level as 關卡,
    mg.FinalScore as 最終分數,
    mg.MonstersDefeated as 擊敗怪物,
    CAST(mg.DurationMinutes AS DECIMAL(5,2)) as 遊戲時長_分鐘,
    CASE WHEN mg.Result = 1 THEN '勝利' ELSE '失敗' END as 結果
FROM MiniGame mg
JOIN Users u ON mg.UserID = u.User_ID
WHERE mg.Aborted = 0
ORDER BY mg.FinalScore DESC;

-- 最高關卡玩家
PRINT '最高關卡挑戰者:';
SELECT TOP 10
    u.User_name as 玩家名稱,
    MAX(mg.Level) as 最高關卡,
    COUNT(*) as 總遊戲次數,
    SUM(CASE WHEN mg.Result = 1 THEN 1 ELSE 0 END) as 勝利次數,
    CAST(SUM(CASE WHEN mg.Result = 1 THEN 1 ELSE 0 END) * 100.0 / COUNT(*) AS DECIMAL(5,2)) as 勝率
FROM MiniGame mg
JOIN Users u ON mg.UserID = u.User_ID
WHERE mg.Aborted = 0
GROUP BY mg.UserID, u.User_name
ORDER BY MAX(mg.Level) DESC, 勝率 DESC;

-- 每日次數限制驗證
PRINT '每日次數限制驗證:';
WITH DailyGameCounts AS (
    SELECT 
        UserID,
        CAST(StartTime AS DATE) as GameDate,
        COUNT(*) as DailyCount
    FROM MiniGame
    WHERE Aborted = 0 -- 排除中斷遊戲
    GROUP BY UserID, CAST(StartTime AS DATE)
)
SELECT 
    CASE 
        WHEN DailyCount <= 3 THEN '符合限制(≤3場)'
        ELSE '超出限制(>3場)'
    END as 限制檢查,
    COUNT(*) as 天數統計
FROM DailyGameCounts
GROUP BY 
    CASE 
        WHEN DailyCount <= 3 THEN '符合限制(≤3場)'
        ELSE '超出限制(>3場)'
    END;

-- 關卡通過率分析
PRINT '關卡通過率分析:';
SELECT 
    Level as 關卡,
    COUNT(*) as 嘗試次數,
    SUM(CASE WHEN Result = 1 THEN 1 ELSE 0 END) as 通過次數,
    CAST(SUM(CASE WHEN Result = 1 THEN 1 ELSE 0 END) * 100.0 / COUNT(*) AS DECIMAL(5,2)) as 通過率,
    AVG(FinalScore) as 平均分數
FROM MiniGame
WHERE Aborted = 0 AND Level <= 10 -- 分析前10關
GROUP BY Level
ORDER BY Level;

PRINT '小遊戲系統種子資料插入完成！';
GO