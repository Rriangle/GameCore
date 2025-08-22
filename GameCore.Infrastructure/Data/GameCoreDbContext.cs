using Microsoft.EntityFrameworkCore;
using GameCore.Core.Entities;

namespace GameCore.Infrastructure.Data
{
    /// <summary>
    /// GameCore 資料庫上下文
    /// 完全按照提供的資料庫結構設計，包含所有資料表和關聯關係
    /// </summary>
    public class GameCoreDbContext : DbContext
    {
        public GameCoreDbContext(DbContextOptions<GameCoreDbContext> options) : base(options)
        {
        }

        #region 使用者相關資料表
        /// <summary>
        /// 使用者基本資料表
        /// </summary>
        public DbSet<User> Users { get; set; }

        /// <summary>
        /// 使用者介紹表
        /// </summary>
        public DbSet<UserIntroduce> UserIntroduces { get; set; }

        /// <summary>
        /// 使用者權限表
        /// </summary>
        public DbSet<UserRights> UserRights { get; set; }

        /// <summary>
        /// 使用者錢包表
        /// </summary>
        public DbSet<UserWallet> UserWallets { get; set; }

        /// <summary>
        /// 開通銷售功能表
        /// </summary>
        public DbSet<MemberSalesProfile> MemberSalesProfiles { get; set; }

        /// <summary>
        /// 使用者銷售資訊表
        /// </summary>
        public DbSet<UserSalesInformation> UserSalesInformations { get; set; }
        #endregion

        #region 管理者相關資料表
        /// <summary>
        /// 管理者資料表
        /// </summary>
        public DbSet<ManagerData> ManagerData { get; set; }

        /// <summary>
        /// 角色權限定義表
        /// </summary>
        public DbSet<ManagerRolePermission> ManagerRolePermissions { get; set; }

        /// <summary>
        /// 管理者角色指派表
        /// </summary>
        public DbSet<ManagerRole> ManagerRoles { get; set; }

        /// <summary>
        /// 後台管理員表
        /// </summary>
        public DbSet<Admin> Admins { get; set; }

        /// <summary>
        /// 禁言選項表
        /// </summary>
        public DbSet<Mute> Mutes { get; set; }

        /// <summary>
        /// 樣式表
        /// </summary>
        public DbSet<Style> Styles { get; set; }
        #endregion

        #region 遊戲和熱度相關資料表
        /// <summary>
        /// 遊戲主檔表
        /// </summary>
        public DbSet<Game> Games { get; set; }

        /// <summary>
        /// 數據來源字典表
        /// </summary>
        public DbSet<MetricSource> MetricSources { get; set; }

        /// <summary>
        /// 指標字典表
        /// </summary>
        public DbSet<Metric> Metrics { get; set; }

        /// <summary>
        /// 外部ID對應表
        /// </summary>
        public DbSet<GameSourceMap> GameSourceMaps { get; set; }

        /// <summary>
        /// 每天每指標的原始值表
        /// </summary>
        public DbSet<GameMetricDaily> GameMetricDailies { get; set; }

        /// <summary>
        /// 每日熱度指數表
        /// </summary>
        public DbSet<PopularityIndexDaily> PopularityIndexDailies { get; set; }

        /// <summary>
        /// 榜單快照表
        /// </summary>
        public DbSet<LeaderboardSnapshot> LeaderboardSnapshots { get; set; }
        #endregion

        #region 貼文和論壇相關資料表
        /// <summary>
        /// 統一貼文表
        /// </summary>
        public DbSet<Post> Posts { get; set; }

        /// <summary>
        /// 洞察發佈時的數據快照表
        /// </summary>
        public DbSet<PostMetricSnapshot> PostMetricSnapshots { get; set; }

        /// <summary>
        /// 洞察引用來源清單表
        /// </summary>
        public DbSet<PostSource> PostSources { get; set; }

        /// <summary>
        /// 論壇版主檔表
        /// </summary>
        public DbSet<Forum> Forums { get; set; }

        /// <summary>
        /// 版內主題表
        /// </summary>
        public DbSet<Thread> Threads { get; set; }

        /// <summary>
        /// 主題回覆表
        /// </summary>
        public DbSet<ThreadPost> ThreadPosts { get; set; }

        /// <summary>
        /// 通用讚表
        /// </summary>
        public DbSet<Reaction> Reactions { get; set; }

