using System.ComponentModel.DataAnnotations;

namespace GameCore.Application.DTOs
{
    /// <summary>
    /// 錢包回應
    /// </summary>
    public class WalletDto
    {
        /// <summary>
        /// 錢包 ID
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 用戶 ID
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// 餘額
        /// </summary>
        public decimal Balance { get; set; }

        /// <summary>
        /// 貨幣類型
        /// </summary>
        public string Currency { get; set; } = "TWD";

        /// <summary>
        /// 最後更新時間
        /// </summary>
        public DateTime LastUpdated { get; set; }

        /// <summary>
        /// 是否啟用
        /// </summary>
        public bool IsActive { get; set; }
    }

    /// <summary>
    /// 交易回應
    /// </summary>
    public class TransactionDto
    {
        /// <summary>
        /// 交易 ID
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 錢包 ID
        /// </summary>
        public int WalletId { get; set; }

        /// <summary>
        /// 交易類型
        /// </summary>
        public string Type { get; set; } = string.Empty;

        /// <summary>
        /// 金額
        /// </summary>
        public decimal Amount { get; set; }

        /// <summary>
        /// 餘額變更
        /// </summary>
        public decimal BalanceChange { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// 交易時間
        /// </summary>
        public DateTime TransactionTime { get; set; }

        /// <summary>
        /// 狀態
        /// </summary>
        public string Status { get; set; } = string.Empty;

        /// <summary>
        /// 參考 ID
        /// </summary>
        public string? ReferenceId { get; set; }
    }

    /// <summary>
    /// 交易統計回應
    /// </summary>
    public class TransactionStatisticsDto
    {
        /// <summary>
        /// 總收入
        /// </summary>
        public decimal TotalIncome { get; set; }

        /// <summary>
        /// 總支出
        /// </summary>
        public decimal TotalExpense { get; set; }

        /// <summary>
        /// 淨收入
        /// </summary>
        public decimal NetIncome { get; set; }

        /// <summary>
        /// 交易次數
        /// </summary>
        public int TransactionCount { get; set; }

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
    /// 存款請求
    /// </summary>
    public class DepositRequest
    {
        /// <summary>
        /// 用戶 ID
        /// </summary>
        [Required(ErrorMessage = "用戶 ID 為必填")]
        public int UserId { get; set; }

        /// <summary>
        /// 金額
        /// </summary>
        [Required(ErrorMessage = "金額為必填")]
        [Range(0.01, double.MaxValue, ErrorMessage = "金額必須大於 0")]
        public decimal Amount { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        [StringLength(500, ErrorMessage = "描述長度不能超過 500 字元")]
        public string? Description { get; set; }
    }

    /// <summary>
    /// 提款請求
    /// </summary>
    public class WithdrawRequest
    {
        /// <summary>
        /// 用戶 ID
        /// </summary>
        [Required(ErrorMessage = "用戶 ID 為必填")]
        public int UserId { get; set; }

        /// <summary>
        /// 金額
        /// </summary>
        [Required(ErrorMessage = "金額為必填")]
        [Range(0.01, double.MaxValue, ErrorMessage = "金額必須大於 0")]
        public decimal Amount { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        [StringLength(500, ErrorMessage = "描述長度不能超過 500 字元")]
        public string? Description { get; set; }
    }

    /// <summary>
    /// 轉帳請求
    /// </summary>
    public class TransferRequest
    {
        /// <summary>
        /// 來源用戶 ID
        /// </summary>
        [Required(ErrorMessage = "來源用戶 ID 為必填")]
        public int FromUserId { get; set; }

        /// <summary>
        /// 目標用戶 ID
        /// </summary>
        [Required(ErrorMessage = "目標用戶 ID 為必填")]
        public int ToUserId { get; set; }

        /// <summary>
        /// 金額
        /// </summary>
        [Required(ErrorMessage = "金額為必填")]
        [Range(0.01, double.MaxValue, ErrorMessage = "金額必須大於 0")]
        public decimal Amount { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        [StringLength(500, ErrorMessage = "描述長度不能超過 500 字元")]
        public string? Description { get; set; }
    }
} 