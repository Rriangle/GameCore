using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GameCore.Domain.Entities
{
    /// <summary>
    /// ?��?實�?
    /// ?�於論�?主�??��?覆�???
    /// </summary>
    [Table("replies")]
    public partial class Reply
    {
        /// <summary>
        /// ?��?ID（主?��?
        /// </summary>
        [Key]
        [Column("reply_id")]
        public int ReplyId { get; set; }

        /// <summary>
        /// 主�?ID（�??��?
        /// </summary>
        [Column("thread_id")]
        public long ThreadId { get; set; }

        /// <summary>
        /// 作者用?�ID（�??��?
        /// </summary>
        [Column("author_user_id")]
        public int AuthorUserId { get; set; }

        /// <summary>
        /// ?��??�容
        /// </summary>
        [Column("content")]
        [StringLength(4000)]
        public string Content { get; set; } = string.Empty;

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
        /// ?�新?��?
        /// </summary>
        [Column("updated_at")]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // 導航屬�?
        /// <summary>
        /// ?��??�屬�?主�?
        /// </summary>
        public virtual Thread? Thread { get; set; }

        /// <summary>
        /// ?��??��???
        /// </summary>
        public virtual User? Author { get; set; }
    }
} 
