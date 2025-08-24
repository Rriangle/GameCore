-- =============================================
-- GameCore 自由市場系統種子資料
-- 建立完整的C2C交易測試資料，包含商品上架、訂單、交易頁面、即時訊息等
-- 嚴格按照規格要求生成符合業務邏輯的完整C2C交易資料
-- =============================================

USE GameCore;
GO

PRINT '開始插入自由市場系統種子資料...';

-- 清除現有的自由市場相關記錄 (按照外鍵順序)
PRINT '清除現有自由市場資料...';
DELETE FROM PlayerMarketTradeMsg;
DELETE FROM PlayerMarketOrderTradepage;
DELETE FROM PlayerMarketOrderInfo;
DELETE FROM PlayerMarketRanking;
DELETE FROM PlayerMarketProductImgs;
DELETE FROM PlayerMarketProductInfo;

-- 重置自增ID
DBCC CHECKIDENT ('PlayerMarketProductInfo', RESEED, 0);
DBCC CHECKIDENT ('PlayerMarketOrderInfo', RESEED, 0);
DBCC CHECKIDENT ('PlayerMarketOrderTradepage', RESEED, 0);
DBCC CHECKIDENT ('PlayerMarketTradeMsg', RESEED, 0);
DBCC CHECKIDENT ('PlayerMarketRanking', RESEED, 0);

PRINT '開始生成自由市場商品資料...';

-- 為有銷售權限的使用者建立市場商品
DECLARE @SellerId INT = 1;
DECLARE @MaxSellerId INT = (SELECT MIN(20, MAX(User_ID)) FROM Users WHERE User_ID IN (SELECT User_Id FROM User_Rights WHERE SalesAuthority = 1));
DECLARE @ProductCount INT = 0;

-- 確保有些使用者擁有銷售權限
UPDATE User_Rights SET SalesAuthority = 1 WHERE User_Id <= 20;

PRINT '為銷售用戶生成商品...';

