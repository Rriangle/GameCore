namespace GameCore.Core.Types
{
    // 移除重複定義，使用 Entities 目錄中的 User 類別

    /// <summary>
    /// 註冊請求DTO
    /// </summary>
    public class RegisterRequestDto
    {
        /// <summary>
        /// 用戶名
        /// </summary>
        public string Username { get; set; } = string.Empty;

        /// <summary>
        /// 電子郵件
        /// </summary>
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// 密碼
        /// </summary>
        public string Password { get; set; } = string.Empty;

        /// <summary>
        /// 確認密碼
        /// </summary>
        public string ConfirmPassword { get; set; } = string.Empty;

        /// <summary>
        /// 暱稱
        /// </summary>
        public string Nickname { get; set; } = string.Empty;

        /// <summary>
        /// 同意條款
        /// </summary>
        public bool AgreeToTerms { get; set; }

        /// <summary>
        /// 同意行銷
        /// </summary>
        public bool AgreeToMarketing { get; set; }
    }

    /// <summary>
    /// 登入請求DTO
    /// </summary>
    public class LoginRequestDto
    {
        /// <summary>
        /// 用戶名或電子郵件
        /// </summary>
        public string UsernameOrEmail { get; set; } = string.Empty;

        /// <summary>
        /// 密碼
        /// </summary>
        public string Password { get; set; } = string.Empty;

        /// <summary>
        /// 記住我
        /// </summary>
        public bool RememberMe { get; set; }

        /// <summary>
        /// 雙重認證代碼
        /// </summary>
        public string? TwoFactorCode { get; set; }
    }

    /// <summary>
    /// OAuth登入請求DTO
    /// </summary>
    public class OAuthLoginRequestDto
    {
        /// <summary>
        /// 提供者
        /// </summary>
        public string Provider { get; set; } = string.Empty;

        /// <summary>
        /// 授權碼
        /// </summary>
        public string AuthorizationCode { get; set; } = string.Empty;

        /// <summary>
        /// 重定向URI
        /// </summary>
        public string RedirectUri { get; set; } = string.Empty;

        /// <summary>
        /// 狀態
        /// </summary>
        public string? State { get; set; }
    }

    /// <summary>
    /// 更新個人資料請求DTO
    /// </summary>
    public class UpdateProfileRequestDto
    {
        /// <summary>
        /// 暱稱
        /// </summary>
        public string? Nickname { get; set; }

        /// <summary>
        /// 頭像URL
        /// </summary>
        public string? AvatarUrl { get; set; }

        /// <summary>
        /// 生日
        /// </summary>
        public DateTime? Birthday { get; set; }

        /// <summary>
        /// 性別
        /// </summary>
        public string? Gender { get; set; }

        /// <summary>
        /// 電話號碼
        /// </summary>
        public string? PhoneNumber { get; set; }

        /// <summary>
        /// 地址
        /// </summary>
        public string? Address { get; set; }

        /// <summary>
        /// 國家
        /// </summary>
        public string? Country { get; set; }

        /// <summary>
        /// 時區
        /// </summary>
        public string? Timezone { get; set; }

        /// <summary>
        /// 語言偏好
        /// </summary>
        public string? Language { get; set; }
    }

    /// <summary>
    /// 變更密碼請求DTO
    /// </summary>
    public class ChangePasswordRequestDto
    {
        /// <summary>
        /// 當前密碼
        /// </summary>
        public string CurrentPassword { get; set; } = string.Empty;

        /// <summary>
        /// 新密碼
        /// </summary>
        public string NewPassword { get; set; } = string.Empty;

        /// <summary>
        /// 確認新密碼
        /// </summary>
        public string ConfirmNewPassword { get; set; } = string.Empty;
    }
} 
