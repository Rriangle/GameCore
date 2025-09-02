using Microsoft.EntityFrameworkCore;
using GameCore.Domain.Entities;
using GameCore.Domain.Enums;
using GameCore.Domain.Interfaces;

namespace GameCore.Infrastructure.Data
{
    /// <summary>
    /// GameCore 資�?庫�?下�?
    /// 完全?�照?��??��??�庫結�?設�?，�??��??��??�表?��??��?�?
    /// </summary>
    public class GameCoreDbContext : DbContext
    {
        public GameCoreDbContext(DbContextOptions<GameCoreDbContext> options) : base(options)
        {
        }

        #region 使用?�相?��??�表
        /// <summary>
        /// 使用?�基?��??�表
        /// </summary>
        public DbSet<User> Users { get; set; }

        /// <summary>
        /// 使用?��?紹表
        /// </summary>
        public DbSet<UserIntroduce> UserIntroduces { get; set; }

        /// <summary>
        /// 使用?��??�表
        /// </summary>
        public DbSet<UserRights> UserRights { get; set; }

        /// <summary>
        /// 使用?�錢?�表
        /// </summary>
        public DbSet<UserWallet> UserWallets { get; set; }

        /// <summary>
        /// ?�通銷?��??�表
        /// </summary>
        public DbSet<MemberSalesProfile> MemberSalesProfiles { get; set; }

        /// <summary>
        /// 使用?�銷?��?訊表
        /// </summary>
        public DbSet<UserSalesInformation> UserSalesInformations { get; set; }
        #endregion

        #region 管�??�相?��??�表
        /// <summary>
        /// 管�??��??�表
        /// </summary>
        public DbSet<ManagerData> ManagerData { get; set; }

        /// <summary>
        /// 角色權�?定義�?
        /// </summary>
        public DbSet<ManagerRolePermission> ManagerRolePermissions { get; set; }

        /// <summary>
        /// 管�??��??��?派表
        /// </summary>
        public DbSet<GameCore.Domain.Entities.ManagerRole> ManagerRoles { get; set; }

        /// <summary>
        /// 後台管�??�表
        /// </summary>
        public DbSet<Admin> Admins { get; set; }

        /// <summary>
        /// 禁�??��?�?
        /// </summary>
        public DbSet<Mute> Mutes { get; set; }

        /// <summary>
        /// �??�?
        /// </summary>
        public DbSet<Style> Styles { get; set; }
        #endregion

        #region ?�戲?�熱度相?��??�表
        /// <summary>
        /// ?�戲主�?�?
        /// </summary>
        public DbSet<Game> Games { get; set; }

        /// <summary>
        /// ?��?來�?字典�?
        /// </summary>
        public DbSet<MetricSource> MetricSources { get; set; }

        /// <summary>
        /// ?��?字典�?
        /// </summary>
        public DbSet<Metric> Metrics { get; set; }

        /// <summary>
        /// 外部ID對�?�?
        /// </summary>
        public DbSet<GameSourceMap> GameSourceMaps { get; set; }

        /// <summary>
        /// 每天每�?標�??��??�表
        /// </summary>
        public DbSet<GameMetricDaily> GameMetricDailies { get; set; }

        /// <summary>
        /// 每日?�度?�數�?
        /// </summary>
        public DbSet<PopularityIndexDaily> PopularityIndexDailies { get; set; }

        /// <summary>
        /// 榜單快照�?
        /// </summary>
        public DbSet<LeaderboardSnapshot> LeaderboardSnapshots { get; set; }
        #endregion

        #region 貼�??��?壇相?��??�表
        /// <summary>
        /// 統�?貼�?�?
        /// </summary>
        public DbSet<Post> Posts { get; set; }

        /// <summary>
        /// 洞�??��??��??��?快照�?
        /// </summary>
        public DbSet<PostMetricSnapshot> PostMetricSnapshots { get; set; }

