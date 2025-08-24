using GameCore.Core.Entities;
using GameCore.Core.Interfaces;
using Microsoft.Extensions.Logging;

namespace GameCore.Core.Services
{
    public class StoreService : IStoreService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<StoreService> _logger;

        public StoreService(IUnitOfWork unitOfWork, ILogger<StoreService> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<IEnumerable<StoreProduct>> GetProductsAsync(string? category = null, bool activeOnly = true)
        {
            try
            {
                return await _unitOfWork.StoreRepository.GetProductsAsync(category, activeOnly);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "獲取商城商品失敗");
                return Enumerable.Empty<StoreProduct>();
            }
        }

        public async Task<StoreProduct?> GetProductByIdAsync(int productId)
        {
            try
            {
                return await _unitOfWork.StoreRepository.GetProductByIdAsync(productId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "獲取商品詳情失敗: {ProductId}", productId);
                return null;
            }
        }

        public async Task<StoreOrderResult> CreateOrderAsync(int userId, List<OrderItemDto> items, string deliveryAddress)
        {
            try
            {
                // 驗證商品
                var orderItems = new List<StoreOrderItem>();
                decimal totalAmount = 0;

                foreach (var item in items)
                {
                    var product = await _unitOfWork.StoreRepository.GetProductByIdAsync(item.ProductId);
                    if (product == null)
                    {
                        return new StoreOrderResult
                        {
                            Success = false,
                            Message = $"商品不存在: {item.ProductId}"
                        };
                    }

                    if (!product.IsActive)
                    {
                        return new StoreOrderResult
                        {
                            Success = false,
                            Message = $"商品已下架: {product.Name}"
                        };
                    }

                    if (product.Stock < item.Quantity)
                    {
                        return new StoreOrderResult
                        {
                            Success = false,
                            Message = $"商品庫存不足: {product.Name}"
                        };
                    }

                    var orderItem = new StoreOrderItem
                    {
                        ProductId = item.ProductId,
                        Quantity = item.Quantity,
                        UnitPrice = product.Price,
                        TotalPrice = product.Price * item.Quantity
                    };

                    orderItems.Add(orderItem);
                    totalAmount += orderItem.TotalPrice;
                }

                // 創建訂單
                var order = new StoreOrder
                {
                    UserId = userId,
                    OrderNumber = GenerateOrderNumber(),
                    Status = "Created", // Created → ToShip → Shipped → Completed
                    TotalAmount = totalAmount,
                    DeliveryAddress = deliveryAddress,
                    PaymentMethod = "Pending", // Placed → Pending → Paid
                    CreateTime = DateTime.UtcNow,
                    UpdateTime = DateTime.UtcNow
                };

                await _unitOfWork.StoreRepository.CreateOrderAsync(order);

                // 創建訂單項目
                foreach (var item in orderItems)
                {
                    item.OrderId = order.Id;
                    await _unitOfWork.StoreRepository.CreateOrderItemAsync(item);
                }

                // 更新庫存
                foreach (var item in items)
                {
                    await _unitOfWork.StoreRepository.UpdateProductStockAsync(item.ProductId, -item.Quantity);
                }

                await _unitOfWork.SaveChangesAsync();

                _logger.LogInformation("創建商城訂單成功: {OrderId}, 用戶: {UserId}, 總金額: {TotalAmount}", 
                    order.Id, userId, totalAmount);

                return new StoreOrderResult
                {
                    Success = true,
                    Message = "訂單創建成功",
                    Order = order,
                    TotalAmount = totalAmount
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "創建商城訂單失敗: {UserId}", userId);
                return new StoreOrderResult
                {
                    Success = false,
                    Message = "創建訂單失敗，請稍後重試"
                };
            }
        }

        public async Task<StoreOrderResult> UpdateOrderStatusAsync(int orderId, string newStatus)
        {
            try
            {
                var order = await _unitOfWork.StoreRepository.GetOrderByIdAsync(orderId);
                if (order == null)
                {
                    return new StoreOrderResult
                    {
                        Success = false,
                        Message = "訂單不存在"
                    };
                }

                // 驗證狀態轉換
                if (!IsValidStatusTransition(order.Status, newStatus))
                {
                    return new StoreOrderResult
                    {
                        Success = false,
                        Message = $"無效的狀態轉換: {order.Status} → {newStatus}"
                    };
                }

                order.Status = newStatus;
                order.UpdateTime = DateTime.UtcNow;

                // 設置相應的時間戳
                switch (newStatus)
                {
                    case "Paid":
                        order.PaymentTime = DateTime.UtcNow;
                        break;
                    case "Shipped":
                        order.ShippedAt = DateTime.UtcNow;
                        break;
                    case "Completed":
                        order.CompletedAt = DateTime.UtcNow;
                        break;
                }

                await _unitOfWork.StoreRepository.UpdateOrderAsync(order);
                await _unitOfWork.SaveChangesAsync();

                _logger.LogInformation("更新訂單狀態成功: {OrderId}, {OldStatus} → {NewStatus}", 
                    orderId, order.Status, newStatus);

                return new StoreOrderResult
                {
                    Success = true,
                    Message = "訂單狀態更新成功",
                    Order = order
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "更新訂單狀態失敗: {OrderId}", orderId);
                return new StoreOrderResult
                {
                    Success = false,
                    Message = "更新訂單狀態失敗"
                };
            }
        }

