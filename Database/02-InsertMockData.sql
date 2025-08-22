-- =====================================================
-- GameCore 大量假資料插入腳本
-- 插入 1000+ 筆真實感測試資料，用於展示和測試
-- =====================================================

USE GameCore;
GO

PRINT '開始插入大量假資料...';

-- =====================================================
-- 1. 插入基礎資料 (通知來源、行為、遊戲等)
-- =====================================================

-- 通知來源
IF NOT EXISTS (SELECT * FROM Notification_Sources)
BEGIN
    INSERT INTO Notification_Sources (source_name) VALUES 
    ('system'), ('forum'), ('store'), ('market'), ('pet'), ('admin'), ('game'), ('social');
    PRINT '✓ 通知來源資料插入完成';
END

-- 通知行為
IF NOT EXISTS (SELECT * FROM Notification_Actions)
BEGIN
    INSERT INTO Notification_Actions (action_name) VALUES 
    ('signin'), ('pet_interact'), ('pet_color_change'), ('points_adjustment'), 
    ('order_created'), ('order_completed'), ('message_received'), ('admin_notice'),
    ('thread_created'), ('thread_replied'), ('like_received'), ('bookmark_added');
    PRINT '✓ 通知行為資料插入完成';
END

-- 遊戲資料 (熱門遊戲)
IF NOT EXISTS (SELECT * FROM games)
BEGIN
    INSERT INTO games (name, genre, created_at) VALUES 
    ('英雄聯盟', 'MOBA', GETUTCDATE()),
    ('原神', 'RPG', GETUTCDATE()),
    ('Valorant', 'FPS', GETUTCDATE()),
    ('Apex Legends', 'FPS', GETUTCDATE()),
    ('Minecraft', 'Sandbox', GETUTCDATE()),
    ('Among Us', 'Social', GETUTCDATE()),
    ('Fortnite', 'Battle Royale', GETUTCDATE()),
    ('Call of Duty: Warzone', 'FPS', GETUTCDATE()),
    ('Overwatch 2', 'FPS', GETUTCDATE()),
    ('Dota 2', 'MOBA', GETUTCDATE()),
    ('Counter-Strike 2', 'FPS', GETUTCDATE()),
    ('Elden Ring', 'RPG', GETUTCDATE()),
    ('Cyberpunk 2077', 'RPG', GETUTCDATE()),
    ('Grand Theft Auto V', 'Action', GETUTCDATE()),
    ('Red Dead Redemption 2', 'Action', GETUTCDATE()),
    ('The Witcher 3', 'RPG', GETUTCDATE()),
    ('Baldur''s Gate 3', 'RPG', GETUTCDATE()),
    ('Starfield', 'RPG', GETUTCDATE()),
    ('Hogwarts Legacy', 'RPG', GETUTCDATE()),
    ('Diablo IV', 'RPG', GETUTCDATE()),
    ('Street Fighter 6', 'Fighting', GETUTCDATE()),
    ('Tekken 8', 'Fighting', GETUTCDATE()),
    ('FIFA 24', 'Sports', GETUTCDATE()),
    ('NBA 2K24', 'Sports', GETUTCDATE()),
    ('Fall Guys', 'Party', GETUTCDATE()),
    ('Rocket League', 'Sports', GETUTCDATE()),
    ('Stardew Valley', 'Simulation', GETUTCDATE()),
    ('Animal Crossing', 'Simulation', GETUTCDATE()),
    ('The Sims 4', 'Simulation', GETUTCDATE()),
    ('Cities: Skylines', 'Simulation', GETUTCDATE()),
    ('Civilization VI', 'Strategy', GETUTCDATE()),
    ('Age of Empires IV', 'Strategy', GETUTCDATE()),
    ('Total War: Warhammer III', 'Strategy', GETUTCDATE()),
    ('Crusader Kings III', 'Strategy', GETUTCDATE()),
    ('Europa Universalis IV', 'Strategy', GETUTCDATE()),
    ('Hearts of Iron IV', 'Strategy', GETUTCDATE()),
    ('Stellaris', 'Strategy', GETUTCDATE()),
    ('XCOM 2', 'Strategy', GETUTCDATE()),
    ('Disco Elysium', 'RPG', GETUTCDATE()),
    ('Hades', 'Roguelike', GETUTCDATE()),
    ('Dead Cells', 'Roguelike', GETUTCDATE()),
    ('Hollow Knight', 'Metroidvania', GETUTCDATE()),
    ('Ori and the Will of the Wisps', 'Platformer', GETUTCDATE()),
    ('Celeste', 'Platformer', GETUTCDATE()),
    ('A Hat in Time', 'Platformer', GETUTCDATE()),
    ('Cuphead', 'Platformer', GETUTCDATE()),
    ('Undertale', 'RPG', GETUTCDATE()),
    ('Deltarune', 'RPG', GETUTCDATE()),
    ('Omori', 'RPG', GETUTCDATE()),
    ('Pizza Tower', 'Platformer', GETUTCDATE());
    PRINT '✓ 遊戲資料插入完成 (50 款遊戲)';
END

-- 為每個遊戲建立論壇版面
IF NOT EXISTS (SELECT * FROM forums)
BEGIN
    INSERT INTO forums (game_id, name, description, created_at)
    SELECT 
        game_id, 
        name + ' 討論版', 
        '歡迎來到 ' + name + ' 的專屬討論區！在這裡分享攻略、心得、組隊資訊。',
        GETUTCDATE()
    FROM games;
    PRINT '✓ 論壇版面建立完成';
END

-- 供應商資料
IF NOT EXISTS (SELECT * FROM Supplier)
BEGIN
    INSERT INTO Supplier (supplier_name) VALUES 
    ('Steam 官方'),
    ('Epic Games Store'),
    ('Ubisoft Connect'),
    ('Origin (EA)'),
    ('Battle.net (Blizzard)'),
    ('PlayStation Store'),
    ('Xbox Game Store'),
    ('Nintendo eShop'),
    ('GOG.com'),
    ('Humble Bundle'),
    ('Green Man Gaming'),
    ('Fanatical'),
    ('GamersGate'),
    ('Direct2Drive'),
    ('Gamesplanet'),
    ('雷蛇 Razer'),
    ('羅技 Logitech'),
    ('海盜船 Corsair'),
    ('華碩 ASUS'),
    ('微星 MSI'),
    ('技嘉 GIGABYTE'),
    ('宏碁 Acer'),
    ('聯想 Lenovo'),
    ('戴爾 Dell'),
    ('惠普 HP');
    PRINT '✓ 供應商資料插入完成 (25 家供應商)';
END

