using GameCore.Core.Models;

namespace GameCore.Core.Interfaces
{
    /// <summary>
    /// 銷售服務接口 - 定義銷售相關的業務邏輯方法
    /// </summary>
    public interface ISalesService
    {
        /// <summary>
        /// 申請銷售權限
        /// </summary>
        /// <param name="userId">用戶ID</param>
        /// <param name="applySalesDto">申請資料</param>
        /// <returns>申請結果</returns>
        Task<ServiceResult<SalesApplicationResultDto>> ApplySalesPermissionAsync(int userId, ApplySalesDto applySalesDto);

        /// <summary>
        /// 獲取銷售權限申請狀態
        /// </summary>
        /// <param name="userId">用戶ID</param>
        /// <returns>申請狀態結果</returns>
        Task<ServiceResult<SalesApplicationStatusDto>> GetApplicationStatusAsync(int userId);

        /// <summary>
        /// 獲取銷售錢包資訊
        /// </summary>
        /// <param name="userId">用戶ID</param>
        /// <returns>銷售錢包結果</returns>
        Task<ServiceResult<SalesWalletDto>> GetSalesWalletAsync(int userId);

        /// <summary>
        /// 申請提現
        /// </summary>
        /// <param name="userId">用戶ID</param>
        /// <param name="withdrawDto">提現資料</param>
        /// <returns>提現申請結果</returns>
        Task<ServiceResult<WithdrawalResultDto>> RequestWithdrawalAsync(int userId, WithdrawDto withdrawDto);

        /// <summary>
        /// 獲取提現記錄
        /// </summary>
        /// <param name="userId">用戶ID</param>
        /// <param name="page">頁碼</param>
        /// <param name="pageSize">每頁大小</param>
        /// <param name="status">狀態篩選</param>
        /// <returns>提現記錄結果</returns>
        Task<ServiceResult<WithdrawalHistoryResultDto>> GetWithdrawalHistoryAsync(
            int userId, 
            int page = 1, 
            int pageSize = 20, 
            string? status = null);

        /// <summary>
        /// 獲取銷售統計
        /// </summary>
        /// <param name="userId">用戶ID</param>
        /// <param name="period">統計期間</param>
        /// <returns>銷售統計結果</returns>
        Task<ServiceResult<SalesStatisticsDto>> GetSalesStatisticsAsync(int userId, string period);

        /// <summary>
        /// 獲取銷售排行榜
        /// </summary>
        /// <param name="period">排行榜期間</param>
        /// <param name="top">前幾名</param>
        /// <param name="currentUserId">當前用戶ID (用於標記)</param>
        /// <returns>銷售排行榜結果</returns>
        Task<ServiceResult<SalesLeaderboardResultDto>> GetSalesLeaderboardAsync(string period, int top, int? currentUserId = null);

        /// <summary>
        /// 獲取銷售指南
        /// </summary>
        /// <returns>銷售指南結果</returns>
        Task<ServiceResult<SalesGuideDto>> GetSalesGuideAsync();

        /// <summary>
        /// 更新銀行帳戶資訊
        /// </summary>
        /// <param name="userId">用戶ID</param>
        /// <param name="updateBankDto">銀行帳戶資料</param>
        /// <returns>更新結果</returns>
        Task<ServiceResult<BankAccountResultDto>> UpdateBankAccountAsync(int userId, UpdateBankAccountDto updateBankDto);

        /// <summary>
        /// 獲取銀行帳戶資訊
        /// </summary>
        /// <param name="userId">用戶ID</param>
        /// <returns>銀行帳戶資訊結果</returns>
        Task<ServiceResult<BankAccountInfoDto>> GetBankAccountAsync(int userId);

        /// <summary>
        /// 檢查用戶是否有銷售權限
        /// </summary>
        /// <param name="userId">用戶ID</param>
        /// <returns>權限檢查結果</returns>
        Task<ServiceResult<bool>> CheckSalesPermissionAsync(int userId);

