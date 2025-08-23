-- GameCore 資料庫種子資料腳本
-- 版本: 1.0.0
-- 建立時間: 2025年1月16日

USE GameCore;
GO

-- 清空現有資料（可選）
-- 注意：在生產環境中請謹慎使用
-- DELETE FROM MarketReviews;
-- DELETE FROM MarketTransactions;
-- DELETE FROM ShoppingCartItems;
-- DELETE FROM StoreOrderItems;
-- DELETE FROM StoreOrders;
-- DELETE FROM StoreProducts;
-- DELETE FROM PostBookmarks;
-- DELETE FROM PostLikes;
-- DELETE FROM PostReplies;
-- DELETE FROM Posts;
-- DELETE FROM Forums;
-- DELETE FROM PrivateMessages;
-- DELETE FROM PrivateChats;
-- DELETE FROM ChatMessages;
-- DELETE FROM ChatRoomMembers;
-- DELETE FROM ChatRooms;
-- DELETE FROM Notifications;
-- DELETE FROM NotificationActions;
-- DELETE FROM NotificationSources;
-- DELETE FROM MiniGameRecords;
-- DELETE FROM SignInRecords;
-- DELETE FROM PetInteractions;
-- DELETE FROM Pets;
-- DELETE FROM RolePermissions;
-- DELETE FROM Permissions;
-- DELETE FROM ManagerRoles;
-- DELETE FROM Managers;
-- DELETE FROM Users;

-- 插入權限資料
INSERT INTO Permissions (Name, Code, Description, Category) VALUES
('用戶管理', 'USER_MANAGE', '管理用戶帳號、狀態、權限', 'User'),
('寵物管理', 'PET_MANAGE', '管理寵物系統、設定', 'Pet'),
('商城管理', 'STORE_MANAGE', '管理官方商城商品、訂單', 'Store'),
('市場管理', 'MARKET_MANAGE', '管理玩家市場、交易', 'Market'),
('論壇管理', 'FORUM_MANAGE', '管理論壇、貼文、回覆', 'Forum'),
('聊天管理', 'CHAT_MANAGE', '管理聊天室、私聊', 'Chat'),
('通知管理', 'NOTIFICATION_MANAGE', '管理系統通知', 'Notification'),
('系統設定', 'SYSTEM_CONFIG', '系統參數設定', 'System'),
('數據統計', 'DATA_STATS', '查看統計數據、報表', 'Statistics'),
('內容審核', 'CONTENT_MODERATE', '審核用戶生成內容', 'Moderation');

-- 插入管理者角色
INSERT INTO ManagerRoles (Name, Description) VALUES
('超級管理員', '擁有所有權限的系統管理員'),
('內容管理員', '負責內容審核和用戶管理'),
('商城管理員', '負責商城和市場管理'),
('客服管理員', '負責客服和通知管理'),
('數據分析員', '負責數據統計和分析');

-- 插入角色權限關聯
-- 超級管理員：所有權限
INSERT INTO RolePermissions (RoleId, PermissionId)
SELECT 1, Id FROM Permissions;

-- 內容管理員：用戶、寵物、論壇、內容審核
INSERT INTO RolePermissions (RoleId, PermissionId)
SELECT 2, Id FROM Permissions WHERE Code IN ('USER_MANAGE', 'PET_MANAGE', 'FORUM_MANAGE', 'CONTENT_MODERATE');

-- 商城管理員：商城、市場、數據統計
INSERT INTO RolePermissions (RoleId, PermissionId)
SELECT 3, Id FROM Permissions WHERE Code IN ('STORE_MANAGE', 'MARKET_MANAGE', 'DATA_STATS');

-- 客服管理員：聊天、通知、用戶管理
INSERT INTO RolePermissions (RoleId, PermissionId)
SELECT 4, Id FROM Permissions WHERE Code IN ('CHAT_MANAGE', 'NOTIFICATION_MANAGE', 'USER_MANAGE');

-- 數據分析員：數據統計、內容審核
INSERT INTO RolePermissions (RoleId, PermissionId)
SELECT 5, Id FROM Permissions WHERE Code IN ('DATA_STATS', 'CONTENT_MODERATE');

