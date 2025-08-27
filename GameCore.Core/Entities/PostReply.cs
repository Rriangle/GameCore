using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GameCore.Core.Entities
{
    /// <summary>
    /// 文章回覆實體
    /// </summary>
    [Table("post_replies")]
    public class PostReply
    {
        /// <summary>
        /// 回覆編號 (主鍵)
        /// </summary>
        [Key]
        [Column("reply_id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ReplyId { get; set; }

        /// <summary>
        /// 文章編號 (外鍵)
        /// </summary>
        [Required]
        [Column("post_id")]
        [ForeignKey("Post")]
        public int PostId { get; set; }

        /// <summary>
        /// 回覆者編號 (外鍵)
        /// </summary>
        [Required]
        [Column("replier_id")]
        [ForeignKey("Replier")]
        public int ReplierId { get; set; }

        /// <summary>
        /// 父回覆編號 (外鍵，可選，用於巢狀回覆)
        /// </summary>
        [Column("parent_reply_id")]
        [ForeignKey("ParentReply")]
        public int? ParentReplyId { get; set; }

        /// <summary>
        /// 回覆內容
        /// </summary>
        [Required]
        [Column("content")]
        public string Content { get; set; } = string.Empty;

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
        public virtual Post Post { get; set; } = null!;
        public virtual User Replier { get; set; } = null!;
        public virtual PostReply? ParentReply { get; set; }
        public virtual ICollection<PostReply> ChildReplies { get; set; } = new List<PostReply>();
    }
} 