WHILE @SellerId <= @MaxSellerId
BEGIN
    PRINT '為賣家 ' + CAST(@SellerId AS VARCHAR) + ' 建立商品...';
    
    -- 每個賣家隨機上架 2-8 個商品
    DECLARE @SellerProductCount INT = 2 + (ABS(CHECKSUM(NEWID())) % 7); -- 2-8個商品
    DECLARE @ProductIndex INT = 0;
    
    WHILE @ProductIndex < @SellerProductCount
    BEGIN
        -- 隨機選擇商品類型
        DECLARE @ProductType NVARCHAR(100);
        DECLARE @TypeRand INT = ABS(CHECKSUM(NEWID())) % 6;
        
        SET @ProductType = CASE @TypeRand
            WHEN 0 THEN '遊戲道具'
            WHEN 1 THEN '遊戲帳號'
            WHEN 2 THEN '虛擬貨幣'
            WHEN 3 THEN '稀有裝備'
            WHEN 4 THEN '限定商品'
            ELSE '收藏品'
        END;
        
        -- 生成商品名稱
        DECLARE @ProductName NVARCHAR(200);
        DECLARE @ProductTitle NVARCHAR(200);
        
        SET @ProductName = CASE @ProductType
            WHEN '遊戲道具' THEN 
                CASE (ABS(CHECKSUM(NEWID())) % 5)
                    WHEN 0 THEN '史詩級武器 +15'
                    WHEN 1 THEN '傳說防具套裝'
                    WHEN 2 THEN '稀有寵物'
                    WHEN 3 THEN '強化石包'
                    ELSE '經驗藥水包'
                END
            WHEN '遊戲帳號' THEN 
                CASE (ABS(CHECKSUM(NEWID())) % 4)
                    WHEN 0 THEN '滿級角色帳號'
                    WHEN 1 THEN '稀有職業帳號'
                    WHEN 2 THEN '公會會長帳號'
                    ELSE '競技場高分帳號'
                END
            WHEN '虛擬貨幣' THEN 
                CASE (ABS(CHECKSUM(NEWID())) % 3)
                    WHEN 0 THEN '遊戲金幣 100萬'
                    WHEN 1 THEN '鑽石包 1000顆'
                    ELSE '點數卡餘額'
                END
            WHEN '稀有裝備' THEN 
                CASE (ABS(CHECKSUM(NEWID())) % 4)
                    WHEN 0 THEN '神器級武器'
                    WHEN 1 THEN '絕版坐騎'
                    WHEN 2 THEN '限定時裝'
                    ELSE '稀有翅膀'
                END
            WHEN '限定商品' THEN 
                CASE (ABS(CHECKSUM(NEWID())) % 3)
                    WHEN 0 THEN '絕版稱號'
                    WHEN 1 THEN '活動限定道具'
                    ELSE '紀念品收藏'
                END
            ELSE 
                CASE (ABS(CHECKSUM(NEWID())) % 4)
                    WHEN 0 THEN '遊戲週邊商品'
                    WHEN 1 THEN '限量手辦'
                    WHEN 2 THEN '簽名卡片'
                    ELSE '特典商品'
                END
        END;
        
        -- 生成吸引人的商品標題
        SET @ProductTitle = CASE 
            WHEN @ProductType = '遊戲道具' THEN '【超值包】' + @ProductName + ' 限時優惠！'
            WHEN @ProductType = '遊戲帳號' THEN '【頂級帳號】' + @ProductName + ' 直接開玩！'
            WHEN @ProductType = '虛擬貨幣' THEN '【快速發貨】' + @ProductName + ' 安全交易！'
            WHEN @ProductType = '稀有裝備' THEN '【稀有收藏】' + @ProductName + ' 錯過可惜！'
            WHEN @ProductType = '限定商品' THEN '【限量發售】' + @ProductName + ' 僅此一件！'
            ELSE '【精品收藏】' + @ProductName + ' 完美品相！'
        END;
        
        -- 生成商品描述
        DECLARE @ProductDescription NVARCHAR(1000);
        SET @ProductDescription = CASE @ProductType
            WHEN '遊戲道具' THEN '精心培養的高品質道具，屬性完美，適合各種副本和PVP。誠信賣家，安全交易，支援遊戲內當面交易。'
            WHEN '遊戲帳號' THEN '親自培養的高等級帳號，裝備齊全，技能滿級，多個稀有成就。帳號資料完整，支援改密碼和綁定。'
            WHEN '虛擬貨幣' THEN '正當途徑獲得的遊戲貨幣，數量充足，可用於購買各種遊戲內商品。快速發貨，24小時在線服務。'
            WHEN '稀有裝備' THEN '稀有度極高的頂級裝備，外觀華麗，屬性強大。收藏價值很高，錯過就很難再找到同款了。'
            WHEN '限定商品' THEN '限時活動獲得的珍貴道具，現在已經絕版無法再取得。對收藏家來說是必備之選。'
            ELSE '精美的遊戲相關收藏品，做工精細，適合遊戲愛好者收藏或送禮。全新未拆封，品相完美。'
        END;
        
        -- 隨機定價
        DECLARE @Price DECIMAL(18,2);
        SET @Price = CASE @ProductType
            WHEN '遊戲道具' THEN (50 + (ABS(CHECKSUM(NEWID())) % 200)) * 1.0 -- 50-250
            WHEN '遊戲帳號' THEN (200 + (ABS(CHECKSUM(NEWID())) % 800)) * 1.0 -- 200-1000
            WHEN '虛擬貨幣' THEN (30 + (ABS(CHECKSUM(NEWID())) % 120)) * 1.0 -- 30-150
            WHEN '稀有裝備' THEN (100 + (ABS(CHECKSUM(NEWID())) % 400)) * 1.0 -- 100-500
            WHEN '限定商品' THEN (300 + (ABS(CHECKSUM(NEWID())) % 700)) * 1.0 -- 300-1000
            ELSE (80 + (ABS(CHECKSUM(NEWID())) % 320)) * 1.0 -- 80-400
        END;
        
        -- 隨機選擇商品狀態
        DECLARE @ProductStatus NVARCHAR(50);
        DECLARE @StatusRand INT = ABS(CHECKSUM(NEWID())) % 100;
        
        SET @ProductStatus = CASE 
            WHEN @StatusRand < 80 THEN 'active'      -- 80% 上架中
            WHEN @StatusRand < 90 THEN 'sold'        -- 10% 已售出
            WHEN @StatusRand < 95 THEN 'removed'     -- 5% 已下架
            ELSE 'suspended'                         -- 5% 已暫停
        END;
        
        -- 隨機關聯官方商品 (30% 機率)
        DECLARE @LinkedProductId INT = NULL;
        IF (ABS(CHECKSUM(NEWID())) % 100) < 30
        BEGIN
            SELECT TOP 1 @LinkedProductId = product_id 
            FROM ProductInfo 
            WHERE product_type IN ('遊戲', '周邊') 
            ORDER BY NEWID();
        END
        
        -- 生成商品建立時間 (最近30天內)
        DECLARE @ProductCreatedAt DATETIME2 = DATEADD(DAY, -(ABS(CHECKSUM(NEWID())) % 30), GETUTCDATE());
        
        -- 插入商品記錄
        INSERT INTO PlayerMarketProductInfo (
            p_product_type, p_product_title, p_product_name, p_product_description,
            product_id, seller_id, p_status, price, 
            p_product_img_id, created_at, updated_at
        )
        VALUES (
            @ProductType, @ProductTitle, @ProductName, @ProductDescription,
            @LinkedProductId, @SellerId, @ProductStatus, @Price,
            'IMG-' + CAST(@SellerId AS VARCHAR) + '-' + CAST(@ProductIndex AS VARCHAR),
            @ProductCreatedAt, @ProductCreatedAt
        );
        
        DECLARE @NewProductId INT = SCOPE_IDENTITY();
        
        -- 為商品生成 1-3 張圖片
        DECLARE @ImageCount INT = 1 + (ABS(CHECKSUM(NEWID())) % 3); -- 1-3張
        DECLARE @ImageIndex INT = 0;
        
        WHILE @ImageIndex < @ImageCount
        BEGIN
            INSERT INTO PlayerMarketProductImgs (
                p_product_id, p_product_img_url
            )
            VALUES (
                @NewProductId,
                0x89504E470D0A1A0A -- 簡化的圖片二進位資料 (PNG header)
            );
            
            SET @ImageIndex = @ImageIndex + 1;
        END
        
        SET @ProductCount = @ProductCount + 1;
        SET @ProductIndex = @ProductIndex + 1;
    END
    
    SET @SellerId = @SellerId + 1;
