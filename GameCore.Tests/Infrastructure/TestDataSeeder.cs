using Bogus;
using GameCore.Core.Entities;
using GameCore.Infrastructure.Data;

namespace GameCore.Tests.Infrastructure
{
    /// <summary>
    /// 測試資料種子類別
    /// 使用 Bogus 庫生成真實的測試資料
    /// </summary>
    public static class TestDataSeeder
    {
        private static readonly Faker _faker = new Faker("zh_TW");

        /// <summary>
        /// 種植基礎測試資料
        /// </summary>
        /// <param name="context">資料庫上下文</param>
        public static void SeedBasicData(GameCoreDbContext context)
        {
            SeedUsers(context, 50);
            SeedGames(context, 20);
            SeedForums(context, 10);
            SeedPets(context, 30);
            SeedProducts(context, 100);
            
            context.SaveChanges();
        }

        /// <summary>
        /// 種植使用者資料
        /// </summary>
        /// <param name="context">資料庫上下文</param>
        /// <param name="count">生成數量</param>
        public static void SeedUsers(GameCoreDbContext context, int count = 10)
        {
            var userFaker = new Faker<User>("zh_TW")
                .RuleFor(u => u.UserAccount, f => f.Internet.UserName())
                .RuleFor(u => u.UserName, f => f.Name.FullName())
                .RuleFor(u => u.Email, f => f.Internet.Email())
                .RuleFor(u => u.UserLevel, f => f.Random.Int(1, 50))
                .RuleFor(u => u.Points, f => f.Random.Int(0, 10000))
                .RuleFor(u => u.Experience, f => f.Random.Int(0, 50000))
                .RuleFor(u => u.DisplayName, f => f.Name.FirstName())
                .RuleFor(u => u.AvatarUrl, f => f.Internet.Avatar())
                .RuleFor(u => u.RegistrationTime, f => f.Date.Past(2))
                .RuleFor(u => u.LastLoginTime, f => f.Date.Recent(30))
                .RuleFor(u => u.IsOnline, f => f.Random.Bool())
                .RuleFor(u => u.Status, f => f.PickRandom("Active", "Inactive", "Suspended"));

            var users = userFaker.Generate(count);
            
            // 為每個使用者創建關聯資料
            foreach (var user in users)
            {
                // 使用者介紹
                user.UserIntroduce = new UserIntroduce
                {
                    User = user,
                    UserNickName = _faker.Name.FirstName(),
                    UserSelfIntroduction = _faker.Lorem.Paragraph(),
                    UserBirthday = _faker.Date.Past(30, DateTime.Now.AddYears(-18)),
                    UserGender = _faker.PickRandom("男", "女", "其他"),
                    UserLocation = _faker.Address.City(),
                    CreatedAt = user.RegistrationTime,
                    UpdatedAt = _faker.Date.Recent(7)
                };

                // 使用者錢包
                user.UserWallet = new UserWallet
                {
                    User = user,
                    UserPoint = user.Points,
                    UserCash = _faker.Random.Decimal(0, 1000),
                    CreatedAt = user.RegistrationTime,
                    UpdatedAt = _faker.Date.Recent(7)
                };

                // 使用者權限
                user.UserRights = new UserRights
                {
                    User = user,
                    CanPost = true,
                    CanComment = true,
                    CanUpload = _faker.Random.Bool(),
                    CanTrade = _faker.Random.Bool(),
                    CreatedAt = user.RegistrationTime,
                    UpdatedAt = _faker.Date.Recent(7)
                };
            }

            context.Users.AddRange(users);
        }

