using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GameCore.Core.Entities
{
    /// <summary>
    /// 玩家市場交易表 (對應 MarketTransactions)
    /// </summary>
    [Table("MarketTransactions")]
    public class MarketTransaction
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int SellerId { get; set; }

        public int? BuyerId { get; set; }

        [Required]
        [StringLength(200)]
        public string ItemName { get; set; } = string.Empty;

        public string? Description { get; set; }

        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public decimal Price { get; set; }

        [Required]
        public int Quantity { get; set; } = 1;

        [Required]
        [StringLength(20)]
        public string Status { get; set; } = "Listed"; // Listed, Sold, Cancelled, Completed

        [Column(TypeName = "decimal(10,2)")]
        public decimal? PlatformFee { get; set; }

        [Required]
        public bool IsActive { get; set; } = true;

        [Required]
        public DateTime CreateTime { get; set; } = DateTime.UtcNow;

        [Required]
        public DateTime UpdateTime { get; set; } = DateTime.UtcNow;

        // Navigation properties
        public virtual User Seller { get; set; } = null!;
        public virtual User? Buyer { get; set; }
        public virtual ICollection<MarketReview> Reviews { get; set; } = new List<MarketReview>();
    }

    /// <summary>
    /// 市場評價表 (對應 MarketReviews)
    /// </summary>
    [Table("MarketReviews")]
    public class MarketReview
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int TransactionId { get; set; }

        [Required]
        public int ReviewerId { get; set; }

        [Required]
        public int RevieweeId { get; set; }

        [Required]
        public int Rating { get; set; } // 1-5

        [StringLength(500)]
        public string? Comment { get; set; }

        [Required]
        public DateTime CreateTime { get; set; } = DateTime.UtcNow;

        // Navigation properties
        public virtual MarketTransaction Transaction { get; set; } = null!;
        public virtual User Reviewer { get; set; } = null!;
        public virtual User Reviewee { get; set; } = null!;
    }
}