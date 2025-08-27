using GameCore.Core.Entities;
using GameCore.Core.Interfaces;
using GameCore.Core.DTOs;
using Microsoft.Extensions.Logging;

namespace GameCore.Core.Services
{
    public class StoreService : IStoreService
    {
        private readonly IStoreRepository _storeRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly ICartRepository _cartRepository;
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<StoreService> _logger;

        public StoreService(
            IStoreRepository storeRepository,
            IOrderRepository orderRepository,
            ICartRepository cartRepository,
            IUserRepository userRepository,
            IUnitOfWork unitOfWork,
            ILogger<StoreService> logger)
        {
            _storeRepository = storeRepository;
            _orderRepository = orderRepository;
            _cartRepository = cartRepository;
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<IEnumerable<ProductDto>> GetActiveProductsAsync(string? category = null, int page = 1, int pageSize = 20)
        {
            try
            {
                var products = await _storeRepository.GetActiveProductsAsync(category, page, pageSize);
                return products.Select(p => new ProductDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description,
                    Price = p.Price,
                    StockQuantity = p.StockQuantity,
                    Category = p.Category,
                    ImageUrl = p.ImageUrl,
                    IsActive = p.IsActive,
                    CreatedAt = p.CreatedAt,
                    UpdatedAt = p.UpdatedAt
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "獲取活躍商品失敗");
                return Enumerable.Empty<ProductDto>();
            }
        }

        public async Task<IEnumerable<ProductDto>> SearchProductsAsync(string keyword, string? category = null, decimal? minPrice = null, decimal? maxPrice = null, int page = 1, int pageSize = 20)
        {
            try
            {
                var products = await _storeRepository.SearchProductsAsync(keyword, category, minPrice, maxPrice, page, pageSize);
                return products.Select(p => new ProductDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description,
                    Price = p.Price,
                    StockQuantity = p.StockQuantity,
                    Category = p.Category,
                    ImageUrl = p.ImageUrl,
                    IsActive = p.IsActive,
                    CreatedAt = p.CreatedAt,
                    UpdatedAt = p.UpdatedAt
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "搜尋商品失敗");
                return Enumerable.Empty<ProductDto>();
            }
        }

        public async Task<ProductDto> GetProductAsync(int productId)
        {
            try
            {
                var product = await _storeRepository.GetProductByIdAsync(productId);
                if (product == null) return null;

                return new ProductDto
                {
                    Id = product.Id,
                    Name = product.Name,
                    Description = product.Description,
                    Price = product.Price,
                    StockQuantity = product.StockQuantity,
                    Category = product.Category,
                    ImageUrl = product.ImageUrl,
                    IsActive = product.IsActive,
                    CreatedAt = product.CreatedAt,
                    UpdatedAt = product.UpdatedAt
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "獲取商品詳情失敗: {ProductId}", productId);
                return null;
            }
        }

        public async Task<IEnumerable<string>> GetProductCategoriesAsync()
        {
            try
            {
                return await _storeRepository.GetProductCategoriesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "獲取商品分類失敗");
                return Enumerable.Empty<string>();
            }
        }

        public async Task<IEnumerable<ProductDto>> GetPopularProductsAsync(int count = 10)
        {
            try
            {
                var products = await _storeRepository.GetPopularProductsAsync(count);
                return products.Select(p => new ProductDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description,
                    Price = p.Price,
                    StockQuantity = p.StockQuantity,
                    Category = p.Category,
                    ImageUrl = p.ImageUrl,
                    IsActive = p.IsActive,
                    CreatedAt = p.CreatedAt,
                    UpdatedAt = p.UpdatedAt
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "獲取熱門商品失敗");
                return Enumerable.Empty<ProductDto>();
            }
        }

        public async Task<IEnumerable<ProductDto>> GetSalesRankingAsync(string category, int count = 10)
        {
            try
            {
                var products = await _storeRepository.GetSalesRankingAsync(category, count);
                return products.Select(p => new ProductDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description,
                    Price = p.Price,
                    StockQuantity = p.StockQuantity,
                    Category = p.Category,
                    ImageUrl = p.ImageUrl,
                    IsActive = p.IsActive,
                    CreatedAt = p.CreatedAt,
                    UpdatedAt = p.UpdatedAt
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "獲取銷售排行失敗");
                return Enumerable.Empty<ProductDto>();
            }
        }

