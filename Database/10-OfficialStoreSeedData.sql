-- =============================================
-- GameCore 官方商城系統種子資料
-- 建立完整的B2C電商測試資料，包含供應商、商品、訂單、排行榜等
-- 嚴格按照規格要求生成符合業務邏輯的完整電商資料
-- =============================================

USE GameCore;
GO

PRINT '開始插入官方商城系統種子資料...';

-- 清除現有的商城相關記錄 (按照外鍵順序)
PRINT '清除現有商城資料...';
DELETE FROM OrderItems;
DELETE FROM OrderInfo;
DELETE FROM Official_Store_Ranking;
DELETE FROM ProductInfoAuditLog;
DELETE FROM OtherProductDetails;
DELETE FROM GameProductDetails;
DELETE FROM ProductInfo;
DELETE FROM Supplier;

-- 重置自增ID
DBCC CHECKIDENT ('Supplier', RESEED, 0);
DBCC CHECKIDENT ('ProductInfo', RESEED, 0);
DBCC CHECKIDENT ('OrderInfo', RESEED, 0);
DBCC CHECKIDENT ('OrderItems', RESEED, 0);
DBCC CHECKIDENT ('Official_Store_Ranking', RESEED, 0);

PRINT '開始生成供應商資料...';

-- 插入供應商資料
INSERT INTO Supplier (supplier_name) VALUES 
('Steam遊戲平台'), ('Epic Games'), ('育碧娛樂'), ('EA遊戲'), ('暴雪娛樂'),
('任天堂'), ('索尼互動娛樂'), ('微軟遊戲工作室'), ('Valve Corporation'), ('2K Games'),
('Take-Two Interactive'), ('Square Enix'), ('卡普空'), ('萬代南夢宮'), ('世嘉'),
('3A電玩精品店'), ('遊戲周邊專賣'), ('電競裝備館'), ('遊戲收藏品'), ('數位娛樂城');

PRINT '供應商資料插入完成！';

PRINT '開始生成商品資料...';

-- 聲明變數
DECLARE @SupplierId INT;
DECLARE @ProductId INT;
DECLARE @ProductCount INT = 0;

-- 插入遊戲商品
PRINT '插入遊戲類商品...';

-- AAA 大作遊戲
INSERT INTO ProductInfo (product_name, product_type, price, currency_code, Shipment_Quantity, product_created_by, product_created_at, product_updated_by, product_updated_at, user_id)
VALUES 
('賽博龐克 2077', '遊戲', 1290.00, 'TWD', 100, '系統管理員', GETUTCDATE(), '系統管理員', GETUTCDATE(), 1),
('巫師3：狂獵 完整版', '遊戲', 890.00, 'TWD', 150, '系統管理員', GETUTCDATE(), '系統管理員', GETUTCDATE(), 1),
('看門狗：自由軍團', '遊戲', 1690.00, 'TWD', 80, '系統管理員', GETUTCDATE(), '系統管理員', GETUTCDATE(), 1),
('刺客教條：維京紀元', '遊戲', 1790.00, 'TWD', 120, '系統管理員', GETUTCDATE(), '系統管理員', GETUTCDATE(), 1),
('暗黑破壞神 IV', '遊戲', 2190.00, 'TWD', 200, '系統管理員', GETUTCDATE(), '系統管理員', GETUTCDATE(), 1),
('魔獸世界：巨龍崛起', '遊戲', 1590.00, 'TWD', 180, '系統管理員', GETUTCDATE(), '系統管理員', GETUTCDATE(), 1),
('薩爾達傳說：王國之淚', '遊戲', 1790.00, 'TWD', 90, '系統管理員', GETUTCDATE(), '系統管理員', GETUTCDATE(), 1),
('超級瑪利歐兄弟 驚奇', '遊戲', 1690.00, 'TWD', 110, '系統管理員', GETUTCDATE(), '系統管理員', GETUTCDATE(), 1),
('最後生還者 第二部', '遊戲', 1390.00, 'TWD', 85, '系統管理員', GETUTCDATE(), '系統管理員', GETUTCDATE(), 1),
('戰神：諸神黃昏', '遊戲', 1590.00, 'TWD', 95, '系統管理員', GETUTCDATE(), '系統管理員', GETUTCDATE(), 1),
('極限競速：地平線5', '遊戲', 1490.00, 'TWD', 75, '系統管理員', GETUTCDATE(), '系統管理員', GETUTCDATE(), 1),
('光環：無限', '遊戲', 1790.00, 'TWD', 88, '系統管理員', GETUTCDATE(), '系統管理員', GETUTCDATE(), 1),
('俠盜獵車手 V', '遊戲', 899.00, 'TWD', 300, '系統管理員', GETUTCDATE(), '系統管理員', GETUTCDATE(), 1),
('NBA 2K24', '遊戲', 1990.00, 'TWD', 60, '系統管理員', GETUTCDATE(), '系統管理員', GETUTCDATE(), 1),
('最終幻想 XVI', '遊戲', 1890.00, 'TWD', 70, '系統管理員', GETUTCDATE(), '系統管理員', GETUTCDATE(), 1),
('惡靈古堡 4 重製版', '遊戲', 1790.00, 'TWD', 100, '系統管理員', GETUTCDATE(), '系統管理員', GETUTCDATE(), 1),
('快打旋風 6', '遊戲', 1990.00, 'TWD', 90, '系統管理員', GETUTCDATE(), '系統管理員', GETUTCDATE(), 1),
('鐵拳 8', '遊戲', 1990.00, 'TWD', 80, '系統管理員', GETUTCDATE(), '系統管理員', GETUTCDATE(), 1),
('音速小子 未知邊境', '遊戲', 1490.00, 'TWD', 65, '系統管理員', GETUTCDATE(), '系統管理員', GETUTCDATE(), 1),
('真人快打 1', '遊戲', 1890.00, 'TWD', 75, '系統管理員', GETUTCDATE(), '系統管理員', GETUTCDATE(), 1);

