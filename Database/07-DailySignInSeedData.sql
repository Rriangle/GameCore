-- =============================================
-- GameCore 每日簽到系統種子資料
-- 建立完整的簽到歷史記錄，包含連續簽到獎勵、週末加成等
-- 嚴格按照Asia/Taipei時區和獎勵規則生成測試資料
-- =============================================

USE GameCore;
GO

PRINT '開始插入每日簽到系統種子資料...';

-- 設定Asia/Taipei時區 (+8小時)
DECLARE @TimezoneOffset INT = 8;

-- 清除現有的簽到記錄 (重新生成)
PRINT '清除現有簽到記錄...';
DELETE FROM UserSignInStats;

-- 重置自增ID
DBCC CHECKIDENT ('UserSignInStats', RESEED, 0);

PRINT '開始生成簽到記錄...';

-- 為每個使用者生成過去90天的簽到記錄
DECLARE @UserId INT;
DECLARE @MaxUserId INT = (SELECT MAX(User_ID) FROM Users);
DECLARE @CurrentUserId INT = 1;

WHILE @CurrentUserId <= @MaxUserId
BEGIN
    PRINT '為使用者 ' + CAST(@CurrentUserId AS VARCHAR) + ' 生成簽到記錄...';
    
    -- 每個使用者的簽到機率和特性
    DECLARE @UserSignInRate DECIMAL(3,2) = 0.70 + (RAND(CHECKSUM(NEWID())) * 0.25); -- 70%-95%簽到率
    DECLARE @UserStreakBonus BIT = CASE WHEN RAND(CHECKSUM(NEWID())) > 0.5 THEN 1 ELSE 0 END; -- 50%機率有連續獎勵
    
    -- 生成過去90天的記錄
    DECLARE @DaysBack INT = 0;
    DECLARE @CurrentStreak INT = 0;
    DECLARE @LastSignInDate DATE = NULL;
    
    WHILE @DaysBack < 90
    BEGIN
        -- 計算Taipei時區的日期
        DECLARE @TaipeiDate DATE = CAST(DATEADD(HOUR, @TimezoneOffset, DATEADD(DAY, -@DaysBack, GETUTCDATE())) AS DATE);
        DECLARE @SignInTime DATETIME2 = DATEADD(
            MINUTE, 
            FLOOR(RAND(CHECKSUM(NEWID())) * 1440), -- 一天內隨機時間
            DATEADD(HOUR, -@TimezoneOffset, CAST(@TaipeiDate AS DATETIME2)) -- 轉回UTC
        );
        
        -- 判斷是否簽到 (基於使用者簽到率和其他因素)
        DECLARE @ShouldSignIn BIT = 0;
        DECLARE @SignInProbability DECIMAL(3,2) = @UserSignInRate;
        
        -- 週末提高簽到機率
        IF DATEPART(WEEKDAY, @TaipeiDate) IN (1, 7) -- 週日=1, 週六=7
        BEGIN
            SET @SignInProbability = @SignInProbability + 0.1;
        END
        
        -- 連續簽到使用者傾向繼續簽到
        IF @CurrentStreak > 0 AND @UserStreakBonus = 1
        BEGIN
            SET @SignInProbability = @SignInProbability + 0.15;
        END
        
        -- 決定是否簽到
        IF RAND(CHECKSUM(NEWID())) < @SignInProbability
        BEGIN
            SET @ShouldSignIn = 1;
        END
        
        -- 執行簽到邏輯
        IF @ShouldSignIn = 1
        BEGIN
            -- 檢查連續性
            IF @LastSignInDate IS NOT NULL AND DATEDIFF(DAY, @LastSignInDate, @TaipeiDate) = 1
            BEGIN
                SET @CurrentStreak = @CurrentStreak + 1;
            END
            ELSE
            BEGIN
                SET @CurrentStreak = 1;
            END
            
            -- 計算獎勵
            DECLARE @BasePoints INT;
            DECLARE @BaseExp INT;
            DECLARE @BonusPoints INT = 0;
            DECLARE @BonusExp INT = 0;
            
            -- 基礎獎勵 (平日/週末)
            IF DATEPART(WEEKDAY, @TaipeiDate) IN (1, 7) -- 週末
            BEGIN
                SET @BasePoints = 30;
                SET @BaseExp = 200;
            END
            ELSE -- 平日
            BEGIN
                SET @BasePoints = 20;
                SET @BaseExp = 0;
            END
            
            -- 連續7天獎勵
            IF @CurrentStreak = 7
            BEGIN
                SET @BonusPoints = @BonusPoints + 40;
                SET @BonusExp = @BonusExp + 300;
            END
            
            -- 當月全勤獎勵 (月末最後一天且全勤)
            DECLARE @IsLastDayOfMonth BIT = 0;
            IF @TaipeiDate = EOMONTH(@TaipeiDate)
            BEGIN
                -- 檢查本月是否全勤 (簡化：有90%以上簽到率就給獎勵)
                IF RAND(CHECKSUM(NEWID())) < 0.1 AND @UserSignInRate > 0.85
                BEGIN
                    SET @BonusPoints = @BonusPoints + 200;
                    SET @BonusExp = @BonusExp + 2000;
                    SET @IsLastDayOfMonth = 1;
                END
            END
            
            -- 插入簽到記錄
            INSERT INTO UserSignInStats (
                SignTime, 
                UserID, 
                PointsChanged, 
                ExpGained, 
                PointsChangedTime, 
                ExpGainedTime
            )
            VALUES (
                @SignInTime,
                @CurrentUserId,
                @BasePoints + @BonusPoints,
                @BaseExp + @BonusExp,
                @SignInTime,
                @SignInTime
            );
            
            SET @LastSignInDate = @TaipeiDate;
        END
        ELSE
        BEGIN
            -- 沒有簽到，連續天數歸零
            SET @CurrentStreak = 0;
        END
        
        SET @DaysBack = @DaysBack + 1;
    END
    
    SET @CurrentUserId = @CurrentUserId + 1;