        /// <summary>
        /// 獲取銷售權限詳細資訊
        /// </summary>
        /// <param name="userId">用戶ID</param>
        /// <returns>銷售權限詳細資訊結果</returns>
        Task<ServiceResult<SalesPermissionDetailsDto>> GetSalesPermissionDetailsAsync(int userId);

        /// <summary>
        /// 獲取銷售商品列表
        /// </summary>
        /// <param name="userId">用戶ID</param>
        /// <param name="page">頁碼</param>
        /// <param name="pageSize">每頁大小</param>
        /// <param name="status">商品狀態篩選</param>
        /// <returns>銷售商品列表結果</returns>
        Task<ServiceResult<SalesProductListResultDto>> GetSalesProductsAsync(
            int userId, 
            int page = 1, 
            int pageSize = 20, 
            string? status = null);

        /// <summary>
        /// 獲取銷售訂單列表
        /// </summary>
        /// <param name="userId">用戶ID</param>
        /// <param name="page">頁碼</param>
        /// <param name="pageSize">每頁大小</param>
        /// <param name="status">訂單狀態篩選</param>
        /// <returns>銷售訂單列表結果</returns>
        Task<ServiceResult<SalesOrderListResultDto>> GetSalesOrdersAsync(
            int userId, 
            int page = 1, 
            int pageSize = 20, 
            string? status = null);

        /// <summary>
        /// 獲取銷售報表
        /// </summary>
        /// <param name="userId">用戶ID</param>
        /// <param name="startDate">開始日期</param>
        /// <param name="endDate">結束日期</param>
        /// <param name="reportType">報表類型</param>
        /// <returns>銷售報表結果</returns>
        Task<ServiceResult<SalesReportDto>> GetSalesReportAsync(
            int userId, 
            DateTime startDate, 
            DateTime endDate, 
            string reportType);

        /// <summary>
        /// 獲取銷售分析數據
        /// </summary>
        /// <param name="userId">用戶ID</param>
        /// <param name="period">分析期間</param>
        /// <returns>銷售分析結果</returns>
        Task<ServiceResult<SalesAnalyticsDto>> GetSalesAnalyticsAsync(int userId, string period);

        /// <summary>
        /// 獲取銷售目標設定
        /// </summary>
        /// <param name="userId">用戶ID</param>
        /// <returns>銷售目標設定結果</returns>
        Task<ServiceResult<SalesTargetDto>> GetSalesTargetAsync(int userId);

        /// <summary>
        /// 設定銷售目標
        /// </summary>
        /// <param name="userId">用戶ID</param>
        /// <param name="targetDto">目標設定資料</param>
        /// <returns>設定結果</returns>
        Task<ServiceResult<bool>> SetSalesTargetAsync(int userId, SetSalesTargetDto targetDto);

        /// <summary>
        /// 獲取銷售績效評估
        /// </summary>
        /// <param name="userId">用戶ID</param>
        /// <param name="period">評估期間</param>
        /// <returns>績效評估結果</returns>
        Task<ServiceResult<SalesPerformanceDto>> GetSalesPerformanceAsync(int userId, string period);
    }

    #region DTOs

    /// <summary>
    /// 申請銷售權限資料傳輸物件
    /// </summary>
    public class ApplySalesDto
    {
        /// <summary>
        /// 銀行代號
        /// </summary>
        public int BankCode { get; set; }

        /// <summary>
        /// 銀行帳號
        /// </summary>
        public string BankAccountNumber { get; set; } = string.Empty;

        /// <summary>
        /// 帳戶封面照片 (Base64 編碼)
        /// </summary>
        public string AccountCoverPhoto { get; set; } = string.Empty;

        /// <summary>
        /// 申請理由
        /// </summary>
        public string? Reason { get; set; }

        /// <summary>
        /// 預計銷售商品類型
        /// </summary>
        public string ProductType { get; set; } = string.Empty;

