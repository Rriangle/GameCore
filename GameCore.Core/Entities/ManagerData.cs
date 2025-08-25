using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GameCore.Core.Entities
{
    /// <summary>
    /// 管理者資料表 (主表)
    /// </summary>
    [Table("ManagerData")]
    public class ManagerData
    {
        /// <summary>
        /// 管理者編號 (主鍵)
        /// </summary>
        [Key]
        public int Manager_Id { get; set; }

        /// <summary>
        /// 管理者姓名
        /// </summary>
        [Required]
        [StringLength(100)]
        public string Manager_Name { get; set; } = string.Empty;

        /// <summary>
        /// 管理者帳號 (建議唯一)
        /// </summary>
        [Required]
        [StringLength(100)]
        public string Manager_Account { get; set; } = string.Empty;

        /// <summary>
        /// 管理者密碼 (實務請存雜湊)
        /// </summary>
        [Required]
        [StringLength(255)]
        public string Manager_Password { get; set; } = string.Empty;

        /// <summary>
        /// 管理者註冊時間
        /// </summary>
        public DateTime Administrator_registration_date { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// 電子郵件
        /// </summary>
        [StringLength(100)]
        [EmailAddress]
        public string? Email { get; set; }

        /// <summary>
        /// 手機號碼
        /// </summary>
        [StringLength(20)]
        public string? Phone { get; set; }

        /// <summary>
        /// 是否啟用
        /// </summary>
        public bool IsActive { get; set; } = true;

        /// <summary>
        /// 最後登入時間
        /// </summary>
        public DateTime? LastLoginAt { get; set; }

        /// <summary>
        /// 最後登入IP
        /// </summary>
        [StringLength(45)]
        public string? LastLoginIp { get; set; }

        /// <summary>
        /// 備註
        /// </summary>
        [StringLength(500)]
        public string? Notes { get; set; }

        // 導航屬性
        /// <summary>
        /// 管理者角色
        /// </summary>
        public virtual ICollection<ManagerRole> ManagerRoles { get; set; } = new List<ManagerRole>();

        /// <summary>
        /// 管理員登入追蹤
        /// </summary>
        public virtual Admin? Admin { get; set; }

        /// <summary>
        /// 禁言設定
        /// </summary>
        public virtual ICollection<Mute> Mutes { get; set; } = new List<Mute>();

        /// <summary>
        /// 樣式設定
        /// </summary>
        public virtual ICollection<Style> Styles { get; set; } = new List<Style>();

        /// <summary>
        /// 發送的通知
        /// </summary>
        public virtual ICollection<Notification> Notifications { get; set; } = new List<Notification>();

        /// <summary>
        /// 聊天訊息 (客服)
        /// </summary>
        public virtual ICollection<ChatMessage> ChatMessages { get; set; } = new List<ChatMessage>();

        /// <summary>
        /// 商品審計日誌
        /// </summary>
        public virtual ICollection<ProductInfoAuditLog> ProductAuditLogs { get; set; } = new List<ProductInfoAuditLog>();
    }
}