        /// <summary>
        /// 種植遊戲資料
        /// </summary>
        /// <param name="context">資料庫上下文</param>
        /// <param name="count">生成數量</param>
        public static void SeedGames(GameCoreDbContext context, int count = 5)
        {
            var gameGenres = new[] { "動作", "角色扮演", "策略", "射擊", "運動", "模擬", "冒險", "益智" };
            var platforms = new[] { "PC", "PlayStation", "Xbox", "Nintendo Switch", "Mobile" };

            var gameFaker = new Faker<Game>("zh_TW")
                .RuleFor(g => g.GameName, f => $"{f.Commerce.ProductName()} {f.Random.Number(2000, 2024)}")
                .RuleFor(g => g.GameGenre, f => f.PickRandom(gameGenres))
                .RuleFor(g => g.Platform, f => f.PickRandom(platforms))
                .RuleFor(g => g.Developer, f => f.Company.CompanyName())
                .RuleFor(g => g.Publisher, f => f.Company.CompanyName())
                .RuleFor(g => g.ReleaseDate, f => f.Date.Past(5))
                .RuleFor(g => g.Price, f => f.Random.Decimal(0, 2000))
                .RuleFor(g => g.Rating, f => f.Random.Double(1, 10))
                .RuleFor(g => g.Description, f => f.Lorem.Paragraphs(3, "\n"))
                .RuleFor(g => g.ImageUrl, f => f.Image.PicsumUrl(800, 600))
                .RuleFor(g => g.TrailerUrl, f => f.Internet.Url())
                .RuleFor(g => g.IsActive, f => f.Random.Bool(0.9f))
                .RuleFor(g => g.CreatedAt, f => f.Date.Past(1))
                .RuleFor(g => g.UpdatedAt, f => f.Date.Recent(30));

            var games = gameFaker.Generate(count);
            context.Games.AddRange(games);
        }

        /// <summary>
        /// 種植論壇資料
        /// </summary>
        /// <param name="context">資料庫上下文</param>
        /// <param name="count">生成數量</param>
        public static void SeedForums(GameCoreDbContext context, int count = 5)
        {
            // 確保有遊戲資料
            var games = context.Games.ToList();
            if (!games.Any())
            {
                SeedGames(context, 5);
                context.SaveChanges();
                games = context.Games.ToList();
            }

            var forumNames = new[] 
            {
                "新手討論區", "遊戲攻略", "裝備交流", "公會招募", 
                "Bug回報", "建議反饋", "閒聊天地", "競技討論"
            };

            var forumFaker = new Faker<Forum>("zh_TW")
                .RuleFor(f => f.ForumName, (faker, forum) => faker.PickRandom(forumNames))
                .RuleFor(f => f.ForumDescription, f => f.Lorem.Paragraph())
                .RuleFor(f => f.GameId, f => f.PickRandom(games).GameId)
                .RuleFor(f => f.IsActive, f => f.Random.Bool(0.95f))
                .RuleFor(f => f.CreatedAt, f => f.Date.Past(1))
                .RuleFor(f => f.UpdatedAt, f => f.Date.Recent(30));

            var forums = forumFaker.Generate(count);
            context.Forums.AddRange(forums);
        }

        /// <summary>
        /// 種植寵物資料
        /// </summary>
        /// <param name="context">資料庫上下文</param>
        /// <param name="count">生成數量</param>
        public static void SeedPets(GameCoreDbContext context, int count = 10)
        {
            // 確保有使用者資料
            var users = context.Users.ToList();
            if (!users.Any())
            {
                SeedUsers(context, 20);
                context.SaveChanges();
                users = context.Users.ToList();
            }

            var petNames = new[] 
            {
                "小白", "小黑", "阿福", "美美", "球球", "毛毛", "花花", "豆豆",
                "咪咪", "旺旺", "乖乖", "皮皮", "糖糖", "寶寶", "妞妞", "樂樂"
            };

            var colors = new[] { "#FF6B9D", "#4ECDC4", "#45B7D1", "#96CEB4", "#FFEAA7", "#DDA0DD", "#98D8C8" };

            var petFaker = new Faker<Pet>("zh_TW")
                .RuleFor(p => p.Name, f => f.PickRandom(petNames))
                .RuleFor(p => p.UserId, f => f.PickRandom(users).UserId)
                .RuleFor(p => p.Level, f => f.Random.Int(1, 20))
                .RuleFor(p => p.Experience, f => f.Random.Int(0, 10000))
                .RuleFor(p => p.Health, f => f.Random.Int(50, 100))
                .RuleFor(p => p.Hunger, f => f.Random.Int(30, 100))
                .RuleFor(p => p.Happiness, f => f.Random.Int(40, 100))
                .RuleFor(p => p.Color, f => f.PickRandom(colors))
                .RuleFor(p => p.LastFeedTime, f => f.Date.Recent(3))
                .RuleFor(p => p.LastPlayTime, f => f.Date.Recent(2))
                .RuleFor(p => p.CreatedAt, f => f.Date.Past(1))
                .RuleFor(p => p.UpdatedAt, f => f.Date.Recent(7));

            var pets = petFaker.Generate(count);
            context.Pets.AddRange(pets);
        }

