-- =============================================
-- GameCore 社交通知系統種子資料
-- 建立完整的通知、聊天、群組、封鎖測試資料
-- 嚴格按照規格要求生成符合業務邏輯的完整社交系統資料
-- =============================================

USE GameCore;
GO

PRINT '開始插入社交通知系統種子資料...';

-- 清除現有的社交相關記錄 (按照外鍵順序)
PRINT '清除現有社交資料...';
DELETE FROM Group_Block;
DELETE FROM Group_Chat;
DELETE FROM Group_Member;
DELETE FROM Groups;
DELETE FROM Chat_Message;
DELETE FROM Notification_Recipients;
DELETE FROM Notifications;
DELETE FROM Notification_Actions WHERE action_id > 0;
DELETE FROM Notification_Sources WHERE source_id > 0;

-- 重置自增ID
DBCC CHECKIDENT ('Notification_Sources', RESEED, 0);
DBCC CHECKIDENT ('Notification_Actions', RESEED, 0);
DBCC CHECKIDENT ('Notifications', RESEED, 0);
DBCC CHECKIDENT ('Notification_Recipients', RESEED, 0);
DBCC CHECKIDENT ('Chat_Message', RESEED, 0);
DBCC CHECKIDENT ('Groups', RESEED, 0);
DBCC CHECKIDENT ('Group_Chat', RESEED, 0);
DBCC CHECKIDENT ('Group_Block', RESEED, 0);

PRINT '開始生成通知來源和行為字典...';

-- 建立通知來源字典
INSERT INTO Notification_Sources (source_name) VALUES
('系統'),
('論壇'),
('商店'),
('玩家市場'),
('寵物'),
('小遊戲'),
('每日簽到'),
('群組'),
('私訊'),
('錢包'),
('管理員');

-- 建立通知行為字典
INSERT INTO Notification_Actions (action_name) VALUES
('系統公告'),
('新回覆'),
('新主題'),
('被讚'),
('被收藏'),
('訂單更新'),
('交易完成'),
('寵物升級'),
('遊戲獎勵'),
('簽到獎勵'),
('群組邀請'),
('加入群組'),
('退出群組'),
('新訊息'),
('餘額變動'),
('警告'),
('封鎖'),
('解封');

PRINT '通知字典建立完成！';

-- 生成通知資料
PRINT '開始生成通知資料...';

DECLARE @SourceCount INT = (SELECT COUNT(*) FROM Notification_Sources);
DECLARE @ActionCount INT = (SELECT COUNT(*) FROM Notification_Actions);
DECLARE @NotificationCount INT = 0;

-- 為每個使用者生成 10-20 個通知
DECLARE @UserId INT = 1;
DECLARE @MaxUserId INT = 50;

