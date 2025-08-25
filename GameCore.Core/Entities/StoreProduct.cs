using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using GameCore.Core.Enums;

namespace GameCore.Core.Entities
{
    /// <summary>
    /// 商城商品實體
    /// </summary>
    [Table("store_products")]
    public class StoreProduct
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [Column("name", TypeName = "varchar(200)")]
        public string Name { get; set; } = string.Empty;

        [Column("description", TypeName = "text")]
        public string Description { get; set; } = string.Empty;

        [Column("price", TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        [Column("stock_quantity")]
        public int StockQuantity { get; set; } = 0;

        [Column("category", TypeName = "varchar(50)")]
        public string Category { get; set; } = string.Empty;

        [Column("image_url", TypeName = "varchar(500)")]
        public string? ImageUrl { get; set; }

        [Column("is_active")]
        public bool IsActive { get; set; } = true;

        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Column("updated_at")]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // 導航屬性
        public virtual ICollection<StoreOrderItem> OrderItems { get; set; } = new List<StoreOrderItem>();
        public virtual ICollection<ShoppingCartItem> CartItems { get; set; } = new List<ShoppingCartItem>();
    }

    /// <summary>
    /// 商城訂單實體
    /// </summary>
    [Table("store_orders")]
    public class StoreOrder
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("user_id")]
        public int UserId { get; set; }

        [Column("order_total", TypeName = "decimal(18,2)")]
        public decimal OrderTotal { get; set; }

        [Column("status")]
        public OrderStatus Status { get; set; } = OrderStatus.Pending;

        [Column("shipping_address", TypeName = "text")]
        public string? ShippingAddress { get; set; }

        [Column("payment_method", TypeName = "varchar(50)")]
        public string? PaymentMethod { get; set; }

        [Column("order_date")]
        public DateTime OrderDate { get; set; } = DateTime.UtcNow;

        [Column("shipped_at")]
        public DateTime? ShippedAt { get; set; }

        [Column("delivered_at")]
        public DateTime? DeliveredAt { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Column("updated_at")]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // 導航屬性
        [ForeignKey("UserId")]
        public virtual User User { get; set; } = null!;
        public virtual ICollection<StoreOrderItem> OrderItems { get; set; } = new List<StoreOrderItem>();
    }

    /// <summary>
    /// 商城訂單項目實體
    /// </summary>
    [Table("store_order_items")]
    public class StoreOrderItem
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("order_id")]
        public int OrderId { get; set; }

        [Column("product_id")]
        public int ProductId { get; set; }

        [Column("quantity")]
        public int Quantity { get; set; }

        [Column("unit_price", TypeName = "decimal(18,2)")]
        public decimal UnitPrice { get; set; }

        [Column("subtotal", TypeName = "decimal(18,2)")]
        public decimal Subtotal { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // 導航屬性
        [ForeignKey("OrderId")]
        public virtual StoreOrder Order { get; set; } = null!;
        [ForeignKey("ProductId")]
        public virtual StoreProduct Product { get; set; } = null!;
    }

    /// <summary>
    /// 購物車實體
    /// </summary>
    [Table("shopping_carts")]
    public class ShoppingCart
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("user_id")]
        public int UserId { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Column("updated_at")]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // 導航屬性
        [ForeignKey("UserId")]
        public virtual User User { get; set; } = null!;
        public virtual ICollection<ShoppingCartItem> CartItems { get; set; } = new List<ShoppingCartItem>();
    }

    /// <summary>
    /// 購物車項目實體
    /// </summary>
    [Table("shopping_cart_items")]
    public class ShoppingCartItem
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("cart_id")]
        public int CartId { get; set; }

        [Column("product_id")]
        public int ProductId { get; set; }

        [Column("quantity")]
        public int Quantity { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Column("updated_at")]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // 導航屬性
        [ForeignKey("CartId")]
        public virtual ShoppingCart Cart { get; set; } = null!;
        [ForeignKey("ProductId")]
        public virtual StoreProduct Product { get; set; } = null!;
    }
}