END

PRINT '市場商品生成完成！總計: ' + CAST(@ProductCount AS VARCHAR) + ' 個商品';

-- 生成訂單資料
PRINT '開始生成市場訂單資料...';

DECLARE @BuyerId INT = 21; -- 從第21個用戶開始作為買家
DECLARE @MaxBuyerId INT = (SELECT MIN(40, MAX(User_ID)) FROM Users);
DECLARE @OrderCount INT = 0;

WHILE @BuyerId <= @MaxBuyerId
BEGIN
    -- 每個買家隨機生成 0-3 個訂單
    DECLARE @BuyerOrderCount INT = ABS(CHECKSUM(NEWID())) % 4; -- 0-3個訂單
    DECLARE @OrderIndex INT = 0;
    
    WHILE @OrderIndex < @BuyerOrderCount
    BEGIN
        -- 隨機選擇一個可購買的商品
        DECLARE @SelectedProductId INT;
        DECLARE @SelectedSellerId INT;
        DECLARE @SelectedPrice DECIMAL(18,2);
        
        SELECT TOP 1 
            @SelectedProductId = p_product_id,
            @SelectedSellerId = seller_id,
            @SelectedPrice = price
        FROM PlayerMarketProductInfo 
        WHERE p_status = 'active' 
        AND seller_id != @BuyerId -- 不能買自己的商品
        ORDER BY NEWID();
        
        IF @SelectedProductId IS NOT NULL
        BEGIN
            -- 生成訂單日期 (最近20天內)
            DECLARE @OrderDate DATETIME2 = DATEADD(DAY, -(ABS(CHECKSUM(NEWID())) % 20), GETUTCDATE());
            
            -- 隨機決定訂單狀態
            DECLARE @OrderStatus NVARCHAR(50);
            DECLARE @PaymentStatus NVARCHAR(50);
            DECLARE @OrderStatusRand INT = ABS(CHECKSUM(NEWID())) % 100;
            
            IF @OrderStatusRand < 60 -- 60% 已完成
            BEGIN
                SET @OrderStatus = 'Completed';
                SET @PaymentStatus = 'Paid';
            END
            ELSE IF @OrderStatusRand < 80 -- 20% 交易中
            BEGIN
                SET @OrderStatus = 'Trading';
                SET @PaymentStatus = 'Paid';
            END
            ELSE IF @OrderStatusRand < 95 -- 15% 已建立
            BEGIN
                SET @OrderStatus = 'Created';
                SET @PaymentStatus = 'Pending';
            END
            ELSE -- 5% 已取消
            BEGIN
                SET @OrderStatus = 'Cancelled';
                SET @PaymentStatus = 'N/A';
            END
            
            -- 數量通常是1
            DECLARE @Quantity INT = 1;
            DECLARE @OrderTotal DECIMAL(18,2) = @SelectedPrice * @Quantity;
            
            -- 插入訂單記錄
            INSERT INTO PlayerMarketOrderInfo (
                p_product_id, seller_id, buyer_id, p_order_date,
                p_order_status, p_payment_status, p_unit_price, p_quantity, p_order_total,
                p_order_created_at, p_order_updated_at
            )
            VALUES (
                @SelectedProductId, @SelectedSellerId, @BuyerId, @OrderDate,
                @OrderStatus, @PaymentStatus, @SelectedPrice, @Quantity, @OrderTotal,
                @OrderDate, @OrderDate
            );
            
            DECLARE @NewOrderId INT = SCOPE_IDENTITY();
            
            -- 如果訂單是交易中或已完成，建立交易頁面
            IF @OrderStatus IN ('Trading', 'Completed')
            BEGIN
                -- 計算平台抽成 (5%)
                DECLARE @PlatformFee DECIMAL(18,2) = @OrderTotal * 0.05;
                
                -- 生成移交和接收時間
                DECLARE @SellerTransferredAt DATETIME2 = NULL;
                DECLARE @BuyerReceivedAt DATETIME2 = NULL;
                DECLARE @CompletedAt DATETIME2 = NULL;
                
                IF @OrderStatus = 'Trading'
                BEGIN
                    -- 交易中：隨機決定移交狀態
                    IF (ABS(CHECKSUM(NEWID())) % 2) = 0
                    BEGIN
                        SET @SellerTransferredAt = DATEADD(HOUR, 2, @OrderDate);
                    END
                END
                ELSE IF @OrderStatus = 'Completed'
                BEGIN
                    -- 已完成：都有時間戳
                    SET @SellerTransferredAt = DATEADD(HOUR, 2, @OrderDate);
                    SET @BuyerReceivedAt = DATEADD(HOUR, 4, @OrderDate);
                    SET @CompletedAt = DATEADD(HOUR, 4, @OrderDate);
                    
                    -- 更新商品狀態為已售出
                    UPDATE PlayerMarketProductInfo 
                    SET p_status = 'sold', updated_at = @CompletedAt
                    WHERE p_product_id = @SelectedProductId;
                END
                
                -- 插入交易頁面記錄
                INSERT INTO PlayerMarketOrderTradepage (
                    p_order_id, p_product_id, p_order_platform_fee,
                    seller_transferred_at, buyer_received_at, completed_at
                )
                VALUES (
                    @NewOrderId, @SelectedProductId, @PlatformFee,
                    @SellerTransferredAt, @BuyerReceivedAt, @CompletedAt
                );
                
                DECLARE @TradepageId INT = SCOPE_IDENTITY();
                
                -- 為交易頁面生成 1-5 條訊息
                DECLARE @MessageCount INT = 1 + (ABS(CHECKSUM(NEWID())) % 5); -- 1-5條訊息
                DECLARE @MessageIndex INT = 0;
                
                WHILE @MessageIndex < @MessageCount
                BEGIN
                    DECLARE @MsgFrom NVARCHAR(20);
                    DECLARE @MessageText NVARCHAR(500);
                    
                    -- 隨機決定發送者
                    IF (ABS(CHECKSUM(NEWID())) % 2) = 0
                    BEGIN
                        SET @MsgFrom = 'seller';
                        SET @MessageText = CASE (ABS(CHECKSUM(NEWID())) % 4)
                            WHEN 0 THEN '您好，感謝購買！我會盡快安排交易。'
                            WHEN 1 THEN '商品已準備好，請確認收貨地點和時間。'
                            WHEN 2 THEN '道具已移交，請確認收到後點選確認接收。'
                            ELSE '交易愉快！有任何問題歡迎聯繫。'
                        END;
                    END
                    ELSE
                    BEGIN
                        SET @MsgFrom = 'buyer';
                        SET @MessageText = CASE (ABS(CHECKSUM(NEWID())) % 4)
                            WHEN 0 THEN '您好，請問什麼時候可以交易呢？'
                            WHEN 1 THEN '我已經在線上了，可以開始交易嗎？'
                            WHEN 2 THEN '商品收到了，謝謝！'
                            ELSE '賣家很專業，推薦！'
                        END;
                    END
                    
                    -- 生成訊息時間
                    DECLARE @MessageTime DATETIME2 = DATEADD(MINUTE, @MessageIndex * 30, @OrderDate);
                    
                    INSERT INTO PlayerMarketTradeMsg (
                        p_order_tradepage_id, msg_from, message_text, created_at
                    )
                    VALUES (
                        @TradepageId, @MsgFrom, @MessageText, @MessageTime
                    );
                    
                    SET @MessageIndex = @MessageIndex + 1;
                END
            END
            
            SET @OrderCount = @OrderCount + 1;
        END
        
        SET @OrderIndex = @OrderIndex + 1;
    END
    
    SET @BuyerId = @BuyerId + 1;
