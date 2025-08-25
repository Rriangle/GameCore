namespace GameCore.Core.Models
{
    /// <summary>
    /// 認證請求 DTO
    /// </summary>
    public class AuthRequestDto
    {
        /// <summary>
        /// 用戶名或郵箱
        /// </summary>
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// 密碼
        /// </summary>
        public string Password { get; set; } = string.Empty;

        /// <summary>
        /// 記住我
        /// </summary>
        public bool RememberMe { get; set; }
    }

    /// <summary>
    /// 認證回應 DTO
    /// </summary>
    public class AuthResponseDto
    {
        /// <summary>
        /// 是否成功
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// 訊息
        /// </summary>
        public string Message { get; set; } = string.Empty;

        /// <summary>
        /// 用戶ID
        /// </summary>
        public int? UserId { get; set; }

        /// <summary>
        /// 用戶名
        /// </summary>
        public string? Username { get; set; }

        /// <summary>
        /// 郵箱
        /// </summary>
        public string? Email { get; set; }

        /// <summary>
        /// JWT Token
        /// </summary>
        public string? Token { get; set; }

        /// <summary>
        /// 過期時間
        /// </summary>
        public DateTime? ExpiresAt { get; set; }

        /// <summary>
        /// 錯誤代碼
        /// </summary>
        public string? ErrorCode { get; set; }
    }

    /// <summary>
    /// 第三方認證請求 DTO
    /// </summary>
    public class ThirdPartyAuthRequestDto
    {
        /// <summary>
        /// 第三方提供者
        /// </summary>
        public string Provider { get; set; } = string.Empty;

        /// <summary>
        /// 第三方用戶ID
        /// </summary>
        public string ProviderId { get; set; } = string.Empty;

        /// <summary>
        /// 第三方 Token
        /// </summary>
        public string ProviderToken { get; set; } = string.Empty;

        /// <summary>
        /// 用戶名
        /// </summary>
        public string? Username { get; set; }

        /// <summary>
        /// 郵箱
        /// </summary>
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// 名字
        /// </summary>
        public string? FirstName { get; set; }

        /// <summary>
        /// 姓氏
        /// </summary>
        public string? LastName { get; set; }

        /// <summary>
        /// 個人照片
        /// </summary>
        public string? ProfilePicture { get; set; }
    }

    /// <summary>
    /// 第三方認證回應 DTO
    /// </summary>
    public class ThirdPartyAuthResponseDto
    {
        /// <summary>
        /// 是否成功
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// 訊息
        /// </summary>
        public string Message { get; set; } = string.Empty;

        /// <summary>
        /// 用戶ID
        /// </summary>
        public int? UserId { get; set; }

        /// <summary>
        /// 用戶名
        /// </summary>
        public string? Username { get; set; }

        /// <summary>
        /// 郵箱
        /// </summary>
        public string? Email { get; set; }

        /// <summary>
        /// JWT Token
        /// </summary>
        public string? Token { get; set; }

        /// <summary>
        /// 過期時間
        /// </summary>
        public DateTime? ExpiresAt { get; set; }

        /// <summary>
        /// 是否為新用戶
        /// </summary>
        public bool IsNewUser { get; set; }

        /// <summary>
        /// 錯誤代碼
        /// </summary>
        public string? ErrorCode { get; set; }
    }

    /// <summary>
    /// 登出請求 DTO
    /// </summary>
    public class LogoutRequestDto
    {
        /// <summary>
        /// 用戶ID
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// 登出原因
        /// </summary>
        public string? Reason { get; set; }
    }

    /// <summary>
    /// 登出回應 DTO
    /// </summary>
    public class LogoutResponseDto
    {
        /// <summary>
        /// 是否成功
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// 訊息
        /// </summary>
        public string Message { get; set; } = string.Empty;

        /// <summary>
        /// 登出時間
        /// </summary>
        public DateTime LogoutTime { get; set; }
    }

    /// <summary>
    /// Token 驗證請求 DTO
    /// </summary>
    public class TokenValidationRequestDto
    {
        /// <summary>
        /// JWT Token
        /// </summary>
        public string Token { get; set; } = string.Empty;
    }

    /// <summary>
    /// Token 驗證回應 DTO
    /// </summary>
    public class TokenValidationResponseDto
    {
        /// <summary>
        /// 是否有效
        /// </summary>
        public bool IsValid { get; set; }

        /// <summary>
        /// 用戶ID
        /// </summary>
        public int? UserId { get; set; }

        /// <summary>
        /// 用戶名
        /// </summary>
        public string? Username { get; set; }

        /// <summary>
        /// 郵箱
        /// </summary>
        public string? Email { get; set; }

        /// <summary>
        /// 過期時間
        /// </summary>
        public DateTime? ExpiresAt { get; set; }

        /// <summary>
        /// 剩餘有效時間（分鐘）
        /// </summary>
        public int? RemainingMinutes { get; set; }

        /// <summary>
        /// 錯誤訊息
        /// </summary>
        public string? ErrorMessage { get; set; }
    }

    /// <summary>
    /// Token 刷新請求 DTO
    /// </summary>
    public class TokenRefreshRequestDto
    {
        /// <summary>
        /// 刷新 Token
        /// </summary>
        public string RefreshToken { get; set; } = string.Empty;
    }

    /// <summary>
    /// Token 刷新回應 DTO
    /// </summary>
    public class TokenRefreshResponseDto
    {
        /// <summary>
        /// 是否成功
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// 訊息
        /// </summary>
        public string Message { get; set; } = string.Empty;

        /// <summary>
        /// 新的 JWT Token
        /// </summary>
        public string? NewToken { get; set; }

        /// <summary>
        /// 新的刷新 Token
        /// </summary>
        public string? NewRefreshToken { get; set; }

        /// <summary>
        /// 過期時間
        /// </summary>
        public DateTime? ExpiresAt { get; set; }

        /// <summary>
        /// 錯誤代碼
        /// </summary>
        public string? ErrorCode { get; set; }
    }

    /// <summary>
    /// 忘記密碼請求 DTO
    /// </summary>
    public class ForgotPasswordRequestDto
    {
        /// <summary>
        /// 郵箱
        /// </summary>
        public string Email { get; set; } = string.Empty;
    }

    /// <summary>
    /// 忘記密碼回應 DTO
    /// </summary>
    public class ForgotPasswordResponseDto
    {
        /// <summary>
        /// 是否成功
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// 訊息
        /// </summary>
        public string Message { get; set; } = string.Empty;

        /// <summary>
        /// 重置連結已發送
        /// </summary>
        public bool ResetLinkSent { get; set; }

        /// <summary>
        /// 錯誤代碼
        /// </summary>
        public string? ErrorCode { get; set; }
    }

    /// <summary>
    /// 重置密碼請求 DTO
    /// </summary>
    public class ResetPasswordRequestDto
    {
        /// <summary>
        /// 重置 Token
        /// </summary>
        public string Token { get; set; } = string.Empty;

        /// <summary>
        /// 新密碼
        /// </summary>
        public string NewPassword { get; set; } = string.Empty;

        /// <summary>
        /// 確認新密碼
        /// </summary>
        public string ConfirmNewPassword { get; set; } = string.Empty;
    }

    /// <summary>
    /// 重置密碼回應 DTO
    /// </summary>
    public class ResetPasswordResponseDto
    {
        /// <summary>
        /// 是否成功
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// 訊息
        /// </summary>
        public string Message { get; set; } = string.Empty;

        /// <summary>
        /// 密碼重置時間
        /// </summary>
        public DateTime? ResetTime { get; set; }

        /// <summary>
        /// 錯誤代碼
        /// </summary>
        public string? ErrorCode { get; set; }
    }

    /// <summary>
    /// 郵箱驗證請求 DTO
    /// </summary>
    public class EmailVerificationRequestDto
    {
        /// <summary>
        /// 郵箱
        /// </summary>
        public string Email { get; set; } = string.Empty;
    }

    /// <summary>
    /// 郵箱驗證回應 DTO
    /// </summary>
    public class EmailVerificationResponseDto
    {
        /// <summary>
        /// 是否成功
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// 訊息
        /// </summary>
        public string Message { get; set; } = string.Empty;

        /// <summary>
        /// 驗證連結已發送
        /// </summary>
        public bool VerificationLinkSent { get; set; }

        /// <summary>
        /// 錯誤代碼
        /// </summary>
        public string? ErrorCode { get; set; }
    }

    /// <summary>
    /// 郵箱驗證確認請求 DTO
    /// </summary>
    public class EmailVerificationConfirmRequestDto
    {
        /// <summary>
        /// 驗證 Token
        /// </summary>
        public string Token { get; set; } = string.Empty;
    }

    /// <summary>
    /// 郵箱驗證確認回應 DTO
    /// </summary>
    public class EmailVerificationConfirmResponseDto
    {
        /// <summary>
        /// 是否成功
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// 訊息
        /// </summary>
        public string Message { get; set; } = string.Empty;

        /// <summary>
        /// 郵箱是否已驗證
        /// </summary>
        public bool EmailVerified { get; set; }

        /// <summary>
        /// 驗證時間
        /// </summary>
        public DateTime? VerificationTime { get; set; }

        /// <summary>
        /// 錯誤代碼
        /// </summary>
        public string? ErrorCode { get; set; }
    }

    /// <summary>
    /// 電話驗證請求 DTO
    /// </summary>
    public class PhoneVerificationRequestDto
    {
        /// <summary>
        /// 電話號碼
        /// </summary>
        public string Phone { get; set; } = string.Empty;
    }

    /// <summary>
    /// 電話驗證回應 DTO
    /// </summary>
    public class PhoneVerificationResponseDto
    {
        /// <summary>
        /// 是否成功
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// 訊息
        /// </summary>
        public string Message { get; set; } = string.Empty;

        /// <summary>
        /// 驗證碼已發送
        /// </summary>
        public bool VerificationCodeSent { get; set; }

        /// <summary>
        /// 錯誤代碼
        /// </summary>
        public string? ErrorCode { get; set; }
    }

    /// <summary>
    /// 電話驗證確認請求 DTO
    /// </summary>
    public class PhoneVerificationConfirmRequestDto
    {
        /// <summary>
        /// 電話號碼
        /// </summary>
        public string Phone { get; set; } = string.Empty;

        /// <summary>
        /// 驗證碼
        /// </summary>
        public string VerificationCode { get; set; } = string.Empty;
    }

    /// <summary>
    /// 電話驗證確認回應 DTO
    /// </summary>
    public class PhoneVerificationConfirmResponseDto
    {
        /// <summary>
        /// 是否成功
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// 訊息
        /// </summary>
        public string Message { get; set; } = string.Empty;

        /// <summary>
        /// 電話是否已驗證
        /// </summary>
        public bool PhoneVerified { get; set; }

        /// <summary>
        /// 驗證時間
        /// </summary>
        public DateTime? VerificationTime { get; set; }

        /// <summary>
        /// 錯誤代碼
        /// </summary>
        public string? ErrorCode { get; set; }
    }

    /// <summary>
    /// 會話狀態 DTO
    /// </summary>
    public class SessionStatusDto
    {
        /// <summary>
        /// 是否已登入
        /// </summary>
        public bool IsAuthenticated { get; set; }

        /// <summary>
        /// 用戶ID
        /// </summary>
        public int? UserId { get; set; }

        /// <summary>
        /// 用戶名
        /// </summary>
        public string? Username { get; set; }

        /// <summary>
        /// 郵箱
        /// </summary>
        public string? Email { get; set; }

        /// <summary>
        /// 角色
        /// </summary>
        public string? Role { get; set; }

        /// <summary>
        /// 登入時間
        /// </summary>
        public DateTime? LoginTime { get; set; }

        /// <summary>
        /// 最後活動時間
        /// </summary>
        public DateTime? LastActivityTime { get; set; }

        /// <summary>
        /// 會話剩餘時間（分鐘）
        /// </summary>
        public int? SessionRemainingMinutes { get; set; }
    }

    /// <summary>
    /// 會話延長請求 DTO
    /// </summary>
    public class SessionExtendRequestDto
    {
        /// <summary>
        /// 延長分鐘數
        /// </summary>
        public int ExtendMinutes { get; set; } = 30;
    }

    /// <summary>
    /// 會話延長回應 DTO
    /// </summary>
    public class SessionExtendResponseDto
    {
        /// <summary>
        /// 是否成功
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// 訊息
        /// </summary>
        public string Message { get; set; } = string.Empty;

        /// <summary>
        /// 新的過期時間
        /// </summary>
        public DateTime? NewExpiresAt { get; set; }

        /// <summary>
        /// 會話剩餘時間（分鐘）
        /// </summary>
        public int? SessionRemainingMinutes { get; set; }

        /// <summary>
        /// 錯誤代碼
        /// </summary>
        public string? ErrorCode { get; set; }
    }
}