using GameCore.Core.Models;

namespace GameCore.Core.Interfaces
{
    /// <summary>
    /// 錢包服務接口 - 定義錢包相關的業務邏輯方法
    /// </summary>
    public interface IWalletService
    {
        /// <summary>
        /// 獲取用戶錢包餘額
        /// </summary>
        /// <param name="userId">用戶ID</param>
        /// <returns>錢包餘額結果</returns>
        Task<ServiceResult<WalletBalanceDto>> GetBalanceAsync(int userId);

        /// <summary>
        /// 獲取點數流水記錄
        /// </summary>
        /// <param name="userId">用戶ID</param>
        /// <param name="page">頁碼</param>
        /// <param name="pageSize">每頁大小</param>
        /// <param name="type">流水類型篩選</param>
        /// <param name="startDate">開始日期</param>
        /// <param name="endDate">結束日期</param>
        /// <returns>點數流水記錄結果</returns>
        Task<ServiceResult<LedgerResultDto>> GetLedgerAsync(
            int userId, 
            int page = 1, 
            int pageSize = 20, 
            string? type = null, 
            DateTime? startDate = null, 
            DateTime? endDate = null);

        /// <summary>
        /// 獲取點數統計資訊
        /// </summary>
        /// <param name="userId">用戶ID</param>
        /// <param name="period">統計期間</param>
        /// <returns>點數統計結果</returns>
        Task<ServiceResult<PointsStatisticsDto>> GetStatisticsAsync(int userId, string period);

        /// <summary>
        /// 獲取優惠券資訊
        /// </summary>
        /// <param name="userId">用戶ID</param>
        /// <returns>優惠券列表結果</returns>
        Task<ServiceResult<List<CouponDto>>> GetCouponsAsync(int userId);

        /// <summary>
        /// 使用優惠券
        /// </summary>
        /// <param name="userId">用戶ID</param>
        /// <param name="couponCode">優惠券代碼</param>
        /// <param name="orderAmount">訂單金額</param>
        /// <returns>使用優惠券結果</returns>
        Task<ServiceResult<CouponUsageResultDto>> UseCouponAsync(int userId, string couponCode, decimal orderAmount);

        /// <summary>
        /// 獲取點數排行榜
        /// </summary>
        /// <param name="period">排行榜期間</param>
        /// <param name="top">前幾名</param>
        /// <param name="currentUserId">當前用戶ID (用於標記)</param>
        /// <returns>點數排行榜結果</returns>
        Task<ServiceResult<LeaderboardResultDto>> GetLeaderboardAsync(string period, int top, int? currentUserId = null);

        /// <summary>
        /// 獲取點數獲得方式說明
        /// </summary>
        /// <returns>點數獲得方式列表結果</returns>
        Task<ServiceResult<List<EarningMethodDto>>> GetEarningMethodsAsync();

        /// <summary>
        /// 獲取點數消費記錄
        /// </summary>
        /// <param name="userId">用戶ID</param>
        /// <param name="page">頁碼</param>
        /// <param name="pageSize">每頁大小</param>
        /// <param name="category">消費類別</param>
        /// <returns>消費記錄結果</returns>
        Task<ServiceResult<SpendingHistoryResultDto>> GetSpendingHistoryAsync(
            int userId, 
            int page = 1, 
            int pageSize = 20, 
            string? category = null);

        /// <summary>
        /// 獲取點數預測
        /// </summary>
        /// <param name="userId">用戶ID</param>
        /// <param name="days">預測天數</param>
        /// <returns>點數預測結果</returns>
        Task<ServiceResult<PointsForecastDto>> GetPointsForecastAsync(int userId, int days);

        /// <summary>
        /// 調整用戶點數 (管理員功能)
        /// </summary>
        /// <param name="userId">用戶ID</param>
        /// <param name="delta">點數變化量 (正數為增加，負數為減少)</param>
        /// <param name="reason">調整原因</param>
        /// <param name="adminId">管理員ID</param>
        /// <returns>調整結果</returns>
        Task<ServiceResult<PointsAdjustmentResultDto>> AdjustPointsAsync(int userId, int delta, string reason, int adminId);

        /// <summary>
        /// 獲取點數變化歷史
        /// </summary>
        /// <param name="userId">用戶ID</param>
        /// <param name="page">頁碼</param>
        /// <param name="pageSize">每頁大小</param>
        /// <returns>點數變化歷史結果</returns>
        Task<ServiceResult<PointsHistoryResultDto>> GetPointsHistoryAsync(int userId, int page = 1, int pageSize = 20);

