using GameCore.Application.Common;
using GameCore.Application.DTOs;

namespace GameCore.Application.Services
{
    /// <summary>
    /// 銷售服務介面
    /// </summary>
    public interface ISalesService
    {
        /// <summary>
        /// 取得銷售報告
        /// </summary>
        /// <param name="startDate">開始日期</param>
        /// <param name="endDate">結束日期</param>
        /// <param name="reportType">報告類型</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>銷售報告</returns>
        Task<Result<SalesReportResponse>> GetSalesReportAsync(DateTime startDate, DateTime endDate, string reportType, CancellationToken cancellationToken = default);

        /// <summary>
        /// 取得系統統計
        /// </summary>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>系統統計</returns>
        Task<Result<SystemStatsResponse>> GetSystemStatsAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// 取得管理員用戶列表
        /// </summary>
        /// <param name="parameters">查詢參數</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>管理員用戶列表</returns>
        Task<Result<PagedResult<AdminUserResponse>>> GetAdminUsersAsync(AdminUserQueryParameters parameters, CancellationToken cancellationToken = default);

        /// <summary>
        /// 封禁用戶
        /// </summary>
        /// <param name="userId">用戶 ID</param>
        /// <param name="reason">封禁原因</param>
        /// <param name="duration">封禁時長（天）</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>封禁結果</returns>
        Task<OperationResult> BanUserAsync(int userId, string reason, int duration, CancellationToken cancellationToken = default);

        /// <summary>
        /// 解除封禁用戶
        /// </summary>
        /// <param name="userId">用戶 ID</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>解除封禁結果</returns>
        Task<OperationResult> UnbanUserAsync(int userId, CancellationToken cancellationToken = default);

        /// <summary>
        /// 提升用戶權限
        /// </summary>
        /// <param name="userId">用戶 ID</param>
        /// <param name="role">角色</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>提升結果</returns>
        Task<OperationResult> PromoteUserAsync(int userId, string role, CancellationToken cancellationToken = default);

        /// <summary>
        /// 降低用戶權限
        /// </summary>
        /// <param name="userId">用戶 ID</param>
        /// <param name="role">角色</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>降低結果</returns>
        Task<OperationResult> DemoteUserAsync(int userId, string role, CancellationToken cancellationToken = default);
    }

    /// <summary>
    /// 銷售報告回應
    /// </summary>
    public class SalesReportResponse
    {
        /// <summary>
        /// 報告類型
        /// </summary>
        public string ReportType { get; set; } = string.Empty;

        /// <summary>
        /// 開始日期
        /// </summary>
        public DateTime StartDate { get; set; }

        /// <summary>
        /// 結束日期
        /// </summary>
        public DateTime EndDate { get; set; }

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
        /// 活躍用戶數
        /// </summary>
        public int ActiveUsers { get; set; }

        /// <summary>
        /// 新增用戶數
        /// </summary>
        public int NewUsers { get; set; }

        /// <summary>
        /// 每日銷售數據
        /// </summary>
        public List<DailySalesData> DailySales { get; set; } = new List<DailySalesData>();

        /// <summary>
        /// 熱門商品
        /// </summary>
        public List<TopProductData> TopProducts { get; set; } = new List<TopProductData>();
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
        public decimal Sales { get; set; }

        /// <summary>
        /// 訂單數
        /// </summary>
        public int Orders { get; set; }

        /// <summary>
        /// 用戶數
        /// </summary>
        public int Users { get; set; }
    }

    /// <summary>
    /// 熱門商品數據
    /// </summary>
    public class TopProductData
    {
        /// <summary>
        /// 商品 ID
        /// </summary>
        public int ProductId { get; set; }

        /// <summary>
        /// 商品名稱
        /// </summary>
        public string ProductName { get; set; } = string.Empty;

        /// <summary>
        /// 銷售數量
        /// </summary>
        public int SalesQuantity { get; set; }

        /// <summary>
        /// 銷售金額
        /// </summary>
        public decimal SalesAmount { get; set; }
    }

    /// <summary>
    /// 系統統計回應
    /// </summary>
    public class SystemStatsResponse
    {
        /// <summary>
        /// 總用戶數
        /// </summary>
        public int TotalUsers { get; set; }

        /// <summary>
        /// 活躍用戶數
        /// </summary>
        public int ActiveUsers { get; set; }

        /// <summary>
        /// 今日新增用戶數
        /// </summary>
        public int NewUsersToday { get; set; }

        /// <summary>
        /// 總訂單數
        /// </summary>
        public int TotalOrders { get; set; }

        /// <summary>
        /// 今日訂單數
        /// </summary>
        public int OrdersToday { get; set; }

        /// <summary>
        /// 總銷售額
        /// </summary>
        public decimal TotalSales { get; set; }

        /// <summary>
        /// 今日銷售額
        /// </summary>
        public decimal SalesToday { get; set; }

        /// <summary>
        /// 系統運行時間
        /// </summary>
        public TimeSpan Uptime { get; set; }

        /// <summary>
        /// 記憶體使用量
        /// </summary>
        public long MemoryUsage { get; set; }

        /// <summary>
        /// CPU 使用率
        /// </summary>
        public double CpuUsage { get; set; }
    }

    /// <summary>
    /// 管理員用戶回應
    /// </summary>
    public class AdminUserResponse
    {
        /// <summary>
        /// 用戶 ID
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// 用戶名
        /// </summary>
        public string UserName { get; set; } = string.Empty;

        /// <summary>
        /// 電子郵件
        /// </summary>
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// 是否啟用
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// 角色列表
        /// </summary>
        public List<string> Roles { get; set; } = new List<string>();

        /// <summary>
        /// 建立時間
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// 最後登入時間
        /// </summary>
        public DateTime? LastLoginAt { get; set; }

        /// <summary>
        /// 是否被封禁
        /// </summary>
        public bool IsBanned { get; set; }

        /// <summary>
        /// 封禁原因
        /// </summary>
        public string? BanReason { get; set; }

        /// <summary>
        /// 封禁結束時間
        /// </summary>
        public DateTime? BanEndTime { get; set; }
    }

    /// <summary>
    /// 管理員用戶查詢參數
    /// </summary>
    public class AdminUserQueryParameters
    {
        /// <summary>
        /// 搜尋關鍵字
        /// </summary>
        public string? SearchTerm { get; set; }

        /// <summary>
        /// 角色
        /// </summary>
        public string? Role { get; set; }

        /// <summary>
        /// 是否啟用
        /// </summary>
        public bool? IsActive { get; set; }

        /// <summary>
        /// 是否被封禁
        /// </summary>
        public bool? IsBanned { get; set; }

        /// <summary>
        /// 建立時間開始
        /// </summary>
        public DateTime? CreatedAfter { get; set; }

        /// <summary>
        /// 建立時間結束
        /// </summary>
        public DateTime? CreatedBefore { get; set; }

        /// <summary>
        /// 頁碼
        /// </summary>
        public int PageNumber { get; set; } = 1;

        /// <summary>
        /// 每頁大小
        /// </summary>
        public int PageSize { get; set; } = 10;

        /// <summary>
        /// 排序欄位
        /// </summary>
        public string? SortBy { get; set; }

        /// <summary>
        /// 是否降序排序
        /// </summary>
        public bool SortDescending { get; set; } = false;
    }
} 