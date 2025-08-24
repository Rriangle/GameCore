-- =============================================
-- GameCore 論壇系統種子資料
-- 建立完整的論壇測試資料，包含版面、主題、回覆、反應、收藏等
-- 嚴格按照規格要求生成符合業務邏輯的完整論壇資料
-- =============================================

USE GameCore;
GO

PRINT '開始插入論壇系統種子資料...';

-- 清除現有的論壇相關記錄 (按照外鍵順序)
PRINT '清除現有論壇資料...';
DELETE FROM bookmarks WHERE target_type IN ('thread', 'thread_post', 'forum');
DELETE FROM reactions WHERE target_type IN ('thread', 'thread_post');
DELETE FROM thread_posts;
DELETE FROM threads;
DELETE FROM forums WHERE game_id IS NOT NULL;

-- 重置自增ID
DBCC CHECKIDENT ('forums', RESEED, 0);
DBCC CHECKIDENT ('threads', RESEED, 0);
DBCC CHECKIDENT ('thread_posts', RESEED, 0);
DBCC CHECKIDENT ('reactions', RESEED, 0);
DBCC CHECKIDENT ('bookmarks', RESEED, 0);

PRINT '開始生成論壇版面資料...';

-- 為每個遊戲建立論壇版面 (每遊戲一版面，game_id unique)
INSERT INTO forums (game_id, forum_name, forum_description, is_active, sort_order, created_at, updated_at, thread_count, post_count, view_count)
SELECT 
    g.game_id,
    g.game_name + ' 討論區' as forum_name,
    '歡迎來到 ' + g.game_name + ' 的官方討論區！在這裡您可以與其他玩家交流遊戲心得、分享攻略技巧、討論遊戲更新等。請遵守社群規範，共同營造良好的討論環境。' as forum_description,
    1 as is_active,
    ROW_NUMBER() OVER (ORDER BY g.game_id) as sort_order,
    GETUTCDATE() as created_at,
    GETUTCDATE() as updated_at,
    0 as thread_count,
    0 as post_count,
    0 as view_count
FROM games g
WHERE g.game_id <= 20; -- 限制前20個遊戲

PRINT '論壇版面建立完成！';

-- 生成討論主題
PRINT '開始生成討論主題資料...';

DECLARE @ForumId INT = 1;
DECLARE @MaxForumId INT = (SELECT MAX(forum_id) FROM forums WHERE game_id IS NOT NULL);
DECLARE @ThreadCount INT = 0;

WHILE @ForumId <= @MaxForumId
BEGIN
    PRINT '為論壇 ' + CAST(@ForumId AS VARCHAR) + ' 建立主題...';
    
    -- 每個論壇隨機生成 15-25 個主題
    DECLARE @ForumThreadCount INT = 15 + (ABS(CHECKSUM(NEWID())) % 11); -- 15-25個主題
    DECLARE @ThreadIndex INT = 0;
    
    WHILE @ThreadIndex < @ForumThreadCount
    BEGIN
        -- 隨機選擇作者
        DECLARE @AuthorId INT;
        SELECT TOP 1 @AuthorId = User_ID 
        FROM Users 
        WHERE User_ID <= 50 
        ORDER BY NEWID();
        
        -- 生成主題標題
        DECLARE @ThreadTitle NVARCHAR(200);
        DECLARE @TitleType INT = ABS(CHECKSUM(NEWID())) % 8;
        
        SET @ThreadTitle = CASE @TitleType
            WHEN 0 THEN '【攻略分享】新手必看！完整攻略指南'
            WHEN 1 THEN '【心得討論】遊戲體驗感想與建議'
            WHEN 2 THEN '【BUG回報】發現遊戲異常狀況'
            WHEN 3 THEN '【活動討論】最新活動介紹與心得'
            WHEN 4 THEN '【玩家交流】尋找隊友一起遊戲'
            WHEN 5 THEN '【建議反饋】希望遊戲能夠改進的地方'
            WHEN 6 THEN '【炫耀時刻】分享遊戲成就與收穫'
            ELSE '【閒聊討論】關於遊戲的各種話題'
        END;
        
        -- 隨機決定主題狀態
        DECLARE @ThreadStatus NVARCHAR(20);
        DECLARE @StatusRand INT = ABS(CHECKSUM(NEWID())) % 100;
        
        SET @ThreadStatus = CASE 
            WHEN @StatusRand < 85 THEN 'normal'      -- 85% 正常
            WHEN @StatusRand < 95 THEN 'hidden'      -- 10% 隱藏
            WHEN @StatusRand < 98 THEN 'archived'    -- 3% 封存
            ELSE 'deleted'                           -- 2% 已刪除
        END;
        
        -- 生成主題建立時間 (最近60天內)
        DECLARE @ThreadCreatedAt DATETIME2 = DATEADD(DAY, -(ABS(CHECKSUM(NEWID())) % 60), GETUTCDATE());
        
        -- 插入主題記錄
        INSERT INTO threads (
            forum_id, author_user_id, title, status, created_at, updated_at
        )
        VALUES (
            @ForumId, @AuthorId, @ThreadTitle, @ThreadStatus, @ThreadCreatedAt, @ThreadCreatedAt
        );
        
        DECLARE @NewThreadId BIGINT = SCOPE_IDENTITY();
        
        SET @ThreadCount = @ThreadCount + 1;
        SET @ThreadIndex = @ThreadIndex + 1;
    END
    
    SET @ForumId = @ForumId + 1;