-- 管理員角色權限
IF NOT EXISTS (SELECT * FROM ManagerRolePermission)
BEGIN
    INSERT INTO ManagerRolePermission (role_name, AdministratorPrivilegesManagement, UserStatusManagement, ShoppingPermissionManagement, MessagePermissionManagement, Pet_Rights_Management, customer_service) VALUES 
    ('超級管理員', 1, 1, 1, 1, 1, 1),
    ('使用者管理員', 0, 1, 0, 1, 0, 1),
    ('商城管理員', 0, 0, 1, 0, 0, 0),
    ('論壇管理員', 0, 0, 0, 1, 0, 1),
    ('寵物管理員', 0, 0, 0, 0, 1, 0),
    ('客服專員', 0, 0, 0, 0, 0, 1),
    ('內容審核員', 0, 0, 0, 1, 0, 0),
    ('數據分析師', 0, 0, 0, 0, 0, 0);
    PRINT '✓ 管理員角色權限插入完成';
END

-- =====================================================
-- 2. 插入大量使用者資料 (1000+ 筆)
-- =====================================================

DECLARE @UserCounter INT = 1;
DECLARE @BatchSize INT = 100;
DECLARE @TotalUsers INT = 1200;

PRINT '開始插入 ' + CAST(@TotalUsers AS VARCHAR) + ' 筆使用者資料...';

WHILE @UserCounter <= @TotalUsers
BEGIN
    DECLARE @BatchEnd INT = @UserCounter + @BatchSize - 1;
    IF @BatchEnd > @TotalUsers SET @BatchEnd = @TotalUsers;

    -- 批次插入使用者基本資料
    WITH UserData AS (
        SELECT 
            @UserCounter + ROW_NUMBER() OVER (ORDER BY (SELECT NULL)) - 1 AS UserNum,
            CASE ((@UserCounter + ROW_NUMBER() OVER (ORDER BY (SELECT NULL)) - 1) % 20)
                WHEN 0 THEN '王小明'
                WHEN 1 THEN '李美麗'
                WHEN 2 THEN '張志強'
                WHEN 3 THEN '陳雅婷'
                WHEN 4 THEN '林建宏'
                WHEN 5 THEN '黃淑芬'
                WHEN 6 THEN '吳俊傑'
                WHEN 7 THEN '蔡心怡'
                WHEN 8 THEN '劉宗翰'
                WHEN 9 THEN '鄭佳玲'
                WHEN 10 THEN '許志明'
                WHEN 11 THEN '楊雅雯'
                WHEN 12 THEN '洪建成'
                WHEN 13 THEN '謝美玲'
                WHEN 14 THEN '周俊宏'
                WHEN 15 THEN '郭淑娟'
                WHEN 16 THEN '馬志豪'
                WHEN 17 THEN '趙雅琪'
                WHEN 18 THEN '錢建國'
                ELSE '孫美華'
            END AS UserName,
            CASE ((@UserCounter + ROW_NUMBER() OVER (ORDER BY (SELECT NULL)) - 1) % 15)
                WHEN 0 THEN 'gamer'
                WHEN 1 THEN 'player'
                WHEN 2 THEN 'master'
                WHEN 3 THEN 'legend'
                WHEN 4 THEN 'hero'
                WHEN 5 THEN 'champion'
                WHEN 6 THEN 'warrior'
                WHEN 7 THEN 'knight'
                WHEN 8 THEN 'wizard'
                WHEN 9 THEN 'archer'
                WHEN 10 THEN 'ninja'
                WHEN 11 THEN 'samurai'
                WHEN 12 THEN 'dragon'
                WHEN 13 THEN 'phoenix'
                ELSE 'tiger'
            END AS AccountPrefix
        FROM sys.objects
        WHERE @UserCounter + ROW_NUMBER() OVER (ORDER BY (SELECT NULL)) - 1 <= @BatchEnd
    )
    INSERT INTO Users (User_name, User_Account, User_Password)
    SELECT 
        UserName + CAST(UserNum AS VARCHAR),
        AccountPrefix + CAST(UserNum AS VARCHAR),
        '$2a$11$' + REPLICATE('a', 53) -- BCrypt 雜湊格式 (假的雜湊值)
    FROM UserData
    WHERE UserNum BETWEEN @UserCounter AND @BatchEnd;

    PRINT '✓ 使用者批次 ' + CAST(@UserCounter AS VARCHAR) + '-' + CAST(@BatchEnd AS VARCHAR) + ' 插入完成';
    SET @UserCounter = @BatchEnd + 1;
END

-- 為所有使用者建立詳細資料
INSERT INTO User_Introduce (User_ID, User_NickName, Gender, IdNumber, Cellphone, Email, Address, DateOfBirth, Create_Account)
SELECT 
    User_ID,
    CASE (User_ID % 20)
        WHEN 0 THEN '遊戲大師'
        WHEN 1 THEN '攻略達人'
        WHEN 2 THEN '電競選手'
        WHEN 3 THEN '休閒玩家'
        WHEN 4 THEN '策略專家'
        WHEN 5 THEN '射擊高手'
        WHEN 6 THEN 'RPG 愛好者'
        WHEN 7 THEN '模擬專家'
        WHEN 8 THEN '競速狂人'
        WHEN 9 THEN '解謎達人'
        WHEN 10 THEN '格鬥王者'
        WHEN 11 THEN '音遊高手'
        WHEN 12 THEN '卡牌專家'
        WHEN 13 THEN '塔防大師'
        WHEN 14 THEN 'MOBA 王者'
        WHEN 15 THEN 'FPS 神槍手'
        WHEN 16 THEN '沙盒建築師'
        WHEN 17 THEN '生存專家'
        WHEN 18 THEN '冒險探索者'
        ELSE '全能玩家'
    END + CAST(User_ID AS VARCHAR),
    CASE (User_ID % 2) WHEN 0 THEN 'M' ELSE 'F' END,
    CASE (User_ID % 2) 
        WHEN 0 THEN 'A1' + RIGHT('00000000' + CAST(123456780 + User_ID AS VARCHAR), 8)
        ELSE 'B2' + RIGHT('00000000' + CAST(223456780 + User_ID AS VARCHAR), 8)
    END,
    '09' + RIGHT('00000000' + CAST(10000000 + User_ID AS VARCHAR), 8),
    User_Account + '@gamecore.com',
    CASE (User_ID % 10)
        WHEN 0 THEN '台北市信義區信義路五段7號'
        WHEN 1 THEN '新北市板橋區中山路一段161號'
        WHEN 2 THEN '桃園市桃園區復興路21號'
        WHEN 3 THEN '台中市西屯區台灣大道三段99號'
        WHEN 4 THEN '台南市東區大學路1號'
        WHEN 5 THEN '高雄市鳳山區建國路三段123號'
        WHEN 6 THEN '新竹市東區光復路二段101號'
        WHEN 7 THEN '彰化縣彰化市中山路二段416號'
        WHEN 8 THEN '雲林縣斗六市大學路三段123號'
        ELSE '嘉義市西區中山路412號'
    END,
    DATEADD(YEAR, -25 - (User_ID % 20), GETUTCDATE()),
    DATEADD(DAY, -(User_ID % 365), GETUTCDATE())