-- 插入管理者帳號
INSERT INTO Managers (Account, PasswordHash, Name, Email, RoleId) VALUES
('admin', '$2a$11$92IXUNpkjO0rOQ5byMi.Ye4oKoEa3Ro9llC/.og/at2.uheWG/igi', '系統管理員', 'admin@gamecore.com', 1),
('content', '$2a$11$92IXUNpkjO0rOQ5byMi.Ye4oKoEa3Ro9llC/.og/at2.uheWG/igi', '內容管理員', 'content@gamecore.com', 2),
('store', '$2a$11$92IXUNpkjO0rOQ5byMi.Ye4oKoEa3Ro9llC/.og/at2.uheWG/igi', '商城管理員', 'store@gamecore.com', 3),
('service', '$2a$11$92IXUNpkjO0rOQ5byMi.Ye4oKoEa3Ro9llC/.og/at2.uheWG/igi', '客服管理員', 'service@gamecore.com', 4),
('analyst', '$2a$11$92IXUNpkjO0rOQ5byMi.Ye4oKoEa3Ro9llC/.og/at2.uheWG/igi', '數據分析員', 'analyst@gamecore.com', 5);

-- 插入測試用戶
INSERT INTO Users (Username, Email, PasswordHash, Nickname, Avatar, Level, Experience, Points, Coins) VALUES
('testuser1', 'user1@test.com', '$2a$11$92IXUNpkjO0rOQ5byMi.Ye4oKoEa3Ro9llC/.og/at2.uheWG/igi', '測試用戶1', '/avatars/user1.jpg', 5, 1250, 500, 1000),
('testuser2', 'user2@test.com', '$2a$11$92IXUNpkjO0rOQ5byMi.Ye4oKoEa3Ro9llC/.og/at2.uheWG/igi', '測試用戶2', '/avatars/user2.jpg', 3, 800, 300, 500),
('testuser3', 'user3@test.com', '$2a$11$92IXUNpkjO0rOQ5byMi.Ye4oKoEa3Ro9llC/.og/at2.uheWG/igi', '測試用戶3', '/avatars/user3.jpg', 7, 2100, 800, 1500),
('seller1', 'seller1@test.com', '$2a$11$92IXUNpkjO0rOQ5byMi.Ye4oKoEa3Ro9llC/.og/at2.uheWG/igi', '賣家1', '/avatars/seller1.jpg', 10, 5000, 2000, 5000),
('buyer1', 'buyer1@test.com', '$2a$11$92IXUNpkjO0rOQ5byMi.Ye4oKoEa3Ro9llC/.og/at2.uheWG/igi', '買家1', '/avatars/buyer1.jpg', 6, 1800, 700, 1200);

-- 插入寵物資料
INSERT INTO Pets (UserId, Name, Type, Level, Experience, Hunger, Mood, Stamina, Cleanliness, Health, Color) VALUES
(1, '小可愛', '貓', 5, 1250, 85, 90, 80, 95, 88, '橘色'),
(1, '小寶貝', '狗', 3, 800, 90, 85, 75, 90, 92, '黑色'),
(2, '小天使', '兔子', 3, 800, 88, 92, 85, 88, 90, '白色'),
(3, '小勇士', '龍', 7, 2100, 75, 80, 70, 85, 78, '紅色'),
(4, '小商人', '狐狸', 10, 5000, 95, 90, 85, 92, 95, '金色');

-- 插入寵物互動記錄
INSERT INTO PetInteractions (PetId, UserId, InteractionType, Description) VALUES
(1, 1, '餵食', '餵食寵物，飢餓值+15'),
(1, 1, '玩耍', '與寵物玩耍，心情值+10'),
(2, 1, '清潔', '清潔寵物，清潔值+12'),
(3, 2, '訓練', '訓練寵物，體力值+8'),
(4, 3, '治療', '治療寵物，健康值+20');

-- 插入簽到記錄
INSERT INTO SignInRecords (UserId, SignInDate, Points, Experience, IsWeekend, IsPerfect) VALUES
(1, DATEADD(day, -1, GETDATE()), 10, 20, 0, 1),
(1, DATEADD(day, -2, GETDATE()), 10, 20, 0, 1),
(1, DATEADD(day, -3, GETDATE()), 10, 20, 0, 1),
(2, DATEADD(day, -1, GETDATE()), 10, 20, 0, 1),
(2, DATEADD(day, -2, GETDATE()), 10, 20, 0, 1),
(3, DATEADD(day, -1, GETDATE()), 10, 20, 0, 1);

