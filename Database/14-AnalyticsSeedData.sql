-- =============================================
-- GameCore 分析系統種子資料
-- 建立完整的遊戲熱度、指標、排行榜、洞察測試資料
-- 嚴格按照規格要求生成符合業務邏輯的完整分析系統資料
-- =============================================

USE GameCore;
GO

PRINT '開始插入分析系統種子資料...';

-- 清除現有的分析相關記錄 (按照外鍵順序)
PRINT '清除現有分析資料...';
DELETE FROM post_sources;
DELETE FROM post_metric_snapshot;
DELETE FROM posts WHERE post_id > 0;
DELETE FROM leaderboard_snapshots;
DELETE FROM popularity_index_daily;
DELETE FROM game_metric_daily;
DELETE FROM game_source_map;
DELETE FROM metrics WHERE metric_id > 0;
DELETE FROM metric_sources WHERE source_id > 0;
DELETE FROM games WHERE game_id > 0;

-- 重置自增ID
DBCC CHECKIDENT ('games', RESEED, 0);
DBCC CHECKIDENT ('metric_sources', RESEED, 0);
DBCC CHECKIDENT ('metrics', RESEED, 0);
DBCC CHECKIDENT ('game_source_map', RESEED, 0);
DBCC CHECKIDENT ('game_metric_daily', RESEED, 0);
DBCC CHECKIDENT ('popularity_index_daily', RESEED, 0);
DBCC CHECKIDENT ('leaderboard_snapshots', RESEED, 0);
DBCC CHECKIDENT ('posts', RESEED, 0);
DBCC CHECKIDENT ('post_metric_snapshot', RESEED, 0);
DBCC CHECKIDENT ('post_sources', RESEED, 0);

PRINT '開始生成遊戲資料...';

-- 建立遊戲清單
INSERT INTO games (game_name, game_description, is_active, created_at) VALUES
('原神', '開放世界冒險RPG遊戲，擁有精美的畫面和豐富的角色系統', 1, GETUTCDATE()),
('英雄聯盟', '5v5戰術競技遊戲，全球最受歡迎的MOBA遊戲之一', 1, GETUTCDATE()),
('絕地求生', '大逃殺射擊遊戲，考驗玩家的生存技巧和戰術', 1, GETUTCDATE()),
('我的世界', '沙盒建造遊戲，無限創意和探索的世界', 1, GETUTCDATE()),
('守望先鋒2', '6v6英雄射擊遊戲，團隊合作至關重要', 1, GETUTCDATE()),
('VALORANT', '戰術射擊遊戲，結合精準射擊和獨特能力', 1, GETUTCDATE()),
('Apex英雄', '大逃殺射擊遊戲，快節奏的戰鬥體驗', 1, GETUTCDATE()),
('Counter-Strike 2', '經典戰術射擊遊戲的最新版本', 1, GETUTCDATE()),
('Dota 2', '複雜策略的MOBA遊戲，高技巧要求', 1, GETUTCDATE()),
('FIFA 24', '足球模擬遊戲，真實的球員和球隊體驗', 1, GETUTCDATE()),
('糖豆人：終極淘汰賽', '多人聚會遊戲，可愛的競賽挑戰', 1, GETUTCDATE()),
('Among Us', '社交推理遊戲，找出隱藏的內鬼', 1, GETUTCDATE()),
('暗黑破壞神4', 'ARPG遊戲，史詩級的冒險和戰利品收集', 1, GETUTCDATE()),
('最終幻想XIV', 'MMORPG遊戲，豐富的故事和社交體驗', 1, GETUTCDATE()),
('魔獸世界', '經典MMORPG，廣闊的幻想世界', 1, GETUTCDATE()),
('星際戰甲', '免費科幻動作遊戲，太空忍者冒險', 1, GETUTCDATE()),
('巫師3：狂獵', '開放世界RPG，深度的故事和選擇', 1, GETUTCDATE()),
('賽博朋克2077', '未來科幻RPG，沉浸式的夜城體驗', 1, GETUTCDATE()),
('艾爾登法環', '開放世界動作RPG，挑戰性的戰鬥', 1, GETUTCDATE()),
('薩爾達傳說：王國之淚', '開放世界冒險遊戲，創意無限的探索', 1, GETUTCDATE());