        /// <summary>
        /// 通用收藏表
        /// </summary>
        public DbSet<Bookmark> Bookmarks { get; set; }
        #endregion

        #region 寵物和遊戲相關資料表
        /// <summary>
        /// 使用者簽到統計表
        /// </summary>
        public DbSet<UserSignInStats> UserSignInStats { get; set; }

        /// <summary>
        /// 寵物狀態表
        /// </summary>
        public DbSet<Pet> Pets { get; set; }

        /// <summary>
        /// 小冒險遊戲紀錄表
        /// </summary>
        public DbSet<MiniGame> MiniGames { get; set; }
        #endregion

        #region 商城相關資料表
        /// <summary>
        /// 供應商表
        /// </summary>
        public DbSet<Supplier> Suppliers { get; set; }

        /// <summary>
        /// 商品資訊表
        /// </summary>
        public DbSet<ProductInfo> ProductInfos { get; set; }

        /// <summary>
        /// 遊戲主檔商品資訊表
        /// </summary>
        public DbSet<GameProductDetails> GameProductDetails { get; set; }

        /// <summary>
        /// 非遊戲主檔商品資訊表
        /// </summary>
        public DbSet<OtherProductDetails> OtherProductDetails { get; set; }

        /// <summary>
        /// 訂單資訊表
        /// </summary>
        public DbSet<OrderInfo> OrderInfos { get; set; }

        /// <summary>
        /// 訂單詳細表
        /// </summary>
        public DbSet<OrderItem> OrderItems { get; set; }

        /// <summary>
        /// 官方商城排行榜表
        /// </summary>
        public DbSet<OfficialStoreRanking> OfficialStoreRankings { get; set; }

        /// <summary>
        /// 商品修改日誌表
        /// </summary>
        public DbSet<ProductInfoAuditLog> ProductInfoAuditLogs { get; set; }
        #endregion

        #region 玩家市場相關資料表
        /// <summary>
        /// 自由市場商品資訊表
        /// </summary>
        public DbSet<PlayerMarketProductInfo> PlayerMarketProductInfos { get; set; }

        /// <summary>
        /// 自由市場商品圖片表
        /// </summary>
        public DbSet<PlayerMarketProductImg> PlayerMarketProductImgs { get; set; }

        /// <summary>
        /// 自由市場訂單表
        /// </summary>
        public DbSet<PlayerMarketOrderInfo> PlayerMarketOrderInfos { get; set; }

        /// <summary>
        /// 交易中頁面表
        /// </summary>
        public DbSet<PlayerMarketOrderTradepage> PlayerMarketOrderTradepages { get; set; }

        /// <summary>
        /// 自由市場交易頁面對話表
        /// </summary>
        public DbSet<PlayerMarketTradeMsg> PlayerMarketTradeMsgs { get; set; }

        /// <summary>
        /// 自由市場排行榜表
        /// </summary>
        public DbSet<PlayerMarketRanking> PlayerMarketRankings { get; set; }
        #endregion

        #region 通知和社交相關資料表
        /// <summary>
        /// 通知來源表
        /// </summary>
        public DbSet<NotificationSource> NotificationSources { get; set; }

        /// <summary>
        /// 通知行為表
        /// </summary>
        public DbSet<NotificationAction> NotificationActions { get; set; }

        /// <summary>
        /// 通知主表
        /// </summary>
        public DbSet<Notification> Notifications { get; set; }

        /// <summary>
        /// 通知接收者表
        /// </summary>
        public DbSet<NotificationRecipient> NotificationRecipients { get; set; }

        /// <summary>
        /// 聊天訊息表
        /// </summary>
        public DbSet<ChatMessage> ChatMessages { get; set; }

        /// <summary>
        /// 群組表
        /// </summary>
        public DbSet<Group> Groups { get; set; }

        /// <summary>
        /// 群組成員表
        /// </summary>
        public DbSet<GroupMember> GroupMembers { get; set; }

        /// <summary>
        /// 群組專用聊天表
        /// </summary>
        public DbSet<GroupChat> GroupChats { get; set; }

        /// <summary>
        /// 封鎖表 (群組專用)
        /// </summary>
        public DbSet<GroupBlock> GroupBlocks { get; set; }
        #endregion

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            #region 設定唯一索引約束
            // 使用者相關唯一約束
            modelBuilder.Entity<User>()
                .HasIndex(u => u.UserAccount)
                .IsUnique()
                .HasDatabaseName("IX_Users_UserAccount_Unique");