END

PRINT '市場訂單生成完成！總計: ' + CAST(@OrderCount AS VARCHAR) + ' 個訂單';

-- 生成自由市場排行榜資料
PRINT '開始生成自由市場排行榜資料...';

-- 生成最近30天的日榜資料
DECLARE @RankingDate DATE = CAST(DATEADD(DAY, -30, GETDATE()) AS DATE);
DECLARE @EndDate DATE = CAST(GETDATE() AS DATE);

WHILE @RankingDate <= @EndDate
BEGIN
    -- 計算當日各商品的交易額和交易量
    WITH DailyStats AS (
        SELECT 
            pmo.p_product_id,
            SUM(pmo.p_order_total) as trading_amount,
            SUM(pmo.p_quantity) as trading_volume,
            ROW_NUMBER() OVER (ORDER BY SUM(pmo.p_order_total) DESC) as amount_rank,
            ROW_NUMBER() OVER (ORDER BY SUM(pmo.p_quantity) DESC) as volume_rank
        FROM PlayerMarketOrderInfo pmo
        WHERE CAST(pmo.p_order_date AS DATE) = @RankingDate
        AND pmo.p_order_status = 'Completed'
        GROUP BY pmo.p_product_id
    )
    INSERT INTO PlayerMarketRanking (p_period_type, p_ranking_date, p_product_id, p_ranking_metric, p_ranking_position, p_trading_amount, created_at, updated_at)
    SELECT 
        'daily' as p_period_type,
        @RankingDate as p_ranking_date,
        p_product_id,
        'trading_amount' as p_ranking_metric,
        amount_rank as p_ranking_position,
        trading_amount as p_trading_amount,
        GETUTCDATE() as created_at,
        GETUTCDATE() as updated_at
    FROM DailyStats
    WHERE amount_rank <= 20 -- 只保留前20名
    
    UNION ALL
    
    SELECT 
        'daily' as p_period_type,
        @RankingDate as p_ranking_date,
        p_product_id,
        'trading_volume' as p_ranking_metric,
        volume_rank as p_ranking_position,
        trading_volume as p_trading_amount,
        GETUTCDATE() as created_at,
        GETUTCDATE() as updated_at
    FROM DailyStats
    WHERE volume_rank <= 20; -- 只保留前20名
    
    SET @RankingDate = DATEADD(DAY, 1, @RankingDate);