        /// <summary>
        /// 檢查點數餘額是否足夠
        /// </summary>
        /// <param name="userId">用戶ID</param>
        /// <param name="requiredAmount">所需點數</param>
        /// <returns>餘額檢查結果</returns>
        Task<ServiceResult<bool>> CheckBalanceAsync(int userId, int requiredAmount);

        /// <summary>
        /// 凍結點數 (用於交易預留)
        /// </summary>
        /// <param name="userId">用戶ID</param>
        /// <param name="amount">凍結金額</param>
        /// <param name="transactionId">交易ID</param>
        /// <returns>凍結結果</returns>
        Task<ServiceResult<bool>> FreezePointsAsync(int userId, int amount, string transactionId);

        /// <summary>
        /// 解凍點數
        /// </summary>
        /// <param name="userId">用戶ID</param>
        /// <param name="transactionId">交易ID</param>
        /// <returns>解凍結果</returns>
        Task<ServiceResult<bool>> UnfreezePointsAsync(int userId, string transactionId);

        /// <summary>
        /// 獲取凍結點數資訊
        /// </summary>
        /// <param name="userId">用戶ID</param>
        /// <returns>凍結點數資訊結果</returns>
        Task<ServiceResult<FrozenPointsDto>> GetFrozenPointsAsync(int userId);
    }

    #region DTOs

    /// <summary>
    /// 錢包餘額資料傳輸物件
    /// </summary>
    public class WalletBalanceDto
    {
        /// <summary>
        /// 用戶ID
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// 用戶點數
        /// </summary>
        public int UserPoint { get; set; }

        /// <summary>
        /// 優惠券編號
        /// </summary>
        public string? CouponNumber { get; set; }

        /// <summary>
        /// 最後更新時間
        /// </summary>
        public DateTime? LastUpdated { get; set; }

        /// <summary>
        /// 可用點數 (扣除凍結點數)
        /// </summary>
        public int AvailablePoints { get; set; }

        /// <summary>
        /// 凍結點數
        /// </summary>
        public int FrozenPoints { get; set; }
    }

    /// <summary>
    /// 點數流水記錄結果資料傳輸物件
    /// </summary>
    public class LedgerResultDto
    {
        /// <summary>
        /// 流水記錄列表
        /// </summary>
        public List<LedgerRecordDto> Records { get; set; } = new List<LedgerRecordDto>();

        /// <summary>
        /// 分頁資訊
        /// </summary>
        public PaginationDto Pagination { get; set; } = new PaginationDto();
    }

    /// <summary>
    /// 點數流水記錄資料傳輸物件
    /// </summary>
    public class LedgerRecordDto
    {
        /// <summary>
        /// 記錄ID
        /// </summary>
        public string Id { get; set; } = string.Empty;

        /// <summary>
        /// 流水類型
        /// </summary>
        public string Type { get; set; } = string.Empty;

        /// <summary>
        /// 點數變化量
        /// </summary>
        public int Delta { get; set; }

        /// <summary>
        /// 變化後餘額
        /// </summary>
        public int BalanceAfter { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// 時間戳
        /// </summary>
        public DateTime Timestamp { get; set; }

        /// <summary>
        /// 元資料 (JSON 格式)
        /// </summary>
        public string? Metadata { get; set; }
    }

    /// <summary>
    /// 點數統計資料傳輸物件
    /// </summary>
    public class PointsStatisticsDto
    {
        /// <summary>
        /// 統計期間
        /// </summary>
        public string Period { get; set; } = string.Empty;

        /// <summary>
        /// 總獲得點數
        /// </summary>
        public int TotalEarned { get; set; }

        /// <summary>
        /// 總消費點數
        /// </summary>
        public int TotalSpent { get; set; }

        /// <summary>
        /// 淨變化
        /// </summary>
        public int NetChange { get; set; }

        /// <summary>
        /// 交易次數
        /// </summary>
        public int TransactionCount { get; set; }

        /// <summary>
        /// 每日平均
        /// </summary>
        public decimal AveragePerDay { get; set; }

        /// <summary>
        /// 主要來源
        /// </summary>
        public List<PointsSourceDto> TopSources { get; set; } = new List<PointsSourceDto>();
    }

    /// <summary>
    /// 點數來源資料傳輸物件
    /// </summary>
    public class PointsSourceDto
    {
        /// <summary>
        /// 來源類型
        /// </summary>
        public string Type { get; set; } = string.Empty;

        /// <summary>
        /// 點數金額
        /// </summary>
        public int Amount { get; set; }

        /// <summary>
        /// 次數
        /// </summary>
        public int Count { get; set; }
    }

    /// <summary>
    /// 優惠券資料傳輸物件
    /// </summary>
    public class CouponDto
    {
        /// <summary>
        /// 優惠券ID
        /// </summary>
        public string Id { get; set; } = string.Empty;

