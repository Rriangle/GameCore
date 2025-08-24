using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GameCore.Core.Entities
{
    /// <summary>
    /// 商城商品表 (對應 StoreProducts)
    /// </summary>
    [Table("StoreProducts")]
    public class StoreProduct
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }

        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public decimal Price { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal? OriginalPrice { get; set; }

        [Required]
        [StringLength(50)]
        public string Category { get; set; } = string.Empty;

        [StringLength(255)]
        public string? ImageUrl { get; set; }

        [Required]
        public int Stock { get; set; } = 0;

        [Required]
        public bool IsActive { get; set; } = true;

        [Required]
        public DateTime CreateTime { get; set; } = DateTime.UtcNow;

        [Required]
        public DateTime UpdateTime { get; set; } = DateTime.UtcNow;

        // Navigation properties
        public virtual ICollection<StoreOrderItem> OrderItems { get; set; } = new List<StoreOrderItem>();
        public virtual ICollection<ShoppingCartItem> CartItems { get; set; } = new List<ShoppingCartItem>();
    }

    /// <summary>
    /// 商城訂單表 (對應 StoreOrders)
    /// </summary>
    [Table("StoreOrders")]
    public class StoreOrder
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        [StringLength(50)]
        public string OrderNumber { get; set; } = string.Empty;

        [Required]
        [StringLength(20)]
        public string Status { get; set; } = "Pending"; // Created, ToShip, Shipped, Completed, Cancelled

        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public decimal TotalAmount { get; set; }

        [StringLength(500)]
        public string? DeliveryAddress { get; set; }

        [StringLength(50)]
        public string? PaymentMethod { get; set; }

        public DateTime? PaymentTime { get; set; }

        public DateTime? ShippedAt { get; set; }

        public DateTime? CompletedAt { get; set; }

        [Required]
        public DateTime CreateTime { get; set; } = DateTime.UtcNow;

        [Required]
        public DateTime UpdateTime { get; set; } = DateTime.UtcNow;

        // Navigation properties
        public virtual User User { get; set; } = null!;
        public virtual ICollection<StoreOrderItem> OrderItems { get; set; } = new List<StoreOrderItem>();
    }

    /// <summary>
    /// 商城訂單項目表 (對應 StoreOrderItems)
    /// </summary>
    [Table("StoreOrderItems")]
    public class StoreOrderItem
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int OrderId { get; set; }

        [Required]
        public int ProductId { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public decimal UnitPrice { get; set; }

        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public decimal TotalPrice { get; set; }

        [Required]
        public DateTime CreateTime { get; set; } = DateTime.UtcNow;

        // Navigation properties
        public virtual StoreOrder Order { get; set; } = null!;
        public virtual StoreProduct Product { get; set; } = null!;
    }

    /// <summary>
    /// 購物車表 (對應 ShoppingCartItems)
    /// </summary>
    [Table("ShoppingCartItems")]
    public class ShoppingCartItem
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public int ProductId { get; set; }

        [Required]
        public int Quantity { get; set; } = 1;

        [Required]
        public DateTime CreateTime { get; set; } = DateTime.UtcNow;

        [Required]
        public DateTime UpdateTime { get; set; } = DateTime.UtcNow;

        // Navigation properties
        public virtual User User { get; set; } = null!;
        public virtual StoreProduct Product { get; set; } = null!;
    }
}