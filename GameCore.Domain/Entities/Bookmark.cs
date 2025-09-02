using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GameCore.Domain.Entities
{
    /// <summary>
    /// ?šç”¨?¶è?å¯¦é?
    /// ?¯æ´?¶è?ä¸»é??è²¼?‡ã€é??²ã€è?å£‡ç?
    /// </summary>
    [Table("bookmarks")]
    public class Bookmark
    {
        /// <summary>
        /// ?¶è?IDï¼ˆä¸»?µï?
        /// </summary>
        [Key]
        [Column("id")]
        public long Id { get; set; }

        /// <summary>
        /// ?¶è??…ç”¨?¶IDï¼ˆå??µï?
        /// </summary>
        [Column("user_id")]
        public int UserId { get; set; }

        /// <summary>
        /// ?®æ?é¡å?ï¼ˆpost/thread/game/forumï¼?
        /// </summary>
        [Required]
        [Column("target_type")]
        [StringLength(20)]
        public string TargetType { get; set; } = string.Empty;

        /// <summary>
        /// ?®æ?IDï¼ˆå??‹ï?ä¸è¨­FKï¼?
        /// </summary>
        [Column("target_id")]
        public long TargetId { get; set; }

        /// <summary>
        /// å»ºç??‚é?
        /// </summary>
        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // å°èˆªå±¬æ€?
        /// <summary>
        /// ?¶è???
        /// </summary>
        public virtual User? User { get; set; }
    }
} 
