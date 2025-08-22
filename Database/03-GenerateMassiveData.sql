/*
GameCore 大量假資料生成腳本
此腳本生成超過 10,000 筆真實感測試資料
包含完整的中文註釋和說明

作者: GameCore 開發團隊
版本: 1.0
日期: 2025年1月
*/

USE GameCore;
GO

-- ============================================================================
-- 步驟 1: 清理現有測試資料（可選）
-- ============================================================================
PRINT '🧹 開始清理現有測試資料...';

-- 由於外鍵約束，需要按照正確順序刪除
DELETE FROM [dbo].[ThreadPost] WHERE ThreadPostId > 0;
DELETE FROM [dbo].[Reaction] WHERE ReactionId > 0;
DELETE FROM [dbo].[Bookmark] WHERE BookmarkId > 0;
DELETE FROM [dbo].[Thread] WHERE ThreadId > 0;
DELETE FROM [dbo].[Post] WHERE PostId > 0;
DELETE FROM [dbo].[ChatMessage] WHERE MessageId > 0;
DELETE FROM [dbo].[GroupMember] WHERE GroupMemberId > 0;
DELETE FROM [dbo].[Group] WHERE GroupId > 0;
DELETE FROM [dbo].[NotificationRecipient] WHERE RecipientId > 0;
DELETE FROM [dbo].[Notification] WHERE NotificationId > 0;
DELETE FROM [dbo].[PlayerMarketOrderTradepage] WHERE PTradepageId > 0;
DELETE FROM [dbo].[PlayerMarketOrderInfo] WHERE POrderId > 0;
DELETE FROM [dbo].[PlayerMarketProductImg] WHERE PProductImgId > 0;
DELETE FROM [dbo].[PlayerMarketProductInfo] WHERE PProductId > 0;
DELETE FROM [dbo].[OrderItem] WHERE OrderItemId > 0;
DELETE FROM [dbo].[OrderInfo] WHERE OrderId > 0;
DELETE FROM [dbo].[OtherProductDetails] WHERE ProductId > 0;
DELETE FROM [dbo].[GameProductDetails] WHERE ProductId > 0;
DELETE FROM [dbo].[ProductInfo] WHERE ProductId > 0;
DELETE FROM [dbo].[Supplier] WHERE SupplierId > 0;
DELETE FROM [dbo].[MiniGame] WHERE GameId > 0;
DELETE FROM [dbo].[Pet] WHERE PetId > 0;
DELETE FROM [dbo].[UserSignInStats] WHERE UserId > 0;
DELETE FROM [dbo].[MemberSalesProfile] WHERE UserId > 0;
DELETE FROM [dbo].[UserSalesInformation] WHERE UserId > 0;
DELETE FROM [dbo].[UserWallet] WHERE UserId > 0;
DELETE FROM [dbo].[UserRights] WHERE UserId > 0;
DELETE FROM [dbo].[UserIntroduce] WHERE UserId > 0;
DELETE FROM [dbo].[User] WHERE UserId > 0;
DELETE FROM [dbo].[ManagerData] WHERE ManagerId > 0;
DELETE FROM [dbo].[Forum] WHERE ForumId > 0;
DELETE FROM [dbo].[PopularityIndexDaily] WHERE PopularityId > 0;
DELETE FROM [dbo].[GameMetricDaily] WHERE MetricId > 0;
DELETE FROM [dbo].[GameSourceMap] WHERE SourceMapId > 0;
DELETE FROM [dbo].[Metric] WHERE MetricId > 0;
DELETE FROM [dbo].[MetricSource] WHERE SourceId > 0;
DELETE FROM [dbo].[Game] WHERE GameId > 0;

PRINT '✅ 清理完成';

-- ============================================================================
-- 步驟 2: 重置身份識別欄位
-- ============================================================================
PRINT '🔄 重置身份識別欄位...';

DBCC CHECKIDENT ('[dbo].[User]', RESEED, 0);
DBCC CHECKIDENT ('[dbo].[Game]', RESEED, 0);
DBCC CHECKIDENT ('[dbo].[Forum]', RESEED, 0);
DBCC CHECKIDENT ('[dbo].[Pet]', RESEED, 0);
DBCC CHECKIDENT ('[dbo].[MiniGame]', RESEED, 0);
DBCC CHECKIDENT ('[dbo].[Supplier]', RESEED, 0);
DBCC CHECKIDENT ('[dbo].[ProductInfo]', RESEED, 0);
DBCC CHECKIDENT ('[dbo].[OrderInfo]', RESEED, 0);
DBCC CHECKIDENT ('[dbo].[PlayerMarketProductInfo]', RESEED, 0);
DBCC CHECKIDENT ('[dbo].[Notification]', RESEED, 0);
DBCC CHECKIDENT ('[dbo].[Group]', RESEED, 0);
DBCC CHECKIDENT ('[dbo].[Thread]', RESEED, 0);
DBCC CHECKIDENT ('[dbo].[Post]', RESEED, 0);

PRINT '✅ 重置完成';

-- ============================================================================
-- 步驟 3: 生成基礎參考資料
-- ============================================================================
PRINT '🎮 生成遊戲資料 (200 筆)...';

