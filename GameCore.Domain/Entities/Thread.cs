using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GameCore.Domain.Entities
{
    /// <summary>
    /// è«–å?ä¸»é?å¯¦é?
    /// ä»?¡¨è«–å??ˆé¢ä¸‹ç?è¨è?ä¸»é?
    /// </summary>
    [Table("threads")]
    public class Thread
    {
        /// <summary>
        /// ä¸»é?IDï¼ˆä¸»?µï?
        /// </summary>
        [Key]
        [Column("thread_id")]
        public long ThreadId { get; set; }

        /// <summary>
        /// ?€å±¬è?å£‡ç??¢IDï¼ˆå??µï?
        /// </summary>
        [Column("forum_id")]
        public int ForumId { get; set; }

        /// <summary>
        /// ä½œè€…ç”¨?¶IDï¼ˆå??µï?
        /// </summary>
        [Column("author_user_id")]
        public int AuthorUserId { get; set; }

        /// <summary>
        /// ä¸»é?æ¨™é?
        /// </summary>
        [Required]
        [Column("title")]
        [StringLength(200)]
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// ä¸»é??€?‹ï?normal/hidden/archivedï¼?
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
        /// ?€å±¬è?å£‡ç???
        /// </summary>
        public virtual Forum? Forum { get; set; }

        /// <summary>
        /// ä¸»é?ä½œè€?
        /// </summary>
        public virtual User? Author { get; set; }

        /// <summary>
        /// ä¸»é?ä¸‹ç??è??—è¡¨
        /// </summary>
        public virtual ICollection<ThreadPost> Posts { get; set; } = new List<ThreadPost>();

        /// <summary>
        /// ä¸»é??„å??‰å?è¡?
        /// </summary>
        public virtual ICollection<Reaction> Reactions { get; set; } = new List<Reaction>();

        /// <summary>
        /// ä¸»é??„æ”¶?å?è¡?
        /// </summary>
        public virtual ICollection<Bookmark> Bookmarks { get; set; } = new List<Bookmark>();
    }
} 