-- 獨立遊戲
INSERT INTO ProductInfo (product_name, product_type, price, currency_code, Shipment_Quantity, product_created_by, product_created_at, product_updated_by, product_updated_at, user_id)
VALUES 
('空洞騎士', '遊戲', 468.00, 'TWD', 200, '系統管理員', GETUTCDATE(), '系統管理員', GETUTCDATE(), 1),
('死亡細胞', '遊戲', 590.00, 'TWD', 150, '系統管理員', GETUTCDATE(), '系統管理員', GETUTCDATE(), 1),
('哈帝斯', '遊戲', 590.00, 'TWD', 180, '系統管理員', GETUTCDATE(), '系統管理員', GETUTCDATE(), 1),
('星露谷物語', '遊戲', 398.00, 'TWD', 250, '系統管理員', GETUTCDATE(), '系統管理員', GETUTCDATE(), 1),
('泰拉瑞亞', '遊戲', 268.00, 'TWD', 300, '系統管理員', GETUTCDATE(), '系統管理員', GETUTCDATE(), 1),
('Among Us', '遊戲', 102.00, 'TWD', 500, '系統管理員', GETUTCDATE(), '系統管理員', GETUTCDATE(), 1),
('Fall Guys', '遊戲', 0.00, 'TWD', 1000, '系統管理員', GETUTCDATE(), '系統管理員', GETUTCDATE(), 1),
('糖豆人：終極淘汰賽', '遊戲', 590.00, 'TWD', 200, '系統管理員', GETUTCDATE(), '系統管理員', GETUTCDATE(), 1),
('咖啡談話', '遊戲', 328.00, 'TWD', 100, '系統管理員', GETUTCDATE(), '系統管理員', GETUTCDATE(), 1),
('奧日與黑暗森林', '遊戲', 590.00, 'TWD', 120, '系統管理員', GETUTCDATE(), '系統管理員', GETUTCDATE(), 1);

-- 周邊商品
INSERT INTO ProductInfo (product_name, product_type, price, currency_code, Shipment_Quantity, product_created_by, product_created_at, product_updated_by, product_updated_at, user_id)
VALUES 
('羅技 G Pro X 遊戲耳機', '周邊', 4990.00, 'TWD', 50, '系統管理員', GETUTCDATE(), '系統管理員', GETUTCDATE(), 1),
('Razer DeathAdder V3 滑鼠', '周邊', 2490.00, 'TWD', 80, '系統管理員', GETUTCDATE(), '系統管理員', GETUTCDATE(), 1),
('SteelSeries Apex Pro 機械鍵盤', '周邊', 6990.00, 'TWD', 30, '系統管理員', GETUTCDATE(), '系統管理員', GETUTCDATE(), 1),
('HyperX Cloud Alpha 耳機', '周邊', 3290.00, 'TWD', 60, '系統管理員', GETUTCDATE(), '系統管理員', GETUTCDATE(), 1),
('ROG Swift PG279Q 顯示器', '周邊', 18990.00, 'TWD', 15, '系統管理員', GETUTCDATE(), '系統管理員', GETUTCDATE(), 1),
('Xbox 無線控制器', '周邊', 1790.00, 'TWD', 100, '系統管理員', GETUTCDATE(), '系統管理員', GETUTCDATE(), 1),
('PlayStation 5 DualSense 控制器', '周邊', 2190.00, 'TWD', 90, '系統管理員', GETUTCDATE(), '系統管理員', GETUTCDATE(), 1),
('Nintendo Switch Pro 控制器', '周邊', 2090.00, 'TWD', 70, '系統管理員', GETUTCDATE(), '系統管理員', GETUTCDATE(), 1),
('Corsair K95 RGB 機械鍵盤', '周邊', 5990.00, 'TWD', 25, '系統管理員', GETUTCDATE(), '系統管理員', GETUTCDATE(), 1),
('Logitech G29 賽車方向盤', '周邊', 8990.00, 'TWD', 20, '系統管理員', GETUTCDATE(), '系統管理員', GETUTCDATE(), 1);