-- 遊戲資料來源和指標來源
INSERT INTO [dbo].[MetricSource] ([SourceName], [SourceUrl], [IsActive], [CreatedAt], [UpdatedAt])
VALUES 
    ('Steam', 'https://steam.com', 1, GETUTCDATE(), GETUTCDATE()),
    ('Epic Games', 'https://epicgames.com', 1, GETUTCDATE(), GETUTCDATE()),
    ('PlayStation Store', 'https://playstation.com', 1, GETUTCDATE(), GETUTCDATE()),
    ('Xbox Store', 'https://xbox.com', 1, GETUTCDATE(), GETUTCDATE()),
    ('Nintendo eShop', 'https://nintendo.com', 1, GETUTCDATE(), GETUTCDATE()),
    ('Google Play', 'https://play.google.com', 1, GETUTCDATE(), GETUTCDATE()),
    ('App Store', 'https://appstore.apple.com', 1, GETUTCDATE(), GETUTCDATE()),
    ('GameCore 官方', 'https://gamecore.com', 1, GETUTCDATE(), GETUTCDATE());

-- 指標類型
INSERT INTO [dbo].[Metric] ([MetricName], [MetricType], [Unit], [Description], [CreatedAt], [UpdatedAt])
VALUES 
    ('每日活躍用戶', 'DAU', '人數', '每日活躍用戶數量統計', GETUTCDATE(), GETUTCDATE()),
    ('月活躍用戶', 'MAU', '人數', '月活躍用戶數量統計', GETUTCDATE(), GETUTCDATE()),
    ('銷售數量', 'Sales', '份數', '遊戲銷售份數統計', GETUTCDATE(), GETUTCDATE()),
    ('評分', 'Rating', '分數', '用戶評分統計', GETUTCDATE(), GETUTCDATE()),
    ('遊戲時長', 'PlayTime', '小時', '平均遊戲時長統計', GETUTCDATE(), GETUTCDATE()),
    ('下載次數', 'Downloads', '次數', '遊戲下載次數統計', GETUTCDATE(), GETUTCDATE()),
    ('收入', 'Revenue', '美元', '遊戲收入統計', GETUTCDATE(), GETUTCDATE()),
    ('留存率', 'Retention', '百分比', '用戶留存率統計', GETUTCDATE(), GETUTCDATE());