-- 插入小遊戲記錄
INSERT INTO MiniGameRecords (UserId, GameType, Level, Score, IsWin, Experience, Points) VALUES
(1, '冒險', 1, 1500, 1, 50, 20),
(1, '冒險', 2, 2200, 1, 60, 25),
(2, '冒險', 1, 1800, 1, 55, 22),
(3, '冒險', 3, 3000, 1, 80, 35),
(4, '冒險', 5, 4500, 1, 100, 50);

-- 插入論壇資料
INSERT INTO Forums (Name, Description, Category) VALUES
('遊戲討論', '討論各種遊戲相關話題', '遊戲'),
('技術交流', '程式設計、技術分享', '技術'),
('生活分享', '日常生活、心情分享', '生活'),
('交易市場', '遊戲道具、帳號交易', '交易'),
('新手專區', '新手指導、問題解答', '新手');

-- 插入貼文資料
INSERT INTO Posts (ForumId, UserId, Title, Content, Tags, ViewCount, LikeCount, ReplyCount) VALUES
(1, 1, '推薦一款超好玩的RPG遊戲', '最近發現了一款超棒的RPG遊戲，畫面精美，劇情豐富...', 'RPG,推薦,遊戲', 150, 25, 8),
(1, 2, '大家最喜歡的遊戲類型是什麼？', '想了解一下大家最喜歡玩什麼類型的遊戲，我自己比較喜歡策略類...', '討論,遊戲類型,投票', 89, 12, 15),
(2, 3, 'Unity開發心得分享', '使用Unity開發遊戲已經兩年了，想分享一下心得和經驗...', 'Unity,開發,心得', 234, 45, 23),
(3, 4, '今天遇到一件有趣的事', '今天在捷運上遇到一個很友善的陌生人，讓我心情很好...', '生活,分享,心情', 67, 18, 5),
(4, 5, '出售遊戲帳號', '因為工作忙碌，想出售一個遊戲帳號，有意者請私訊...', '交易,帳號,出售', 189, 3, 12);

-- 插入貼文回覆
INSERT INTO PostReplies (PostId, UserId, Content, LikeCount) VALUES
(1, 2, '這款遊戲我也玩過，確實很棒！', 5),
(1, 3, '感謝推薦，馬上去試試看', 3),
(2, 1, '我比較喜歡動作類遊戲', 7),
(2, 4, '策略類遊戲我也很愛', 4),
(3, 1, 'Unity確實很強大，感謝分享', 8);

-- 插入貼文按讚
INSERT INTO PostLikes (PostId, UserId) VALUES
(1, 2), (1, 3), (1, 4), (1, 5),
(2, 1), (2, 3), (2, 4),
(3, 1), (3, 2), (3, 4), (3, 5),
(4, 1), (4, 2), (4, 3),
(5, 1), (5, 2);

-- 插入貼文收藏
INSERT INTO PostBookmarks (PostId, UserId) VALUES
(1, 2), (1, 3),
(2, 1), (2, 4),
(3, 1), (3, 2), (3, 5),
(4, 1), (4, 3),
(5, 2), (5, 4);

-- 插入聊天室資料
INSERT INTO ChatRooms (Name, Description, Type) VALUES
('遊戲交流群', '遊戲玩家交流群組', 'Group'),
('技術討論群', '程式開發技術討論', 'Group'),
('新手互助群', '新手指導和互助', 'Group'),
('交易群', '遊戲道具交易群組', 'Group');

-- 插入聊天室成員
INSERT INTO ChatRoomMembers (ChatRoomId, UserId) VALUES
(1, 1), (1, 2), (1, 3), (1, 4), (1, 5),
(2, 1), (2, 3), (2, 4),
(3, 1), (3, 2), (3, 5),
(4, 2), (4, 4), (4, 5);

-- 插入聊天訊息
INSERT INTO ChatMessages (ChatRoomId, UserId, Content, Type) VALUES
(1, 1, '大家好！歡迎來到遊戲交流群', 'Text'),
(1, 2, '大家好，我是新手', 'Text'),
(1, 3, '歡迎歡迎！', 'Text'),
(2, 1, '有人用過Unity嗎？', 'Text'),
(2, 3, '我用過，有什麼問題嗎？', 'Text');

