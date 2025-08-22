-- =====================================================
-- GameCore 資料庫建立腳本
-- 嚴格按照提供的資料庫結構建立，包含詳細中文註釋
-- 建立日期: 2025年1月
-- =====================================================

-- 建立資料庫
IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = N'GameCore')
BEGIN
    CREATE DATABASE GameCore;
    PRINT 'GameCore 資料庫建立成功';
END
ELSE
BEGIN
    PRINT 'GameCore 資料庫已存在';
END
GO

USE GameCore;
GO

-- =====================================================
-- 使用者相關資料表
-- =====================================================

-- 使用者基本資料表
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Users' AND xtype='U')
BEGIN
    CREATE TABLE Users (
        User_ID int IDENTITY(1,1) PRIMARY KEY, -- 使用者編號 (主鍵, 自動遞增)
        User_name nvarchar(100) NOT NULL UNIQUE, -- 使用者姓名 (唯一約束)
        User_Account nvarchar(100) NOT NULL UNIQUE, -- 登入帳號 (唯一約束)
        User_Password nvarchar(255) NOT NULL -- 使用者密碼 (雜湊儲存)
    );
    PRINT '✓ Users 資料表建立完成';
END

-- 使用者介紹資料表
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='User_Introduce' AND xtype='U')
BEGIN
    CREATE TABLE User_Introduce (
        User_ID int PRIMARY KEY, -- 使用者編號 (主鍵, 外鍵到 Users)
        User_NickName NVARCHAR(50) NOT NULL UNIQUE, -- 使用者暱稱 (唯一約束)
        Gender CHAR(1) NOT NULL, -- 性別
        IdNumber varchar(20) NOT NULL UNIQUE, -- 身分證字號 (唯一約束)
        Cellphone varchar(20) NOT NULL UNIQUE, -- 聯繫電話 (唯一約束)
        Email nvarchar(100) NOT NULL UNIQUE, -- 電子郵件 (唯一約束)
        Address nvarchar(200) NOT NULL, -- 地址
        DateOfBirth date NOT NULL, -- 出生年月日
        Create_Account datetime2 NOT NULL DEFAULT GETUTCDATE(), -- 創建帳號日期 (預設 UTC 時間)
        User_Picture varbinary(max) NULL, -- 頭像圖片 (二進位資料, 最大值)
        User_Introduce NVARCHAR(200) NULL, -- 使用者自介
        CONSTRAINT FK_UserIntroduce_Users FOREIGN KEY (User_ID) REFERENCES Users(User_ID) ON DELETE CASCADE
    );
    PRINT '✓ User_Introduce 資料表建立完成';
END

-- 使用者權限資料表
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='User_Rights' AND xtype='U')
BEGIN
    CREATE TABLE User_Rights (
        User_Id int PRIMARY KEY, -- 使用者編號 (主鍵, 外鍵到 Users)
        User_Status bit NULL DEFAULT 1, -- 使用者狀態 (啟用/停權, 預設啟用)
        ShoppingPermission bit NULL DEFAULT 1, -- 購物權限 (預設允許)
        MessagePermission bit NULL DEFAULT 1, -- 留言權限 (預設允許)
        SalesAuthority bit NULL DEFAULT 0, -- 銷售權限 (預設不允許)
        CONSTRAINT FK_UserRights_Users FOREIGN KEY (User_Id) REFERENCES Users(User_ID) ON DELETE CASCADE
    );
    PRINT '✓ User_Rights 資料表建立完成';
END

-- 使用者錢包資料表
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='User_wallet' AND xtype='U')
BEGIN
    CREATE TABLE User_wallet (
        User_Id int PRIMARY KEY, -- 使用者編號 (主鍵, 外鍵到 Users)
        User_Point int NULL DEFAULT 0, -- 使用者點數 (預設 0)
        Coupon_Number VARCHAR(50) NULL, -- 優惠券編號
        CONSTRAINT FK_UserWallet_Users FOREIGN KEY (User_Id) REFERENCES Users(User_ID) ON DELETE CASCADE
    );
    PRINT '✓ User_wallet 資料表建立完成';
END

-- 開通銷售功能資料表
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='MemberSalesProfile' AND xtype='U')
BEGIN
    CREATE TABLE MemberSalesProfile (
        User_Id int PRIMARY KEY, -- 使用者編號 (主鍵, 外鍵到 Users)
        BankCode int NULL, -- 銀行代號
        BankAccountNumber varchar(50) NULL, -- 銀行帳號
        AccountCoverPhoto varbinary(max) NULL, -- 帳戶封面照片
        CONSTRAINT FK_MemberSalesProfile_Users FOREIGN KEY (User_Id) REFERENCES Users(User_ID) ON DELETE CASCADE
    );
    PRINT '✓ MemberSalesProfile 資料表建立完成';
END

-- 使用者銷售資訊資料表
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='User_Sales_Information' AND xtype='U')
BEGIN
    CREATE TABLE User_Sales_Information (
        User_Id int PRIMARY KEY, -- 使用者編號 (主鍵, 外鍵到 Users)
        UserSales_Wallet int NULL DEFAULT 0, -- 使用者銷售錢包 (預設 0)
        CONSTRAINT FK_UserSalesInformation_Users FOREIGN KEY (User_Id) REFERENCES Users(User_ID) ON DELETE CASCADE
    );
    PRINT '✓ User_Sales_Information 資料表建立完成';
END

-- =====================================================
-- 管理者相關資料表
-- =====================================================

-- 管理者資料表 (主表)
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='ManagerData' AND xtype='U')
BEGIN
    CREATE TABLE ManagerData (
        Manager_Id INT IDENTITY(1,1) PRIMARY KEY, -- 管理者編號 (主鍵, 自動遞增)
        Manager_Name NVARCHAR(100) NULL, -- 管理者姓名
        Manager_Account VARCHAR(100) NULL UNIQUE, -- 管理者帳號 (建議唯一)
        Manager_Password NVARCHAR(255) NULL, -- 管理者密碼 (實務請存雜湊)
        Administrator_registration_date datetime2 NULL DEFAULT GETUTCDATE() -- 管理者註冊時間
    );
    PRINT '✓ ManagerData 資料表建立完成';
END

-- 角色權限定義資料表
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='ManagerRolePermission' AND xtype='U')
BEGIN
    CREATE TABLE ManagerRolePermission (
        ManagerRole_Id INT IDENTITY(1,1) PRIMARY KEY, -- 管理者角色編號 (主鍵)
        role_name NVARCHAR(100) NOT NULL, -- 顯示名稱 (例如: 商城管理員)
        AdministratorPrivilegesManagement bit NULL DEFAULT 0, -- 管理者權限管理
        UserStatusManagement bit NULL DEFAULT 0, -- 使用者狀態管理
        ShoppingPermissionManagement bit NULL DEFAULT 0, -- 商城權限管理
        MessagePermissionManagement bit NULL DEFAULT 0, -- 論壇權限管理
        Pet_Rights_Management bit NULL DEFAULT 0, -- 寵物權限管理
        customer_service bit NULL DEFAULT 0 -- 客服權限管理
    );
    PRINT '✓ ManagerRolePermission 資料表建立完成';
END