WHILE @UserId <= @MaxUserId
BEGIN
    PRINT '為使用者 ' + CAST(@UserId AS VARCHAR) + ' 生成通知...';
    
    -- 每個使用者隨機生成 10-20 個通知
    DECLARE @UserNotificationCount INT = 10 + (ABS(CHECKSUM(NEWID())) % 11); -- 10-20個通知
    DECLARE @NotificationIndex INT = 0;
    
    WHILE @NotificationIndex < @UserNotificationCount
    BEGIN
        -- 隨機選擇來源和行為
        DECLARE @SourceId INT = 1 + (ABS(CHECKSUM(NEWID())) % @SourceCount);
        DECLARE @ActionId INT = 1 + (ABS(CHECKSUM(NEWID())) % @ActionCount);
        
        -- 隨機選擇發送者 (可能是系統或其他使用者)
        DECLARE @SenderId INT = NULL;
        DECLARE @SenderManagerId INT = NULL;
        
        -- 70% 機率是使用者發送，30% 機率是管理員/系統發送
        IF (ABS(CHECKSUM(NEWID())) % 100) < 70
        BEGIN
            -- 隨機選擇其他使用者作為發送者
            SELECT TOP 1 @SenderId = User_ID 
            FROM Users 
            WHERE User_ID != @UserId AND User_ID <= 50 
            ORDER BY NEWID();
        END
        ELSE
        BEGIN
            -- 選擇管理員作為發送者
            SET @SenderManagerId = 1 + (ABS(CHECKSUM(NEWID())) % 3); -- 假設有3個管理員
        END
        
        -- 生成通知標題和內容
        DECLARE @NotificationTitle NVARCHAR(200);
        DECLARE @NotificationMessage NVARCHAR(1000);
        DECLARE @TitleType INT = ABS(CHECKSUM(NEWID())) % 10;
        
        SET @NotificationTitle = CASE @TitleType
            WHEN 0 THEN '論壇新回覆通知'
            WHEN 1 THEN '商店訂單更新'
            WHEN 2 THEN '玩家市場交易通知'
            WHEN 3 THEN '寵物狀態提醒'
            WHEN 4 THEN '小遊戲獎勵發送'
            WHEN 5 THEN '每日簽到獎勵'
            WHEN 6 THEN '群組活動通知'
            WHEN 7 THEN '新私訊'
            WHEN 8 THEN '錢包餘額變動'
            ELSE '系統公告'
        END;
        
        SET @NotificationMessage = CASE @TitleType
            WHEN 0 THEN '您的論壇主題收到了新的回覆，快去查看其他玩家的精彩回應吧！'
            WHEN 1 THEN '您的商店訂單狀態已更新，請查看最新的物流資訊和配送進度。'
            WHEN 2 THEN '恭喜！您的玩家市場交易已完成，點數已成功轉入您的錢包。'
            WHEN 3 THEN '您的寵物需要照顧了！記得餵食、洗澡和陪玩，保持寵物的健康狀態。'
            WHEN 4 THEN '小遊戲挑戰成功！您獲得了經驗值和點數獎勵，繼續加油！'
            WHEN 5 THEN '每日簽到完成！您獲得了簽到獎勵，連續簽到可獲得更多獎勵。'
            WHEN 6 THEN '您參與的群組有新的活動和討論，快來參與群組互動吧！'
            WHEN 7 THEN '您收到了一條新的私訊，快去查看朋友發送的訊息內容。'
            WHEN 8 THEN '您的錢包餘額發生變動，詳細資訊請查看交易記錄。'
            ELSE '系統重要公告：遊戲版本更新，新增多項功能和優化，快來體驗吧！'
        END;
        
        -- 生成通知建立時間 (最近30天內)
        DECLARE @NotificationCreatedAt DATETIME2 = DATEADD(DAY, -(ABS(CHECKSUM(NEWID())) % 30), GETUTCDATE());
        
        -- 插入通知記錄
        INSERT INTO Notifications (
            source_id, action_id, sender_id, sender_manager_id,
            notification_title, notification_message, created_at
        )
        VALUES (
            @SourceId, @ActionId, @SenderId, @SenderManagerId,
            @NotificationTitle, @NotificationMessage, @NotificationCreatedAt
        );
        
        DECLARE @NewNotificationId INT = SCOPE_IDENTITY();
        
        -- 插入通知接收者記錄
        DECLARE @IsRead BIT = CASE WHEN (ABS(CHECKSUM(NEWID())) % 100) < 30 THEN 1 ELSE 0 END; -- 30% 已讀
        DECLARE @ReadAt DATETIME2 = CASE WHEN @IsRead = 1 THEN DATEADD(HOUR, 1 + (ABS(CHECKSUM(NEWID())) % 48), @NotificationCreatedAt) ELSE NULL END;
        
        INSERT INTO Notification_Recipients (
            notification_id, recipient_id, is_read, read_at, received_at
        )
        VALUES (
            @NewNotificationId, @UserId, @IsRead, @ReadAt, @NotificationCreatedAt
        );
        
        SET @NotificationCount = @NotificationCount + 1;
        SET @NotificationIndex = @NotificationIndex + 1;
    END
    
    SET @UserId = @UserId + 1;
END

PRINT '通知資料生成完成！總計: ' + CAST(@NotificationCount AS VARCHAR) + ' 個通知';

-- 生成聊天訊息資料
PRINT '開始生成聊天訊息資料...';

DECLARE @ChatMessageCount INT = 0;
DECLARE @Sender INT = 1;

