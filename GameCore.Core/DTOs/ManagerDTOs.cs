using System.ComponentModel.DataAnnotations;
using GameCore.Core.Enums;

namespace GameCore.Core.DTOs
{
    /// <summary>
    /// 管理員登入請求 DTO
    /// </summary>
    public class ManagerLoginDto
    {
        [Required(ErrorMessage = "帳號不能為空")]
        public string Account { get; set; } = string.Empty;

        [Required(ErrorMessage = "密碼不能為空")]
        public string Password { get; set; } = string.Empty;

        public bool RememberMe { get; set; }
    }

    /// <summary>
    /// 管理員登入結果 DTO
    /// </summary>
    public class ManagerLoginResult
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public string Token { get; set; } = string.Empty;
        public ManagerDto? Manager { get; set; }
        public List<string> Roles { get; set; } = new();
        public List<string> Permissions { get; set; } = new();
    }

    /// <summary>
    /// 管理員資料 DTO
    /// </summary>
    public class ManagerDto
    {
        public int ManagerId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Account { get; set; } = string.Empty;
        public DateTime RegistrationDate { get; set; }
        public DateTime? LastLogin { get; set; }
        public bool IsActive { get; set; }
        public List<string> Roles { get; set; } = new();
        public List<string> Permissions { get; set; } = new();
    }

    /// <summary>
    /// 管理員個人資料結果 DTO
    /// </summary>
    public class ManagerProfileResult
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public ManagerDto? Manager { get; set; }
    }

    /// <summary>
    /// 管理員更新請求 DTO
    /// </summary>
    public class ManagerUpdateDto
    {
        [Required(ErrorMessage = "姓名不能為空")]
        [StringLength(100, ErrorMessage = "姓名長度不能超過100個字符")]
        public string Name { get; set; } = string.Empty;

        [StringLength(100, ErrorMessage = "帳號長度不能超過100個字符")]
        public string? Account { get; set; }

        public bool? IsActive { get; set; }
    }

    /// <summary>
    /// 管理員更新結果 DTO
    /// </summary>
    public class ManagerUpdateResult
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public ManagerDto? Manager { get; set; }
    }

    /// <summary>
    /// 管理員創建請求 DTO
    /// </summary>
    public class ManagerCreateDto
    {
        [Required(ErrorMessage = "姓名不能為空")]
        [StringLength(100, ErrorMessage = "姓名長度不能超過100個字符")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "帳號不能為空")]
        [StringLength(100, ErrorMessage = "帳號長度不能超過100個字符")]
        public string Account { get; set; } = string.Empty;

        [Required(ErrorMessage = "密碼不能為空")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "密碼長度必須在6-100個字符之間")]
        public string Password { get; set; } = string.Empty;

        public List<ManagerRole> Roles { get; set; } = new();
    }

    /// <summary>
    /// 管理員創建結果 DTO
    /// </summary>
    public class ManagerCreateResult
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public ManagerDto? Manager { get; set; }
    }

    /// <summary>
    /// 密碼變更請求 DTO
    /// </summary>
    public class PasswordChangeDto
    {
        [Required(ErrorMessage = "舊密碼不能為空")]
        public string OldPassword { get; set; } = string.Empty;

        [Required(ErrorMessage = "新密碼不能為空")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "新密碼長度必須在6-100個字符之間")]
        public string NewPassword { get; set; } = string.Empty;

        [Required(ErrorMessage = "確認密碼不能為空")]
        [Compare("NewPassword", ErrorMessage = "確認密碼與新密碼不匹配")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }

    /// <summary>
    /// 密碼變更結果 DTO
    /// </summary>
    public class PasswordChangeResult
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
    }

    /// <summary>
    /// 管理員角色權限 DTO
    /// </summary>
    public class ManagerRolePermissionDto
    {
        public int RoleId { get; set; }
        public string RoleName { get; set; } = string.Empty;
        public bool AdministratorPrivilegesManagement { get; set; }
        public bool UserStatusManagement { get; set; }
        public bool ShoppingPermissionManagement { get; set; }
        public bool MessagePermissionManagement { get; set; }
        public bool PetRightsManagement { get; set; }
        public bool CustomerService { get; set; }
    }
}