-- 管理者角色指派資料表 (多對多關聯表)
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='ManagerRole' AND xtype='U')
BEGIN
    CREATE TABLE ManagerRole (
        Manager_Id INT NOT NULL, -- 管理者編號 (外鍵: 管理者)
        ManagerRole_Id INT NOT NULL, -- 角色編號 (外鍵: 角色)
        ManagerRole NVARCHAR(100) NULL, -- 角色名稱
        CONSTRAINT PK_ManagerRole PRIMARY KEY (Manager_Id, ManagerRole_Id), -- 複合主鍵避免重複指派同一角色
        CONSTRAINT FK_ManagerRole_ManagerData FOREIGN KEY (Manager_Id) REFERENCES ManagerData(Manager_Id) ON DELETE CASCADE,
        CONSTRAINT FK_ManagerRole_ManagerRolePermission FOREIGN KEY (ManagerRole_Id) REFERENCES ManagerRolePermission(ManagerRole_Id) ON DELETE CASCADE
    );
    PRINT '✓ ManagerRole 資料表建立完成';
END

-- 後台管理員資料表
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Admins' AND xtype='U')
BEGIN
    CREATE TABLE Admins (
        manager_id int PRIMARY KEY, -- 管理員編號 (主鍵, 外鍵到 ManagerData)
        last_login datetime2 NULL, -- 上次登入時間 (用於後台登入追蹤)
        CONSTRAINT FK_Admins_ManagerData FOREIGN KEY (manager_id) REFERENCES ManagerData(Manager_Id) ON DELETE CASCADE
    );
    PRINT '✓ Admins 資料表建立完成';
END

-- 禁言選項資料表 (字典)
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Mutes' AND xtype='U')
BEGIN
    CREATE TABLE Mutes (
        mute_id int IDENTITY(1,1) PRIMARY KEY, -- 禁言選項編號 (字典)
        mute_name nvarchar(100) NULL, -- 禁言名稱
        created_at datetime2 NULL DEFAULT GETUTCDATE(), -- 建立時間
        is_active bit NOT NULL DEFAULT 1, -- 是否啟用
        manager_id int NULL, -- 設置者編號
        CONSTRAINT FK_Mutes_ManagerData FOREIGN KEY (manager_id) REFERENCES ManagerData(Manager_Id)
    );
    PRINT '✓ Mutes 資料表建立完成';
END

-- 樣式資料表
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Styles' AND xtype='U')
BEGIN
    CREATE TABLE Styles (
        style_id int IDENTITY(1,1) PRIMARY KEY, -- 樣式編號
        style_name nvarchar(100) NULL, -- 樣式名稱
        effect_desc nvarchar(200) NULL, -- 效果說明
        created_at datetime2 NULL DEFAULT GETUTCDATE(), -- 建立時間
        manager_id int NULL, -- 設置者編號
        CONSTRAINT FK_Styles_ManagerData FOREIGN KEY (manager_id) REFERENCES ManagerData(Manager_Id)
    );
    PRINT '✓ Styles 資料表建立完成';
END

-- =====================================================
-- 遊戲和熱度相關資料表 (管理員: 溫傑揚)
-- =====================================================

-- 遊戲主檔資料表: 列出平台所有遊戲
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='games' AND xtype='U')
BEGIN
    CREATE TABLE games (
        game_id int IDENTITY(1,1) PRIMARY KEY, -- 遊戲ID (主鍵, 自動遞增)
        name nvarchar(200) NULL, -- 遊戲名稱
        genre nvarchar(100) NULL, -- 類型 (FPS/MOBA/RPG 等)
        created_at datetime2 NULL DEFAULT GETUTCDATE() -- 建立時間
    );
    PRINT '✓ games 資料表建立完成';
END

-- 數據來源字典資料表: 定義要抓的外部平台
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='metric_sources' AND xtype='U')
BEGIN
    CREATE TABLE metric_sources (
        source_id int IDENTITY(1,1) PRIMARY KEY, -- 來源編號 (主鍵)
        name varchar(100) NULL, -- 來源名 (Steam/Bahamut/YouTube 等)
        note nvarchar(500) NULL, -- 備註 (抓法/限制)
        created_at datetime2 NULL DEFAULT GETUTCDATE() -- 建立時間
    );
    PRINT '✓ metric_sources 資料表建立完成';
END

-- 指標字典資料表: 來源底下的可用指標清單
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='metrics' AND xtype='U')
BEGIN
    CREATE TABLE metrics (
        metric_id int IDENTITY(1,1) PRIMARY KEY, -- 指標編號 (主鍵)
        source_id int NOT NULL, -- 所屬來源編號
        code varchar(100) NULL, -- 指標代碼 (concurrent_users/forum_posts 等)
        unit varchar(50) NULL, -- 單位 (users/posts/views)
        description nvarchar(200) NULL, -- 指標中文說明
        is_active bit NULL DEFAULT 1, -- 是否啟用
        created_at datetime2 NULL DEFAULT GETUTCDATE(), -- 建立時間
        CONSTRAINT FK_Metrics_MetricSources FOREIGN KEY (source_id) REFERENCES metric_sources(source_id),
        CONSTRAINT UQ_Metrics_SourceCode UNIQUE (source_id, code) -- 同一來源下代碼唯一
    );
    PRINT '✓ metrics 資料表建立完成';
END

-- 外部ID對應資料表: 把內部遊戲對應到各來源的外部鍵
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='game_source_map' AND xtype='U')
BEGIN
    CREATE TABLE game_source_map (
        id int IDENTITY(1,1) PRIMARY KEY, -- 對應編號
        game_id int NOT NULL, -- 內部遊戲編號
        source_id int NOT NULL, -- 外部來源編號
        external_key varchar(100) NULL, -- 外部ID (Steam appid / 巴哈 slug)
        CONSTRAINT FK_GameSourceMap_Games FOREIGN KEY (game_id) REFERENCES games(game_id),
        CONSTRAINT FK_GameSourceMap_MetricSources FOREIGN KEY (source_id) REFERENCES metric_sources(source_id),
        CONSTRAINT UQ_GameSourceMap_GameSource UNIQUE (game_id, source_id) -- 一遊戲在一來源只對應一次
    );
    PRINT '✓ game_source_map 資料表建立完成';
END

-- 每天每指標的原始值資料表 (清洗後), 是計算指數的底稿
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='game_metric_daily' AND xtype='U')
BEGIN
    CREATE TABLE game_metric_daily (
        id int IDENTITY(1,1) PRIMARY KEY, -- 流水號
        game_id int NOT NULL, -- 遊戲編號
        metric_id int NOT NULL, -- 指標編號
        date date NOT NULL, -- 日期 (日粒度)
        value decimal(18,4) NOT NULL, -- 數值 (清洗後)
        agg_method varchar(20) NULL, -- 聚合方法 (sum/avg/max)
        quality varchar(20) NULL, -- 資料品質 (real/estimate/seed)
        created_at datetime2 NULL DEFAULT GETUTCDATE(), -- 建立時間
        updated_at datetime2 NULL DEFAULT GETUTCDATE(), -- 更新時間
        CONSTRAINT FK_GameMetricDaily_Games FOREIGN KEY (game_id) REFERENCES games(game_id),
        CONSTRAINT FK_GameMetricDaily_Metrics FOREIGN KEY (metric_id) REFERENCES metrics(metric_id),
        CONSTRAINT UQ_GameMetricDaily_GameMetricDate UNIQUE (game_id, metric_id, date) -- UPSERT 防重
    );
    PRINT '✓ game_metric_daily 資料表建立完成';
END

-- 每日熱度指數資料表 (計算結果)
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='popularity_index_daily' AND xtype='U')
BEGIN
    CREATE TABLE popularity_index_daily (
        id bigint IDENTITY(1,1) PRIMARY KEY, -- 流水號
        game_id int NOT NULL, -- 遊戲編號
        date date NOT NULL, -- 日期
        index_value decimal(18,4) NOT NULL, -- 熱度指數 (加權計算)
        created_at datetime2 NULL DEFAULT GETUTCDATE(), -- 建立時間
        CONSTRAINT FK_PopularityIndexDaily_Games FOREIGN KEY (game_id) REFERENCES games(game_id),
        CONSTRAINT UQ_PopularityIndexDaily_GameDate UNIQUE (game_id, date) -- 每日每遊戲唯一
    );
    PRINT '✓ popularity_index_daily 資料表建立完成';
