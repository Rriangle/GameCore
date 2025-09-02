using System.ComponentModel.DataAnnotations;

namespace GameCore.Application.DTOs
{
    /// <summary>
    /// 銷售統計回應
    /// </summary>
    public class SalesStatisticsResponse
    {
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
        /// 新用戶數
        /// </summary>
        public int NewUsers { get; set; }

        /// <summary>
        /// 統計期間開始
        /// </summary>
        public DateTime PeriodStart { get; set; }

        /// <summary>
        /// 統計期間結束
        /// </summary>
        public DateTime PeriodEnd { get; set; }
    }

    /// <summary>
    /// 銷售統計 DTO (舊版本相容性)
    /// </summary>
    public class SalesStatisticsDto : SalesStatisticsResponse
    {
    }

    /// <summary>
    /// 銷售檔案回應
    /// </summary>
    public class SalesProfileResponse
    {
        /// <summary>
        /// 檔案 ID
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 用戶 ID
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// 用戶名稱
        /// </summary>
        public string UserName { get; set; } = string.Empty;

        /// <summary>
        /// 總銷售額
        /// </summary>
        public decimal TotalSales { get; set; }

        /// <summary>
        /// 總訂單數
        /// </summary>
        public int TotalOrders { get; set; }

        /// <summary>
        /// 佣金率
        /// </summary>
        public decimal CommissionRate { get; set; }

        /// <summary>
        /// 總佣金
        /// </summary>
        public decimal TotalCommission { get; set; }

        /// <summary>
        /// 等級
        /// </summary>
        public string Level { get; set; } = string.Empty;

        /// <summary>
        /// 最後更新時間
        /// </summary>
        public DateTime LastUpdated { get; set; }
    }

    /// <summary>
    /// 銷售檔案 DTO (舊版本相容性)
    /// </summary>
    public class SalesProfileDto : SalesProfileResponse
    {
    }

    /// <summary>
    /// 銷售記錄回應
    /// </summary>
    public class SalesRecordResponse
    {
        /// <summary>
        /// 記錄 ID
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 用戶 ID
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// 訂單 ID
        /// </summary>
        public int OrderId { get; set; }

        /// <summary>
        /// 銷售金額
        /// </summary>
        public decimal SalesAmount { get; set; }

        /// <summary>
        /// 佣金金額
        /// </summary>
        public decimal CommissionAmount { get; set; }

        /// <summary>
        /// 銷售時間
        /// </summary>
        public DateTime SalesTime { get; set; }

        /// <summary>
        /// 狀態
        /// </summary>
        public string Status { get; set; } = string.Empty;
    }

    /// <summary>
    /// 銷售記錄 DTO (舊版本相容性)
    /// </summary>
    public class SalesRecordDto : SalesRecordResponse
    {
    }

    /// <summary>
    /// 佣金統計回應
    /// </summary>
    public class CommissionStatisticsResponse
    {
        /// <summary>
        /// 總佣金
        /// </summary>
        public decimal TotalCommission { get; set; }

        /// <summary>
        /// 已結算佣金
        /// </summary>
        public decimal SettledCommission { get; set; }

        /// <summary>
        /// 待結算佣金
        /// </summary>
        public decimal PendingCommission { get; set; }

        /// <summary>
        /// 佣金筆數
        /// </summary>
        public int CommissionCount { get; set; }

        /// <summary>
        /// 統計期間開始
        /// </summary>
        public DateTime PeriodStart { get; set; }

        /// <summary>
        /// 統計期間結束
        /// </summary>
        public DateTime PeriodEnd { get; set; }
    }

    /// <summary>
    /// 佣金提現回應
    /// </summary>
    public class CommissionWithdrawalResponse
    {
        /// <summary>
        /// 提現 ID
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 用戶 ID
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// 提現金額
        /// </summary>
        public decimal Amount { get; set; }

        /// <summary>
        /// 提現狀態
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
    /// 佣金提現 DTO (舊版本相容性)
    /// </summary>
    public class CommissionWithdrawalDto : CommissionWithdrawalResponse
    {
    }

    /// <summary>
    /// 銷售排行榜項目回應
    /// </summary>
    public class SalesLeaderboardEntryResponse
    {
        /// <summary>
        /// 排名
        /// </summary>
        public int Rank { get; set; }

