using System.ComponentModel.DataAnnotations;

namespace GameCore.Core.DTOs
{
    /// <summary>
    /// 用戶註冊請求 DTO
    /// </summary>
    public class UserRegistrationDto
    {
        [Required(ErrorMessage = "用戶名不能為空")]
        [StringLength(50, ErrorMessage = "用戶名長度不能超過50個字符")]
        public string UserName { get; set; } = string.Empty;

        [Required(ErrorMessage = "帳號不能為空")]
        [StringLength(100, ErrorMessage = "帳號長度不能超過100個字符")]
        public string Account { get; set; } = string.Empty;

        [Required(ErrorMessage = "密碼不能為空")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "密碼長度必須在6-100個字符之間")]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "確認密碼不能為空")]
        [Compare("Password", ErrorMessage = "確認密碼與密碼不匹配")]
        public string ConfirmPassword { get; set; } = string.Empty;

        [Required(ErrorMessage = "暱稱不能為空")]
        [StringLength(50, ErrorMessage = "暱稱長度不能超過50個字符")]
        public string NickName { get; set; } = string.Empty;

        [Required(ErrorMessage = "性別不能為空")]
        [StringLength(1, ErrorMessage = "性別必須是單個字符")]
        public string Gender { get; set; } = string.Empty;

        [Required(ErrorMessage = "身分證字號不能為空")]
        [StringLength(20, ErrorMessage = "身分證字號長度不能超過20個字符")]
        public string IdNumber { get; set; } = string.Empty;

        [Required(ErrorMessage = "手機號碼不能為空")]
        [StringLength(20, ErrorMessage = "手機號碼長度不能超過20個字符")]
        public string Cellphone { get; set; } = string.Empty;

        [Required(ErrorMessage = "電子郵件不能為空")]
        [EmailAddress(ErrorMessage = "電子郵件格式不正確")]
        [StringLength(100, ErrorMessage = "電子郵件長度不能超過100個字符")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "地址不能為空")]
        [StringLength(200, ErrorMessage = "地址長度不能超過200個字符")]
        public string Address { get; set; } = string.Empty;

        [Required(ErrorMessage = "出生日期不能為空")]
        public DateTime DateOfBirth { get; set; }

        [StringLength(200, ErrorMessage = "自我介紹長度不能超過200個字符")]
        public string? UserIntroduce { get; set; }
    }

    /// <summary>
    /// 用戶登入請求 DTO
    /// </summary>
    public class UserLoginDto
    {
        [Required(ErrorMessage = "帳號不能為空")]
        public string Account { get; set; } = string.Empty;

        [Required(ErrorMessage = "密碼不能為空")]
        public string Password { get; set; } = string.Empty;

        public bool RememberMe { get; set; }
    }

    /// <summary>
    /// 用戶資料 DTO
    /// </summary>
    public class UserDto
    {
        public int UserId { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string Account { get; set; } = string.Empty;
        public string NickName { get; set; } = string.Empty;
        public string Gender { get; set; } = string.Empty;
        public string IdNumber { get; set; } = string.Empty;
        public string Cellphone { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public DateTime DateOfBirth { get; set; }
        public DateTime CreateAccountDate { get; set; }
        public string? UserIntroduce { get; set; }
        public bool UserStatus { get; set; }
        public bool ShoppingPermission { get; set; }
        public bool MessagePermission { get; set; }
        public bool SalesAuthority { get; set; }
        public int UserPoint { get; set; }
        public string? CouponNumber { get; set; }
    }

    /// <summary>
    /// 用戶個人資料結果 DTO
    /// </summary>
    public class UserProfileResult
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public UserDto? User { get; set; }
    }

    /// <summary>
    /// 用戶更新請求 DTO
    /// </summary>
    public class UserUpdateDto
    {
        [StringLength(50, ErrorMessage = "用戶名長度不能超過50個字符")]
        public string? UserName { get; set; }

        [StringLength(50, ErrorMessage = "暱稱長度不能超過50個字符")]
        public string? NickName { get; set; }

        [StringLength(20, ErrorMessage = "手機號碼長度不能超過20個字符")]
        public string? Cellphone { get; set; }

        [EmailAddress(ErrorMessage = "電子郵件格式不正確")]
        [StringLength(100, ErrorMessage = "電子郵件長度不能超過100個字符")]
        public string? Email { get; set; }

        [StringLength(200, ErrorMessage = "地址長度不能超過200個字符")]
        public string? Address { get; set; }

        [StringLength(200, ErrorMessage = "自我介紹長度不能超過200個字符")]
        public string? UserIntroduce { get; set; }

        public DateTime? DateOfBirth { get; set; }
    }

    /// <summary>
    /// 用戶更新結果 DTO
    /// </summary>
    public class UserUpdateResult
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public UserDto? User { get; set; }
    }

    /// <summary>
    /// 用戶註冊請求（簡化版）
    /// </summary>
    public class UserRegistration
    {
        public string UserName { get; set; } = string.Empty;
        public string Account { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string NickName { get; set; } = string.Empty;
        public string Gender { get; set; } = string.Empty;
        public string IdNumber { get; set; } = string.Empty;
        public string Cellphone { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public DateTime DateOfBirth { get; set; }
        public string? UserIntroduce { get; set; }
    }

    /// <summary>
    /// 用戶登入請求（簡化版）
    /// </summary>
    public class UserLoginRequest
    {
        public string Account { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public bool RememberMe { get; set; }
    }

    /// <summary>
    /// 用戶更新請求（簡化版）
    /// </summary>
    public class UserUpdate
    {
        public string? UserName { get; set; }
        public string? NickName { get; set; }
        public string? Cellphone { get; set; }
        public string? Email { get; set; }
        public string? Address { get; set; }
        public string? UserIntroduce { get; set; }
        public DateTime? DateOfBirth { get; set; }
    }

    /// <summary>
    /// 密碼變更請求（簡化版）
    /// </summary>
    public class PasswordChange
    {
        public string OldPassword { get; set; } = string.Empty;
        public string NewPassword { get; set; } = string.Empty;
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}