PRINT '遊戲資料生成完成！總計: 20 個遊戲';

-- 建立指標來源
PRINT '開始生成指標來源資料...';

INSERT INTO metric_sources (source_name, api_endpoint, is_active, created_at) VALUES
('Steam', 'https://api.steampowered.com/ISteamUserStats/GetNumberOfCurrentPlayers/v1/', 1, GETUTCDATE()),
('Twitch', 'https://api.twitch.tv/helix/games/top', 1, GETUTCDATE()),
('YouTube Gaming', 'https://www.googleapis.com/youtube/v3/search', 1, GETUTCDATE()),
('Discord', 'https://discord.com/api/v10/applications', 1, GETUTCDATE()),
('Reddit', 'https://www.reddit.com/r/{game}/about.json', 1, GETUTCDATE()),
('Twitter', 'https://api.twitter.com/2/tweets/search/recent', 1, GETUTCDATE()),
('Google Trends', 'https://trends.googleapis.com/trends/api/explore', 1, GETUTCDATE()),
('內部論壇', NULL, 1, GETUTCDATE()),
('官方商城', NULL, 1, GETUTCDATE()),
('玩家市場', NULL, 1, GETUTCDATE());

PRINT '指標來源生成完成！總計: 10 個來源';

-- 建立指標定義
PRINT '開始生成指標定義資料...';

INSERT INTO metrics (source_id, code, unit, description, is_active, created_at) VALUES
-- Steam 指標
(1, 'concurrent_users', 'users', 'Steam平台同時在線玩家數', 1, GETUTCDATE()),
(1, 'peak_users', 'users', 'Steam平台當日最高在線玩家數', 1, GETUTCDATE()),
(1, 'reviews_positive', 'count', 'Steam平台正面評價數量', 1, GETUTCDATE()),
(1, 'reviews_negative', 'count', 'Steam平台負面評價數量', 1, GETUTCDATE()),

-- Twitch 指標
(2, 'viewers', 'viewers', 'Twitch平台觀看人數', 1, GETUTCDATE()),
(2, 'streamers', 'streamers', 'Twitch平台直播主數量', 1, GETUTCDATE()),
(2, 'chat_messages', 'messages', 'Twitch聊天室訊息數量', 1, GETUTCDATE()),

-- YouTube Gaming 指標
(3, 'video_views', 'views', 'YouTube遊戲影片觀看次數', 1, GETUTCDATE()),
(3, 'video_uploads', 'videos', 'YouTube新上傳遊戲影片數量', 1, GETUTCDATE()),
(3, 'subscriber_growth', 'subscribers', 'YouTube遊戲頻道訂閱者增長', 1, GETUTCDATE()),

-- Discord 指標
(4, 'server_members', 'members', 'Discord伺服器成員數量', 1, GETUTCDATE()),
(4, 'active_members', 'members', 'Discord活躍成員數量', 1, GETUTCDATE()),
(4, 'message_count', 'messages', 'Discord訊息數量', 1, GETUTCDATE()),

-- Reddit 指標
(5, 'subscribers', 'subscribers', 'Reddit社群訂閱者數量', 1, GETUTCDATE()),
(5, 'posts', 'posts', 'Reddit貼文數量', 1, GETUTCDATE()),
(5, 'comments', 'comments', 'Reddit評論數量', 1, GETUTCDATE()),
(5, 'upvotes', 'votes', 'Reddit讚數統計', 1, GETUTCDATE()),

-- Twitter 指標
(6, 'mentions', 'mentions', 'Twitter提及次數', 1, GETUTCDATE()),
(6, 'hashtag_usage', 'hashtags', 'Twitter標籤使用次數', 1, GETUTCDATE()),
(6, 'retweets', 'retweets', 'Twitter轉推次數', 1, GETUTCDATE()),

-- Google Trends 指標
(7, 'search_interest', 'index', 'Google搜尋熱度指數', 1, GETUTCDATE()),
(7, 'rising_queries', 'queries', 'Google熱門搜尋查詢', 1, GETUTCDATE()),