FROM Users
WHERE User_ID NOT IN (SELECT User_ID FROM User_Introduce);

PRINT '✓ 使用者詳細資料插入完成 (' + CAST(@@ROWCOUNT AS VARCHAR) + ' 筆)';

-- 為所有使用者建立權限設定
INSERT INTO User_Rights (User_Id, User_Status, ShoppingPermission, MessagePermission, SalesAuthority)
SELECT 
    User_ID,
    CASE WHEN User_ID % 50 = 0 THEN 0 ELSE 1 END, -- 2% 的使用者被停權
    1, -- 預設允許購物
    CASE WHEN User_ID % 20 = 0 THEN 0 ELSE 1 END, -- 5% 的使用者被禁言
    CASE WHEN User_ID % 10 = 0 THEN 1 ELSE 0 END  -- 10% 的使用者有銷售權限
FROM Users
WHERE User_ID NOT IN (SELECT User_Id FROM User_Rights);

PRINT '✓ 使用者權限設定插入完成 (' + CAST(@@ROWCOUNT AS VARCHAR) + ' 筆)';

-- 為所有使用者建立錢包
INSERT INTO User_wallet (User_Id, User_Point, Coupon_Number)
SELECT 
    User_ID,
    (User_ID * 123 + 456) % 10000 + 1000, -- 隨機點數 1000-11000
    CASE WHEN User_ID % 5 = 0 THEN 'COUPON' + CAST(User_ID AS VARCHAR) ELSE NULL END
FROM Users
WHERE User_ID NOT IN (SELECT User_Id FROM User_wallet);

PRINT '✓ 使用者錢包插入完成 (' + CAST(@@ROWCOUNT AS VARCHAR) + ' 筆)';

-- =====================================================
-- 3. 插入寵物資料 (每個使用者一隻寵物)
-- =====================================================

INSERT INTO Pet (UserID, PetName, Level, Experience, Hunger, Mood, Stamina, Cleanliness, Health, SkinColor, BackgroundColor)
SELECT 
    User_ID,
    CASE (User_ID % 25)
        WHEN 0 THEN '小可愛'
        WHEN 1 THEN '史萊姆王'
        WHEN 2 THEN '藍色小精靈'
        WHEN 3 THEN '綠色守護者'
        WHEN 4 THEN '紅色戰士'
        WHEN 5 THEN '黃色閃電'
        WHEN 6 THEN '紫色魔法師'
        WHEN 7 THEN '橙色火焰'
        WHEN 8 THEN '粉色公主'
        WHEN 9 THEN '黑色忍者'
        WHEN 10 THEN '白色天使'
        WHEN 11 THEN '彩虹史萊姆'
        WHEN 12 THEN '星空守護者'
        WHEN 13 THEN '海洋之子'
        WHEN 14 THEN '森林精靈'
        WHEN 15 THEN '沙漠之鷹'
        WHEN 16 THEN '雪山勇者'
        WHEN 17 THEN '火山霸主'
        WHEN 18 THEN '雷電法王'
        WHEN 19 THEN '冰霜女王'
        WHEN 20 THEN '光明使者'
        WHEN 21 THEN '暗影刺客'
        WHEN 22 THEN '時空旅者'
        WHEN 23 THEN '次元守護'
        ELSE '夢幻史萊姆'
    END,
    1 + (User_ID % 50), -- 等級 1-50
    (User_ID * 47 + 123) % 5000, -- 隨機經驗值
    80 + (User_ID % 21), -- 飢餓值 80-100
    75 + (User_ID % 26), -- 心情值 75-100
    70 + (User_ID % 31), -- 體力值 70-100
    85 + (User_ID % 16), -- 清潔值 85-100
    90 + (User_ID % 11), -- 健康度 90-100
    CASE (User_ID % 10)
        WHEN 0 THEN '#ADD8E6' -- 淺藍色
        WHEN 1 THEN '#FFB6C1' -- 淺粉色
        WHEN 2 THEN '#98FB98' -- 淺綠色
        WHEN 3 THEN '#F0E68C' -- 卡其色
        WHEN 4 THEN '#DDA0DD' -- 梅紅色
        WHEN 5 THEN '#87CEEB' -- 天空藍
        WHEN 6 THEN '#F5DEB3' -- 小麥色
        WHEN 7 THEN '#FFE4E1' -- 霧玫瑰
        WHEN 8 THEN '#E0FFFF' -- 淺青色
        ELSE '#FFEFD5'       -- 木瓜色
    END,
    CASE (User_ID % 8)
        WHEN 0 THEN '粉藍'
        WHEN 1 THEN '淺綠'
        WHEN 2 THEN '溫黃'
        WHEN 3 THEN '柔紫'
        WHEN 4 THEN '薄荷'
        WHEN 5 THEN '櫻花'
        WHEN 6 THEN '天空'
        ELSE '夕陽'
    END
FROM Users
WHERE User_ID NOT IN (SELECT UserID FROM Pet);

PRINT '✓ 寵物資料插入完成 (' + CAST(@@ROWCOUNT AS VARCHAR) + ' 隻寵物)';

-- =====================================================
-- 4. 插入簽到記錄 (模擬過去 3 個月的簽到)
-- =====================================================

DECLARE @SignInStartDate DATE = DATEADD(MONTH, -3, GETDATE());
DECLARE @SignInEndDate DATE = GETDATE();
DECLARE @CurrentDate DATE = @SignInStartDate;

PRINT '開始插入簽到記錄 (過去 3 個月)...';

WHILE @CurrentDate <= @SignInEndDate
BEGIN
    -- 每天隨機 60-80% 的使用者會簽到
    DECLARE @SignInRate FLOAT = 0.6 + (CAST(DATEPART(DAYOFYEAR, @CurrentDate) AS FLOAT) % 100) / 500;
    
    INSERT INTO UserSignInStats (UserID, SignTime, PointsChanged, ExpGained, PointsChangedTime, ExpGainedTime)
    SELECT 
        User_ID,
        DATEADD(HOUR, (User_ID % 18) + 6, @CurrentDate), -- 隨機簽到時間 6-24 點
        CASE 
            WHEN DATEPART(WEEKDAY, @CurrentDate) IN (1, 7) THEN 30 -- 假日 30 點
            ELSE 20 -- 平日 20 點
        END +
        CASE WHEN User_ID % 7 = DATEPART(WEEKDAY, @CurrentDate) THEN 40 ELSE 0 END, -- 連續 7 天獎勵
        CASE 
            WHEN DATEPART(WEEKDAY, @CurrentDate) IN (1, 7) THEN 200 -- 假日 200 經驗
            ELSE 0 -- 平日 0 經驗
        END +
        CASE WHEN User_ID % 7 = DATEPART(WEEKDAY, @CurrentDate) THEN 300 ELSE 0 END, -- 連續 7 天獎勵
        DATEADD(HOUR, (User_ID % 18) + 6, @CurrentDate),
        DATEADD(HOUR, (User_ID % 18) + 6, @CurrentDate)
    FROM Users
    WHERE (User_ID * DATEPART(DAYOFYEAR, @CurrentDate)) % 100 < (@SignInRate * 100);

    SET @CurrentDate = DATEADD(DAY, 1, @CurrentDate);