END

-- 榜單快照資料表: 直接給前台讀, 避免重算
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='leaderboard_snapshots' AND xtype='U')
BEGIN
    CREATE TABLE leaderboard_snapshots (
        snapshot_id bigint IDENTITY(1,1) PRIMARY KEY, -- 快照編號
        period varchar(20) NULL, -- 期間類型 (daily/weekly)
        ts datetime2 NOT NULL, -- 快照時間
        rank int NOT NULL, -- 名次 (1..N)
        game_id int NOT NULL, -- 遊戲編號
        index_value decimal(18,4) NOT NULL, -- 當時指數值
        created_at datetime2 NULL DEFAULT GETUTCDATE(), -- 建立時間
        CONSTRAINT FK_LeaderboardSnapshots_Games FOREIGN KEY (game_id) REFERENCES games(game_id),
        CONSTRAINT UQ_LeaderboardSnapshots_PeriodTsRankGame UNIQUE (period, ts, rank, game_id)
    );
    PRINT '✓ leaderboard_snapshots 資料表建立完成';
END

-- =====================================================
-- 統一動態牆相關資料表 (管理員洞察)
-- =====================================================

-- 統一貼文資料表: 洞察與未來UGC都走這
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='posts' AND xtype='U')
BEGIN
    CREATE TABLE posts (
        post_id int IDENTITY(1,1) PRIMARY KEY, -- 文章編號
        type nvarchar(50) NULL, -- 類型 (insight/user)
        game_id int NULL, -- 關聯遊戲 (可為NULL)
        title nvarchar(200) NULL, -- 標題
        tldr nvarchar(500) NULL, -- 三行摘要 (卡片用)
        body_md nvarchar(max) NULL, -- 內文 (Markdown)
        visibility bit NULL DEFAULT 1, -- 可見性 (public/hidden)
        status varchar(20) NULL, -- 狀態 (draft/published/hidden)
        pinned bit NULL DEFAULT 0, -- 是否置頂
        created_by int NULL, -- 作者編號
        published_at datetime2 NULL, -- 發佈時間
        created_at datetime2 NULL DEFAULT GETUTCDATE(), -- 建立時間
        updated_at datetime2 NULL DEFAULT GETUTCDATE(), -- 更新時間
        CONSTRAINT FK_Posts_Games FOREIGN KEY (game_id) REFERENCES games(game_id),
        CONSTRAINT FK_Posts_Users FOREIGN KEY (created_by) REFERENCES Users(User_ID)
    );
    PRINT '✓ posts 資料表建立完成';
END

-- 洞察發佈時的數據快照資料表 (固定展示)
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='post_metric_snapshot' AND xtype='U')
BEGIN
    CREATE TABLE post_metric_snapshot (
        post_id bigint PRIMARY KEY, -- 文章編號 (主鍵)
        game_id int NULL, -- 當時的遊戲編號
        date date NOT NULL, -- 拍照日期
        index_value decimal(18,4) NOT NULL, -- 當日指數
        details_json nvarchar(max) NULL, -- 當日各指標值/Δ%/權重 (JSON)
        created_at datetime2 NULL DEFAULT GETUTCDATE(), -- 建立時間
        CONSTRAINT FK_PostMetricSnapshot_Posts FOREIGN KEY (post_id) REFERENCES posts(post_id) ON DELETE CASCADE,
        CONSTRAINT FK_PostMetricSnapshot_Games FOREIGN KEY (game_id) REFERENCES games(game_id)
    );
    PRINT '✓ post_metric_snapshot 資料表建立完成';
END

-- 洞察引用來源清單資料表
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='post_sources' AND xtype='U')
BEGIN
    CREATE TABLE post_sources (
        id bigint IDENTITY(1,1) PRIMARY KEY, -- 流水號
        post_id bigint NOT NULL, -- 文章編號
        source_name varchar(100) NULL, -- 顯示名稱
        url varchar(500) NULL, -- 外部連結
        created_at datetime2 NULL DEFAULT GETUTCDATE(), -- 建立時間
        CONSTRAINT FK_PostSources_Posts FOREIGN KEY (post_id) REFERENCES posts(post_id) ON DELETE CASCADE
    );
    PRINT '✓ post_sources 資料表建立完成';
END

-- =====================================================
-- 論壇相關資料表 (每遊戲一版) + 互動 (讚/收藏)
-- =====================================================

-- 論壇版主檔資料表: 每遊戲一個版
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='forums' AND xtype='U')
BEGIN
    CREATE TABLE forums (
        forum_id int IDENTITY(1,1) PRIMARY KEY, -- 論壇版編號
        game_id int NOT NULL UNIQUE, -- 遊戲編號 (一對一)
        name varchar(100) NULL, -- 版名
        description nvarchar(500) NULL, -- 版說明
        created_at datetime2 NULL DEFAULT GETUTCDATE(), -- 建立時間
        CONSTRAINT FK_Forums_Games FOREIGN KEY (game_id) REFERENCES games(game_id) ON DELETE CASCADE
    );
    PRINT '✓ forums 資料表建立完成';
END

-- 版內主題資料表 (討論串)
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='threads' AND xtype='U')
BEGIN
    CREATE TABLE threads (
        thread_id bigint IDENTITY(1,1) PRIMARY KEY, -- 主題編號
        forum_id int NOT NULL, -- 所屬版編號
        author_user_id int NOT NULL, -- 作者編號
        title nvarchar(200) NULL, -- 標題
        status varchar(20) NULL DEFAULT 'normal', -- 狀態 (normal/hidden/archived)
        created_at datetime2 NULL DEFAULT GETUTCDATE(), -- 建立時間
        updated_at datetime2 NULL DEFAULT GETUTCDATE(), -- 更新時間
        CONSTRAINT FK_Threads_Forums FOREIGN KEY (forum_id) REFERENCES forums(forum_id) ON DELETE CASCADE,
        CONSTRAINT FK_Threads_Users FOREIGN KEY (author_user_id) REFERENCES Users(User_ID)
    );
    PRINT '✓ threads 資料表建立完成';
END

-- 主題回覆資料表 (支援二層結構)
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='thread_posts' AND xtype='U')
BEGIN
    CREATE TABLE thread_posts (
        id bigint IDENTITY(1,1) PRIMARY KEY, -- 回覆編號
        thread_id bigint NOT NULL, -- 主題編號
        author_user_id int NOT NULL, -- 回覆者編號
        content_md nvarchar(max) NULL, -- 內容 (Markdown)
        parent_post_id bigint NULL, -- 父回覆編號 (支援二層)
        status varchar(20) NULL DEFAULT 'normal', -- 狀態 (normal/hidden/deleted)
        created_at datetime2 NULL DEFAULT GETUTCDATE(), -- 建立時間
        updated_at datetime2 NULL DEFAULT GETUTCDATE(), -- 更新時間
        CONSTRAINT FK_ThreadPosts_Threads FOREIGN KEY (thread_id) REFERENCES threads(thread_id) ON DELETE CASCADE,
        CONSTRAINT FK_ThreadPosts_Users FOREIGN KEY (author_user_id) REFERENCES Users(User_ID),
        CONSTRAINT FK_ThreadPosts_Parent FOREIGN KEY (parent_post_id) REFERENCES thread_posts(id)
    );
    PRINT '✓ thread_posts 資料表建立完成';