END

-- 生成月榜資料 (最近3個月)
DECLARE @MonthStart DATE = CAST(DATEADD(MONTH, -3, GETDATE()) AS DATE);
DECLARE @MonthEnd DATE = CAST(GETDATE() AS DATE);

WITH MonthlyStats AS (
    SELECT 
        YEAR(pmo.p_order_date) as year_val,
        MONTH(pmo.p_order_date) as month_val,
        pmo.p_product_id,
        SUM(pmo.p_order_total) as trading_amount,
        SUM(pmo.p_quantity) as trading_volume,
        ROW_NUMBER() OVER (PARTITION BY YEAR(pmo.p_order_date), MONTH(pmo.p_order_date) ORDER BY SUM(pmo.p_order_total) DESC) as amount_rank,
        ROW_NUMBER() OVER (PARTITION BY YEAR(pmo.p_order_date), MONTH(pmo.p_order_date) ORDER BY SUM(pmo.p_quantity) DESC) as volume_rank
    FROM PlayerMarketOrderInfo pmo
    WHERE pmo.p_order_date >= @MonthStart AND pmo.p_order_date <= @MonthEnd
    AND pmo.p_order_status = 'Completed'
    GROUP BY YEAR(pmo.p_order_date), MONTH(pmo.p_order_date), pmo.p_product_id
)
INSERT INTO PlayerMarketRanking (p_period_type, p_ranking_date, p_product_id, p_ranking_metric, p_ranking_position, p_trading_amount, created_at, updated_at)
SELECT 
    'monthly' as p_period_type,
    CAST(CONCAT(year_val, '-', FORMAT(month_val, '00'), '-01') AS DATE) as p_ranking_date,
    p_product_id,
    'trading_amount' as p_ranking_metric,
    amount_rank as p_ranking_position,
    trading_amount as p_trading_amount,
    GETUTCDATE() as created_at,
    GETUTCDATE() as updated_at