-- 插入私聊資料
INSERT INTO PrivateChats (User1Id, User2Id) VALUES
(1, 2), (1, 3), (2, 3), (4, 5);

-- 插入私聊訊息
INSERT INTO PrivateMessages (PrivateChatId, SenderId, ReceiverId, Content, IsRead) VALUES
(1, 1, 2, '你好！', 1),
(1, 2, 1, '你好！很高興認識你', 0),
(2, 1, 3, '想請教一下Unity的問題', 1),
(2, 3, 1, '好的，請說', 0),
(3, 2, 3, '有空一起玩遊戲嗎？', 1),
(3, 3, 2, '好啊！', 0);

-- 插入通知來源
INSERT INTO NotificationSources (SourceId, SourceType, Title, Description) VALUES
(1, 'Post', '貼文通知', '論壇貼文相關通知'),
(2, 'Chat', '聊天通知', '聊天相關通知'),
(3, 'Market', '市場通知', '玩家市場相關通知'),
(4, 'Store', '商城通知', '官方商城相關通知'),
(5, 'System', '系統通知', '系統相關通知');

-- 插入通知動作
INSERT INTO NotificationActions (ActionId, ActionType, Name, Description) VALUES
(1, 'Like', '按讚', '用戶按讚通知'),
(2, 'Reply', '回覆', '用戶回覆通知'),
(3, 'Follow', '關注', '用戶關注通知'),
(4, 'Purchase', '購買', '商品購買通知'),
(5, 'Message', '訊息', '新訊息通知');

-- 插入通知資料
INSERT INTO Notifications (UserId, Type, Title, Message, SourceId, SourceType) VALUES
(1, 'Like', '有人按讚了你的貼文', 'testuser2 按讚了你的貼文「推薦一款超好玩的RPG遊戲」', 1, 'Post'),
(1, 'Reply', '有人回覆了你的貼文', 'testuser3 回覆了你的貼文「推薦一款超好玩的RPG遊戲」', 1, 'Post'),
(2, 'Like', '有人按讚了你的貼文', 'testuser1 按讚了你的貼文「大家最喜歡的遊戲類型是什麼？」', 1, 'Post'),
(3, 'Like', '有人按讚了你的貼文', 'testuser1 按讚了你的貼文「Unity開發心得分享」', 1, 'Post'),
(4, 'Message', '你有新的私聊訊息', 'testuser1 發送了一條私聊訊息給你', 2, 'Chat');

-- 插入商城商品
INSERT INTO StoreProducts (Name, Description, Price, OriginalPrice, Category, ImageUrl, Stock) VALUES
('遊戲點數卡 100點', '可在遊戲內使用的點數卡，面額100點', 100.00, 120.00, '點數卡', '/images/points100.jpg', 999),
('遊戲點數卡 500點', '可在遊戲內使用的點數卡，面額500點', 450.00, 500.00, '點數卡', '/images/points500.jpg', 500),
('遊戲點數卡 1000點', '可在遊戲內使用的點數卡，面額1000點', 850.00, 1000.00, '點數卡', '/images/points1000.jpg', 200),
('限定寵物造型', '限時販售的稀有寵物造型', 299.00, 399.00, '造型', '/images/pet_skin1.jpg', 50),
('遊戲道具包', '包含多種實用遊戲道具的組合包', 199.00, 250.00, '道具', '/images/item_pack1.jpg', 100),
('VIP會員月卡', '享受VIP特權的月費會員卡', 299.00, 399.00, '會員', '/images/vip_month.jpg', 200),
('遊戲周邊商品', '精美的遊戲周邊商品', 599.00, 699.00, '周邊', '/images/merchandise1.jpg', 30),
('數位遊戲序號', '熱門遊戲的數位序號', 1299.00, 1499.00, '遊戲', '/images/game_code1.jpg', 150);

-- 插入商城訂單
INSERT INTO StoreOrders (UserId, OrderNumber, Status, TotalAmount, DeliveryAddress, PaymentMethod) VALUES
(1, 'ORD20250116001', 'Delivered', 100.00, '台北市信義區信義路五段7號', '信用卡'),
(2, 'ORD20250116002', 'Paid', 450.00, '台北市大安區忠孝東路四段1號', '信用卡'),
(3, 'ORD20250116003', 'Pending', 299.00, '台北市中山區中山北路二段1號', '信用卡'),
(4, 'ORD20250116004', 'Shipped', 199.00, '台北市松山區敦化北路1號', '信用卡'),
(5, 'ORD20250116005', 'Delivered', 299.00, '台北市內湖區內湖路一段1號', '信用卡');