-- 內部論壇指標
(8, 'forum_posts', 'posts', '論壇貼文數量', 1, GETUTCDATE()),
(8, 'forum_replies', 'replies', '論壇回覆數量', 1, GETUTCDATE()),
(8, 'forum_likes', 'likes', '論壇按讚數量', 1, GETUTCDATE()),
(8, 'forum_views', 'views', '論壇瀏覽次數', 1, GETUTCDATE()),

-- 官方商城指標
(9, 'product_sales', 'sales', '商品銷售數量', 1, GETUTCDATE()),
(9, 'revenue', 'currency', '銷售收入', 1, GETUTCDATE()),
(9, 'product_views', 'views', '商品瀏覽次數', 1, GETUTCDATE()),

-- 玩家市場指標
(10, 'market_trades', 'trades', '市場交易數量', 1, GETUTCDATE()),
(10, 'market_volume', 'currency', '市場交易額', 1, GETUTCDATE()),
(10, 'active_listings', 'listings', '活躍商品列表數量', 1, GETUTCDATE());

PRINT '指標定義生成完成！總計: 28 個指標';

-- 建立遊戲來源對應
PRINT '開始生成遊戲來源對應資料...';

DECLARE @GameId INT = 1;
DECLARE @MaxGameId INT = (SELECT MAX(game_id) FROM games);

WHILE @GameId <= @MaxGameId
BEGIN
    -- 為每個遊戲隨機對應 3-6 個數據來源
    DECLARE @SourceCount INT = 3 + (ABS(CHECKSUM(NEWID())) % 4); -- 3-6個來源
    DECLARE @SourceIndex INT = 0;
    
    WHILE @SourceIndex < @SourceCount
    BEGIN
        -- 隨機選擇來源
        DECLARE @SourceId INT = 1 + (ABS(CHECKSUM(NEWID())) % 10);
        
        -- 生成外部ID
        DECLARE @ExternalId NVARCHAR(100);
        SET @ExternalId = CASE @SourceId
            WHEN 1 THEN CAST(100000 + @GameId * 1000 + @SourceIndex AS NVARCHAR) -- Steam App ID
            WHEN 2 THEN 'game_' + CAST(@GameId AS NVARCHAR) + '_' + CAST(@SourceIndex AS NVARCHAR) -- Twitch Game ID
            WHEN 3 THEN 'yt_' + CAST(@GameId AS NVARCHAR) + '_channel' -- YouTube Channel ID
            WHEN 4 THEN CAST(200000000000000000 + @GameId * 1000000000000000 AS NVARCHAR) -- Discord Server ID
            WHEN 5 THEN 'r/game' + CAST(@GameId AS NVARCHAR) -- Reddit Subreddit
            WHEN 6 THEN '#game' + CAST(@GameId AS NVARCHAR) -- Twitter Hashtag
            WHEN 7 THEN '/m/' + CAST(@GameId AS NVARCHAR) -- Google Trends Topic
            ELSE 'internal_' + CAST(@GameId AS NVARCHAR)
        END;
        
        -- 檢查是否已存在相同對應
        IF NOT EXISTS (
            SELECT 1 FROM game_source_map 
            WHERE game_id = @GameId AND source_id = @SourceId
        )
        BEGIN
            INSERT INTO game_source_map (game_id, source_id, external_id, is_active, created_at)
            VALUES (@GameId, @SourceId, @ExternalId, 1, GETUTCDATE());
        END
        
        SET @SourceIndex = @SourceIndex + 1;
    END
    
    SET @GameId = @GameId + 1;
END

PRINT '遊戲來源對應生成完成！';

-- 生成每日指標數據 (最近30天)
PRINT '開始生成每日指標數據...';

DECLARE @StartDate DATE = DATEADD(DAY, -30, CAST(GETUTCDATE() AS DATE));
DECLARE @EndDate DATE = CAST(GETUTCDATE() AS DATE);
DECLARE @CurrentDate DATE = @StartDate;
DECLARE @MetricCount INT = 0;

