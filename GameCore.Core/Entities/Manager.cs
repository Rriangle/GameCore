using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using GameCore.Core.Enums;

namespace GameCore.Core.Entities
{
    /// <summary>
    /// 管理員實體
    /// </summary>
    [Table("managers")]
    public class Manager
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [Column("username", TypeName = "varchar(50)")]
        public string Username { get; set; } = string.Empty;

        [Required]
        [Column("email", TypeName = "varchar(100)")]
        public string Email { get; set; } = string.Empty;

        [Required]
        [Column("password_hash", TypeName = "varchar(255)")]
        public string PasswordHash { get; set; } = string.Empty;

        [Column("role")]
        public ManagerRole Role { get; set; } = ManagerRole.Manager;

        [Column("is_active")]
        public bool IsActive { get; set; } = true;

        [Column("last_login_at")]
        public DateTime? LastLoginAt { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Column("updated_at")]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // 導航屬性
        public virtual ICollection<ManagerRolePermission> RolePermissions { get; set; } = new List<ManagerRolePermission>();
    }

    /// <summary>
    /// 管理員角色權限實體
    /// </summary>
    [Table("manager_role_permissions")]
    public class ManagerRolePermission
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("manager_id")]
        public int ManagerId { get; set; }

        [Column("permission_name", TypeName = "varchar(100)")]
        public string PermissionName { get; set; } = string.Empty;

        [Column("permission_value", TypeName = "varchar(255)")]
        public string PermissionValue { get; set; } = string.Empty;

        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // 導航屬性
        [ForeignKey("ManagerId")]
        public virtual Manager Manager { get; set; } = null!;
    }
}