-- 插入商城訂單項目
INSERT INTO StoreOrderItems (OrderId, ProductId, Quantity, UnitPrice, TotalPrice) VALUES
(1, 1, 1, 100.00, 100.00),
(2, 2, 1, 450.00, 450.00),
(3, 4, 1, 299.00, 299.00),
(4, 5, 1, 199.00, 199.00),
(5, 6, 1, 299.00, 299.00);

-- 插入購物車項目
INSERT INTO ShoppingCartItems (UserId, ProductId, Quantity) VALUES
(1, 3, 1),
(1, 7, 1),
(2, 4, 1),
(3, 6, 1),
(4, 8, 1);

-- 插入玩家市場交易
INSERT INTO MarketTransactions (SellerId, BuyerId, ItemName, Description, Price, Quantity, Status, PlatformFee) VALUES
(4, 5, '稀有寵物蛋', '限時活動獲得的稀有寵物蛋，孵化後可獲得特殊寵物', 500.00, 1, 'Completed', 25.00),
(4, 1, '遊戲道具套裝', '包含多種實用道具的套裝組合', 300.00, 1, 'Completed', 15.00),
(5, 2, '限定造型', '活動限定的稀有造型，錯過就沒有了', 800.00, 1, 'Listed', NULL),
(1, 3, '遊戲帳號', '高等級遊戲帳號，包含多種稀有道具', 2000.00, 1, 'Listed', NULL),
(2, 4, '寵物進化材料', '寵物進化所需的稀有材料', 150.00, 5, 'Listed', NULL);

-- 插入市場評價
INSERT INTO MarketReviews (TransactionId, ReviewerId, RevieweeId, Rating, Comment) VALUES
(1, 5, 4, 5, '賣家很友善，交易很順利，商品品質很好！'),
(1, 4, 5, 5, '買家很準時，付款很快，很棒的買家！'),
(2, 1, 4, 4, '商品符合描述，包裝也很用心，推薦！'),
(2, 4, 1, 5, '買家很爽快，溝通很愉快，謝謝！');

-- 更新統計數據
UPDATE Posts SET ReplyCount = (SELECT COUNT(*) FROM PostReplies WHERE PostId = Posts.Id);
UPDATE Posts SET LikeCount = (SELECT COUNT(*) FROM PostLikes WHERE PostId = Posts.Id);

-- 建立統計視圖的索引
CREATE INDEX IX_vw_UserStats_Level ON vw_UserStats(Level);
CREATE INDEX IX_vw_ForumStats_PostCount ON vw_ForumStats(PostCount);
CREATE INDEX IX_vw_MarketStats_Metric ON vw_MarketStats(Metric);

GO

-- 輸出統計資訊
PRINT '=== GameCore 資料庫種子資料插入完成 ===';
PRINT '建立時間: ' + CONVERT(NVARCHAR, GETUTCDATE(), 120);
PRINT '';

-- 統計各表資料量
SELECT 'Users' AS TableName, COUNT(*) AS RecordCount FROM Users
UNION ALL
SELECT 'Pets', COUNT(*) FROM Pets
UNION ALL
SELECT 'Forums', COUNT(*) FROM Forums
UNION ALL
SELECT 'Posts', COUNT(*) FROM Posts
UNION ALL
SELECT 'StoreProducts', COUNT(*) FROM StoreProducts
UNION ALL
SELECT 'MarketTransactions', COUNT(*) FROM MarketTransactions
UNION ALL
SELECT 'ChatRooms', COUNT(*) FROM ChatRooms
UNION ALL
SELECT 'Notifications', COUNT(*) FROM Notifications;

PRINT '';
PRINT '✅ 種子資料插入完成！';
PRINT '✅ 測試帳號已建立：';
PRINT '   - 管理員: admin/admin (超級管理員)';
PRINT '   - 用戶: testuser1/admin, testuser2/admin, testuser3/admin';
PRINT '   - 賣家: seller1/admin, buyer1/admin';
PRINT '✅ 測試資料已準備就緒，可以開始測試系統功能！';