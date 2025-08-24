-- =============================================
-- GameCore 虛擬寵物系統種子資料
-- 建立完整的寵物測試資料，包含5維屬性、等級經驗、換色記錄等
-- 嚴格按照規格要求生成多樣化的寵物狀態和互動歷史
-- =============================================

USE GameCore;
GO

PRINT '開始插入虛擬寵物系統種子資料...';

-- 清除現有的寵物記錄 (重新生成)
PRINT '清除現有寵物記錄...';
DELETE FROM Pet;

-- 重置自增ID
DBCC CHECKIDENT ('Pet', RESEED, 0);

PRINT '開始生成寵物記錄...';

-- 為前50名使用者建立寵物
DECLARE @UserId INT = 1;
DECLARE @MaxUserId INT = (SELECT MIN(50, MAX(User_ID)) FROM Users);

WHILE @UserId <= @MaxUserId
BEGIN
    PRINT '為使用者 ' + CAST(@UserId AS VARCHAR) + ' 建立寵物...';
    
    -- 隨機生成寵物特性
    DECLARE @RandomSeed INT = ABS(CHECKSUM(NEWID()));
    DECLARE @PetLevel INT = 1 + (@RandomSeed % 25); -- 等級 1-25
    DECLARE @PetExperience INT;
    
    -- 計算該等級應有的經驗值 (簡化計算)
    SET @PetExperience = 
        CASE 
            WHEN @PetLevel <= 10 THEN @PetLevel * 100 + (ABS(CHECKSUM(NEWID())) % 50)
            WHEN @PetLevel <= 20 THEN @PetLevel * 200 + (ABS(CHECKSUM(NEWID())) % 100)
            ELSE @PetLevel * 300 + (ABS(CHECKSUM(NEWID())) % 150)
        END;
    
    -- 隨機寵物名稱陣列
    DECLARE @PetNames TABLE (Name NVARCHAR(50));
    INSERT INTO @PetNames VALUES 
        ('小可愛'), ('史萊姆王'), ('藍寶'), ('小圓'), ('軟糖'),
        ('泡泡'), ('果凍'), ('彈彈'), ('QQ'), ('萌萌'),
        ('小藍'), ('水滴'), ('晶晶'), ('嘟嘟'), ('圓圓'),
        ('軟軟'), ('黏黏'), ('滑滑'), ('彈力球'), ('小精靈');
    
    DECLARE @SelectedName NVARCHAR(50);
    SELECT TOP 1 @SelectedName = Name FROM @PetNames ORDER BY NEWID();
    
    -- 生成5維屬性 (根據寵物"個性"決定)
    DECLARE @PetPersonality INT = ABS(CHECKSUM(NEWID())) % 5; -- 0-4 不同個性
    DECLARE @BaseHunger INT, @BaseMood INT, @BaseStamina INT, @BaseCleanliness INT, @BaseHealth INT;
    
    -- 根據個性設定基礎屬性範圍
    SELECT 
        @BaseHunger = CASE @PetPersonality
            WHEN 0 THEN 80 + (ABS(CHECKSUM(NEWID())) % 21) -- 愛吃型 80-100
            WHEN 1 THEN 60 + (ABS(CHECKSUM(NEWID())) % 31) -- 普通型 60-90
            WHEN 2 THEN 40 + (ABS(CHECKSUM(NEWID())) % 41) -- 厭食型 40-80
            WHEN 3 THEN 70 + (ABS(CHECKSUM(NEWID())) % 31) -- 健康型 70-100
            ELSE 50 + (ABS(CHECKSUM(NEWID())) % 51)         -- 隨機型 50-100
        END,
        @BaseMood = CASE @PetPersonality
            WHEN 0 THEN 70 + (ABS(CHECKSUM(NEWID())) % 31) -- 樂觀型 70-100
            WHEN 1 THEN 85 + (ABS(CHECKSUM(NEWID())) % 16) -- 開朗型 85-100
            WHEN 2 THEN 30 + (ABS(CHECKSUM(NEWID())) % 41) -- 憂鬱型 30-70
            WHEN 3 THEN 60 + (ABS(CHECKSUM(NEWID())) % 31) -- 平靜型 60-90
            ELSE 40 + (ABS(CHECKSUM(NEWID())) % 61)         -- 隨機型 40-100
        END,
        @BaseStamina = CASE @PetPersonality
            WHEN 0 THEN 50 + (ABS(CHECKSUM(NEWID())) % 31) -- 懶惰型 50-80
            WHEN 1 THEN 80 + (ABS(CHECKSUM(NEWID())) % 21) -- 活力型 80-100
            WHEN 2 THEN 90 + (ABS(CHECKSUM(NEWID())) % 11) -- 超活力 90-100
            WHEN 3 THEN 65 + (ABS(CHECKSUM(NEWID())) % 26) -- 穩定型 65-90
            ELSE 30 + (ABS(CHECKSUM(NEWID())) % 71)         -- 隨機型 30-100
        END,
        @BaseCleanliness = CASE @PetPersonality
            WHEN 0 THEN 20 + (ABS(CHECKSUM(NEWID())) % 41) -- 邋遢型 20-60
            WHEN 1 THEN 85 + (ABS(CHECKSUM(NEWID())) % 16) -- 愛乾淨 85-100
            WHEN 2 THEN 70 + (ABS(CHECKSUM(NEWID())) % 31) -- 普通型 70-100
            WHEN 3 THEN 90 + (ABS(CHECKSUM(NEWID())) % 11) -- 潔癖型 90-100
            ELSE 40 + (ABS(CHECKSUM(NEWID())) % 61)         -- 隨機型 40-100
        END;
    
    -- 健康度基於其他屬性計算 (按規格實現)
    SET @BaseHealth = (@BaseHunger + @BaseMood + @BaseStamina + @BaseCleanliness) / 4;
    
    -- 如果所有屬性都是100，健康度也是100
    IF @BaseHunger = 100 AND @BaseMood = 100 AND @BaseStamina = 100 AND @BaseCleanliness = 100
        SET @BaseHealth = 100;
    
    -- 應用健康度懲罰規則
    IF @BaseHunger < 30 SET @BaseHealth = @BaseHealth - 20;
    IF @BaseCleanliness < 30 SET @BaseHealth = @BaseHealth - 20;
    IF @BaseStamina < 30 SET @BaseHealth = @BaseHealth - 20;
    
    -- 健康度鉗位 0-100
    SET @BaseHealth = CASE WHEN @BaseHealth < 0 THEN 0 WHEN @BaseHealth > 100 THEN 100 ELSE @BaseHealth END;
    
    -- 隨機顏色選擇
    DECLARE @SkinColors TABLE (Color NVARCHAR(50), BgColor NVARCHAR(50), MinLevel INT);
    INSERT INTO @SkinColors VALUES 
        ('#ADD8E6', '粉藍', 1),      -- 預設淺藍
        ('#FFB6C1', '粉紅', 1),      -- 櫻花粉
        ('#98FB98', '薄荷', 5),      -- 薄荷綠
        ('#FFFFE0', '金黃', 10),     -- 陽光黃
        ('#DDA0DD', '紫羅蘭', 15),   -- 夢幻紫
        ('#FFD700', '金色', 20);     -- 黃金色
    
    DECLARE @SelectedSkinColor NVARCHAR(50), @SelectedBgColor NVARCHAR(50);
    SELECT TOP 1 @SelectedSkinColor = Color, @SelectedBgColor = BgColor 
    FROM @SkinColors 
    WHERE MinLevel <= @PetLevel 
    ORDER BY NEWID();
    
    -- 如果沒有符合等級的顏色，使用預設
    IF @SelectedSkinColor IS NULL
    BEGIN
        SET @SelectedSkinColor = '#ADD8E6';
        SET @SelectedBgColor = '粉藍';
    END
    
    -- 隨機換色次數和花費 (根據等級)
    DECLARE @ColorChanges INT = CASE WHEN @PetLevel > 10 THEN ABS(CHECKSUM(NEWID())) % 3 ELSE 0 END;
    DECLARE @PointsSpent INT = @ColorChanges * 2000;
    
    -- 計算寵物建立時間 (過去1-90天內)
    DECLARE @DaysAgo INT = 1 + (ABS(CHECKSUM(NEWID())) % 90);
    DECLARE @CreatedTime DATETIME2 = DATEADD(DAY, -@DaysAgo, GETUTCDATE());
    DECLARE @LastLevelUpTime DATETIME2 = DATEADD(HOUR, ABS(CHECKSUM(NEWID())) % 24, @CreatedTime);
    DECLARE @LastColorChangeTime DATETIME2 = DATEADD(DAY, ABS(CHECKSUM(NEWID())) % @DaysAgo, @CreatedTime);
    
    -- 插入寵物記錄
    INSERT INTO Pet (
        UserID, PetName, Level, LevelUpTime, Experience,
        Hunger, Mood, Stamina, Cleanliness, Health,
        SkinColor, ColorChangedTime, BackgroundColor, BackgroundColorChangedTime,
        PointsChanged, PointsChangedTime
    )
    VALUES (
        @UserId, @SelectedName, @PetLevel, @LastLevelUpTime, @PetExperience,
        @BaseHunger, @BaseMood, @BaseStamina, @BaseCleanliness, @BaseHealth,
        @SelectedSkinColor, @LastColorChangeTime, @SelectedBgColor, @LastColorChangeTime,
        @PointsSpent, @LastColorChangeTime
    );
    
    -- 為換色記錄生成通知
    IF @ColorChanges > 0
    BEGIN
        DECLARE @ChangeCount INT = 0;
        WHILE @ChangeCount < @ColorChanges
        BEGIN
            DECLARE @ChangeTime DATETIME2 = DATEADD(DAY, @ChangeCount * 10, @LastColorChangeTime);
            
            INSERT INTO Notifications (UserID, Title, Content, Type, Source, Action, IsRead, CreatedAt, ReadAt)
            VALUES (
                @UserId,
                '🎨 寵物換色成功',
                '您的寵物「' + @SelectedName + '」已成功換色！花費了2000點數。',
                'pet_color_change',
                'system',
                'pet_color_change',
                1, -- 標記為已讀
                @ChangeTime,
                DATEADD(HOUR, 1, @ChangeTime)
            );
            
            SET @ChangeCount = @ChangeCount + 1;
        END
    END
    
    SET @UserId = @UserId + 1;