END

PRINT '✓ 簽到記錄插入完成 (' + CAST((SELECT COUNT(*) FROM UserSignInStats) AS VARCHAR) + ' 筆記錄)';

-- =====================================================
-- 5. 插入小遊戲記錄 (模擬遊戲歷史)
-- =====================================================

PRINT '開始插入小遊戲記錄...';

DECLARE @GameCounter INT = 1;
DECLARE @TotalGames INT = 5000;

WHILE @GameCounter <= @TotalGames
BEGIN
    DECLARE @RandomUserId INT = 1 + (@GameCounter % (SELECT COUNT(*) FROM Users));
    DECLARE @RandomPetId INT = (SELECT PetID FROM Pet WHERE UserID = @RandomUserId);
    DECLARE @GameLevel INT = 1 + (@GameCounter % 10);
    DECLARE @IsWin BIT = CASE WHEN (@GameCounter % 3) = 0 THEN 1 ELSE 0 END;
    DECLARE @GameStartTime DATETIME2 = DATEADD(DAY, -(@GameCounter % 90), GETUTCDATE());

    IF @RandomPetId IS NOT NULL
    BEGIN
        INSERT INTO MiniGame (
            UserID, PetID, Level, MonsterCount, SpeedMultiplier, Result,
            ExpGained, PointsChanged, HungerDelta, MoodDelta, StaminaDelta, CleanlinessDelta,
            StartTime, EndTime, Aborted
        ) VALUES (
            @RandomUserId,
            @RandomPetId,
            @GameLevel,
            6 + (@GameLevel - 1) * 2, -- 怪物數量隨關卡增加
            1.0 + (@GameLevel - 1) * 0.1, -- 速度隨關卡增加
            CASE @IsWin WHEN 1 THEN 'Win' ELSE 'Lose' END,
            CASE @IsWin WHEN 1 THEN 100 * @GameLevel ELSE 50 * @GameLevel END, -- 勝利經驗更多
            CASE @IsWin WHEN 1 THEN 10 * @GameLevel ELSE 5 * @GameLevel END,   -- 勝利點數更多
            -20, -- 飢餓值變化
            CASE @IsWin WHEN 1 THEN 30 ELSE -30 END, -- 心情值變化 (勝負影響)
            -20, -- 體力值變化
            -20, -- 清潔值變化
            @GameStartTime,
            DATEADD(MINUTE, 5 + (@GameLevel * 2), @GameStartTime), -- 遊戲時長
            0 -- 沒有中途退出
        );
    END

    SET @GameCounter = @GameCounter + 1;
END

PRINT '✓ 小遊戲記錄插入完成 (' + CAST(@@ROWCOUNT AS VARCHAR) + ' 筆記錄)';

-- =====================================================
-- 6. 插入商品資料
-- =====================================================

-- 遊戲商品
INSERT INTO ProductInfo (product_name, product_type, price, currency_code, Shipment_Quantity, product_created_by, product_created_at, user_id)
SELECT 
    g.name + ' - ' + 
    CASE (g.game_id % 5)
        WHEN 0 THEN '標準版'
        WHEN 1 THEN '豪華版'
        WHEN 2 THEN '典藏版'
        WHEN 3 THEN '數位版'
        ELSE '完整版'
    END,
    'Game',
    CASE (g.game_id % 5)
        WHEN 0 THEN 990.00  -- 標準版
        WHEN 1 THEN 1590.00 -- 豪華版
        WHEN 2 THEN 2990.00 -- 典藏版
        WHEN 3 THEN 790.00  -- 數位版
        ELSE 1290.00        -- 完整版
    END,
    'TWD',
    100 + (g.game_id % 500),
    'System',
    GETUTCDATE(),
    NULL
FROM games g;

-- 遊戲商品詳細資料
INSERT INTO GameProductDetails (product_id, product_name, product_description, supplier_id, platform_id, game_id, game_name, download_link)
SELECT 
    p.product_id,
    p.product_name,
    '這是 ' + g.name + ' 的精彩' + 
    CASE (p.product_id % 3)
        WHEN 0 THEN '冒險遊戲，帶你進入奇幻世界！'
        WHEN 1 THEN '競技遊戲，挑戰你的技巧極限！'
        ELSE '策略遊戲，考驗你的智慧與決策！'
    END + 
    ' 包含豐富的遊戲內容、精美的畫面效果、動人的音樂配樂，以及引人入勝的劇情故事。適合各年齡層玩家，提供單人和多人遊戲模式。',
    1 + (p.product_id % (SELECT COUNT(*) FROM Supplier)),
    1 + (p.product_id % 5), -- 平台 ID
    g.game_id,
    g.name,
    'https://store.gamecore.com/download/' + CAST(p.product_id AS VARCHAR)
FROM ProductInfo p
INNER JOIN games g ON p.product_name LIKE g.name + '%'
WHERE p.product_type = 'Game';

-- 硬體周邊商品
INSERT INTO ProductInfo (product_name, product_type, price, currency_code, Shipment_Quantity, product_created_by, product_created_at)
VALUES 
('雷蛇 DeathAdder V3 電競滑鼠', 'Hardware', 2290.00, 'TWD', 50, 'System', GETUTCDATE()),
('羅技 G Pro X 機械鍵盤', 'Hardware', 4990.00, 'TWD', 30, 'System', GETUTCDATE()),
('海盜船 HS80 RGB 電競耳機', 'Hardware', 3590.00, 'TWD', 40, 'System', GETUTCDATE()),
('華碩 ROG Swift PG279QM 電競螢幕', 'Hardware', 25900.00, 'TWD', 10, 'System', GETUTCDATE()),
('微星 GeForce RTX 4080 顯示卡', 'Hardware', 39900.00, 'TWD', 5, 'System', GETUTCDATE()),
('技嘉 B650 AORUS ELITE AX 主機板', 'Hardware', 8990.00, 'TWD', 20, 'System', GETUTCDATE()),
('海盜船 Vengeance LPX 32GB DDR4', 'Hardware', 4590.00, 'TWD', 25, 'System', GETUTCDATE()),
('Samsung 980 PRO 1TB NVMe SSD', 'Hardware', 3290.00, 'TWD', 35, 'System', GETUTCDATE()),
('Cooler Master MasterLiquid ML240L', 'Hardware', 2790.00, 'TWD', 15, 'System', GETUTCDATE()),
('Fractal Design Define 7 機殼', 'Hardware', 5490.00, 'TWD', 12, 'System', GETUTCDATE());

