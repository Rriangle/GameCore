namespace GameCore.Core.Types
{
    /// <summary>
    /// 交易
    /// </summary>
    public class Transaction
    {
        /// <summary>
        /// 交易ID
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 交易編號
        /// </summary>
        public string TransactionNumber { get; set; } = string.Empty;

        /// <summary>
        /// 用戶ID
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// 交易類型
        /// </summary>
        public string TransactionType { get; set; } = string.Empty;

        /// <summary>
        /// 交易金額
        /// </summary>
        public decimal Amount { get; set; }

        /// <summary>
        /// 交易前餘額
        /// </summary>
        public decimal BalanceBefore { get; set; }

        /// <summary>
        /// 交易後餘額
        /// </summary>
        public decimal BalanceAfter { get; set; }

        /// <summary>
        /// 交易狀態
        /// </summary>
        public string Status { get; set; } = string.Empty;

        /// <summary>
        /// 交易描述
        /// </summary>
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// 相關實體ID
        /// </summary>
        public int? RelatedEntityId { get; set; }

        /// <summary>
        /// 相關實體類型
        /// </summary>
        public string? RelatedEntityType { get; set; }

        /// <summary>
        /// 交易時間
        /// </summary>
        public DateTime TransactionTime { get; set; }

        /// <summary>
        /// 完成時間
        /// </summary>
        public DateTime? CompletedTime { get; set; }

        /// <summary>
        /// 失敗時間
        /// </summary>
        public DateTime? FailedTime { get; set; }

        /// <summary>
        /// 失敗原因
        /// </summary>
        public string? FailureReason { get; set; }

        /// <summary>
        /// 交易費用
        /// </summary>
        public decimal Fee { get; set; }

        /// <summary>
        /// 交易稅金
        /// </summary>
        public decimal Tax { get; set; }

        /// <summary>
        /// 淨額
        /// </summary>
        public decimal NetAmount => Amount - Fee - Tax;

        /// <summary>
        /// 交易來源
        /// </summary>
        public string Source { get; set; } = string.Empty;

        /// <summary>
        /// 交易目標
        /// </summary>
        public string Target { get; set; } = string.Empty;

        /// <summary>
        /// 交易IP
        /// </summary>
        public string? IpAddress { get; set; }

        /// <summary>
        /// 用戶代理
        /// </summary>
        public string? UserAgent { get; set; }

        /// <summary>
        /// 地理位置
        /// </summary>
        public string? Location { get; set; }

        /// <summary>
        /// 風險分數
        /// </summary>
        public int? RiskScore { get; set; }

        /// <summary>
        /// 是否可疑
        /// </summary>
        public bool IsSuspicious { get; set; }

        /// <summary>
        /// 是否已審核
        /// </summary>
        public bool IsReviewed { get; set; }

        /// <summary>
        /// 審核者ID
        /// </summary>
        public int? ReviewerId { get; set; }

        /// <summary>
        /// 審核時間
        /// </summary>
        public DateTime? ReviewedAt { get; set; }

        /// <summary>
        /// 審核結果
        /// </summary>
        public string? ReviewResult { get; set; }

        /// <summary>
        /// 審核備註
        /// </summary>
        public string? ReviewNotes { get; set; }

        /// <summary>
        /// 元數據
        /// </summary>
        public Dictionary<string, object> Metadata { get; set; } = new();

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
