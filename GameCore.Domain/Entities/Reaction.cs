using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GameCore.Domain.Entities
{
    /// <summary>
    /// ?šç”¨?æ?å¯¦é?
    /// ?¯æ´å°ä¸»é¡Œã€å?è¦†ã€è²¼?‡ç??²è??æ?ï¼ˆè??è¡¨?…ç?ï¼?
    /// </summary>
    [Table("reactions")]
    public class Reaction
    {
        /// <summary>
        /// ?æ?IDï¼ˆä¸»?µï?
        /// </summary>
        [Key]
        [Column("id")]
        public long Id { get; set; }

        /// <summary>
        /// ?æ??…ç”¨?¶IDï¼ˆå??µï?
        /// </summary>
        [Column("user_id")]
        public int UserId { get; set; }

        /// <summary>
        /// ?®æ?é¡å?ï¼ˆpost/thread/thread_postï¼?
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
        /// ?æ?é¡å?ï¼ˆlike/emojiç­‰ï?
        /// </summary>
        [Required]
        [Column("kind")]
        [StringLength(20)]
        public string Kind { get; set; } = string.Empty;

        /// <summary>
        /// å»ºç??‚é?
        /// </summary>
        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // å°èˆªå±¬æ€?
        /// <summary>
        /// ?æ???
        /// </summary>
        public virtual User? User { get; set; }
    }
} 