END

-- 通用讚資料表 (多型)
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='reactions' AND xtype='U')
BEGIN
    CREATE TABLE reactions (
        id bigint IDENTITY(1,1) PRIMARY KEY, -- 流水號
        user_id int NOT NULL, -- 誰按的
        target_type varchar(50) NULL, -- 目標類型 (post/thread/thread_post)
        target_id bigint NOT NULL, -- 目標編號 (多型, 不設FK)
        kind varchar(50) NULL, -- 反應類型 (like/emoji 等)
        created_at datetime2 NULL DEFAULT GETUTCDATE(), -- 建立時間
        CONSTRAINT FK_Reactions_Users FOREIGN KEY (user_id) REFERENCES Users(User_ID) ON DELETE CASCADE,
        CONSTRAINT UQ_Reactions_UserTargetKind UNIQUE (user_id, target_type, target_id, kind)
    );
    PRINT '✓ reactions 資料表建立完成';
END

-- 通用收藏資料表 (多型)
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='bookmarks' AND xtype='U')
BEGIN
    CREATE TABLE bookmarks (
        id bigint IDENTITY(1,1) PRIMARY KEY, -- 流水號
        user_id int NOT NULL, -- 收藏者編號
        target_type varchar(50) NULL, -- 目標類型 ('post' | 'thread' | 'game' | 'forum')
        target_id bigint NOT NULL, -- 目標編號 (多型, 不設FK)
        created_at datetime2 NULL DEFAULT GETUTCDATE(), -- 建立時間
        CONSTRAINT FK_Bookmarks_Users FOREIGN KEY (user_id) REFERENCES Users(User_ID) ON DELETE CASCADE,
        CONSTRAINT UQ_Bookmarks_UserTarget UNIQUE (user_id, target_type, target_id)
    );
    PRINT '✓ bookmarks 資料表建立完成';
END

-- =====================================================
-- 寵物和小遊戲相關資料表 (管理員: 鐘群能)
-- =====================================================

-- 使用者簽到統計資料表
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='UserSignInStats' AND xtype='U')
BEGIN
    CREATE TABLE UserSignInStats (
        LogID int IDENTITY(1,1) PRIMARY KEY, -- 簽到記錄編號 (自動遞增)
        SignTime datetime2 NOT NULL DEFAULT GETUTCDATE(), -- 簽到時間 (預設 UTC 當下時間)
        UserID int NOT NULL, -- 會員編號 (外鍵參考 Users.User_ID)
        PointsChanged int NOT NULL DEFAULT 0, -- 此次簽到會員點數增減數量
        ExpGained int NOT NULL DEFAULT 0, -- 此次簽到寵物獲得經驗值
        PointsChangedTime datetime2 NOT NULL DEFAULT GETUTCDATE(), -- 點數變動時間
        ExpGainedTime datetime2 NOT NULL DEFAULT GETUTCDATE(), -- 寵物經驗值獲得時間
        CONSTRAINT FK_UserSignInStats_Users FOREIGN KEY (UserID) REFERENCES Users(User_ID) ON DELETE CASCADE
    );
    PRINT '✓ UserSignInStats 資料表建立完成';
END

-- 寵物狀態資料表
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Pet' AND xtype='U')
BEGIN
    CREATE TABLE Pet (
        PetID int IDENTITY(1,1) PRIMARY KEY, -- 寵物編號 (自動遞增)
        UserID int NOT NULL, -- 寵物主人會員編號 (外鍵參考 Users.User_ID)
        PetName nvarchar(50) NOT NULL DEFAULT '小可愛', -- 寵物名稱 (若未提供則預設)
        Level int NOT NULL DEFAULT 1, -- 寵物當前等級
        LevelUpTime datetime2 NOT NULL DEFAULT GETUTCDATE(), -- 寵物最後一次升級時間
        Experience int NOT NULL DEFAULT 0, -- 寵物累計總經驗值
        Hunger int NOT NULL DEFAULT 100, -- 飢餓值 (0–100) - 應用層預設 100
        Mood int NOT NULL DEFAULT 100, -- 心情值 (0–100) - 應用層預設 100
        Stamina int NOT NULL DEFAULT 100, -- 體力值 (0–100) - 應用層預設 100
        Cleanliness int NOT NULL DEFAULT 100, -- 清潔值 (0–100) - 應用層預設 100
        Health int NOT NULL DEFAULT 100, -- 健康度 (0–100) - 應用層預設 100
        SkinColor nvarchar(50) NOT NULL DEFAULT '#ADD8E6', -- 膚色十六進位
        ColorChangedTime datetime2 NOT NULL DEFAULT GETUTCDATE(), -- 最後一次膚色更換時間
        BackgroundColor nvarchar(50) NOT NULL DEFAULT '粉藍', -- 背景色
        BackgroundColorChangedTime datetime2 NOT NULL DEFAULT GETUTCDATE(), -- 最後一次背景色更換時間
        PointsChanged int NOT NULL DEFAULT 0, -- 最近一次幫寵物換色所花費之會員點數
        PointsChangedTime datetime2 NOT NULL DEFAULT GETUTCDATE(), -- 幫寵物換色所花費之會員點數變動時間
        CONSTRAINT FK_Pet_Users FOREIGN KEY (UserID) REFERENCES Users(User_ID) ON DELETE CASCADE,
        CONSTRAINT CK_Pet_Hunger CHECK (Hunger >= 0 AND Hunger <= 100),
        CONSTRAINT CK_Pet_Mood CHECK (Mood >= 0 AND Mood <= 100),
        CONSTRAINT CK_Pet_Stamina CHECK (Stamina >= 0 AND Stamina <= 100),
        CONSTRAINT CK_Pet_Cleanliness CHECK (Cleanliness >= 0 AND Cleanliness <= 100),
        CONSTRAINT CK_Pet_Health CHECK (Health >= 0 AND Health <= 100),
        CONSTRAINT CK_Pet_Level CHECK (Level >= 1 AND Level <= 250)
    );
    PRINT '✓ Pet 資料表建立完成';
END

-- 小冒險遊戲紀錄資料表
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='MiniGame' AND xtype='U')
BEGIN
    CREATE TABLE MiniGame (
        PlayID int IDENTITY(1,1) PRIMARY KEY, -- 遊戲執行記錄編號 (自動遞增)
        UserID int NOT NULL, -- 玩家會員編號 (外鍵參考 Users.User_ID)
        PetID int NOT NULL, -- 出戰寵物編號 (外鍵參考 Pet.PetID)
        Level int NOT NULL DEFAULT 1, -- 遊戲關卡等級
        MonsterCount int NOT NULL DEFAULT 0, -- 需面對的怪物數量
        SpeedMultiplier decimal(5,2) NOT NULL DEFAULT 1.00, -- 怪物移動速度倍率
        Result nvarchar(10) NOT NULL DEFAULT 'Unknown', -- 遊戲結果: Win(贏)/Lose(輸)/Abort(中退)
        ExpGained int NOT NULL DEFAULT 0, -- 寵物本次獲得經驗值
        ExpGainedTime datetime2 NULL, -- 寵物獲得經驗值時間
        PointsChanged int NOT NULL DEFAULT 0, -- 本次會員點數增減
        PointsChangedTime datetime2 NULL, -- 本次會員點數變動時間
        HungerDelta int NOT NULL DEFAULT 0, -- 寵物飢餓值變化量
        MoodDelta int NOT NULL DEFAULT 0, -- 寵物心情值變化量
        StaminaDelta int NOT NULL DEFAULT 0, -- 寵物體力值變化量
        CleanlinessDelta int NOT NULL DEFAULT 0, -- 寵物清潔值變化量
        StartTime datetime2 NOT NULL DEFAULT GETUTCDATE(), -- 遊戲開始時間
        EndTime datetime2 NULL, -- 遊戲結束時間 (若中退則為null)
        Aborted bit NOT NULL DEFAULT 0, -- 是否中途放棄 (0=否,1=是), 預設為0(否)
        CONSTRAINT FK_MiniGame_Users FOREIGN KEY (UserID) REFERENCES Users(User_ID) ON DELETE CASCADE,
        CONSTRAINT FK_MiniGame_Pet FOREIGN KEY (PetID) REFERENCES Pet(PetID)
    );
    PRINT '✓ MiniGame 資料表建立完成';
