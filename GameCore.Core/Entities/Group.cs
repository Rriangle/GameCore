using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GameCore.Core.Entities
{
    /// <summary>
    /// 群組表
    /// </summary>
    [Table("Groups")]
    public class Group
    {
        /// <summary>
        /// 群組ID (主鍵，自動遞增)
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int group_id { get; set; }

        /// <summary>
        /// 群組名稱
        /// </summary>
        [Required]
        [StringLength(200)]
        public string group_name { get; set; } = string.Empty;

        /// <summary>
        /// 建立者ID (外鍵到Users)
        /// </summary>
        [Required]
        [ForeignKey("CreatedBy")]
        public int created_by { get; set; }

        /// <summary>
        /// 建立時間
        /// </summary>
        public DateTime created_at { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// 群組描述
        /// </summary>
        [StringLength(1000)]
        public string? description { get; set; }

        /// <summary>
        /// 群組頭像URL
        /// </summary>
        [StringLength(500)]
        public string? avatar_url { get; set; }

        /// <summary>
        /// 群組類型 (public/private/secret)
        /// </summary>
        [StringLength(50)]
        public string group_type { get; set; } = "public";

        /// <summary>
        /// 最大成員數
        /// </summary>
        public int max_members { get; set; } = 1000;

        /// <summary>
        /// 是否啟用
        /// </summary>
        public bool is_active { get; set; } = true;

        /// <summary>
        /// 群組規則
        /// </summary>
        [StringLength(2000)]
        public string? rules { get; set; }

        /// <summary>
        /// 標籤
        /// </summary>
        [StringLength(500)]
        public string? tags { get; set; }

        // 導航屬性
        /// <summary>
        /// 建立者
        /// </summary>
        public virtual User CreatedBy { get; set; } = null!;

        /// <summary>
        /// 群組成員
        /// </summary>
        public virtual ICollection<GroupMember> GroupMembers { get; set; } = new List<GroupMember>();

        /// <summary>
        /// 群組聊天
        /// </summary>
        public virtual ICollection<GroupChat> GroupChats { get; set; } = new List<GroupChat>();

        /// <summary>
        /// 群組封鎖
        /// </summary>
        public virtual ICollection<GroupBlock> GroupBlocks { get; set; } = new List<GroupBlock>();

        /// <summary>
        /// 通知
        /// </summary>
        public virtual ICollection<Notification> Notifications { get; set; } = new List<Notification>();
    }
}