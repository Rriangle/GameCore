using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GameCore.Domain.Entities
{
    /// <summary>
    /// 管�??��??��??�實�?
    /// </summary>
    [Table("manager_role_permissions")]
    public partial class ManagerRolePermission
    {
        /// <summary>
        /// 權�?編�? (主鍵)
        /// </summary>
        [Key]
        [Column("permission_id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PermissionId { get; set; }

        /// <summary>
        /// 管�??�編??(外鍵)
        /// </summary>
        [Required]
        [Column("manager_id")]
        [ForeignKey("Manager")]
        public int ManagerId { get; set; }

        /// <summary>
        /// 權�??�稱
        /// </summary>
        [Required]
        [Column("permission_name")]
        [StringLength(100)]
        public string PermissionName { get; set; } = string.Empty;

        /// <summary>
        /// 權�??�述
        /// </summary>
        [Column("description")]
        [StringLength(500)]
        public string? Description { get; set; }

        /// <summary>
        /// ?�否?�用
        /// </summary>
        [Column("is_granted")]
        public bool IsGranted { get; set; } = true;

        /// <summary>
        /// 建�??��?
        /// </summary>
        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// ?�新?��?
        /// </summary>
        [Column("updated_at")]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // 導航屬�?
        public virtual Manager Manager { get; set; } = null!;
    }
} 