END

-- =====================================================
-- 商城相關資料表 (管理員: 房立堯 & 成博儒)
-- =====================================================

-- 供應商資料表
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Supplier' AND xtype='U')
BEGIN
    CREATE TABLE Supplier (
        supplier_id INT IDENTITY(1,1) PRIMARY KEY, -- 廠商編號
        supplier_name NVARCHAR(200) NULL -- 廠商名稱
    );
    PRINT '✓ Supplier 資料表建立完成';
END

-- 商品資訊資料表
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='ProductInfo' AND xtype='U')
BEGIN
    CREATE TABLE ProductInfo (
        product_id INT IDENTITY(1,1) PRIMARY KEY, -- 商品編號
        product_name NVARCHAR(200) NULL, -- 商品名稱
        product_type NVARCHAR(100) NULL, -- 商品類型
        price DECIMAL(18,2) NOT NULL DEFAULT 0, -- 售價
        currency_code NVARCHAR(10) NULL, -- 使用幣別
        Shipment_Quantity INT NOT NULL DEFAULT 0, -- 庫存
        product_created_by NVARCHAR(100) NULL, -- 創建者
        product_created_at DATETIME2 NULL DEFAULT GETUTCDATE(), -- 建立時間
        product_updated_by NVARCHAR(100) NULL, -- 最後修改者
        product_updated_at DATETIME2 NULL DEFAULT GETUTCDATE(), -- 更新時間
        user_id INT NULL -- 會員編號
    );
    PRINT '✓ ProductInfo 資料表建立完成';
END

-- 遊戲主檔商品資訊資料表
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='GameProductDetails' AND xtype='U')
BEGIN
    CREATE TABLE GameProductDetails (
        product_id int PRIMARY KEY, -- 商品編號 (主鍵)
        product_name NVARCHAR(200) NULL, -- 商品名稱
        product_description NVARCHAR(1000) NULL, -- 商品描述
        supplier_id int NULL, -- 廠商編號
        platform_id int NULL, -- 遊戲平台編號
        game_id int NULL, -- 遊戲編號
        game_name NVARCHAR(200) NULL, -- 遊戲名稱
        download_link NVARCHAR(500) NULL, -- 下載連結
        CONSTRAINT FK_GameProductDetails_ProductInfo FOREIGN KEY (product_id) REFERENCES ProductInfo(product_id) ON DELETE CASCADE,
        CONSTRAINT FK_GameProductDetails_Supplier FOREIGN KEY (supplier_id) REFERENCES Supplier(supplier_id),
        CONSTRAINT FK_GameProductDetails_Games FOREIGN KEY (game_id) REFERENCES games(game_id)
    );
    PRINT '✓ GameProductDetails 資料表建立完成';
END

-- 非遊戲主檔商品資訊資料表
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='OtherProductDetails' AND xtype='U')
BEGIN
    CREATE TABLE OtherProductDetails (
        product_id int PRIMARY KEY, -- 商品編號 (主鍵)
        product_name NVARCHAR(200) NULL, -- 商品名稱
        product_description NVARCHAR(1000) NULL, -- 商品描述
        supplier_id int NULL, -- 廠商編號
        platform_id int NULL, -- 遊戲平台編號
        digital_code NVARCHAR(100) NULL, -- 數位序號兌換碼
        size NVARCHAR(50) NULL, -- 尺寸
        color NVARCHAR(50) NULL, -- 顏色
        weight NVARCHAR(50) NULL, -- 重量
        dimensions NVARCHAR(100) NULL, -- 尺寸
        material NVARCHAR(100) NULL, -- 材質
        stock_quantity NVARCHAR(50) NULL, -- 庫存數量
        CONSTRAINT FK_OtherProductDetails_ProductInfo FOREIGN KEY (product_id) REFERENCES ProductInfo(product_id) ON DELETE CASCADE,
        CONSTRAINT FK_OtherProductDetails_Supplier FOREIGN KEY (supplier_id) REFERENCES Supplier(supplier_id)
    );
    PRINT '✓ OtherProductDetails 資料表建立完成';
END

-- 訂單資訊資料表
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='OrderInfo' AND xtype='U')
BEGIN
    CREATE TABLE OrderInfo (
        order_id INT IDENTITY(1,1) PRIMARY KEY, -- 訂單編號
        user_id INT NOT NULL, -- 下訂會員編號
        order_date DATETIME2 NULL DEFAULT GETUTCDATE(), -- 下單日期
        order_status NVARCHAR(50) NULL DEFAULT 'Created', -- 訂單狀態 (Created/ToShip/Shipped/Completed)
        payment_status NVARCHAR(50) NULL DEFAULT 'Placed', -- 付款狀態 (Placed/Pending/Paid)
        order_total DECIMAL(18,2) NOT NULL DEFAULT 0, -- 訂單總額
        payment_at DATETIME2 NULL, -- 付款時間
        shipped_at DATETIME2 NULL, -- 出貨時間
        completed_at DATETIME2 NULL, -- 完成時間
        CONSTRAINT FK_OrderInfo_Users FOREIGN KEY (user_id) REFERENCES Users(User_ID) ON DELETE CASCADE
    );
    PRINT '✓ OrderInfo 資料表建立完成';
END

-- 訂單詳細資料表
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='OrderItems' AND xtype='U')
BEGIN
    CREATE TABLE OrderItems (
        item_id INT IDENTITY(1,1) PRIMARY KEY, -- 訂單詳細編號
        order_id INT NOT NULL, -- 訂單編號 (指向訂單)
        product_id INT NOT NULL, -- 商品編號 (指向商品)
        line_no INT NOT NULL, -- 實際物品編號 1.2.3…
        unit_price DECIMAL(18,2) NOT NULL, -- 單價
        quantity INT NOT NULL, -- 下單數量
        subtotal DECIMAL(18,2) NOT NULL, -- 小計
        CONSTRAINT FK_OrderItems_OrderInfo FOREIGN KEY (order_id) REFERENCES OrderInfo(order_id) ON DELETE CASCADE,
        CONSTRAINT FK_OrderItems_ProductInfo FOREIGN KEY (product_id) REFERENCES ProductInfo(product_id)
    );
    PRINT '✓ OrderItems 資料表建立完成';
END

