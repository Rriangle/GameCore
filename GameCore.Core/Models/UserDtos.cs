namespace GameCore.Core.Models
{
    /// <summary>
    /// 用戶註冊 DTO
    /// </summary>
    public class UserRegistrationDto
    {
        /// <summary>
        /// 用戶名
        /// </summary>
        public string Username { get; set; } = string.Empty;

        /// <summary>
        /// 郵箱
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
        /// 名字
        /// </summary>
        public string? FirstName { get; set; }

        /// <summary>
        /// 姓氏
        /// </summary>
        public string? LastName { get; set; }

        /// <summary>
        /// 電話
        /// </summary>
        public string? Phone { get; set; }

        /// <summary>
        /// 出生日期
        /// </summary>
        public DateTime? DateOfBirth { get; set; }

        /// <summary>
        /// 性別
        /// </summary>
        public string? Gender { get; set; }

        /// <summary>
        /// 國家
        /// </summary>
        public string? Country { get; set; }

        /// <summary>
        /// 城市
        /// </summary>
        public string? City { get; set; }

        /// <summary>
        /// 地址
        /// </summary>
        public string? Address { get; set; }

        /// <summary>
        /// 個人照片
        /// </summary>
        public string? ProfilePicture { get; set; }

        /// <summary>
        /// 個人簡介
        /// </summary>
        public string? Bio { get; set; }

        /// <summary>
        /// 興趣愛好
        /// </summary>
        public string? Interests { get; set; }

        /// <summary>
        /// 個人網站
        /// </summary>
        public string? Website { get; set; }

        /// <summary>
        /// 社交媒體
        /// </summary>
        public string? SocialMedia { get; set; }
    }

    /// <summary>
    /// 用戶註冊結果 DTO
    /// </summary>
    public class UserRegistrationResultDto
    {
        /// <summary>
        /// 用戶ID
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// 用戶名
        /// </summary>
        public string Username { get; set; } = string.Empty;

        /// <summary>
        /// 郵箱
        /// </summary>
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// 訊息
        /// </summary>
        public string Message { get; set; } = string.Empty;
    }

    /// <summary>
    /// 用戶登入 DTO
    /// </summary>
    public class UserLoginDto
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
    /// 用戶登入結果 DTO
    /// </summary>
    public class UserLoginResultDto
    {
        /// <summary>
        /// 用戶ID
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// 用戶名
        /// </summary>
        public string Username { get; set; } = string.Empty;

        /// <summary>
        /// 郵箱
        /// </summary>
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// JWT Token
        /// </summary>
        public string Token { get; set; } = string.Empty;

        /// <summary>
        /// 過期時間
        /// </summary>
        public DateTime ExpiresAt { get; set; }

        /// <summary>
        /// 訊息
        /// </summary>
        public string Message { get; set; } = string.Empty;
    }

    /// <summary>
    /// 第三方登入 DTO
    /// </summary>
    public class ThirdPartyLoginDto
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
    /// 用戶資料 DTO
    /// </summary>
    public class UserProfileDto
    {
        /// <summary>
        /// 用戶ID
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// 用戶名
        /// </summary>
        public string Username { get; set; } = string.Empty;

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
        /// 電話
        /// </summary>
        public string? Phone { get; set; }

        /// <summary>
        /// 出生日期
        /// </summary>
        public DateTime? DateOfBirth { get; set; }

        /// <summary>
        /// 性別
        /// </summary>
        public string? Gender { get; set; }

        /// <summary>
        /// 國家
        /// </summary>
        public string? Country { get; set; }

        /// <summary>
        /// 城市
        /// </summary>
        public string? City { get; set; }

        /// <summary>
        /// 地址
        /// </summary>
        public string? Address { get; set; }

        /// <summary>
        /// 個人照片
        /// </summary>
        public string? ProfilePicture { get; set; }

        /// <summary>
        /// 個人簡介
        /// </summary>
        public string? Bio { get; set; }

        /// <summary>
        /// 興趣愛好
        /// </summary>
        public string? Interests { get; set; }

        /// <summary>
        /// 個人網站
        /// </summary>
        public string? Website { get; set; }

        /// <summary>
        /// 社交媒體
        /// </summary>
        public string? SocialMedia { get; set; }

        /// <summary>
        /// 角色
        /// </summary>
        public string? Role { get; set; }

        /// <summary>
        /// 是否啟用
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// 郵箱是否驗證
        /// </summary>
        public bool EmailVerified { get; set; }

        /// <summary>
        /// 電話是否驗證
        /// </summary>
        public bool PhoneVerified { get; set; }

        /// <summary>
        /// 創建時間
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// 最後登入時間
        /// </summary>
        public DateTime? LastLoginAt { get; set; }
    }

    /// <summary>
    /// 用戶資料更新 DTO
    /// </summary>
    public class UserProfileUpdateDto
    {
        /// <summary>
        /// 名字
        /// </summary>
        public string? FirstName { get; set; }

        /// <summary>
        /// 姓氏
        /// </summary>
        public string? LastName { get; set; }

        /// <summary>
        /// 電話
        /// </summary>
        public string? Phone { get; set; }

        /// <summary>
        /// 出生日期
        /// </summary>
        public DateTime? DateOfBirth { get; set; }

        /// <summary>
        /// 性別
        /// </summary>
        public string? Gender { get; set; }

        /// <summary>
        /// 國家
        /// </summary>
        public string? Country { get; set; }

        /// <summary>
        /// 城市
        /// </summary>
        public string? City { get; set; }

        /// <summary>
        /// 地址
        /// </summary>
        public string? Address { get; set; }

        /// <summary>
        /// 個人照片
        /// </summary>
        public string? ProfilePicture { get; set; }

        /// <summary>
        /// 個人簡介
        /// </summary>
        public string? Bio { get; set; }

        /// <summary>
        /// 興趣愛好
        /// </summary>
        public string? Interests { get; set; }

        /// <summary>
        /// 個人網站
        /// </summary>
        public string? Website { get; set; }

        /// <summary>
        /// 社交媒體
        /// </summary>
        public string? SocialMedia { get; set; }
    }

    /// <summary>
    /// 變更密碼 DTO
    /// </summary>
    public class ChangePasswordDto
    {
        /// <summary>
        /// 舊密碼
        /// </summary>
        public string OldPassword { get; set; } = string.Empty;

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
    /// 忘記密碼 DTO
    /// </summary>
    public class ForgotPasswordDto
    {
        /// <summary>
        /// 郵箱
        /// </summary>
        public string Email { get; set; } = string.Empty;
    }

    /// <summary>
    /// 重置密碼 DTO
    /// </summary>
    public class ResetPasswordDto
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
    /// Token 驗證結果 DTO
    /// </summary>
    public class TokenValidationResultDto
    {
        /// <summary>
        /// 是否有效
        /// </summary>
        public bool IsValid { get; set; }

        /// <summary>
        /// 用戶ID
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// 用戶名
        /// </summary>
        public string Username { get; set; } = string.Empty;

        /// <summary>
        /// 郵箱
        /// </summary>
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// 過期時間
        /// </summary>
        public DateTime ExpiresAt { get; set; }
    }

    /// <summary>
    /// Token 刷新結果 DTO
    /// </summary>
    public class TokenRefreshResultDto
    {
        /// <summary>
        /// 新的 JWT Token
        /// </summary>
        public string NewToken { get; set; } = string.Empty;

        /// <summary>
        /// 新的刷新 Token
        /// </summary>
        public string RefreshToken { get; set; } = string.Empty;

        /// <summary>
        /// 過期時間
        /// </summary>
        public DateTime ExpiresAt { get; set; }
    }
}