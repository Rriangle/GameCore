-- GameCore 資料庫建表腳本
-- 版本: 1.0.0
-- 建立時間: 2025年1月16日

USE master;
GO

-- 如果資料庫存在則刪除
IF EXISTS (SELECT name FROM sys.databases WHERE name = 'GameCore')
BEGIN
    ALTER DATABASE GameCore SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE GameCore;
END
GO

-- 建立新資料庫
CREATE DATABASE GameCore;
GO

USE GameCore;
GO

-- 建立用戶表
CREATE TABLE Users (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Username NVARCHAR(50) NOT NULL UNIQUE,
    Email NVARCHAR(100) NOT NULL UNIQUE,
    PasswordHash NVARCHAR(255) NOT NULL,
    Nickname NVARCHAR(50) NOT NULL,
    Avatar NVARCHAR(255),
    Level INT NOT NULL DEFAULT 1,
    Experience INT NOT NULL DEFAULT 0,
    Points INT NOT NULL DEFAULT 0,
    Coins INT NOT NULL DEFAULT 0,
    IsActive BIT NOT NULL DEFAULT 1,
    LastLoginTime DATETIME2,
    CreateTime DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    UpdateTime DATETIME2 NOT NULL DEFAULT GETUTCDATE()
);

-- 建立管理者表
CREATE TABLE Managers (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Account NVARCHAR(50) NOT NULL UNIQUE,
    PasswordHash NVARCHAR(255) NOT NULL,
    Name NVARCHAR(50) NOT NULL,
    Email NVARCHAR(100) NOT NULL,
    RoleId INT NOT NULL,
    IsActive BIT NOT NULL DEFAULT 1,
    LastLoginTime DATETIME2,
    CreateTime DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    UpdateTime DATETIME2 NOT NULL DEFAULT GETUTCDATE()
);

-- 建立管理者角色表
CREATE TABLE ManagerRoles (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(50) NOT NULL UNIQUE,
    Description NVARCHAR(255),
    IsActive BIT NOT NULL DEFAULT 1,
    CreateTime DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    UpdateTime DATETIME2 NOT NULL DEFAULT GETUTCDATE()
);

-- 建立權限表
CREATE TABLE Permissions (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(50) NOT NULL,
    Code NVARCHAR(50) NOT NULL UNIQUE,
    Description NVARCHAR(255),
    Category NVARCHAR(50) NOT NULL,
    IsActive BIT NOT NULL DEFAULT 1,
    CreateTime DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    UpdateTime DATETIME2 NOT NULL DEFAULT GETUTCDATE()
);

-- 建立角色權限關聯表
CREATE TABLE RolePermissions (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    RoleId INT NOT NULL,
    PermissionId INT NOT NULL,
    CreateTime DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    FOREIGN KEY (RoleId) REFERENCES ManagerRoles(Id),
    FOREIGN KEY (PermissionId) REFERENCES Permissions(Id)
);

-- 建立寵物表
CREATE TABLE Pets (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    UserId INT NOT NULL,
    Name NVARCHAR(50) NOT NULL,
    Type NVARCHAR(50) NOT NULL,
    Level INT NOT NULL DEFAULT 1,
    Experience INT NOT NULL DEFAULT 0,
    Hunger INT NOT NULL DEFAULT 100,
    Mood INT NOT NULL DEFAULT 100,
    Stamina INT NOT NULL DEFAULT 100,
    Cleanliness INT NOT NULL DEFAULT 100,
    Health INT NOT NULL DEFAULT 100,
    Color NVARCHAR(20) NOT NULL DEFAULT 'Default',
    IsActive BIT NOT NULL DEFAULT 1,
    CreateTime DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    UpdateTime DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    FOREIGN KEY (UserId) REFERENCES Users(Id)
);