-- 官方商城排行榜資料表
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Official_Store_Ranking' AND xtype='U')
BEGIN
    CREATE TABLE Official_Store_Ranking (
        ranking_id INT IDENTITY(1,1) PRIMARY KEY, -- 排行榜流水號
        period_type NVARCHAR(50) NULL, -- 榜單型態 (日、月、季、年)
        ranking_date DATE NOT NULL, -- 榜單日期 (計算日期)
        product_ID INT NOT NULL, -- 商品編號 (指向排名目標)
        ranking_metric VARCHAR(50) NULL, -- 排名指標 (交易額/量)
        ranking_position TINYINT NOT NULL, -- 名次
        trading_amount DECIMAL(18,2) NOT NULL DEFAULT 0, -- 交易額
        trading_volume INT NOT NULL DEFAULT 0, -- 交易量
        ranking_updated_at DATETIME2 NULL DEFAULT GETUTCDATE(), -- 排行榜更新時間
        CONSTRAINT FK_OfficialStoreRanking_ProductInfo FOREIGN KEY (product_ID) REFERENCES ProductInfo(product_id)
    );
    PRINT '✓ Official_Store_Ranking 資料表建立完成';
END

-- 商品修改日誌資料表
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='ProductInfoAuditLog' AND xtype='U')
BEGIN
    CREATE TABLE ProductInfoAuditLog (
        log_id BIGINT IDENTITY(1,1) PRIMARY KEY, -- 日誌編號
        product_id INT NOT NULL, -- 商品編號
        action_type NVARCHAR(50) NULL, -- 動作類型
        field_name NVARCHAR(100) NULL, -- 修改欄位名稱
        old_value NVARCHAR(1000) NULL, -- 舊值
        new_value NVARCHAR(1000) NULL, -- 新值
        Manager_Id INT NULL, -- 操作人編號
        changed_at DATETIME2 NULL DEFAULT GETUTCDATE(), -- 修改時間
        CONSTRAINT FK_ProductInfoAuditLog_ProductInfo FOREIGN KEY (product_id) REFERENCES ProductInfo(product_id) ON DELETE CASCADE,
        CONSTRAINT FK_ProductInfoAuditLog_ManagerData FOREIGN KEY (Manager_Id) REFERENCES ManagerData(Manager_Id)
    );
    PRINT '✓ ProductInfoAuditLog 資料表建立完成';
END

-- =====================================================
-- 自由市場相關資料表
-- =====================================================

-- 自由市場商品資訊資料表
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='PlayerMarketProductInfo' AND xtype='U')
BEGIN
    CREATE TABLE PlayerMarketProductInfo (
        p_product_id int IDENTITY(1,1) PRIMARY KEY, -- 自由市場商品編號
        p_product_type nvarchar(100) NULL, -- 商品類型
        p_product_title nvarchar(200) NULL, -- 商品標題 (噱頭標語)
        p_product_name nvarchar(200) NULL, -- 商品名稱
        p_product_description nvarchar(1000) NULL, -- 商品描述
        product_id int NULL, -- 官方商品編號 (可選關聯)
        seller_id int NOT NULL, -- 賣家編號 (Seller 是用會員帳號)
        p_status nvarchar(50) NULL DEFAULT 'Active', -- 商品狀態
        price decimal(18,2) NOT NULL DEFAULT 0, -- 售價
        p_product_img_id nvarchar(100) NULL, -- 商品圖片編號
        created_at datetime2 NULL DEFAULT GETUTCDATE(), -- 建立時間
        updated_at datetime2 NULL DEFAULT GETUTCDATE(), -- 更新時間
        CONSTRAINT FK_PlayerMarketProductInfo_ProductInfo FOREIGN KEY (product_id) REFERENCES ProductInfo(product_id),
        CONSTRAINT FK_PlayerMarketProductInfo_Seller FOREIGN KEY (seller_id) REFERENCES Users(User_ID)
    );
    PRINT '✓ PlayerMarketProductInfo 資料表建立完成';
END

-- 自由市場商品圖片資料表
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='PlayerMarketProductImgs' AND xtype='U')
BEGIN
    CREATE TABLE PlayerMarketProductImgs (
        p_product_img_id int IDENTITY(1,1) PRIMARY KEY, -- 商品圖片編號
        p_product_id int NOT NULL, -- 指向自由市場商品
        p_product_img_url varbinary(max) NULL, -- 商品圖片網址 (二進位資料)
        CONSTRAINT FK_PlayerMarketProductImgs_PlayerMarketProductInfo FOREIGN KEY (p_product_id) REFERENCES PlayerMarketProductInfo(p_product_id) ON DELETE CASCADE
    );
    PRINT '✓ PlayerMarketProductImgs 資料表建立完成';
END

-- 自由市場訂單資料表
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='PlayerMarketOrderInfo' AND xtype='U')
BEGIN
    CREATE TABLE PlayerMarketOrderInfo (
        p_order_id int IDENTITY(1,1) PRIMARY KEY, -- 自由市場訂單編號
        p_product_id int NOT NULL, -- 自由市場商品編號
        seller_id int NOT NULL, -- 賣家編號 (Seller 是用會員帳號)
        buyer_id int NOT NULL, -- 買家編號 (Buyer 是用會員帳號)
        p_order_date datetime2 NULL DEFAULT GETUTCDATE(), -- 訂單日期
        p_order_status nvarchar(50) NULL DEFAULT 'Created', -- 訂單狀態
        p_payment_status nvarchar(50) NULL DEFAULT 'Pending', -- 付款狀態
        p_unit_price int NOT NULL DEFAULT 0, -- 單價
        p_quantity int NOT NULL DEFAULT 1, -- 數量
        p_order_total int NOT NULL DEFAULT 0, -- 總額
        p_order_created_at datetime2 NULL DEFAULT GETUTCDATE(), -- 建立時間
        p_order_updated_at datetime2 NULL DEFAULT GETUTCDATE(), -- 更新時間
        CONSTRAINT FK_PlayerMarketOrderInfo_PlayerMarketProductInfo FOREIGN KEY (p_product_id) REFERENCES PlayerMarketProductInfo(p_product_id),
        CONSTRAINT FK_PlayerMarketOrderInfo_Seller FOREIGN KEY (seller_id) REFERENCES Users(User_ID),
        CONSTRAINT FK_PlayerMarketOrderInfo_Buyer FOREIGN KEY (buyer_id) REFERENCES Users(User_ID)
    );
    PRINT '✓ PlayerMarketOrderInfo 資料表建立完成';
END

-- 交易中頁面資料表
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='PlayerMarketOrderTradepage' AND xtype='U')
BEGIN
    CREATE TABLE PlayerMarketOrderTradepage (
        p_order_tradepage_id int IDENTITY(1,1) PRIMARY KEY, -- 流水號 交易頁面編號
        p_order_id int NOT NULL, -- 自由市場訂單編號
        p_product_id int NOT NULL, -- 自由市場商品編號
        p_order_platform_fee int NOT NULL DEFAULT 0, -- 平台抽成
        seller_transferred_at datetime2 NULL, -- user1(seller) 賣家移交時間
        buyer_received_at datetime2 NULL, -- user2(buyer) 買家接收時間
        completed_at datetime2 NULL, -- 交易完成時間
        CONSTRAINT FK_PlayerMarketOrderTradepage_PlayerMarketOrderInfo FOREIGN KEY (p_order_id) REFERENCES PlayerMarketOrderInfo(p_order_id) ON DELETE CASCADE,
        CONSTRAINT FK_PlayerMarketOrderTradepage_PlayerMarketProductInfo FOREIGN KEY (p_product_id) REFERENCES PlayerMarketProductInfo(p_product_id)
    );
    PRINT '✓ PlayerMarketOrderTradepage 資料表建立完成';
END