END

PRINT '簽到記錄生成完成！';

-- 生成一些特殊的簽到記錄示例
PRINT '生成特殊簽到記錄示例...';

-- VIP使用者 (ID 1-5) 的完美簽到記錄
DECLARE @VipUserId INT = 1;
WHILE @VipUserId <= 5
BEGIN
    -- 為VIP使用者補充最近7天的完美簽到
    DECLARE @VipDay INT = 0;
    WHILE @VipDay < 7
    BEGIN
        DECLARE @VipTaipeiDate DATE = CAST(DATEADD(HOUR, @TimezoneOffset, DATEADD(DAY, -@VipDay, GETUTCDATE())) AS DATE);
        DECLARE @VipSignInTime DATETIME2 = DATEADD(
            HOUR, -@TimezoneOffset + 9, -- 早上9點簽到
            CAST(@VipTaipeiDate AS DATETIME2)
        );
        
        -- 檢查是否已存在記錄
        IF NOT EXISTS (
            SELECT 1 FROM UserSignInStats 
            WHERE UserID = @VipUserId 
            AND CAST(DATEADD(HOUR, @TimezoneOffset, SignTime) AS DATE) = @VipTaipeiDate
        )
        BEGIN
            DECLARE @VipPoints INT = CASE WHEN DATEPART(WEEKDAY, @VipTaipeiDate) IN (1, 7) THEN 30 ELSE 20 END;
            DECLARE @VipExp INT = CASE WHEN DATEPART(WEEKDAY, @VipTaipeiDate) IN (1, 7) THEN 200 ELSE 0 END;
            
            -- 第7天給連續獎勵
            IF @VipDay = 0 -- 今天是第7天
            BEGIN
                SET @VipPoints = @VipPoints + 40;
                SET @VipExp = @VipExp + 300;
            END
            
            INSERT INTO UserSignInStats (
                SignTime, UserID, PointsChanged, ExpGained, PointsChangedTime, ExpGainedTime
            )
            VALUES (
                @VipSignInTime, @VipUserId, @VipPoints, @VipExp, @VipSignInTime, @VipSignInTime
            );
        END
        
        SET @VipDay = @VipDay + 1;
    END
    
    SET @VipUserId = @VipUserId + 1;
