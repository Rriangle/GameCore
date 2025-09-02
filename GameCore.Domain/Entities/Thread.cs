using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GameCore.Domain.Entities
{
    /// <summary>
    /// 論�?主�?實�?
    /// �?��論�??�面下�?討�?主�?
    /// </summary>
    [Table("threads")]
    public class Thread
    {
        /// <summary>
        /// 主�?ID（主?��?
        /// </summary>
        [Key]
        [Column("thread_id")]
        public long ThreadId { get; set; }

        /// <summary>
        /// ?�屬�?壇�??�ID（�??��?
        /// </summary>
        [Column("forum_id")]
        public int ForumId { get; set; }

        /// <summary>
        /// 作者用?�ID（�??��?
        /// </summary>
        [Column("author_user_id")]
        public int AuthorUserId { get; set; }

        /// <summary>
        /// 主�?標�?
        /// </summary>
        [Required]
        [Column("title")]
        [StringLength(200)]
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// 主�??�?��?normal/hidden/archived�?
        /// </summary>
        [Column("status")]
        [StringLength(20)]
        public string Status { get; set; } = "normal";

        /// <summary>
        /// 建�??��?
        /// </summary>
        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// ?�後更?��???
        /// </summary>
        [Column("updated_at")]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // 導航屬�?
        /// <summary>
        /// ?�屬�?壇�???
        /// </summary>
        public virtual Forum? Forum { get; set; }

        /// <summary>
        /// 主�?作�?
        /// </summary>
        public virtual User? Author { get; set; }

        /// <summary>
        /// 主�?下�??��??�表
        /// </summary>
        public virtual ICollection<ThreadPost> Posts { get; set; } = new List<ThreadPost>();

        /// <summary>
        /// 主�??��??��?�?
        /// </summary>
        public virtual ICollection<Reaction> Reactions { get; set; } = new List<Reaction>();

        /// <summary>
        /// 主�??�收?��?�?
        /// </summary>
        public virtual ICollection<Bookmark> Bookmarks { get; set; } = new List<Bookmark>();
    }
} 