        /// <summary>
        /// 種植商品資料
        /// </summary>
        /// <param name="context">資料庫上下文</param>
        /// <param name="count">生成數量</param>
        public static void SeedProducts(GameCoreDbContext context, int count = 20)
        {
            var categories = new[] { "遊戲", "裝備", "道具", "會員", "點數卡", "周邊商品" };
            var suppliers = new[] { "官方商城", "第三方廠商", "獨立開發者", "合作夥伴" };

            // 創建供應商
            var supplierFaker = new Faker<Supplier>("zh_TW")
                .RuleFor(s => s.SupplierName, f => f.PickRandom(suppliers))
                .RuleFor(s => s.ContactInfo, f => f.Phone)
                .RuleFor(s => s.Address, f => f.Address.FullAddress())
                .RuleFor(s => s.CreatedAt, f => f.Date.Past(2))
                .RuleFor(s => s.UpdatedAt, f => f.Date.Recent(30));

            var supplierList = supplierFaker.Generate(5);
            context.Suppliers.AddRange(supplierList);
            context.SaveChanges();

            var productFaker = new Faker<ProductInfo>("zh_TW")
                .RuleFor(p => p.ProductName, f => f.Commerce.ProductName())
                .RuleFor(p => p.ProductDescription, f => f.Lorem.Paragraph())
                .RuleFor(p => p.Price, f => f.Random.Decimal(10, 2000))
                .RuleFor(p => p.Stock, f => f.Random.Int(0, 1000))
                .RuleFor(p => p.Category, f => f.PickRandom(categories))
                .RuleFor(p => p.ImageUrl, f => f.Image.PicsumUrl(400, 400))
                .RuleFor(p => p.SupplierId, f => f.PickRandom(supplierList).SupplierId)
                .RuleFor(p => p.IsActive, f => f.Random.Bool(0.9f))
                .RuleFor(p => p.CreatedAt, f => f.Date.Past(1))
                .RuleFor(p => p.UpdatedAt, f => f.Date.Recent(30));

            var products = productFaker.Generate(count);
            context.ProductInfos.AddRange(products);
        }

        /// <summary>
        /// 清理所有測試資料
        /// </summary>
        /// <param name="context">資料庫上下文</param>
        public static void CleanupTestData(GameCoreDbContext context)
        {
            context.Pets.RemoveRange(context.Pets);
            context.ProductInfos.RemoveRange(context.ProductInfos);
            context.Suppliers.RemoveRange(context.Suppliers);
            context.Forums.RemoveRange(context.Forums);
            context.Games.RemoveRange(context.Games);
            context.UserWallets.RemoveRange(context.UserWallets);
            context.UserRights.RemoveRange(context.UserRights);
            context.UserIntroduces.RemoveRange(context.UserIntroduces);
            context.Users.RemoveRange(context.Users);
            
            context.SaveChanges();
        }
    }
}