            modelBuilder.Entity<User>()
                .HasIndex(u => u.UserName)
                .IsUnique()
                .HasDatabaseName("IX_Users_UserName_Unique");

            modelBuilder.Entity<UserIntroduce>()
                .HasIndex(ui => ui.UserNickName)
                .IsUnique()
                .HasDatabaseName("IX_UserIntroduce_UserNickName_Unique");

            modelBuilder.Entity<UserIntroduce>()
                .HasIndex(ui => ui.IdNumber)
                .IsUnique()
                .HasDatabaseName("IX_UserIntroduce_IdNumber_Unique");

            modelBuilder.Entity<UserIntroduce>()
                .HasIndex(ui => ui.Cellphone)
                .IsUnique()
                .HasDatabaseName("IX_UserIntroduce_Cellphone_Unique");

            modelBuilder.Entity<UserIntroduce>()
                .HasIndex(ui => ui.Email)
                .IsUnique()
                .HasDatabaseName("IX_UserIntroduce_Email_Unique");

            // 管理者帳號唯一約束
            modelBuilder.Entity<ManagerData>()
                .HasIndex(m => m.ManagerAccount)
                .IsUnique()
                .HasDatabaseName("IX_ManagerData_ManagerAccount_Unique");

            // 遊戲和來源對應唯一約束
            modelBuilder.Entity<GameSourceMap>()
                .HasIndex(gsm => new { gsm.GameId, gsm.SourceId })
                .IsUnique()
                .HasDatabaseName("IX_GameSourceMap_GameId_SourceId_Unique");

            // 來源和指標代碼唯一約束
            modelBuilder.Entity<Metric>()
                .HasIndex(m => new { m.SourceId, m.Code })
                .IsUnique()
                .HasDatabaseName("IX_Metrics_SourceId_Code_Unique");

            // 遊戲每日指標唯一約束 (UPSERT 防重)
            modelBuilder.Entity<GameMetricDaily>()
                .HasIndex(gmd => new { gmd.GameId, gmd.MetricId, gmd.Date })
                .IsUnique()
                .HasDatabaseName("IX_GameMetricDaily_GameId_MetricId_Date_Unique");

            // 每日熱度指數唯一約束
            modelBuilder.Entity<PopularityIndexDaily>()
                .HasIndex(pid => new { pid.GameId, pid.Date })
                .IsUnique()
                .HasDatabaseName("IX_PopularityIndexDaily_GameId_Date_Unique");

            // 論壇遊戲一對一約束
            modelBuilder.Entity<Forum>()
                .HasIndex(f => f.GameId)
                .IsUnique()
                .HasDatabaseName("IX_Forums_GameId_Unique");

            // 通知接收者不重複約束
            modelBuilder.Entity<NotificationRecipient>()
                .HasIndex(nr => new { nr.NotificationId, nr.UserId })
                .IsUnique()
                .HasDatabaseName("IX_NotificationRecipients_NotificationId_UserId_Unique");

            // 群組封鎖唯一約束
            modelBuilder.Entity<GroupBlock>()
                .HasIndex(gb => new { gb.GroupId, gb.UserId })
                .IsUnique()
                .HasDatabaseName("IX_GroupBlock_GroupId_UserId_Unique");

            // 反應唯一約束 (同一人對同一目標同一類型只能有一個反應)
            modelBuilder.Entity<Reaction>()
                .HasIndex(r => new { r.UserId, r.TargetType, r.TargetId, r.Kind })
                .IsUnique()
                .HasDatabaseName("IX_Reactions_UserId_TargetType_TargetId_Kind_Unique");

            // 收藏唯一約束
            modelBuilder.Entity<Bookmark>()
                .HasIndex(b => new { b.UserId, b.TargetType, b.TargetId })
                .IsUnique()
                .HasDatabaseName("IX_Bookmarks_UserId_TargetType_TargetId_Unique");
            #endregion

            #region 設定複合主鍵
            // 管理者角色指派複合主鍵
            modelBuilder.Entity<ManagerRole>()
                .HasKey(mr => new { mr.ManagerId, mr.ManagerRoleId });

            // 群組成員複合主鍵
            modelBuilder.Entity<GroupMember>()
                .HasKey(gm => new { gm.GroupId, gm.UserId });
            #endregion