-- 自由市場交易頁面對話資料表
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='PlayerMarketTradeMsg' AND xtype='U')
BEGIN
    CREATE TABLE PlayerMarketTradeMsg (
        trade_msg_id int IDENTITY(1,1) PRIMARY KEY, -- 交易中雙方訊息編號
        p_order_tradepage_id int NOT NULL, -- 交易頁面編號
        msg_from nvarchar(20) NULL, -- 誰傳的訊息 (seller/buyer)
        message_text nvarchar(1000) NULL, -- 訊息內容
        created_at datetime2 NULL DEFAULT GETUTCDATE(), -- 傳訊時間
        CONSTRAINT FK_PlayerMarketTradeMsg_PlayerMarketOrderTradepage FOREIGN KEY (p_order_tradepage_id) REFERENCES PlayerMarketOrderTradepage(p_order_tradepage_id) ON DELETE CASCADE
    );
    PRINT '✓ PlayerMarketTradeMsg 資料表建立完成';
END

-- 自由市場排行榜資料表
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='PlayerMarketRanking' AND xtype='U')
BEGIN
    CREATE TABLE PlayerMarketRanking (
        p_ranking_id int IDENTITY(1,1) PRIMARY KEY, -- 排行榜編號
        p_period_type varchar(50) NULL, -- 榜單型態
        p_ranking_date date NOT NULL, -- 榜單日期
        p_product_id int NOT NULL, -- 商品編號
        p_ranking_metric varchar(50) NULL, -- 排名指標
        p_ranking_position int NOT NULL, -- 名次
        p_trading_amount DECIMAL(18,2) NOT NULL DEFAULT 0, -- 交易額
        p_trading_volume int NOT NULL DEFAULT 0, -- 交易量
        updated_at datetime2 NULL DEFAULT GETUTCDATE(), -- 排行榜更新時間
        CONSTRAINT FK_PlayerMarketRanking_PlayerMarketProductInfo FOREIGN KEY (p_product_id) REFERENCES PlayerMarketProductInfo(p_product_id)
    );
    PRINT '✓ PlayerMarketRanking 資料表建立完成';
END

-- =====================================================
-- 通知和社交相關資料表
-- =====================================================

-- 通知來源資料表
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Notification_Sources' AND xtype='U')
BEGIN
    CREATE TABLE Notification_Sources (
        source_id int IDENTITY(1,1) PRIMARY KEY, -- 來源類型編號
        source_name nvarchar(100) NULL -- 來源名稱
    );
    PRINT '✓ Notification_Sources 資料表建立完成';
END

-- 通知行為資料表
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Notification_Actions' AND xtype='U')
BEGIN
    CREATE TABLE Notification_Actions (
        action_id int IDENTITY(1,1) PRIMARY KEY, -- 行為類型編號
        action_name nvarchar(100) NULL, -- 行為名稱
        CONSTRAINT UQ_NotificationActions_ActionName UNIQUE (action_name)
    );
    PRINT '✓ Notification_Actions 資料表建立完成';
END

-- 通知主資料表
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Notifications' AND xtype='U')
BEGIN
    CREATE TABLE Notifications (
        notification_id int IDENTITY(1,1) PRIMARY KEY, -- 通知編號
        source_id int NOT NULL, -- 來源類型編號
        action_id int NOT NULL, -- 行為類型編號
        sender_id int NOT NULL, -- 發送者編號
        sender_manager_id int NULL, -- 發送者編號 (管理員)
        notification_title nvarchar(200) NULL, -- 通知標題
        notification_message nvarchar(1000) NULL, -- 通知內容
        created_at datetime2 NOT NULL DEFAULT GETUTCDATE(), -- 建立時間
        group_id int NULL, -- 群組編號 (若為群組相關)
        CONSTRAINT FK_Notifications_NotificationSources FOREIGN KEY (source_id) REFERENCES Notification_Sources(source_id),
        CONSTRAINT FK_Notifications_NotificationActions FOREIGN KEY (action_id) REFERENCES Notification_Actions(action_id),
        CONSTRAINT FK_Notifications_Users FOREIGN KEY (sender_id) REFERENCES Users(User_ID)
    );
    PRINT '✓ Notifications 資料表建立完成';
END

-- 通知接收者資料表
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Notification_Recipients' AND xtype='U')
BEGIN
    CREATE TABLE Notification_Recipients (
        recipient_id int IDENTITY(1,1) PRIMARY KEY, -- 接收紀錄編號
        notification_id int NOT NULL, -- 通知編號
        user_id int NOT NULL, -- 使用者編號
        is_read bit NOT NULL DEFAULT 0, -- 是否已讀
        read_at datetime2 NULL, -- 已讀時間
        CONSTRAINT FK_NotificationRecipients_Notifications FOREIGN KEY (notification_id) REFERENCES Notifications(notification_id) ON DELETE CASCADE,
        CONSTRAINT FK_NotificationRecipients_Users FOREIGN KEY (user_id) REFERENCES Users(User_ID) ON DELETE CASCADE,
        CONSTRAINT UQ_NotificationRecipients_NotificationUser UNIQUE (notification_id, user_id) -- 同一通知不重複投遞給同一人
    );
    PRINT '✓ Notification_Recipients 資料表建立完成';
END

-- 聊天訊息資料表
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Chat_Message' AND xtype='U')
BEGIN
    CREATE TABLE Chat_Message (
        message_id int IDENTITY(1,1) PRIMARY KEY, -- 訊息編號
        manager_id int NULL, -- 管理員編號 (客服)
        sender_id int NOT NULL, -- 發送者編號
        receiver_id int NULL, -- 接收者編號
        chat_content nvarchar(1000) NOT NULL, -- 訊息內容
        sent_at datetime2 NOT NULL DEFAULT GETUTCDATE(), -- 發送時間
        is_read bit NOT NULL DEFAULT 0, -- 是否已讀
        is_sent bit NOT NULL DEFAULT 1, -- 是否寄送
        CONSTRAINT FK_ChatMessage_Sender FOREIGN KEY (sender_id) REFERENCES Users(User_ID),
        CONSTRAINT FK_ChatMessage_Receiver FOREIGN KEY (receiver_id) REFERENCES Users(User_ID)
    );
    PRINT '✓ Chat_Message 資料表建立完成';
END

-- 群組資料表
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Groups' AND xtype='U')
BEGIN
    CREATE TABLE Groups (
        group_id int IDENTITY(1,1) PRIMARY KEY, -- 群組編號
        group_name nvarchar(100) NULL, -- 群組名稱
        created_by int NULL, -- 建立者編號
        created_at datetime2 NULL DEFAULT GETUTCDATE(), -- 建立時間
        CONSTRAINT FK_Groups_Users FOREIGN KEY (created_by) REFERENCES Users(User_ID)
    );
    PRINT '✓ Groups 資料表建立完成';
END

-- 群組成員資料表
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Group_Member' AND xtype='U')
BEGIN
    CREATE TABLE Group_Member (
        group_id int NOT NULL, -- 群組編號
        user_id int NOT NULL, -- 使用者編號
        joined_at datetime2 NULL DEFAULT GETUTCDATE(), -- 加入時間
        is_admin bit NOT NULL DEFAULT 0, -- 是否為管理員
        CONSTRAINT PK_GroupMember PRIMARY KEY (group_id, user_id), -- 群組成員複合主鍵, 不重複
        CONSTRAINT FK_GroupMember_Groups FOREIGN KEY (group_id) REFERENCES Groups(group_id) ON DELETE CASCADE,
        CONSTRAINT FK_GroupMember_Users FOREIGN KEY (user_id) REFERENCES Users(User_ID) ON DELETE CASCADE
    );
    PRINT '✓ Group_Member 資料表建立完成';
END

