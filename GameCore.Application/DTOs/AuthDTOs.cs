using System.ComponentModel.DataAnnotations;

namespace GameCore.Application.DTOs
{
    /// <summary>
    /// 登入請求
    /// </summary>
    public class LoginRequest
    {
        /// <summary>
        /// 用戶名或電子郵件
        /// </summary>
        [Required(ErrorMessage = "用戶名或電子郵件為必填")]
        public string UserNameOrEmail { get; set; } = string.Empty;

        /// <summary>
        /// 密碼
        /// </summary>
        [Required(ErrorMessage = "密碼為必填")]
        public string Password { get; set; } = string.Empty;

        /// <summary>
        /// 記住我
        /// </summary>
        public bool RememberMe { get; set; } = false;
    }

    /// <summary>
    /// 登入回應
    /// </summary>
    public class LoginResponse
    {
        /// <summary>
        /// 是否成功
        /// </summary>
        public bool IsSuccess { get; set; }

        /// <summary>
        /// 存取令牌
        /// </summary>
        public string? AccessToken { get; set; }

        /// <summary>
        /// 重新整理令牌
        /// </summary>
        public string? RefreshToken { get; set; }

        /// <summary>
        /// 過期時間
        /// </summary>
        public DateTime? ExpiresAt { get; set; }

        /// <summary>
        /// 用戶資訊
        /// </summary>
        public UserResponse? User { get; set; }

        /// <summary>
        /// 錯誤訊息
        /// </summary>
        public string? ErrorMessage { get; set; }
    }

    /// <summary>
    /// 註冊請求
    /// </summary>
    public class RegisterRequest
    {
        /// <summary>
        /// 用戶名
        /// </summary>
        [Required(ErrorMessage = "用戶名為必填")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "用戶名長度必須在 3-100 字元之間")]
        [RegularExpression(@"^[a-zA-Z0-9_]+$", ErrorMessage = "用戶名只能包含字母、數字和底線")]
        public string UserName { get; set; } = string.Empty;

        /// <summary>
        /// 電子郵件
        /// </summary>
        [Required(ErrorMessage = "電子郵件為必填")]
        [EmailAddress(ErrorMessage = "電子郵件格式不正確")]
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// 密碼
        /// </summary>
        [Required(ErrorMessage = "密碼為必填")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "密碼長度必須至少 6 字元")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{6,}$", ErrorMessage = "密碼必須包含大小寫字母和數字")]
        public string Password { get; set; } = string.Empty;

        /// <summary>
        /// 確認密碼
        /// </summary>
        [Required(ErrorMessage = "確認密碼為必填")]
        [Compare("Password", ErrorMessage = "密碼和確認密碼不一致")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }

    /// <summary>
    /// 註冊回應
    /// </summary>
    public class RegisterResponse
    {
        /// <summary>
        /// 是否成功
        /// </summary>
        public bool IsSuccess { get; set; }

        /// <summary>
        /// 用戶資訊
        /// </summary>
        public UserResponse? User { get; set; }

        /// <summary>
        /// 錯誤訊息
        /// </summary>
        public string? ErrorMessage { get; set; }
    }

    /// <summary>
    /// 變更密碼請求
    /// </summary>
    public class ChangePasswordRequest
    {
        /// <summary>
        /// 當前密碼
        /// </summary>
        [Required(ErrorMessage = "當前密碼為必填")]
        public string CurrentPassword { get; set; } = string.Empty;

        /// <summary>
        /// 新密碼
        /// </summary>
        [Required(ErrorMessage = "新密碼為必填")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "密碼長度必須至少 6 字元")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{6,}$", ErrorMessage = "密碼必須包含大小寫字母和數字")]
        public string NewPassword { get; set; } = string.Empty;

        /// <summary>
        /// 確認新密碼
        /// </summary>
        [Required(ErrorMessage = "確認新密碼為必填")]
        [Compare("NewPassword", ErrorMessage = "新密碼和確認密碼不一致")]
        public string ConfirmNewPassword { get; set; } = string.Empty;
    }

    /// <summary>
    /// 重新整理令牌請求
    /// </summary>
    public class RefreshTokenRequest
    {
        /// <summary>
        /// 重新整理令牌
        /// </summary>
        [Required(ErrorMessage = "重新整理令牌為必填")]
        public string RefreshToken { get; set; } = string.Empty;
    }

    /// <summary>
    /// 重新整理令牌回應
    /// </summary>
    public class RefreshTokenResponse
    {
        /// <summary>
        /// 是否成功
        /// </summary>
        public bool IsSuccess { get; set; }

        /// <summary>
        /// 存取令牌
        /// </summary>
        public string? AccessToken { get; set; }

        /// <summary>
        /// 重新整理令牌
        /// </summary>
        public string? RefreshToken { get; set; }

        /// <summary>
        /// 過期時間
        /// </summary>
        public DateTime? ExpiresAt { get; set; }

        /// <summary>
        /// 錯誤訊息
        /// </summary>
        public string? ErrorMessage { get; set; }
    }
} 