WHILE @CurrentDate <= @EndDate
BEGIN
    PRINT '生成日期: ' + CAST(@CurrentDate AS VARCHAR);
    
    -- 為每個遊戲生成指標數據
    SET @GameId = 1;
    WHILE @GameId <= @MaxGameId
    BEGIN
        -- 為每個遊戲的每個指標生成數據
        DECLARE @MetricId INT = 1;
        DECLARE @MaxMetricId INT = (SELECT MAX(metric_id) FROM metrics);
        
        WHILE @MetricId <= @MaxMetricId
        BEGIN
            -- 檢查此遊戲是否有此來源的對應
            DECLARE @MetricSourceId INT = (SELECT source_id FROM metrics WHERE metric_id = @MetricId);
            
            IF EXISTS (
                SELECT 1 FROM game_source_map 
                WHERE game_id = @GameId AND source_id = @MetricSourceId AND is_active = 1
            )
            BEGIN
                -- 生成符合指標特性的隨機數值
                DECLARE @BaseValue DECIMAL(18,4);
                DECLARE @MetricCode NVARCHAR(50) = (SELECT code FROM metrics WHERE metric_id = @MetricId);
                
                -- 根據指標類型設定基礎值範圍
                SET @BaseValue = CASE 
                    WHEN @MetricCode LIKE '%concurrent_users%' THEN 10000 + (ABS(CHECKSUM(NEWID())) % 50000)
                    WHEN @MetricCode LIKE '%peak_users%' THEN 15000 + (ABS(CHECKSUM(NEWID())) % 75000)
                    WHEN @MetricCode LIKE '%viewers%' THEN 5000 + (ABS(CHECKSUM(NEWID())) % 25000)
                    WHEN @MetricCode LIKE '%streamers%' THEN 100 + (ABS(CHECKSUM(NEWID())) % 500)
                    WHEN @MetricCode LIKE '%views%' THEN 50000 + (ABS(CHECKSUM(NEWID())) % 200000)
                    WHEN @MetricCode LIKE '%subscribers%' THEN 1000 + (ABS(CHECKSUM(NEWID())) % 10000)
                    WHEN @MetricCode LIKE '%posts%' THEN 50 + (ABS(CHECKSUM(NEWID())) % 200)
                    WHEN @MetricCode LIKE '%messages%' THEN 500 + (ABS(CHECKSUM(NEWID())) % 2000)
                    WHEN @MetricCode LIKE '%sales%' THEN 10 + (ABS(CHECKSUM(NEWID())) % 100)
                    WHEN @MetricCode LIKE '%revenue%' THEN 1000 + (ABS(CHECKSUM(NEWID())) % 10000)
                    ELSE 100 + (ABS(CHECKSUM(NEWID())) % 1000)
                END;
                
                -- 加入遊戲熱度調整 (前5個遊戲較熱門)
                IF @GameId <= 5
                    SET @BaseValue = @BaseValue * (1.2 + (ABS(CHECKSUM(NEWID())) % 30) / 100.0);
                ELSE IF @GameId <= 10
                    SET @BaseValue = @BaseValue * (1.0 + (ABS(CHECKSUM(NEWID())) % 20) / 100.0);
                ELSE
                    SET @BaseValue = @BaseValue * (0.8 + (ABS(CHECKSUM(NEWID())) % 40) / 100.0);
                
                -- 加入時間趨勢 (近期數據較高)
                DECLARE @DaysFromStart INT = DATEDIFF(DAY, @StartDate, @CurrentDate);
                DECLARE @TrendMultiplier DECIMAL(5,2) = 0.8 + (@DaysFromStart * 0.4 / 30.0) + (ABS(CHECKSUM(NEWID())) % 20) / 100.0;
                SET @BaseValue = @BaseValue * @TrendMultiplier;
                
                -- 週末效應 (週末數據可能不同)
                IF DATENAME(WEEKDAY, @CurrentDate) IN ('Saturday', 'Sunday')
                BEGIN
                    IF @MetricCode LIKE '%concurrent_users%' OR @MetricCode LIKE '%viewers%'
                        SET @BaseValue = @BaseValue * (1.1 + (ABS(CHECKSUM(NEWID())) % 20) / 100.0);
                    ELSE
                        SET @BaseValue = @BaseValue * (0.9 + (ABS(CHECKSUM(NEWID())) % 20) / 100.0);
                END
                
                -- 插入數據
                INSERT INTO game_metric_daily (game_id, metric_id, date, value, agg_method, created_at, updated_at)
                VALUES (@GameId, @MetricId, @CurrentDate, @BaseValue, 'sum', GETUTCDATE(), GETUTCDATE());
                
                SET @MetricCount = @MetricCount + 1;
            END
            
            SET @MetricId = @MetricId + 1;
        END
        
        SET @GameId = @GameId + 1;
    END
    
    SET @CurrentDate = DATEADD(DAY, 1, @CurrentDate);