FROM MonthlyStats
WHERE amount_rank <= 20

UNION ALL

SELECT 
    'monthly' as p_period_type,
    CAST(CONCAT(year_val, '-', FORMAT(month_val, '00'), '-01') AS DATE) as p_ranking_date,
    p_product_id,
    'trading_volume' as p_ranking_metric,
    volume_rank as p_ranking_position,
    trading_volume as p_trading_amount,
    GETUTCDATE() as created_at,
    GETUTCDATE() as updated_at
FROM MonthlyStats
WHERE volume_rank <= 20;

PRINT '自由市場排行榜資料生成完成！';

-- 統計報告
PRINT '=== 自由市場系統種子資料統計報告 ===';

-- 總商品數量
DECLARE @TotalMarketProducts INT = (SELECT COUNT(*) FROM PlayerMarketProductInfo);
PRINT '總市場商品數量: ' + CAST(@TotalMarketProducts AS VARCHAR);

-- 商品狀態分布
PRINT '商品狀態分布:';
SELECT 
    p_status as 商品狀態,
    COUNT(*) as 商品數量,
    CAST(COUNT(*) * 100.0 / @TotalMarketProducts AS DECIMAL(5,2)) as 佔比
FROM PlayerMarketProductInfo
GROUP BY p_status
ORDER BY 商品數量 DESC;