-- 遊戲點數卡
INSERT INTO ProductInfo (product_name, product_type, price, currency_code, Shipment_Quantity, product_created_by, product_created_at, product_updated_by, product_updated_at, user_id)
VALUES 
('Steam 錢包 500 元', '點數卡', 500.00, 'TWD', 200, '系統管理員', GETUTCDATE(), '系統管理員', GETUTCDATE(), 1),
('Steam 錢包 1000 元', '點數卡', 1000.00, 'TWD', 150, '系統管理員', GETUTCDATE(), '系統管理員', GETUTCDATE(), 1),
('PlayStation Store 1000 元', '點數卡', 1000.00, 'TWD', 100, '系統管理員', GETUTCDATE(), '系統管理員', GETUTCDATE(), 1),
('Xbox Gift Card 1000 元', '點數卡', 1000.00, 'TWD', 120, '系統管理員', GETUTCDATE(), '系統管理員', GETUTCDATE(), 1),
('Nintendo eShop 1000 元', '點數卡', 1000.00, 'TWD', 110, '系統管理員', GETUTCDATE(), '系統管理員', GETUTCDATE(), 1),
('Epic Games 錢包 500 元', '點數卡', 500.00, 'TWD', 80, '系統管理員', GETUTCDATE(), '系統管理員', GETUTCDATE(), 1),
('暴雪戰網點數 500 元', '點數卡', 500.00, 'TWD', 90, '系統管理員', GETUTCDATE(), '系統管理員', GETUTCDATE(), 1),
('Riot Points 1000 點', '點數卡', 1000.00, 'TWD', 200, '系統管理員', GETUTCDATE(), '系統管理員', GETUTCDATE(), 1),
('Garena 貝殼幣 1000 元', '點數卡', 1000.00, 'TWD', 150, '系統管理員', GETUTCDATE(), '系統管理員', GETUTCDATE(), 1),
('Google Play 禮品卡 1000 元', '點數卡', 1000.00, 'TWD', 180, '系統管理員', GETUTCDATE(), '系統管理員', GETUTCDATE(), 1);