        /// <summary>
        /// 預計月銷售額
        /// </summary>
        public decimal? ExpectedMonthlySales { get; set; }
    }

    /// <summary>
    /// 銷售權限申請結果資料傳輸物件
    /// </summary>
    public class SalesApplicationResultDto
    {
        /// <summary>
        /// 申請ID
        /// </summary>
        public string ApplicationId { get; set; } = string.Empty;

        /// <summary>
        /// 申請狀態
        /// </summary>
        public string Status { get; set; } = string.Empty;

        /// <summary>
        /// 提交時間
        /// </summary>
        public DateTime SubmittedAt { get; set; }

        /// <summary>
        /// 預計審核時間
        /// </summary>
        public string EstimatedReviewTime { get; set; } = string.Empty;
    }

    /// <summary>
    /// 銷售權限申請狀態資料傳輸物件
    /// </summary>
    public class SalesApplicationStatusDto
    {
        /// <summary>
        /// 是否有申請
        /// </summary>
        public bool HasApplication { get; set; }

        /// <summary>
        /// 申請狀態
        /// </summary>
        public string ApplicationStatus { get; set; } = string.Empty;

        /// <summary>
        /// 提交時間
        /// </summary>
        public DateTime? SubmittedAt { get; set; }

        /// <summary>
        /// 審核時間
        /// </summary>
        public DateTime? ReviewedAt { get; set; }

        /// <summary>
        /// 審核者姓名
        /// </summary>
        public string? ReviewerName { get; set; }

        /// <summary>
        /// 審核備註
        /// </summary>
        public string? ReviewNotes { get; set; }

        /// <summary>
        /// 是否可以重新申請
        /// </summary>
        public bool CanReapply { get; set; }

        /// <summary>
        /// 重新申請日期
        /// </summary>
        public DateTime? ReapplyDate { get; set; }
    }

    /// <summary>
    /// 銷售錢包資料傳輸物件
    /// </summary>
    public class SalesWalletDto
    {
        /// <summary>
        /// 用戶ID
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// 銷售錢包餘額
        /// </summary>
        public decimal UserSalesWallet { get; set; }

        /// <summary>
        /// 總收入
        /// </summary>
        public decimal TotalEarned { get; set; }

        /// <summary>
        /// 總提現
        /// </summary>
        public decimal TotalWithdrawn { get; set; }

        /// <summary>
        /// 待處理金額
        /// </summary>
        public decimal PendingAmount { get; set; }

        /// <summary>
        /// 最後更新時間
        /// </summary>
        public DateTime? LastUpdated { get; set; }
    }

    /// <summary>
    /// 提現申請資料傳輸物件
    /// </summary>
    public class WithdrawDto
    {
        /// <summary>
        /// 提現金額
        /// </summary>
        public decimal Amount { get; set; }

        /// <summary>
        /// 提現原因
        /// </summary>
        public string? Reason { get; set; }

        /// <summary>
        /// 緊急聯絡電話
        /// </summary>
        public string? EmergencyContact { get; set; }
    }

    /// <summary>
    /// 提現申請結果資料傳輸物件
    /// </summary>
    public class WithdrawalResultDto
    {
        /// <summary>
        /// 提現ID
        /// </summary>
        public string WithdrawalId { get; set; } = string.Empty;

        /// <summary>
        /// 提現金額
        /// </summary>
        public decimal Amount { get; set; }

        /// <summary>
        /// 手續費
        /// </summary>
        public decimal Fee { get; set; }

        /// <summary>
        /// 實際到帳金額
        /// </summary>
        public decimal NetAmount { get; set; }

        /// <summary>
        /// 狀態
        /// </summary>
        public string Status { get; set; } = string.Empty;

        /// <summary>
        /// 預計處理時間
        /// </summary>
        public string EstimatedProcessingTime { get; set; } = string.Empty;
    }