        public async Task<StoreOrderResult> ProcessPaymentAsync(int orderId, string paymentMethod)
        {
            try
            {
                var order = await _unitOfWork.StoreRepository.GetOrderByIdAsync(orderId);
                if (order == null)
                {
                    return new StoreOrderResult
                    {
                        Success = false,
                        Message = "訂單不存在"
                    };
                }

                if (order.Status != "Created")
                {
                    return new StoreOrderResult
                    {
                        Success = false,
                        Message = "訂單狀態不允許付款"
                    };
                }

                // 更新訂單狀態為已付款
                order.Status = "Paid";
                order.PaymentMethod = paymentMethod;
                order.PaymentTime = DateTime.UtcNow;
                order.UpdateTime = DateTime.UtcNow;

                await _unitOfWork.StoreRepository.UpdateOrderAsync(order);
                await _unitOfWork.SaveChangesAsync();

                _logger.LogInformation("訂單付款成功: {OrderId}, 付款方式: {PaymentMethod}", 
                    orderId, paymentMethod);

                return new StoreOrderResult
                {
                    Success = true,
                    Message = "付款成功",
                    Order = order
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "處理訂單付款失敗: {OrderId}", orderId);
                return new StoreOrderResult
                {
                    Success = false,
                    Message = "付款處理失敗"
                };
            }
        }

        public async Task<IEnumerable<StoreOrder>> GetUserOrdersAsync(int userId, string? status = null)
        {
            try
            {
                return await _unitOfWork.StoreRepository.GetUserOrdersAsync(userId, status);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "獲取用戶訂單失敗: {UserId}", userId);
                return Enumerable.Empty<StoreOrder>();
            }
        }

        public async Task<StoreOrder?> GetOrderByIdAsync(int orderId)
        {
            try
            {
                return await _unitOfWork.StoreRepository.GetOrderByIdAsync(orderId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "獲取訂單詳情失敗: {OrderId}", orderId);
                return null;
            }
        }

        public async Task<bool> AddToCartAsync(int userId, int productId, int quantity)
        {
            try
            {
                var existingItem = await _unitOfWork.StoreRepository.GetCartItemAsync(userId, productId);
                
                if (existingItem != null)
                {
                    existingItem.Quantity += quantity;
                    existingItem.UpdateTime = DateTime.UtcNow;
                    await _unitOfWork.StoreRepository.UpdateCartItemAsync(existingItem);
                }
                else
                {
                    var cartItem = new ShoppingCartItem
                    {
                        UserId = userId,
                        ProductId = productId,
                        Quantity = quantity,
                        CreateTime = DateTime.UtcNow,
                        UpdateTime = DateTime.UtcNow
                    };
                    await _unitOfWork.StoreRepository.AddToCartAsync(cartItem);
                }

                await _unitOfWork.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "添加到購物車失敗: {UserId}, {ProductId}", userId, productId);
                return false;
            }
        }

        public async Task<IEnumerable<ShoppingCartItem>> GetCartItemsAsync(int userId)
        {
            try
            {
                return await _unitOfWork.StoreRepository.GetCartItemsAsync(userId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "獲取購物車失敗: {UserId}", userId);
                return Enumerable.Empty<ShoppingCartItem>();
            }
        }

        public async Task<bool> RemoveFromCartAsync(int userId, int productId)
        {
            try
            {
                await _unitOfWork.StoreRepository.RemoveFromCartAsync(userId, productId);
                await _unitOfWork.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "從購物車移除失敗: {UserId}, {ProductId}", userId, productId);
                return false;
            }
        }

        private string GenerateOrderNumber()
        {
            return $"SO{DateTime.UtcNow:yyyyMMddHHmmss}{Random.Shared.Next(1000, 9999)}";
        }

        private bool IsValidStatusTransition(string currentStatus, string newStatus)
        {
            return (currentStatus, newStatus) switch
            {
                ("Created", "ToShip") => true,
                ("ToShip", "Shipped") => true,
                ("Shipped", "Completed") => true,
                ("Created", "Cancelled") => true,
                _ => false
            };
        }
    }

    public class OrderItemDto
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }
}