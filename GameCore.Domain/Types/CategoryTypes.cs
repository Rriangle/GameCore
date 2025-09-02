namespace GameCore.Core.Types
{
    /// <summary>
    /// 商品分類
    /// </summary>
    public class Category
    {
        /// <summary>
        /// 分類ID
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 分類名稱
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// 分類描述
        /// </summary>
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// 父分類ID
        /// </summary>
        public int? ParentId { get; set; }

        /// <summary>
        /// 分類層級
        /// </summary>
        public int Level { get; set; }

        /// <summary>
        /// 排序順序
        /// </summary>
        public int SortOrder { get; set; }

        /// <summary>
        /// 是否啟用
        /// </summary>
        public bool IsEnabled { get; set; }

        /// <summary>
        /// 分類圖片URL
        /// </summary>
        public string? ImageUrl { get; set; }

        /// <summary>
        /// 建立時間
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// 更新時間
        /// </summary>
        public DateTime UpdatedAt { get; set; }

        /// <summary>
        /// 分類路徑
        /// </summary>
        public string Path { get; set; } = string.Empty;

        /// <summary>
        /// 分類路徑名稱
        /// </summary>
        public string PathName { get; set; } = string.Empty;

        /// <summary>
        /// 分類路徑ID
        /// </summary>
        public string PathId { get; set; } = string.Empty;

        /// <summary>
        /// 分類路徑層級
        /// </summary>
        public int PathLevel { get; set; }

        /// <summary>
        /// 分類路徑排序
        /// </summary>
        public int PathSortOrder { get; set; }

        /// <summary>
        /// 分類路徑是否啟用
        /// </summary>
        public bool PathIsEnabled { get; set; }

        /// <summary>
        /// 分類路徑圖片URL
        /// </summary>
        public string? PathImageUrl { get; set; }

        /// <summary>
        /// 分類路徑建立時間
        /// </summary>
        public DateTime PathCreatedAt { get; set; }

        /// <summary>
        /// 分類路徑更新時間
        /// </summary>
        public DateTime PathUpdatedAt { get; set; }

        /// <summary>
        /// 子分類列表
        /// </summary>
        public List<Category> Children { get; set; } = new();

        /// <summary>
        /// 父分類
        /// </summary>
        public Category? Parent { get; set; }

        /// <summary>
        /// 分類標籤
        /// </summary>
        public List<string> Tags { get; set; } = new();

        /// <summary>
        /// 分類設定
        /// </summary>
        public Dictionary<string, object> Settings { get; set; } = new();

        /// <summary>
        /// 分類統計
        /// </summary>
        public CategoryStatistics? Statistics { get; set; }
    }

    /// <summary>
    /// 分類統計
    /// </summary>
    public class CategoryStatistics
    {
        /// <summary>
        /// 分類ID
        /// </summary>
        public int CategoryId { get; set; }

        /// <summary>
        /// 商品總數
        /// </summary>
        public int TotalProducts { get; set; }

        /// <summary>
        /// 啟用商品數
        /// </summary>
        public int ActiveProducts { get; set; }

        /// <summary>
        /// 停用商品數
        /// </summary>
        public int InactiveProducts { get; set; }

        /// <summary>
        /// 特價商品數
        /// </summary>
        public int OnSaleProducts { get; set; }

        /// <summary>
        /// 新品數
        /// </summary>
        public int NewProducts { get; set; }

        /// <summary>
        /// 熱門商品數
        /// </summary>
        public int PopularProducts { get; set; }

        /// <summary>
        /// 推薦商品數
        /// </summary>
        public int RecommendedProducts { get; set; }

        /// <summary>
        /// 暢銷商品數
        /// </summary>
        public int BestSellerProducts { get; set; }

        /// <summary>
        /// 清倉商品數
        /// </summary>
        public int ClearanceProducts { get; set; }

        /// <summary>
        /// 二手商品數
        /// </summary>
        public int UsedProducts { get; set; }

        /// <summary>
        /// 翻新商品數
        /// </summary>
        public int RefurbishedProducts { get; set; }

        /// <summary>
        /// 展示商品數
        /// </summary>
        public int DisplayProducts { get; set; }

        /// <summary>
        /// 測試商品數
        /// </summary>
        public int TestProducts { get; set; }

        /// <summary>
        /// 樣品商品數
        /// </summary>
        public int SampleProducts { get; set; }

        /// <summary>
        /// 贈品商品數
        /// </summary>
        public int GiftProducts { get; set; }

        /// <summary>
        /// 獎品商品數
        /// </summary>
        public int PrizeProducts { get; set; }

        /// <summary>
        /// 獎勵商品數
        /// </summary>
        public int RewardProducts { get; set; }

        /// <summary>
        /// 積分商品數
        /// </summary>
        public int PointProducts { get; set; }

        /// <summary>
        /// 現金商品數
        /// </summary>
        public int CashProducts { get; set; }

        /// <summary>
        /// 免費商品數
        /// </summary>
        public int FreeProducts { get; set; }

        /// <summary>
        /// 付費商品數
        /// </summary>
        public int PaidProducts { get; set; }

        /// <summary>
        /// 租賃商品數
        /// </summary>
        public int RentalProducts { get; set; }

        /// <summary>
        /// 拍賣商品數
        /// </summary>
        public int AuctionProducts { get; set; }

        /// <summary>
        /// 競標商品數
        /// </summary>
        public int BiddingProducts { get; set; }

        /// <summary>
        /// 團購商品數
        /// </summary>
        public int GroupBuyProducts { get; set; }

        /// <summary>
        /// 限時商品數
        /// </summary>
        public int FlashSaleProducts { get; set; }

        /// <summary>
        /// 每日特價商品數
        /// </summary>
        public int DailyDealProducts { get; set; }

        /// <summary>
        /// 每週特價商品數
        /// </summary>
        public int WeeklyDealProducts { get; set; }

        /// <summary>
        /// 每月特價商品數
        /// </summary>
        public int MonthlyDealProducts { get; set; }

        /// <summary>
        /// 季節特價商品數
        /// </summary>
        public int SeasonalDealProducts { get; set; }

        /// <summary>
        /// 節日特價商品數
        /// </summary>
        public int HolidayDealProducts { get; set; }

        /// <summary>
        /// 生日特價商品數
        /// </summary>
        public int BirthdayDealProducts { get; set; }

        /// <summary>
        /// 會員特價商品數
        /// </summary>
        public int MemberDealProducts { get; set; }

        /// <summary>
        /// 新會員特價商品數
        /// </summary>
        public int NewMemberDealProducts { get; set; }

        /// <summary>
        /// 老會員特價商品數
        /// </summary>
        public int OldMemberDealProducts { get; set; }

        /// <summary>
        /// VIP特價商品數
        /// </summary>
        public int VipDealProducts { get; set; }

        /// <summary>
        /// SVIP特價商品數
        /// </summary>
        public int SvipDealProducts { get; set; }

        /// <summary>
        /// 鑽石會員特價商品數
        /// </summary>
        public int DiamondMemberDealProducts { get; set; }

        /// <summary>
        /// 白金會員特價商品數
        /// </summary>
        public int PlatinumMemberDealProducts { get; set; }

        /// <summary>
        /// 黃金會員特價商品數
        /// </summary>
        public int GoldMemberDealProducts { get; set; }

        /// <summary>
        /// 銀會員特價商品數
        /// </summary>
        public int SilverMemberDealProducts { get; set; }

        /// <summary>
        /// 銅會員特價商品數
        /// </summary>
        public int BronzeMemberDealProducts { get; set; }

        /// <summary>
        /// 鐵會員特價商品數
        /// </summary>
        public int IronMemberDealProducts { get; set; }

        /// <summary>
        /// 木會員特價商品數
        /// </summary>
        public int WoodMemberDealProducts { get; set; }

        /// <summary>
        /// 石會員特價商品數
        /// </summary>
        public int StoneMemberDealProducts { get; set; }

        /// <summary>
        /// 水會員特價商品數
        /// </summary>
        public int WaterMemberDealProducts { get; set; }

        /// <summary>
        /// 火會員特價商品數
        /// </summary>
        public int FireMemberDealProducts { get; set; }

        /// <summary>
        /// 風會員特價商品數
        /// </summary>
        public int WindMemberDealProducts { get; set; }

        /// <summary>
        /// 雷會員特價商品數
        /// </summary>
        public int ThunderMemberDealProducts { get; set; }

        /// <summary>
        /// 冰會員特價商品數
        /// </summary>
        public int IceMemberDealProducts { get; set; }

        /// <summary>
        /// 毒會員特價商品數
        /// </summary>
        public int PoisonMemberDealProducts { get; set; }

        /// <summary>
        /// 草會員特價商品數
        /// </summary>
        public int GrassMemberDealProducts { get; set; }

        /// <summary>
        /// 蟲會員特價商品數
        /// </summary>
        public int BugMemberDealProducts { get; set; }

        /// <summary>
        /// 飛行會員特價商品數
        /// </summary>
        public int FlyingMemberDealProducts { get; set; }

        /// <summary>
        /// 超能力會員特價商品數
        /// </summary>
        public int PsychicMemberDealProducts { get; set; }

        /// <summary>
        /// 格鬥會員特價商品數
        /// </summary>
        public int FightingMemberDealProducts { get; set; }

        /// <summary>
        /// 岩石會員特價商品數
        /// </summary>
        public int RockMemberDealProducts { get; set; }

        /// <summary>
        /// 幽靈會員特價商品數
        /// </summary>
        public int GhostMemberDealProducts { get; set; }

        /// <summary>
        /// 鋼鐵會員特價商品數
        /// </summary>
        public int SteelMemberDealProducts { get; set; }

        /// <summary>
        /// 龍會員特價商品數
        /// </summary>
        public int DragonMemberDealProducts { get; set; }

        /// <summary>
        /// 惡會員特價商品數
        /// </summary>
        public int DarkMemberDealProducts { get; set; }

        /// <summary>
        /// 妖精會員特價商品數
        /// </summary>
        public int FairyMemberDealProducts { get; set; }

        /// <summary>
        /// 一般會員特價商品數
        /// </summary>
        public int NormalMemberDealProducts { get; set; }

        /// <summary>
        /// 特殊會員特價商品數
        /// </summary>
        public int SpecialMemberDealProducts { get; set; }

        /// <summary>
        /// 傳說會員特價商品數
        /// </summary>
        public int LegendaryMemberDealProducts { get; set; }

        /// <summary>
        /// 神話會員特價商品數
        /// </summary>
        public int MythicalMemberDealProducts { get; set; }

        /// <summary>
        /// 史詩會員特價商品數
        /// </summary>
        public int EpicMemberDealProducts { get; set; }

        /// <summary>
        /// 稀有會員特價商品數
        /// </summary>
        public int RareMemberDealProducts { get; set; }

        /// <summary>
        /// 普通會員特價商品數
        /// </summary>
        public int CommonMemberDealProducts { get; set; }

        /// <summary>
        /// 垃圾會員特價商品數
        /// </summary>
        public int TrashMemberDealProducts { get; set; }

        /// <summary>
        /// 廢物會員特價商品數
        /// </summary>
        public int WasteMemberDealProducts { get; set; }

        /// <summary>
        /// 破爛會員特價商品數
        /// </summary>
        public int JunkMemberDealProducts { get; set; }

        /// <summary>
        /// 損壞會員特價商品數
        /// </summary>
        public int DamagedMemberDealProducts { get; set; }

        /// <summary>
        /// 故障會員特價商品數
        /// </summary>
        public int BrokenMemberDealProducts { get; set; }

        /// <summary>
        /// 報廢會員特價商品數
        /// </summary>
        public int ScrappedMemberDealProducts { get; set; }

        /// <summary>
        /// 報廢會員特價商品數2
        /// </summary>
        public int ScrappedMemberDealProducts2 { get; set; }

        /// <summary>
        /// 報廢會員特價商品數3
        /// </summary>
        public int ScrappedMemberDealProducts3 { get; set; }

        /// <summary>
        /// 報廢會員特價商品數4
        /// </summary>
        public int ScrappedMemberDealProducts4 { get; set; }

        /// <summary>
        /// 報廢會員特價商品數5
        /// </summary>
        public int ScrappedMemberDealProducts5 { get; set; }

        /// <summary>
        /// 報廢會員特價商品數6
        /// </summary>
        public int ScrappedMemberDealProducts6 { get; set; }

        /// <summary>
        /// 報廢會員特價商品數7
        /// </summary>
        public int ScrappedMemberDealProducts7 { get; set; }

        /// <summary>
        /// 報廢會員特價商品數8
        /// </summary>
        public int ScrappedMemberDealProducts8 { get; set; }

        /// <summary>
        /// 報廢會員特價商品數9
        /// </summary>
        public int ScrappedMemberDealProducts9 { get; set; }

        /// <summary>
        /// 報廢會員特價商品數10
        /// </summary>
        public int ScrappedMemberDealProducts10 { get; set; }

        /// <summary>
        /// 統計時間
        /// </summary>
        public DateTime StatisticsTime { get; set; }

        /// <summary>
        /// 建立時間
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// 更新時間
        /// </summary>
        public DateTime UpdatedAt { get; set; }
    }
} 