-- 商品類型分布
PRINT '商品類型分布:';
SELECT 
    p_product_type as 商品類型,
    COUNT(*) as 商品數量,
    CAST(AVG(price) AS DECIMAL(10,2)) as 平均價格,
    CAST(MIN(price) AS DECIMAL(10,2)) as 最低價格,
    CAST(MAX(price) AS DECIMAL(10,2)) as 最高價格
FROM PlayerMarketProductInfo
GROUP BY p_product_type
ORDER BY 商品數量 DESC;

-- 訂單統計
PRINT '市場訂單統計:';
SELECT 
    COUNT(*) as 總訂單數,
    COUNT(CASE WHEN p_order_status = 'Completed' THEN 1 END) as 已完成訂單,
    COUNT(CASE WHEN p_order_status = 'Trading' THEN 1 END) as 交易中訂單,
    COUNT(CASE WHEN p_order_status = 'Created' THEN 1 END) as 已建立訂單,
    COUNT(CASE WHEN p_order_status = 'Cancelled' THEN 1 END) as 已取消訂單,
    CAST(AVG(p_order_total) AS DECIMAL(10,2)) as 平均訂單金額,
    CAST(SUM(p_order_total) AS DECIMAL(15,2)) as 總交易額
FROM PlayerMarketOrderInfo;

-- 交易頁面統計
PRINT '交易頁面統計:';
SELECT 
    COUNT(*) as 總交易頁面數,
    COUNT(CASE WHEN completed_at IS NOT NULL THEN 1 END) as 已完成交易,
    COUNT(CASE WHEN seller_transferred_at IS NOT NULL AND buyer_received_at IS NULL THEN 1 END) as 等待買家確認,
    COUNT(CASE WHEN seller_transferred_at IS NULL AND buyer_received_at IS NULL THEN 1 END) as 等待移交,
    CAST(SUM(p_order_platform_fee) AS DECIMAL(12,2)) as 總平台抽成
FROM PlayerMarketOrderTradepage;

-- 熱門商品 TOP 10
PRINT '最熱門市場商品 TOP 10 (按交易額):';
SELECT TOP 10
    pmp.p_product_name as 商品名稱,
    pmp.p_product_type as 商品類型,
    u.User_name as 賣家,
    COUNT(pmo.p_order_id) as 銷售次數,
    CAST(SUM(pmo.p_order_total) AS DECIMAL(12,2)) as 總交易額,
    CAST(AVG(pmo.p_order_total) AS DECIMAL(10,2)) as 平均成交價
FROM PlayerMarketProductInfo pmp
LEFT JOIN PlayerMarketOrderInfo pmo ON pmp.p_product_id = pmo.p_product_id AND pmo.p_order_status = 'Completed'
LEFT JOIN Users u ON pmp.seller_id = u.User_ID
GROUP BY pmp.p_product_id, pmp.p_product_name, pmp.p_product_type, u.User_name
ORDER BY 總交易額 DESC;

-- 活躍賣家統計
PRINT '活躍賣家統計:';
SELECT TOP 10
    u.User_name as 賣家名稱,
    COUNT(DISTINCT pmp.p_product_id) as 上架商品數,
    COUNT(pmo.p_order_id) as 總銷售筆數,
    COUNT(CASE WHEN pmo.p_order_status = 'Completed' THEN 1 END) as 成功交易數,
    CAST(SUM(CASE WHEN pmo.p_order_status = 'Completed' THEN pmo.p_order_total ELSE 0 END) AS DECIMAL(12,2)) as 總銷售額,
    CAST(AVG(CASE WHEN pmo.p_order_status = 'Completed' THEN pmo.p_order_total END) AS DECIMAL(10,2)) as 平均成交價