END

PRINT '每日指標數據生成完成！總計: ' + CAST(@MetricCount AS VARCHAR) + ' 個數據點';

-- 計算熱度指數
PRINT '開始計算熱度指數...';

SET @CurrentDate = @StartDate;
DECLARE @IndexCount INT = 0;

WHILE @CurrentDate <= @EndDate
BEGIN
    -- 為每個遊戲計算當日熱度指數
    SET @GameId = 1;
    WHILE @GameId <= @MaxGameId
    BEGIN
        -- 計算加權熱度指數
        DECLARE @IndexValue DECIMAL(18,4) = 0;
        DECLARE @TotalWeight DECIMAL(18,4) = 0;
        
        -- 取得該遊戲當日的所有指標數據
        DECLARE metric_cursor CURSOR FOR
            SELECT gmd.value, 
                   CASE 
                       WHEN m.code LIKE '%concurrent_users%' THEN 3.0
                       WHEN m.code LIKE '%viewers%' THEN 2.5
                       WHEN m.code LIKE '%posts%' THEN 2.0
                       WHEN m.code LIKE '%views%' THEN 1.5
                       WHEN m.code LIKE '%sales%' THEN 2.8
                       WHEN m.code LIKE '%revenue%' THEN 3.2
                       ELSE 1.0
                   END as weight
            FROM game_metric_daily gmd
            JOIN metrics m ON gmd.metric_id = m.metric_id
            WHERE gmd.game_id = @GameId AND gmd.date = @CurrentDate;
        
        DECLARE @MetricValue DECIMAL(18,4), @Weight DECIMAL(18,4);
        
        OPEN metric_cursor;
        FETCH NEXT FROM metric_cursor INTO @MetricValue, @Weight;
        
        WHILE @@FETCH_STATUS = 0
        BEGIN
            -- 正規化數值 (使用對數縮放)
            DECLARE @NormalizedValue DECIMAL(18,4) = LOG(1 + @MetricValue);
            SET @IndexValue = @IndexValue + (@NormalizedValue * @Weight);
            SET @TotalWeight = @TotalWeight + @Weight;
            
            FETCH NEXT FROM metric_cursor INTO @MetricValue, @Weight;
        END
        
        CLOSE metric_cursor;
        DEALLOCATE metric_cursor;
        
        -- 計算最終指數 (避免除零)
        IF @TotalWeight > 0
        BEGIN
            SET @IndexValue = @IndexValue / @TotalWeight;
            
            -- 插入熱度指數
            INSERT INTO popularity_index_daily (game_id, date, index_value, created_at)
            VALUES (@GameId, @CurrentDate, @IndexValue, GETUTCDATE());
            
            SET @IndexCount = @IndexCount + 1;
        END
        
        SET @GameId = @GameId + 1;
    END
    
    SET @CurrentDate = DATEADD(DAY, 1, @CurrentDate);
END

PRINT '熱度指數計算完成！總計: ' + CAST(@IndexCount AS VARCHAR) + ' 個指數';

-- 生成排行榜快照
PRINT '開始生成排行榜快照...';

SET @CurrentDate = @StartDate;
DECLARE @SnapshotCount INT = 0;