        public async Task<CartItemResult> AddToCartAsync(int userId, CartItem cartItem)
        {
            try
            {
                var result = await _cartRepository.AddToCartAsync(userId, cartItem);
                if (result == null) return null;

                return new CartItemResult
                {
                    CartItemId = result.Id,
                    CartId = result.CartId,
                    ProductId = result.ProductId,
                    ProductName = result.Product.Name,
                    ProductImage = result.Product.ImageUrl,
                    Price = result.Price,
                    Quantity = result.Quantity,
                    Subtotal = result.Price * result.Quantity,
                    AddedAt = result.CreatedAt
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "添加商品到購物車失敗: {UserId}, {ProductId}", userId, cartItem.ProductId);
                return null;
            }
        }

        public async Task<CartResult> GetUserCartAsync(int userId)
        {
            try
            {
                var cart = await _cartRepository.GetUserCartAsync(userId);
                if (cart == null) return null;

                return new CartResult
                {
                    CartId = cart.Id,
                    UserId = cart.UserId,
                    Items = cart.Items.Select(item => new CartItemResult
                    {
                        CartItemId = item.Id,
                        CartId = item.CartId,
                        ProductId = item.ProductId,
                        ProductName = item.Product.Name,
                        ProductImage = item.Product.ImageUrl,
                        Price = item.Price,
                        Quantity = item.Quantity,
                        Subtotal = item.Price * item.Quantity,
                        AddedAt = item.CreatedAt
                    }).ToList(),
                    TotalAmount = cart.Items.Sum(item => item.Price * item.Quantity),
                    ItemCount = cart.Items.Count,
                    CreatedAt = cart.CreatedAt,
                    UpdatedAt = cart.UpdatedAt
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "獲取用戶購物車失敗: {UserId}", userId);
                return null;
            }
        }

        public async Task<CartItemResult> UpdateCartQuantityAsync(int cartItemId, int quantity)
        {
            try
            {
                var result = await _cartRepository.UpdateCartQuantityAsync(cartItemId, quantity);
                if (result == null) return null;

                return new CartItemResult
                {
                    CartItemId = result.Id,
                    CartId = result.CartId,
                    ProductId = result.ProductId,
                    ProductName = result.Product.Name,
                    ProductImage = result.Product.ImageUrl,
                    Price = result.Price,
                    Quantity = result.Quantity,
                    Subtotal = result.Price * result.Quantity,
                    AddedAt = result.CreatedAt
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "更新購物車數量失敗: {CartItemId}", cartItemId);
                return null;
            }
        }

        public async Task<bool> RemoveFromCartAsync(int cartItemId)
        {
            try
            {
                return await _cartRepository.RemoveFromCartAsync(cartItemId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "移除購物車商品失敗: {CartItemId}", cartItemId);
                return false;
            }
        }

        public async Task<bool> ClearUserCartAsync(int userId)
        {
            try
            {
                return await _cartRepository.ClearUserCartAsync(userId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "清空用戶購物車失敗: {UserId}", userId);
                return false;
            }
        }

        public async Task<OrderResult> CreateOrderAsync(int userId, OrderCreateDto orderCreate)
        {
            try
            {
                var order = await _orderRepository.CreateOrderAsync(userId, orderCreate);
                if (order == null) return null;

                return new OrderResult
                {
                    OrderId = order.Id,
                    UserId = order.UserId,
                    OrderNumber = order.OrderNumber,
                    TotalAmount = order.TotalAmount,
                    Status = order.Status,
                    ShippingAddress = order.ShippingAddress,
                    PaymentMethod = order.PaymentMethod,
                    Items = order.Items.Select(item => new OrderItemResult
                    {
                        OrderItemId = item.Id,
                        OrderId = item.OrderId,
                        ProductId = item.ProductId,
                        ProductName = item.Product.Name,
                        Price = item.Price,
                        Quantity = item.Quantity,
                        Subtotal = item.Price * item.Quantity
                    }).ToList(),
                    CreatedAt = order.CreatedAt,
                    UpdatedAt = order.UpdatedAt
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "創建訂單失敗: {UserId}", userId);
                return null;
            }
        }