END

-- 生成本月全勤使用者記錄
PRINT '生成本月全勤使用者記錄...';
DECLARE @PerfectUserId INT = 10;
DECLARE @MonthStart DATE = DATEFROMPARTS(YEAR(GETUTCDATE()), MONTH(GETUTCDATE()), 1);
DECLARE @CurrentDate DATE = @MonthStart;
DECLARE @MonthEnd DATE = EOMONTH(@MonthStart);

WHILE @CurrentDate <= @MonthEnd
BEGIN
    -- 跳過今天之後的日期
    IF @CurrentDate > CAST(GETUTCDATE() AS DATE)
        BREAK;
        
    DECLARE @PerfectSignInTime DATETIME2 = DATEADD(
        HOUR, -@TimezoneOffset + 8 + FLOOR(RAND(CHECKSUM(NEWID())) * 12), -- 8-20點隨機
        CAST(@CurrentDate AS DATETIME2)
    );
    
    -- 檢查是否已存在記錄
    IF NOT EXISTS (
        SELECT 1 FROM UserSignInStats 
        WHERE UserID = @PerfectUserId 
        AND CAST(DATEADD(HOUR, @TimezoneOffset, SignTime) AS DATE) = @CurrentDate
    )
    BEGIN
        DECLARE @PerfectPoints INT = CASE WHEN DATEPART(WEEKDAY, @CurrentDate) IN (1, 7) THEN 30 ELSE 20 END;
        DECLARE @PerfectExp INT = CASE WHEN DATEPART(WEEKDAY, @CurrentDate) IN (1, 7) THEN 200 ELSE 0 END;
        
        INSERT INTO UserSignInStats (
            SignTime, UserID, PointsChanged, ExpGained, PointsChangedTime, ExpGainedTime
        )
        VALUES (
            @PerfectSignInTime, @PerfectUserId, @PerfectPoints, @PerfectExp, @PerfectSignInTime, @PerfectSignInTime
        );
    END
    
    SET @CurrentDate = DATEADD(DAY, 1, @CurrentDate);
END

-- 更新使用者錢包餘額 (加上簽到獲得的點數)
PRINT '更新使用者錢包餘額...';
UPDATE uw
SET User_Point = uw.User_Point + ISNULL(signin.total_points, 0)
FROM User_wallet uw
LEFT JOIN (
    SELECT UserID, SUM(PointsChanged) as total_points
    FROM UserSignInStats
    GROUP BY UserID
) signin ON uw.User_Id = signin.UserID;

-- 統計報告
PRINT '=== 每日簽到系統種子資料統計報告 ===';

-- 總簽到記錄數
DECLARE @TotalRecords INT = (SELECT COUNT(*) FROM UserSignInStats);
PRINT '總簽到記錄數: ' + CAST(@TotalRecords AS VARCHAR);

-- 有簽到記錄的使用者數
DECLARE @UsersWithRecords INT = (SELECT COUNT(DISTINCT UserID) FROM UserSignInStats);
PRINT '有簽到記錄的使用者數: ' + CAST(@UsersWithRecords AS VARCHAR);

-- 總發放點數
DECLARE @TotalPoints INT = (SELECT SUM(PointsChanged) FROM UserSignInStats);
PRINT '總發放點數: ' + CAST(@TotalPoints AS VARCHAR);

-- 總發放經驗
DECLARE @TotalExp INT = (SELECT SUM(ExpGained) FROM UserSignInStats);
PRINT '總發放經驗: ' + CAST(@TotalExp AS VARCHAR);

-- 連續獎勵記錄數 (點數>40的記錄)
DECLARE @BonusRecords INT = (SELECT COUNT(*) FROM UserSignInStats WHERE PointsChanged > 40);
PRINT '含連續獎勵的記錄數: ' + CAST(@BonusRecords AS VARCHAR);

