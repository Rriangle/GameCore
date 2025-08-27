using GameCore.Core.Entities;
using GameCore.Core.Services.Enhanced;

namespace GameCore.Core.Services
{
    /// <summary>
    /// 認證服務介面
    /// </summary>
    public interface IAuthService
    {
        /// <summary>
        /// 使用者登入
        /// </summary>
        /// <param name="username">使用者名稱</param>
        /// <param name="password">密碼</param>
        /// <returns>認證結果</returns>
        Task<AuthResult> LoginAsync(string username, string password);

        /// <summary>
        /// 使用者註冊
        /// </summary>
        /// <param name="username">使用者名稱</param>
        /// <param name="email">電子郵件</param>
        /// <param name="password">密碼</param>
        /// <returns>註冊結果</returns>
        Task<AuthResult> RegisterAsync(string username, string email, string password);

        /// <summary>
        /// 登出
        /// </summary>
        /// <param name="userId">使用者ID</param>
        /// <returns>登出結果</returns>
        Task<bool> LogoutAsync(int userId);

        /// <summary>
        /// 驗證使用者
        /// </summary>
        /// <param name="userId">使用者ID</param>
        /// <returns>驗證結果</returns>
        Task<bool> ValidateUserAsync(int userId);

        /// <summary>
        /// 重設密碼
        /// </summary>
        /// <param name="email">電子郵件</param>
        /// <returns>重設結果</returns>
        Task<bool> ResetPasswordAsync(string email);

        /// <summary>
        /// 變更密碼
        /// </summary>
        /// <param name="userId">使用者ID</param>
        /// <param name="oldPassword">舊密碼</param>
        /// <param name="newPassword">新密碼</param>
        /// <returns>變更結果</returns>
        Task<bool> ChangePasswordAsync(int userId, string oldPassword, string newPassword);
    }

    /// <summary>
    /// 認證結果
    /// </summary>
    public class AuthResult
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public int? UserId { get; set; }
        public User? User { get; set; }
        public string? AccessToken { get; set; }
        public bool RequiresMFA { get; set; }
        public DateTime? LockoutEndTime { get; set; }
        public FraudRiskAssessment? FraudRisk { get; set; }
    }
} 