    /// <summary>
    /// 提現記錄結果資料傳輸物件
    /// </summary>
    public class WithdrawalHistoryResultDto
    {
        /// <summary>
        /// 提現記錄列表
        /// </summary>
        public List<WithdrawalRecordDto> Withdrawals { get; set; } = new List<WithdrawalRecordDto>();

        /// <summary>
        /// 分頁資訊
        /// </summary>
        public PaginationDto Pagination { get; set; } = new PaginationDto();
    }

    /// <summary>
    /// 提現記錄資料傳輸物件
    /// </summary>
    public class WithdrawalRecordDto
    {
        /// <summary>
        /// 記錄ID
        /// </summary>
        public string Id { get; set; } = string.Empty;

        /// <summary>
        /// 提現金額
        /// </summary>
        public decimal Amount { get; set; }

        /// <summary>
        /// 手續費
        /// </summary>
        public decimal Fee { get; set; }

        /// <summary>
        /// 實際到帳金額
        /// </summary>
        public decimal NetAmount { get; set; }

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

        /// <summary>
        /// 備註
        /// </summary>
        public string? Notes { get; set; }
    }

    /// <summary>
    /// 銷售統計資料傳輸物件
    /// </summary>
    public class SalesStatisticsDto
    {
        /// <summary>
        /// 統計期間
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
        /// 熱門商品
        /// </summary>
        public List<TopProductDto> TopProducts { get; set; } = new List<TopProductDto>();

        /// <summary>
        /// 銷售趨勢
        /// </summary>
        public List<SalesTrendDto> SalesTrend { get; set; } = new List<SalesTrendDto>();
    }

    /// <summary>
    /// 熱門商品資料傳輸物件
    /// </summary>
    public class TopProductDto
    {
        /// <summary>
        /// 商品ID
        /// </summary>
        public int ProductId { get; set; }

        /// <summary>
        /// 商品名稱
        /// </summary>
        public string ProductName { get; set; } = string.Empty;

        /// <summary>
        /// 銷售數量
        /// </summary>
        public int SalesCount { get; set; }

        /// <summary>
        /// 總收入
        /// </summary>
        public decimal TotalRevenue { get; set; }
    }