-- 週末簽到記錄數 (經驗>0的記錄)
DECLARE @WeekendRecords INT = (SELECT COUNT(*) FROM UserSignInStats WHERE ExpGained > 0 AND ExpGained <= 200);
PRINT '週末簽到記錄數: ' + CAST(@WeekendRecords AS VARCHAR);

-- 月度全勤獎勵記錄數 (經驗>=2000的記錄)
DECLARE @MonthlyBonusRecords INT = (SELECT COUNT(*) FROM UserSignInStats WHERE ExpGained >= 2000);
PRINT '月度全勤獎勵記錄數: ' + CAST(@MonthlyBonusRecords AS VARCHAR);

-- 最近7天簽到情況
PRINT '最近7天簽到情況:';
SELECT 
    CAST(DATEADD(HOUR, @TimezoneOffset, SignTime) AS DATE) as 簽到日期,
    COUNT(*) as 簽到人數,
    SUM(PointsChanged) as 總發放點數,
    SUM(ExpGained) as 總發放經驗,
    AVG(CAST(PointsChanged AS FLOAT)) as 平均點數,
    MAX(PointsChanged) as 最高點數
FROM UserSignInStats
WHERE SignTime >= DATEADD(DAY, -7, GETUTCDATE())
GROUP BY CAST(DATEADD(HOUR, @TimezoneOffset, SignTime) AS DATE)
ORDER BY 簽到日期 DESC;

-- 使用者簽到統計 TOP 10
PRINT '簽到統計 TOP 10:';
SELECT TOP 10
    u.User_name as 使用者名稱,
    COUNT(*) as 簽到天數,
    SUM(s.PointsChanged) as 獲得點數,
    SUM(s.ExpGained) as 獲得經驗,
    AVG(CAST(s.PointsChanged AS FLOAT)) as 平均點數,
    MAX(s.PointsChanged) as 最高單次點數
FROM UserSignInStats s
JOIN Users u ON s.UserID = u.User_ID
GROUP BY s.UserID, u.User_name
ORDER BY 簽到天數 DESC, 獲得點數 DESC;

-- 獎勵分布統計
PRINT '獎勵分布統計:';
SELECT 
    CASE 
        WHEN PointsChanged = 20 THEN '平日基礎(20)'
        WHEN PointsChanged = 30 THEN '週末基礎(30)'
        WHEN PointsChanged = 60 THEN '平日+連續獎勵(60)'
        WHEN PointsChanged = 70 THEN '週末+連續獎勵(70)'
        WHEN PointsChanged > 100 THEN '含月度獎勵(>100)'
        ELSE '其他(' + CAST(PointsChanged AS VARCHAR) + ')'
    END as 獎勵類型,
    COUNT(*) as 記錄數量,
    CAST(COUNT(*) * 100.0 / @TotalRecords AS DECIMAL(5,2)) as 佔比
FROM UserSignInStats
GROUP BY 
    CASE 
        WHEN PointsChanged = 20 THEN '平日基礎(20)'
        WHEN PointsChanged = 30 THEN '週末基礎(30)'
        WHEN PointsChanged = 60 THEN '平日+連續獎勵(60)'
        WHEN PointsChanged = 70 THEN '週末+連續獎勵(70)'
        WHEN PointsChanged > 100 THEN '含月度獎勵(>100)'
        ELSE '其他(' + CAST(PointsChanged AS VARCHAR) + ')'
    END
ORDER BY 記錄數量 DESC;

-- 驗證Asia/Taipei時區資料
PRINT '時區驗證 - 最近簽到記錄的UTC和Taipei時間對照:';
SELECT TOP 5
    UserID as 使用者ID,
    SignTime as UTC時間,
    DATEADD(HOUR, @TimezoneOffset, SignTime) as Taipei時間,
    DATEPART(WEEKDAY, DATEADD(HOUR, @TimezoneOffset, SignTime)) as 星期幾,
    PointsChanged as 點數,
    ExpGained as 經驗
FROM UserSignInStats
ORDER BY SignTime DESC;

PRINT '每日簽到系統種子資料插入完成！';
GO