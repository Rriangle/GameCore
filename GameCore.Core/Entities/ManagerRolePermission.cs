using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GameCore.Core.Entities
{
    /// <summary>
    /// 管理員角色權限實體
    /// </summary>
    [Table("manager_role_permissions")]
    public class ManagerRolePermission
    {
        /// <summary>
        /// 權限編號 (主鍵)
        /// </summary>
        [Key]
        [Column("permission_id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PermissionId { get; set; }

        /// <summary>
        /// 管理員編號 (外鍵)
        /// </summary>
        [Required]
        [Column("manager_id")]
        [ForeignKey("Manager")]
        public int ManagerId { get; set; }

        /// <summary>
        /// 權限名稱
        /// </summary>
        [Required]
        [Column("permission_name")]
        [StringLength(100)]
        public string PermissionName { get; set; } = string.Empty;

        /// <summary>
        /// 權限描述
        /// </summary>
        [Column("description")]
        [StringLength(500)]
        public string? Description { get; set; }

        /// <summary>
        /// 是否啟用
        /// </summary>
        [Column("is_granted")]
        public bool IsGranted { get; set; } = true;

        /// <summary>
        /// 建立時間
        /// </summary>
        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// 更新時間
        /// </summary>
        [Column("updated_at")]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // 導航屬性
        public virtual Manager Manager { get; set; } = null!;
    }
} 