-- 硬體商品詳細資料
INSERT INTO OtherProductDetails (product_id, product_name, product_description, supplier_id, size, color, weight, material)
SELECT 
    p.product_id,
    p.product_name,
    p.product_name + ' 是專為遊戲玩家設計的高品質硬體設備。' +
    CASE (p.product_id % 3)
        WHEN 0 THEN '採用最新技術，提供卓越的遊戲體驗和持久的耐用性。'
        WHEN 1 THEN '人體工學設計，長時間使用也不會感到疲勞。'
        ELSE '高性能規格，滿足專業電競選手的嚴格要求。'
    END,
    16 + (p.product_id % 10), -- 供應商 ID
    CASE (p.product_id % 4)
        WHEN 0 THEN '標準尺寸'
        WHEN 1 THEN '大尺寸'
        WHEN 2 THEN '小尺寸'
        ELSE '自訂尺寸'
    END,
    CASE (p.product_id % 6)
        WHEN 0 THEN '黑色'
        WHEN 1 THEN '白色'
        WHEN 2 THEN 'RGB 多色'
        WHEN 3 THEN '銀色'
        WHEN 4 THEN '紅色'
        ELSE '藍色'
    END,
    CAST((p.product_id % 2000 + 100) AS VARCHAR) + 'g',
    CASE (p.product_id % 4)
        WHEN 0 THEN '鋁合金'
        WHEN 1 THEN '塑膠'
        WHEN 2 THEN '碳纖維'
        ELSE '不鏽鋼'
    END
FROM ProductInfo p
WHERE p.product_type = 'Hardware';

PRINT '✓ 商品資料插入完成 (' + CAST((SELECT COUNT(*) FROM ProductInfo) AS VARCHAR) + ' 項商品)';

-- =====================================================
-- 7. 插入論壇討論資料
-- =====================================================

-- 討論主題 (每個版面 20-50 個主題)
DECLARE @ThreadCounter INT = 1;
DECLARE @TotalThreads INT = 2000;

PRINT '開始插入論壇討論主題...';

WHILE @ThreadCounter <= @TotalThreads
BEGIN
    DECLARE @RandomForumId INT = 1 + (@ThreadCounter % (SELECT COUNT(*) FROM forums));
    DECLARE @RandomAuthorId INT = 1 + (@ThreadCounter % (SELECT COUNT(*) FROM Users));
    DECLARE @ThreadTitle NVARCHAR(200) = 
        CASE (@ThreadCounter % 20)
            WHEN 0 THEN '【攻略】新手入門完整指南'
            WHEN 1 THEN '【心得】遊戲體驗分享'
            WHEN 2 THEN '【討論】版本更新內容分析'
            WHEN 3 THEN '【求助】遇到技術問題'
            WHEN 4 THEN '【情報】最新活動資訊'
            WHEN 5 THEN '【閒聊】日常遊戲趣事'
            WHEN 6 THEN '【建議】遊戲改進意見'
            WHEN 7 THEN '【分享】精彩遊戲截圖'
            WHEN 8 THEN '【組隊】尋找遊戲夥伴'
            WHEN 9 THEN '【競賽】比賽結果討論'
            WHEN 10 THEN '【教學】進階技巧分享'
            WHEN 11 THEN '【評測】遊戲深度評價'
            WHEN 12 THEN '【預告】未來更新預測'
            WHEN 13 THEN '【回憶】經典時刻回顧'
            WHEN 14 THEN '【創作】同人作品分享'
            WHEN 15 THEN '【理論】遊戲機制分析'
            WHEN 16 THEN '【統計】數據趨勢觀察'
            WHEN 17 THEN '【比較】與其他遊戲對比'
            WHEN 18 THEN '【預測】未來發展方向'
            ELSE '【雜談】隨意聊聊'
        END + ' #' + CAST(@ThreadCounter AS VARCHAR);

    INSERT INTO threads (forum_id, author_user_id, title, status, created_at, updated_at)
    VALUES (
        @RandomForumId,
        @RandomAuthorId,
        @ThreadTitle,
        CASE WHEN @ThreadCounter % 100 = 0 THEN 'hidden' ELSE 'normal' END,
        DATEADD(DAY, -(@ThreadCounter % 90), GETUTCDATE()),
        DATEADD(DAY, -(@ThreadCounter % 30), GETUTCDATE())
    );

    SET @ThreadCounter = @ThreadCounter + 1;
END

PRINT '✓ 討論主題插入完成 (' + CAST(@TotalThreads AS VARCHAR) + ' 個主題)';

-- 討論回覆 (每個主題 5-30 個回覆)
DECLARE @PostCounter INT = 1;
DECLARE @TotalPosts INT = 15000;

PRINT '開始插入討論回覆...';