FROM Users u
LEFT JOIN PlayerMarketProductInfo pmp ON u.User_ID = pmp.seller_id
LEFT JOIN PlayerMarketOrderInfo pmo ON pmp.p_product_id = pmo.p_product_id
WHERE pmp.p_product_id IS NOT NULL
GROUP BY u.User_ID, u.User_name
ORDER BY 總銷售額 DESC;

-- 交易訊息統計
PRINT '交易訊息統計:';
SELECT 
    COUNT(*) as 總訊息數,
    COUNT(CASE WHEN msg_from = 'seller' THEN 1 END) as 賣家訊息數,
    COUNT(CASE WHEN msg_from = 'buyer' THEN 1 END) as 買家訊息數,
    COUNT(DISTINCT p_order_tradepage_id) as 有訊息的交易頁面數,
    CAST(AVG(msg_per_trade) AS DECIMAL(5,2)) as 平均每筆交易訊息數
FROM PlayerMarketTradeMsg
CROSS JOIN (
    SELECT CAST(COUNT(*) AS FLOAT) / COUNT(DISTINCT p_order_tradepage_id) as msg_per_trade
    FROM PlayerMarketTradeMsg
) avg_calc;

-- 排行榜統計
PRINT '市場排行榜統計:';
SELECT 
    p_period_type as 榜單類型,
    p_ranking_metric as 排名指標,
    COUNT(*) as 榜單記錄數,
    COUNT(DISTINCT p_product_id) as 上榜商品數,
    COUNT(DISTINCT p_ranking_date) as 榜單天數
FROM PlayerMarketRanking
GROUP BY p_period_type, p_ranking_metric
ORDER BY 榜單類型, 排名指標;

-- 最近7天交易趨勢
PRINT '最近7天市場交易趨勢:';
SELECT 
    CAST(p_order_date AS DATE) as 日期,
    COUNT(*) as 訂單數,
    COUNT(CASE WHEN p_order_status = 'Completed' THEN 1 END) as 完成訂單數,
    CAST(SUM(CASE WHEN p_order_status = 'Completed' THEN p_order_total ELSE 0 END) AS DECIMAL(12,2)) as 當日交易額,
    COUNT(DISTINCT p_product_id) as 交易商品數
FROM PlayerMarketOrderInfo
WHERE p_order_date >= DATEADD(DAY, -7, GETDATE())
GROUP BY CAST(p_order_date AS DATE)
ORDER BY 日期 DESC;

-- 價格區間分析
PRINT '商品價格區間分析:';
SELECT 
    CASE 
        WHEN price < 100 THEN '低價區(0-100)'
        WHEN price < 300 THEN '中低價區(100-300)'
        WHEN price < 500 THEN '中價區(300-500)'
        WHEN price < 800 THEN '中高價區(500-800)'
        ELSE '高價區(800+)'
    END as 價格區間,
    COUNT(*) as 商品數量,
    COUNT(CASE WHEN p_status = 'active' THEN 1 END) as 上架中數量,
    COUNT(CASE WHEN p_status = 'sold' THEN 1 END) as 已售出數量,
    CAST(AVG(price) AS DECIMAL(10,2)) as 平均價格
FROM PlayerMarketProductInfo
GROUP BY 
    CASE 
        WHEN price < 100 THEN '低價區(0-100)'
        WHEN price < 300 THEN '中低價區(100-300)'
        WHEN price < 500 THEN '中價區(300-500)'
        WHEN price < 800 THEN '中高價區(500-800)'
        ELSE '高價區(800+)'
    END
ORDER BY 價格區間;

PRINT '自由市場系統種子資料插入完成！';
GO