        /// <summary>
        /// 優惠券代碼
        /// </summary>
        public string Code { get; set; } = string.Empty;

        /// <summary>
        /// 優惠券名稱
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// 折扣類型
        /// </summary>
        public string DiscountType { get; set; } = string.Empty;

        /// <summary>
        /// 折扣值
        /// </summary>
        public decimal DiscountValue { get; set; }

        /// <summary>
        /// 最低消費金額
        /// </summary>
        public decimal MinimumAmount { get; set; }

        /// <summary>
        /// 有效開始時間
        /// </summary>
        public DateTime ValidFrom { get; set; }

        /// <summary>
        /// 有效結束時間
        /// </summary>
        public DateTime ValidTo { get; set; }

        /// <summary>
        /// 是否已使用
        /// </summary>
        public bool IsUsed { get; set; }

        /// <summary>
        /// 使用時間
        /// </summary>
        public DateTime? UsedAt { get; set; }
    }

    /// <summary>
    /// 優惠券使用結果資料傳輸物件
    /// </summary>
    public class CouponUsageResultDto
    {
        /// <summary>
        /// 折扣金額
        /// </summary>
        public decimal DiscountAmount { get; set; }

        /// <summary>
        /// 最終金額
        /// </summary>
        public decimal FinalAmount { get; set; }

        /// <summary>
        /// 優惠券代碼
        /// </summary>
        public string CouponCode { get; set; } = string.Empty;
    }

    /// <summary>
    /// 排行榜結果資料傳輸物件
    /// </summary>
    public class LeaderboardResultDto
    {
        /// <summary>
        /// 排行榜列表
        /// </summary>
        public List<LeaderboardEntryDto> Rankings { get; set; } = new List<LeaderboardEntryDto>();
    }

    /// <summary>
    /// 排行榜項目資料傳輸物件
    /// </summary>
    public class LeaderboardEntryDto
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
        /// 點數
        /// </summary>
        public int Points { get; set; }

        /// <summary>
        /// 變化量
        /// </summary>
        public int Change { get; set; }

        /// <summary>
        /// 是否為當前用戶
        /// </summary>
        public bool IsCurrentUser { get; set; }
    }

    /// <summary>
    /// 點數獲得方式資料傳輸物件
    /// </summary>
    public class EarningMethodDto
    {
        /// <summary>
        /// 方式ID
        /// </summary>
        public string Id { get; set; } = string.Empty;

        /// <summary>
        /// 方式名稱
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// 點數範圍
        /// </summary>
        public string PointsRange { get; set; } = string.Empty;

        /// <summary>
        /// 頻率
        /// </summary>
        public string Frequency { get; set; } = string.Empty;

        /// <summary>
        /// 要求
        /// </summary>
        public string Requirements { get; set; } = string.Empty;

        /// <summary>
        /// 圖示
        /// </summary>
        public string Icon { get; set; } = string.Empty;

        /// <summary>
        /// 是否啟用
        /// </summary>
        public bool IsActive { get; set; }
    }

    /// <summary>
    /// 消費記錄結果資料傳輸物件
    /// </summary>
    public class SpendingHistoryResultDto
    {
        /// <summary>
        /// 消費記錄列表
        /// </summary>
        public List<SpendingRecordDto> Records { get; set; } = new List<SpendingRecordDto>();

        /// <summary>
        /// 分頁資訊
        /// </summary>
        public PaginationDto Pagination { get; set; } = new PaginationDto();
    }

    /// <summary>
    /// 消費記錄資料傳輸物件
    /// </summary>
    public class SpendingRecordDto
    {
        /// <summary>
        /// 記錄ID
        /// </summary>
        public string Id { get; set; } = string.Empty;

        /// <summary>
        /// 消費類別
        /// </summary>
        public string Category { get; set; } = string.Empty;

        /// <summary>
        /// 金額
        /// </summary>
        public int Amount { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// 時間戳
        /// </summary>
        public DateTime Timestamp { get; set; }

        /// <summary>
        /// 相關項目
        /// </summary>
        public string? RelatedItem { get; set; }
    }

    /// <summary>
    /// 點數預測資料傳輸物件
    /// </summary>
    public class PointsForecastDto
    {
        /// <summary>
        /// 當前點數
        /// </summary>
        public int CurrentPoints { get; set; }

        /// <summary>
        /// 預測點數
        /// </summary>
        public int PredictedPoints { get; set; }

        /// <summary>
        /// 每日平均
        /// </summary>
        public decimal DailyAverage { get; set; }

        /// <summary>
        /// 每週趨勢
        /// </summary>
        public decimal WeeklyTrend { get; set; }

