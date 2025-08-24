using System.ComponentModel.DataAnnotations;

namespace GameCore.Core.DTOs
{
    #region 基本錢包 DTOs (Basic Wallet DTOs)

    /// <summary>
    /// 錢包資訊 DTO - 顯示使用者當前點數餘額和優惠券資訊
    /// Wallet information DTO - displays user's current point balance and coupon info
    /// </summary>
    public class WalletDto
    {
        /// <summary>使用者編號</summary>
        public int UserId { get; set; }

        /// <summary>當前點數餘額</summary>
        public int CurrentPoints { get; set; }

        /// <summary>優惠券編號</summary>
        public string? CouponNumber { get; set; }

        /// <summary>錢包最後更新時間</summary>
        public DateTime LastUpdated { get; set; }

        /// <summary>是否有銷售權限</summary>
        public bool HasSalesAuthority { get; set; }

        /// <summary>銷售錢包餘額 (如果有開通銷售功能)</summary>
        public decimal? SalesWalletBalance { get; set; }
    }

    #endregion

    #region 收支明細 DTOs (Ledger DTOs)

    /// <summary>
    /// 收支明細查詢條件 DTO
    /// Ledger query conditions DTO
    /// </summary>
    public class LedgerQueryDto
    {
        /// <summary>查詢開始時間</summary>
        public DateTime? FromDate { get; set; }

        /// <summary>查詢結束時間</summary>
        public DateTime? ToDate { get; set; }

        /// <summary>交易類型篩選 (signin, minigame, pet_color, adjustment)</summary>
        public string? TransactionType { get; set; }

        /// <summary>頁碼 (預設 1)</summary>
        public int Page { get; set; } = 1;

        /// <summary>每頁筆數 (預設 20)</summary>
        public int PageSize { get; set; } = 20;

        /// <summary>排序方式 (date_desc, date_asc, amount_desc, amount_asc)</summary>
        public string SortBy { get; set; } = "date_desc";
    }

    /// <summary>
    /// 收支明細項目 DTO - 彙整各來源的點數異動記錄
    /// Ledger entry DTO - aggregated point change records from various sources
    /// </summary>
    public class LedgerEntryDto
    {
        /// <summary>流水號 (組合ID，用於唯一識別)</summary>
        public string Id { get; set; } = string.Empty;

        /// <summary>交易時間</summary>
        public DateTime Timestamp { get; set; }

        /// <summary>交易類型 (signin=簽到, minigame=小遊戲, pet_color=寵物換色, adjustment=管理調整)</summary>
        public string Type { get; set; } = string.Empty;

        /// <summary>點數變化量 (正數=收入, 負數=支出)</summary>
        public int PointsDelta { get; set; }

        /// <summary>交易後餘額 (可選，用於顯示)</summary>
        public int? BalanceAfter { get; set; }

        /// <summary>交易描述</summary>
        public string Description { get; set; } = string.Empty;

        /// <summary>額外元資料 (JSON 格式，包含來源詳細資訊)</summary>
        public string? Metadata { get; set; }

        /// <summary>關聯的來源記錄ID</summary>
        public string? SourceRecordId { get; set; }
    }

    /// <summary>
    /// 點數統計摘要 DTO
    /// Points statistics summary DTO
    /// </summary>
    public class PointsStatisticsDto
    {
        /// <summary>當前總點數</summary>
        public int TotalPoints { get; set; }

        /// <summary>今日獲得點數</summary>
        public int TodayEarned { get; set; }

        /// <summary>今日消費點數</summary>
        public int TodaySpent { get; set; }

        /// <summary>本週獲得點數</summary>
        public int WeekEarned { get; set; }

        /// <summary>本週消費點數</summary>
        public int WeekSpent { get; set; }

        /// <summary>本月獲得點數</summary>
        public int MonthEarned { get; set; }

        /// <summary>本月消費點數</summary>
        public int MonthSpent { get; set; }

        /// <summary>歷史總獲得點數</summary>
        public int TotalEarned { get; set; }

        /// <summary>歷史總消費點數</summary>
        public int TotalSpent { get; set; }

        /// <summary>主要收入來源統計</summary>
        public Dictionary<string, int> EarningsBySource { get; set; } = new();

        /// <summary>主要支出類型統計</summary>
        public Dictionary<string, int> SpendingByCategory { get; set; } = new();
    }

    #endregion

    #region 銷售功能 DTOs (Sales DTOs)

    /// <summary>
    /// 建立銷售檔案請求 DTO
    /// Create sales profile request DTO
    /// </summary>
    public class CreateSalesProfileDto
    {
        /// <summary>銀行代號</summary>
        [Required(ErrorMessage = "銀行代號必填")]
        [Range(1, 9999, ErrorMessage = "銀行代號格式錯誤")]
        public int BankCode { get; set; }

