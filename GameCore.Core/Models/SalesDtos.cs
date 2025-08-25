namespace GameCore.Core.Models
{
    /// <summary>
    /// 銷售權限申請請求 DTO
    /// </summary>
    public class SalesPermissionRequestDto
    {
        /// <summary>
        /// 營業執照
        /// </summary>
        public string BusinessLicense { get; set; } = string.Empty;

        /// <summary>
        /// 稅號
        /// </summary>
        public string TaxId { get; set; } = string.Empty;

        /// <summary>
        /// 銀行帳戶
        /// </summary>
        public string BankAccount { get; set; } = string.Empty;

        /// <summary>
        /// 聯絡電話
        /// </summary>
        public string ContactPhone { get; set; } = string.Empty;

        /// <summary>
        /// 營業地址
        /// </summary>
        public string BusinessAddress { get; set; } = string.Empty;
    }

    /// <summary>
    /// 銷售權限 DTO
    /// </summary>
    public class SalesPermissionDto
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
        /// 狀態
        /// </summary>
        public string Status { get; set; } = string.Empty;

        /// <summary>
        /// 申請時間
        /// </summary>
        public DateTime AppliedAt { get; set; }

        /// <summary>
        /// 審核時間
        /// </summary>
        public DateTime? ApprovedAt { get; set; }

        /// <summary>
        /// 拒絕時間
        /// </summary>
        public DateTime? RejectedAt { get; set; }

        /// <summary>
        /// 拒絕原因
        /// </summary>
        public string? RejectionReason { get; set; }
    }

    /// <summary>
    /// 銷售錢包 DTO
    /// </summary>
    public class SalesWalletDto
    {
        /// <summary>
        /// 用戶ID
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// 總收益
        /// </summary>
        public decimal TotalEarnings { get; set; }

        /// <summary>
        /// 可用餘額
        /// </summary>
        public decimal AvailableBalance { get; set; }

        /// <summary>
        /// 待處理餘額
        /// </summary>
        public decimal PendingBalance { get; set; }

        /// <summary>
        /// 最後更新時間
        /// </summary>
        public DateTime LastUpdated { get; set; }
    }

    /// <summary>
    /// 提現申請請求 DTO
    /// </summary>
    public class WithdrawalRequestRequestDto
    {
        /// <summary>
        /// 提現金額
        /// </summary>
        public decimal Amount { get; set; }

        /// <summary>
        /// 銀行帳戶
        /// </summary>
        public string BankAccount { get; set; } = string.Empty;
    }

    /// <summary>
    /// 提現申請 DTO
    /// </summary>
    public class WithdrawalRequestDto
    {
        /// <summary>
        /// 申請ID
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 用戶ID
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// 提現金額
        /// </summary>
        public decimal Amount { get; set; }

        /// <summary>
        /// 狀態
        /// </summary>
        public string Status { get; set; } = string.Empty;

        /// <summary>
        /// 申請時間
        /// </summary>
        public DateTime RequestedAt { get; set; }
    }

    /// <summary>
    /// 提現歷史 DTO
    /// </summary>
    public class WithdrawalHistoryDto
    {
        /// <summary>
        /// 提現ID
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 提現金額
        /// </summary>
        public decimal Amount { get; set; }

        /// <summary>
        /// 狀態
        /// </summary>
        public string Status { get; set; } = string.Empty;

        /// <summary>
        /// 申請時間
        /// </summary>
        public DateTime RequestedAt { get; set; }

        /// <summary>
        /// 處理時間
        /// </summary>
        public DateTime? ProcessedAt { get; set; }
    }

    /// <summary>
    /// 銷售統計 DTO
    /// </summary>
    public class SalesStatisticsDto
    {
        /// <summary>
        /// 統計週期
        /// </summary>
        public string Period { get; set; } = string.Empty;

        /// <summary>
        /// 總訂單數
        /// </summary>
        public int TotalOrders { get; set; }

        /// <summary>
        /// 總營收
        /// </summary>
        public decimal TotalRevenue { get; set; }

        /// <summary>
        /// 平均訂單金額
        /// </summary>
        public decimal AverageOrderValue { get; set; }

        /// <summary>
        /// 開始日期
        /// </summary>
        public DateTime StartDate { get; set; }

        /// <summary>
        /// 結束日期
        /// </summary>
        public DateTime EndDate { get; set; }
    }

    /// <summary>
    /// 銷售排行榜 DTO
    /// </summary>
    public class SalesLeaderboardDto
    {
        /// <summary>
        /// 排名
        /// </summary>
        public int Rank { get; set; }

        /// <summary>
        /// 用戶ID
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// 用戶名
        /// </summary>
        public string Username { get; set; } = string.Empty;

        /// <summary>
        /// 總營收
        /// </summary>
        public decimal TotalRevenue { get; set; }

        /// <summary>
        /// 訂單數量
        /// </summary>
        public int OrderCount { get; set; }
    }

    /// <summary>
    /// 銷售指南 DTO
    /// </summary>
    public class SalesGuideDto
    {
        /// <summary>
        /// 標題
        /// </summary>
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// 內容
        /// </summary>
        public string Content { get; set; } = string.Empty;

        /// <summary>
        /// 分類
        /// </summary>
        public string Category { get; set; } = string.Empty;
    }

    /// <summary>
    /// 銀行帳戶更新 DTO
    /// </summary>
    public class BankAccountUpdateDto
    {
        /// <summary>
        /// 銀行帳戶
        /// </summary>
        public string BankAccount { get; set; } = string.Empty;
    }

    /// <summary>
    /// 銀行帳戶 DTO
    /// </summary>
    public class BankAccountDto
    {
        /// <summary>
        /// 用戶ID
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// 銀行帳戶
        /// </summary>
        public string BankAccount { get; set; } = string.Empty;

        /// <summary>
        /// 更新時間
        /// </summary>
        public DateTime? UpdatedAt { get; set; }
    }

    // 管理員用 DTO

    /// <summary>
    /// 銷售權限詳情 DTO
    /// </summary>
    public class SalesPermissionDetailsDto
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
        /// 狀態
        /// </summary>
        public string Status { get; set; } = string.Empty;

        /// <summary>
        /// 申請時間
        /// </summary>
        public DateTime AppliedAt { get; set; }

        /// <summary>
        /// 審核時間
        /// </summary>
        public DateTime? ApprovedAt { get; set; }

        /// <summary>
        /// 拒絕時間
        /// </summary>
        public DateTime? RejectedAt { get; set; }

        /// <summary>
        /// 拒絕原因
        /// </summary>
        public string? RejectionReason { get; set; }

        /// <summary>
        /// 營業執照
        /// </summary>
        public string BusinessLicense { get; set; } = string.Empty;

        /// <summary>
        /// 稅號
        /// </summary>
        public string TaxId { get; set; } = string.Empty;

        /// <summary>
        /// 銀行帳戶
        /// </summary>
        public string BankAccount { get; set; } = string.Empty;

        /// <summary>
        /// 聯絡電話
        /// </summary>
        public string ContactPhone { get; set; } = string.Empty;

        /// <summary>
        /// 營業地址
        /// </summary>
        public string BusinessAddress { get; set; } = string.Empty;
    }

    /// <summary>
    /// 銷售商品 DTO
    /// </summary>
    public class SalesProductDto
    {
        /// <summary>
        /// 商品ID
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 商品名稱
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// 商品描述
        /// </summary>
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// 價格
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// 庫存
        /// </summary>
        public int Stock { get; set; }

        /// <summary>
        /// 狀態
        /// </summary>
        public string Status { get; set; } = string.Empty;

        /// <summary>
        /// 創建時間
        /// </summary>
        public DateTime CreatedAt { get; set; }
    }

    /// <summary>
    /// 銷售訂單 DTO
    /// </summary>
    public class SalesOrderDto
    {
        /// <summary>
        /// 訂單ID
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 訂單編號
        /// </summary>
        public string OrderNumber { get; set; } = string.Empty;

        /// <summary>
        /// 總金額
        /// </summary>
        public decimal TotalAmount { get; set; }

        /// <summary>
        /// 狀態
        /// </summary>
        public string Status { get; set; } = string.Empty;

        /// <summary>
        /// 創建時間
        /// </summary>
        public DateTime CreatedAt { get; set; }
    }

    /// <summary>
    /// 銷售報表 DTO
    /// </summary>
    public class SalesReportDto
    {
        /// <summary>
        /// 用戶ID
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// 開始日期
        /// </summary>
        public DateTime StartDate { get; set; }

        /// <summary>
        /// 結束日期
        /// </summary>
        public DateTime EndDate { get; set; }

        /// <summary>
        /// 總訂單數
        /// </summary>
        public int TotalOrders { get; set; }

        /// <summary>
        /// 總營收
        /// </summary>
        public decimal TotalRevenue { get; set; }

        /// <summary>
        /// 平均訂單金額
        /// </summary>
        public decimal AverageOrderValue { get; set; }
    }

    /// <summary>
    /// 銷售分析 DTO
    /// </summary>
    public class SalesAnalyticsDto
    {
        /// <summary>
        /// 用戶ID
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// 分析週期
        /// </summary>
        public string Period { get; set; } = string.Empty;

        /// <summary>
        /// 開始日期
        /// </summary>
        public DateTime StartDate { get; set; }

        /// <summary>
        /// 結束日期
        /// </summary>
        public DateTime EndDate { get; set; }

        /// <summary>
        /// 成長率
        /// </summary>
        public double GrowthRate { get; set; }

        /// <summary>
        /// 熱門商品
        /// </summary>
        public List<string> TopProducts { get; set; } = new List<string>();

        /// <summary>
        /// 客戶留存率
        /// </summary>
        public double CustomerRetentionRate { get; set; }
    }

    /// <summary>
    /// 銷售目標 DTO
    /// </summary>
    public class SalesTargetDto
    {
        /// <summary>
        /// 用戶ID
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// 月度目標
        /// </summary>
        public decimal MonthlyTarget { get; set; }

        /// <summary>
        /// 季度目標
        /// </summary>
        public decimal QuarterlyTarget { get; set; }

        /// <summary>
        /// 年度目標
        /// </summary>
        public decimal YearlyTarget { get; set; }

        /// <summary>
        /// 更新時間
        /// </summary>
        public DateTime? UpdatedAt { get; set; }
    }

    /// <summary>
    /// 銷售目標設定 DTO
    /// </summary>
    public class SalesTargetSetDto
    {
        /// <summary>
        /// 月度目標
        /// </summary>
        public decimal MonthlyTarget { get; set; }

        /// <summary>
        /// 季度目標
        /// </summary>
        public decimal QuarterlyTarget { get; set; }

        /// <summary>
        /// 年度目標
        /// </summary>
        public decimal YearlyTarget { get; set; }
    }

    /// <summary>
    /// 銷售績效 DTO
    /// </summary>
    public class SalesPerformanceDto
    {
        /// <summary>
        /// 用戶ID
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// 當月銷售
        /// </summary>
        public decimal CurrentMonthSales { get; set; }

        /// <summary>
        /// 當季銷售
        /// </summary>
        public decimal CurrentQuarterSales { get; set; }

        /// <summary>
        /// 當年銷售
        /// </summary>
        public decimal CurrentYearSales { get; set; }

        /// <summary>
        /// 目標達成率
        /// </summary>
        public double TargetAchievement { get; set; }

        /// <summary>
        /// 績效評級
        /// </summary>
        public string PerformanceRating { get; set; } = string.Empty;
    }
}