WHILE @Sender <= 30 -- 前30個使用者參與聊天
BEGIN
    -- 每個使用者與 2-5 個其他使用者聊天
    DECLARE @ChatPartnerCount INT = 2 + (ABS(CHECKSUM(NEWID())) % 4); -- 2-5個聊天對象
    DECLARE @PartnerIndex INT = 0;
    
    WHILE @PartnerIndex < @ChatPartnerCount
    BEGIN
        -- 隨機選擇聊天對象
        DECLARE @Receiver INT;
        SELECT TOP 1 @Receiver = User_ID 
        FROM Users 
        WHERE User_ID != @Sender AND User_ID <= 50 
        ORDER BY NEWID();
        
        -- 檢查是否已存在對話 (避免重複)
        IF NOT EXISTS (
            SELECT 1 FROM Chat_Message 
            WHERE (sender_id = @Sender AND receiver_id = @Receiver) 
               OR (sender_id = @Receiver AND receiver_id = @Sender)
        )
        BEGIN
            -- 生成 3-10 條聊天訊息
            DECLARE @MessageCount INT = 3 + (ABS(CHECKSUM(NEWID())) % 8); -- 3-10條訊息
            DECLARE @MessageIndex INT = 0;
            DECLARE @ConversationStartTime DATETIME2 = DATEADD(DAY, -(ABS(CHECKSUM(NEWID())) % 15), GETUTCDATE());
            
            WHILE @MessageIndex < @MessageCount
            BEGIN
                -- 交替發送者和接收者
                DECLARE @CurrentSender INT = CASE WHEN @MessageIndex % 2 = 0 THEN @Sender ELSE @Receiver END;
                DECLARE @CurrentReceiver INT = CASE WHEN @MessageIndex % 2 = 0 THEN @Receiver ELSE @Sender END;
                
                -- 生成聊天內容
                DECLARE @ChatContent NVARCHAR(1000);
                DECLARE @ContentType INT = ABS(CHECKSUM(NEWID())) % 8;
                
                SET @ChatContent = CASE @ContentType
                    WHEN 0 THEN '嗨！最近在玩什麼遊戲呢？我剛完成了一個很困難的關卡！'
                    WHEN 1 THEN '謝謝你昨天在論壇的幫助，你的攻略真的很有用！'
                    WHEN 2 THEN '你看到最新的活動了嗎？獎勵看起來很不錯呢！'
                    WHEN 3 THEN '我的寵物剛升級了！你的寵物現在幾級了？'
                    WHEN 4 THEN '想組隊一起玩小遊戲嗎？我覺得合作會更有趣！'
                    WHEN 5 THEN '今天簽到了嗎？記得保持連續簽到獲得更多獎勵哦！'
                    WHEN 6 THEN '商店有新商品上架，你有什麼推薦的嗎？'
                    ELSE '哈哈，遊戲真的很有趣！我們可以經常交流心得。'
                END;
                
                -- 生成訊息時間
                DECLARE @MessageSentAt DATETIME2 = DATEADD(MINUTE, @MessageIndex * 15 + (ABS(CHECKSUM(NEWID())) % 60), @ConversationStartTime);
                
                -- 隨機決定是否已讀
                DECLARE @MessageIsRead BIT = CASE WHEN (ABS(CHECKSUM(NEWID())) % 100) < 80 THEN 1 ELSE 0 END; -- 80% 已讀
                
                -- 插入聊天訊息
                INSERT INTO Chat_Message (
                    sender_id, receiver_id, chat_content, sent_at, is_read, is_sent
                )
                VALUES (
                    @CurrentSender, @CurrentReceiver, @ChatContent, @MessageSentAt, @MessageIsRead, 1
                );
                
                SET @ChatMessageCount = @ChatMessageCount + 1;
                SET @MessageIndex = @MessageIndex + 1;
            END
        END
        
        SET @PartnerIndex = @PartnerIndex + 1;
    END
    
    SET @Sender = @Sender + 1;
END

PRINT '聊天訊息生成完成！總計: ' + CAST(@ChatMessageCount AS VARCHAR) + ' 條訊息';

-- 生成群組資料
PRINT '開始生成群組資料...';

DECLARE @GroupCount INT = 0;
DECLARE @GroupIndex INT = 0;

-- 生成 8-12 個群組
DECLARE @TotalGroupCount INT = 8 + (ABS(CHECKSUM(NEWID())) % 5); -- 8-12個群組