END

PRINT '基本寵物記錄生成完成！';

-- 生成特殊狀態的寵物示例
PRINT '生成特殊狀態寵物示例...';

-- VIP使用者的完美寵物 (ID 1-5)
UPDATE Pet 
SET Hunger = 100, Mood = 100, Stamina = 100, Cleanliness = 100, Health = 100,
    Level = 25, Experience = 5000,
    SkinColor = '#FFD700', BackgroundColor = '金色',
    PetName = '黃金史萊姆'
WHERE UserID IN (1, 2, 3, 4, 5);

-- 生成一些需要照料的寵物 (模擬每日衰減後的狀態)
UPDATE Pet 
SET Hunger = 10, Mood = 15, Stamina = 20, Cleanliness = 5, Health = 25
WHERE UserID IN (10, 11, 12);

-- 生成一些中等狀態的寵物
UPDATE Pet 
SET Hunger = 60, Mood = 70, Stamina = 65, Cleanliness = 55, Health = 60
WHERE UserID IN (15, 16, 17, 18, 19, 20);

-- 生成一些高等級寵物
UPDATE Pet 
SET Level = 50 + (UserID % 10), Experience = 10000 + (UserID * 100)
WHERE UserID IN (25, 26, 27, 28, 29, 30);

-- 為所有寵物生成歡迎通知
PRINT '生成寵物建立通知...';

