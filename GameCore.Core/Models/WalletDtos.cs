namespace GameCore.Core.Models
{
    /// <summary>
    /// 錢包餘額 DTO
    /// </summary>
    public class WalletBalanceDto
    {
        /// <summary>
        /// 用戶ID
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// 總點數
        /// </summary>
        public decimal TotalPoints { get; set; }

        /// <summary>
        /// 可用點數
        /// </summary>
        public decimal AvailablePoints { get; set; }

        /// <summary>
        /// 凍結點數
        /// </summary>
        public decimal FrozenPoints { get; set; }

        /// <summary>
        /// 最後更新時間
        /// </summary>
        public DateTime LastUpdated { get; set; }
    }

    /// <summary>
    /// 點數明細 DTO
    /// </summary>
    public class PointLedgerDto
    {
        /// <summary>
        /// 明細項目列表
        /// </summary>
        public List<PointLedgerItemDto> Items { get; set; } = new List<PointLedgerItemDto>();

        /// <summary>
        /// 總數量
        /// </summary>
        public int TotalCount { get; set; }

        /// <summary>
        /// 當前頁碼
        /// </summary>
        public int Page { get; set; }

        /// <summary>
        /// 每頁大小
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// 總頁數
        /// </summary>
        public int TotalPages { get; set; }
    }

    /// <summary>
    /// 點數明細項目 DTO
    /// </summary>
    public class PointLedgerItemDto
    {
        /// <summary>
        /// 明細ID
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 類型
        /// </summary>
        public string Type { get; set; } = string.Empty;

        /// <summary>
        /// 金額
        /// </summary>
        public decimal Amount { get; set; }

        /// <summary>
        /// 餘額
        /// </summary>
        public decimal Balance { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// 創建時間
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// 關聯ID
        /// </summary>
        public int? RelatedId { get; set; }

        /// <summary>
        /// 關聯類型
        /// </summary>
        public string? RelatedType { get; set; }
    }

    /// <summary>
    /// 點數統計 DTO
    /// </summary>
    public class PointStatisticsDto
    {
        /// <summary>
        /// 統計週期
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
        /// 總收入
        /// </summary>
        public decimal TotalIncome { get; set; }

        /// <summary>
        /// 總支出
        /// </summary>
        public decimal TotalExpense { get; set; }

        /// <summary>
        /// 淨變更
        /// </summary>
        public decimal NetChange { get; set; }

        /// <summary>
        /// 各類型收入
        /// </summary>
        public List<PointTypeSummaryDto> IncomeByType { get; set; } = new List<PointTypeSummaryDto>();

        /// <summary>
        /// 各類型支出
        /// </summary>
        public List<PointTypeSummaryDto> ExpenseByType { get; set; } = new List<PointTypeSummaryDto>();
    }

    /// <summary>
    /// 點數類型摘要 DTO
    /// </summary>
    public class PointTypeSummaryDto
    {
        /// <summary>
        /// 類型
        /// </summary>
        public string Type { get; set; } = string.Empty;

        /// <summary>
        /// 金額
        /// </summary>
        public decimal Amount { get; set; }

        /// <summary>
        /// 數量
        /// </summary>
        public int Count { get; set; }
    }

    /// <summary>
    /// 優惠券 DTO
    /// </summary>
    public class CouponDto
    {
        /// <summary>
        /// 優惠券ID
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 優惠券代碼
        /// </summary>
        public string Code { get; set; } = string.Empty;

        /// <summary>
        /// 類型
        /// </summary>
        public string Type { get; set; } = string.Empty;

        /// <summary>
        /// 面值
        /// </summary>
        public decimal Value { get; set; }

        /// <summary>
        /// 最低使用金額
        /// </summary>
        public decimal MinAmount { get; set; }

        /// <summary>
        /// 最大折扣金額
        /// </summary>
        public decimal? MaxDiscount { get; set; }

        /// <summary>
        /// 狀態
        /// </summary>
        public string Status { get; set; } = string.Empty;

        /// <summary>
        /// 過期時間
        /// </summary>
        public DateTime? ExpiredAt { get; set; }

        /// <summary>
        /// 使用時間
        /// </summary>
        public DateTime? UsedAt { get; set; }

        /// <summary>
        /// 創建時間
        /// </summary>
        public DateTime CreatedAt { get; set; }
    }

    /// <summary>
    /// 優惠券使用結果 DTO
    /// </summary>
    public class CouponUsageResultDto
    {
        /// <summary>
        /// 優惠券ID
        /// </summary>
        public int CouponId { get; set; }

        /// <summary>
        /// 優惠券代碼
        /// </summary>
        public string CouponCode { get; set; } = string.Empty;

        /// <summary>
        /// 原始金額
        /// </summary>
        public decimal OriginalAmount { get; set; }

        /// <summary>
        /// 折扣金額
        /// </summary>
        public decimal DiscountAmount { get; set; }

        /// <summary>
        /// 最終金額
        /// </summary>
        public decimal FinalAmount { get; set; }

        /// <summary>
        /// 使用時間
        /// </summary>
        public DateTime UsedAt { get; set; }
    }

    /// <summary>
    /// 點數排行榜 DTO
    /// </summary>
    public class PointLeaderboardDto
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
        /// 郵箱
        /// </summary>
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// 總點數
        /// </summary>
        public decimal TotalPoints { get; set; }

        /// <summary>
        /// 交易次數
        /// </summary>
        public int TransactionCount { get; set; }
    }

    /// <summary>
    /// 點數賺取方式 DTO
    /// </summary>
    public class PointEarningMethodDto
    {
        /// <summary>
        /// 方式代碼
        /// </summary>
        public string Method { get; set; } = string.Empty;

        /// <summary>
        /// 方式名稱
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// 基礎點數
        /// </summary>
        public decimal BasePoints { get; set; }

        /// <summary>
        /// 獎勵點數
        /// </summary>
        public decimal BonusPoints { get; set; }

        /// <summary>
        /// 最大獎勵天數
        /// </summary>
        public int MaxBonusDays { get; set; }

        /// <summary>
        /// 冷卻時間（小時）
        /// </summary>
        public int CooldownHours { get; set; }
    }

    /// <summary>
    /// 點數支出歷史 DTO
    /// </summary>
    public class PointSpendingHistoryDto
    {
        /// <summary>
        /// 支出項目列表
        /// </summary>
        public List<PointSpendingItemDto> Items { get; set; } = new List<PointSpendingItemDto>();

        /// <summary>
        /// 總數量
        /// </summary>
        public int TotalCount { get; set; }

        /// <summary>
        /// 當前頁碼
        /// </summary>
        public int Page { get; set; }

        /// <summary>
        /// 每頁大小
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// 總頁數
        /// </summary>
        public int TotalPages { get; set; }
    }

    /// <summary>
    /// 點數支出項目 DTO
    /// </summary>
    public class PointSpendingItemDto
    {
        /// <summary>
        /// 項目ID
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 類型
        /// </summary>
        public string Type { get; set; } = string.Empty;

        /// <summary>
        /// 金額
        /// </summary>
        public decimal Amount { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// 創建時間
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// 關聯ID
        /// </summary>
        public int? RelatedId { get; set; }

        /// <summary>
        /// 關聯類型
        /// </summary>
        public string? RelatedType { get; set; }
    }

    /// <summary>
    /// 點數預測 DTO
    /// </summary>
    public class PointForecastDto
    {
        /// <summary>
        /// 用戶ID
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// 預測天數
        /// </summary>
        public int ForecastDays { get; set; }

        /// <summary>
        /// 當前餘額
        /// </summary>
        public decimal CurrentBalance { get; set; }

        /// <summary>
        /// 預測收入
        /// </summary>
        public List<PointForecastItemDto> PredictedIncome { get; set; } = new List<PointForecastItemDto>();

        /// <summary>
        /// 預測支出
        /// </summary>
        public List<PointForecastItemDto> PredictedExpense { get; set; } = new List<PointForecastItemDto>();

        /// <summary>
        /// 淨預測
        /// </summary>
        public decimal NetPrediction { get; set; }
    }

    /// <summary>
    /// 點數預測項目 DTO
    /// </summary>
    public class PointForecastItemDto
    {
        /// <summary>
        /// 類型
        /// </summary>
        public string Type { get; set; } = string.Empty;

        /// <summary>
        /// 預測金額
        /// </summary>
        public decimal PredictedAmount { get; set; }

        /// <summary>
        /// 信心度
        /// </summary>
        public double Confidence { get; set; }
    }

    // 管理員用 DTO

    /// <summary>
    /// 點數調整 DTO
    /// </summary>
    public class PointAdjustmentDto
    {
        /// <summary>
        /// 用戶ID
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// 舊餘額
        /// </summary>
        public decimal OldBalance { get; set; }

        /// <summary>
        /// 新餘額
        /// </summary>
        public decimal NewBalance { get; set; }

        /// <summary>
        /// 調整金額
        /// </summary>
        public decimal AdjustmentAmount { get; set; }

        /// <summary>
        /// 調整類型
        /// </summary>
        public string Type { get; set; } = string.Empty;

        /// <summary>
        /// 調整說明
        /// </summary>
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// 管理員ID
        /// </summary>
        public int AdminId { get; set; }

        /// <summary>
        /// 調整時間
        /// </summary>
        public DateTime AdjustedAt { get; set; }
    }

    /// <summary>
    /// 點數變更歷史 DTO
    /// </summary>
    public class PointHistoryDto
    {
        /// <summary>
        /// 變更項目列表
        /// </summary>
        public List<PointHistoryItemDto> Items { get; set; } = new List<PointHistoryItemDto>();

        /// <summary>
        /// 總數量
        /// </summary>
        public int TotalCount { get; set; }

        /// <summary>
        /// 當前頁碼
        /// </summary>
        public int Page { get; set; }

        /// <summary>
        /// 每頁大小
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// 總頁數
        /// </summary>
        public int TotalPages { get; set; }
    }

    /// <summary>
    /// 點數變更歷史項目 DTO
    /// </summary>
    public class PointHistoryItemDto
    {
        /// <summary>
        /// 項目ID
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 類型
        /// </summary>
        public string Type { get; set; } = string.Empty;

        /// <summary>
        /// 金額
        /// </summary>
        public decimal Amount { get; set; }

        /// <summary>
        /// 餘額
        /// </summary>
        public decimal Balance { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// 創建時間
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// 關聯ID
        /// </summary>
        public int? RelatedId { get; set; }

        /// <summary>
        /// 關聯類型
        /// </summary>
        public string? RelatedType { get; set; }
    }

    /// <summary>
    /// 點數餘額檢查 DTO
    /// </summary>
    public class PointBalanceCheckDto
    {
        /// <summary>
        /// 用戶ID
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// 總點數
        /// </summary>
        public decimal TotalPoints { get; set; }

        /// <summary>
        /// 可用點數
        /// </summary>
        public decimal AvailablePoints { get; set; }

        /// <summary>
        /// 凍結點數
        /// </summary>
        public decimal FrozenPoints { get; set; }

        /// <summary>
        /// 最後更新時間
        /// </summary>
        public DateTime LastUpdated { get; set; }

        /// <summary>
        /// 是否充足
        /// </summary>
        public bool IsSufficient { get; set; }
    }

    /// <summary>
    /// 點數凍結 DTO
    /// </summary>
    public class PointFreezeDto
    {
        /// <summary>
        /// 用戶ID
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// 凍結金額
        /// </summary>
        public decimal FrozenAmount { get; set; }

        /// <summary>
        /// 可用點數
        /// </summary>
        public decimal AvailablePoints { get; set; }

        /// <summary>
        /// 凍結點數
        /// </summary>
        public decimal FrozenPoints { get; set; }

        /// <summary>
        /// 凍結原因
        /// </summary>
        public string Reason { get; set; } = string.Empty;

        /// <summary>
        /// 管理員ID
        /// </summary>
        public int AdminId { get; set; }

        /// <summary>
        /// 凍結時間
        /// </summary>
        public DateTime FrozenAt { get; set; }
    }

    /// <summary>
    /// 點數解凍 DTO
    /// </summary>
    public class PointUnfreezeDto
    {
        /// <summary>
        /// 用戶ID
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// 解凍金額
        /// </summary>
        public decimal UnfrozenAmount { get; set; }

        /// <summary>
        /// 可用點數
        /// </summary>
        public decimal AvailablePoints { get; set; }

        /// <summary>
        /// 凍結點數
        /// </summary>
        public decimal FrozenPoints { get; set; }

        /// <summary>
        /// 解凍原因
        /// </summary>
        public string Reason { get; set; } = string.Empty;

        /// <summary>
        /// 管理員ID
        /// </summary>
        public int AdminId { get; set; }

        /// <summary>
        /// 解凍時間
        /// </summary>
        public DateTime UnfrozenAt { get; set; }
    }

    /// <summary>
    /// 凍結點數資訊 DTO
    /// </summary>
    public class FrozenPointsDto
    {
        /// <summary>
        /// 用戶ID
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// 凍結點數
        /// </summary>
        public decimal FrozenPoints { get; set; }

        /// <summary>
        /// 可用點數
        /// </summary>
        public decimal AvailablePoints { get; set; }

        /// <summary>
        /// 總點數
        /// </summary>
        public decimal TotalPoints { get; set; }

        /// <summary>
        /// 最後更新時間
        /// </summary>
        public DateTime LastUpdated { get; set; }
    }
}