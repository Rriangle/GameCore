using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GameCore.Domain.Entities
{
    /// <summary>
    /// ?�家市場訂單資�?實�?
    /// ?�於管�??�家市場?��???
    /// </summary>
    [Table("player_market_order_info")]
    public partial class PlayerMarketOrderInfo
    {
        /// <summary>
        /// 訂單ID（主?��?
        /// </summary>
        [Key]
        [Column("p_order_id")]
        public int POrderId { get; set; }

        /// <summary>
        /// ?��?ID（�??��?
        /// </summary>
        [Column("p_product_id")]
        public int PProductId { get; set; }

        /// <summary>
        /// �?��ID（�??�到Users�?
        /// </summary>
        [Column("seller_id")]
        public int SellerId { get; set; }

        /// <summary>
        /// 買家ID（�??�到Users�?
        /// </summary>
        [Column("buyer_id")]
        public int BuyerId { get; set; }

        /// <summary>
        /// 訂單?��?
        /// </summary>
        [Column("p_order_date")]
        public DateTime POrderDate { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// 訂單?�??
        /// </summary>
        [Column("p_order_status")]
        [StringLength(50)]
        public string POrderStatus { get; set; } = "Created";

        /// <summary>
        /// 付款?�??
        /// </summary>
        [Column("p_payment_status")]
        [StringLength(50)]
        public string PPaymentStatus { get; set; } = "Pending";

        /// <summary>
        /// ?�價
        /// </summary>
        [Column("p_unit_price")]
        public int PUnitPrice { get; set; }

        /// <summary>
        /// ?��?
        /// </summary>
        [Column("p_quantity")]
        public int PQuantity { get; set; }

        /// <summary>
        /// 訂單總�?
        /// </summary>
        [Column("p_order_total")]
        public int POrderTotal { get; set; }

        /// <summary>
        /// 建�??��?
        /// </summary>
        [Column("p_order_created_at")]
        public DateTime POrderCreatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// ?�新?��?
        /// </summary>
        [Column("p_order_updated_at")]
        public DateTime POrderUpdatedAt { get; set; } = DateTime.UtcNow;

        // 導航屬�?
        /// <summary>
        /// �?��
        /// </summary>
        public virtual User? Seller { get; set; }

        /// <summary>
        /// 買家
        /// </summary>
        public virtual User? Buyer { get; set; }
    }
} 