END

PRINT '討論主題生成完成！總計: ' + CAST(@ThreadCount AS VARCHAR) + ' 個主題';

-- 生成回覆資料
PRINT '開始生成回覆資料...';

DECLARE @ThreadId BIGINT = 1;
DECLARE @MaxThreadId BIGINT = (SELECT MAX(thread_id) FROM threads);
DECLARE @PostCount INT = 0;

WHILE @ThreadId <= @MaxThreadId
BEGIN
    -- 只為正常狀態的主題生成回覆
    DECLARE @ThreadStatus NVARCHAR(20);
    SELECT @ThreadStatus = status FROM threads WHERE thread_id = @ThreadId;
    
    IF @ThreadStatus = 'normal'
    BEGIN
        -- 每個主題隨機生成 0-15 個回覆
        DECLARE @ThreadPostCount INT = ABS(CHECKSUM(NEWID())) % 16; -- 0-15個回覆
        DECLARE @PostIndex INT = 0;
        
        WHILE @PostIndex < @ThreadPostCount
        BEGIN
            -- 隨機選擇回覆者
            DECLARE @PostAuthorId INT;
            SELECT TOP 1 @PostAuthorId = User_ID 
            FROM Users 
            WHERE User_ID <= 50 
            ORDER BY NEWID();
            
            -- 生成回覆內容
            DECLARE @PostContent NVARCHAR(MAX);
            DECLARE @ContentType INT = ABS(CHECKSUM(NEWID())) % 6;
            
            SET @PostContent = CASE @ContentType
                WHEN 0 THEN '感謝樓主的分享！這個內容很有用，我也遇到過類似的情況。根據我的經驗，還可以補充一點：建議大家在實際操作時要注意細節，有時候一些小地方的差異會影響最終結果。'
                
                WHEN 1 THEN '樓主說得很對！我也有同樣的感受。特別是你提到的那幾個要點，確實是新手容易忽略的地方。我之前就是沒注意這些，走了不少彎路。'
                
                WHEN 2 THEN '有不同的看法想和大家討論一下。雖然樓主的方法很好，但我覺得還有其他的解決方案。比如可以嘗試另一種方式，可能效果會更好。'
                
                WHEN 3 THEN '正好需要這方面的資訊！樓主分享得很及時。我想請教一個相關問題：在具體實施過程中，如果遇到特殊情況應該怎麼處理？'
                
                WHEN 4 THEN '支持樓主！這種分享精神值得讚揚。我也來補充一些相關的經驗：除了樓主提到的方法外，還可以關注一些細節優化。'
                
                ELSE '很實用的內容！已經收藏了，準備照著試試看。有一個小建議：希望樓主有時間的話可以補充一些具體的操作截圖。'
            END;
            
            -- 隨機決定回覆狀態
            DECLARE @PostStatus NVARCHAR(20);
            DECLARE @PostStatusRand INT = ABS(CHECKSUM(NEWID())) % 100;
            
            SET @PostStatus = CASE 
                WHEN @PostStatusRand < 90 THEN 'normal'      -- 90% 正常
                WHEN @PostStatusRand < 95 THEN 'hidden'      -- 5% 隱藏
                WHEN @PostStatusRand < 98 THEN 'archived'    -- 3% 封存
                ELSE 'deleted'                               -- 2% 已刪除
            END;
            
            -- 隨機決定是否為父回覆 (20% 機率)
            DECLARE @ParentPostId BIGINT = NULL;
            IF @PostIndex > 0 AND (ABS(CHECKSUM(NEWID())) % 100) < 20
            BEGIN
                -- 選擇之前的回覆作為父回覆
                SELECT TOP 1 @ParentPostId = id 
                FROM thread_posts 
                WHERE thread_id = @ThreadId AND parent_post_id IS NULL
                ORDER BY NEWID();
            END
            
            -- 生成回覆時間 (在主題建立時間之後)
            DECLARE @ThreadCreatedTime DATETIME2;
            SELECT @ThreadCreatedTime = created_at FROM threads WHERE thread_id = @ThreadId;
            
            DECLARE @PostCreatedAt DATETIME2 = DATEADD(HOUR, @PostIndex * 2 + (ABS(CHECKSUM(NEWID())) % 24), @ThreadCreatedTime);
            
            -- 插入回覆記錄
            INSERT INTO thread_posts (
                thread_id, author_user_id, content_md, parent_post_id, status, created_at, updated_at
            )
            VALUES (
                @ThreadId, @PostAuthorId, @PostContent, @ParentPostId, @PostStatus, @PostCreatedAt, @PostCreatedAt
            );
            
            SET @PostCount = @PostCount + 1;
            SET @PostIndex = @PostIndex + 1;
        END
    END
    
    SET @ThreadId = @ThreadId + 1;
