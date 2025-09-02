using System.ComponentModel.DataAnnotations;

namespace GameCore.Core.Types
{
    // 移除重複的 Product 類別定義，使用 Entities 中的定義
    // public class Product { ... }

    // 移除重複的 MarketTransaction 類別定義，使用 Entities 中的定義
    // public class MarketTransaction { ... }

    /// <summary>
    /// 原子交易請求
    /// </summary>
    public class AtomicTransactionRequest
    {
        /// <summary>
        /// 使用者編號
        /// </summary>
        [Required]
        public int UserId { get; set; }

        /// <summary>
        /// 交易金額
        /// </summary>
        [Required]
        [Range(0.01, double.MaxValue)]
        public decimal Amount { get; set; }

        /// <summary>
        /// 交易類型
        /// </summary>
        [Required]
        public string TransactionType { get; set; } = string.Empty;

        /// <summary>
        /// 交易描述
        /// </summary>
        [StringLength(500)]
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// 相關交易編號
        /// </summary>
        public int? RelatedTransactionId { get; set; }

        /// <summary>
        /// 請求時間戳
        /// </summary>
        public DateTime RequestTimestamp { get; set; } = DateTime.UtcNow;
    }

    /// <summary>
    /// 錢包交易結果
    /// </summary>
    public class WalletTransactionResult
    {
        /// <summary>
        /// 是否成功
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// 結果訊息
        /// </summary>
        public string Message { get; set; } = string.Empty;

        /// <summary>
        /// 交易編號
        /// </summary>
        public int? TransactionId { get; set; }

        /// <summary>
        /// 交易金額
        /// </summary>
        public decimal Amount { get; set; }

        /// <summary>
        /// 交易後餘額
        /// </summary>
        public decimal NewBalance { get; set; }

        /// <summary>
        /// 交易時間
        /// </summary>
        public DateTime TransactionTime { get; set; }

        /// <summary>
        /// 交易狀態
        /// </summary>
        public string Status { get; set; } = string.Empty;
    }
} 
