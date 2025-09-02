using GameCore.Core.DTOs;

namespace GameCore.Domain.Interfaces
{
    /// <summary>
    /// 銷售服務介面
    /// </summary>
    public interface ISalesService
    {
        /// <summary>
        /// 取得用戶銷售統計
        /// </summary>
        /// <param name="userId">用戶ID</param>
        /// <returns>銷售統計資訊</returns>
        Task<SalesStatisticsResponse> GetSalesStatisticsAsync(int userId);

        /// <summary>
        /// 取得銷售權限申請列表（管理員功能）
        /// </summary>
        /// <param name="request">查詢條件</param>
        /// <returns>申請列表</returns>
        Task<SalesPermissionListResponse> GetSalesPermissionApplicationsAsync(SalesPermissionListRequest request);

        /// <summary>
        /// 審核銷售權限申請（管理員功能）
        /// </summary>
        /// <param name="request">審核請求</param>
        /// <returns>審核結果</returns>
        Task<SalesPermissionReviewResponse> ReviewSalesPermissionAsync(SalesPermissionReviewRequest request);

        /// <summary>
        /// 取得銷售排行榜
        /// </summary>
        /// <param name="period">期間類型（daily/weekly/monthly）</param>
        /// <param name="limit">顯示數量</param>
        /// <returns>銷售排行榜</returns>
        Task<List<SalesRankingItem>> GetSalesRankingAsync(string period, int limit = 10);

        /// <summary>
        /// 取得銷售報表
        /// </summary>
        /// <param name="userId">用戶ID</param>
        /// <param name="startDate">開始日期</param>
        /// <param name="endDate">結束日期</param>
        /// <returns>銷售報表</returns>
        Task<SalesReportResponse> GetSalesReportAsync(int userId, DateTime startDate, DateTime endDate);
    }

    /// <summary>
    /// 銷售統計回應
    /// </summary>
    public class SalesStatisticsResponse
    {
        /// <summary>
        /// 用戶ID
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// 本月銷售額
        /// </summary>
        public decimal MonthlySales { get; set; }

        /// <summary>
        /// 本月訂單數
        /// </summary>
        public int MonthlyOrders { get; set; }

        /// <summary>
        /// 累計銷售額
        /// </summary>
        public decimal TotalSales { get; set; }

        /// <summary>
        /// 累計訂單數
        /// </summary>
        public int TotalOrders { get; set; }

        /// <summary>
        /// 平均訂單金額
        /// </summary>
        public decimal AverageOrderValue { get; set; }

        /// <summary>
        /// 銷售權限狀態
        /// </summary>
        public string PermissionStatus { get; set; } = string.Empty;
    }

    /// <summary>
    /// 銷售權限申請列表請求
    /// </summary>
    public class SalesPermissionListRequest
    {
        /// <summary>
        /// 狀態篩選
        /// </summary>
        public string? Status { get; set; }

        /// <summary>
        /// 開始日期
        /// </summary>
        public DateTime? FromDate { get; set; }

        /// <summary>
        /// 結束日期
        /// </summary>
        public DateTime? ToDate { get; set; }

        /// <summary>
        /// 頁碼
        /// </summary>
        public int Page { get; set; } = 1;

        /// <summary>
        /// 每頁筆數
        /// </summary>
        public int PageSize { get; set; } = 20;
    }

    /// <summary>
    /// 銷售權限申請列表回應
    /// </summary>
    public class SalesPermissionListResponse
    {
        /// <summary>
        /// 申請列表
        /// </summary>
        public List<SalesPermissionApplicationItem> Applications { get; set; } = new();

        /// <summary>
        /// 總筆數
        /// </summary>
        public int TotalCount { get; set; }

        /// <summary>
        /// 總頁數
        /// </summary>
        public int TotalPages { get; set; }
    }

    /// <summary>
    /// 銷售權限申請項目
    /// </summary>
    public class SalesPermissionApplicationItem
    {
        /// <summary>
        /// 申請ID
        /// </summary>
        public int ApplicationId { get; set; }

        /// <summary>
        /// 用戶ID
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// 用戶名稱
        /// </summary>
        public string UserName { get; set; } = string.Empty;

        /// <summary>
        /// 申請時間
        /// </summary>
        public DateTime ApplicationTime { get; set; }

        /// <summary>
        /// 申請狀態
        /// </summary>
        public string Status { get; set; } = string.Empty;

        /// <summary>
        /// 銀行代號
        /// </summary>
        public int BankCode { get; set; }

        /// <summary>
        /// 銀行帳號（部分隱藏）
        /// </summary>
        public string MaskedBankAccount { get; set; } = string.Empty;

        /// <summary>
        /// 申請說明
        /// </summary>
        public string? ApplicationNote { get; set; }
    }

    /// <summary>
    /// 銷售權限審核請求
    /// </summary>
    public class SalesPermissionReviewRequest
    {
        /// <summary>
        /// 申請ID
        /// </summary>
        public int ApplicationId { get; set; }

        /// <summary>
        /// 審核結果（approved/rejected）
        /// </summary>
        public string ReviewResult { get; set; } = string.Empty;

        /// <summary>
        /// 審核說明
        /// </summary>
        public string ReviewNote { get; set; } = string.Empty;

        /// <summary>
        /// 管理員ID
        /// </summary>
        public int ManagerId { get; set; }
    }

    /// <summary>
    /// 銷售權限審核回應
    /// </summary>
    public class SalesPermissionReviewResponse
    {
        /// <summary>
        /// 審核是否成功
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// 審核時間
        /// </summary>
        public DateTime ReviewTime { get; set; }

        /// <summary>
        /// 通知ID（若成功發送通知）
        /// </summary>
        public int? NotificationId { get; set; }
    }

    /// <summary>
    /// 銷售排行榜項目
    /// </summary>
    public class SalesRankingItem
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
        /// 用戶名稱
        /// </summary>
        public string UserName { get; set; } = string.Empty;

        /// <summary>
        /// 銷售額
        /// </summary>
        public decimal SalesAmount { get; set; }

        /// <summary>
        /// 訂單數
        /// </summary>
        public int OrderCount { get; set; }

        /// <summary>
        /// 與上期排名變化
        /// </summary>
        public int RankChange { get; set; }
    }

    /// <summary>
    /// 銷售報表回應
    /// </summary>
    public class SalesReportResponse
    {
        /// <summary>
        /// 用戶ID
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// 報表期間
        /// </summary>
        public string Period { get; set; } = string.Empty;

        /// <summary>
        /// 總銷售額
        /// </summary>
        public decimal TotalSales { get; set; }

        /// <summary>
        /// 總訂單數
        /// </summary>
        public int TotalOrders { get; set; }

        /// <summary>
        /// 平均訂單金額
        /// </summary>
        public decimal AverageOrderValue { get; set; }

        /// <summary>
        /// 最高單筆銷售
        /// </summary>
        public decimal HighestOrderValue { get; set; }

        /// <summary>
        /// 最低單筆銷售
        /// </summary>
        public decimal LowestOrderValue { get; set; }

        /// <summary>
        /// 每日銷售數據
        /// </summary>
        public List<DailySalesData> DailyData { get; set; } = new();
    }

    /// <summary>
    /// 每日銷售數據
    /// </summary>
    public class DailySalesData
    {
        /// <summary>
        /// 日期
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// 銷售額
        /// </summary>
        public decimal SalesAmount { get; set; }

        /// <summary>
        /// 訂單數
        /// </summary>
        public int OrderCount { get; set; }
    }
} 