        /// <summary>
        /// 論�??�主檔表
        /// </summary>
        public DbSet<Forum> Forums { get; set; }

        /// <summary>
        /// ?�內主�?�?
        /// </summary>
        public DbSet<GameCore.Domain.Entities.Thread> Threads { get; set; }

        /// <summary>
        /// 主�??��?�?
        /// </summary>
        public DbSet<ThreadPost> ThreadPosts { get; set; }

        /// <summary>
        /// ?�用讚表
        /// </summary>
        public DbSet<Reaction> Reactions { get; set; }

        /// <summary>
        /// ?�用?��?�?
        /// </summary>
        public DbSet<Bookmark> Bookmarks { get; set; }
        #endregion

        #region 寵物?��??�相?��??�表
        /// <summary>
        /// 使用?�簽?�統計表
        /// </summary>
        public DbSet<UserSignInStats> UserSignInStats { get; set; }

        /// <summary>
        /// 寵物?�?�表
        /// </summary>
        public DbSet<Pet> Pets { get; set; }

        /// <summary>
        /// 小�??��??��??�表
        /// </summary>
        public DbSet<MiniGame> MiniGames { get; set; }
        #endregion

        #region ?��??��?資�?�?
        /// <summary>
        /// 供�??�表
        /// </summary>
        public DbSet<Supplier> Suppliers { get; set; }

        /// <summary>
        /// ?��?資�?�?
        /// </summary>
        public DbSet<ProductInfo> ProductInfos { get; set; }

        /// <summary>
        /// ?�戲主�??��?資�?�?
        /// </summary>
        public DbSet<GameProductDetails> GameProductDetails { get; set; }

        /// <summary>
        /// ?��??�主檔�??��?訊表
        /// </summary>
        public DbSet<OtherProductDetails> OtherProductDetails { get; set; }

        /// <summary>
        /// 訂單資�?�?
        /// </summary>
        public DbSet<OrderInfo> OrderInfos { get; set; }

        /// <summary>
        /// 訂單詳細�?
        /// </summary>
        public DbSet<OrderItem> OrderItems { get; set; }

        /// <summary>
        /// 官方?��??��?榜表
        /// </summary>
        public DbSet<OfficialStoreRanking> OfficialStoreRankings { get; set; }

        /// <summary>
        /// ?��?修改?��?�?
        /// </summary>
        public DbSet<ProductInfoAuditLog> ProductInfoAuditLogs { get; set; }
        #endregion

        #region ?�家市場?��?資�?�?
        /// <summary>
        /// ?�由市場?��?資�?�?
        /// </summary>
        public DbSet<PlayerMarketProductInfo> PlayerMarketProductInfos { get; set; }

        /// <summary>
        /// ?�由市場?��??��?�?
        /// </summary>
        public DbSet<PlayerMarketProductImg> PlayerMarketProductImgs { get; set; }

        /// <summary>
        /// ?�由市場訂單�?
        /// </summary>
        public DbSet<PlayerMarketOrderInfo> PlayerMarketOrderInfos { get; set; }

        /// <summary>
        /// 交�?中�??�表
        /// </summary>
        public DbSet<PlayerMarketOrderTradepage> PlayerMarketOrderTradepages { get; set; }

        /// <summary>
        /// ?�由市場交�??�面對話�?
        /// </summary>
        public DbSet<PlayerMarketTradeMsg> PlayerMarketTradeMsgs { get; set; }

        /// <summary>
        /// ?�由市場?��?榜表
        /// </summary>
        public DbSet<PlayerMarketRanking> PlayerMarketRankings { get; set; }
        #endregion

        #region ?�知?�社交相?��??�表
        /// <summary>
        /// ?�知來�?�?
        /// </summary>
        public DbSet<NotificationSource> NotificationSources { get; set; }

        /// <summary>
        /// ?�知行為�?
        /// </summary>
        public DbSet<NotificationAction> NotificationActions { get; set; }