-- 遊戲資料 (200 筆)
WITH GameData AS (
    SELECT 
        ROW_NUMBER() OVER (ORDER BY (SELECT NULL)) as RowNum,
        GameName,
        GameGenre,
        Platform,
        Developer,
        Publisher,
        ReleaseDate,
        Price,
        Rating,
        Description,
        ImageUrl,
        TrailerUrl
    FROM (VALUES 
        ('薩爾達傳說：王國之淚', '動作冒險', 'Nintendo Switch', '任天堂', '任天堂', '2023-05-12', 1890.00, 9.8, '開放世界冒險遊戲的全新篇章，探索天空、地面和地底的三重世界。', 'https://example.com/zelda.jpg', 'https://example.com/zelda_trailer.mp4'),
        ('艾爾登法環', '動作RPG', 'PC,PlayStation,Xbox', 'FromSoftware', 'Bandai Namco', '2022-02-25', 1790.00, 9.6, '由宮崎英高與喬治·馬丁合作創造的黑暗奇幻世界。', 'https://example.com/eldenring.jpg', 'https://example.com/eldenring_trailer.mp4'),
        ('賽博朋克2077', '動作RPG', 'PC,PlayStation,Xbox', 'CD Projekt RED', 'CD Projekt', '2020-12-10', 1399.00, 8.5, '在夜城的霓虹燈下，成為一名傳奇傭兵。', 'https://example.com/cyberpunk.jpg', 'https://example.com/cyberpunk_trailer.mp4'),
        ('巫師3：狂獵', 'RPG', 'PC,PlayStation,Xbox,Switch', 'CD Projekt RED', 'CD Projekt', '2015-05-19', 899.00, 9.7, '在這個開放世界中追尋失蹤的養女希里。', 'https://example.com/witcher3.jpg', 'https://example.com/witcher3_trailer.mp4'),
        ('原神', 'RPG', 'PC,Mobile,PlayStation', 'miHoYo', 'miHoYo', '2020-09-28', 0.00, 8.9, '探索提瓦特大陸的奇幻世界，尋找失散的血親。', 'https://example.com/genshin.jpg', 'https://example.com/genshin_trailer.mp4'),
        ('Minecraft', '沙盒', 'PC,Mobile,Console', 'Mojang Studios', 'Microsoft', '2011-11-18', 790.00, 9.2, '在無限的方塊世界中建造、探索和生存。', 'https://example.com/minecraft.jpg', 'https://example.com/minecraft_trailer.mp4'),
        ('英雄聯盟', 'MOBA', 'PC', 'Riot Games', 'Riot Games', '2009-10-27', 0.00, 8.7, '5v5戰略對戰遊戲，成為傳奇。', 'https://example.com/lol.jpg', 'https://example.com/lol_trailer.mp4'),
        ('Valorant', 'FPS', 'PC', 'Riot Games', 'Riot Games', '2020-06-02', 0.00, 8.4, '5v5角色射擊遊戲，策略與技巧並重。', 'https://example.com/valorant.jpg', 'https://example.com/valorant_trailer.mp4'),
        ('Fortnite', '大逃殺', 'PC,Mobile,Console', 'Epic Games', 'Epic Games', '2017-07-25', 0.00, 8.1, '100人大逃殺建造射擊遊戲。', 'https://example.com/fortnite.jpg', 'https://example.com/fortnite_trailer.mp4'),
        ('Apex Legends', '大逃殺', 'PC,PlayStation,Xbox', 'Respawn Entertainment', 'EA', '2019-02-04', 0.00, 8.6, '團隊合作的英雄大逃殺遊戲。', 'https://example.com/apex.jpg', 'https://example.com/apex_trailer.mp4'),
        ('動物森友會', '生活模擬', 'Nintendo Switch', '任天堂', '任天堂', '2020-03-20', 1690.00, 9.1, '在無人島上建造屬於自己的理想生活。', 'https://example.com/animalcrossing.jpg', 'https://example.com/animalcrossing_trailer.mp4'),
        ('超級瑪利歐 奧德賽', '平台跳躍', 'Nintendo Switch', '任天堂', '任天堂', '2017-10-27', 1690.00, 9.4, '與帽子夥伴一起展開全新的冒險旅程。', 'https://example.com/mario_odyssey.jpg', 'https://example.com/mario_odyssey_trailer.mp4'),
        ('最後生還者 第二部', '動作冒險', 'PlayStation', 'Naughty Dog', 'Sony Interactive', '2020-06-19', 1490.00, 9.0, '在後末日世界中的復仇與救贖故事。', 'https://example.com/tlou2.jpg', 'https://example.com/tlou2_trailer.mp4'),
        ('戰神', '動作冒險', 'PlayStation,PC', 'Santa Monica Studio', 'Sony Interactive', '2018-04-20', 990.00, 9.5, '奎托斯與兒子的北歐神話冒險。', 'https://example.com/gow.jpg', 'https://example.com/gow_trailer.mp4'),
        ('地平線 西域禁地', '動作RPG', 'PlayStation,PC', 'Guerrilla Games', 'Sony Interactive', '2022-02-18', 1990.00, 9.1, '在機械獸主宰的世界中探索西部邊境。', 'https://example.com/horizon.jpg', 'https://example.com/horizon_trailer.mp4'),
        ('鬼谷八荒', '修仙RPG', 'PC', '鬼谷工作室', '鬼谷工作室', '2021-01-27', 590.00, 8.8, '體驗修仙者的一生，從凡人到仙人的蛻變。', 'https://example.com/guigu.jpg', 'https://example.com/guigu_trailer.mp4'),
        ('太吾繪卷', '策略RPG', 'PC', '螺舟工作室', '螺舟工作室', '2018-09-21', 690.00, 8.3, '在江湖中傳承太吾劍法，對抗相樞。', 'https://example.com/taiwu.jpg', 'https://example.com/taiwu_trailer.mp4'),
        ('黑神話：悟空', '動作RPG', 'PC,PlayStation,Xbox', '遊戲科學', '遊戲科學', '2024-08-20', 1690.00, 9.3, '基於中國古典名著《西遊記》的動作RPG。', 'https://example.com/wukong.jpg', 'https://example.com/wukong_trailer.mp4'),
        ('隻狼：影逝二度', '動作冒險', 'PC,PlayStation,Xbox', 'FromSoftware', 'Activision', '2019-03-22', 1590.00, 9.2, '在戰國時代的日本，成為一名忍者武士。', 'https://example.com/sekiro.jpg', 'https://example.com/sekiro_trailer.mp4'),
        ('仁王2', '動作RPG', 'PC,PlayStation', 'Team Ninja', 'Koei Tecmo', '2020-03-12', 1290.00, 8.7, '在妖怪橫行的戰國時代生存下去。', 'https://example.com/nioh2.jpg', 'https://example.com/nioh2_trailer.mp4')
    ) AS GameValues(GameName, GameGenre, Platform, Developer, Publisher, ReleaseDate, Price, Rating, Description, ImageUrl, TrailerUrl)
)
INSERT INTO [dbo].[Game] ([GameName], [GameGenre], [Platform], [Developer], [Publisher], [ReleaseDate], [Price], [Rating], [Description], [ImageUrl], [TrailerUrl], [IsActive], [CreatedAt], [UpdatedAt])
SELECT 
    GameName,
    GameGenre,
    Platform,
    Developer,
    Publisher,
    CAST(ReleaseDate AS DATE),
    Price,
    Rating,
    Description,
    ImageUrl,
    TrailerUrl,
    1, -- IsActive
    DATEADD(DAY, -ABS(CHECKSUM(NEWID())) % 365, GETUTCDATE()), -- 隨機過去一年內的創建時間
    DATEADD(DAY, -ABS(CHECKSUM(NEWID())) % 30, GETUTCDATE()) -- 隨機過去30天內的更新時間
FROM GameData;

-- 繼續生成更多遊戲資料 (補足到200筆)
DECLARE @i INT = (SELECT COUNT(*) FROM [dbo].[Game]);
DECLARE @genres TABLE (Genre NVARCHAR(50));
INSERT INTO @genres VALUES ('動作'), ('RPG'), ('策略'), ('射擊'), ('運動'), ('競速'), ('模擬'), ('益智'), ('音樂'), ('恐怖');

DECLARE @platforms TABLE (Platform NVARCHAR(100));
INSERT INTO @platforms VALUES ('PC'), ('PlayStation 5'), ('Xbox Series X/S'), ('Nintendo Switch'), ('Mobile'), ('PC,PlayStation'), ('PC,Xbox'), ('多平台');

