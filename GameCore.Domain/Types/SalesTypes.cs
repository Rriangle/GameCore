namespace GameCore.Core.Types
{
    /// <summary>
    /// 銷售權限
    /// </summary>
    public class SalesPermission
    {
        /// <summary>
        /// 權限ID
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 用戶ID
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// 權限狀態
        /// </summary>
        public string Status { get; set; } = string.Empty;

        /// <summary>
        /// 申請時間
        /// </summary>
        public DateTime RequestTime { get; set; }

        /// <summary>
        /// 審核時間
        /// </summary>
        public DateTime? ReviewTime { get; set; }

        /// <summary>
        /// 審核者ID
        /// </summary>
        public int? ReviewerId { get; set; }

        /// <summary>
        /// 審核結果
        /// </summary>
        public string? ReviewResult { get; set; }

        /// <summary>
        /// 審核備註
        /// </summary>
        public string? ReviewNotes { get; set; }

        /// <summary>
        /// 生效時間
        /// </summary>
        public DateTime? EffectiveTime { get; set; }

        /// <summary>
        /// 過期時間
        /// </summary>
        public DateTime? ExpiryTime { get; set; }

        /// <summary>
        /// 權限等級
        /// </summary>
        public string PermissionLevel { get; set; } = string.Empty;

        /// <summary>
        /// 權限範圍
        /// </summary>
        public List<string> PermissionScope { get; set; } = new();

        /// <summary>
        /// 是否啟用
        /// </summary>
        public bool IsEnabled { get; set; }

        /// <summary>
        /// 建立時間
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// 更新時間
        /// </summary>
        public DateTime UpdatedAt { get; set; }
    }

    /// <summary>
    /// 銷售統計
    /// </summary>
    public class SalesStatistics
    {
        /// <summary>
        /// 統計ID
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 用戶ID
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// 統計日期
        /// </summary>
        public DateTime StatDate { get; set; }

        /// <summary>
        /// 總銷售額
        /// </summary>
        public decimal TotalSales { get; set; }

        /// <summary>
        /// 總銷售數量
        /// </summary>
        public int TotalQuantity { get; set; }

        /// <summary>
        /// 總訂單數
        /// </summary>
        public int TotalOrders { get; set; }

        /// <summary>
        /// 平均訂單金額
        /// </summary>
        public decimal AverageOrderValue => TotalOrders > 0 ? TotalSales / TotalOrders : 0;

        /// <summary>
        /// 退款金額
        /// </summary>
        public decimal RefundAmount { get; set; }

        /// <summary>
        /// 退款數量
        /// </summary>
        public int RefundQuantity { get; set; }

        /// <summary>
        /// 淨銷售額
        /// </summary>
        public decimal NetSales => TotalSales - RefundAmount;

        /// <summary>
        /// 佣金金額
        /// </summary>
        public decimal CommissionAmount { get; set; }

        /// <summary>
        /// 佣金率
        /// </summary>
        public decimal CommissionRate { get; set; }

        /// <summary>
        /// 淨利潤
        /// </summary>
        public decimal NetProfit => NetSales - CommissionAmount;

        /// <summary>
        /// 客戶數量
        /// </summary>
        public int CustomerCount { get; set; }

        /// <summary>
        /// 新客戶數量
        /// </summary>
        public int NewCustomerCount { get; set; }

        /// <summary>
        /// 重複客戶數量
        /// </summary>
        public int RepeatCustomerCount { get; set; }

        /// <summary>
        /// 客戶保留率
        /// </summary>
        public decimal CustomerRetentionRate => CustomerCount > 0 ? (decimal)RepeatCustomerCount / CustomerCount * 100 : 0;

        /// <summary>
        /// 建立時間
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// 更新時間
        /// </summary>
        public DateTime UpdatedAt { get; set; }
    }

    /// <summary>
    /// 銷售排行榜
    /// </summary>
    public class SalesLeaderboard
    {
        /// <summary>
        /// 排行榜ID
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 排行榜名稱
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// 排行榜類型
        /// </summary>
        public string Type { get; set; } = string.Empty;

        /// <summary>
        /// 排行榜期間
        /// </summary>
        public string Period { get; set; } = string.Empty;

        /// <summary>
        /// 排行榜項目列表
        /// </summary>
        public List<SalesLeaderboardItem> Items { get; set; } = new();

        /// <summary>
        /// 排行榜開始時間
        /// </summary>
        public DateTime StartTime { get; set; }

        /// <summary>
        /// 排行榜結束時間
        /// </summary>
        public DateTime EndTime { get; set; }

        /// <summary>
        /// 排行榜狀態
        /// </summary>
        public string Status { get; set; } = string.Empty;

        /// <summary>
        /// 是否啟用
        /// </summary>
        public bool IsEnabled { get; set; }

        /// <summary>
        /// 建立時間
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// 更新時間
        /// </summary>
        public DateTime UpdatedAt { get; set; }
    }

    /// <summary>
    /// 銷售排行榜項目
    /// </summary>
    public class SalesLeaderboardItem
    {
        /// <summary>
        /// 項目ID
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 排行榜ID
        /// </summary>
        public int LeaderboardId { get; set; }

        /// <summary>
        /// 用戶ID
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// 排名
        /// </summary>
        public int Rank { get; set; }

        /// <summary>
        /// 銷售額
        /// </summary>
        public decimal SalesAmount { get; set; }

        /// <summary>
        /// 銷售數量
        /// </summary>
        public int SalesQuantity { get; set; }

        /// <summary>
        /// 訂單數量
        /// </summary>
        public int OrderCount { get; set; }

        /// <summary>
        /// 客戶數量
        /// </summary>
        public int CustomerCount { get; set; }

        /// <summary>
        /// 評分
        /// </summary>
        public decimal Rating { get; set; }

        /// <summary>
        /// 評分數量
        /// </summary>
        public int RatingCount { get; set; }

        /// <summary>
        /// 獎勵
        /// </summary>
        public string? Reward { get; set; }

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