        /// <summary>
        /// 用戶 ID
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// 用戶名稱
        /// </summary>
        public string UserName { get; set; } = string.Empty;

        /// <summary>
        /// 總銷售額
        /// </summary>
        public decimal TotalSales { get; set; }

        /// <summary>
        /// 總訂單數
        /// </summary>
        public int TotalOrders { get; set; }

        /// <summary>
        /// 總佣金
        /// </summary>
        public decimal TotalCommission { get; set; }
    }

    /// <summary>
    /// 銷售排行榜項目 DTO (舊版本相容性)
    /// </summary>
    public class SalesLeaderboardEntryDto : SalesLeaderboardEntryResponse
    {
    }

    /// <summary>
    /// 系統統計回應
    /// </summary>
    public class SystemStatisticsResponse
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
        /// 總訂單數
        /// </summary>
        public int TotalOrders { get; set; }

        /// <summary>
        /// 總銷售額
        /// </summary>
        public decimal TotalSales { get; set; }

        /// <summary>
        /// 系統負載
        /// </summary>
        public double SystemLoad { get; set; }

        /// <summary>
        /// 統計時間
        /// </summary>
        public DateTime StatisticsTime { get; set; }
    }

    /// <summary>
    /// 系統統計 DTO (舊版本相容性)
    /// </summary>
    public class SystemStatisticsDto : SystemStatisticsResponse
    {
    }

    /// <summary>
    /// 系統設定回應
    /// </summary>
    public class SystemSettingsResponse
    {
        /// <summary>
        /// 設定 ID
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 設定鍵
        /// </summary>
        public string Key { get; set; } = string.Empty;

        /// <summary>
        /// 設定值
        /// </summary>
        public string Value { get; set; } = string.Empty;

        /// <summary>
        /// 設定描述
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// 設定類型
        /// </summary>
        public string Type { get; set; } = string.Empty;

        /// <summary>
        /// 是否啟用
        /// </summary>
        public bool IsEnabled { get; set; }

        /// <summary>
        /// 更新時間
        /// </summary>
        public DateTime UpdatedAt { get; set; }
    }

    /// <summary>
    /// 系統設定 DTO (舊版本相容性)
    /// </summary>
    public class SystemSettingsDto : SystemSettingsResponse
    {
    }

    /// <summary>
    /// 系統日誌回應
    /// </summary>
    public class SystemLogResponse
    {
        /// <summary>
        /// 日誌 ID
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 日誌級別
        /// </summary>
        public string Level { get; set; } = string.Empty;

        /// <summary>
        /// 日誌訊息
        /// </summary>
        public string Message { get; set; } = string.Empty;

        /// <summary>
        /// 來源
        /// </summary>
        public string Source { get; set; } = string.Empty;

        /// <summary>
        /// 用戶 ID
        /// </summary>
        public int? UserId { get; set; }

        /// <summary>
        /// IP 地址
        /// </summary>
        public string? IpAddress { get; set; }

        /// <summary>
        /// 建立時間
        /// </summary>
        public DateTime CreatedAt { get; set; }
    }

    /// <summary>
    /// 系統日誌 DTO (舊版本相容性)
    /// </summary>
    public class SystemLogDto : SystemLogResponse
    {
    }

    /// <summary>
    /// 資料庫備份回應
    /// </summary>
    public class DatabaseBackupResponse
    {
        /// <summary>
        /// 備份 ID
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 備份檔案名稱
        /// </summary>
        public string FileName { get; set; } = string.Empty;

        /// <summary>
        /// 備份大小 (MB)
        /// </summary>
        public decimal SizeMB { get; set; }

        /// <summary>
        /// 備份狀態
        /// </summary>
        public string Status { get; set; } = string.Empty;

        /// <summary>
        /// 備份時間
        /// </summary>
        public DateTime BackupTime { get; set; }

        /// <summary>
        /// 備份類型
        /// </summary>
        public string BackupType { get; set; } = string.Empty;
    }

    /// <summary>
    /// 資料庫備份 DTO (舊版本相容性)
    /// </summary>
    public class DatabaseBackupDto : DatabaseBackupResponse
    {
    }