WHILE @CurrentDate <= @EndDate
BEGIN
    -- 生成每日排行榜快照
    INSERT INTO leaderboard_snapshots (period, ts, game_id, rank, score, created_at)
    SELECT 
        'daily' as period,
        CAST(@CurrentDate AS DATETIME2) as ts,
        game_id,
        ROW_NUMBER() OVER (ORDER BY index_value DESC) as rank,
        index_value as score,
        GETUTCDATE() as created_at
    FROM popularity_index_daily
    WHERE date = @CurrentDate
    ORDER BY index_value DESC;
    
    SET @SnapshotCount = @SnapshotCount + @@ROWCOUNT;
    
    -- 每週生成週榜快照 (週日)
    IF DATENAME(WEEKDAY, @CurrentDate) = 'Sunday'
    BEGIN
        DECLARE @WeekStart DATE = DATEADD(DAY, -6, @CurrentDate);
        
        INSERT INTO leaderboard_snapshots (period, ts, game_id, rank, score, created_at)
        SELECT 
            'weekly' as period,
            CAST(@CurrentDate AS DATETIME2) as ts,
            game_id,
            ROW_NUMBER() OVER (ORDER BY avg_index DESC) as rank,
            avg_index as score,
            GETUTCDATE() as created_at
        FROM (
            SELECT 
                game_id,
                AVG(index_value) as avg_index
            FROM popularity_index_daily
            WHERE date BETWEEN @WeekStart AND @CurrentDate
            GROUP BY game_id
        ) weekly_avg
        ORDER BY avg_index DESC;
        
        SET @SnapshotCount = @SnapshotCount + @@ROWCOUNT;
    END
    
    SET @CurrentDate = DATEADD(DAY, 1, @CurrentDate);
END

PRINT '排行榜快照生成完成！總計: ' + CAST(@SnapshotCount AS VARCHAR) + ' 個快照';

-- 生成洞察貼文
PRINT '開始生成洞察貼文...';

DECLARE @PostCount INT = 0;
DECLARE @PostIndex INT = 0;

-- 生成 10-15 篇洞察貼文
DECLARE @TotalPosts INT = 10 + (ABS(CHECKSUM(NEWID())) % 6);

