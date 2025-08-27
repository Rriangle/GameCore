using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GameCore.Core.Entities
{
    /// <summary>
    /// 自由市場訂單實體
    /// </summary>
    [Table("PlayerMarketOrderInfo")]
    public class PlayerMarketOrderInfo
    {
        /// <summary>
        /// 訂單編號 (主鍵)
        /// </summary>
        [Key]
        [Column("order_id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int OrderId { get; set; }

        /// <summary>
        /// 買家編號 (外鍵到 User)
        /// </summary>
        [Required]
        [Column("buyer_id")]
        [ForeignKey("Buyer")]
        public int BuyerId { get; set; }

        /// <summary>
        /// 賣家編號 (外鍵到 User)
        /// </summary>
        [Required]
        [Column("seller_id")]
        [ForeignKey("Seller")]
        public int SellerId { get; set; }

        /// <summary>
        /// 商品編號 (外鍵到 PlayerMarketProductInfo)
        /// </summary>
        [Required]
        [Column("product_id")]
        [ForeignKey("Product")]
        public int ProductId { get; set; }

        /// <summary>
        /// 訂單狀態 (Pending, Confirmed, Shipped, Completed, Cancelled)
        /// </summary>
        [Required]
        [Column("status")]
        [StringLength(20)]
        public string Status { get; set; } = "Pending";

        /// <summary>
        /// 購買數量
        /// </summary>
        [Required]
        [Column("quantity")]
        [Range(1, int.MaxValue)]
        public int Quantity { get; set; } = 1;

        /// <summary>
        /// 單價 (下單時的價格)
        /// </summary>
        [Required]
        [Column("unit_price")]
        [Range(0, double.MaxValue)]
        public decimal UnitPrice { get; set; }

        /// <summary>
        /// 總金額
        /// </summary>
        [Required]
        [Column("total_amount")]
        [Range(0, double.MaxValue)]
        public decimal TotalAmount { get; set; }

        /// <summary>
        /// 訂單創建時間
        /// </summary>
        [Required]
        [Column("created_time")]
        public DateTime CreatedTime { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// 訂單更新時間
        /// </summary>
        [Required]
        [Column("updated_time")]
        public DateTime UpdatedTime { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// 交易完成時間
        /// </summary>
        [Column("completed_time")]
        public DateTime? CompletedTime { get; set; }

        /// <summary>
        /// 買家備註
        /// </summary>
        [Column("buyer_note")]
        [StringLength(500)]
        public string? BuyerNote { get; set; }

        /// <summary>
        /// 賣家備註
        /// </summary>
        [Column("seller_note")]
        [StringLength(500)]
        public string? SellerNote { get; set; }

        /// <summary>
        /// 交易評價 (1-5分)
        /// </summary>
        [Column("rating")]
        [Range(1, 5)]
        public int? Rating { get; set; }

        /// <summary>
        /// 評價內容
        /// </summary>
        [Column("review")]
        [StringLength(1000)]
        public string? Review { get; set; }

        /// <summary>
        /// 是否已評價
        /// </summary>
        [Required]
        [Column("is_reviewed")]
        public bool IsReviewed { get; set; } = false;

        // 導航屬性
        /// <summary>
        /// 買家
        /// </summary>
        public virtual User Buyer { get; set; } = null!;

        /// <summary>
        /// 賣家
        /// </summary>
        public virtual User Seller { get; set; } = null!;

        /// <summary>
        /// 商品資訊
        /// </summary>
        public virtual PlayerMarketProductInfo Product { get; set; } = null!;
    }
}