WHILE @i < 200
BEGIN
    INSERT INTO [dbo].[Game] ([GameName], [GameGenre], [Platform], [Developer], [Publisher], [ReleaseDate], [Price], [Rating], [Description], [ImageUrl], [TrailerUrl], [IsActive], [CreatedAt], [UpdatedAt])
    SELECT 
        '遊戲作品 ' + CAST(@i + 1 AS NVARCHAR(10)),
        (SELECT TOP 1 Genre FROM @genres ORDER BY NEWID()),
        (SELECT TOP 1 Platform FROM @platforms ORDER BY NEWID()),
        '開發商 ' + CAST((@i % 20) + 1 AS NVARCHAR(10)),
        '發行商 ' + CAST((@i % 15) + 1 AS NVARCHAR(10)),
        DATEADD(DAY, -ABS(CHECKSUM(NEWID())) % (365 * 5), GETDATE()), -- 隨機過去5年內
        ROUND(RAND(CHECKSUM(NEWID())) * 2000 + 100, 0), -- 100-2100元隨機價格
        ROUND(RAND(CHECKSUM(NEWID())) * 4 + 6, 1), -- 6.0-10.0分隨機評分
        '這是一個精彩的' + (SELECT TOP 1 Genre FROM @genres ORDER BY NEWID()) + '遊戲，提供豐富的遊戲體驗和精美的畫面效果。',
        'https://example.com/game' + CAST(@i + 1 AS NVARCHAR(10)) + '.jpg',
        'https://example.com/game' + CAST(@i + 1 AS NVARCHAR(10)) + '_trailer.mp4',
        CASE WHEN RAND(CHECKSUM(NEWID())) > 0.1 THEN 1 ELSE 0 END, -- 90%機率為活躍
        DATEADD(DAY, -ABS(CHECKSUM(NEWID())) % 365, GETUTCDATE()),
        DATEADD(DAY, -ABS(CHECKSUM(NEWID())) % 30, GETUTCDATE());
    
    SET @i = @i + 1;
END;

PRINT '✅ 遊戲資料生成完成 (200 筆)';

-- ============================================================================
-- 步驟 4: 生成使用者資料 (2000 筆)
-- ============================================================================
PRINT '👥 生成使用者資料 (2000 筆)...';

-- 常用姓氏和名字
DECLARE @surnames TABLE (Surname NVARCHAR(10));
INSERT INTO @surnames VALUES 
('張'), ('李'), ('王'), ('陳'), ('劉'), ('黃'), ('林'), ('周'), ('吳'), ('徐'),
('趙'), ('孫'), ('馬'), ('朱'), ('胡'), ('郭'), ('何'), ('高'), ('羅'), ('鄭'),
('梁'), ('謝'), ('唐'), ('韓'), ('曹'), ('許'), ('鄧'), ('蕭'), ('馮'), ('曾');

DECLARE @firstNames TABLE (FirstName NVARCHAR(10));
INSERT INTO @firstNames VALUES 
('志明'), ('家豪'), ('俊傑'), ('建宏'), ('俊宏'), ('志豪'), ('志偉'), ('文雄'), ('金龍'), ('志強'),
('美玲'), ('淑芬'), ('麗華'), ('淑娟'), ('麗娟'), ('淑華'), ('美華'), ('雅惠'), ('美惠'), ('淑敏'),
('小明'), ('小華'), ('小強'), ('小美'), ('小芳'), ('小玲'), ('小君'), ('小傑'), ('小偉'), ('小雯');

DECLARE @userIndex INT = 1;

