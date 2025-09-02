using System.ComponentModel.DataAnnotations;

namespace GameCore.Core.DTOs
{
    /// <summary>
    /// 管理員登入請求 DTO
    /// </summary>
    public class ManagerLoginDto
    {
        /// <summary>
        /// 管理員帳號
        /// </summary>
        [Required]
        [StringLength(50)]
        public string Username { get; set; } = string.Empty;

        /// <summary>
        /// 管理員密碼
        /// </summary>
        [Required]
        [StringLength(100)]
        public string Password { get; set; } = string.Empty;
    }

    /// <summary>
    /// 管理員登入結果 DTO
    /// </summary>
    public class ManagerLoginResult
    {
        /// <summary>
        /// 是否登入成功
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// 錯誤訊息
        /// </summary>
        public string? ErrorMessage { get; set; }

        /// <summary>
        /// 管理員資訊
        /// </summary>
        public ManagerDto? Manager { get; set; }

        /// <summary>
        /// 登入 Token
        /// </summary>
        public string? Token { get; set; }
    }

    /// <summary>
    /// 管理員資料 DTO
    /// </summary>
    public class ManagerDto
    {
        /// <summary>
        /// 管理員 ID
        /// </summary>
        public int ManagerId { get; set; }

        /// <summary>
        /// 管理員帳號
        /// </summary>
        public string Username { get; set; } = string.Empty;

        /// <summary>
        /// 管理員姓名
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// 管理員 Email
        /// </summary>
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// 管理員角色
        /// </summary>
        public string Role { get; set; } = string.Empty;

        /// <summary>
        /// 是否啟用
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// 最後登入時間
        /// </summary>
        public DateTime? LastLoginTime { get; set; }

        /// <summary>
        /// 創建時間
        /// </summary>
        public DateTime CreatedAt { get; set; }
    }

    /// <summary>
    /// 管理員更新請求 DTO
    /// </summary>
    public class ManagerUpdateDto
    {
        /// <summary>
        /// 管理員姓名
        /// </summary>
        [StringLength(100)]
        public string? Name { get; set; }

        /// <summary>
        /// 管理員 Email
        /// </summary>
        [EmailAddress]
        [StringLength(100)]
        public string? Email { get; set; }

        /// <summary>
        /// 新密碼
        /// </summary>
        [StringLength(100)]
        public string? NewPassword { get; set; }

        /// <summary>
        /// 管理員角色
        /// </summary>
        [StringLength(50)]
        public string? Role { get; set; }

        /// <summary>
        /// 是否啟用
        /// </summary>
        public bool? IsActive { get; set; }
    }

    /// <summary>
    /// 管理員更新結果 DTO
    /// </summary>
    public class ManagerUpdateResult
    {
        /// <summary>
        /// 是否更新成功
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// 錯誤訊息
        /// </summary>
        public string? ErrorMessage { get; set; }

        /// <summary>
        /// 更新後的管理員資訊
        /// </summary>
        public ManagerDto? Manager { get; set; }
    }

    /// <summary>
    /// 管理員創建請求 DTO
    /// </summary>
    public class ManagerCreateDto
    {
        /// <summary>
        /// 管理員帳號
        /// </summary>
        [Required]
        [StringLength(50)]
        public string Username { get; set; } = string.Empty;

        /// <summary>
        /// 管理員密碼
        /// </summary>
        [Required]
        [StringLength(100)]
        public string Password { get; set; } = string.Empty;

        /// <summary>
        /// 管理員姓名
        /// </summary>
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// 管理員 Email
        /// </summary>
        [Required]
        [EmailAddress]
        [StringLength(100)]
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// 管理員角色
        /// </summary>
        [Required]
        [StringLength(50)]
        public string Role { get; set; } = string.Empty;
    }

    /// <summary>
    /// 管理員創建結果 DTO
    /// </summary>
    public class ManagerCreateResult
    {
        /// <summary>
        /// 是否創建成功
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// 錯誤訊息
        /// </summary>
        public string? ErrorMessage { get; set; }

        /// <summary>
        /// 創建的管理員資訊
        /// </summary>
        public ManagerDto? Manager { get; set; }
    }

    /// <summary>
    /// 管理員角色權限 DTO
    /// </summary>
    public class ManagerRolePermissionDto
    {
        /// <summary>
        /// 角色 ID
        /// </summary>
        public int RoleId { get; set; }

        /// <summary>
        /// 角色名稱
        /// </summary>
        public string RoleName { get; set; } = string.Empty;

        /// <summary>
        /// 權限列表
        /// </summary>
        public List<string> Permissions { get; set; } = new List<string>();
    }

    /// <summary>
    /// 管理員資料結果 DTO
    /// </summary>
    public class ManagerProfileResult
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
        /// 管理員資料
        /// </summary>
        public ManagerDto? Manager { get; set; }

        /// <summary>
        /// 角色權限
        /// </summary>
        public ManagerRolePermissionDto? RolePermission { get; set; }
    }
} 
