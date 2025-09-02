using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GameCore.Domain.Entities
{
    /// <summary>
    /// ?��??��?實�?
    /// </summary>
    [Table("post_replies")]
    public partial class PostReply
    {
        /// <summary>
        /// ?��?編�? (主鍵)
        /// </summary>
        [Key]
        [Column("reply_id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ReplyId { get; set; }

        /// <summary>
        /// ?��?編�? (外鍵)
        /// </summary>
        [Required]
        [Column("post_id")]
        [ForeignKey("Post")]
        public int PostId { get; set; }

        /// <summary>
        /// ?��??�編??(外鍵)
        /// </summary>
        [Required]
        [Column("replier_id")]
        [ForeignKey("Replier")]
        public int ReplierId { get; set; }

        /// <summary>
        /// ?��?覆編??(外鍵，可?��??�於巢�??��?)
        /// </summary>
        [Column("parent_reply_id")]
        [ForeignKey("ParentReply")]
        public int? ParentReplyId { get; set; }

        /// <summary>
        /// ?��??�容
        /// </summary>
        [Required]
        [Column("content")]
        public string Content { get; set; } = string.Empty;

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
        public virtual Post Post { get; set; } = null!;
        public virtual User Replier { get; set; } = null!;
        public virtual PostReply? ParentReply { get; set; }
        public virtual ICollection<PostReply> ChildReplies { get; set; } = new List<PostReply>();
    }
} 
