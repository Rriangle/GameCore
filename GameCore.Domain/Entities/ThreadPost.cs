using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GameCore.Domain.Entities
{
    /// <summary>
    /// ä¸»é??è?å¯¦é?
    /// ?¯æ´äºŒå±¤?è?çµæ?
    /// </summary>
    [Table("thread_posts")]
    public class ThreadPost
    {
        /// <summary>
        /// ?è?IDï¼ˆä¸»?µï?
        /// </summary>
        [Key]
        [Column("id")]
        public long Id { get; set; }

        /// <summary>
        /// ?€å±¬ä¸»é¡ŒIDï¼ˆå??µï?
        /// </summary>
        [Column("thread_id")]
        public long ThreadId { get; set; }

        /// <summary>
        /// ?è??…ç”¨?¶IDï¼ˆå??µï?
        /// </summary>
        [Column("author_user_id")]
        public int AuthorUserId { get; set; }

        /// <summary>
        /// ?è??§å®¹ï¼ˆMarkdown?¼å?ï¼?
        /// </summary>
        [Required]
        [Column("content_md")]
        public string ContentMd { get; set; } = string.Empty;

        /// <summary>
        /// ?¶å?è¦†IDï¼ˆæ”¯?´ä?å±¤ç?æ§‹ï?
        /// </summary>
        [Column("parent_post_id")]
        public long? ParentPostId { get; set; }

        /// <summary>
        /// ?è??€?‹ï?normal/hidden/deletedï¼?
        /// </summary>
        [Column("status")]
        [StringLength(20)]
        public string Status { get; set; } = "normal";

        /// <summary>
        /// å»ºç??‚é?
        /// </summary>
        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// ?€å¾Œæ›´?°æ???
        /// </summary>
        [Column("updated_at")]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // å°èˆªå±¬æ€?
        /// <summary>
        /// ?€å±¬ä¸»é¡?
        /// </summary>
        public virtual Thread? Thread { get; set; }

        /// <summary>
        /// ?è?ä½œè€?
        /// </summary>
        public virtual User? Author { get; set; }

        /// <summary>
        /// ?¶å?è¦†ï?å¦‚æ??‰ç?è©±ï?
        /// </summary>
        public virtual ThreadPost? ParentPost { get; set; }

        /// <summary>
        /// å­å?è¦†å?è¡?
        /// </summary>
        public virtual ICollection<ThreadPost> ChildPosts { get; set; } = new List<ThreadPost>();

        /// <summary>
        /// ?è??„å??‰å?è¡?
        /// </summary>
        public virtual ICollection<Reaction> Reactions { get; set; } = new List<Reaction>();
    }
} 
