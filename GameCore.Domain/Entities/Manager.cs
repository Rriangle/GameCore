using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GameCore.Domain.Entities
{
    /// <summary>
    /// 管??實?
    /// </summary>
    [Table("managers")]
    public partial class Manager
    {
        /// <summary>
        /// 管??編??(主鍵)
        /// </summary>
        [Key]
        [Column("manager_id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ManagerId { get; set; }

        /// <summary>
        /// 使用?��?�?
        /// </summary>
        [Required]
        [Column("username")]
        [StringLength(50)]
        public string Username { get; set; } = string.Empty;

        /// <summary>
        /// ?��??�件
        /// </summary>
        [Required]
        [Column("email")]
        [StringLength(100)]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// 密碼?��?
        /// </summary>
        [Required]
        [Column("password_hash")]
        public string PasswordHash { get; set; } = string.Empty;

        /// <summary>
        /// ?��?
        /// </summary>
        [Required]
        [Column("full_name")]
        [StringLength(100)]
        public string FullName { get; set; } = string.Empty;

        /// <summary>
        /// 角色
        /// </summary>
        [Required]
        [Column("role")]
        [StringLength(50)]
        public string Role { get; set; } = string.Empty;

        /// <summary>
        /// ?��?
        /// </summary>
        [Column("department")]
        [StringLength(50)]
        public string? Department { get; set; }

        /// <summary>
        /// ?�否?�用
        /// </summary>
        [Column("is_active")]
        public bool IsActive { get; set; } = true;

        /// <summary>
        /// ?�後登?��???
        /// </summary>
        [Column("last_login_at")]
        public DateTime? LastLoginAt { get; set; }

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
        public virtual ICollection<ManagerRolePermission> RolePermissions { get; set; } = new List<ManagerRolePermission>();
    }



    /// <summary>
    /// 管?????派表 (多?多???
    /// </summary>
    [Table("ManagerRole")]
    public partial class ManagerRole
    {
        /// <summary>
        /// 管??編??(外鍵??ManagerData)
        /// </summary>
        [Column("Manager_Id")]
        [ForeignKey("ManagerData")]
        public int ManagerId { get; set; }

        /// <summary>
        /// 角色編�? (外鍵??ManagerRolePermission)
        /// </summary>
        [Column("ManagerRole_Id")]
        [ForeignKey("ManagerRolePermission")]
        public int ManagerRoleId { get; set; }

        /// <summary>
        /// 角色?�稱
        /// </summary>
        [Column("ManagerRole")]
        [StringLength(100)]
        public string? ManagerRoleName { get; set; }

        // 導航屬�?
        public virtual ManagerData ManagerData { get; set; } = null!;
        public virtual ManagerRolePermission ManagerRolePermission { get; set; } = null!;
    }

    /// <summary>
    /// 後台管??表 (?入追蹤)
    /// </summary>
    [Table("Admins")]
    public class Admin
    {
        /// <summary>
        /// 管??編??(主鍵, 外鍵??ManagerRole)
        /// </summary>
        [Key]
        [Column("manager_id")]
        [ForeignKey("ManagerData")]
        public int ManagerId { get; set; }

        /// <summary>
        /// 上次?�入?��? (?�於後台?�入追蹤)
        /// </summary>
        [Column("last_login")]
        public DateTime? LastLogin { get; set; }

        // 導航屬�?
        public virtual ManagerData ManagerData { get; set; } = null!;
    }

    /// <summary>
    /// 禁????
    /// </summary>
    [Table("Mutes")]
    public class Mute
    {
        /// <summary>
        /// 禁???編? (主鍵)
        /// </summary>
        [Key]
        [Column("mute_id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int MuteId { get; set; }

        /// <summary>
        /// 禁??稱
        /// </summary>
        [Column("mute_name")]
        [StringLength(100)]
        public string? MuteName { get; set; }

        /// <summary>
        /// 建???
        /// </summary>
        [Column("created_at")]
        public DateTime? CreatedAt { get; set; }

        /// <summary>
        /// ?�否?�用
        /// </summary>
        [Required]
        [Column("is_active")]
        public bool IsActive { get; set; } = true;

        /// <summary>
        /// 設置?�編??(外鍵??ManagerRole)
        /// </summary>
        [Column("manager_id")]
        [ForeignKey("ManagerData")]
        public int? ManagerId { get; set; }

        // 導航屬�?
        public virtual ManagerData? ManagerData { get; set; }
    }

    /// <summary>
    /// �??�?
    /// </summary>
    [Table("Styles")]
    public class Style
    {
        /// <summary>
        /// �??編�? (主鍵)
        /// </summary>
        [Key]
        [Column("style_id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int StyleId { get; set; }

        /// <summary>
        /// �???�稱
        /// </summary>
        [Column("style_name")]
        [StringLength(100)]
        public string? StyleName { get; set; }

        /// <summary>
        /// ?��?說�?
        /// </summary>
        [Column("effect_desc")]
        [StringLength(200)]
        public string? EffectDesc { get; set; }

        /// <summary>
        /// 建�??��?
        /// </summary>
        [Column("created_at")]
        public DateTime? CreatedAt { get; set; }

        /// <summary>
        /// 設置?�編??(外鍵??ManagerRole)
        /// </summary>
        [Column("manager_id")]
        [ForeignKey("ManagerData")]
        public int? ManagerId { get; set; }

        // 導航屬�?
        public virtual ManagerData? ManagerData { get; set; }
    }
}