INSERT INTO Notifications (UserID, Title, Content, Type, Source, Action, IsRead, CreatedAt, ReadAt)
SELECT 
    p.UserID,
    '🎉 歡迎來到寵物世界！',
    '恭喜你獲得了可愛的史萊姆「' + p.PetName + '」！快來與它互動吧！',
    'pet_created',
    'system',
    'pet_created',
    1, -- 標記為已讀
    p.LevelUpTime,
    DATEADD(HOUR, 1, p.LevelUpTime)
FROM Pet p;

PRINT '寵物通知記錄生成完成！';

-- 統計報告
PRINT '=== 虛擬寵物系統種子資料統計報告 ===';

-- 總寵物數量
DECLARE @TotalPets INT = (SELECT COUNT(*) FROM Pet);
PRINT '總寵物數量: ' + CAST(@TotalPets AS VARCHAR);

-- 等級分布
PRINT '寵物等級分布:';
SELECT 
    CASE 
        WHEN Level BETWEEN 1 AND 10 THEN '1-10級'
        WHEN Level BETWEEN 11 AND 20 THEN '11-20級'
        WHEN Level BETWEEN 21 AND 30 THEN '21-30級'
        WHEN Level BETWEEN 31 AND 50 THEN '31-50級'
        ELSE '50級以上'
    END as 等級範圍,
    COUNT(*) as 寵物數量,
    CAST(COUNT(*) * 100.0 / @TotalPets AS DECIMAL(5,2)) as 佔比