    /// <summary>
    /// 效能監控回應
    /// </summary>
    public class PerformanceMonitoringResponse
    {
        /// <summary>
        /// 監控 ID
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// CPU 使用率
        /// </summary>
        public double CpuUsage { get; set; }

        /// <summary>
        /// 記憶體使用率
        /// </summary>
        public double MemoryUsage { get; set; }

        /// <summary>
        /// 磁碟使用率
        /// </summary>
        public double DiskUsage { get; set; }

        /// <summary>
        /// 網路流量 (MB/s)
        /// </summary>
        public double NetworkTraffic { get; set; }

        /// <summary>
        /// 監控時間
        /// </summary>
        public DateTime MonitoredAt { get; set; }
    }

    /// <summary>
    /// 效能監控 DTO (舊版本相容性)
    /// </summary>
    public class PerformanceMonitoringDto : PerformanceMonitoringResponse
    {
    }

    /// <summary>
    /// 安全事件回應
    /// </summary>
    public class SecurityEventResponse
    {
        /// <summary>
        /// 事件 ID
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 事件類型
        /// </summary>
        public string EventType { get; set; } = string.Empty;

        /// <summary>
        /// 事件描述
        /// </summary>
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// 嚴重程度
        /// </summary>
        public string Severity { get; set; } = string.Empty;

        /// <summary>
        /// 用戶 ID
        /// </summary>
        public int? UserId { get; set; }

        /// <summary>
        /// IP 地址
        /// </summary>
        public string? IpAddress { get; set; }

        /// <summary>
        /// 事件時間
        /// </summary>
        public DateTime EventTime { get; set; }
    }

    /// <summary>
    /// 安全事件 DTO (舊版本相容性)
    /// </summary>
    public class SecurityEventDto : SecurityEventResponse
    {
    }

    /// <summary>
    /// 用戶管理回應
    /// </summary>
    public class UserManagementResponse
    {
        /// <summary>
        /// 用戶 ID
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 用戶名稱
        /// </summary>
        public string UserName { get; set; } = string.Empty;

        /// <summary>
        /// 電子郵件
        /// </summary>
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// 角色
        /// </summary>
        public string Role { get; set; } = string.Empty;

        /// <summary>
        /// 狀態
        /// </summary>
        public string Status { get; set; } = string.Empty;

        /// <summary>
        /// 最後登入時間
        /// </summary>
        public DateTime? LastLoginAt { get; set; }

        /// <summary>
        /// 建立時間
        /// </summary>
        public DateTime CreatedAt { get; set; }
    }

    /// <summary>
    /// 用戶管理 DTO (舊版本相容性)
    /// </summary>
    public class UserManagementDto : UserManagementResponse
    {
    }

    /// <summary>
    /// 用戶權限回應
    /// </summary>
    public class UserPermissionsResponse
    {
        /// <summary>
        /// 用戶 ID
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// 權限列表
        /// </summary>
        public List<string> Permissions { get; set; } = new List<string>();

        /// <summary>
        /// 角色列表
        /// </summary>
        public List<string> Roles { get; set; } = new List<string>();

        /// <summary>
        /// 是否為管理員
        /// </summary>
        public bool IsAdmin { get; set; }

        /// <summary>
        /// 最後更新時間
        /// </summary>
        public DateTime LastUpdated { get; set; }
    }

    /// <summary>
    /// 用戶權限 DTO (舊版本相容性)
    /// </summary>
    public class UserPermissionsDto : UserPermissionsResponse
    {
    }

    /// <summary>
    /// 評論項目回應
    /// </summary>
    public class ReviewItemResponse
    {
        /// <summary>
        /// 評論 ID
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 評論者 ID
        /// </summary>
        public int ReviewerId { get; set; }

        /// <summary>
        /// 評論者名稱
        /// </summary>
        public string ReviewerName { get; set; } = string.Empty;

        /// <summary>
        /// 評論內容
        /// </summary>
        public string Content { get; set; } = string.Empty;

        /// <summary>
        /// 評分
        /// </summary>
        public int Rating { get; set; }

        /// <summary>
        /// 評論時間
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// 狀態
        /// </summary>
        public string Status { get; set; } = string.Empty;
    }

