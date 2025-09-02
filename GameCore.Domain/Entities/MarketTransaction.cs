using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GameCore.Domain.Entities
{
    /// <summary>
    /// 市場交�?實�?
    /// ?�於管�??�家市場?�交?��???
    /// </summary>
    [Table("market_transactions")]
    public partial class MarketTransaction
    {
        /// <summary>
        /// 交�?ID（主?��?
        /// </summary>
        [Key]
        [Column("id")]
        public int Id { get; set; }

        /// <summary>
        /// 市場?��?ID（�??��?
        /// </summary>
        [Column("market_item_id")]
        public int MarketItemId { get; set; }

        /// <summary>
        /// 買家ID（�??�到Users�?
        /// </summary>
        [Column("buyer_id")]
        public int BuyerId { get; set; }

        /// <summary>
        /// �?��ID（�??�到Users�?
        /// </summary>
        [Column("seller_id")]
        public int SellerId { get; set; }

        /// <summary>
        /// 交�??��?
        /// </summary>
        [Column("amount")]
        public decimal Amount { get; set; }

        /// <summary>
        /// 交�??�??
        /// </summary>
        [Column("status")]
        [StringLength(50)]
        public string Status { get; set; } = string.Empty;

        /// <summary>
        /// 交�?完�??��?
        /// </summary>
        [Column("completed_at")]
        public DateTime? CompletedAt { get; set; }

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
        public virtual PlayerMarketProductInfo MarketItem { get; set; } = null!;
        public virtual User Buyer { get; set; } = null!;
        public virtual User Seller { get; set; } = null!;
    }
} 