FROM Pet
GROUP BY 
    CASE 
        WHEN Level BETWEEN 1 AND 10 THEN '1-10級'
        WHEN Level BETWEEN 11 AND 20 THEN '11-20級'
        WHEN Level BETWEEN 21 AND 30 THEN '21-30級'
        WHEN Level BETWEEN 31 AND 50 THEN '31-50級'
        ELSE '50級以上'
    END
ORDER BY 等級範圍;

-- 健康狀態分布
PRINT '寵物健康狀態分布:';
SELECT 
    CASE 
        WHEN Health >= 80 THEN '非常健康(80-100)'
        WHEN Health >= 60 THEN '健康(60-79)'
        WHEN Health >= 40 THEN '一般(40-59)'
        WHEN Health >= 20 THEN '不太好(20-39)'
        WHEN Health > 0 THEN '很糟糕(1-19)'
        ELSE '極度虛弱(0)'
    END as 健康狀態,
    COUNT(*) as 寵物數量,
    CAST(COUNT(*) * 100.0 / @TotalPets AS DECIMAL(5,2)) as 佔比
FROM Pet
GROUP BY 
    CASE 
        WHEN Health >= 80 THEN '非常健康(80-100)'
        WHEN Health >= 60 THEN '健康(60-79)'
        WHEN Health >= 40 THEN '一般(40-59)'
        WHEN Health >= 20 THEN '不太好(20-39)'
        WHEN Health > 0 THEN '很糟糕(1-19)'
        ELSE '極度虛弱(0)'
    END
ORDER BY 健康狀態;

-- 顏色分布
PRINT '寵物顏色分布:';
SELECT 
    CASE SkinColor
        WHEN '#ADD8E6' THEN '預設淺藍'
        WHEN '#FFB6C1' THEN '櫻花粉'
        WHEN '#98FB98' THEN '薄荷綠'
        WHEN '#FFFFE0' THEN '陽光黃'
        WHEN '#DDA0DD' THEN '夢幻紫'
        WHEN '#FFD700' THEN '黃金色'
        ELSE '其他'
    END as 顏色,
    COUNT(*) as 寵物數量
FROM Pet
GROUP BY SkinColor
ORDER BY COUNT(*) DESC;