END

PRINT '回覆資料生成完成！總計: ' + CAST(@PostCount AS VARCHAR) + ' 個回覆';

-- 生成反應資料
PRINT '開始生成反應資料...';

DECLARE @ReactionCount INT = 0;
DECLARE @CurrentThreadId BIGINT = 1;

WHILE @CurrentThreadId <= @MaxThreadId
BEGIN
    -- 為主題生成反應 (50% 機率)
    IF (ABS(CHECKSUM(NEWID())) % 100) < 50
    BEGIN
        -- 每個主題 1-5 個反應
        DECLARE @ThreadReactionCount INT = 1 + (ABS(CHECKSUM(NEWID())) % 5);
        DECLARE @ReactionIndex INT = 0;
        
        WHILE @ReactionIndex < @ThreadReactionCount
        BEGIN
            -- 隨機選擇反應者
            DECLARE @ReactionUserId INT;
            SELECT TOP 1 @ReactionUserId = User_ID 
            FROM Users 
            WHERE User_ID <= 50 
            ORDER BY NEWID();
            
            -- 隨機選擇反應類型
            DECLARE @ReactionKind NVARCHAR(20);
            DECLARE @ReactionTypeRand INT = ABS(CHECKSUM(NEWID())) % 6;
            
            SET @ReactionKind = CASE @ReactionTypeRand
                WHEN 0 THEN 'like'
                WHEN 1 THEN 'love'
                WHEN 2 THEN 'laugh'
                WHEN 3 THEN 'wow'
                WHEN 4 THEN 'sad'
                ELSE 'angry'
            END;
            
            -- 檢查是否已存在相同的反應 (去重)
            IF NOT EXISTS (
                SELECT 1 FROM reactions 
                WHERE target_type = 'thread' AND target_id = @CurrentThreadId 
                AND user_id = @ReactionUserId AND kind = @ReactionKind
            )
            BEGIN
                INSERT INTO reactions (target_type, target_id, user_id, kind, created_at)
                VALUES ('thread', @CurrentThreadId, @ReactionUserId, @ReactionKind, GETUTCDATE());
                
                SET @ReactionCount = @ReactionCount + 1;
            END
            
            SET @ReactionIndex = @ReactionIndex + 1;
        END
    END
    
    -- 為回覆生成反應
    DECLARE @PostId BIGINT;
    DECLARE post_cursor CURSOR FOR 
        SELECT id FROM thread_posts WHERE thread_id = @CurrentThreadId AND status = 'normal';
    
    OPEN post_cursor;
    FETCH NEXT FROM post_cursor INTO @PostId;
    
    WHILE @@FETCH_STATUS = 0
    BEGIN
        -- 30% 機率為回覆生成反應
        IF (ABS(CHECKSUM(NEWID())) % 100) < 30
        BEGIN
            -- 每個回覆 1-3 個反應
            DECLARE @PostReactionCount INT = 1 + (ABS(CHECKSUM(NEWID())) % 3);
            DECLARE @PostReactionIndex INT = 0;
            
            WHILE @PostReactionIndex < @PostReactionCount
            BEGIN
                -- 隨機選擇反應者
                SELECT TOP 1 @ReactionUserId = User_ID 
                FROM Users 
                WHERE User_ID <= 50 
                ORDER BY NEWID();
                
                -- 隨機選擇反應類型
                SET @ReactionTypeRand = ABS(CHECKSUM(NEWID())) % 6;
                SET @ReactionKind = CASE @ReactionTypeRand
                    WHEN 0 THEN 'like'
                    WHEN 1 THEN 'love'
                    WHEN 2 THEN 'laugh'
                    WHEN 3 THEN 'wow'
                    WHEN 4 THEN 'sad'
                    ELSE 'angry'
                END;
                
                -- 檢查是否已存在相同的反應 (去重)
                IF NOT EXISTS (
                    SELECT 1 FROM reactions 
                    WHERE target_type = 'thread_post' AND target_id = @PostId 
                    AND user_id = @ReactionUserId AND kind = @ReactionKind
                )
                BEGIN
                    INSERT INTO reactions (target_type, target_id, user_id, kind, created_at)
                    VALUES ('thread_post', @PostId, @ReactionUserId, @ReactionKind, GETUTCDATE());
                    
                    SET @ReactionCount = @ReactionCount + 1;
                END
                
                SET @PostReactionIndex = @PostReactionIndex + 1;
            END
        END
        
        FETCH NEXT FROM post_cursor INTO @PostId;
    END
    
    CLOSE post_cursor;
    DEALLOCATE post_cursor;
    
    SET @CurrentThreadId = @CurrentThreadId + 1;