-- 建立寵物互動記錄表
CREATE TABLE PetInteractions (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    PetId INT NOT NULL,
    UserId INT NOT NULL,
    InteractionType NVARCHAR(50) NOT NULL,
    Description NVARCHAR(255),
    CreateTime DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    FOREIGN KEY (PetId) REFERENCES Pets(Id),
    FOREIGN KEY (UserId) REFERENCES Users(Id)
);

-- 建立簽到記錄表
CREATE TABLE SignInRecords (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    UserId INT NOT NULL,
    SignInDate DATE NOT NULL,
    Points INT NOT NULL DEFAULT 0,
    Experience INT NOT NULL DEFAULT 0,
    IsWeekend BIT NOT NULL DEFAULT 0,
    IsPerfect BIT NOT NULL DEFAULT 0,
    CreateTime DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    FOREIGN KEY (UserId) REFERENCES Users(Id),
    UNIQUE(UserId, SignInDate)
);

-- 建立小遊戲記錄表
CREATE TABLE MiniGameRecords (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    UserId INT NOT NULL,
    GameType NVARCHAR(50) NOT NULL,
    Level INT NOT NULL,
    Score INT NOT NULL,
    IsWin BIT NOT NULL,
    Experience INT NOT NULL DEFAULT 0,
    Points INT NOT NULL DEFAULT 0,
    CreateTime DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    FOREIGN KEY (UserId) REFERENCES Users(Id)
);

-- 建立論壇表
CREATE TABLE Forums (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(100) NOT NULL,
    Description NVARCHAR(500),
    Category NVARCHAR(50) NOT NULL,
    IsActive BIT NOT NULL DEFAULT 1,
    CreateTime DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    UpdateTime DATETIME2 NOT NULL DEFAULT GETUTCDATE()
);

-- 建立貼文表
CREATE TABLE Posts (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    ForumId INT NOT NULL,
    UserId INT NOT NULL,
    Title NVARCHAR(200) NOT NULL,
    Content NVARCHAR(MAX) NOT NULL,
    Tags NVARCHAR(500),
    ViewCount INT NOT NULL DEFAULT 0,
    LikeCount INT NOT NULL DEFAULT 0,
    ReplyCount INT NOT NULL DEFAULT 0,
    IsSticky BIT NOT NULL DEFAULT 0,
    IsLocked BIT NOT NULL DEFAULT 0,
    IsActive BIT NOT NULL DEFAULT 1,
    CreateTime DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    UpdateTime DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    FOREIGN KEY (ForumId) REFERENCES Forums(Id),
    FOREIGN KEY (UserId) REFERENCES Users(Id)
);

-- 建立貼文回覆表
CREATE TABLE PostReplies (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    PostId INT NOT NULL,
    UserId INT NOT NULL,
    ParentReplyId INT,
    Content NVARCHAR(MAX) NOT NULL,
    LikeCount INT NOT NULL DEFAULT 0,
    IsActive BIT NOT NULL DEFAULT 1,
    CreateTime DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    UpdateTime DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    FOREIGN KEY (PostId) REFERENCES Posts(Id),
    FOREIGN KEY (UserId) REFERENCES Users(Id),
    FOREIGN KEY (ParentReplyId) REFERENCES PostReplies(Id)
);

-- 建立貼文按讚表
CREATE TABLE PostLikes (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    PostId INT NOT NULL,
    UserId INT NOT NULL,
    CreateTime DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    FOREIGN KEY (PostId) REFERENCES Posts(Id),
    FOREIGN KEY (UserId) REFERENCES Users(Id),
    UNIQUE(PostId, UserId)
);

-- 建立貼文收藏表
CREATE TABLE PostBookmarks (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    PostId INT NOT NULL,
    UserId INT NOT NULL,
    CreateTime DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    FOREIGN KEY (PostId) REFERENCES Posts(Id),
    FOREIGN KEY (UserId) REFERENCES Users(Id),
    UNIQUE(PostId, UserId)
);

