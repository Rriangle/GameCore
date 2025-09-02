using System.ComponentModel.DataAnnotations;

namespace GameCore.Core.DTOs
{
    /// <summary>
    /// 用戶註冊請求DTO
    /// </summary>
    public class RegisterRequestDto
    {
        /// <summary>
        /// 使用者姓名
        /// </summary>
        [Required(ErrorMessage = "使用者姓名為必填項目")]
        [StringLength(100, ErrorMessage = "使用者姓名不能超過100個字元")]
        public string UserName { get; set; } = string.Empty;

        /// <summary>
        /// 登入帳號
        /// </summary>
        [Required(ErrorMessage = "登入帳號為必填項目")]
        [StringLength(50, ErrorMessage = "登入帳號不能超過50個字元")]
        [RegularExpression(@"^[a-zA-Z0-9_]+$", ErrorMessage = "帳號只能包含英文字母、數字和底線")]
        public string UserAccount { get; set; } = string.Empty;

        /// <summary>
        /// 密碼
        /// </summary>
        [Required(ErrorMessage = "密碼為必填項目")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "密碼長度必須在6-100個字元之間")]
        public string Password { get; set; } = string.Empty;

        /// <summary>
        /// 確認密碼
        /// </summary>
        [Required(ErrorMessage = "確認密碼為必填項目")]
        [Compare("Password", ErrorMessage = "密碼與確認密碼不符")]
        public string ConfirmPassword { get; set; } = string.Empty;

        /// <summary>
        /// 電子郵件
        /// </summary>
        [Required(ErrorMessage = "電子郵件為必填項目")]
        [EmailAddress(ErrorMessage = "請輸入有效的電子郵件地址")]
        public string Email { get; set; } = string.Empty;
    }

    /// <summary>
    /// 用戶登入請求DTO
    /// </summary>
    public class LoginRequestDto
    {
        /// <summary>
        /// 登入帳號或電子郵件
        /// </summary>
        [Required(ErrorMessage = "帳號或電子郵件為必填項目")]
        public string AccountOrEmail { get; set; } = string.Empty;

        /// <summary>
        /// 密碼
        /// </summary>
        [Required(ErrorMessage = "密碼為必填項目")]
        public string Password { get; set; } = string.Empty;

        /// <summary>
        /// 記住我
        /// </summary>
        public bool RememberMe { get; set; } = false;
    }

    /// <summary>
    /// 登入回應DTO
    /// </summary>
    public class LoginResponseDto
    {
        /// <summary>
        /// 是否成功
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// 錯誤訊息
        /// </summary>
        public string? ErrorMessage { get; set; }

        /// <summary>
        /// JWT Token
        /// </summary>
        public string? Token { get; set; }

        /// <summary>
        /// 用戶資訊
        /// </summary>
        public UserInfoDto? UserInfo { get; set; }
    }

    /// <summary>
    /// 用戶資訊DTO
    /// </summary>
    public class UserInfoDto
    {
        /// <summary>
        /// 用戶ID
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// 使用者姓名
        /// </summary>
        public string UserName { get; set; } = string.Empty;

        /// <summary>
        /// 登入帳號
        /// </summary>
        public string UserAccount { get; set; } = string.Empty;

        /// <summary>
        /// 電子郵件
        /// </summary>
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// 暱稱
        /// </summary>
        public string? NickName { get; set; }

        /// <summary>
        /// 頭像URL
        /// </summary>
        public string? AvatarUrl { get; set; }

        /// <summary>
        /// 用戶權限
        /// </summary>
        public UserRightsDto? Rights { get; set; }

        /// <summary>
        /// 錢包資訊
        /// </summary>
        public WalletInfoDto? Wallet { get; set; }
    }

    /// <summary>
    /// 用戶權限DTO
    /// </summary>
    public class UserRightsDto
    {
        /// <summary>
        /// 使用者狀態
        /// </summary>
        public bool UserStatus { get; set; }

        /// <summary>
        /// 購物權限
        /// </summary>
        public bool ShoppingPermission { get; set; }

        /// <summary>
        /// 留言權限
        /// </summary>
        public bool MessagePermission { get; set; }

        /// <summary>
        /// 銷售權限
        /// </summary>
        public bool SalesAuthority { get; set; }
    }

    /// <summary>
    /// 錢包資訊DTO
    /// </summary>
    public class WalletInfoDto
    {
        /// <summary>
        /// 點數餘額
        /// </summary>
        public int UserPoint { get; set; }

        /// <summary>
        /// 優惠券編號
        /// </summary>
        public string? CouponNumber { get; set; }
    }

    /// <summary>
    /// OAuth登入請求DTO
    /// </summary>
    public class OAuthLoginRequestDto
    {
        /// <summary>
        /// 提供者 (Google, Facebook, Discord)
        /// </summary>
        [Required(ErrorMessage = "提供者為必填項目")]
        public string Provider { get; set; } = string.Empty;

        /// <summary>
        /// 授權碼
        /// </summary>
        [Required(ErrorMessage = "授權碼為必填項目")]
        public string Code { get; set; } = string.Empty;

        /// <summary>
        /// 重定向URI
        /// </summary>
        public string? RedirectUri { get; set; }
    }

    /// <summary>
    /// 更新個人資料請求DTO
    /// </summary>
    public class UpdateProfileRequestDto
    {
        /// <summary>
        /// 暱稱
        /// </summary>
        [StringLength(50, ErrorMessage = "暱稱不能超過50個字元")]
        public string? NickName { get; set; }

        /// <summary>
        /// 性別
        /// </summary>
        [StringLength(1, ErrorMessage = "性別只能是一個字元")]
        public string? Gender { get; set; }

        /// <summary>
        /// 聯繫電話
        /// </summary>
        [StringLength(20, ErrorMessage = "聯繫電話不能超過20個字元")]
        public string? Cellphone { get; set; }

        /// <summary>
        /// 地址
        /// </summary>
        [StringLength(200, ErrorMessage = "地址不能超過200個字元")]
        public string? Address { get; set; }

        /// <summary>
        /// 出生年月日
        /// </summary>
        public DateTime? DateOfBirth { get; set; }

        /// <summary>
        /// 使用者自介
        /// </summary>
        [StringLength(200, ErrorMessage = "自介不能超過200個字元")]
        public string? UserIntroduce { get; set; }
    }

    /// <summary>
    /// 變更密碼請求DTO
    /// </summary>
    public class ChangePasswordRequestDto
    {
        /// <summary>
        /// 目前密碼
        /// </summary>
        [Required(ErrorMessage = "目前密碼為必填項目")]
        public string CurrentPassword { get; set; } = string.Empty;

        /// <summary>
        /// 新密碼
        /// </summary>
        [Required(ErrorMessage = "新密碼為必填項目")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "密碼長度必須在6-100個字元之間")]
        public string NewPassword { get; set; } = string.Empty;

        /// <summary>
        /// 確認新密碼
        /// </summary>
        [Required(ErrorMessage = "確認新密碼為必填項目")]
        [Compare("NewPassword", ErrorMessage = "新密碼與確認密碼不符")]
        public string ConfirmNewPassword { get; set; } = string.Empty;
    }
} 