END

PRINT '反應資料生成完成！總計: ' + CAST(@ReactionCount AS VARCHAR) + ' 個反應';

-- 生成收藏資料
PRINT '開始生成收藏資料...';

DECLARE @BookmarkCount INT = 0;
DECLARE @UserId INT = 1;
DECLARE @MaxUserId INT = 50;

WHILE @UserId <= @MaxUserId
BEGIN
    -- 每個使用者隨機收藏 0-8 個項目
    DECLARE @UserBookmarkCount INT = ABS(CHECKSUM(NEWID())) % 9; -- 0-8個收藏
    DECLARE @BookmarkIndex INT = 0;
    
    WHILE @BookmarkIndex < @UserBookmarkCount
    BEGIN
        -- 隨機選擇收藏類型
        DECLARE @BookmarkTargetType NVARCHAR(20);
        DECLARE @BookmarkTargetId BIGINT;
        DECLARE @BookmarkTargetTypeRand INT = ABS(CHECKSUM(NEWID())) % 4;
        
        IF @BookmarkTargetTypeRand = 0 -- 收藏主題
        BEGIN
            SET @BookmarkTargetType = 'thread';
            SELECT TOP 1 @BookmarkTargetId = thread_id 
            FROM threads 
            WHERE status = 'normal' 
            ORDER BY NEWID();
        END
        ELSE IF @BookmarkTargetTypeRand = 1 -- 收藏回覆
        BEGIN
            SET @BookmarkTargetType = 'thread_post';
            SELECT TOP 1 @BookmarkTargetId = id 
            FROM thread_posts 
            WHERE status = 'normal' 
            ORDER BY NEWID();
        END
        ELSE IF @BookmarkTargetTypeRand = 2 -- 收藏版面
        BEGIN
            SET @BookmarkTargetType = 'forum';
            SELECT TOP 1 @BookmarkTargetId = forum_id 
            FROM forums 
            WHERE is_active = 1 AND game_id IS NOT NULL
            ORDER BY NEWID();
        END
        ELSE -- 收藏遊戲
        BEGIN
            SET @BookmarkTargetType = 'game';
            SELECT TOP 1 @BookmarkTargetId = game_id 
            FROM games 
            ORDER BY NEWID();
        END
        
        -- 檢查是否已存在相同的收藏 (去重)
        IF @BookmarkTargetId IS NOT NULL AND NOT EXISTS (
            SELECT 1 FROM bookmarks 
            WHERE target_type = @BookmarkTargetType AND target_id = @BookmarkTargetId AND user_id = @UserId
        )
        BEGIN
            INSERT INTO bookmarks (target_type, target_id, user_id, created_at)
            VALUES (@BookmarkTargetType, @BookmarkTargetId, @UserId, GETUTCDATE());
            
            SET @BookmarkCount = @BookmarkCount + 1;
        END
        
        SET @BookmarkIndex = @BookmarkIndex + 1;
    END
    
    SET @UserId = @UserId + 1;