        /// <summary>銀行帳號</summary>
        [Required(ErrorMessage = "銀行帳號必填")]
        [StringLength(20, MinimumLength = 8, ErrorMessage = "銀行帳號長度需介於 8-20 字元")]
        public string BankAccountNumber { get; set; } = string.Empty;

        /// <summary>帳戶封面照片 (Base64 編碼)</summary>
        public string? AccountCoverPhotoBase64 { get; set; }

        /// <summary>申請理由</summary>
        [StringLength(500, ErrorMessage = "申請理由不可超過 500 字")]
        public string? ApplicationReason { get; set; }
    }

    /// <summary>
    /// 銷售檔案資訊 DTO
    /// Sales profile information DTO
    /// </summary>
    public class MemberSalesProfileDto
    {
        /// <summary>使用者編號</summary>
        public int UserId { get; set; }

        /// <summary>銀行代號</summary>
        public int BankCode { get; set; }

        /// <summary>銀行名稱 (查詢得出)</summary>
        public string BankName { get; set; } = string.Empty;

        /// <summary>銀行帳號 (遮蔽顯示)</summary>
        public string MaskedBankAccountNumber { get; set; } = string.Empty;

        /// <summary>帳戶封面照片 URL</summary>
        public string? AccountCoverPhotoUrl { get; set; }

        /// <summary>銷售權限狀態</summary>
        public bool SalesAuthorityEnabled { get; set; }

        /// <summary>檔案建立時間</summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>審核狀態 (pending=待審核, approved=已通過, rejected=已拒絕)</summary>
        public string ReviewStatus { get; set; } = "pending";

        /// <summary>審核備註</summary>
        public string? ReviewNotes { get; set; }
    }

    /// <summary>
    /// 使用者銷售資訊 DTO (銷售錢包)
    /// User sales information DTO (sales wallet)
    /// </summary>
    public class UserSalesInformationDto
    {
        /// <summary>使用者編號</summary>
        public int UserId { get; set; }

        /// <summary>銷售錢包餘額</summary>
        public int UserSalesWallet { get; set; }

        /// <summary>最後更新時間</summary>
        public DateTime LastUpdated { get; set; }

        /// <summary>累計銷售總額</summary>
        public decimal TotalSalesAmount { get; set; }

        /// <summary>累計平台手續費</summary>
        public decimal TotalPlatformFees { get; set; }

        /// <summary>可提領金額</summary>
        public decimal WithdrawableAmount { get; set; }

        /// <summary>本月銷售額</summary>
        public decimal MonthlyRevenue { get; set; }
    }

    #endregion

    #region 交易處理 DTOs (Transaction DTOs)

    /// <summary>
    /// 錢包交易結果 DTO
    /// Wallet transaction result DTO
    /// </summary>
    public class WalletTransactionDto
    {
        /// <summary>交易ID</summary>
        public string TransactionId { get; set; } = string.Empty;

        /// <summary>使用者編號</summary>
        public int UserId { get; set; }

        /// <summary>交易類型 (earn=獲得, spend=消費)</summary>
        public string TransactionType { get; set; } = string.Empty;

        /// <summary>點數變化量</summary>
        public int PointsDelta { get; set; }

        /// <summary>交易前餘額</summary>
        public int BalanceBefore { get; set; }

        /// <summary>交易後餘額</summary>
        public int BalanceAfter { get; set; }

        /// <summary>交易描述</summary>
        public string Description { get; set; } = string.Empty;

        /// <summary>關聯ID (如訂單ID、寵物ID等)</summary>
        public string? ReferenceId { get; set; }

        /// <summary>交易時間</summary>
        public DateTime Timestamp { get; set; }

        /// <summary>交易狀態 (success=成功, failed=失敗, pending=處理中)</summary>
        public string Status { get; set; } = "success";
    }

    #endregion

    #region 分頁結果 (Paged Result)

    /// <summary>
    /// 分頁結果 DTO
    /// Paged result DTO
    /// </summary>
    /// <typeparam name="T">項目類型</typeparam>
    public class PagedResult<T>
    {
        /// <summary>項目清單</summary>
        public List<T> Items { get; set; } = new();

        /// <summary>總項目數</summary>
        public int TotalCount { get; set; }

        /// <summary>目前頁碼</summary>
        public int CurrentPage { get; set; }

        /// <summary>每頁項目數</summary>
        public int PageSize { get; set; }

        /// <summary>總頁數</summary>
        public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);

        /// <summary>是否有下一頁</summary>
        public bool HasNextPage => CurrentPage < TotalPages;

        /// <summary>是否有上一頁</summary>
        public bool HasPreviousPage => CurrentPage > 1;
    }

    #endregion
}