WHILE @PostCounter <= @TotalPosts
BEGIN
    DECLARE @RandomThreadId BIGINT = (SELECT TOP 1 thread_id FROM threads ORDER BY NEWID());
    DECLARE @RandomReplyAuthorId INT = 1 + (@PostCounter % (SELECT COUNT(*) FROM Users));
    DECLARE @ReplyContent NVARCHAR(MAX) = 
        CASE (@PostCounter % 15)
            WHEN 0 THEN '感謝分享！這個攻略真的很實用，我試過了效果很好。'
            WHEN 1 THEN '我也遇到同樣的問題，期待有經驗的玩家能夠解答。'
            WHEN 2 THEN '這個更新確實有很多改進，但也有一些需要適應的地方。'
            WHEN 3 THEN '樓主說得對，我覺得這個策略在當前版本很有效。'
            WHEN 4 THEN '補充一下我的經驗：建議新手先從簡單模式開始練習。'
            WHEN 5 THEN '圖片很精美！可以分享一下拍攝的設定嗎？'
            WHEN 6 THEN '我們團隊還缺一個人，有興趣的可以私訊我。'
            WHEN 7 THEN '這場比賽真的很精彩，雙方都發揮得很好。'
            WHEN 8 THEN '詳細的教學！收藏了，改天來試試看這個技巧。'
            WHEN 9 THEN '客觀的評測，幫助我決定要不要入手這款遊戲。'
            WHEN 10 THEN '期待後續更新，希望能加入更多有趣的內容。'
            WHEN 11 THEN '懷念以前的版本，那時候的平衡性更好一些。'
            WHEN 12 THEN '創意十足的作品！作者的想像力真豐富。'
            WHEN 13 THEN '深入的分析，讓我對遊戲機制有了更好的理解。'
            ELSE '同意樓主的觀點，這確實是一個值得討論的話題。'
        END + 
        CASE (@PostCounter % 10)
            WHEN 0 THEN ' 另外想問一下，有推薦的相關資源嗎？'
            WHEN 1 THEN ' 我會繼續關注這個話題的發展。'
            WHEN 2 THEN ' 希望官方能看到這些建議並採納。'
            WHEN 3 THEN ' 大家還有其他想法歡迎一起討論。'
            WHEN 4 THEN ' 感謝樓主花時間整理這些資訊。'
            ELSE ''
        END;

    INSERT INTO thread_posts (thread_id, author_user_id, content_md, parent_post_id, status, created_at, updated_at)
    VALUES (
        @RandomThreadId,
        @RandomReplyAuthorId,
        @ReplyContent,
        CASE WHEN @PostCounter % 10 = 0 THEN (SELECT TOP 1 id FROM thread_posts WHERE thread_id = @RandomThreadId ORDER BY NEWID()) ELSE NULL END,
        'normal',
        DATEADD(HOUR, -(@PostCounter % 720), GETUTCDATE()),
        DATEADD(HOUR, -(@PostCounter % 720), GETUTCDATE())
    );

    SET @PostCounter = @PostCounter + 1;
END

PRINT '✓ 討論回覆插入完成 (' + CAST(@TotalPosts AS VARCHAR) + ' 則回覆)';

-- =====================================================
-- 8. 插入互動資料 (讚、收藏)
-- =====================================================

-- 文章按讚 (隨機產生)
INSERT INTO reactions (user_id, target_type, target_id, kind, created_at)
SELECT TOP 10000
    1 + (ABS(CHECKSUM(NEWID())) % (SELECT COUNT(*) FROM Users)),
    'thread_post',
    tp.id,
    CASE (ABS(CHECKSUM(NEWID())) % 5)
        WHEN 0 THEN 'like'
        WHEN 1 THEN 'love'
        WHEN 2 THEN 'laugh'
        WHEN 3 THEN 'wow'
        ELSE 'thumbs_up'
    END,
    DATEADD(HOUR, -(ABS(CHECKSUM(NEWID())) % 720), GETUTCDATE())
FROM thread_posts tp
ORDER BY NEWID();

-- 主題收藏
INSERT INTO bookmarks (user_id, target_type, target_id, created_at)
SELECT TOP 3000
    1 + (ABS(CHECKSUM(NEWID())) % (SELECT COUNT(*) FROM Users)),
    'thread',
    t.thread_id,
    DATEADD(DAY, -(ABS(CHECKSUM(NEWID())) % 30), GETUTCDATE())
FROM threads t
ORDER BY NEWID();

PRINT '✓ 互動資料插入完成 (讚: ' + CAST((SELECT COUNT(*) FROM reactions) AS VARCHAR) + ', 收藏: ' + CAST((SELECT COUNT(*) FROM bookmarks) AS VARCHAR) + ')';

-- =====================================================
-- 9. 插入玩家市場資料
-- =====================================================

-- 玩家市場商品 (有銷售權限的使用者)
INSERT INTO PlayerMarketProductInfo (p_product_type, p_product_title, p_product_name, p_product_description, seller_id, p_status, price, created_at)
SELECT TOP 800
    CASE (ur.User_Id % 5)
        WHEN 0 THEN '遊戲道具'
        WHEN 1 THEN '虛擬貨幣'
        WHEN 2 THEN '角色裝備'
        WHEN 3 THEN '稀有材料'
        ELSE '收藏品'
    END,
    CASE (ur.User_Id % 10)
        WHEN 0 THEN '🔥限時特價🔥 '
        WHEN 1 THEN '⭐稀有物品⭐ '
        WHEN 2 THEN '💎頂級裝備💎 '
        WHEN 3 THEN '🎁超值組合🎁 '
        WHEN 4 THEN '🏆冠軍專用🏆 '
        WHEN 5 THEN '✨閃亮登場✨ '
        WHEN 6 THEN '🚀火箭加速🚀 '
        WHEN 7 THEN '🌟明星推薦🌟 '
        WHEN 8 THEN '💰划算好物💰 '
        ELSE '🎯精準命中🎯 '
    END + 
    CASE (ur.User_Id % 8)
        WHEN 0 THEN '傳說級武器'
        WHEN 1 THEN '史詩裝備'
        WHEN 2 THEN '稀有坐騎'
        WHEN 3 THEN '限定外觀'
        WHEN 4 THEN '強化石'
        WHEN 5 THEN '經驗藥水'
        WHEN 6 THEN '復活道具'
        ELSE '神秘寶箱'
    END,
    CASE (ur.User_Id % 8)
        WHEN 0 THEN '傳說級武器 - 龍鱗劍'
        WHEN 1 THEN '史詩裝備 - 鳳凰戰甲'
        WHEN 2 THEN '稀有坐騎 - 獨角獸'
        WHEN 3 THEN '限定外觀 - 星空套裝'
        WHEN 4 THEN '強化石 +15'
        WHEN 5 THEN '大型經驗藥水'
        WHEN 6 THEN '高級復活道具'
        ELSE '神秘黃金寶箱'
    END,
    '這是一個' + 
    CASE (ur.User_Id % 6)
        WHEN 0 THEN '極其稀有的頂級道具，擁有強大的屬性加成和華麗的外觀效果。'
        WHEN 1 THEN '實用的遊戲道具，能大幅提升角色能力和遊戲體驗。'
        WHEN 2 THEN '限時活動獲得的特殊物品，數量有限，錯過不再。'
        WHEN 3 THEN '高品質的裝備，適合中高級玩家使用，性價比超高。'
        WHEN 4 THEN '新手友好的道具，幫助快速提升等級和戰鬥力。'
        ELSE '收藏價值極高的紀念品，展示玩家的成就和地位。'
    END + ' 保證品質，安全交易，售後無憂！',
    ur.User_Id,
    CASE WHEN ur.User_Id % 20 = 0 THEN 'Sold' ELSE 'Active' END,
    (ur.User_Id * 7 + 50) % 5000 + 100, -- 價格 100-5100
    DATEADD(DAY, -(ur.User_Id % 60), GETUTCDATE())
FROM User_Rights ur
WHERE ur.SalesAuthority = 1
ORDER BY NEWID();

PRINT '✓ 玩家市場商品插入完成 (' + CAST(@@ROWCOUNT AS VARCHAR) + ' 項商品)';

