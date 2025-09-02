using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GameCore.Domain.Entities
{
    /// <summary>
    /// 主�??��?實�?
    /// ?�援二層?��?結�?
    /// </summary>
    [Table("thread_posts")]
    public class ThreadPost
    {
        /// <summary>
        /// ?��?ID（主?��?
        /// </summary>
        [Key]
        [Column("id")]
        public long Id { get; set; }

        /// <summary>
        /// ?�屬主題ID（�??��?
        /// </summary>
        [Column("thread_id")]
        public long ThreadId { get; set; }

        /// <summary>
        /// ?��??�用?�ID（�??��?
        /// </summary>
        [Column("author_user_id")]
        public int AuthorUserId { get; set; }

        /// <summary>
        /// ?��??�容（Markdown?��?�?
        /// </summary>
        [Required]
        [Column("content_md")]
        public string ContentMd { get; set; } = string.Empty;

        /// <summary>
        /// ?��?覆ID（支?��?層�?構�?
        /// </summary>
        [Column("parent_post_id")]
        public long? ParentPostId { get; set; }

        /// <summary>
        /// ?��??�?��?normal/hidden/deleted�?
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
        /// ?�屬主�?
        /// </summary>
        public virtual Thread? Thread { get; set; }

        /// <summary>
        /// ?��?作�?
        /// </summary>
        public virtual User? Author { get; set; }

        /// <summary>
        /// ?��?覆�?如�??��?話�?
        /// </summary>
        public virtual ThreadPost? ParentPost { get; set; }

        /// <summary>
        /// 子�?覆�?�?
        /// </summary>
        public virtual ICollection<ThreadPost> ChildPosts { get; set; } = new List<ThreadPost>();

        /// <summary>
        /// ?��??��??��?�?
        /// </summary>
        public virtual ICollection<Reaction> Reactions { get; set; } = new List<Reaction>();
    }
} 
