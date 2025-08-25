using GameCore.Core.Entities;
using GameCore.Core.Interfaces;
using GameCore.Core.Services;
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

        public async Task<IEnumerable<ProductDto>> GetProductsAsync(int page = 1, int pageSize = 20)
        {
            try
            {
                var products = await _storeRepository.GetProductsAsync(page, pageSize);
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
                _logger.LogError(ex, "獲取商品列表失敗");
                return Enumerable.Empty<ProductDto>();
            }
        }

        public async Task<ProductDto> GetProductByIdAsync(int productId)
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

        public async Task<IEnumerable<ProductDto>> SearchProductsAsync(string keyword, int page = 1, int pageSize = 20)
        {
            try
            {
                var products = await _storeRepository.SearchProductsAsync(keyword, page, pageSize);
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
                _logger.LogError(ex, "搜尋商品失敗: {Keyword}", keyword);
                return Enumerable.Empty<ProductDto>();
            }
        }

        public async Task<IEnumerable<ProductDto>> GetProductsByCategoryAsync(string category, int page = 1, int pageSize = 20)
        {
            try
            {
                var products = await _storeRepository.GetProductsByCategoryAsync(category, page, pageSize);
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
                _logger.LogError(ex, "按分類獲取商品失敗: {Category}", category);
                return Enumerable.Empty<ProductDto>();
            }
        }

        public async Task<CartResult> GetCartAsync(int userId)
        {
            try
            {
                var cartItems = await _cartRepository.GetCartItemsAsync(userId);
                var cartDtos = cartItems.Select(ci => new CartItemDto
                {
                    Id = ci.Id,
                    ProductId = ci.ProductId,
                    ProductName = ci.Product?.Name ?? "未知商品",
                    ProductPrice = ci.Product?.Price ?? 0,
                    ProductImageUrl = ci.Product?.ImageUrl,
                    Quantity = ci.Quantity,
                    AddedAt = ci.AddedAt
                });

                var totalAmount = cartDtos.Sum(ci => ci.ProductPrice * ci.Quantity);

                return new CartResult
                {
                    Success = true,
                    CartItems = cartDtos,
                    TotalAmount = totalAmount,
                    ItemCount = cartDtos.Count()
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "獲取購物車失敗: {UserId}", userId);
                return new CartResult
                {
                    Success = false,
                    Message = "獲取購物車失敗"
                };
            }
        }

        public async Task<CartItemResult> AddToCartAsync(CartItemCreateDto createDto)
        {
            try
            {
                // 檢查商品是否存在且有庫存
                var product = await _storeRepository.GetProductByIdAsync(createDto.ProductId);
                if (product == null)
                {
                    return new CartItemResult
                    {
                        Success = false,
                        Message = "商品不存在"
                    };
                }

                if (product.StockQuantity < createDto.Quantity)
                {
                    return new CartItemResult
                    {
                        Success = false,
                        Message = "商品庫存不足"
                    };
                }

                // 檢查購物車是否已有此商品
                var existingItem = await _cartRepository.GetCartItemAsync(createDto.UserId, createDto.ProductId);
                if (existingItem != null)
                {
                    // 更新數量
                    existingItem.Quantity += createDto.Quantity;
                    _cartRepository.UpdateCartItem(existingItem);
                }
                else
                {
                    // 新增購物車項目
                    var cartItem = new CartItem
                    {
                        UserId = createDto.UserId,
                        ProductId = createDto.ProductId,
                        Quantity = createDto.Quantity,
                        AddedAt = DateTime.UtcNow
                    };
                    _cartRepository.AddCartItem(cartItem);
                }

                await _unitOfWork.SaveChangesAsync();

                _logger.LogInformation("商品添加到購物車成功: 用戶 {UserId}, 商品 {ProductId}", createDto.UserId, createDto.ProductId);

                return new CartItemResult
                {
                    Success = true,
                    Message = "商品已添加到購物車"
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "添加商品到購物車失敗: 用戶 {UserId}, 商品 {ProductId}", createDto.UserId, createDto.ProductId);
                return new CartItemResult
                {
                    Success = false,
                    Message = "添加失敗"
                };
            }
        }

        public async Task<CartItemResult> UpdateCartItemAsync(int cartItemId, int userId, int quantity)
        {
            try
            {
                var cartItem = await _cartRepository.GetCartItemByIdAsync(cartItemId);
                if (cartItem == null)
                {
                    return new CartItemResult
                    {
                        Success = false,
                        Message = "購物車項目不存在"
                    };
                }

                if (cartItem.UserId != userId)
                {
                    return new CartItemResult
                    {
                        Success = false,
                        Message = "無權限修改此購物車項目"
                    };
                }

                if (quantity <= 0)
                {
                    // 刪除購物車項目
                    _cartRepository.RemoveCartItem(cartItem);
                }
                else
                {
                    // 檢查庫存
                    var product = await _storeRepository.GetProductByIdAsync(cartItem.ProductId);
                    if (product != null && product.StockQuantity < quantity)
                    {
                        return new CartItemResult
                        {
                            Success = false,
                            Message = "商品庫存不足"
                        };
                    }

                    cartItem.Quantity = quantity;
                    _cartRepository.UpdateCartItem(cartItem);
                }

                await _unitOfWork.SaveChangesAsync();

                _logger.LogInformation("購物車項目更新成功: {CartItemId}", cartItemId);

                return new CartItemResult
                {
                    Success = true,
                    Message = "購物車項目更新成功"
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "更新購物車項目失敗: {CartItemId}", cartItemId);
                return new CartItemResult
                {
                    Success = false,
                    Message = "更新失敗"
                };
            }
        }

        public async Task<CartItemResult> RemoveFromCartAsync(int cartItemId, int userId)
        {
            try
            {
                var cartItem = await _cartRepository.GetCartItemByIdAsync(cartItemId);
                if (cartItem == null)
                {
                    return new CartItemResult
                    {
                        Success = false,
                        Message = "購物車項目不存在"
                    };
                }

                if (cartItem.UserId != userId)
                {
                    return new CartItemResult
                    {
                        Success = false,
                        Message = "無權限刪除此購物車項目"
                    };
                }

                _cartRepository.RemoveCartItem(cartItem);
                await _unitOfWork.SaveChangesAsync();

                _logger.LogInformation("購物車項目刪除成功: {CartItemId}", cartItemId);

                return new CartItemResult
                {
                    Success = true,
                    Message = "商品已從購物車移除"
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "刪除購物車項目失敗: {CartItemId}", cartItemId);
                return new CartItemResult
                {
                    Success = false,
                    Message = "刪除失敗"
                };
            }
        }

        public async Task<OrderCreateResult> CreateOrderAsync(OrderCreateDto createDto)
        {
            try
            {
                // 獲取購物車項目
                var cartItems = await _cartRepository.GetCartItemsAsync(createDto.UserId);
                if (!cartItems.Any())
                {
                    return new OrderCreateResult
                    {
                        Success = false,
                        Message = "購物車為空"
                    };
                }

                // 檢查庫存
                foreach (var cartItem in cartItems)
                {
                    var product = await _storeRepository.GetProductByIdAsync(cartItem.ProductId);
                    if (product == null || product.StockQuantity < cartItem.Quantity)
                    {
                        return new OrderCreateResult
                        {
                            Success = false,
                            Message = $"商品 {product?.Name ?? "未知"} 庫存不足"
                        };
                    }
                }

                // 創建訂單
                var order = new Order
                {
                    UserId = createDto.UserId,
                    OrderNumber = GenerateOrderNumber(),
                    Status = OrderStatus.Pending,
                    TotalAmount = cartItems.Sum(ci => ci.Product.Price * ci.Quantity),
                    ShippingAddress = createDto.ShippingAddress,
                    ContactPhone = createDto.ContactPhone,
                    Notes = createDto.Notes,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                _orderRepository.Add(order);
                await _unitOfWork.SaveChangesAsync();

                // 創建訂單項目
                foreach (var cartItem in cartItems)
                {
                    var orderItem = new OrderItem
                    {
                        OrderId = order.Id,
                        ProductId = cartItem.ProductId,
                        ProductName = cartItem.Product.Name,
                        ProductPrice = cartItem.Product.Price,
                        Quantity = cartItem.Quantity,
                        Subtotal = cartItem.Product.Price * cartItem.Quantity
                    };
                    _orderRepository.AddOrderItem(orderItem);

                    // 減少庫存
                    var product = await _storeRepository.GetProductByIdAsync(cartItem.ProductId);
                    product.StockQuantity -= cartItem.Quantity;
                    _storeRepository.UpdateProduct(product);
                }

                // 清空購物車
                foreach (var cartItem in cartItems)
                {
                    _cartRepository.RemoveCartItem(cartItem);
                }

                await _unitOfWork.SaveChangesAsync();

                _logger.LogInformation("訂單創建成功: {OrderId}, 用戶: {UserId}", order.Id, order.UserId);

                return new OrderCreateResult
                {
                    Success = true,
                    Message = "訂單創建成功",
                    OrderId = order.Id,
                    OrderNumber = order.OrderNumber
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "創建訂單失敗: 用戶 {UserId}", createDto.UserId);
                return new OrderCreateResult
                {
                    Success = false,
                    Message = "創建訂單失敗"
                };
            }
        }

        public async Task<OrderResult> GetOrderByIdAsync(int orderId, int userId)
        {
            try
            {
                var order = await _orderRepository.GetByIdAsync(orderId);
                if (order == null)
                {
                    return new OrderResult
                    {
                        Success = false,
                        Message = "訂單不存在"
                    };
                }

                if (order.UserId != userId)
                {
                    return new OrderResult
                    {
                        Success = false,
                        Message = "無權限查看此訂單"
                    };
                }

                var orderItems = await _orderRepository.GetOrderItemsAsync(orderId);
                var orderItemDtos = orderItems.Select(oi => new OrderItemDto
                {
                    Id = oi.Id,
                    ProductId = oi.ProductId,
                    ProductName = oi.ProductName,
                    ProductPrice = oi.ProductPrice,
                    Quantity = oi.Quantity,
                    Subtotal = oi.Subtotal
                });

                var orderDto = new OrderDto
                {
                    Id = order.Id,
                    OrderNumber = order.OrderNumber,
                    UserId = order.UserId,
                    Status = order.Status,
                    TotalAmount = order.TotalAmount,
                    ShippingAddress = order.ShippingAddress,
                    ContactPhone = order.ContactPhone,
                    Notes = order.Notes,
                    CreatedAt = order.CreatedAt,
                    UpdatedAt = order.UpdatedAt,
                    OrderItems = orderItemDtos
                };

                return new OrderResult
                {
                    Success = true,
                    Order = orderDto
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "獲取訂單失敗: {OrderId}", orderId);
                return new OrderResult
                {
                    Success = false,
                    Message = "獲取訂單失敗"
                };
            }
        }

        public async Task<IEnumerable<OrderDto>> GetUserOrdersAsync(int userId, int page = 1, int pageSize = 20)
        {
            try
            {
                var orders = await _orderRepository.GetByUserIdAsync(userId, page, pageSize);
                var orderDtos = new List<OrderDto>();

                foreach (var order in orders)
                {
                    var orderItems = await _orderRepository.GetOrderItemsAsync(order.Id);
                    var orderItemDtos = orderItems.Select(oi => new OrderItemDto
                    {
                        Id = oi.Id,
                        ProductId = oi.ProductId,
                        ProductName = oi.ProductName,
                        ProductPrice = oi.ProductPrice,
                        Quantity = oi.Quantity,
                        Subtotal = oi.Subtotal
                    });

                    orderDtos.Add(new OrderDto
                    {
                        Id = order.Id,
                        OrderNumber = order.OrderNumber,
                        UserId = order.UserId,
                        Status = order.Status,
                        TotalAmount = order.TotalAmount,
                        ShippingAddress = order.ShippingAddress,
                        ContactPhone = order.ContactPhone,
                        Notes = order.Notes,
                        CreatedAt = order.CreatedAt,
                        UpdatedAt = order.UpdatedAt,
                        OrderItems = orderItemDtos
                    });
                }

                return orderDtos;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "獲取用戶訂單失敗: {UserId}", userId);
                return Enumerable.Empty<OrderDto>();
            }
        }

        public async Task<OrderUpdateResult> UpdateOrderStatusAsync(int orderId, int userId, OrderStatus newStatus)
        {
            try
            {
                var order = await _orderRepository.GetByIdAsync(orderId);
                if (order == null)
                {
                    return new OrderUpdateResult
                    {
                        Success = false,
                        Message = "訂單不存在"
                    };
                }

                if (order.UserId != userId)
                {
                    return new OrderUpdateResult
                    {
                        Success = false,
                        Message = "無權限修改此訂單"
                    };
                }

                // 檢查狀態轉換是否有效
                if (!IsValidStatusTransition(order.Status, newStatus))
                {
                    return new OrderUpdateResult
                    {
                        Success = false,
                        Message = "無效的狀態轉換"
                    };
                }

                order.Status = newStatus;
                order.UpdatedAt = DateTime.UtcNow;

                _orderRepository.Update(order);
                await _unitOfWork.SaveChangesAsync();

                _logger.LogInformation("訂單狀態更新成功: {OrderId}, 新狀態: {Status}", orderId, newStatus);

                return new OrderUpdateResult
                {
                    Success = true,
                    Message = "訂單狀態更新成功"
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "更新訂單狀態失敗: {OrderId}", orderId);
                return new OrderUpdateResult
                {
                    Success = false,
                    Message = "更新失敗"
                };
            }
        }

        private string GenerateOrderNumber()
        {
            // 生成訂單號：年月日時分秒 + 4位隨機數
            var timestamp = DateTime.UtcNow.ToString("yyyyMMddHHmmss");
            var random = new Random();
            var randomPart = random.Next(1000, 9999).ToString();
            return $"GC{timestamp}{randomPart}";
        }

        private bool IsValidStatusTransition(OrderStatus currentStatus, OrderStatus newStatus)
        {
            // 定義有效的狀態轉換
            var validTransitions = new Dictionary<OrderStatus, OrderStatus[]>
            {
                { OrderStatus.Pending, new[] { OrderStatus.Confirmed, OrderStatus.Cancelled } },
                { OrderStatus.Confirmed, new[] { OrderStatus.Shipped, OrderStatus.Cancelled } },
                { OrderStatus.Shipped, new[] { OrderStatus.Delivered, OrderStatus.Returned } },
                { OrderStatus.Delivered, new[] { OrderStatus.Completed, OrderStatus.Returned } },
                { OrderStatus.Completed, new[] { OrderStatus.Returned } },
                { OrderStatus.Cancelled, new OrderStatus[] { } },
                { OrderStatus.Returned, new OrderStatus[] { } }
            };

            return validTransitions.ContainsKey(currentStatus) && 
                   validTransitions[currentStatus].Contains(newStatus);
        }
    }
}