-- 5維屬性統計
PRINT '寵物5維屬性平均值:';
SELECT 
    CAST(AVG(CAST(Hunger AS FLOAT)) AS DECIMAL(5,2)) as 平均飢餓值,
    CAST(AVG(CAST(Mood AS FLOAT)) AS DECIMAL(5,2)) as 平均心情值,
    CAST(AVG(CAST(Stamina AS FLOAT)) AS DECIMAL(5,2)) as 平均體力值,
    CAST(AVG(CAST(Cleanliness AS FLOAT)) AS DECIMAL(5,2)) as 平均清潔值,
    CAST(AVG(CAST(Health AS FLOAT)) AS DECIMAL(5,2)) as 平均健康度
FROM Pet;

-- 冒險準備度統計
PRINT '寵物冒險準備度統計:';
SELECT 
    CASE 
        WHEN Health = 0 OR Hunger = 0 OR Mood = 0 OR Stamina = 0 OR Cleanliness = 0 THEN '無法冒險'
        ELSE '可以冒險'
    END as 冒險狀態,
    COUNT(*) as 寵物數量,
    CAST(COUNT(*) * 100.0 / @TotalPets AS DECIMAL(5,2)) as 佔比
FROM Pet
GROUP BY 
    CASE 
        WHEN Health = 0 OR Hunger = 0 OR Mood = 0 OR Stamina = 0 OR Cleanliness = 0 THEN '無法冒險'
        ELSE '可以冒險'
    END;

-- 等級排行榜 TOP 10
PRINT '寵物等級排行榜 TOP 10:';
SELECT TOP 10
    u.User_name as 主人名稱,
    p.PetName as 寵物名稱,
    p.Level as 等級,
    p.Experience as 總經驗,
    p.Health as 健康度,
    (p.Hunger + p.Mood + p.Stamina + p.Cleanliness + p.Health) as 總屬性,
    CASE p.SkinColor
        WHEN '#ADD8E6' THEN '預設淺藍'
        WHEN '#FFB6C1' THEN '櫻花粉'
        WHEN '#98FB98' THEN '薄荷綠'
        WHEN '#FFFFE0' THEN '陽光黃'
        WHEN '#DDA0DD' THEN '夢幻紫'
        WHEN '#FFD700' THEN '黃金色'
        ELSE '其他'
    END as 顏色
FROM Pet p
JOIN Users u ON p.UserID = u.User_ID
ORDER BY p.Level DESC, p.Experience DESC;

-- 完美狀態寵物 (所有屬性100)
PRINT '完美狀態寵物數量:';
SELECT COUNT(*) as 完美寵物數量
FROM Pet
WHERE Hunger = 100 AND Mood = 100 AND Stamina = 100 AND Cleanliness = 100 AND Health = 100;

-- 換色統計
PRINT '換色統計:';
SELECT 
    COUNT(CASE WHEN PointsChanged > 0 THEN 1 END) as 有換色記錄的寵物,
    SUM(PointsChanged) as 總換色花費點數,
    AVG(CASE WHEN PointsChanged > 0 THEN PointsChanged ELSE NULL END) as 平均換色花費
FROM Pet;

-- 寵物通知統計
PRINT '寵物相關通知統計:';
SELECT 
    Action as 通知類型,
    COUNT(*) as 通知數量
FROM Notifications
WHERE Action IN ('pet_created', 'pet_color_change', 'pet_renamed')
GROUP BY Action
ORDER BY COUNT(*) DESC;

-- 驗證一人一寵規則
PRINT '一人一寵規則驗證:';
SELECT 
    CASE 
        WHEN MAX(pet_count) = 1 THEN '✓ 符合一人一寵規則'
        ELSE '✗ 違反一人一寵規則'
    END as 規則檢查結果
FROM (
    SELECT UserID, COUNT(*) as pet_count
    FROM Pet
    GROUP BY UserID
) pet_counts;

PRINT '虛擬寵物系統種子資料插入完成！';
GO