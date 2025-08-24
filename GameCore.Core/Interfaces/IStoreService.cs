using GameCore.Core.Entities;

namespace GameCore.Core.Interfaces
{
    public interface IStoreService
    {
        Task<IEnumerable<StoreProduct>> GetProductsAsync(string? category = null, bool activeOnly = true);
        Task<StoreProduct?> GetProductByIdAsync(int productId);
        Task<StoreOrderResult> CreateOrderAsync(int userId, List<OrderItemDto> items, string deliveryAddress);
        Task<StoreOrderResult> UpdateOrderStatusAsync(int orderId, string newStatus);
        Task<StoreOrderResult> ProcessPaymentAsync(int orderId, string paymentMethod);
        Task<IEnumerable<StoreOrder>> GetUserOrdersAsync(int userId, string? status = null);
        Task<StoreOrder?> GetOrderByIdAsync(int orderId);
        Task<bool> AddToCartAsync(int userId, int productId, int quantity);
        Task<IEnumerable<ShoppingCartItem>> GetCartItemsAsync(int userId);
        Task<bool> RemoveFromCartAsync(int userId, int productId);
    }

    public class StoreOrderResult
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public StoreOrder? Order { get; set; }
        public decimal TotalAmount { get; set; }
    }

    public class OrderItemDto
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }
}