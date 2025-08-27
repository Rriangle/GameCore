using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GameCore.Core.Entities
{
    /// <summary>
    /// 管理員實體
    /// </summary>
    [Table("managers")]
    public class Manager
    {
        /// <summary>
        /// 管理員編號 (主鍵)
        /// </summary>
        [Key]
        [Column("manager_id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ManagerId { get; set; }

        /// <summary>
        /// 使用者名稱
        /// </summary>
        [Required]
        [Column("username")]
        [StringLength(50)]
        public string Username { get; set; } = string.Empty;

        /// <summary>
        /// 電子郵件
        /// </summary>
        [Required]
        [Column("email")]
        [StringLength(100)]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// 密碼雜湊
        /// </summary>
        [Required]
        [Column("password_hash")]
        public string PasswordHash { get; set; } = string.Empty;

        /// <summary>
        /// 全名
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
        /// 部門
        /// </summary>
        [Column("department")]
        [StringLength(50)]
        public string? Department { get; set; }

        /// <summary>
        /// 是否啟用
        /// </summary>
        [Column("is_active")]
        public bool IsActive { get; set; } = true;

        /// <summary>
        /// 最後登入時間
        /// </summary>
        [Column("last_login_at")]
        public DateTime? LastLoginAt { get; set; }

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
        public virtual ICollection<ManagerRolePermission> RolePermissions { get; set; } = new List<ManagerRolePermission>();
    }



    /// <summary>
    /// 管理者角色指派表 (多對多關聯)
    /// </summary>
    [Table("ManagerRole")]
    public class ManagerRole
    {
        /// <summary>
        /// 管理者編號 (外鍵到 ManagerData)
        /// </summary>
        [Column("Manager_Id")]
        [ForeignKey("ManagerData")]
        public int ManagerId { get; set; }

        /// <summary>
        /// 角色編號 (外鍵到 ManagerRolePermission)
        /// </summary>
        [Column("ManagerRole_Id")]
        [ForeignKey("ManagerRolePermission")]
        public int ManagerRoleId { get; set; }

        /// <summary>
        /// 角色名稱
        /// </summary>
        [Column("ManagerRole")]
        [StringLength(100)]
        public string? ManagerRoleName { get; set; }

        // 導航屬性
        public virtual ManagerData ManagerData { get; set; } = null!;
        public virtual ManagerRolePermission ManagerRolePermission { get; set; } = null!;
    }

    /// <summary>
    /// 後台管理員表 (登入追蹤)
    /// </summary>
    [Table("Admins")]
    public class Admin
    {
        /// <summary>
        /// 管理員編號 (主鍵, 外鍵到 ManagerRole)
        /// </summary>
        [Key]
        [Column("manager_id")]
        [ForeignKey("ManagerData")]
        public int ManagerId { get; set; }

        /// <summary>
        /// 上次登入時間 (用於後台登入追蹤)
        /// </summary>
        [Column("last_login")]
        public DateTime? LastLogin { get; set; }

        // 導航屬性
        public virtual ManagerData ManagerData { get; set; } = null!;
    }

    /// <summary>
    /// 禁言選項表
    /// </summary>
    [Table("Mutes")]
    public class Mute
    {
        /// <summary>
        /// 禁言選項編號 (主鍵)
        /// </summary>
        [Key]
        [Column("mute_id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int MuteId { get; set; }

        /// <summary>
        /// 禁言名稱
        /// </summary>
        [Column("mute_name")]
        [StringLength(100)]
        public string? MuteName { get; set; }

        /// <summary>
        /// 建立時間
        /// </summary>
        [Column("created_at")]
        public DateTime? CreatedAt { get; set; }

        /// <summary>
        /// 是否啟用
        /// </summary>
        [Required]
        [Column("is_active")]
        public bool IsActive { get; set; } = true;

        /// <summary>
        /// 設置者編號 (外鍵到 ManagerRole)
        /// </summary>
        [Column("manager_id")]
        [ForeignKey("ManagerData")]
        public int? ManagerId { get; set; }

        // 導航屬性
        public virtual ManagerData? ManagerData { get; set; }
    }

    /// <summary>
    /// 樣式表
    /// </summary>
    [Table("Styles")]
    public class Style
    {
        /// <summary>
        /// 樣式編號 (主鍵)
        /// </summary>
        [Key]
        [Column("style_id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int StyleId { get; set; }

        /// <summary>
        /// 樣式名稱
        /// </summary>
        [Column("style_name")]
        [StringLength(100)]
        public string? StyleName { get; set; }

        /// <summary>
        /// 效果說明
        /// </summary>
        [Column("effect_desc")]
        [StringLength(200)]
        public string? EffectDesc { get; set; }

        /// <summary>
        /// 建立時間
        /// </summary>
        [Column("created_at")]
        public DateTime? CreatedAt { get; set; }

        /// <summary>
        /// 設置者編號 (外鍵到 ManagerRole)
        /// </summary>
        [Column("manager_id")]
        [ForeignKey("ManagerData")]
        public int? ManagerId { get; set; }

        // 導航屬性
        public virtual ManagerData? ManagerData { get; set; }
    }
}