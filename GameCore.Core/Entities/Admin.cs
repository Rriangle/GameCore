using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GameCore.Core.Entities
{
    /// <summary>
    /// 後台管理員表
    /// </summary>
    [Table("Admins")]
    public class Admin
    {
        /// <summary>
        /// 管理員ID (主鍵，外鍵到ManagerRole)
        /// </summary>
        [Key]
        [ForeignKey("ManagerData")]
        public int manager_id { get; set; }

        /// <summary>
        /// 上次登入時間，用於後台登入追蹤
        /// </summary>
        public DateTime? last_login { get; set; }

        /// <summary>
        /// 最後登入IP
        /// </summary>
        [StringLength(45)]
        public string? last_login_ip { get; set; }

        /// <summary>
        /// 登入次數
        /// </summary>
        public int login_count { get; set; } = 0;

        /// <summary>
        /// 是否在線
        /// </summary>
        public bool is_online { get; set; } = false;

        /// <summary>
        /// 最後活動時間
        /// </summary>
        public DateTime? last_activity { get; set; }

        /// <summary>
        /// 會話ID
        /// </summary>
        [StringLength(100)]
        public string? session_id { get; set; }

        /// <summary>
        /// 建立時間
        /// </summary>
        public DateTime created_at { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// 更新時間
        /// </summary>
        public DateTime updated_at { get; set; } = DateTime.UtcNow;

        // 導航屬性
        /// <summary>
        /// 管理員資料
        /// </summary>
        public virtual ManagerData ManagerData { get; set; } = null!;
    }
}