-- =====================================================
-- 10. 插入聊天和通知資料
-- =====================================================

-- 私人聊天訊息 (隨機使用者間的對話)
INSERT INTO Chat_Message (sender_id, receiver_id, chat_content, sent_at, is_read)
SELECT TOP 5000
    1 + (ABS(CHECKSUM(NEWID())) % (SELECT COUNT(*) FROM Users)),
    1 + (ABS(CHECKSUM(NEWID())) % (SELECT COUNT(*) FROM Users)),
    CASE (ABS(CHECKSUM(NEWID())) % 15)
        WHEN 0 THEN '你好！想請問一下遊戲的問題'
        WHEN 1 THEN '謝謝你的幫助，問題解決了！'
        WHEN 2 THEN '有空一起玩遊戲嗎？'
        WHEN 3 THEN '你的攻略寫得真好，學到很多'
        WHEN 4 THEN '最近有什麼好玩的新遊戲推薦嗎？'
        WHEN 5 THEN '今天的活動你參加了嗎？'
        WHEN 6 THEN '這個道具要怎麼獲得？'
        WHEN 7 THEN '你的寵物好可愛！'
        WHEN 8 THEN '一起組隊刷副本吧'
        WHEN 9 THEN '恭喜你升級了！'
        WHEN 10 THEN '你在線上嗎？有事想討論'
        WHEN 11 THEN '感謝分享遊戲心得'
        WHEN 12 THEN '這個技巧太實用了'
        WHEN 13 THEN '下次活動什麼時候開始？'
        ELSE '晚安，明天見！'
    END,
    DATEADD(HOUR, -(ABS(CHECKSUM(NEWID())) % 168), GETUTCDATE()),
    CASE WHEN ABS(CHECKSUM(NEWID())) % 3 = 0 THEN 1 ELSE 0 END
FROM sys.objects o1, sys.objects o2
WHERE ABS(CHECKSUM(NEWID())) % (SELECT COUNT(*) FROM Users) != ABS(CHECKSUM(NEWID())) % (SELECT COUNT(*) FROM Users);

PRINT '✓ 聊天訊息插入完成 (' + CAST(@@ROWCOUNT AS VARCHAR) + ' 則訊息)';

-- 群組資料
INSERT INTO Groups (group_name, created_by, created_at)
SELECT TOP 50
    CASE (ROW_NUMBER() OVER (ORDER BY User_ID) % 10)
        WHEN 0 THEN '遊戲攻略討論群'
        WHEN 1 THEN '電競戰隊招募'
        WHEN 2 THEN '休閒玩家聚會'
        WHEN 3 THEN '新手互助會'
        WHEN 4 THEN '高手技巧分享'
        WHEN 5 THEN '遊戲情報站'
        WHEN 6 THEN '周末組隊群'
        WHEN 7 THEN '裝備交易所'
        WHEN 8 THEN '賽事觀戰團'
        ELSE '綜合討論區'
    END + ' #' + CAST(ROW_NUMBER() OVER (ORDER BY User_ID) AS VARCHAR),
    User_ID,
    DATEADD(DAY, -(ROW_NUMBER() OVER (ORDER BY User_ID) % 180), GETUTCDATE())
FROM Users
ORDER BY NEWID();

PRINT '✓ 群組資料插入完成 (' + CAST(@@ROWCOUNT AS VARCHAR) + ' 個群組)';

-- =====================================================
-- 11. 更新使用者點數 (根據簽到和遊戲記錄)
-- =====================================================

-- 根據簽到記錄更新使用者點數
UPDATE uw SET User_Point = uw.User_Point + ISNULL(signin_points.total_points, 0)
FROM User_wallet uw
LEFT JOIN (
    SELECT 
        UserID, 
        SUM(PointsChanged) as total_points
    FROM UserSignInStats 
    GROUP BY UserID
) signin_points ON uw.User_Id = signin_points.UserID;

-- 根據小遊戲記錄更新使用者點數
UPDATE uw SET User_Point = uw.User_Point + ISNULL(game_points.total_points, 0)
FROM User_wallet uw
LEFT JOIN (
    SELECT 
        UserID, 
        SUM(PointsChanged) as total_points
    FROM MiniGame 
    GROUP BY UserID
) game_points ON uw.User_Id = game_points.UserID;

-- 根據小遊戲記錄更新寵物經驗
UPDATE p SET Experience = p.Experience + ISNULL(game_exp.total_exp, 0)
FROM Pet p
LEFT JOIN (
    SELECT 
        UserID, 
        SUM(ExpGained) as total_exp
    FROM MiniGame 
    GROUP BY UserID
) game_exp ON p.UserID = game_exp.UserID;

-- 根據簽到記錄更新寵物經驗
UPDATE p SET Experience = p.Experience + ISNULL(signin_exp.total_exp, 0)
FROM Pet p
LEFT JOIN (
    SELECT 
        UserID, 
        SUM(ExpGained) as total_exp
    FROM UserSignInStats 
    GROUP BY UserID
) signin_exp ON p.UserID = signin_exp.UserID;

PRINT '✓ 使用者點數和寵物經驗更新完成';

-- =====================================================
-- 12. 插入熱度和排行榜資料
-- =====================================================

-- 數據來源
INSERT INTO metric_sources (name, note, created_at) VALUES 
('Steam', 'Steam 平台同時在線人數和評論數據', GETUTCDATE()),
('Twitch', 'Twitch 直播觀看人數和聊天活躍度', GETUTCDATE()),
('YouTube', 'YouTube 遊戲相關影片觀看數和互動數', GETUTCDATE()),
('Reddit', 'Reddit 遊戲板塊貼文數和討論熱度', GETUTCDATE()),
('巴哈姆特', '巴哈姆特論壇討論數和文章熱度', GETUTCDATE()),
('Discord', 'Discord 遊戲社群活躍度', GETUTCDATE());

-- 指標定義
INSERT INTO metrics (source_id, code, unit, description, is_active, created_at)
SELECT 
    s.source_id,
    CASE (s.source_id % 4)
        WHEN 1 THEN 'concurrent_users'
        WHEN 2 THEN 'forum_posts'
        WHEN 3 THEN 'video_views'
        ELSE 'chat_messages'
    END,
    CASE (s.source_id % 4)
        WHEN 1 THEN 'users'
        WHEN 2 THEN 'posts'
        WHEN 3 THEN 'views'
        ELSE 'messages'
    END,
    CASE (s.source_id % 4)
        WHEN 1 THEN '同時在線人數'
        WHEN 2 THEN '論壇貼文數量'
        WHEN 3 THEN '影片觀看次數'
        ELSE '聊天訊息數量'
    END,
    1,
    GETUTCDATE()