        /// <summary>
        /// 每月預測
        /// </summary>
        public int MonthlyProjection { get; set; }

        /// <summary>
        /// 影響因素
        /// </summary>
        public List<ForecastFactorDto> Factors { get; set; } = new List<ForecastFactorDto>();
    }

    /// <summary>
    /// 預測因素資料傳輸物件
    /// </summary>
    public class ForecastFactorDto
    {
        /// <summary>
        /// 因素類型
        /// </summary>
        public string Type { get; set; } = string.Empty;

        /// <summary>
        /// 影響程度
        /// </summary>
        public decimal Impact { get; set; }

        /// <summary>
        /// 信心度
        /// </summary>
        public decimal Confidence { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; } = string.Empty;
    }

    /// <summary>
    /// 點數調整結果資料傳輸物件
    /// </summary>
    public class PointsAdjustmentResultDto
    {
        /// <summary>
        /// 調整前餘額
        /// </summary>
        public int BalanceBefore { get; set; }

        /// <summary>
        /// 調整後餘額
        /// </summary>
        public int BalanceAfter { get; set; }

        /// <summary>
        /// 調整量
        /// </summary>
        public int Delta { get; set; }

        /// <summary>
        /// 調整時間
        /// </summary>
        public DateTime AdjustedAt { get; set; }

        /// <summary>
        /// 調整原因
        /// </summary>
        public string Reason { get; set; } = string.Empty;

        /// <summary>
        /// 管理員ID
        /// </summary>
        public int AdminId { get; set; }
    }

    /// <summary>
    /// 點數歷史結果資料傳輸物件
    /// </summary>
    public class PointsHistoryResultDto
    {
        /// <summary>
        /// 點數歷史列表
        /// </summary>
        public List<PointsHistoryEntryDto> History { get; set; } = new List<PointsHistoryEntryDto>();

        /// <summary>
        /// 分頁資訊
        /// </summary>
        public PaginationDto Pagination { get; set; } = new PaginationDto();
    }

    /// <summary>
    /// 點數歷史項目資料傳輸物件
    /// </summary>
    public class PointsHistoryEntryDto
    {
        /// <summary>
        /// 歷史ID
        /// </summary>
        public string Id { get; set; } = string.Empty;

        /// <summary>
        /// 變化前餘額
        /// </summary>
        public int BalanceBefore { get; set; }

        /// <summary>
        /// 變化量
        /// </summary>
        public int Delta { get; set; }

        /// <summary>
        /// 變化後餘額
        /// </summary>
        public int BalanceAfter { get; set; }

        /// <summary>
        /// 變化類型
        /// </summary>
        public string Type { get; set; } = string.Empty;

        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// 時間戳
        /// </summary>
        public DateTime Timestamp { get; set; }

        /// <summary>
        /// 相關交易ID
        /// </summary>
        public string? TransactionId { get; set; }
    }

    /// <summary>
    /// 凍結點數資料傳輸物件
    /// </summary>
    public class FrozenPointsDto
    {
        /// <summary>
        /// 用戶ID
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// 凍結點數總額
        /// </summary>
        public int TotalFrozen { get; set; }

        /// <summary>
        /// 凍結記錄列表
        /// </summary>
        public List<FrozenPointsRecordDto> Records { get; set; } = new List<FrozenPointsRecordDto>();
    }

    /// <summary>
    /// 凍結點數記錄資料傳輸物件
    /// </summary>
    public class FrozenPointsRecordDto
    {
        /// <summary>
        /// 記錄ID
        /// </summary>
        public string Id { get; set; } = string.Empty;

        /// <summary>
        /// 凍結金額
        /// </summary>
        public int Amount { get; set; }

        /// <summary>
        /// 交易ID
        /// </summary>
        public string TransactionId { get; set; } = string.Empty;

        /// <summary>
        /// 凍結時間
        /// </summary>
        public DateTime FrozenAt { get; set; }

        /// <summary>
        /// 預計解凍時間
        /// </summary>
        public DateTime? ExpectedUnfreezeAt { get; set; }

        /// <summary>
        /// 狀態
        /// </summary>
        public string Status { get; set; } = string.Empty;
    }

    /// <summary>
    /// 分頁資料傳輸物件
    /// </summary>
    public class PaginationDto
    {
        /// <summary>
        /// 當前頁碼
        /// </summary>
        public int Page { get; set; }

        /// <summary>
        /// 每頁大小
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// 總記錄數
        /// </summary>
        public int TotalCount { get; set; }

        /// <summary>
        /// 總頁數
        /// </summary>
        public int TotalPages { get; set; }
    }

    #endregion
}