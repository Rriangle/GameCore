using System.ComponentModel.DataAnnotations;

namespace GameCore.Core.DTOs
{
    public class CartResult
    {
        public int CartId { get; set; }
        public int UserId { get; set; }
        public List<CartItemResult> Items { get; set; } = new();
        public decimal TotalAmount { get; set; }
        public int ItemCount { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public class CartItemResult
    {
        public int CartItemId { get; set; }
        public int CartId { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public string ProductImage { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public decimal Subtotal { get; set; }
        public DateTime AddedAt { get; set; }
    }

    public class CartItemCreateDto
    {
        [Required]
        public int ProductId { get; set; }
        
        [Required]
        [Range(1, 999)]
        public int Quantity { get; set; }
    }

    public class OrderCreateDto
    {
        [Required]
        public int UserId { get; set; }
        
        [Required]
        public List<OrderItemDto> Items { get; set; } = new();
        
        [Required]
        public string ShippingAddress { get; set; } = string.Empty;
        
        [Required]
        public string PaymentMethod { get; set; } = string.Empty;
        
        public string? Notes { get; set; }
    }

    public class OrderItemDto
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }

    public class OrderResult
    {
        public int OrderId { get; set; }
        public int UserId { get; set; }
        public string OrderNumber { get; set; } = string.Empty;
        public decimal TotalAmount { get; set; }
        public string Status { get; set; } = string.Empty;
        public string ShippingAddress { get; set; } = string.Empty;
        public string PaymentMethod { get; set; } = string.Empty;
        public List<OrderItemResult> Items { get; set; } = new();
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }

    public class OrderItemResult
    {
        public int OrderItemId { get; set; }
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public decimal Subtotal { get; set; }
    }

    public class OrderUpdateResult
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public OrderResult? Order { get; set; }
    }
} 