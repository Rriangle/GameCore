using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GameCore.Domain.Entities
{
    /// <summary>
    /// 論�??�面實�?
    /// 每個�??��??��??��?壇�???
    /// </summary>
    [Table("forums")]
    public partial class Forum
    {
        /// <summary>
        /// 論�??�面ID（主?��?
        /// </summary>
        [Key]
        [Column("forum_id")]
        public int ForumId { get; set; }

        /// <summary>
        /// ?�聯?��??�ID（�??��?
        /// </summary>
        [Column("game_id")]
        public int GameId { get; set; }

        /// <summary>
        /// ?�面?�稱
        /// </summary>
        [Required]
        [Column("name")]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// ?�面說�?
        /// </summary>
        [Column("description")]
        [StringLength(500)]
        public string? Description { get; set; }

        /// <summary>
        /// 建�??��?
        /// </summary>
        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // 導航屬�?
        /// <summary>
        /// ?�聯?��???
        /// </summary>
        public virtual Game? Game { get; set; }

        /// <summary>
        /// ?�面下�?主�??�表
        /// </summary>
        public virtual ICollection<Thread> Threads { get; set; } = new List<Thread>();
    }
} 