WHILE @GroupIndex < @TotalGroupCount
BEGIN
    -- 隨機選擇群組建立者
    DECLARE @GroupCreator INT;
    SELECT TOP 1 @GroupCreator = User_ID 
    FROM Users 
    WHERE User_ID <= 50 
    ORDER BY NEWID();
    
    -- 生成群組名稱
    DECLARE @GroupName NVARCHAR(100);
    DECLARE @GroupNameType INT = ABS(CHECKSUM(NEWID())) % 8;
    
    SET @GroupName = CASE @GroupNameType
        WHEN 0 THEN '遊戲攻略討論群'
        WHEN 1 THEN '新手互助群組'
        WHEN 2 THEN '高手交流圈'
        WHEN 3 THEN '寵物愛好者聯盟'
        WHEN 4 THEN '每日簽到提醒群'
        WHEN 5 THEN '商店情報分享'
        WHEN 6 THEN '玩家市場交流'
        ELSE '休閒聊天群'
    END + ' #' + CAST(@GroupIndex + 1 AS VARCHAR);
    
    -- 生成群組建立時間
    DECLARE @GroupCreatedAt DATETIME2 = DATEADD(DAY, -(ABS(CHECKSUM(NEWID())) % 60), GETUTCDATE());
    
    -- 插入群組記錄
    INSERT INTO Groups (group_name, created_by, created_at)
    VALUES (@GroupName, @GroupCreator, @GroupCreatedAt);
    
    DECLARE @NewGroupId INT = SCOPE_IDENTITY();
    
    -- 建立者自動成為群組管理員
    INSERT INTO Group_Member (group_id, user_id, joined_at, is_admin)
    VALUES (@NewGroupId, @GroupCreator, @GroupCreatedAt, 1);
    
    -- 隨機添加 5-15 個群組成員
    DECLARE @GroupMemberCount INT = 5 + (ABS(CHECKSUM(NEWID())) % 11); -- 5-15個成員
    DECLARE @MemberIndex INT = 0;
    
    WHILE @MemberIndex < @GroupMemberCount
    BEGIN
        -- 隨機選擇群組成員
        DECLARE @GroupMember INT;
        SELECT TOP 1 @GroupMember = User_ID 
        FROM Users 
        WHERE User_ID != @GroupCreator AND User_ID <= 50 
        AND NOT EXISTS (
            SELECT 1 FROM Group_Member 
            WHERE group_id = @NewGroupId AND user_id = User_ID
        )
        ORDER BY NEWID();
        
        IF @GroupMember IS NOT NULL
        BEGIN
            -- 隨機決定是否為管理員 (10% 機率)
            DECLARE @IsGroupAdmin BIT = CASE WHEN (ABS(CHECKSUM(NEWID())) % 100) < 10 THEN 1 ELSE 0 END;
            
            -- 生成加入時間
            DECLARE @MemberJoinedAt DATETIME2 = DATEADD(DAY, (ABS(CHECKSUM(NEWID())) % 30), @GroupCreatedAt);
            
            -- 插入群組成員記錄
            INSERT INTO Group_Member (group_id, user_id, joined_at, is_admin)
            VALUES (@NewGroupId, @GroupMember, @MemberJoinedAt, @IsGroupAdmin);
        END
        
        SET @MemberIndex = @MemberIndex + 1;
    END
    
    SET @GroupCount = @GroupCount + 1;
    SET @GroupIndex = @GroupIndex + 1;
END

PRINT '群組資料生成完成！總計: ' + CAST(@GroupCount AS VARCHAR) + ' 個群組';

-- 生成群組聊天資料
PRINT '開始生成群組聊天資料...';

DECLARE @GroupChatCount INT = 0;
DECLARE @GroupId INT = 1;
DECLARE @MaxGroupId INT = (SELECT MAX(group_id) FROM Groups);