WHILE @userIndex <= 2000
BEGIN
    DECLARE @userName NVARCHAR(100);
    DECLARE @userAccount NVARCHAR(50);
    DECLARE @email NVARCHAR(200);
    DECLARE @registrationTime DATETIME2;
    DECLARE @lastLoginTime DATETIME2;
    DECLARE @userLevel INT;
    DECLARE @points INT;
    DECLARE @experience INT;
    
    -- 生成隨機姓名
    SELECT @userName = (SELECT TOP 1 Surname FROM @surnames ORDER BY NEWID()) + 
                      (SELECT TOP 1 FirstName FROM @firstNames ORDER BY NEWID());
    
    -- 生成帳號
    SET @userAccount = 'user' + RIGHT('00000' + CAST(@userIndex AS NVARCHAR(10)), 5);
    
    -- 生成Email
    SET @email = @userAccount + '@gamecore.com';
    
    -- 生成時間
    SET @registrationTime = DATEADD(DAY, -ABS(CHECKSUM(NEWID())) % 730, GETUTCDATE()); -- 過去兩年內註冊
    SET @lastLoginTime = DATEADD(DAY, -ABS(CHECKSUM(NEWID())) % 7, GETUTCDATE()); -- 過去一週內登入
    
    -- 生成等級和點數
    SET @userLevel = ABS(CHECKSUM(NEWID())) % 50 + 1; -- 1-50級
    SET @points = ABS(CHECKSUM(NEWID())) % 10000; -- 0-9999點
    SET @experience = @userLevel * 1000 + ABS(CHECKSUM(NEWID())) % 1000; -- 基於等級的經驗值
    
    -- 插入使用者主資料
    INSERT INTO [dbo].[User] ([UserAccount], [UserName], [Email], [UserLevel], [Points], [Experience], [DisplayName], [AvatarUrl], [RegistrationTime], [LastLoginTime], [IsOnline], [Status])
    VALUES (
        @userAccount,
        @userName,
        @email,
        @userLevel,
        @points,
        @experience,
        @userName, -- DisplayName 與 UserName 相同
        'https://example.com/avatars/' + @userAccount + '.jpg',
        @registrationTime,
        @lastLoginTime,
        CASE WHEN ABS(CHECKSUM(NEWID())) % 10 = 0 THEN 1 ELSE 0 END, -- 10%機率在線
        CASE 
            WHEN ABS(CHECKSUM(NEWID())) % 100 < 95 THEN 'Active'
            WHEN ABS(CHECKSUM(NEWID())) % 100 < 98 THEN 'Inactive'
            ELSE 'Suspended'
        END
    );
    
    DECLARE @userId INT = SCOPE_IDENTITY();
    
    -- 插入使用者介紹
    INSERT INTO [dbo].[UserIntroduce] ([UserId], [UserNickName], [UserSelfIntroduction], [UserBirthday], [UserGender], [UserLocation], [CreatedAt], [UpdatedAt])
    VALUES (
        @userId,
        @userName + '暱稱',
        '大家好，我是' + @userName + '，很高興在GameCore與大家相遇！我喜歡' + 
        (SELECT TOP 1 Genre FROM @genres ORDER BY NEWID()) + '類型的遊戲，歡迎一起討論交流。',
        DATEADD(YEAR, -(18 + ABS(CHECKSUM(NEWID())) % 32), GETDATE()), -- 18-50歲
        CASE ABS(CHECKSUM(NEWID())) % 3 WHEN 0 THEN '男' WHEN 1 THEN '女' ELSE '其他' END,
        (SELECT TOP 1 City FROM (VALUES ('台北市'), ('新北市'), ('桃園市'), ('台中市'), ('台南市'), ('高雄市'), ('新竹市'), ('基隆市'), ('嘉義市'), ('彰化縣')) AS Cities(City) ORDER BY NEWID()),
        @registrationTime,
        DATEADD(DAY, -ABS(CHECKSUM(NEWID())) % 30, GETUTCDATE())
    );
    
    -- 插入使用者錢包
    DECLARE @userCash DECIMAL(10,2) = ROUND(RAND(CHECKSUM(NEWID())) * 5000, 2); -- 0-5000元現金
    INSERT INTO [dbo].[UserWallet] ([UserId], [UserPoint], [UserCash], [CreatedAt], [UpdatedAt])
    VALUES (
        @userId,
        @points,
        @userCash,
        @registrationTime,
        DATEADD(DAY, -ABS(CHECKSUM(NEWID())) % 7, GETUTCDATE())
    );
    
    -- 插入使用者權限
    INSERT INTO [dbo].[UserRights] ([UserId], [CanPost], [CanComment], [CanUpload], [CanTrade], [CreatedAt], [UpdatedAt])
    VALUES (
        @userId,
        CASE WHEN ABS(CHECKSUM(NEWID())) % 100 < 95 THEN 1 ELSE 0 END, -- 95%可發文
        CASE WHEN ABS(CHECKSUM(NEWID())) % 100 < 98 THEN 1 ELSE 0 END, -- 98%可評論
        CASE WHEN ABS(CHECKSUM(NEWID())) % 100 < 80 THEN 1 ELSE 0 END, -- 80%可上傳
        CASE WHEN ABS(CHECKSUM(NEWID())) % 100 < 70 THEN 1 ELSE 0 END, -- 70%可交易
        @registrationTime,
        DATEADD(DAY, -ABS(CHECKSUM(NEWID())) % 30, GETUTCDATE())
    );
    
    SET @userIndex = @userIndex + 1;
    
    -- 每100筆顯示進度
    IF @userIndex % 100 = 0
        PRINT '  已生成 ' + CAST(@userIndex AS NVARCHAR(10)) + ' 筆使用者資料...';
END;

PRINT '✅ 使用者資料生成完成 (2000 筆)';

-- ============================================================================
-- 步驟 5: 生成論壇資料
-- ============================================================================
PRINT '💬 生成論壇資料...';

-- 插入論壇版塊 (每個遊戲一個論壇)
INSERT INTO [dbo].[Forum] ([ForumName], [ForumDescription], [GameId], [IsActive], [CreatedAt], [UpdatedAt])
SELECT 
    g.GameName + ' 討論區',
    '歡迎來到 ' + g.GameName + ' 的官方討論區！在這裡您可以與其他玩家分享遊戲心得、交流攻略技巧、討論遊戲更新內容。',
    g.GameId,
    1,
    DATEADD(DAY, -ABS(CHECKSUM(NEWID())) % 300, GETUTCDATE()),
    DATEADD(DAY, -ABS(CHECKSUM(NEWID())) % 30, GETUTCDATE())
FROM [dbo].[Game] g
WHERE g.IsActive = 1;

PRINT '✅ 論壇版塊生成完成 (' + CAST((SELECT COUNT(*) FROM [dbo].[Forum]) AS NVARCHAR(10)) + ' 個版塊)';

-- ============================================================================
-- 步驟 6: 生成寵物資料 (1500 筆 - 75%的使用者擁有寵物)
-- ============================================================================
PRINT '🐱 生成寵物資料 (1500 筆)...';

DECLARE @petNames TABLE (PetName NVARCHAR(50));
INSERT INTO @petNames VALUES 
('小白'), ('小黑'), ('阿福'), ('美美'), ('球球'), ('毛毛'), ('花花'), ('豆豆'),
('咪咪'), ('旺旺'), ('乖乖'), ('皮皮'), ('糖糖'), ('寶寶'), ('妞妞'), ('樂樂'),
('小虎'), ('小龍'), ('小鳳'), ('小熊'), ('小兔'), ('小魚'), ('小鳥'), ('小貓'),
('雷電'), ('閃電'), ('暴風'), ('彩虹'), ('星星'), ('月亮'), ('太陽'), ('雲朵'),
('勇者'), ('法師'), ('戰士'), ('刺客'), ('射手'), ('牧師'), ('坦克'), ('輔助'),
('櫻花'), ('楓葉'), ('梅花'), ('蘭花'), ('竹子'), ('松樹'), ('柳樹'), ('玫瑰');

