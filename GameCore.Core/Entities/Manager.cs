using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GameCore.Core.Entities
{
    /// <summary>
    /// 管理者資料表
    /// </summary>
    [Table("ManagerData")]
    public class ManagerData
    {
        /// <summary>
        /// 管理者編號 (主鍵)
        /// </summary>
        [Key]
        [Column("Manager_Id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ManagerId { get; set; }

        /// <summary>
        /// 管理者姓名
        /// </summary>
        [Column("Manager_Name")]
        [StringLength(100)]
        public string? ManagerName { get; set; }

        /// <summary>
        /// 管理者帳號 (建議唯一)
        /// </summary>
        [Column("Manager_Account")]
        [StringLength(100)]
        public string? ManagerAccount { get; set; }

        /// <summary>
        /// 管理者密碼 (實務請存雜湊)
        /// </summary>
        [Column("Manager_Password")]
        [StringLength(255)]
        public string? ManagerPassword { get; set; }

        /// <summary>
        /// 管理者註冊時間
        /// </summary>
        [Column("Administrator_registration_date")]
        public DateTime? AdministratorRegistrationDate { get; set; }

        // 導航屬性
        public virtual ICollection<ManagerRole> ManagerRoles { get; set; } = new List<ManagerRole>();
        public virtual Admin? Admin { get; set; }
        public virtual ICollection<Mute> Mutes { get; set; } = new List<Mute>();
        public virtual ICollection<Style> Styles { get; set; } = new List<Style>();
        public virtual ICollection<Notification> SentNotifications { get; set; } = new List<Notification>();
        public virtual ICollection<ChatMessage> ChatMessages { get; set; } = new List<ChatMessage>();
        public virtual ICollection<ProductInfoAuditLog> AuditLogs { get; set; } = new List<ProductInfoAuditLog>();
    }

    /// <summary>
    /// 角色權限定義表
    /// </summary>
    [Table("ManagerRolePermission")]
    public class ManagerRolePermission
    {
        /// <summary>
        /// 管理者角色編號 (主鍵)
        /// </summary>
        [Key]
        [Column("ManagerRole_Id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ManagerRoleId { get; set; }

        /// <summary>
        /// 角色名稱 (顯示名稱 例如: 商城管理員)
        /// </summary>
        [Required]
        [Column("role_name")]
        [StringLength(100)]
        public string RoleName { get; set; } = string.Empty;

        /// <summary>
        /// 管理者權限管理
        /// </summary>
        [Column("AdministratorPrivilegesManagement")]
        public bool AdministratorPrivilegesManagement { get; set; } = false;

        /// <summary>
        /// 使用者狀態管理
        /// </summary>
        [Column("UserStatusManagement")]
        public bool UserStatusManagement { get; set; } = false;

        /// <summary>
        /// 商城權限管理
        /// </summary>
        [Column("ShoppingPermissionManagement")]
        public bool ShoppingPermissionManagement { get; set; } = false;

        /// <summary>
        /// 論壇權限管理
        /// </summary>
        [Column("MessagePermissionManagement")]
        public bool MessagePermissionManagement { get; set; } = false;

        /// <summary>
        /// 寵物權限管理
        /// </summary>
        [Column("Pet_Rights_Management")]
        public bool PetRightsManagement { get; set; } = false;

        /// <summary>
        /// 客服權限管理
        /// </summary>
        [Column("customer_service")]
        public bool CustomerService { get; set; } = false;

        // 導航屬性
        public virtual ICollection<ManagerRole> ManagerRoles { get; set; } = new List<ManagerRole>();
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