    /// <summary>
    /// 評論項目 DTO (舊版本相容性)
    /// </summary>
    public class ReviewItemDto : ReviewItemResponse
    {
    }

    /// <summary>
    /// 銷售目標回應
    /// </summary>
    public class SalesTargetResponse
    {
        /// <summary>
        /// 目標 ID
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 用戶 ID
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// 目標金額
        /// </summary>
        public decimal TargetAmount { get; set; }

        /// <summary>
        /// 當前金額
        /// </summary>
        public decimal CurrentAmount { get; set; }

        /// <summary>
        /// 完成率
        /// </summary>
        public double CompletionRate { get; set; }

        /// <summary>
        /// 目標期間開始
        /// </summary>
        public DateTime PeriodStart { get; set; }

        /// <summary>
        /// 目標期間結束
        /// </summary>
        public DateTime PeriodEnd { get; set; }

        /// <summary>
        /// 狀態
        /// </summary>
        public string Status { get; set; } = string.Empty;
    }

    /// <summary>
    /// 銷售目標 DTO (舊版本相容性)
    /// </summary>
    public class SalesTargetDto : SalesTargetResponse
    {
    }

    /// <summary>
    /// 銷售績效報告回應
    /// </summary>
    public class SalesPerformanceReportResponse
    {
        /// <summary>
        /// 報告 ID
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 用戶 ID
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// 報告期間開始
        /// </summary>
        public DateTime PeriodStart { get; set; }

        /// <summary>
        /// 報告期間結束
        /// </summary>
        public DateTime PeriodEnd { get; set; }

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
        /// 總佣金
        /// </summary>
        public decimal TotalCommission { get; set; }

        /// <summary>
        /// 績效評級
        /// </summary>
        public string PerformanceRating { get; set; } = string.Empty;
    }

    /// <summary>
    /// 銷售績效報告 DTO (舊版本相容性)
    /// </summary>
    public class SalesPerformanceReportDto : SalesPerformanceReportResponse
    {
    }

    /// <summary>
    /// 被封鎖用戶回應
    /// </summary>
    public class BlockedUserResponse
    {
        /// <summary>
        /// 用戶 ID
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// 用戶名稱
        /// </summary>
        public string UserName { get; set; } = string.Empty;

        /// <summary>
        /// 封鎖原因
        /// </summary>
        public string BlockReason { get; set; } = string.Empty;

        /// <summary>
        /// 封鎖時間
        /// </summary>
        public DateTime BlockedAt { get; set; }

        /// <summary>
        /// 封鎖者 ID
        /// </summary>
        public int BlockedBy { get; set; }

        /// <summary>
        /// 封鎖者名稱
        /// </summary>
        public string BlockedByName { get; set; } = string.Empty;

        /// <summary>
        /// 解封時間
        /// </summary>
        public DateTime? UnblockedAt { get; set; }
    }

    /// <summary>
    /// 被封鎖用戶 DTO (舊版本相容性)
    /// </summary>
    public class BlockedUserDto : BlockedUserResponse
    {
    }

    /// <summary>
    /// 佣金統計 DTO
    /// </summary>
    public class CommissionStatisticsDto
    {
        /// <summary>
        /// 用戶 ID
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// 用戶名稱
        /// </summary>
        public string UserName { get; set; } = string.Empty;

        /// <summary>
        /// 統計期間開始
        /// </summary>
        public DateTime PeriodStart { get; set; }

        /// <summary>
        /// 統計期間結束
        /// </summary>
        public DateTime PeriodEnd { get; set; }

        /// <summary>
        /// 總佣金
        /// </summary>
        public decimal TotalCommission { get; set; }

        /// <summary>
        /// 佣金筆數
        /// </summary>
        public int CommissionCount { get; set; }

        /// <summary>
        /// 平均佣金
        /// </summary>
        public decimal AverageCommission { get; set; }

        /// <summary>
        /// 最高佣金
        /// </summary>
        public decimal MaxCommission { get; set; }

        /// <summary>
        /// 最低佣金
        /// </summary>
        public decimal MinCommission { get; set; }

        /// <summary>
        /// 佣金率
        /// </summary>
        public decimal CommissionRate { get; set; }
    }
} 