-- 收藏品
INSERT INTO ProductInfo (product_name, product_type, price, currency_code, Shipment_Quantity, product_created_by, product_created_at, product_updated_by, product_updated_at, user_id)
VALUES 
('薩爾達傳說 林克 figma 可動人偶', '收藏品', 2890.00, 'TWD', 30, '系統管理員', GETUTCDATE(), '系統管理員', GETUTCDATE(), 1),
('Final Fantasy VII 克勞德 雕像', '收藏品', 12990.00, 'TWD', 10, '系統管理員', GETUTCDATE(), '系統管理員', GETUTCDATE(), 1),
('Overwatch 獵空 雕像', '收藏品', 8990.00, 'TWD', 15, '系統管理員', GETUTCDATE(), '系統管理員', GETUTCDATE(), 1),
('Pokemon 皮卡丘 毛絨玩偶', '收藏品', 990.00, 'TWD', 100, '系統管理員', GETUTCDATE(), '系統管理員', GETUTCDATE(), 1),
('魔獸世界 阿薩斯 雕像', '收藏品', 15990.00, 'TWD', 8, '系統管理員', GETUTCDATE(), '系統管理員', GETUTCDATE(), 1),
('英雄聯盟 亞索 模型', '收藏品', 3990.00, 'TWD', 25, '系統管理員', GETUTCDATE(), '系統管理員', GETUTCDATE(), 1),
('Cyberpunk 2077 V 雕像', '收藏品', 11990.00, 'TWD', 12, '系統管理員', GETUTCDATE(), '系統管理員', GETUTCDATE(), 1),
('Minecraft 終界龍 模型', '收藏品', 2490.00, 'TWD', 40, '系統管理員', GETUTCDATE(), '系統管理員', GETUTCDATE', 1),
('哈利波特 魔法杖 複製品', '收藏品', 1990.00, 'TWD', 50, '系統管理員', GETUTCDATE(), '系統管理員', GETUTCDATE(), 1),
('星際大戰 光劍 複製品', '收藏品', 8990.00, 'TWD', 20, '系統管理員', GETUTCDATE(), '系統管理員', GETUTCDATE(), 1);

PRINT '商品基本資料插入完成！';

-- 為遊戲商品插入 GameProductDetails
PRINT '插入遊戲商品詳細資料...';

INSERT INTO GameProductDetails (product_id, product_name, product_description, supplier_id, platform_id, game_id, download_link)
SELECT 
    p.product_id,
    p.product_name,
    CASE 
        WHEN p.product_name LIKE '%賽博龐克%' THEN '在夜之城這個科技與身體改造掛帥的未來世界，扮演一名雇傭兵V，追求一個獨一無二、能夠帶來永生的植入物。'
        WHEN p.product_name LIKE '%巫師%' THEN '在這個龐大的開放世界中扮演傭兵獵魔士傑洛特，展開尋找你的養女希里的任務。'
        WHEN p.product_name LIKE '%看門狗%' THEN '在近未來的倫敦，加入抵抗運動並招募任何能看到的人來反擊。'
        WHEN p.product_name LIKE '%刺客%' THEN '化身為維京戰士，在野蠻而美麗的英格蘭世界中開拓新家園。'
        WHEN p.product_name LIKE '%暗黑%' THEN '重回聖休亞瑞，在這個永恆衝突的世界中對抗古老邪惡的力量。'
        WHEN p.product_name LIKE '%魔獸%' THEN '踏上巨龍群島，發現古老的秘密並找回龍族失落的遺產。'
        WHEN p.product_name LIKE '%薩爾達%' THEN '使用琳克的新能力在天空、地面和地底探索海拉魯廣大的土地。'
        WHEN p.product_name LIKE '%瑪利歐%' THEN '與瑪利歐和朋友們一同體驗前所未見的全新2D瑪利歐冒險。'
        WHEN p.product_name LIKE '%最後生還者%' THEN '在這個殘酷的後末世界中，追隨艾莉的史詩復仇之旅。'
        WHEN p.product_name LIKE '%戰神%' THEN '奎托斯和阿特柔斯踏上九界的危險旅程，尋找戰爭的答案。'
        ELSE '精彩的遊戲體驗等著您！'
    END,
    (ABS(CHECKSUM(NEWID())) % 10) + 1, -- 隨機供應商ID 1-10
    (ABS(CHECKSUM(NEWID())) % 5) + 1,  -- 隨機平台ID 1-5 (PC, PlayStation, Xbox, Nintendo, Mobile)
    p.product_id, -- 使用product_id作為game_id
    CASE 
        WHEN p.product_name LIKE '%Steam%' OR p.product_type = '遊戲' THEN 'https://store.steampowered.com/download/' + CAST(p.product_id AS VARCHAR)
        ELSE NULL
    END
FROM ProductInfo p 
WHERE p.product_type = '遊戲';

-- 為非遊戲商品插入 OtherProductDetails
PRINT '插入其他商品詳細資料...';

INSERT INTO OtherProductDetails (product_id, product_name, product_description, supplier_id, platform_id, digital_code, size, color, material, stock_quantity)
SELECT 
    p.product_id,
    p.product_name,
    CASE 
        WHEN p.product_type = '周邊' THEN 
            CASE 
                WHEN p.product_name LIKE '%耳機%' THEN '專業電競耳機，提供沉浸式音效體驗，搭載降噪技術和舒適耳墊。'
                WHEN p.product_name LIKE '%滑鼠%' THEN '高精度遊戲滑鼠，配備可程式按鍵和RGB背光，適合各種遊戲類型。'
                WHEN p.product_name LIKE '%鍵盤%' THEN '機械式電競鍵盤，採用高品質軸體，提供優越的觸感和耐用性。'
                WHEN p.product_name LIKE '%顯示器%' THEN '高解析度電競顯示器，支援高更新率和低延遲，提供流暢遊戲體驗。'
                WHEN p.product_name LIKE '%控制器%' THEN '無線遊戲控制器，精準操控和長久電池續航力，完美遊戲體驗。'
                ELSE '優質遊戲周邊產品'
            END
        WHEN p.product_type = '點數卡' THEN '數位遊戲平台點數卡，可用於購買遊戲、DLC和其他數位內容。'
        WHEN p.product_type = '收藏品' THEN '限量版遊戲收藏品，精工製作，適合收藏家和粉絲。'
        ELSE '精品商品，品質保證。'
    END,
    (ABS(CHECKSUM(NEWID())) % 10) + 11, -- 隨機供應商ID 11-20
    CASE 
        WHEN p.product_type = '周邊' THEN (ABS(CHECKSUM(NEWID())) % 3) + 6 -- 平台ID 6-8 (PC周邊, 主機周邊, 通用)
        ELSE (ABS(CHECKSUM(NEWID())) % 5) + 1
    END,
    CASE 
        WHEN p.product_type = '點數卡' THEN 'DGC-' + RIGHT('0000' + CAST(p.product_id AS VARCHAR), 4) + '-' + RIGHT('0000' + CAST(ABS(CHECKSUM(NEWID())) % 10000 AS VARCHAR), 4)
        ELSE NULL
    END,
    CASE 
        WHEN p.product_name LIKE '%耳機%' THEN '標準尺寸'
        WHEN p.product_name LIKE '%滑鼠%' THEN '120 x 65 x 40 mm'
        WHEN p.product_name LIKE '%鍵盤%' THEN '440 x 135 x 36 mm'
        WHEN p.product_name LIKE '%顯示器%' THEN '27吋'
        WHEN p.product_name LIKE '%控制器%' THEN '標準尺寸'
        WHEN p.product_type = '收藏品' AND p.product_name LIKE '%雕像%' THEN '高度 25-30cm'
        WHEN p.product_type = '收藏品' AND p.product_name LIKE '%毛絨%' THEN '25cm'
        WHEN p.product_type = '收藏品' AND p.product_name LIKE '%模型%' THEN '1/8比例'
        ELSE NULL
    END,
    CASE 
        WHEN p.product_name LIKE '%RGB%' OR p.product_name LIKE '%G Pro%' THEN '黑色/RGB'
        WHEN p.product_name LIKE '%DeathAdder%' THEN '黑色'
        WHEN p.product_name LIKE '%Apex%' THEN '黑色'
        WHEN p.product_name LIKE '%Cloud Alpha%' THEN '黑紅色'
        WHEN p.product_name LIKE '%Swift%' THEN '黑色'
        WHEN p.product_name LIKE '%Xbox%' THEN '白色'
        WHEN p.product_name LIKE '%PlayStation%' THEN '白色'
        WHEN p.product_name LIKE '%Nintendo%' THEN '黑色'
        WHEN p.product_name LIKE '%皮卡丘%' THEN '黃色'
        WHEN p.product_type = '收藏品' THEN '多色'
        ELSE '黑色'
    END,
    CASE 
        WHEN p.product_name LIKE '%耳機%' THEN '塑膠+金屬+皮革'
        WHEN p.product_name LIKE '%滑鼠%' THEN '塑膠+金屬'
        WHEN p.product_name LIKE '%鍵盤%' THEN '鋁合金+塑膠'
        WHEN p.product_name LIKE '%顯示器%' THEN 'IPS面板+金屬支架'
        WHEN p.product_name LIKE '%控制器%' THEN '塑膠+金屬'
        WHEN p.product_type = '收藏品' AND p.product_name LIKE '%雕像%' THEN '樹脂'
        WHEN p.product_type = '收藏品' AND p.product_name LIKE '%毛絨%' THEN '絨毛'
        WHEN p.product_type = '收藏品' AND p.product_name LIKE '%模型%' THEN 'PVC'
        WHEN p.product_type = '收藏品' AND p.product_name LIKE '%魔法杖%' THEN '樹脂+金屬'
        WHEN p.product_type = '收藏品' AND p.product_name LIKE '%光劍%' THEN '金屬+電子零件'
        ELSE NULL
    END,
    CAST(p.Shipment_Quantity AS NVARCHAR)
FROM ProductInfo p 
WHERE p.product_type != '遊戲';

PRINT '商品詳細資料插入完成！';

-- 生成訂單資料
PRINT '開始生成訂單資料...';

DECLARE @UserId INT = 1;
DECLARE @MaxUserId INT = (SELECT MIN(30, MAX(User_ID)) FROM Users); -- 限制在前30名使用者
DECLARE @OrderCount INT = 0;

WHILE @UserId <= @MaxUserId
BEGIN
    -- 檢查使用者是否有購物權限
    DECLARE @HasShoppingPermission BIT = (SELECT ISNULL(ShoppingPermission, 1) FROM User_Rights WHERE User_Id = @UserId);
    
    IF @HasShoppingPermission = 1
    BEGIN
        -- 每個使用者隨機生成 0-5 個訂單
        DECLARE @UserOrderCount INT = ABS(CHECKSUM(NEWID())) % 6;
        DECLARE @OrderIndex INT = 0;
        
        WHILE @OrderIndex < @UserOrderCount
        BEGIN
            -- 隨機選擇訂單日期 (最近60天內)
            DECLARE @OrderDate DATETIME2 = DATEADD(DAY, -(ABS(CHECKSUM(NEWID())) % 60), GETUTCDATE());
            
            -- 隨機決定訂單狀態和付款狀態
            DECLARE @OrderStatus NVARCHAR(50);
            DECLARE @PaymentStatus NVARCHAR(50);
            DECLARE @PaymentAt DATETIME2 = NULL;
            DECLARE @ShippedAt DATETIME2 = NULL;
            DECLARE @CompletedAt DATETIME2 = NULL;
            
            DECLARE @StatusRand INT = ABS(CHECKSUM(NEWID())) % 100;
            
            IF @StatusRand < 70 -- 70% 已完成訂單
            BEGIN
                SET @OrderStatus = 'Completed';
                SET @PaymentStatus = 'Paid';
                SET @PaymentAt = DATEADD(MINUTE, 30, @OrderDate);
                SET @ShippedAt = DATEADD(DAY, 1, @PaymentAt);
                SET @CompletedAt = DATEADD(DAY, 3, @ShippedAt);
            END
            ELSE IF @StatusRand < 85 -- 15% 已出貨訂單
            BEGIN
                SET @OrderStatus = 'Shipped';
                SET @PaymentStatus = 'Paid';
                SET @PaymentAt = DATEADD(MINUTE, 30, @OrderDate);
                SET @ShippedAt = DATEADD(DAY, 1, @PaymentAt);
            END
            ELSE IF @StatusRand < 95 -- 10% 待出貨訂單
            BEGIN
                SET @OrderStatus = 'ToShip';
                SET @PaymentStatus = 'Paid';
                SET @PaymentAt = DATEADD(MINUTE, 30, @OrderDate);
            END
            ELSE -- 5% 待付款訂單
            BEGIN
                SET @OrderStatus = 'Created';
                SET @PaymentStatus = 'Pending';
            END
            
            -- 插入訂單主資料
            INSERT INTO OrderInfo (user_id, order_date, order_status, payment_status, order_total, payment_at, shipped_at, completed_at)
            VALUES (@UserId, @OrderDate, @OrderStatus, @PaymentStatus, 0, @PaymentAt, @ShippedAt, @CompletedAt);
            
            SET @OrderId = SCOPE_IDENTITY();
            
            -- 為訂單添加 1-4 個商品項目
            DECLARE @ItemCount INT = (ABS(CHECKSUM(NEWID())) % 4) + 1;
            DECLARE @ItemIndex INT = 0;
            DECLARE @OrderTotal DECIMAL(10,2) = 0;
            
            WHILE @ItemIndex < @ItemCount
            BEGIN
                -- 隨機選擇商品
                DECLARE @RandomProductId INT = (SELECT TOP 1 product_id FROM ProductInfo ORDER BY NEWID());
                DECLARE @UnitPrice DECIMAL(10,2) = (SELECT price FROM ProductInfo WHERE product_id = @RandomProductId);
                DECLARE @Quantity INT = (ABS(CHECKSUM(NEWID())) % 3) + 1; -- 1-3個
                DECLARE @Subtotal DECIMAL(10,2) = @UnitPrice * @Quantity;
                
                -- 插入訂單項目
                INSERT INTO OrderItems (order_id, product_id, line_no, unit_price, quantity, subtotal)
                VALUES (@OrderId, @RandomProductId, @ItemIndex + 1, @UnitPrice, @Quantity, @Subtotal);
                
                SET @OrderTotal = @OrderTotal + @Subtotal;
                SET @ItemIndex = @ItemIndex + 1;
            END
            
            -- 更新訂單總額
            UPDATE OrderInfo SET order_total = @OrderTotal WHERE order_id = @OrderId;
            
            SET @OrderCount = @OrderCount + 1;
            SET @OrderIndex = @OrderIndex + 1;
        END
    END
    
    SET @UserId = @UserId + 1;
END

PRINT '訂單資料生成完成！總計: ' + CAST(@OrderCount AS VARCHAR) + ' 個訂單';

-- 生成商城排行榜資料
PRINT '開始生成商城排行榜資料...';

-- 生成最近30天的日榜資料
DECLARE @RankingDate DATE = CAST(DATEADD(DAY, -30, GETDATE()) AS DATE);
DECLARE @EndDate DATE = CAST(GETDATE() AS DATE);

WHILE @RankingDate <= @EndDate
BEGIN
    -- 計算當日各商品的交易額和交易量
    WITH DailyStats AS (
        SELECT 
            oi.product_id,
            SUM(od.subtotal) as trading_amount,
            SUM(od.quantity) as trading_volume,
            ROW_NUMBER() OVER (ORDER BY SUM(od.subtotal) DESC) as amount_rank,
            ROW_NUMBER() OVER (ORDER BY SUM(od.quantity) DESC) as volume_rank
        FROM OrderInfo oi
        JOIN OrderItems od ON oi.order_id = od.order_id
        WHERE CAST(oi.order_date AS DATE) = @RankingDate
        AND oi.order_status = 'Completed'
        GROUP BY oi.product_id
    )
    INSERT INTO Official_Store_Ranking (period_type, ranking_date, product_ID, ranking_metric, ranking_position, trading_amount)
    SELECT 
        'daily' as period_type,
        @RankingDate as ranking_date,
        product_id,
        'trading_amount' as ranking_metric,
        amount_rank as ranking_position,
        trading_amount
    FROM DailyStats
    WHERE amount_rank <= 50 -- 只保留前50名
    
    UNION ALL
    
    SELECT 
        'daily' as period_type,
        @RankingDate as ranking_date,
        product_id,
        'trading_volume' as ranking_metric,
        volume_rank as ranking_position,
        trading_volume
    FROM DailyStats
    WHERE volume_rank <= 50; -- 只保留前50名
    
    SET @RankingDate = DATEADD(DAY, 1, @RankingDate);
END

-- 生成月榜資料 (最近3個月)
DECLARE @MonthStart DATE = CAST(DATEADD(MONTH, -3, GETDATE()) AS DATE);
DECLARE @MonthEnd DATE = CAST(GETDATE() AS DATE);

WITH MonthlyStats AS (
    SELECT 
        YEAR(oi.order_date) as year_val,
        MONTH(oi.order_date) as month_val,
        oi.product_id,
        SUM(od.subtotal) as trading_amount,
        SUM(od.quantity) as trading_volume,
        ROW_NUMBER() OVER (PARTITION BY YEAR(oi.order_date), MONTH(oi.order_date) ORDER BY SUM(od.subtotal) DESC) as amount_rank,
        ROW_NUMBER() OVER (PARTITION BY YEAR(oi.order_date), MONTH(oi.order_date) ORDER BY SUM(od.quantity) DESC) as volume_rank
    FROM OrderInfo oi
    JOIN OrderItems od ON oi.order_id = od.order_id
    WHERE oi.order_date >= @MonthStart AND oi.order_date <= @MonthEnd
    AND oi.order_status = 'Completed'
    GROUP BY YEAR(oi.order_date), MONTH(oi.order_date), oi.product_id
)
INSERT INTO Official_Store_Ranking (period_type, ranking_date, product_ID, ranking_metric, ranking_position, trading_amount)
SELECT 
    'monthly' as period_type,
    CAST(CONCAT(year_val, '-', FORMAT(month_val, '00'), '-01') AS DATE) as ranking_date,
    product_id,
    'trading_amount' as ranking_metric,
    amount_rank as ranking_position,
    trading_amount
FROM MonthlyStats
WHERE amount_rank <= 50

UNION ALL

SELECT 
    'monthly' as period_type,
    CAST(CONCAT(year_val, '-', FORMAT(month_val, '00'), '-01') AS DATE) as ranking_date,
    product_id,
    'trading_volume' as ranking_metric,
    volume_rank as ranking_position,
    trading_volume
FROM MonthlyStats
WHERE volume_rank <= 50;

PRINT '商城排行榜資料生成完成！';

-- 統計報告
PRINT '=== 官方商城系統種子資料統計報告 ===';

-- 總商品數量
DECLARE @TotalProducts INT = (SELECT COUNT(*) FROM ProductInfo);
PRINT '總商品數量: ' + CAST(@TotalProducts AS VARCHAR);

-- 商品分類分布
PRINT '商品分類分布:';
SELECT 
    product_type as 商品分類,
    COUNT(*) as 商品數量,
    CAST(COUNT(*) * 100.0 / @TotalProducts AS DECIMAL(5,2)) as 佔比
FROM ProductInfo
GROUP BY product_type
ORDER BY 商品數量 DESC;

-- 庫存統計
PRINT '庫存統計:';
SELECT 
    COUNT(*) as 總商品數,
    SUM(Shipment_Quantity) as 總庫存量,
    AVG(Shipment_Quantity) as 平均庫存,
    MIN(Shipment_Quantity) as 最低庫存,
    MAX(Shipment_Quantity) as 最高庫存
FROM ProductInfo;

-- 價格統計
PRINT '價格統計:';
SELECT 
    product_type as 商品分類,
    COUNT(*) as 商品數量,
    CAST(AVG(price) AS DECIMAL(10,2)) as 平均價格,
    CAST(MIN(price) AS DECIMAL(10,2)) as 最低價格,
    CAST(MAX(price) AS DECIMAL(10,2)) as 最高價格
FROM ProductInfo
GROUP BY product_type
ORDER BY 平均價格 DESC;

-- 訂單統計
PRINT '訂單統計:';
SELECT 
    COUNT(*) as 總訂單數,
    COUNT(CASE WHEN order_status = 'Completed' THEN 1 END) as 已完成訂單,
    COUNT(CASE WHEN order_status = 'Shipped' THEN 1 END) as 已出貨訂單,
    COUNT(CASE WHEN order_status = 'ToShip' THEN 1 END) as 待出貨訂單,
    COUNT(CASE WHEN order_status = 'Created' THEN 1 END) as 待付款訂單,
    CAST(AVG(order_total) AS DECIMAL(10,2)) as 平均訂單金額,
    CAST(SUM(order_total) AS DECIMAL(15,2)) as 總交易額
FROM OrderInfo;

-- 訂單狀態分布
PRINT '訂單狀態分布:';
SELECT 
    order_status as 訂單狀態,
    COUNT(*) as 訂單數量,
    CAST(COUNT(*) * 100.0 / (SELECT COUNT(*) FROM OrderInfo) AS DECIMAL(5,2)) as 佔比,
    CAST(AVG(order_total) AS DECIMAL(10,2)) as 平均金額
FROM OrderInfo
GROUP BY order_status
ORDER BY 訂單數量 DESC;

-- 熱門商品 TOP 10
PRINT '最熱門商品 TOP 10 (按交易額):';
SELECT TOP 10
    p.product_name as 商品名稱,
    p.product_type as 商品分類,
    COUNT(od.item_id) as 銷售次數,
    SUM(od.quantity) as 銷售數量,
    CAST(SUM(od.subtotal) AS DECIMAL(12,2)) as 總交易額,
    CAST(AVG(od.unit_price) AS DECIMAL(10,2)) as 平均售價
FROM ProductInfo p
LEFT JOIN OrderItems od ON p.product_id = od.product_id
LEFT JOIN OrderInfo oi ON od.order_id = oi.order_id AND oi.order_status = 'Completed'
GROUP BY p.product_id, p.product_name, p.product_type
ORDER BY 總交易額 DESC;

-- 使用者購買統計
PRINT '使用者購買統計:';
SELECT 
    COUNT(DISTINCT user_id) as 購買使用者數,
    COUNT(*) as 總訂單數,
    CAST(AVG(order_count) AS DECIMAL(5,2)) as 平均每人訂單數,
    CAST(AVG(total_spent) AS DECIMAL(10,2)) as 平均每人消費額
FROM (
    SELECT 
        user_id,
        COUNT(*) as order_count,
        SUM(order_total) as total_spent
    FROM OrderInfo
    WHERE order_status = 'Completed'
    GROUP BY user_id
) user_stats;

-- 供應商統計
PRINT '供應商統計:';
SELECT 
    s.supplier_name as 供應商名稱,
    COUNT(DISTINCT p.product_id) as 商品數量,
    COALESCE(SUM(od.quantity), 0) as 總銷售量,
    CAST(COALESCE(SUM(od.subtotal), 0) AS DECIMAL(12,2)) as 總交易額
FROM Supplier s
LEFT JOIN GameProductDetails gpd ON s.supplier_id = gpd.supplier_id
LEFT JOIN ProductInfo p ON gpd.product_id = p.product_id
LEFT JOIN OrderItems od ON p.product_id = od.product_id
LEFT JOIN OrderInfo oi ON od.order_id = oi.order_id AND oi.order_status = 'Completed'
GROUP BY s.supplier_id, s.supplier_name
ORDER BY 總交易額 DESC;

-- 排行榜統計
PRINT '排行榜統計:';
SELECT 
    period_type as 榜單類型,
    ranking_metric as 排名指標,
    COUNT(*) as 榜單記錄數,
    COUNT(DISTINCT product_ID) as 上榜商品數,
    COUNT(DISTINCT ranking_date) as 榜單天數
FROM Official_Store_Ranking
GROUP BY period_type, ranking_metric
ORDER BY 榜單類型, 排名指標;

-- 最近7天銷售趨勢
PRINT '最近7天銷售趨勢:';
SELECT 
    CAST(order_date AS DATE) as 日期,
    COUNT(*) as 訂單數,
    COUNT(CASE WHEN order_status = 'Completed' THEN 1 END) as 完成訂單數,
    CAST(SUM(CASE WHEN order_status = 'Completed' THEN order_total ELSE 0 END) AS DECIMAL(12,2)) as 當日交易額
FROM OrderInfo
WHERE order_date >= DATEADD(DAY, -7, GETDATE())
GROUP BY CAST(order_date AS DATE)
ORDER BY 日期 DESC;

PRINT '官方商城系統種子資料插入完成！';
GO