        /// <summary>
        /// ?�知主表
        /// </summary>
        public DbSet<Notification> Notifications { get; set; }

        /// <summary>
        /// ?�知?�收?�表
        /// </summary>
        public DbSet<NotificationRecipient> NotificationRecipients { get; set; }

        /// <summary>
        /// ?�天訊息�?
        /// </summary>
        public DbSet<ChatMessage> ChatMessages { get; set; }

        /// <summary>
        /// 群�?�?
        /// </summary>
        public DbSet<Group> Groups { get; set; }

        /// <summary>
        /// 群�??�員�?
        /// </summary>
        public DbSet<GroupMember> GroupMembers { get; set; }

        /// <summary>
        /// 群�?專用?�天�?
        /// </summary>
        public DbSet<GroupChat> GroupChats { get; set; }

        /// <summary>
        /// 封�?�?(群�?專用)
        /// </summary>
        public DbSet<GroupBlock> GroupBlocks { get; set; }

        /// <summary>
        /// 聊天室表
        /// </summary>
        public DbSet<ChatRoom> ChatRooms { get; set; }

        /// <summary>
        /// 聊天室成員表
        /// </summary>
        public DbSet<ChatRoomMember> ChatRoomMembers { get; set; }

        /// <summary>
        /// 私聊表
        /// </summary>
        public DbSet<PrivateChat> PrivateChats { get; set; }

        /// <summary>
        /// 私聊訊息表
        /// </summary>
        public DbSet<PrivateMessage> PrivateMessages { get; set; }

        /// <summary>
        /// 貼文回覆表
        /// </summary>
        public DbSet<PostReply> PostReplies { get; set; }

        /// <summary>
        /// 小遊戲記錄表
        /// </summary>
        public DbSet<MiniGameRecord> MiniGameRecords { get; set; }

        /// <summary>
        /// 小遊戲設定表
        /// </summary>
        public DbSet<MiniGameSettings> MiniGameSettings { get; set; }

        /// <summary>
        /// 簽到記錄表
        /// </summary>
        public DbSet<SignInRecord> SignInRecords { get; set; }

        /// <summary>
        /// 簽到統計表
        /// </summary>
        public DbSet<SignInStatistics> SignInStatistics { get; set; }

        /// <summary>
        /// 市場交易表
        /// </summary>
        public DbSet<MarketTransaction> MarketTransactions { get; set; }

        /// <summary>
        /// 管理員表
        /// </summary>
        public DbSet<Manager> Managers { get; set; }

        /// <summary>
        /// 商品表
        /// </summary>
        public DbSet<Product> Products { get; set; }
        #endregion

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            #region 設�??��?索�?約�?
            // 調整使用者唯一索引以符合實體屬性（相容性）
            modelBuilder.Entity<User>()
                .HasIndex(u => u.User_Account)
                .IsUnique()
                .HasDatabaseName("IX_Users_UserAccount_Unique");

            modelBuilder.Entity<User>()
                .HasIndex(u => u.User_name)
                .IsUnique()
                .HasDatabaseName("IX_Users_UserName_Unique");

            // ManagerData 帳號唯一索引 - 使用 Username 對應
            modelBuilder.Entity<ManagerData>()
                .HasIndex(m => m.Username)
                .IsUnique()
                .HasDatabaseName("IX_ManagerData_ManagerAccount_Unique");

            // 取消不存在的 1:1 Post ↔ PostMetricSnapshot 配置，改為一對多集合已存在
            // 保留論壇、使用者等關聯設定

            // ChatMessage 索引修正：使用現有屬性
            modelBuilder.Entity<ChatMessage>()
                .HasIndex(cm => cm.CreatedAt)
                .HasDatabaseName("IX_ChatMessages_SentAt");

            // 取消 ReceiverId 索引因屬性為相容性 NotMapped