-- 建立聊天室表
CREATE TABLE ChatRooms (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(100) NOT NULL,
    Description NVARCHAR(500),
    Type NVARCHAR(20) NOT NULL DEFAULT 'Group', -- Group, Private
    IsActive BIT NOT NULL DEFAULT 1,
    CreateTime DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    UpdateTime DATETIME2 NOT NULL DEFAULT GETUTCDATE()
);

-- 建立聊天室成員表
CREATE TABLE ChatRoomMembers (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    ChatRoomId INT NOT NULL,
    UserId INT NOT NULL,
    JoinTime DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    LastReadTime DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    IsActive BIT NOT NULL DEFAULT 1,
    FOREIGN KEY (ChatRoomId) REFERENCES ChatRooms(Id),
    FOREIGN KEY (UserId) REFERENCES Users(Id),
    UNIQUE(ChatRoomId, UserId)
);

-- 建立聊天訊息表
CREATE TABLE ChatMessages (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    ChatRoomId INT NOT NULL,
    UserId INT NOT NULL,
    Content NVARCHAR(MAX) NOT NULL,
    Type NVARCHAR(20) NOT NULL DEFAULT 'Text', -- Text, Image, File
    IsActive BIT NOT NULL DEFAULT 1,
    CreateTime DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    FOREIGN KEY (ChatRoomId) REFERENCES ChatRooms(Id),
    FOREIGN KEY (UserId) REFERENCES Users(Id)
);

-- 建立私聊表
CREATE TABLE PrivateChats (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    User1Id INT NOT NULL,
    User2Id INT NOT NULL,
    IsActive BIT NOT NULL DEFAULT 1,
    CreateTime DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    UpdateTime DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    FOREIGN KEY (User1Id) REFERENCES Users(Id),
    FOREIGN KEY (User2Id) REFERENCES Users(Id),
    UNIQUE(User1Id, User2Id)
);

-- 建立私聊訊息表
CREATE TABLE PrivateMessages (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    PrivateChatId INT NOT NULL,
    SenderId INT NOT NULL,
    ReceiverId INT NOT NULL,
    Content NVARCHAR(MAX) NOT NULL,
    IsRead BIT NOT NULL DEFAULT 0,
    ReadTime DATETIME2,
    CreateTime DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    FOREIGN KEY (PrivateChatId) REFERENCES PrivateChats(Id),
    FOREIGN KEY (SenderId) REFERENCES Users(Id),
    FOREIGN KEY (ReceiverId) REFERENCES Users(Id)
);

-- 建立通知表
CREATE TABLE Notifications (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    UserId INT NOT NULL,
    Type NVARCHAR(50) NOT NULL,
    Title NVARCHAR(200) NOT NULL,
    Message NVARCHAR(500) NOT NULL,
    IsRead BIT NOT NULL DEFAULT 0,
    ReadTime DATETIME2,
    SourceId INT,
    SourceType NVARCHAR(50),
    IsActive BIT NOT NULL DEFAULT 1,
    CreateTime DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    FOREIGN KEY (UserId) REFERENCES Users(Id)
);

-- 建立通知來源表
CREATE TABLE NotificationSources (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    SourceId INT NOT NULL,
    SourceType NVARCHAR(50) NOT NULL,
    Title NVARCHAR(200),
    Description NVARCHAR(500),
    CreateTime DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    UNIQUE(SourceId, SourceType)
);

-- 建立通知動作表
CREATE TABLE NotificationActions (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    ActionId INT NOT NULL,
    ActionType NVARCHAR(50) NOT NULL,
    Name NVARCHAR(100) NOT NULL,
    Description NVARCHAR(500),
    CreateTime DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    UNIQUE(ActionId, ActionType)
);

-- 建立商城商品表
CREATE TABLE StoreProducts (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(200) NOT NULL,
    Description NVARCHAR(MAX),
    Price DECIMAL(10,2) NOT NULL,
    OriginalPrice DECIMAL(10,2),
    Category NVARCHAR(50) NOT NULL,
    ImageUrl NVARCHAR(255),
    Stock INT NOT NULL DEFAULT 0,
    IsActive BIT NOT NULL DEFAULT 1,
    CreateTime DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    UpdateTime DATETIME2 NOT NULL DEFAULT GETUTCDATE()
);