WHILE @GroupId <= @MaxGroupId
BEGIN
    -- 每個群組生成 10-30 條聊天記錄
    DECLARE @GroupChatMessageCount INT = 10 + (ABS(CHECKSUM(NEWID())) % 21); -- 10-30條訊息
    DECLARE @GroupChatIndex INT = 0;
    
    -- 獲取群組建立時間
    DECLARE @GroupCreatedTime DATETIME2;
    SELECT @GroupCreatedTime = created_at FROM Groups WHERE group_id = @GroupId;
    
    WHILE @GroupChatIndex < @GroupChatMessageCount
    BEGIN
        -- 隨機選擇群組成員作為發送者
        DECLARE @GroupChatSender INT;
        SELECT TOP 1 @GroupChatSender = user_id 
        FROM Group_Member 
        WHERE group_id = @GroupId 
        ORDER BY NEWID();
        
        -- 生成群組聊天內容
        DECLARE @GroupChatContent NVARCHAR(1000);
        DECLARE @GroupContentType INT = ABS(CHECKSUM(NEWID())) % 10;
        
        SET @GroupChatContent = CASE @GroupContentType
            WHEN 0 THEN '大家好！很高興加入這個群組，希望能和大家多多交流！'
            WHEN 1 THEN '今天有什麼新的遊戲心得可以分享嗎？我剛發現一個很有趣的技巧！'
            WHEN 2 THEN '看到最新的更新公告了嗎？新功能看起來很棒呢！'
            WHEN 3 THEN '有人想組隊一起玩嗎？我現在有空，可以一起挑戰困難關卡！'
            WHEN 4 THEN '分享一個實用攻略：記得每天簽到和照顧寵物，獎勵很豐富！'
            WHEN 5 THEN '商店有限時特價活動，大家有興趣的可以去看看！'
            WHEN 6 THEN '我的寵物終於升級了！大家的寵物都幾級了？'
            WHEN 7 THEN '玩家市場有什麼好東西嗎？最近想買一些裝備。'
            WHEN 8 THEN '感謝群組管理員的辛苦管理，讓我們有這麼好的交流環境！'
            ELSE '哈哈，這個遊戲真的很有趣！和大家一起玩更開心！'
        END;
        
        -- 生成聊天時間
        DECLARE @GroupChatSentAt DATETIME2 = DATEADD(HOUR, @GroupChatIndex * 2 + (ABS(CHECKSUM(NEWID())) % 12), @GroupCreatedTime);
        
        -- 插入群組聊天記錄
        INSERT INTO Group_Chat (group_id, sender_id, group_chat_content, sent_at, is_sent)
        VALUES (@GroupId, @GroupChatSender, @GroupChatContent, @GroupChatSentAt, 1);
        
        SET @GroupChatCount = @GroupChatCount + 1;
        SET @GroupChatIndex = @GroupChatIndex + 1;
    END
    
    SET @GroupId = @GroupId + 1;
END

PRINT '群組聊天生成完成！總計: ' + CAST(@GroupChatCount AS VARCHAR) + ' 條群組訊息';

-- 生成群組封鎖資料
PRINT '開始生成群組封鎖資料...';

DECLARE @GroupBlockCount INT = 0;
DECLARE @BlockGroupId INT = 1;

WHILE @BlockGroupId <= @MaxGroupId
BEGIN
    -- 每個群組隨機生成 0-3 個封鎖記錄
    DECLARE @GroupBlockRecordCount INT = ABS(CHECKSUM(NEWID())) % 4; -- 0-3個封鎖
    DECLARE @BlockIndex INT = 0;
    
    WHILE @BlockIndex < @GroupBlockRecordCount
    BEGIN
        -- 隨機選擇被封鎖者 (非管理員)
        DECLARE @BlockedUser INT;
        SELECT TOP 1 @BlockedUser = user_id 
        FROM Group_Member 
        WHERE group_id = @BlockGroupId AND is_admin = 0
        AND NOT EXISTS (
            SELECT 1 FROM Group_Block 
            WHERE group_id = @BlockGroupId AND user_id = Group_Member.user_id
        )
        ORDER BY NEWID();
        
        IF @BlockedUser IS NOT NULL
        BEGIN
            -- 隨機選擇封鎖者 (管理員)
            DECLARE @BlockedByUser INT;
            SELECT TOP 1 @BlockedByUser = user_id 
            FROM Group_Member 
            WHERE group_id = @BlockGroupId AND is_admin = 1 
            ORDER BY NEWID();
            
            -- 插入封鎖記錄
            INSERT INTO Group_Block (group_id, user_id, blocked_by, created_at)
            VALUES (@BlockGroupId, @BlockedUser, @BlockedByUser, GETUTCDATE());
            
            SET @GroupBlockCount = @GroupBlockCount + 1;
        END
        
        SET @BlockIndex = @BlockIndex + 1;
    END
    
    SET @BlockGroupId = @BlockGroupId + 1;
