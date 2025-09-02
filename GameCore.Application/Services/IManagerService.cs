using GameCore.Application.Common;
using GameCore.Application.DTOs;

namespace GameCore.Application.Services
{
    /// <summary>
    /// 管理員服務介面
    /// </summary>
    public interface IManagerService
    {
        /// <summary>
        /// 取得用戶列表
        /// </summary>
        /// <param name="parameters">查詢參數</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>用戶列表</returns>
        Task<Result<PagedResult<AdminUserResponse>>> GetUsersAsync(AdminUserQueryParameters parameters, CancellationToken cancellationToken = default);

        /// <summary>
        /// 取得用戶詳情
        /// </summary>
        /// <param name="userId">用戶 ID</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>用戶詳情</returns>
        Task<Result<AdminUserResponse>> GetUserAsync(int userId, CancellationToken cancellationToken = default);

        /// <summary>
        /// 更新用戶資訊
        /// </summary>
        /// <param name="userId">用戶 ID</param>
        /// <param name="request">更新請求</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>更新結果</returns>
        Task<Result<AdminUserResponse>> UpdateUserAsync(int userId, UpdateAdminUserRequest request, CancellationToken cancellationToken = default);

        /// <summary>
        /// 刪除用戶
        /// </summary>
        /// <param name="userId">用戶 ID</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>刪除結果</returns>
        Task<OperationResult> DeleteUserAsync(int userId, CancellationToken cancellationToken = default);

        /// <summary>
        /// 取得系統日誌
        /// </summary>
        /// <param name="parameters">查詢參數</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>系統日誌</returns>
        Task<Result<PagedResult<SystemLogResponse>>> GetSystemLogsAsync(SystemLogQueryParameters parameters, CancellationToken cancellationToken = default);

        /// <summary>
        /// 取得系統設定
        /// </summary>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>系統設定</returns>
        Task<Result<SystemSettingsResponse>> GetSystemSettingsAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// 更新系統設定
        /// </summary>
        /// <param name="request">更新請求</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>更新結果</returns>
        Task<Result<SystemSettingsResponse>> UpdateSystemSettingsAsync(UpdateSystemSettingsRequest request, CancellationToken cancellationToken = default);
    }

    /// <summary>
    /// 更新管理員用戶請求
    /// </summary>
    public class UpdateAdminUserRequest
    {
        /// <summary>
        /// 用戶名
        /// </summary>
        public string? UserName { get; set; }

        /// <summary>
        /// 電子郵件
        /// </summary>
        public string? Email { get; set; }

        /// <summary>
        /// 是否啟用
        /// </summary>
        public bool? IsActive { get; set; }

        /// <summary>
        /// 角色列表
        /// </summary>
        public List<string>? Roles { get; set; }
    }

    /// <summary>
    /// 系統日誌回應
    /// </summary>
    public class SystemLogResponse
    {
        /// <summary>
        /// 日誌 ID
        /// </summary>
        public int LogId { get; set; }

        /// <summary>
        /// 日誌級別
        /// </summary>
        public string Level { get; set; } = string.Empty;

        /// <summary>
        /// 訊息
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
        /// 用戶名
        /// </summary>
        public string? UserName { get; set; }

        /// <summary>
        /// IP 地址
        /// </summary>
        public string? IpAddress { get; set; }

        /// <summary>
        /// 請求路徑
        /// </summary>
        public string? RequestPath { get; set; }

        /// <summary>
        /// 例外詳情
        /// </summary>
        public string? ExceptionDetails { get; set; }

        /// <summary>
        /// 建立時間
        /// </summary>
        public DateTime CreatedAt { get; set; }
    }

    /// <summary>
    /// 系統日誌查詢參數
    /// </summary>
    public class SystemLogQueryParameters
    {
        /// <summary>
        /// 日誌級別
        /// </summary>
        public string? Level { get; set; }