            #region 設定一對一關聯
            // 使用者與介紹一對一
            modelBuilder.Entity<User>()
                .HasOne(u => u.UserIntroduce)
                .WithOne(ui => ui.User)
                .HasForeignKey<UserIntroduce>(ui => ui.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // 使用者與權限一對一
            modelBuilder.Entity<User>()
                .HasOne(u => u.UserRights)
                .WithOne(ur => ur.User)
                .HasForeignKey<UserRights>(ur => ur.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // 使用者與錢包一對一
            modelBuilder.Entity<User>()
                .HasOne(u => u.UserWallet)
                .WithOne(uw => uw.User)
                .HasForeignKey<UserWallet>(uw => uw.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // 使用者與寵物一對一
            modelBuilder.Entity<User>()
                .HasOne(u => u.Pet)
                .WithOne(p => p.User)
                .HasForeignKey<Pet>(p => p.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // 遊戲與論壇一對一
            modelBuilder.Entity<Game>()
                .HasOne(g => g.Forum)
                .WithOne(f => f.Game)
                .HasForeignKey<Forum>(f => f.GameId)
                .OnDelete(DeleteBehavior.Cascade);

            // 貼文與快照一對一
            modelBuilder.Entity<Post>()
                .HasOne(p => p.PostMetricSnapshot)
                .WithOne(pms => pms.Post)
                .HasForeignKey<PostMetricSnapshot>(pms => pms.PostId)
                .OnDelete(DeleteBehavior.Cascade);
            #endregion

            #region 設定效能索引
            // 簽到記錄索引 (常用於查詢使用者簽到歷史)
            modelBuilder.Entity<UserSignInStats>()
                .HasIndex(uss => new { uss.UserId, uss.SignTime })
                .HasDatabaseName("IX_UserSignInStats_UserId_SignTime");

            // 小遊戲記錄索引
            modelBuilder.Entity<MiniGame>()
                .HasIndex(mg => new { mg.UserId, mg.StartTime })
                .HasDatabaseName("IX_MiniGame_UserId_StartTime");

            // 論壇相關索引
            modelBuilder.Entity<Thread>()
                .HasIndex(t => new { t.ForumId, t.UpdatedAt })
                .HasDatabaseName("IX_Threads_ForumId_UpdatedAt");

            modelBuilder.Entity<ThreadPost>()
                .HasIndex(tp => new { tp.ThreadId, tp.CreatedAt })
                .HasDatabaseName("IX_ThreadPosts_ThreadId_CreatedAt");

            // 通知相關索引
            modelBuilder.Entity<NotificationRecipient>()
                .HasIndex(nr => new { nr.UserId, nr.IsRead, nr.RecipientId })
                .HasDatabaseName("IX_NotificationRecipients_Inbox");

            modelBuilder.Entity<ChatMessage>()
                .HasIndex(cm => cm.SentAt)
                .HasDatabaseName("IX_ChatMessages_SentAt");

            modelBuilder.Entity<ChatMessage>()
                .HasIndex(cm => new { cm.ReceiverId, cm.SentAt })
                .HasDatabaseName("IX_ChatMessages_ReceiverId_SentAt");

            // 商城相關索引
            modelBuilder.Entity<OrderInfo>()
                .HasIndex(oi => new { oi.UserId, oi.OrderStatus, oi.OrderDate })
                .HasDatabaseName("IX_OrderInfo_UserId_OrderStatus_OrderDate");

            modelBuilder.Entity<OrderItem>()
                .HasIndex(oi => oi.OrderId)
                .HasDatabaseName("IX_OrderItems_OrderId");

            // 玩家市場索引
            modelBuilder.Entity<PlayerMarketProductInfo>()
                .HasIndex(pmpi => new { pmpi.SellerId, pmpi.PStatus, pmpi.CreatedAt })
                .HasDatabaseName("IX_PlayerMarketProductInfo_SellerId_Status_CreatedAt");

            modelBuilder.Entity<PlayerMarketOrderInfo>()
                .HasIndex(pmoi => new { pmoi.BuyerId, pmoi.SellerId, pmoi.POrderStatus })
                .HasDatabaseName("IX_PlayerMarketOrderInfo_BuyerId_SellerId_Status");

            // 熱度相關索引
            modelBuilder.Entity<GameMetricDaily>()
                .HasIndex(gmd => new { gmd.Date, gmd.MetricId })
                .HasDatabaseName("IX_GameMetricDaily_Date_MetricId");

            modelBuilder.Entity<GameMetricDaily>()
                .HasIndex(gmd => new { gmd.GameId, gmd.Date })
                .HasDatabaseName("IX_GameMetricDaily_GameId_Date");

            modelBuilder.Entity<LeaderboardSnapshot>()
                .HasIndex(ls => new { ls.Period, ls.Ts, ls.Rank })
                .HasDatabaseName("IX_LeaderboardSnapshots_Period_Ts_Rank");

            modelBuilder.Entity<LeaderboardSnapshot>()
                .HasIndex(ls => new { ls.Period, ls.Ts, ls.Rank, ls.GameId })
                .IsUnique()
                .HasDatabaseName("IX_LeaderboardSnapshots_Period_Ts_Rank_GameId_Unique");
            #endregion

            #region 設定資料表關聯關係
            // 設定 ChatMessage 的多重外鍵關聯
            modelBuilder.Entity<ChatMessage>()
                .HasOne(cm => cm.SenderUser)
                .WithMany(u => u.SentMessages)
                .HasForeignKey(cm => cm.SenderId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ChatMessage>()
                .HasOne(cm => cm.ReceiverUser)
                .WithMany(u => u.ReceivedMessages)
                .HasForeignKey(cm => cm.ReceiverId)
                .OnDelete(DeleteBehavior.Restrict);

            // 設定 PlayerMarketOrderInfo 的買賣家關聯
            modelBuilder.Entity<PlayerMarketOrderInfo>()
                .HasOne(pmoi => pmoi.Seller)
                .WithMany()
                .HasForeignKey(pmoi => pmoi.SellerId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<PlayerMarketOrderInfo>()
                .HasOne(pmoi => pmoi.Buyer)
                .WithMany(u => u.PlayerMarketOrders)
                .HasForeignKey(pmoi => pmoi.BuyerId)
                .OnDelete(DeleteBehavior.Restrict);

            // 設定 GroupBlock 的封鎖關聯
            modelBuilder.Entity<GroupBlock>()
                .HasOne(gb => gb.BlockedUser)
                .WithMany()
                .HasForeignKey(gb => gb.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<GroupBlock>()
                .HasOne(gb => gb.BlockedByUser)
                .WithMany()
                .HasForeignKey(gb => gb.BlockedBy)
                .OnDelete(DeleteBehavior.Restrict);
            #endregion

            #region 設定預設值
            // 設定時間欄位預設值為 UTC 時間
            modelBuilder.Entity<UserIntroduce>()
                .Property(ui => ui.CreateAccount)
                .HasDefaultValueSql("GETUTCDATE()");

            modelBuilder.Entity<Pet>()
                .Property(p => p.LevelUpTime)
                .HasDefaultValueSql("GETUTCDATE()");

            modelBuilder.Entity<UserSignInStats>()
                .Property(uss => uss.SignTime)
                .HasDefaultValueSql("GETUTCDATE()");

            modelBuilder.Entity<MiniGame>()
                .Property(mg => mg.StartTime)
                .HasDefaultValueSql("GETUTCDATE()");
            #endregion

            #region 設定資料驗證約束
            // 寵物屬性值約束 (0-100)
            modelBuilder.Entity<Pet>()
                .Property(p => p.Hunger)
                .HasCheckConstraint("CK_Pet_Hunger", "[Hunger] >= 0 AND [Hunger] <= 100");

            modelBuilder.Entity<Pet>()
                .Property(p => p.Mood)
                .HasCheckConstraint("CK_Pet_Mood", "[Mood] >= 0 AND [Mood] <= 100");

            modelBuilder.Entity<Pet>()
                .Property(p => p.Stamina)
                .HasCheckConstraint("CK_Pet_Stamina", "[Stamina] >= 0 AND [Stamina] <= 100");

            modelBuilder.Entity<Pet>()
                .Property(p => p.Cleanliness)
                .HasCheckConstraint("CK_Pet_Cleanliness", "[Cleanliness] >= 0 AND [Cleanliness] <= 100");

            modelBuilder.Entity<Pet>()
                .Property(p => p.Health)
                .HasCheckConstraint("CK_Pet_Health", "[Health] >= 0 AND [Health] <= 100");

            // 寵物等級約束 (1-250)
            modelBuilder.Entity<Pet>()
                .Property(p => p.Level)
                .HasCheckConstraint("CK_Pet_Level", "[Level] >= 1 AND [Level] <= 250");
            #endregion
        }
    }
}