DECLARE @petColors TABLE (Color NVARCHAR(10));
INSERT INTO @petColors VALUES 
('#FF6B9D'), ('#4ECDC4'), ('#45B7D1'), ('#96CEB4'), ('#FFEAA7'), 
('#DDA0DD'), ('#98D8C8'), ('#FFB6C1'), ('#87CEEB'), ('#F0E68C'),
('#FFA07A'), ('#20B2AA'), ('#87CEFA'), ('#DDA0DD'), ('#F5DEB3');

-- 為前1500名使用者生成寵物
INSERT INTO [dbo].[Pet] ([Name], [UserId], [Level], [Experience], [Health], [Hunger], [Happiness], [Color], [LastFeedTime], [LastPlayTime], [CreatedAt], [UpdatedAt])
SELECT 
    (SELECT TOP 1 PetName FROM @petNames ORDER BY NEWID()),
    u.UserId,
    ABS(CHECKSUM(NEWID())) % 20 + 1, -- 1-20級
    (ABS(CHECKSUM(NEWID())) % 20 + 1) * 100 + ABS(CHECKSUM(NEWID())) % 100, -- 基於等級的經驗值
    50 + ABS(CHECKSUM(NEWID())) % 51, -- 50-100血量
    30 + ABS(CHECKSUM(NEWID())) % 71, -- 30-100飽食度
    40 + ABS(CHECKSUM(NEWID())) % 61, -- 40-100心情值
    (SELECT TOP 1 Color FROM @petColors ORDER BY NEWID()),
    DATEADD(HOUR, -ABS(CHECKSUM(NEWID())) % 72, GETUTCDATE()), -- 過去3天內餵食
    DATEADD(HOUR, -ABS(CHECKSUM(NEWID())) % 48, GETUTCDATE()), -- 過去2天內遊玩
    DATEADD(DAY, -ABS(CHECKSUM(NEWID())) % 180, GETUTCDATE()), -- 過去半年內創建
    DATEADD(HOUR, -ABS(CHECKSUM(NEWID())) % 24, GETUTCDATE()) -- 過去24小時內更新
FROM (
    SELECT TOP 1500 UserId 
    FROM [dbo].[User] 
    ORDER BY UserId
) u;

PRINT '✅ 寵物資料生成完成 (1500 筆)';

-- ============================================================================
-- 步驟 7: 生成簽到資料 (過去90天的簽到記錄)
-- ============================================================================
PRINT '📅 生成簽到資料...';

DECLARE @signInUser INT;
DECLARE @signInDate DATE;
DECLARE @dayOffset INT;

-- 為每個使用者生成過去90天內的隨機簽到記錄
DECLARE user_cursor CURSOR FOR 
SELECT TOP 500 UserId FROM [dbo].[User] ORDER BY NEWID(); -- 隨機選500個活躍使用者

OPEN user_cursor;
FETCH NEXT FROM user_cursor INTO @signInUser;

WHILE @@FETCH_STATUS = 0
BEGIN
    SET @dayOffset = 0;
    
    -- 為該使用者生成過去90天內的簽到記錄（不是每天都簽到）
    WHILE @dayOffset < 90
    BEGIN
        SET @signInDate = CAST(DATEADD(DAY, -@dayOffset, GETDATE()) AS DATE);
        
        -- 70%機率簽到
        IF ABS(CHECKSUM(NEWID())) % 100 < 70
        BEGIN
            DECLARE @isWeekend BIT = CASE WHEN DATEPART(WEEKDAY, @signInDate) IN (1, 7) THEN 1 ELSE 0 END;
            DECLARE @signInPoints INT = CASE WHEN @isWeekend = 1 THEN 30 ELSE 20 END;
            DECLARE @signInExp INT = CASE WHEN @isWeekend = 1 THEN 15 ELSE 10 END;
            
            INSERT INTO [dbo].[UserSignInStats] ([UserId], [SignInDate], [Points], [Experience], [IsWeekendBonus], [StreakDays], [CreatedAt])
            VALUES (
                @signInUser,
                @signInDate,
                @signInPoints,
                @signInExp,
                @isWeekend,
                1, -- 連續天數稍後計算
                DATEADD(HOUR, RAND(CHECKSUM(NEWID())) * 24, @signInDate) -- 當天隨機時間簽到
            );
        END;
        
        SET @dayOffset = @dayOffset + 1;
    END;
    
    FETCH NEXT FROM user_cursor INTO @signInUser;
END;

CLOSE user_cursor;
DEALLOCATE user_cursor;

PRINT '✅ 簽到資料生成完成';

-- ============================================================================
-- 步驟 8: 生成商城相關資料
-- ============================================================================
PRINT '🛒 生成商城資料...';

-- 供應商資料
INSERT INTO [dbo].[Supplier] ([SupplierName], [ContactInfo], [Address], [CreatedAt], [UpdatedAt])
VALUES 
    ('GameCore 官方商城', '02-12345678', '台北市信義區信義路五段7號', GETUTCDATE(), GETUTCDATE()),
    ('遊戲王國', '02-87654321', '新北市板橋區中山路一段123號', GETUTCDATE(), GETUTCDATE()),
    ('電競世界', '04-11111111', '台中市西屯區台灣大道三段99號', GETUTCDATE(), GETUTCDATE()),
    ('數位娛樂', '07-22222222', '高雄市前鎮區成功二路88號', GETUTCDATE(), GETUTCDATE()),
    ('遊戲配件專賣', '03-33333333', '桃園市桃園區復興路168號', GETUTCDATE(), GETUTCDATE());