            // 玩家市場索引修正：使用 Status 欄位
            modelBuilder.Entity<PlayerMarketProductInfo>()
                .HasIndex(p => new { p.SellerId, p.Status, p.CreatedAt })
                .HasDatabaseName("IX_PlayerMarketProductInfo_SellerId_Status_CreatedAt");

            // 檢查約束使用 EntityTypeBuilder 而非 PropertyBuilder
            modelBuilder.Entity<Pet>()
                .ToTable(tb =>
                {
                    tb.HasCheckConstraint("CK_Pet_Hunger", "[Hunger] >= 0 AND [Hunger] <= 100");
                    tb.HasCheckConstraint("CK_Pet_Mood", "[Mood] >= 0 AND [Mood] <= 100");
                    tb.HasCheckConstraint("CK_Pet_Stamina", "[Stamina] >= 0 AND [Stamina] <= 100");
                    tb.HasCheckConstraint("CK_Pet_Cleanliness", "[Cleanliness] >= 0 AND [Cleanliness] <= 100");
                    tb.HasCheckConstraint("CK_Pet_Health", "[Health] >= 0 AND [Health] <= 100");
                    tb.HasCheckConstraint("CK_Pet_Level", "[Level] >= 1 AND [Level] <= 250");
                });
            #endregion

            #region 設?複?主鍵
            // 管?????派??主??
            modelBuilder.Entity<GameCore.Domain.Entities.ManagerRole>()
                .HasKey(mr => new { mr.ManagerId, mr.ManagerRoleId });

            // 群??員複?主鍵
            modelBuilder.Entity<GroupMember>()
                .HasKey(gm => new { gm.GroupId, gm.UserId });
            #endregion

