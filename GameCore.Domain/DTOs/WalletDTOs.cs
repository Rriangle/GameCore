using System.ComponentModel.DataAnnotations;

namespace GameCore.Core.DTOs
{
    /// <summary>
    /// 錢包餘額查詢回應
    /// </summary>
    public class WalletBalanceResponse
    {
        /// <summary>
        /// 用戶ID
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// 當前點數餘額
        /// </summary>
        public int UserPoint { get; set; }

        /// <summary>
        /// 優惠券編號（若有）
        /// </summary>
        public string? CouponNumber { get; set; }

        /// <summary>
        /// 最後更新時間
        /// </summary>
        public DateTime LastUpdated { get; set; }
    }

    /// <summary>
    /// 點數流水記錄項目
    /// </summary>
    public class PointTransactionItem
    {
        /// <summary>
        /// 交易ID
        /// </summary>
        public string TransactionId { get; set; } = string.Empty;

        /// <summary>
        /// 交易時間
        /// </summary>
        public DateTime TransactionTime { get; set; }

        /// <summary>
        /// 交易類型
        /// </summary>
        public string TransactionType { get; set; } = string.Empty;

        /// <summary>
        /// 點數變化量（正數為增加，負數為減少）
        /// </summary>
        public int PointsChanged { get; set; }

        /// <summary>
        /// 交易後餘額
        /// </summary>
        public int BalanceAfter { get; set; }

        /// <summary>
        /// 交易描述
        /// </summary>
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// 相關元資料（JSON格式）
        /// </summary>
        public string? Metadata { get; set; }
    }

    /// <summary>
    /// 點數流水查詢請求
    /// </summary>
    public class PointTransactionQueryRequest
    {
        /// <summary>
        /// 開始日期
        /// </summary>
        public DateTime? FromDate { get; set; }

        /// <summary>
        /// 結束日期
        /// </summary>
        public DateTime? ToDate { get; set; }

        /// <summary>
        /// 交易類型篩選
        /// </summary>
        public string? TransactionType { get; set; }

        /// <summary>
        /// 頁碼（從1開始）
        /// </summary>
        [Range(1, int.MaxValue)]
        public int Page { get; set; } = 1;

        /// <summary>
        /// 每頁筆數
        /// </summary>
        [Range(1, 100)]
        public int PageSize { get; set; } = 20;
    }

    /// <summary>
    /// 點數流水查詢回應
    /// </summary>
    public class PointTransactionQueryResponse
    {
        /// <summary>
        /// 交易記錄列表
        /// </summary>
        public List<PointTransactionItem> Transactions { get; set; } = new();

        /// <summary>
        /// 總筆數
        /// </summary>
        public int TotalCount { get; set; }

        /// <summary>
        /// 總頁數
        /// </summary>
        public int TotalPages { get; set; }

        /// <summary>
        /// 當前頁碼
        /// </summary>
        public int CurrentPage { get; set; }

        /// <summary>
        /// 每頁筆數
        /// </summary>
        public int PageSize { get; set; }
    }

    /// <summary>
    /// 銷售權限申請請求
    /// </summary>
    public class SalesPermissionRequest
    {
        /// <summary>
        /// 銀行代號
        /// </summary>
        [Required]
        [Range(1, 999)]
        public int BankCode { get; set; }

        /// <summary>
        /// 銀行帳號
        /// </summary>
        [Required]
        [StringLength(20)]
        public string BankAccountNumber { get; set; } = string.Empty;

        /// <summary>
        /// 帳戶封面照片（Base64編碼）
        /// </summary>
        [Required]
        public string AccountCoverPhoto { get; set; } = string.Empty;

        /// <summary>
        /// 申請說明
        /// </summary>
        [StringLength(500)]
        public string? ApplicationNote { get; set; }
    }

    /// <summary>
    /// 銷售權限申請回應
    /// </summary>
    public class SalesPermissionResponse
    {
        /// <summary>
        /// 申請ID
        /// </summary>
        public int ApplicationId { get; set; }

        /// <summary>
        /// 申請狀態
        /// </summary>
        public string Status { get; set; } = string.Empty;

        /// <summary>
        /// 申請時間
        /// </summary>
        public DateTime ApplicationTime { get; set; }

        /// <summary>
        /// 審核時間（若已審核）
        /// </summary>
        public DateTime? ReviewedTime { get; set; }

        /// <summary>
        /// 審核結果說明
        /// </summary>
        public string? ReviewNote { get; set; }
    }

    /// <summary>
    /// 銷售錢包資訊
    /// </summary>
    public class SalesWalletInfo
    {
        /// <summary>
        /// 用戶ID
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// 銷售錢包餘額
        /// </summary>
        public int UserSalesWallet { get; set; }

        /// <summary>
        /// 銷售權限狀態
        /// </summary>
        public bool HasSalesAuthority { get; set; }

        /// <summary>
        /// 最後交易時間
        /// </summary>
        public DateTime? LastTransactionTime { get; set; }

        /// <summary>
        /// 本月銷售額
        /// </summary>
        public decimal MonthlySales { get; set; }

        /// <summary>
        /// 累計銷售額
        /// </summary>
        public decimal TotalSales { get; set; }
    }

    /// <summary>
    /// 管理員調整點數請求
    /// </summary>
    public class AdminPointAdjustmentRequest
    {
        /// <summary>
        /// 目標用戶ID
        /// </summary>
        [Required]
        [Range(1, int.MaxValue)]
        public int UserId { get; set; }

        /// <summary>
        /// 點數調整量（正數為增加，負數為減少）
        /// </summary>
        [Required]
        public int PointsDelta { get; set; }

        /// <summary>
        /// 調整原因
        /// </summary>
        [Required]
        [StringLength(200)]
        public string Reason { get; set; } = string.Empty;

        /// <summary>
        /// 管理員ID
        /// </summary>
        [Required]
        public int ManagerId { get; set; }
    }

    /// <summary>
    /// 管理員調整點數回應
    /// </summary>
    public class AdminPointAdjustmentResponse
    {
        /// <summary>
        /// 調整是否成功
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// 調整前餘額
        /// </summary>
        public int PreviousBalance { get; set; }

        /// <summary>
        /// 調整後餘額
        /// </summary>
        public int NewBalance { get; set; }

        /// <summary>
        /// 調整時間
        /// </summary>
        public DateTime AdjustmentTime { get; set; }

        /// <summary>
        /// 通知ID（若成功發送通知）
        /// </summary>
        public int? NotificationId { get; set; }
    }
} 