    /// <summary>
    /// 銷售趨勢資料傳輸物件
    /// </summary>
    public class SalesTrendDto
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
    }

    /// <summary>
    /// 銷售排行榜結果資料傳輸物件
    /// </summary>
    public class SalesLeaderboardResultDto
    {
        /// <summary>
        /// 排行榜列表
        /// </summary>
        public List<SalesLeaderboardEntryDto> Rankings { get; set; } = new List<SalesLeaderboardEntryDto>();
    }

    /// <summary>
    /// 銷售排行榜項目資料傳輸物件
    /// </summary>
    public class SalesLeaderboardEntryDto
    {
        /// <summary>
        /// 用戶ID
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// 用戶姓名
        /// </summary>
        public string UserName { get; set; } = string.Empty;

        /// <summary>
        /// 用戶頭像
        /// </summary>
        public string? UserAvatar { get; set; }

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
        /// 是否為當前用戶
        /// </summary>
        public bool IsCurrentUser { get; set; }
    }

    /// <summary>
    /// 銷售指南資料傳輸物件
    /// </summary>
    public class SalesGuideDto
    {
        /// <summary>
        /// 指南章節
        /// </summary>
        public List<GuideSectionDto> Sections { get; set; } = new List<GuideSectionDto>();

        /// <summary>
        /// 銷售技巧
        /// </summary>
        public List<SalesTipDto> Tips { get; set; } = new List<SalesTipDto>();

        /// <summary>
        /// 常見問題
        /// </summary>
        public List<SalesFaqDto> Faq { get; set; } = new List<SalesFaqDto>();
    }

    /// <summary>
    /// 指南章節資料傳輸物件
    /// </summary>
    public class GuideSectionDto
    {
        /// <summary>
        /// 章節ID
        /// </summary>
        public string Id { get; set; } = string.Empty;

        /// <summary>
        /// 章節標題
        /// </summary>
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// 章節內容
        /// </summary>
        public string Content { get; set; } = string.Empty;

        /// <summary>
        /// 排序
        /// </summary>
        public int Order { get; set; }

        /// <summary>
        /// 是否必讀
        /// </summary>
        public bool IsRequired { get; set; }
    }

    /// <summary>
    /// 銷售技巧資料傳輸物件
    /// </summary>
    public class SalesTipDto
    {
        /// <summary>
        /// 技巧ID
        /// </summary>
        public string Id { get; set; } = string.Empty;

        /// <summary>
        /// 技巧類別
        /// </summary>
        public string Category { get; set; } = string.Empty;

        /// <summary>
        /// 技巧標題
        /// </summary>
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// 技巧描述
        /// </summary>
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// 圖示
        /// </summary>
        public string Icon { get; set; } = string.Empty;
    }

    /// <summary>
    /// 銷售常見問題資料傳輸物件
    /// </summary>
    public class SalesFaqDto
    {
        /// <summary>
        /// 問題ID
        /// </summary>
        public string Id { get; set; } = string.Empty;

        /// <summary>
        /// 問題
        /// </summary>
        public string Question { get; set; } = string.Empty;

        /// <summary>
        /// 答案
        /// </summary>
        public string Answer { get; set; } = string.Empty;

        /// <summary>
        /// 類別
        /// </summary>
        public string Category { get; set; } = string.Empty;
    }

    /// <summary>
    /// 更新銀行帳戶資料傳輸物件
    /// </summary>
    public class UpdateBankAccountDto
    {
        /// <summary>
        /// 銀行代號
        /// </summary>
        public int BankCode { get; set; }

        /// <summary>
        /// 銀行帳號
        /// </summary>
        public string BankAccountNumber { get; set; } = string.Empty;

        /// <summary>
        /// 帳戶封面照片 (Base64 編碼)
        /// </summary>
        public string AccountCoverPhoto { get; set; } = string.Empty;

        /// <summary>
        /// 帳戶持有人姓名
        /// </summary>
        public string AccountHolderName { get; set; } = string.Empty;
    }

    /// <summary>
    /// 銀行帳戶更新結果資料傳輸物件
    /// </summary>
    public class BankAccountResultDto
    {
        /// <summary>
        /// 銀行代號
        /// </summary>
        public int BankCode { get; set; }

        /// <summary>
        /// 銀行名稱
        /// </summary>
        public string BankName { get; set; } = string.Empty;

        /// <summary>
        /// 帳號
        /// </summary>
        public string AccountNumber { get; set; } = string.Empty;

        /// <summary>
        /// 更新時間
        /// </summary>
        public DateTime UpdatedAt { get; set; }
    }

    /// <summary>
    /// 銀行帳戶資訊資料傳輸物件
    /// </summary>
    public class BankAccountInfoDto
    {
        /// <summary>
        /// 銀行代號
        /// </summary>
        public int BankCode { get; set; }

        /// <summary>
        /// 銀行名稱
        /// </summary>
        public string BankName { get; set; } = string.Empty;

        /// <summary>
        /// 帳號
        /// </summary>
        public string AccountNumber { get; set; } = string.Empty;

        /// <summary>
        /// 帳戶持有人姓名
        /// </summary>
        public string AccountHolderName { get; set; } = string.Empty;

        /// <summary>
        /// 最後更新時間
        /// </summary>
        public DateTime? LastUpdated { get; set; }
    }

    /// <summary>
    /// 銷售權限詳細資訊資料傳輸物件
    /// </summary>
    public class SalesPermissionDetailsDto
    {
        /// <summary>
        /// 是否有銷售權限
        /// </summary>
        public bool HasPermission { get; set; }

        /// <summary>
        /// 權限等級
        /// </summary>
        public string PermissionLevel { get; set; } = string.Empty;

        /// <summary>
        /// 權限授予時間
        /// </summary>
        public DateTime? GrantedAt { get; set; }

        /// <summary>
        /// 權限到期時間
        /// </summary>
        public DateTime? ExpiresAt { get; set; }

        /// <summary>
        /// 權限限制
        /// </summary>
        public List<string> Restrictions { get; set; } = new List<string>();

        /// <summary>
        /// 權限說明
        /// </summary>
        public string Description { get; set; } = string.Empty;
    }

    /// <summary>
    /// 銷售商品列表結果資料傳輸物件
    /// </summary>
    public class SalesProductListResultDto
    {
        /// <summary>
        /// 商品列表
        /// </summary>
        public List<SalesProductDto> Products { get; set; } = new List<SalesProductDto>();

        /// <summary>
        /// 分頁資訊
        /// </summary>
        public PaginationDto Pagination { get; set; } = new PaginationDto();
    }

    /// <summary>
    /// 銷售商品資料傳輸物件
    /// </summary>
    public class SalesProductDto
    {
        /// <summary>
        /// 商品ID
        /// </summary>
        public int ProductId { get; set; }

        /// <summary>
        /// 商品名稱
        /// </summary>
        public string ProductName { get; set; } = string.Empty;

        /// <summary>
        /// 商品狀態
        /// </summary>
        public string Status { get; set; } = string.Empty;

        /// <summary>
        /// 價格
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// 庫存
        /// </summary>
        public int Stock { get; set; }

        /// <summary>
        /// 銷售數量
        /// </summary>
        public int SalesCount { get; set; }

        /// <summary>
        /// 上架時間
        /// </summary>
        public DateTime ListedAt { get; set; }
    }

    /// <summary>
    /// 銷售訂單列表結果資料傳輸物件
    /// </summary>
    public class SalesOrderListResultDto
    {
        /// <summary>
        /// 訂單列表
        /// </summary>
        public List<SalesOrderDto> Orders { get; set; } = new List<SalesOrderDto>();

        /// <summary>
        /// 分頁資訊
        /// </summary>
        public PaginationDto Pagination { get; set; } = new PaginationDto();
    }

    /// <summary>
    /// 銷售訂單資料傳輸物件
    /// </summary>
    public class SalesOrderDto
    {
        /// <summary>
        /// 訂單ID
        /// </summary>
        public string OrderId { get; set; } = string.Empty;

        /// <summary>
        /// 買家ID
        /// </summary>
        public int BuyerId { get; set; }

        /// <summary>
        /// 買家姓名
        /// </summary>
        public string BuyerName { get; set; } = string.Empty;

        /// <summary>
        /// 訂單金額
        /// </summary>
        public decimal Amount { get; set; }

        /// <summary>
        /// 訂單狀態
        /// </summary>
        public string Status { get; set; } = string.Empty;

        /// <summary>
        /// 建立時間
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// 完成時間
        /// </summary>
        public DateTime? CompletedAt { get; set; }
    }

    /// <summary>
    /// 銷售報表資料傳輸物件
    /// </summary>
    public class SalesReportDto
    {
        /// <summary>
        /// 報表類型
        /// </summary>
        public string ReportType { get; set; } = string.Empty;

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
        /// 商品銷售排行
        /// </summary>
        public List<ProductSalesRankDto> ProductRanks { get; set; } = new List<ProductSalesRankDto>();

        /// <summary>
        /// 每日銷售數據
        /// </summary>
        public List<DailySalesDataDto> DailyData { get; set; } = new List<DailySalesDataDto>();
    }

    /// <summary>
    /// 商品銷售排行資料傳輸物件
    /// </summary>
    public class ProductSalesRankDto
    {
        /// <summary>
        /// 排名
        /// </summary>
        public int Rank { get; set; }

        /// <summary>
        /// 商品ID
        /// </summary>
        public int ProductId { get; set; }

        /// <summary>
        /// 商品名稱
        /// </summary>
        public string ProductName { get; set; } = string.Empty;

        /// <summary>
        /// 銷售數量
        /// </summary>
        public int SalesCount { get; set; }

        /// <summary>
        /// 銷售金額
        /// </summary>
        public decimal SalesAmount { get; set; }
    }

    /// <summary>
    /// 每日銷售數據資料傳輸物件
    /// </summary>
    public class DailySalesDataDto
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
        /// 商品數
        /// </summary>
        public int Products { get; set; }
    }

    /// <summary>
    /// 銷售分析資料傳輸物件
    /// </summary>
    public class SalesAnalyticsDto
    {
        /// <summary>
        /// 分析期間
        /// </summary>
        public string Period { get; set; } = string.Empty;

        /// <summary>
        /// 銷售成長率
        /// </summary>
        public decimal GrowthRate { get; set; }

        /// <summary>
        /// 客戶回購率
        /// </summary>
        public decimal RepeatCustomerRate { get; set; }

        /// <summary>
        /// 平均客戶價值
        /// </summary>
        public decimal AverageCustomerValue { get; set; }

        /// <summary>
        /// 熱門時段
        /// </summary>
        public List<PeakHourDto> PeakHours { get; set; } = new List<PeakHourDto>();

        /// <summary>
        /// 地區分布
        /// </summary>
        public List<RegionalSalesDto> RegionalSales { get; set; } = new List<RegionalSalesDto>();
    }

    /// <summary>
    /// 熱門時段資料傳輸物件
    /// </summary>
    public class PeakHourDto
    {
        /// <summary>
        /// 時段
        /// </summary>
        public string Hour { get; set; } = string.Empty;

        /// <summary>
        /// 銷售額
        /// </summary>
        public decimal Sales { get; set; }

        /// <summary>
        /// 訂單數
        /// </summary>
        public int Orders { get; set; }
    }

    /// <summary>
    /// 地區銷售資料傳輸物件
    /// </summary>
    public class RegionalSalesDto
    {
        /// <summary>
        /// 地區
        /// </summary>
        public string Region { get; set; } = string.Empty;

        /// <summary>
        /// 銷售額
        /// </summary>
        public decimal Sales { get; set; }

        /// <summary>
        /// 訂單數
        /// </summary>
        public int Orders { get; set; }

        /// <summary>
        /// 佔比
        /// </summary>
        public decimal Percentage { get; set; }
    }

    /// <summary>
    /// 銷售目標資料傳輸物件
    /// </summary>
    public class SalesTargetDto
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

        /// <summary>
        /// 當前進度
        /// </summary>
        public decimal CurrentProgress { get; set; }

        /// <summary>
        /// 達成率
        /// </summary>
        public decimal AchievementRate { get; set; }

        /// <summary>
        /// 目標設定時間
        /// </summary>
        public DateTime SetAt { get; set; }
    }

    /// <summary>
    /// 設定銷售目標資料傳輸物件
    /// </summary>
    public class SetSalesTargetDto
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

        /// <summary>
        /// 備註
        /// </summary>
        public string? Notes { get; set; }
    }

    /// <summary>
    /// 銷售績效資料傳輸物件
    /// </summary>
    public class SalesPerformanceDto
    {
        /// <summary>
        /// 績效期間
        /// </summary>
        public string Period { get; set; } = string.Empty;

        /// <summary>
        /// 績效分數
        /// </summary>
        public decimal Score { get; set; }

        /// <summary>
        /// 績效等級
        /// </summary>
        public string Grade { get; set; } = string.Empty;

        /// <summary>
        /// 績效評語
        /// </summary>
        public string Comment { get; set; } = string.Empty;

        /// <summary>
        /// 改進建議
        /// </summary>
        public List<string> Suggestions { get; set; } = new List<string>();

        /// <summary>
        /// 評估時間
        /// </summary>
        public DateTime EvaluatedAt { get; set; }
    }

    #endregion
}