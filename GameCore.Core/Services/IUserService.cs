using GameCore.Core.Entities;

namespace GameCore.Core.Services
{
    /// <summary>
    /// 使用者服務介面
    /// </summary>
    public interface IUserService
    {
        /// <summary>
        /// 使用者註冊
        /// </summary>
        /// <param name="userRegistration">註冊資訊</param>
        /// <returns>註冊結果</returns>
        Task<UserRegistrationResult> RegisterAsync(UserRegistration userRegistration);

        /// <summary>
        /// 使用者登入
        /// </summary>
        /// <param name="loginRequest">登入請求</param>
        /// <returns>登入結果</returns>
        Task<UserLoginResult> LoginAsync(UserLoginRequest loginRequest);

        /// <summary>
        /// 使用者登出
        /// </summary>
        /// <param name="userId">使用者ID</param>
        /// <returns>登出結果</returns>
        Task<bool> LogoutAsync(int userId);

        /// <summary>
        /// 取得使用者資料
        /// </summary>
        /// <param name="userId">使用者ID</param>
        /// <returns>使用者資料</returns>
        Task<User?> GetUserAsync(int userId);

        /// <summary>
        /// 更新使用者資料
        /// </summary>
        /// <param name="userId">使用者ID</param>
        /// <param name="userUpdate">更新資料</param>
        /// <returns>更新結果</returns>
        Task<bool> UpdateUserAsync(int userId, UserUpdate userUpdate);

        /// <summary>
        /// 變更密碼
        /// </summary>
        /// <param name="userId">使用者ID</param>
        /// <param name="passwordChange">密碼變更</param>
        /// <returns>變更結果</returns>
        Task<bool> ChangePasswordAsync(int userId, PasswordChange passwordChange);

        /// <summary>
        /// 重設密碼
        /// </summary>
        /// <param name="email">電子郵件</param>
        /// <returns>重設結果</returns>
        Task<bool> ResetPasswordAsync(string email);

        /// <summary>
        /// 檢查使用者是否存在
        /// </summary>
        /// <param name="username">使用者名稱</param>
        /// <param name="email">電子郵件</param>
        /// <returns>檢查結果</returns>
        Task<UserExistenceCheck> CheckUserExistenceAsync(string username, string email);
    }

    /// <summary>
    /// 使用者註冊模型
    /// </summary>
    public class UserRegistration
    {
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string ConfirmPassword { get; set; } = string.Empty;
        public string Nickname { get; set; } = string.Empty;
        public DateTime BirthDate { get; set; }
        public string Phone { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
    }

    /// <summary>
    /// 使用者登入請求模型
    /// </summary>
    public class UserLoginRequest
    {
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public bool RememberMe { get; set; }
    }

    /// <summary>
    /// 使用者更新模型
    /// </summary>
    public class UserUpdate
    {
        public string Nickname { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string? Avatar { get; set; }
    }

    /// <summary>
    /// 密碼變更模型
    /// </summary>
    public class PasswordChange
    {
        public string CurrentPassword { get; set; } = string.Empty;
        public string NewPassword { get; set; } = string.Empty;
        public string ConfirmNewPassword { get; set; } = string.Empty;
    }

    /// <summary>
    /// 使用者註冊結果模型
    /// </summary>
    public class UserRegistrationResult
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; } = string.Empty;
        public User? User { get; set; }
        public List<string> Errors { get; set; } = new();
    }

    /// <summary>
    /// 使用者登入結果模型
    /// </summary>
    public class UserLoginResult
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; } = string.Empty;
        public User? User { get; set; }
        public string? Token { get; set; }
    }

    /// <summary>
    /// 使用者存在檢查模型
    /// </summary>
    public class UserExistenceCheck
    {
        public bool UsernameExists { get; set; }
        public bool EmailExists { get; set; }
        public string Message { get; set; } = string.Empty;
    }
}