FROM metric_sources s;

-- 遊戲熱度每日資料 (過去 30 天)
DECLARE @MetricDate DATE = DATEADD(DAY, -30, GETDATE());
WHILE @MetricDate <= GETDATE()
BEGIN
    INSERT INTO game_metric_daily (game_id, metric_id, date, value, agg_method, quality, created_at, updated_at)
    SELECT 
        g.game_id,
        m.metric_id,
        @MetricDate,
        (ABS(CHECKSUM(NEWID())) % 100000) + 1000, -- 隨機數值 1000-101000
        'avg',
        'real',
        GETUTCDATE(),
        GETUTCDATE()
    FROM games g
    CROSS JOIN metrics m
    WHERE (g.game_id + m.metric_id + DATEPART(DAYOFYEAR, @MetricDate)) % 3 = 0; -- 隨機選擇部分組合

    SET @MetricDate = DATEADD(DAY, 1, @MetricDate);
END

-- 每日熱度指數 (根據指標計算)
INSERT INTO popularity_index_daily (game_id, date, index_value, created_at)
SELECT 
    gmd.game_id,
    gmd.date,
    AVG(gmd.value) / 1000.0, -- 簡化的指數計算
    GETUTCDATE()
FROM game_metric_daily gmd
GROUP BY gmd.game_id, gmd.date;

-- 榜單快照 (每日前 10 名)
INSERT INTO leaderboard_snapshots (period, ts, rank, game_id, index_value, created_at)
SELECT 
    'daily',
    CAST(date AS DATETIME2),
    ROW_NUMBER() OVER (PARTITION BY date ORDER BY index_value DESC),
    game_id,
    index_value,
    GETUTCDATE()
FROM (
    SELECT TOP 300
        date,
        game_id,
        index_value,
        ROW_NUMBER() OVER (PARTITION BY date ORDER BY index_value DESC) as daily_rank
    FROM popularity_index_daily
) ranked
WHERE daily_rank <= 10;

PRINT '✓ 熱度和排行榜資料插入完成';

-- =====================================================
-- 13. 插入管理員資料
-- =====================================================

-- 管理員帳號
INSERT INTO ManagerData (Manager_Name, Manager_Account, Manager_Password, Administrator_registration_date) VALUES 
('系統管理員', 'admin', '$2a$11$' + REPLICATE('a', 53), GETUTCDATE()),
('溫傑揚', 'wenjieyang', '$2a$11$' + REPLICATE('b', 53), GETUTCDATE()),
('鐘群能', 'zhongqunneng', '$2a$11$' + REPLICATE('c', 53), GETUTCDATE()),
('房立堯', 'fangliyao', '$2a$11$' + REPLICATE('d', 53), GETUTCDATE()),
('成博儒', 'chengboru', '$2a$11$' + REPLICATE('e', 53), GETUTCDATE()),
('客服小美', 'service01', '$2a$11$' + REPLICATE('f', 53), GETUTCDATE()),
('客服小王', 'service02', '$2a$11$' + REPLICATE('g', 53), GETUTCDATE());

-- 指派管理員角色
INSERT INTO ManagerRole (Manager_Id, ManagerRole_Id, ManagerRole) VALUES 
(1, 1, '超級管理員'),
(2, 1, '超級管理員'),
(3, 5, '寵物管理員'),
(4, 3, '商城管理員'),
(5, 3, '商城管理員'),
(6, 6, '客服專員'),
(7, 6, '客服專員');

-- 管理員登入記錄
INSERT INTO Admins (manager_id, last_login) 
SELECT Manager_Id, DATEADD(HOUR, -(Manager_Id % 48), GETUTCDATE())
FROM ManagerData;

PRINT '✓ 管理員資料插入完成';

-- =====================================================
-- 14. 最終統計和驗證
-- =====================================================

-- 統計插入的資料量
DECLARE @StatsMessage NVARCHAR(2000) = '
🎉 GameCore 假資料插入完成！

📊 資料統計:
• 使用者: ' + CAST((SELECT COUNT(*) FROM Users) AS VARCHAR) + ' 人
• 寵物: ' + CAST((SELECT COUNT(*) FROM Pet) AS VARCHAR) + ' 隻
• 簽到記錄: ' + CAST((SELECT COUNT(*) FROM UserSignInStats) AS VARCHAR) + ' 筆
• 小遊戲記錄: ' + CAST((SELECT COUNT(*) FROM MiniGame) AS VARCHAR) + ' 筆
• 遊戲: ' + CAST((SELECT COUNT(*) FROM games) AS VARCHAR) + ' 款
• 論壇版面: ' + CAST((SELECT COUNT(*) FROM forums) AS VARCHAR) + ' 個
• 討論主題: ' + CAST((SELECT COUNT(*) FROM threads) AS VARCHAR) + ' 個
• 討論回覆: ' + CAST((SELECT COUNT(*) FROM thread_posts) AS VARCHAR) + ' 則
• 商品: ' + CAST((SELECT COUNT(*) FROM ProductInfo) AS VARCHAR) + ' 項
• 玩家市場商品: ' + CAST((SELECT COUNT(*) FROM PlayerMarketProductInfo) AS VARCHAR) + ' 項
• 聊天訊息: ' + CAST((SELECT COUNT(*) FROM Chat_Message) AS VARCHAR) + ' 則
• 互動 (讚): ' + CAST((SELECT COUNT(*) FROM reactions) AS VARCHAR) + ' 次
• 收藏: ' + CAST((SELECT COUNT(*) FROM bookmarks) AS VARCHAR) + ' 次
• 群組: ' + CAST((SELECT COUNT(*) FROM Groups) AS VARCHAR) + ' 個
• 管理員: ' + CAST((SELECT COUNT(*) FROM ManagerData) AS VARCHAR) + ' 人

✅ 總計超過 ' + CAST((
    (SELECT COUNT(*) FROM Users) +
    (SELECT COUNT(*) FROM Pet) +
    (SELECT COUNT(*) FROM UserSignInStats) +
    (SELECT COUNT(*) FROM MiniGame) +
    (SELECT COUNT(*) FROM thread_posts) +
    (SELECT COUNT(*) FROM Chat_Message) +
    (SELECT COUNT(*) FROM reactions)
) AS VARCHAR) + ' 筆資料記錄！

🚀 GameCore 平台已準備就緒，可以開始展示和測試！';

PRINT @StatsMessage;

-- 更新統計資訊
UPDATE STATISTICS Users;
UPDATE STATISTICS Pet;
UPDATE STATISTICS UserSignInStats;
UPDATE STATISTICS MiniGame;
UPDATE STATISTICS threads;
UPDATE STATISTICS thread_posts;

PRINT '✅ 資料庫統計資訊更新完成，效能已優化！';
GO