WHILE @PostIndex < @TotalPosts
BEGIN
    -- 隨機選擇遊戲
    DECLARE @PostGameId INT = 1 + (ABS(CHECKSUM(NEWID())) % @MaxGameId);
    DECLARE @PostGameName NVARCHAR(100) = (SELECT game_name FROM games WHERE game_id = @PostGameId);
    
    -- 生成貼文標題和內容
    DECLARE @PostTitle NVARCHAR(200);
    DECLARE @PostContent NVARCHAR(MAX);
    DECLARE @PostType INT = ABS(CHECKSUM(NEWID())) % 5;
    
    SET @PostTitle = CASE @PostType
        WHEN 0 THEN @PostGameName + ' 熱度飆升！玩家數量創新高'
        WHEN 1 THEN '深度分析：' + @PostGameName + ' 的社群活躍度趨勢'
        WHEN 2 THEN @PostGameName + ' vs 競品遊戲：市場表現對比'
        WHEN 3 THEN '數據洞察：' + @PostGameName + ' 玩家行為分析'
        ELSE '週報：' + @PostGameName + ' 綜合熱度指標解析'
    END;
    
    SET @PostContent = CASE @PostType
        WHEN 0 THEN '根據最新數據顯示，' + @PostGameName + ' 在各大平台的熱度指標都呈現顯著上升趨勢。Steam同時在線人數較上週增長35%，Twitch觀看人數突破新高，顯示該遊戲正處於熱度爆發期。這波增長主要受到最新更新內容的推動，玩家對新功能的反響非常正面。社群討論熱度也達到近期峰值，Reddit和Discord社群活躍度大幅提升。'
        WHEN 1 THEN @PostGameName + ' 的社群生態展現出強勁的活力。從數據分析來看，該遊戲的論壇互動率持續上升，玩家產出的內容質量和數量都有顯著提升。YouTube和Twitch平台上的相關內容創作者數量增加了28%，顯示遊戲的內容生態正在蓬勃發展。同時，官方社群的訂閱者增長速度也明顯加快，反映出玩家對遊戲的長期關注度。'
        WHEN 2 THEN '通過對比分析，' + @PostGameName + ' 在同類型遊戲中表現突出。相比競品，該遊戲在玩家留存率和日活躍度方面都有明顯優勢。特別是在核心玩家群體中，' + @PostGameName + ' 的忠誠度指標領先競品約20%。這種優勢主要體現在遊戲深度和社交功能的完善程度上，為玩家提供了更豐富的長期遊戲體驗。'
        WHEN 3 THEN '深入分析' + @PostGameName + '玩家數據發現一些有趣的行為模式。玩家的遊戲時長分布呈現雙峰特徵，反映出休閒玩家和核心玩家兩個明確的群體。付費玩家的ARPU值穩定增長，顯示遊戲的商業化策略效果良好。同時，玩家對社交功能的使用率不斷提升，群組活動和好友互動成為重要的留存驅動因素。'
        ELSE '本週' + @PostGameName + '綜合表現優異，各項關鍵指標均呈現正向趨勢。熱度指數較上週上升12%，在排行榜中的位置也有所提升。特別值得注意的是，遊戲在新興市場的表現超出預期，國際化策略初見成效。未來一週需要密切關注的指標包括新用戶轉化率和核心功能的使用情況，這些將直接影響遊戲的長期發展軌跡。'
    END;
    
    -- 隨機決定狀態和是否置頂
    DECLARE @PostStatus NVARCHAR(20);
    DECLARE @IsPinned BIT;
    DECLARE @StatusRand INT = ABS(CHECKSUM(NEWID())) % 100;
    
    SET @PostStatus = CASE 
        WHEN @StatusRand < 70 THEN 'published'
        WHEN @StatusRand < 85 THEN 'draft'
        WHEN @StatusRand < 95 THEN 'archived'
        ELSE 'hidden'
    END;
    
    SET @IsPinned = CASE WHEN (ABS(CHECKSUM(NEWID())) % 100) < 20 THEN 1 ELSE 0 END;
    
    -- 生成貼文建立時間
    DECLARE @PostCreatedAt DATETIME2 = DATEADD(DAY, -(ABS(CHECKSUM(NEWID())) % 15), GETUTCDATE());
    DECLARE @PostPublishedAt DATETIME2 = CASE WHEN @PostStatus = 'published' THEN @PostCreatedAt ELSE NULL END;
    
    -- 插入貼文
    INSERT INTO posts (game_id, title, content, status, pinned, created_at, published_at)
    VALUES (@PostGameId, @PostTitle, @PostContent, @PostStatus, @IsPinned, @PostCreatedAt, @PostPublishedAt);
    
    DECLARE @NewPostId INT = SCOPE_IDENTITY();
    
    -- 為已發佈的貼文建立指標快照
    IF @PostStatus = 'published' AND @PostPublishedAt IS NOT NULL
    BEGIN
        DECLARE @SnapshotDate DATE = CAST(@PostPublishedAt AS DATE);
        DECLARE @SnapshotIndexValue DECIMAL(18,4);
        
        SELECT @SnapshotIndexValue = index_value 
        FROM popularity_index_daily 
        WHERE game_id = @PostGameId AND date = @SnapshotDate;
        
        IF @SnapshotIndexValue IS NOT NULL
        BEGIN
            -- 生成詳細資訊 JSON
            DECLARE @DetailsJson NVARCHAR(MAX);
            SET @DetailsJson = '{"index_value":' + CAST(@SnapshotIndexValue AS NVARCHAR) + 
                             ',"rank":' + CAST((SELECT rank FROM leaderboard_snapshots WHERE game_id = @PostGameId AND CAST(ts AS DATE) = @SnapshotDate AND period = 'daily') AS NVARCHAR) +
                             ',"metrics_count":' + CAST((SELECT COUNT(*) FROM game_metric_daily WHERE game_id = @PostGameId AND date = @SnapshotDate) AS NVARCHAR) + 
                             '}';
            
            INSERT INTO post_metric_snapshot (post_id, game_id, date, index_value, details_json, created_at)
            VALUES (@NewPostId, @PostGameId, @SnapshotDate, @SnapshotIndexValue, @DetailsJson, GETUTCDATE());
        END
        
        -- 為貼文建立引用來源
        DECLARE @SourceIndex INT = 0;
        DECLARE @SourceCount INT = 1 + (ABS(CHECKSUM(NEWID())) % 3); -- 1-3個來源
        
        WHILE @SourceIndex < @SourceCount
        BEGIN
            DECLARE @SourceUrl NVARCHAR(500);
            DECLARE @SourceTitle NVARCHAR(200);
            DECLARE @SourceType NVARCHAR(50);
            
            SET @SourceType = CASE @SourceIndex % 4
                WHEN 0 THEN 'steam_stats'
                WHEN 1 THEN 'twitch_analytics'
                WHEN 2 THEN 'reddit_trends'
                ELSE 'internal_data'
            END;
            
            SET @SourceUrl = CASE @SourceType
                WHEN 'steam_stats' THEN 'https://steamcharts.com/app/' + CAST(@PostGameId + 100000 AS NVARCHAR)
                WHEN 'twitch_analytics' THEN 'https://twitchtracker.com/games/' + CAST(@PostGameId AS NVARCHAR)
                WHEN 'reddit_trends' THEN 'https://www.reddit.com/r/game' + CAST(@PostGameId AS NVARCHAR) + '/hot'
                ELSE 'internal://analytics/game/' + CAST(@PostGameId AS NVARCHAR)
            END;
            
            SET @SourceTitle = CASE @SourceType
                WHEN 'steam_stats' THEN 'Steam Charts - ' + @PostGameName
                WHEN 'twitch_analytics' THEN 'Twitch Analytics - ' + @PostGameName
                WHEN 'reddit_trends' THEN 'Reddit Trends - ' + @PostGameName
                ELSE 'Internal Analytics - ' + @PostGameName
            END;
            
            INSERT INTO post_sources (post_id, source_url, source_title, source_type, created_at)
            VALUES (@NewPostId, @SourceUrl, @SourceTitle, @SourceType, GETUTCDATE());
            
            SET @SourceIndex = @SourceIndex + 1;
        END
    END
    
    SET @PostCount = @PostCount + 1;
    SET @PostIndex = @PostIndex + 1;