-- 建立商城訂單表
CREATE TABLE StoreOrders (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    UserId INT NOT NULL,
    OrderNumber NVARCHAR(50) NOT NULL UNIQUE,
    Status NVARCHAR(20) NOT NULL DEFAULT 'Pending', -- Pending, Paid, Shipped, Delivered, Cancelled
    TotalAmount DECIMAL(10,2) NOT NULL,
    DeliveryAddress NVARCHAR(500),
    PaymentMethod NVARCHAR(50),
    PaymentTime DATETIME2,
    DeliveryTime DATETIME2,
    CreateTime DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    UpdateTime DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    FOREIGN KEY (UserId) REFERENCES Users(Id)
);

-- 建立商城訂單項目表
CREATE TABLE StoreOrderItems (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    OrderId INT NOT NULL,
    ProductId INT NOT NULL,
    Quantity INT NOT NULL,
    UnitPrice DECIMAL(10,2) NOT NULL,
    TotalPrice DECIMAL(10,2) NOT NULL,
    CreateTime DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    FOREIGN KEY (OrderId) REFERENCES StoreOrders(Id),
    FOREIGN KEY (ProductId) REFERENCES StoreProducts(Id)
);

-- 建立購物車表
CREATE TABLE ShoppingCartItems (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    UserId INT NOT NULL,
    ProductId INT NOT NULL,
    Quantity INT NOT NULL DEFAULT 1,
    CreateTime DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    UpdateTime DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    FOREIGN KEY (UserId) REFERENCES Users(Id),
    FOREIGN KEY (ProductId) REFERENCES StoreProducts(Id),
    UNIQUE(UserId, ProductId)
);

-- 建立玩家市場交易表
CREATE TABLE MarketTransactions (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    SellerId INT NOT NULL,
    BuyerId INT,
    ItemName NVARCHAR(200) NOT NULL,
    Description NVARCHAR(MAX),
    Price DECIMAL(10,2) NOT NULL,
    Quantity INT NOT NULL DEFAULT 1,
    Status NVARCHAR(20) NOT NULL DEFAULT 'Listed', -- Listed, Sold, Cancelled, Completed
    PlatformFee DECIMAL(10,2),
    IsActive BIT NOT NULL DEFAULT 1,
    CreateTime DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    UpdateTime DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    FOREIGN KEY (SellerId) REFERENCES Users(Id),
    FOREIGN KEY (BuyerId) REFERENCES Users(Id)
);

-- 建立市場評價表
CREATE TABLE MarketReviews (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    TransactionId INT NOT NULL,
    ReviewerId INT NOT NULL,
    RevieweeId INT NOT NULL,
    Rating INT NOT NULL CHECK (Rating >= 1 AND Rating <= 5),
    Comment NVARCHAR(500),
    CreateTime DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    FOREIGN KEY (TransactionId) REFERENCES MarketTransactions(Id),
    FOREIGN KEY (ReviewerId) REFERENCES Users(Id),
    FOREIGN KEY (RevieweeId) REFERENCES Users(Id)
);