-- 群組專用聊天資料表
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Group_Chat' AND xtype='U')
BEGIN
    CREATE TABLE Group_Chat (
        group_chat_id int IDENTITY(1,1) PRIMARY KEY, -- 群組聊天編號
        group_id int NULL, -- 群組編號
        sender_id int NULL, -- 發送者編號
        group_chat_content nvarchar(1000) NULL, -- 訊息內容
        sent_at datetime2 NULL DEFAULT GETUTCDATE(), -- 發送時間
        is_sent bit NOT NULL DEFAULT 1, -- 是否寄送
        CONSTRAINT FK_GroupChat_Groups FOREIGN KEY (group_id) REFERENCES Groups(group_id) ON DELETE CASCADE,
        CONSTRAINT FK_GroupChat_Users FOREIGN KEY (sender_id) REFERENCES Users(User_ID)
    );
    PRINT '✓ Group_Chat 資料表建立完成';
END

-- 封鎖資料表 (群組專用)
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Group_Block' AND xtype='U')
BEGIN
    CREATE TABLE Group_Block (
        block_id int IDENTITY(1,1) PRIMARY KEY, -- 封鎖編號
        group_id int NOT NULL, -- 群組編號
        user_id int NOT NULL, -- 被封鎖者編號
        blocked_by int NOT NULL, -- 封鎖者編號
        created_at datetime2 NULL DEFAULT GETUTCDATE(), -- 建立時間
        CONSTRAINT FK_GroupBlock_Groups FOREIGN KEY (group_id) REFERENCES Groups(group_id) ON DELETE CASCADE,
        CONSTRAINT FK_GroupBlock_BlockedUser FOREIGN KEY (user_id) REFERENCES Users(User_ID),
        CONSTRAINT FK_GroupBlock_BlockedByUser FOREIGN KEY (blocked_by) REFERENCES Users(User_ID),
        CONSTRAINT UQ_GroupBlock_GroupUser UNIQUE (group_id, user_id) -- 關鍵: 同一群×同一人只能有一筆
    );
    PRINT '✓ Group_Block 資料表建立完成';
END

-- =====================================================
-- 建立效能索引
-- =====================================================

-- 使用者相關索引
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_UserSignInStats_UserID_SignTime')
BEGIN
    CREATE INDEX IX_UserSignInStats_UserID_SignTime ON UserSignInStats(UserID, SignTime);
    PRINT '✓ 簽到記錄索引建立完成';
END

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_Pet_UserID')
BEGIN
    CREATE INDEX IX_Pet_UserID ON Pet(UserID);
    PRINT '✓ 寵物使用者索引建立完成';
END

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_MiniGame_UserID_StartTime')
BEGIN
    CREATE INDEX IX_MiniGame_UserID_StartTime ON MiniGame(UserID, StartTime);
    PRINT '✓ 小遊戲記錄索引建立完成';
END

-- 論壇相關索引
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_threads_forum_id_updated_at')
BEGIN
    CREATE INDEX IX_threads_forum_id_updated_at ON threads(forum_id, updated_at);
    PRINT '✓ 討論串索引建立完成';
END

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_thread_posts_thread_id_created_at')
BEGIN
    CREATE INDEX IX_thread_posts_thread_id_created_at ON thread_posts(thread_id, created_at);
    PRINT '✓ 討論回覆索引建立完成';
END

-- 通知相關索引
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_Notification_Recipients_user_id_is_read')
BEGIN
    CREATE INDEX IX_Notification_Recipients_user_id_is_read ON Notification_Recipients(user_id, is_read, recipient_id);
    PRINT '✓ 通知收件匣索引建立完成';
END

-- 熱度相關索引
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_game_metric_daily_date_metric_id')
BEGIN
    CREATE INDEX IX_game_metric_daily_date_metric_id ON game_metric_daily(date, metric_id);
    PRINT '✓ 遊戲指標日期索引建立完成';
END

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_game_metric_daily_game_id_date')
BEGIN
    CREATE INDEX IX_game_metric_daily_game_id_date ON game_metric_daily(game_id, date);
    PRINT '✓ 遊戲指標遊戲索引建立完成';
END

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_leaderboard_snapshots_period_ts_rank')
BEGIN
    CREATE INDEX IX_leaderboard_snapshots_period_ts_rank ON leaderboard_snapshots(period, ts, rank);
    PRINT '✓ 榜單快照索引建立完成';
END

-- 商城相關索引
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_OrderInfo_user_id_order_status_order_date')
BEGIN
    CREATE INDEX IX_OrderInfo_user_id_order_status_order_date ON OrderInfo(user_id, order_status, order_date);
    PRINT '✓ 訂單索引建立完成';
END

-- 玩家市場索引
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_PlayerMarketProductInfo_seller_id_status_created_at')
BEGIN
    CREATE INDEX IX_PlayerMarketProductInfo_seller_id_status_created_at ON PlayerMarketProductInfo(seller_id, p_status, created_at);
    PRINT '✓ 玩家市場商品索引建立完成';
END

-- =====================================================
-- 建立檢查約束
-- =====================================================

-- 寵物屬性值約束 (0-100)
IF NOT EXISTS (SELECT * FROM sys.check_constraints WHERE name = 'CK_Pet_Hunger')
BEGIN
    ALTER TABLE Pet ADD CONSTRAINT CK_Pet_Hunger CHECK (Hunger >= 0 AND Hunger <= 100);
    PRINT '✓ 寵物飢餓值約束建立完成';
END

IF NOT EXISTS (SELECT * FROM sys.check_constraints WHERE name = 'CK_Pet_Mood')
BEGIN
    ALTER TABLE Pet ADD CONSTRAINT CK_Pet_Mood CHECK (Mood >= 0 AND Mood <= 100);
    PRINT '✓ 寵物心情值約束建立完成';
END

IF NOT EXISTS (SELECT * FROM sys.check_constraints WHERE name = 'CK_Pet_Stamina')
BEGIN
    ALTER TABLE Pet ADD CONSTRAINT CK_Pet_Stamina CHECK (Stamina >= 0 AND Stamina <= 100);
    PRINT '✓ 寵物體力值約束建立完成';
END

IF NOT EXISTS (SELECT * FROM sys.check_constraints WHERE name = 'CK_Pet_Cleanliness')
BEGIN
    ALTER TABLE Pet ADD CONSTRAINT CK_Pet_Cleanliness CHECK (Cleanliness >= 0 AND Cleanliness <= 100);
    PRINT '✓ 寵物清潔值約束建立完成';
END

IF NOT EXISTS (SELECT * FROM sys.check_constraints WHERE name = 'CK_Pet_Health')
BEGIN
    ALTER TABLE Pet ADD CONSTRAINT CK_Pet_Health CHECK (Health >= 0 AND Health <= 100);
    PRINT '✓ 寵物健康值約束建立完成';
END

-- 寵物等級約束 (1-250)
IF NOT EXISTS (SELECT * FROM sys.check_constraints WHERE name = 'CK_Pet_Level')
BEGIN
    ALTER TABLE Pet ADD CONSTRAINT CK_Pet_Level CHECK (Level >= 1 AND Level <= 250);
    PRINT '✓ 寵物等級約束建立完成';
END

PRINT '🎉 GameCore 資料庫結構建立完成！';
PRINT '📊 包含 ' + CAST((SELECT COUNT(*) FROM sys.tables) AS VARCHAR) + ' 個資料表';
PRINT '🔗 包含 ' + CAST((SELECT COUNT(*) FROM sys.foreign_keys) AS VARCHAR) + ' 個外鍵約束';
PRINT '📈 包含 ' + CAST((SELECT COUNT(*) FROM sys.indexes WHERE is_primary_key = 0) AS VARCHAR) + ' 個效能索引';
PRINT '✅ 資料庫已準備就緒，可以開始使用 GameCore 平台！';
GO