END

PRINT '洞察貼文生成完成！總計: ' + CAST(@PostCount AS VARCHAR) + ' 篇貼文';

-- 統計報告
PRINT '=== 分析系統種子資料統計報告 ===';

-- 遊戲統計
SELECT 
    COUNT(*) as 總遊戲數,
    COUNT(CASE WHEN is_active = 1 THEN 1 END) as 啟用遊戲數
FROM games;

-- 指標統計
SELECT 
    ms.source_name as 指標來源,
    COUNT(m.metric_id) as 指標數量,
    COUNT(CASE WHEN m.is_active = 1 THEN 1 END) as 啟用指標數
FROM metric_sources ms
LEFT JOIN metrics m ON ms.source_id = m.source_id
GROUP BY ms.source_id, ms.source_name
ORDER BY 指標數量 DESC;

-- 數據統計
SELECT 
    COUNT(*) as 總數據點數,
    COUNT(DISTINCT game_id) as 涵蓋遊戲數,
    COUNT(DISTINCT metric_id) as 涵蓋指標數,
    COUNT(DISTINCT date) as 涵蓋天數,
    AVG(value) as 平均數值,
    MAX(value) as 最大數值
FROM game_metric_daily;

-- 熱度指數統計
SELECT 
    COUNT(*) as 總指數記錄,
    AVG(index_value) as 平均熱度指數,
    MAX(index_value) as 最高熱度指數,
    MIN(index_value) as 最低熱度指數
FROM popularity_index_daily;

-- 排行榜統計
SELECT 
    period as 榜單類型,
    COUNT(*) as 快照數量,
    COUNT(DISTINCT game_id) as 涵蓋遊戲數
FROM leaderboard_snapshots
GROUP BY period;

-- 洞察貼文統計
SELECT 
    status as 貼文狀態,
    COUNT(*) as 貼文數量,
    COUNT(CASE WHEN pinned = 1 THEN 1 END) as 置頂貼文數
FROM posts
GROUP BY status;

-- 熱門遊戲排行 (最新一日)
PRINT '最新一日熱門遊戲排行 TOP 10:';
SELECT TOP 10
    g.game_name as 遊戲名稱,
    pid.index_value as 熱度指數,
    ls.rank as 排名
FROM popularity_index_daily pid
JOIN games g ON pid.game_id = g.game_id
LEFT JOIN leaderboard_snapshots ls ON pid.game_id = ls.game_id 
    AND CAST(ls.ts AS DATE) = pid.date AND ls.period = 'daily'
WHERE pid.date = (SELECT MAX(date) FROM popularity_index_daily)
ORDER BY pid.index_value DESC;

PRINT '分析系統種子資料插入完成！';
GO