        public async Task<IEnumerable<OrderResult>> GetOrdersByUserAsync(int userId, int page = 1, int pageSize = 20)
        {
            try
            {
                var orders = await _orderRepository.GetOrdersByUserAsync(userId, page, pageSize);
                return orders.Select(order => new OrderResult
                {
                    OrderId = order.Id,
                    UserId = order.UserId,
                    OrderNumber = order.OrderNumber,
                    TotalAmount = order.TotalAmount,
                    Status = order.Status,
                    ShippingAddress = order.ShippingAddress,
                    PaymentMethod = order.PaymentMethod,
                    Items = order.Items.Select(item => new OrderItemResult
                    {
                        OrderItemId = item.Id,
                        OrderId = item.OrderId,
                        ProductId = item.ProductId,
                        ProductName = item.Product.Name,
                        Price = item.Price,
                        Quantity = item.Quantity,
                        Subtotal = item.Price * item.Quantity
                    }).ToList(),
                    CreatedAt = order.CreatedAt,
                    UpdatedAt = order.UpdatedAt
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "獲取用戶訂單失敗: {UserId}", userId);
                return Enumerable.Empty<OrderResult>();
            }
        }

        public async Task<OrderResult> GetOrderAsync(int orderId)
        {
            try
            {
                var order = await _orderRepository.GetOrderByIdAsync(orderId);
                if (order == null) return null;

                return new OrderResult
                {
                    OrderId = order.Id,
                    UserId = order.UserId,
                    OrderNumber = order.OrderNumber,
                    TotalAmount = order.TotalAmount,
                    Status = order.Status,
                    ShippingAddress = order.ShippingAddress,
                    PaymentMethod = order.PaymentMethod,
                    Items = order.Items.Select(item => new OrderItemResult
                    {
                        OrderItemId = item.Id,
                        OrderId = item.OrderId,
                        ProductId = item.ProductId,
                        ProductName = item.Product.Name,
                        Price = item.Price,
                        Quantity = item.Quantity,
                        Subtotal = item.Price * item.Quantity
                    }).ToList(),
                    CreatedAt = order.CreatedAt,
                    UpdatedAt = order.UpdatedAt
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "獲取訂單詳情失敗: {OrderId}", orderId);
                return null;
            }
        }

        public async Task<bool> CancelOrderAsync(int userId, int orderId)
        {
            try
            {
                return await _orderRepository.CancelOrderAsync(userId, orderId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "取消訂單失敗: {UserId}, {OrderId}", userId, orderId);
                return false;
            }
        }

        // 實現 IProductService 接口
        public async Task<IEnumerable<ProductDto>> GetAllAsync()
        {
            return await GetActiveProductsAsync();
        }

        public async Task<ProductDto> GetByIdAsync(int id)
        {
            return await GetProductAsync(id);
        }

        public async Task<ProductDto> CreateAsync(ProductDto productDto)
        {
            try
            {
                var product = new Product
                {
                    Name = productDto.Name,
                    Description = productDto.Description,
                    Price = productDto.Price,
                    StockQuantity = productDto.StockQuantity,
                    Category = productDto.Category,
                    ImageUrl = productDto.ImageUrl,
                    IsActive = productDto.IsActive
                };

                var result = await _storeRepository.CreateProductAsync(product);
                if (result == null) return null;

                return new ProductDto
                {
                    Id = result.Id,
                    Name = result.Name,
                    Description = result.Description,
                    Price = result.Price,
                    StockQuantity = result.StockQuantity,
                    Category = result.Category,
                    ImageUrl = result.ImageUrl,
                    IsActive = result.IsActive,
                    CreatedAt = result.CreatedAt,
                    UpdatedAt = result.UpdatedAt
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "創建商品失敗");
                return null;
            }
        }

        public async Task<ProductDto> UpdateAsync(ProductDto productDto)
        {
            try
            {
                var product = new Product
                {
                    Id = productDto.Id,
                    Name = productDto.Name,
                    Description = productDto.Description,
                    Price = productDto.Price,
                    StockQuantity = productDto.StockQuantity,
                    Category = productDto.Category,
                    ImageUrl = productDto.ImageUrl,
                    IsActive = productDto.IsActive
                };

                var result = await _storeRepository.UpdateProductAsync(product);
                if (result == null) return null;

                return new ProductDto
                {
                    Id = result.Id,
                    Name = result.Name,
                    Description = result.Description,
                    Price = result.Price,
                    StockQuantity = result.StockQuantity,
                    Category = result.Category,
                    ImageUrl = result.ImageUrl,
                    IsActive = result.IsActive,
                    CreatedAt = result.CreatedAt,
                    UpdatedAt = result.UpdatedAt
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "更新商品失敗: {ProductId}", productDto.Id);
                return null;
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                return await _storeRepository.DeleteProductAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "刪除商品失敗: {ProductId}", id);
                return false;
            }
        }
    }
} 