END

PRINT '收藏資料生成完成！總計: ' + CAST(@BookmarkCount AS VARCHAR) + ' 個收藏';

-- 更新統計數據
PRINT '更新論壇統計數據...';

-- 更新論壇的統計數據
UPDATE forums 
SET 
    thread_count = (
        SELECT ISNULL(COUNT(*), 0) 
        FROM threads 
        WHERE forum_id = forums.forum_id AND status = 'normal'
    ),
    post_count = (
        SELECT ISNULL(COUNT(*), 0) 
        FROM thread_posts tp
        JOIN threads t ON tp.thread_id = t.thread_id
        WHERE t.forum_id = forums.forum_id AND tp.status = 'normal'
    ),
    updated_at = GETUTCDATE()
WHERE game_id IS NOT NULL;

PRINT '統計數據更新完成！';

-- 統計報告
PRINT '=== 論壇系統種子資料統計報告 ===';

-- 總論壇數量
DECLARE @TotalForums INT = (SELECT COUNT(*) FROM forums WHERE game_id IS NOT NULL);
PRINT '總論壇數量: ' + CAST(@TotalForums AS VARCHAR);

-- 主題統計
PRINT '主題統計:';
SELECT 
    status as 主題狀態,
    COUNT(*) as 主題數量
FROM threads
GROUP BY status
ORDER BY 主題數量 DESC;

-- 回覆統計
PRINT '回覆統計:';
SELECT 
    status as 回覆狀態,
    COUNT(*) as 回覆數量,
    COUNT(CASE WHEN parent_post_id IS NOT NULL THEN 1 END) as 子回覆數量
FROM thread_posts
GROUP BY status
ORDER BY 回覆數量 DESC;

-- 反應統計
PRINT '反應統計:';
SELECT 
    kind as 反應類型,
    COUNT(*) as 反應數量,
    target_type as 目標類型
FROM reactions
GROUP BY kind, target_type
ORDER BY 反應數量 DESC;

-- 收藏統計
PRINT '收藏統計:';
SELECT 
    target_type as 收藏類型,
    COUNT(*) as 收藏數量
FROM bookmarks
GROUP BY target_type
ORDER BY 收藏數量 DESC;

PRINT '論壇系統種子資料插入完成！';
GO