END

PRINT '群組封鎖生成完成！總計: ' + CAST(@GroupBlockCount AS VARCHAR) + ' 個封鎖記錄';

-- 統計報告
PRINT '=== 社交通知系統種子資料統計報告 ===';

-- 通知統計
PRINT '通知統計:';
SELECT 
    ns.source_name as 通知來源,
    na.action_name as 通知行為,
    COUNT(n.notification_id) as 通知數量,
    COUNT(CASE WHEN nr.is_read = 1 THEN 1 END) as 已讀數量,
    COUNT(CASE WHEN nr.is_read = 0 THEN 1 END) as 未讀數量
FROM Notifications n
JOIN Notification_Sources ns ON n.source_id = ns.source_id
JOIN Notification_Actions na ON n.action_id = na.action_id
JOIN Notification_Recipients nr ON n.notification_id = nr.notification_id
GROUP BY ns.source_name, na.action_name
ORDER BY 通知數量 DESC;

-- 聊天統計
PRINT '聊天統計:';
SELECT 
    COUNT(*) as 總訊息數,
    COUNT(CASE WHEN is_read = 1 THEN 1 END) as 已讀訊息數,
    COUNT(CASE WHEN is_read = 0 THEN 1 END) as 未讀訊息數,
    COUNT(DISTINCT sender_id) as 發送者數量,
    COUNT(DISTINCT receiver_id) as 接收者數量
FROM Chat_Message;

-- 群組統計
PRINT '群組統計:';
SELECT 
    COUNT(*) as 總群組數,
    AVG(CAST(成員數 AS FLOAT)) as 平均成員數,
    MAX(成員數) as 最大成員數,
    MIN(成員數) as 最小成員數
FROM (
    SELECT 
        g.group_id,
        COUNT(gm.user_id) as 成員數
    FROM Groups g
    LEFT JOIN Group_Member gm ON g.group_id = gm.group_id
    GROUP BY g.group_id
) as GroupStats;

-- 群組聊天統計
PRINT '群組聊天統計:';
SELECT 
    COUNT(*) as 總群組訊息數,
    COUNT(DISTINCT group_id) as 有訊息的群組數,
    COUNT(DISTINCT sender_id) as 發送者數量,
    AVG(CAST(每群組訊息數 AS FLOAT)) as 平均每群組訊息數
FROM (
    SELECT 
        group_id,
        COUNT(*) as 每群組訊息數
    FROM Group_Chat
    GROUP BY group_id
) as GroupChatStats;

-- 封鎖統計
PRINT '封鎖統計:';
SELECT 
    COUNT(*) as 總封鎖數,
    COUNT(DISTINCT group_id) as 有封鎖的群組數,
    COUNT(DISTINCT user_id) as 被封鎖使用者數,
    COUNT(DISTINCT blocked_by) as 執行封鎖的管理員數
FROM Group_Block;

-- 活躍度統計
PRINT '使用者活躍度 TOP 10:';
SELECT TOP 10
    u.User_name as 使用者名稱,
    COUNT(DISTINCT n.notification_id) as 收到通知數,
    COUNT(DISTINCT cm_sent.message_id) as 發送訊息數,
    COUNT(DISTINCT cm_received.message_id) as 接收訊息數,
    COUNT(DISTINCT gm.group_id) as 參與群組數,
    COUNT(DISTINCT gc.group_chat_id) as 群組發言數
FROM Users u
LEFT JOIN Notification_Recipients nr ON u.User_ID = nr.recipient_id
LEFT JOIN Notifications n ON nr.notification_id = n.notification_id
LEFT JOIN Chat_Message cm_sent ON u.User_ID = cm_sent.sender_id
LEFT JOIN Chat_Message cm_received ON u.User_ID = cm_received.receiver_id
LEFT JOIN Group_Member gm ON u.User_ID = gm.user_id
LEFT JOIN Group_Chat gc ON u.User_ID = gc.sender_id
WHERE u.User_ID <= 50
GROUP BY u.User_ID, u.User_name
ORDER BY (COUNT(DISTINCT n.notification_id) + COUNT(DISTINCT cm_sent.message_id) + COUNT(DISTINCT gm.group_id)) DESC;

PRINT '社交通知系統種子資料插入完成！';
GO