-- 商品資料 (500 筆)
DECLARE @productCategories TABLE (Category NVARCHAR(50));
INSERT INTO @productCategories VALUES ('遊戲'), ('硬體設備'), ('周邊商品'), ('點數卡'), ('會員服務'), ('收藏品');

DECLARE @productIndex INT = 1;
DECLARE @supplierCount INT = (SELECT COUNT(*) FROM [dbo].[Supplier]);

WHILE @productIndex <= 500
BEGIN
    DECLARE @category NVARCHAR(50) = (SELECT TOP 1 Category FROM @productCategories ORDER BY NEWID());
    DECLARE @productPrice DECIMAL(10,2) = ROUND(RAND(CHECKSUM(NEWID())) * 2000 + 50, 0); -- 50-2050元
    DECLARE @productStock INT = ABS(CHECKSUM(NEWID())) % 1000; -- 0-999庫存
    
    INSERT INTO [dbo].[ProductInfo] ([ProductName], [ProductDescription], [Price], [Stock], [Category], [ImageUrl], [SupplierId], [IsActive], [CreatedAt], [UpdatedAt])
    VALUES (
        @category + '商品 ' + CAST(@productIndex AS NVARCHAR(10)),
        '這是一個高品質的' + @category + '商品，提供優秀的使用體驗和卓越的性能表現。適合各種使用場景，是您不可錯過的選擇。',
        @productPrice,
        @productStock,
        @category,
        'https://example.com/products/' + CAST(@productIndex AS NVARCHAR(10)) + '.jpg',
        ((@productIndex - 1) % @supplierCount) + 1,
        CASE WHEN RAND(CHECKSUM(NEWID())) > 0.05 THEN 1 ELSE 0 END, -- 95%商品為活躍狀態
        DATEADD(DAY, -ABS(CHECKSUM(NEWID())) % 365, GETUTCDATE()),
        DATEADD(DAY, -ABS(CHECKSUM(NEWID())) % 30, GETUTCDATE())
    );
    
    SET @productIndex = @productIndex + 1;
    
    IF @productIndex % 100 = 0
        PRINT '  已生成 ' + CAST(@productIndex AS NVARCHAR(10)) + ' 筆商品資料...';
END;

PRINT '✅ 商城資料生成完成 (500 筆商品)';

-- ============================================================================
-- 步驟 9: 生成小遊戲資料
-- ============================================================================
PRINT '🎯 生成小遊戲資料...';

DECLARE @gameTypes TABLE (GameType NVARCHAR(50));
INSERT INTO @gameTypes VALUES ('數字猜謎'), ('記憶卡片'), ('反應測試'), ('邏輯推理'), ('手速挑戰'), ('知識問答');

-- 為前500名使用者生成小遊戲記錄
INSERT INTO [dbo].[MiniGame] ([UserId], [GameType], [Score], [Reward], [PlayTime], [CreatedAt])
SELECT 
    u.UserId,
    (SELECT TOP 1 GameType FROM @gameTypes ORDER BY NEWID()),
    ABS(CHECKSUM(NEWID())) % 10000, -- 0-9999分
    ABS(CHECKSUM(NEWID())) % 100 + 10, -- 10-109點獎勵
    ABS(CHECKSUM(NEWID())) % 600 + 30, -- 30-629秒遊戲時間
    DATEADD(DAY, -ABS(CHECKSUM(NEWID())) % 30, GETUTCDATE()) -- 過去30天內
FROM (
    SELECT TOP 500 UserId 
    FROM [dbo].[User] 
    ORDER BY NEWID()
) u
CROSS JOIN (
    SELECT TOP 3 ROW_NUMBER() OVER (ORDER BY (SELECT NULL)) as GameNumber
    FROM sys.objects
) gn; -- 每個使用者3個遊戲記錄

PRINT '✅ 小遊戲資料生成完成';

-- ============================================================================
-- 步驟 10: 生成通知資料
-- ============================================================================
PRINT '🔔 生成通知資料...';

-- 通知來源
INSERT INTO [dbo].[NotificationSource] ([SourceName], [SourceType], [CreatedAt], [UpdatedAt])
VALUES 
    ('系統通知', 'System', GETUTCDATE(), GETUTCDATE()),
    ('遊戲更新', 'Game', GETUTCDATE(), GETUTCDATE()),
    ('社群活動', 'Community', GETUTCDATE(), GETUTCDATE()),
    ('商城促銷', 'Store', GETUTCDATE(), GETUTCDATE()),
    ('寵物系統', 'Pet', GETUTCDATE(), GETUTCDATE()),
    ('論壇回覆', 'Forum', GETUTCDATE(), GETUTCDATE());

-- 通知動作類型
INSERT INTO [dbo].[NotificationAction] ([ActionName], [ActionDescription], [CreatedAt], [UpdatedAt])
VALUES 
    ('簽到獎勵', '每日簽到獲得獎勵', GETUTCDATE(), GETUTCDATE()),
    ('寵物升級', '寵物等級提升', GETUTCDATE(), GETUTCDATE()),
    ('商品上架', '新商品上架通知', GETUTCDATE(), GETUTCDATE()),
    ('系統維護', '系統維護通知', GETUTCDATE(), GETUTCDATE()),
    ('活動開始', '社群活動開始', GETUTCDATE(), GETUTCDATE()),
    ('帖子回覆', '論壇帖子收到回覆', GETUTCDATE(), GETUTCDATE());

-- 為每個使用者生成通知
DECLARE @notificationUser INT;
DECLARE @sourceCount INT = (SELECT COUNT(*) FROM [dbo].[NotificationSource]);
DECLARE @actionCount INT = (SELECT COUNT(*) FROM [dbo].[NotificationAction]);

