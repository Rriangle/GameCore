using GameCore.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace GameCore.Infrastructure.Data
{
    /// <summary>
    /// GameCore 專案的主要資料庫上下文
    /// </summary>
    public class GameCoreDbContext : DbContext
    {
        public GameCoreDbContext(DbContextOptions<GameCoreDbContext> options) : base(options)
        {
        }

        // 使用者相關
        public DbSet<User> Users { get; set; }
        public DbSet<UserIntroduce> UserIntroduces { get; set; }
        public DbSet<UserRights> UserRights { get; set; }
        public DbSet<UserWallet> UserWallets { get; set; }
        public DbSet<MemberSalesProfile> MemberSalesProfiles { get; set; }
        public DbSet<UserSalesInformation> UserSalesInformations { get; set; }

        // 寵物與遊戲相關
        public DbSet<Pet> Pets { get; set; }
        public DbSet<UserSignInStats> UserSignInStats { get; set; }
        public DbSet<MiniGame> MiniGames { get; set; }

        // 管理員相關
        public DbSet<ManagerData> ManagerData { get; set; }
        public DbSet<ManagerRolePermission> ManagerRolePermissions { get; set; }
        public DbSet<ManagerRole> ManagerRoles { get; set; }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<Mute> Mutes { get; set; }
        public DbSet<Style> Styles { get; set; }

        // 熱度與排行榜相關
        public DbSet<Game> Games { get; set; }
        public DbSet<MetricSource> MetricSources { get; set; }
        public DbSet<Metric> Metrics { get; set; }
        public DbSet<GameSourceMap> GameSourceMaps { get; set; }
        public DbSet<GameMetricDaily> GameMetricDailies { get; set; }
        public DbSet<PopularityIndexDaily> PopularityIndexDailies { get; set; }
        public DbSet<LeaderboardSnapshot> LeaderboardSnapshots { get; set; }

        // 洞察貼文相關
        public DbSet<Post> Posts { get; set; }
        public DbSet<PostMetricSnapshot> PostMetricSnapshots { get; set; }
        public DbSet<PostSource> PostSources { get; set; }

        // 論壇相關
        public DbSet<Forum> Forums { get; set; }
        public DbSet<Thread> Threads { get; set; }
        public DbSet<ThreadPost> ThreadPosts { get; set; }
        public DbSet<Reaction> Reactions { get; set; }
        public DbSet<Bookmark> Bookmarks { get; set; }

        // 官方商城相關
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<ProductInfo> ProductInfos { get; set; }
        public DbSet<GameProductDetails> GameProductDetails { get; set; }
        public DbSet<OtherProductDetails> OtherProductDetails { get; set; }
        public DbSet<OrderInfo> OrderInfos { get; set; }
        public DbSet<OrderItems> OrderItems { get; set; }
        public DbSet<OfficialStoreRanking> OfficialStoreRankings { get; set; }
        public DbSet<ProductInfoAuditLog> ProductInfoAuditLogs { get; set; }

        // 自由市場相關
        public DbSet<PlayerMarketProductInfo> PlayerMarketProductInfos { get; set; }
        public DbSet<PlayerMarketProductImg> PlayerMarketProductImgs { get; set; }
        public DbSet<PlayerMarketOrderInfo> PlayerMarketOrderInfos { get; set; }
        public DbSet<PlayerMarketOrderTradepage> PlayerMarketOrderTradepages { get; set; }
        public DbSet<PlayerMarketTradeMsg> PlayerMarketTradeMsgs { get; set; }
        public DbSet<PlayerMarketRanking> PlayerMarketRankings { get; set; }

        // 社交與通知相關
        public DbSet<NotificationSource> NotificationSources { get; set; }
        public DbSet<NotificationAction> NotificationActions { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<NotificationRecipient> NotificationRecipients { get; set; }
        public DbSet<ChatMessage> ChatMessages { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<GroupMember> GroupMembers { get; set; }
        public DbSet<GroupChat> GroupChats { get; set; }
        public DbSet<GroupBlock> GroupBlocks { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // 配置複合主鍵
            ConfigureCompositeKeys(modelBuilder);

            // 配置索引
            ConfigureIndexes(modelBuilder);

            // 配置關聯關係
            ConfigureRelationships(modelBuilder);

            // 配置預設值
            ConfigureDefaultValues(modelBuilder);

            // 配置檢查約束
            ConfigureCheckConstraints(modelBuilder);
        }

        /// <summary>
        /// 配置複合主鍵
        /// </summary>
        private void ConfigureCompositeKeys(ModelBuilder modelBuilder)
        {
            // ManagerRole 複合主鍵
            modelBuilder.Entity<ManagerRole>()
                .HasKey(mr => new { mr.Manager_Id, mr.ManagerRole_Id });

            // Group_Member 複合主鍵
            modelBuilder.Entity<GroupMember>()
                .HasKey(gm => new { gm.group_id, gm.user_id });

            // Group_Block 複合主鍵
            modelBuilder.Entity<GroupBlock>()
                .HasKey(gb => new { gb.block_id, gb.group_id });
        }

        /// <summary>
        /// 配置索引
        /// </summary>
        private void ConfigureIndexes(ModelBuilder modelBuilder)
        {
            // Users 表索引
            modelBuilder.Entity<User>()
                .HasIndex(u => u.User_Account)
                .IsUnique();

            modelBuilder.Entity<User>()
                .HasIndex(u => u.User_name)
                .IsUnique();

            // User_Introduce 表索引
            modelBuilder.Entity<UserIntroduce>()
                .HasIndex(ui => ui.User_NickName)
                .IsUnique();

            modelBuilder.Entity<UserIntroduce>()
                .HasIndex(ui => ui.IdNumber)
                .IsUnique();

            modelBuilder.Entity<UserIntroduce>()
                .HasIndex(ui => ui.Cellphone)
                .IsUnique();

            modelBuilder.Entity<UserIntroduce>()
                .HasIndex(ui => ui.Email)
                .IsUnique();

            // ManagerData 表索引
            modelBuilder.Entity<ManagerData>()
                .HasIndex(m => m.Manager_Account)
                .IsUnique();

            // 遊戲相關索引
            modelBuilder.Entity<GameMetricDaily>()
                .HasIndex(gmd => new { gmd.game_id, gmd.metric_id, gmd.date })
                .IsUnique();

            modelBuilder.Entity<GameMetricDaily>()
                .HasIndex(gmd => new { gmd.date, gmd.metric_id });

            modelBuilder.Entity<GameMetricDaily>()
                .HasIndex(gmd => new { gmd.game_id, gmd.date });

            // 排行榜索引
            modelBuilder.Entity<LeaderboardSnapshot>()
                .HasIndex(ls => new { ls.period, ls.ts, ls.rank });

            modelBuilder.Entity<LeaderboardSnapshot>()
                .HasIndex(ls => new { ls.period, ls.ts, ls.rank, ls.game_id })
                .IsUnique();

            modelBuilder.Entity<LeaderboardSnapshot>()
                .HasIndex(ls => new { ls.period, ls.ts, ls.game_id });

            // 論壇相關索引
            modelBuilder.Entity<Thread>()
                .HasIndex(t => new { t.forum_id, t.updated_at });

            modelBuilder.Entity<ThreadPost>()
                .HasIndex(tp => new { tp.thread_id, tp.created_at });

            // 反應和收藏索引
            modelBuilder.Entity<Reaction>()
                .HasIndex(r => new { r.user_id, r.target_type, r.target_id, r.kind })
                .IsUnique();

            modelBuilder.Entity<Reaction>()
                .HasIndex(r => new { r.target_type, r.target_id });

            modelBuilder.Entity<Bookmark>()
                .HasIndex(b => new { b.user_id, b.target_type, b.target_id })
                .IsUnique();

            modelBuilder.Entity<Bookmark>()
                .HasIndex(b => new { b.target_type, b.target_id });

            // 通知相關索引
            modelBuilder.Entity<NotificationRecipient>()
                .HasIndex(nr => new { nr.notification_id, nr.user_id })
                .IsUnique();

            modelBuilder.Entity<NotificationRecipient>()
                .HasIndex(nr => new { nr.user_id, nr.is_read, nr.recipient_id })
                .HasDatabaseName("IX_Inbox");

            // 聊天相關索引
            modelBuilder.Entity<ChatMessage>()
                .HasIndex(cm => cm.sent_at);

            modelBuilder.Entity<ChatMessage>()
                .HasIndex(cm => new { cm.receiver_id, cm.sent_at });

            // 群組相關索引
            modelBuilder.Entity<GroupChat>()
                .HasIndex(gc => gc.group_id);

            modelBuilder.Entity<GroupChat>()
                .HasIndex(gc => new { gc.group_id, gc.sent_at });

            // 商城相關索引
            modelBuilder.Entity<OrderInfo>()
                .HasIndex(o => new { o.user_id, o.order_status, o.payment_status, o.order_date });

            modelBuilder.Entity<OrderItems>()
                .HasIndex(oi => oi.order_id);

            // 自由市場相關索引
            modelBuilder.Entity<PlayerMarketProductInfo>()
                .HasIndex(pmp => new { pmp.seller_id, pmp.p_status, pmp.created_at });

            modelBuilder.Entity<PlayerMarketOrderInfo>()
                .HasIndex(pmo => new { pmo.buyer_id, pmo.seller_id, pmo.p_order_status });

            modelBuilder.Entity<PlayerMarketOrderTradepage>()
                .HasIndex(pmot => pmot.p_order_id);
        }

        /// <summary>
        /// 配置關聯關係
        /// </summary>
        private void ConfigureRelationships(ModelBuilder modelBuilder)
        {
            // User 與相關實體的關聯
            modelBuilder.Entity<User>()
                .HasOne(u => u.UserIntroduce)
                .WithOne(ui => ui.User)
                .HasForeignKey<UserIntroduce>(ui => ui.User_ID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<User>()
                .HasOne(u => u.UserRights)
                .WithOne(ur => ur.User)
                .HasForeignKey<UserRights>(ur => ur.User_Id)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<User>()
                .HasOne(u => u.UserWallet)
                .WithOne(uw => uw.User)
                .HasForeignKey<UserWallet>(uw => uw.User_Id)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<User>()
                .HasOne(u => u.MemberSalesProfile)
                .WithOne(msp => msp.User)
                .HasForeignKey<MemberSalesProfile>(msp => msp.User_Id)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<User>()
                .HasOne(u => u.UserSalesInformation)
                .WithOne(usi => usi.User)
                .HasForeignKey<UserSalesInformation>(usi => usi.User_Id)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<User>()
                .HasOne(u => u.Pet)
                .WithOne(p => p.User)
                .HasForeignKey<Pet>(p => p.UserID)
                .OnDelete(DeleteBehavior.Cascade);

            // Pet 與 MiniGame 的關聯
            modelBuilder.Entity<Pet>()
                .HasMany(p => p.MiniGames)
                .WithOne(mg => mg.Pet)
                .HasForeignKey(mg => mg.PetID)
                .OnDelete(DeleteBehavior.Cascade);

            // Game 與 Forum 的關聯 (一對一)
            modelBuilder.Entity<Game>()
                .HasOne(g => g.Forum)
                .WithOne(f => f.Game)
                .HasForeignKey<Forum>(f => f.game_id)
                .OnDelete(DeleteBehavior.Cascade);

            // Forum 與 Thread 的關聯
            modelBuilder.Entity<Forum>()
                .HasMany(f => f.Threads)
                .WithOne(t => t.Forum)
                .HasForeignKey(t => t.forum_id)
                .OnDelete(DeleteBehavior.Cascade);

            // Thread 與 ThreadPost 的關聯
            modelBuilder.Entity<Thread>()
                .HasMany(t => t.ThreadPosts)
                .WithOne(tp => tp.Thread)
                .HasForeignKey(tp => tp.thread_id)
                .OnDelete(DeleteBehavior.Cascade);

            // ProductInfo 與相關實體的關聯
            modelBuilder.Entity<ProductInfo>()
                .HasOne(pi => pi.GameProductDetails)
                .WithOne(gpd => gpd.ProductInfo)
                .HasForeignKey<GameProductDetails>(gpd => gpd.product_id)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ProductInfo>()
                .HasOne(pi => pi.OtherProductDetails)
                .WithOne(opd => opd.ProductInfo)
                .HasForeignKey<OtherProductDetails>(opd => opd.product_id)
                .OnDelete(DeleteBehavior.Cascade);

            // ManagerData 與 ManagerRole 的關聯
            modelBuilder.Entity<ManagerData>()
                .HasMany(md => md.ManagerRoles)
                .WithOne(mr => mr.ManagerData)
                .HasForeignKey(mr => mr.Manager_Id)
                .OnDelete(DeleteBehavior.Cascade);

            // 配置級聯刪除
            ConfigureCascadeDelete(modelBuilder);
        }

        /// <summary>
        /// 配置級聯刪除
        /// </summary>
        private void ConfigureCascadeDelete(ModelBuilder modelBuilder)
        {
            // 當使用者被刪除時，相關資料也會被刪除
            modelBuilder.Entity<User>()
                .HasMany(u => u.UserSignInStats)
                .WithOne(us => us.User)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<User>()
                .HasMany(u => u.MiniGames)
                .WithOne(mg => mg.User)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<User>()
                .HasMany(u => u.Posts)
                .WithOne(p => p.CreatedBy)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<User>()
                .HasMany(u => u.Threads)
                .WithOne(t => t.AuthorUser)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<User>()
                .HasMany(u => u.ThreadPosts)
                .WithOne(tp => tp.AuthorUser)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<User>()
                .HasMany(u => u.Reactions)
                .WithOne(r => r.User)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<User>()
                .HasMany(u => u.Bookmarks)
                .WithOne(b => b.User)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<User>()
                .HasMany(u => u.Notifications)
                .WithOne(n => n.Sender)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<User>()
                .HasMany(u => u.NotificationRecipients)
                .WithOne(nr => nr.User)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<User>()
                .HasMany(u => u.ChatMessages)
                .WithOne(cm => cm.Sender)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<User>()
                .HasMany(u => u.GroupMembers)
                .WithOne(gm => gm.User)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<User>()
                .HasMany(u => u.GroupChats)
                .WithOne(gc => gc.Sender)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<User>()
                .HasMany(u => u.GroupBlocks)
                .WithOne(gb => gb.User)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<User>()
                .HasMany(u => u.OrderInfos)
                .WithOne(o => o.User)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<User>()
                .HasMany(u => u.PlayerMarketProducts)
                .WithOne(pmp => pmp.Seller)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<User>()
                .HasMany(u => u.BuyerOrders)
                .WithOne(pmo => pmo.Buyer)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<User>()
                .HasMany(u => u.SellerOrders)
                .WithOne(pmo => pmo.Seller)
                .OnDelete(DeleteBehavior.Cascade);
        }

        /// <summary>
        /// 配置預設值
        /// </summary>
        private void ConfigureDefaultValues(ModelBuilder modelBuilder)
        {
            // 配置日期時間預設值
            modelBuilder.Entity<User>()
                .Property(u => u.CreatedAt)
                .HasDefaultValueSql("GETUTCDATE()");

            modelBuilder.Entity<User>()
                .Property(u => u.UpdatedAt)
                .HasDefaultValueSql("GETUTCDATE()");

            modelBuilder.Entity<Pet>()
                .Property(p => p.Hunger)
                .HasDefaultValue(100);

            modelBuilder.Entity<Pet>()
                .Property(p => p.Mood)
                .HasDefaultValue(100);

            modelBuilder.Entity<Pet>()
                .Property(p => p.Stamina)
                .HasDefaultValue(100);

            modelBuilder.Entity<Pet>()
                .Property(p => p.Cleanliness)
                .HasDefaultValue(100);

            modelBuilder.Entity<Pet>()
                .Property(p => p.Health)
                .HasDefaultValue(100);
        }

        /// <summary>
        /// 配置檢查約束
        /// </summary>
        private void ConfigureCheckConstraints(ModelBuilder modelBuilder)
        {
            // 寵物屬性範圍檢查 (0-100)
            modelBuilder.Entity<Pet>()
                .ToTable(t => t.HasCheckConstraint("CK_Pet_Hunger_Range", "[Hunger] >= 0 AND [Hunger] <= 100"));

            modelBuilder.Entity<Pet>()
                .ToTable(t => t.HasCheckConstraint("CK_Pet_Mood_Range", "[Mood] >= 0 AND [Mood] <= 100"));

            modelBuilder.Entity<Pet>()
                .ToTable(t => t.HasCheckConstraint("CK_Pet_Stamina_Range", "[Stamina] >= 0 AND [Stamina] <= 100"));

            modelBuilder.Entity<Pet>()
                .ToTable(t => t.HasCheckConstraint("CK_Pet_Cleanliness_Range", "[Cleanliness] >= 0 AND [Cleanliness] <= 100"));

            modelBuilder.Entity<Pet>()
                .ToTable(t => t.HasCheckConstraint("CK_Pet_Health_Range", "[Health] >= 0 AND [Health] <= 100"));

            // 遊戲等級檢查
            modelBuilder.Entity<Pet>()
                .ToTable(t => t.HasCheckConstraint("CK_Pet_Level_Range", "[Level] >= 1 AND [Level] <= 250"));

            // 點數檢查
            modelBuilder.Entity<UserWallet>()
                .ToTable(t => t.HasCheckConstraint("CK_UserWallet_Points_NonNegative", "[User_Point] >= 0"));

            // 價格檢查
            modelBuilder.Entity<ProductInfo>()
                .ToTable(t => t.HasCheckConstraint("CK_ProductInfo_Price_NonNegative", "[price] >= 0"));

            // 庫存檢查
            modelBuilder.Entity<ProductInfo>()
                .ToTable(t => t.HasCheckConstraint("CK_ProductInfo_Stock_NonNegative", "[Shipment_Quantity] >= 0"));
        }
    }
}