        /// <summary>
        /// 來源
        /// </summary>
        public string? Source { get; set; }

        /// <summary>
        /// 用戶 ID
        /// </summary>
        public int? UserId { get; set; }

        /// <summary>
        /// 搜尋關鍵字
        /// </summary>
        public string? SearchTerm { get; set; }

        /// <summary>
        /// 開始時間
        /// </summary>
        public DateTime? StartTime { get; set; }

        /// <summary>
        /// 結束時間
        /// </summary>
        public DateTime? EndTime { get; set; }

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

    /// <summary>
    /// 系統設定回應
    /// </summary>
    public class SystemSettingsResponse
    {
        /// <summary>
        /// 系統名稱
        /// </summary>
        public string SystemName { get; set; } = string.Empty;

        /// <summary>
        /// 系統版本
        /// </summary>
        public string Version { get; set; } = string.Empty;

        /// <summary>
        /// 維護模式
        /// </summary>
        public bool MaintenanceMode { get; set; }

        /// <summary>
        /// 維護訊息
        /// </summary>
        public string MaintenanceMessage { get; set; } = string.Empty;

        /// <summary>
        /// 最大用戶數
        /// </summary>
        public int MaxUsers { get; set; }

        /// <summary>
        /// 註冊功能啟用
        /// </summary>
        public bool RegistrationEnabled { get; set; }

        /// <summary>
        /// 郵件驗證啟用
        /// </summary>
        public bool EmailVerificationEnabled { get; set; }

        /// <summary>
        /// 兩步驟驗證啟用
        /// </summary>
        public bool TwoFactorAuthEnabled { get; set; }

        /// <summary>
        /// 密碼最小長度
        /// </summary>
        public int MinPasswordLength { get; set; }

        /// <summary>
        /// 密碼複雜度要求
        /// </summary>
        public bool PasswordComplexityRequired { get; set; }

        /// <summary>
        /// 會話超時時間（分鐘）
        /// </summary>
        public int SessionTimeoutMinutes { get; set; }

        /// <summary>
        /// 最大登入嘗試次數
        /// </summary>
        public int MaxLoginAttempts { get; set; }

        /// <summary>
        /// 帳戶鎖定時間（分鐘）
        /// </summary>
        public int AccountLockoutMinutes { get; set; }
    }

    /// <summary>
    /// 更新系統設定請求
    /// </summary>
    public class UpdateSystemSettingsRequest
    {
        /// <summary>
        /// 系統名稱
        /// </summary>
        public string? SystemName { get; set; }

        /// <summary>
        /// 維護模式
        /// </summary>
        public bool? MaintenanceMode { get; set; }

        /// <summary>
        /// 維護訊息
        /// </summary>
        public string? MaintenanceMessage { get; set; }

        /// <summary>
        /// 最大用戶數
        /// </summary>
        public int? MaxUsers { get; set; }

        /// <summary>
        /// 註冊功能啟用
        /// </summary>
        public bool? RegistrationEnabled { get; set; }

        /// <summary>
        /// 郵件驗證啟用
        /// </summary>
        public bool? EmailVerificationEnabled { get; set; }

        /// <summary>
        /// 兩步驟驗證啟用
        /// </summary>
        public bool? TwoFactorAuthEnabled { get; set; }

        /// <summary>
        /// 密碼最小長度
        /// </summary>
        public int? MinPasswordLength { get; set; }

        /// <summary>
        /// 密碼複雜度要求
        /// </summary>
        public bool? PasswordComplexityRequired { get; set; }

        /// <summary>
        /// 會話超時時間（分鐘）
        /// </summary>
        public int? SessionTimeoutMinutes { get; set; }

        /// <summary>
        /// 最大登入嘗試次數
        /// </summary>
        public int? MaxLoginAttempts { get; set; }

        /// <summary>
        /// 帳戶鎖定時間（分鐘）
        /// </summary>
        public int? AccountLockoutMinutes { get; set; }
    }
} 