-- 建立索引
CREATE INDEX IX_Users_Username ON Users(Username);
CREATE INDEX IX_Users_Email ON Users(Email);
CREATE INDEX IX_Users_Level ON Users(Level);
CREATE INDEX IX_Pets_UserId ON Pets(UserId);
CREATE INDEX IX_Pets_Level ON Pets(Level);
CREATE INDEX IX_SignInRecords_UserId ON SignInRecords(UserId);
CREATE INDEX IX_SignInRecords_SignInDate ON SignInRecords(SignInDate);
CREATE INDEX IX_MiniGameRecords_UserId ON MiniGameRecords(UserId);
CREATE INDEX IX_MiniGameRecords_GameType ON MiniGameRecords(GameType);
CREATE INDEX IX_Posts_ForumId ON Posts(ForumId);
CREATE INDEX IX_Posts_UserId ON Posts(UserId);
CREATE INDEX IX_Posts_CreateTime ON Posts(CreateTime);
CREATE INDEX IX_PostReplies_PostId ON PostReplies(PostId);
CREATE INDEX IX_PostReplies_UserId ON PostReplies(UserId);
CREATE INDEX IX_ChatMessages_ChatRoomId ON ChatMessages(ChatRoomId);
CREATE INDEX IX_ChatMessages_CreateTime ON ChatMessages(CreateTime);
CREATE INDEX IX_PrivateMessages_PrivateChatId ON PrivateMessages(PrivateChatId);
CREATE INDEX IX_PrivateMessages_CreateTime ON PrivateMessages(CreateTime);
CREATE INDEX IX_Notifications_UserId ON Notifications(UserId);
CREATE INDEX IX_Notifications_IsRead ON Notifications(IsRead);
CREATE INDEX IX_StoreProducts_Category ON StoreProducts(Category);
CREATE INDEX IX_StoreProducts_Price ON StoreProducts(Price);
CREATE INDEX IX_StoreOrders_UserId ON StoreOrders(UserId);
CREATE INDEX IX_StoreOrders_Status ON StoreOrders(Status);
CREATE INDEX IX_MarketTransactions_SellerId ON MarketTransactions(SellerId);
CREATE INDEX IX_MarketTransactions_Status ON MarketTransactions(Status);
CREATE INDEX IX_MarketTransactions_Price ON MarketTransactions(Price);

-- 建立統計視圖
GO
CREATE VIEW vw_UserStats AS
SELECT 
    u.Id,
    u.Username,
    u.Nickname,
    u.Level,
    u.Experience,
    u.Points,
    u.Coins,
    (SELECT COUNT(*) FROM Pets p WHERE p.UserId = u.Id AND p.IsActive = 1) AS PetCount,
    (SELECT COUNT(*) FROM SignInRecords s WHERE s.UserId = u.Id) AS SignInDays,
    (SELECT COUNT(*) FROM Posts p WHERE p.UserId = u.Id AND p.IsActive = 1) AS PostCount,
    (SELECT COUNT(*) FROM StoreOrders o WHERE o.UserId = u.Id) AS OrderCount
FROM Users u;

GO
CREATE VIEW vw_ForumStats AS
SELECT 
    f.Id,
    f.Name,
    f.Category,
    (SELECT COUNT(*) FROM Posts p WHERE p.ForumId = f.Id AND p.IsActive = 1) AS PostCount,
    (SELECT COUNT(*) FROM PostReplies pr 
     JOIN Posts p ON pr.PostId = p.Id 
     WHERE p.ForumId = f.Id AND pr.IsActive = 1) AS ReplyCount,
    (SELECT COUNT(*) FROM Posts p WHERE p.ForumId = f.Id AND p.IsActive = 1 AND p.CreateTime >= DATEADD(day, -7, GETUTCDATE())) AS RecentPosts
FROM Forums f;

GO
CREATE VIEW vw_MarketStats AS
SELECT 
    'Total' AS Metric,
    COUNT(*) AS Value
FROM MarketTransactions
WHERE Status = 'Completed'
UNION ALL
SELECT 
    'Total Volume' AS Metric,
    SUM(Price) AS Value
FROM MarketTransactions
WHERE Status = 'Completed'
UNION ALL
SELECT 
    'Platform Fees' AS Metric,
    SUM(ISNULL(PlatformFee, 0)) AS Value
FROM MarketTransactions
WHERE Status = 'Completed';

GO
PRINT 'GameCore 資料庫建表完成！';
PRINT '建立時間: ' + CONVERT(NVARCHAR, GETUTCDATE(), 120);
PRINT '資料表數量: 25';
PRINT '索引數量: 25';
PRINT '視圖數量: 3';