            #region 設?一對??聯
            // 使用??介紹一對?
            modelBuilder.Entity<User>()
                .HasOne(u => u.UserIntroduce)
                .WithOne(ui => ui.User)
                .HasForeignKey<UserIntroduce>(ui => ui.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // 使用??權?一對?
            modelBuilder.Entity<User>()
                .HasOne(u => u.UserRights)
                .WithOne(ur => ur.User)
                .HasForeignKey<UserRights>(ur => ur.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // 使用????一對?
            modelBuilder.Entity<User>()
                .HasOne(u => u.UserWallet)
                .WithOne(uw => uw.User)
                .HasForeignKey<UserWallet>(uw => uw.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // 使用??寵物一對?
            modelBuilder.Entity<User>()
                .HasOne(u => u.Pet)
                .WithOne(p => p.User)
                .HasForeignKey<Pet>(p => p.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // ?戲??壇?對?
            modelBuilder.Entity<Game>()
                .HasOne(g => g.Forum)
                .WithOne(f => f.Game)
                .HasForeignKey<Forum>(f => f.GameId)
                .OnDelete(DeleteBehavior.Cascade);

            // 貼??快??對?
            modelBuilder.Entity<Post>()
                .HasOne(p => p.PostMetricSnapshot)
                .WithOne(pms => pms.Post)
                .HasForeignKey<PostMetricSnapshot>(pms => pms.PostId)
                .OnDelete(DeleteBehavior.Cascade);
            #endregion

            #region 設??能索?
            // 簽到記?索? (常用?查詢使?者簽?歷??
            modelBuilder.Entity<UserSignInStats>()
                .HasIndex(uss => new { uss.UserId, uss.SignInTime })
                .HasDatabaseName("IX_UserSignInStats_UserId_SignInTime");

            // 小????索?
            modelBuilder.Entity<MiniGame>()
                .HasIndex(mg => new { mg.UserId, mg.StartTime })
                .HasDatabaseName("IX_MiniGame_UserId_StartTime");

            // 論???索?
            modelBuilder.Entity<GameCore.Domain.Entities.Thread>()
                .HasIndex(t => new { t.ForumId, t.UpdatedAt })
                .HasDatabaseName("IX_Threads_ForumId_UpdatedAt");

            modelBuilder.Entity<ThreadPost>()
                .HasIndex(tp => new { tp.ThreadId, tp.CreatedAt })
                .HasDatabaseName("IX_ThreadPosts_ThreadId_CreatedAt");

            // ?知??索?
            modelBuilder.Entity<NotificationRecipient>()
                .HasIndex(nr => new { nr.UserId, nr.IsRead, nr.RecipientId })
                .HasDatabaseName("IX_NotificationRecipients_Inbox");

            modelBuilder.Entity<ChatMessage>()
                .HasIndex(cm => cm.CreatedAt)
                .HasDatabaseName("IX_ChatMessages_SentAt");

            modelBuilder.Entity<ChatMessage>()
                .HasIndex(cm => new { cm.ReceiverId, cm.SentAt })
                .HasDatabaseName("IX_ChatMessages_ReceiverId_SentAt");

            // ?��??��?索�?
            modelBuilder.Entity<OrderInfo>()
                .HasIndex(oi => new { oi.UserId, oi.OrderStatus, oi.OrderDate })
                .HasDatabaseName("IX_OrderInfo_UserId_OrderStatus_OrderDate");

            modelBuilder.Entity<OrderItem>()
                .HasIndex(oi => oi.OrderId)
                .HasDatabaseName("IX_OrderItems_OrderId");

            // ?�家市場索�?
            modelBuilder.Entity<PlayerMarketProductInfo>()
                .HasIndex(p => new { p.SellerId, p.Status, p.CreatedAt })
                .HasDatabaseName("IX_PlayerMarketProductInfo_SellerId_Status_CreatedAt");

            modelBuilder.Entity<PlayerMarketOrderInfo>()
                .HasIndex(pmoi => new { pmoi.BuyerId, pmoi.SellerId, pmoi.POrderStatus })
                .HasDatabaseName("IX_PlayerMarketOrderInfo_BuyerId_SellerId_Status");

            // ?�度?��?索�?
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

            #region 設�?資�?表�??��?�?
            // 設�? ChatMessage ?��??��??��???
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

            // 設�? PlayerMarketOrderInfo ?�買�?��?�聯
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

            // 設�? GroupBlock ?��??��???
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

            #region 設�??�設??
            // 設�??��?欄�??�設?�為 UTC ?��?
            modelBuilder.Entity<UserIntroduce>()
                .Property(ui => ui.CreateAccount)
                .HasDefaultValueSql("GETUTCDATE()");

            modelBuilder.Entity<Pet>()
                .Property(p => p.LevelUpTime)
                .HasDefaultValueSql("GETUTCDATE()");

            modelBuilder.Entity<UserSignInStats>()
                .Property(uss => uss.SignInTime)
                .HasDefaultValueSql("GETUTCDATE()");

            modelBuilder.Entity<MiniGame>()
                .Property(mg => mg.StartTime)
                .HasDefaultValueSql("GETUTCDATE()");
            #endregion

            #region 設�?資�?驗�?約�?
            // 寵物屬性值�???(0-100)
            modelBuilder.Entity<Pet>()
                .ToTable(tb =>
                {
                    tb.HasCheckConstraint("CK_Pet_Hunger", "[Hunger] >= 0 AND [Hunger] <= 100");
                    tb.HasCheckConstraint("CK_Pet_Mood", "[Mood] >= 0 AND [Mood] <= 100");
                    tb.HasCheckConstraint("CK_Pet_Stamina", "[Stamina] >= 0 AND [Stamina] <= 100");
                    tb.HasCheckConstraint("CK_Pet_Cleanliness", "[Cleanliness] >= 0 AND [Cleanliness] <= 100");
                    tb.HasCheckConstraint("CK_Pet_Health", "[Health] >= 0 AND [Health] <= 100");
                    tb.HasCheckConstraint("CK_Pet_Level", "[Level] >= 1 AND [Level] <= 250");
                });
            #endregion
        }
    }
}