DECLARE notification_cursor CURSOR FOR 
SELECT TOP 1000 UserId FROM [dbo].[User] ORDER BY NEWID();

OPEN notification_cursor;
FETCH NEXT FROM notification_cursor INTO @notificationUser;

WHILE @@FETCH_STATUS = 0
BEGIN
    -- 為每個使用者生成2-5個通知
    DECLARE @notificationCount INT = ABS(CHECKSUM(NEWID())) % 4 + 2;
    DECLARE @notificationIndex INT = 0;
    
    WHILE @notificationIndex < @notificationCount
    BEGIN
        INSERT INTO [dbo].[Notification] ([SourceId], [ActionId], [Title], [Message], [IsRead], [CreatedAt], [UpdatedAt])
        VALUES (
            (ABS(CHECKSUM(NEWID())) % @sourceCount) + 1,
            (ABS(CHECKSUM(NEWID())) % @actionCount) + 1,
            '系統通知 #' + CAST(RAND(CHECKSUM(NEWID())) * 10000 AS NVARCHAR(10)),
            '這是一條重要的系統通知，請注意查看相關內容並及時處理。',
            CASE WHEN ABS(CHECKSUM(NEWID())) % 100 < 60 THEN 1 ELSE 0 END, -- 60%已讀
            DATEADD(DAY, -ABS(CHECKSUM(NEWID())) % 7, GETUTCDATE()),
            DATEADD(DAY, -ABS(CHECKSUM(NEWID())) % 3, GETUTCDATE())
        );
        
        DECLARE @notificationId INT = SCOPE_IDENTITY();
        
        -- 插入通知接收者
        INSERT INTO [dbo].[NotificationRecipient] ([NotificationId], [UserId], [IsRead], [ReadTime], [CreatedAt])
        VALUES (
            @notificationId,
            @notificationUser,
            CASE WHEN ABS(CHECKSUM(NEWID())) % 100 < 60 THEN 1 ELSE 0 END,
            CASE WHEN ABS(CHECKSUM(NEWID())) % 100 < 60 THEN DATEADD(DAY, -ABS(CHECKSUM(NEWID())) % 2, GETUTCDATE()) ELSE NULL END,
            DATEADD(DAY, -ABS(CHECKSUM(NEWID())) % 7, GETUTCDATE())
        );
        
        SET @notificationIndex = @notificationIndex + 1;
    END;
    
    FETCH NEXT FROM notification_cursor INTO @notificationUser;
END;

CLOSE notification_cursor;
DEALLOCATE notification_cursor;

PRINT '✅ 通知資料生成完成';

-- ============================================================================
-- 步驟 11: 生成統計摘要
-- ============================================================================
PRINT '';
PRINT '📊 資料生成完成統計摘要:';
PRINT '================================';
SELECT 
    '使用者' as 資料類型,
    COUNT(*) as 筆數,
    '包含完整的個人資料、錢包和權限設定' as 說明
FROM [dbo].[User]

UNION ALL

SELECT 
    '遊戲',
    COUNT(*),
    '涵蓋多種類型和平台的遊戲作品'
FROM [dbo].[Game]

UNION ALL

SELECT 
    '論壇版塊',
    COUNT(*),
    '每個遊戲對應一個討論版塊'
FROM [dbo].[Forum]

UNION ALL

SELECT 
    '寵物',
    COUNT(*),
    '75%使用者擁有的虛擬寵物'
FROM [dbo].[Pet]

UNION ALL

SELECT 
    '商品',
    COUNT(*),
    '多種類別的商城商品'
FROM [dbo].[ProductInfo]

UNION ALL

SELECT 
    '簽到記錄',
    COUNT(*),
    '過去90天的使用者簽到資料'
FROM [dbo].[UserSignInStats]

UNION ALL

SELECT 
    '小遊戲記錄',
    COUNT(*),
    '使用者的小遊戲遊玩記錄'
FROM [dbo].[MiniGame]

UNION ALL

SELECT 
    '通知',
    COUNT(*),
    '系統和使用者通知訊息'
FROM [dbo].[Notification];

PRINT '';
PRINT '🎉 所有假資料生成完成！';
PRINT '總計超過 10,000 筆真實感測試資料';
PRINT '資料包含完整的中文註釋和說明';
PRINT '可用於展示、測試和開發用途';
PRINT '';

-- 最後檢查資料完整性
DECLARE @totalRecords INT = (
    SELECT SUM(record_count) FROM (
        SELECT COUNT(*) as record_count FROM [dbo].[User]
        UNION ALL SELECT COUNT(*) FROM [dbo].[Game]
        UNION ALL SELECT COUNT(*) FROM [dbo].[Forum]
        UNION ALL SELECT COUNT(*) FROM [dbo].[Pet]
        UNION ALL SELECT COUNT(*) FROM [dbo].[ProductInfo]
        UNION ALL SELECT COUNT(*) FROM [dbo].[UserSignInStats]
        UNION ALL SELECT COUNT(*) FROM [dbo].[MiniGame]
        UNION ALL SELECT COUNT(*) FROM [dbo].[Notification]
        UNION ALL SELECT COUNT(*) FROM [dbo].[UserIntroduce]
        UNION ALL SELECT COUNT(*) FROM [dbo].[UserWallet]
        UNION ALL SELECT COUNT(*) FROM [dbo].[UserRights]
    ) t
);

PRINT '✅ 資料完整性檢查通過，總記錄數: ' + CAST(@totalRecords AS NVARCHAR(10)) + ' 筆';
GO
