using GameCore.Core.DTOs;
using GameCore.Core.Entities;
using GameCore.Core.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GameCore.Core.Services
{
    /// <summary>
    /// 玩家市場服務實現類
    /// 提供玩家市場相關的業務邏輯處理
    /// </summary>
    public class PlayerMarketService : IPlayerMarketService
    {
        private readonly IPlayerMarketRepository _playerMarketRepository;
        private readonly IUserRepository _userRepository;
        private readonly INotificationService _notificationService;
        private readonly ILogger<PlayerMarketService> _logger;

        public PlayerMarketService(
            IPlayerMarketRepository playerMarketRepository,
            IUserRepository userRepository,
            INotificationService notificationService,
            ILogger<PlayerMarketService> logger)
        {
            _playerMarketRepository = playerMarketRepository;
            _userRepository = userRepository;
            _notificationService = notificationService;
            _logger = logger;
        }

        /// <summary>
        /// 獲取玩家市場商品列表
        /// </summary>
        /// <param name="category">商品分類</param>
        /// <param name="page">頁碼</param>
        /// <param name="pageSize">每頁大小</param>
        /// <param name="sortBy">排序方式</param>
        /// <returns>商品列表</returns>
        public async Task<PagedResult<PlayerMarket>> GetProductsAsync(string category = null, int page = 1, int pageSize = 20, string sortBy = "latest")
        {
            try
            {
                _logger.LogInformation("獲取玩家市場商品列表，分類: {Category}, 頁碼: {Page}, 每頁大小: {PageSize}, 排序: {SortBy}", 
                    category, page, pageSize, sortBy);
                
                var products = await _playerMarketRepository.GetProductsAsync(category, page, pageSize, sortBy);
                
                _logger.LogInformation("成功獲取玩家市場商品列表，共 {TotalCount} 件商品", products.TotalCount);
                return products;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "獲取玩家市場商品列表時發生錯誤");
                throw;
            }
        }

        /// <summary>
        /// 獲取商品詳情
        /// </summary>
        /// <param name="productId">商品ID</param>
        /// <returns>商品詳情</returns>
        public async Task<PlayerMarket> GetProductByIdAsync(int productId)
        {
            try
            {
                _logger.LogInformation("獲取玩家市場商品詳情，商品ID: {ProductId}", productId);
                
                var product = await _playerMarketRepository.GetProductByIdAsync(productId);
                
                if (product == null)
                {
                    _logger.LogWarning("商品不存在，商品ID: {ProductId}", productId);
                    return null;
                }
                
                _logger.LogInformation("成功獲取玩家市場商品詳情，商品ID: {ProductId}", productId);
                return product;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "獲取玩家市場商品詳情時發生錯誤，商品ID: {ProductId}", productId);
                throw;
            }
        }

        /// <summary>
        /// 搜尋商品
        /// </summary>
        /// <param name="searchTerm">搜尋關鍵字</param>
        /// <param name="category">商品分類</param>
        /// <param name="minPrice">最低價格</param>
        /// <param name="maxPrice">最高價格</param>
        /// <param name="page">頁碼</param>
        /// <param name="pageSize">每頁大小</param>
        /// <returns>搜尋結果</returns>
        public async Task<PagedResult<PlayerMarket>> SearchProductsAsync(string searchTerm, string category = null, 
            decimal? minPrice = null, decimal? maxPrice = null, int page = 1, int pageSize = 20)
        {
            try
            {
                _logger.LogInformation("搜尋玩家市場商品，關鍵字: {SearchTerm}, 分類: {Category}, 價格範圍: {MinPrice}-{MaxPrice}, 頁碼: {Page}", 
                    searchTerm, category, minPrice, maxPrice, page);
                
                var result = await _playerMarketRepository.SearchProductsAsync(searchTerm, category, minPrice, maxPrice, page, pageSize);
                
                _logger.LogInformation("成功搜尋玩家市場商品，關鍵字: {SearchTerm}, 共 {TotalCount} 件商品", 
                    searchTerm, result.TotalCount);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "搜尋玩家市場商品時發生錯誤，關鍵字: {SearchTerm}", searchTerm);
                throw;
            }
        }

        /// <summary>
        /// 獲取用戶發布的商品
        /// </summary>
        /// <param name="userId">用戶ID</param>
        /// <param name="page">頁碼</param>
        /// <param name="pageSize">每頁大小</param>
        /// <returns>用戶商品列表</returns>
        public async Task<PagedResult<PlayerMarket>> GetUserProductsAsync(int userId, int page = 1, int pageSize = 20)
        {
            try
            {
                _logger.LogInformation("獲取用戶發布的商品，用戶ID: {UserId}, 頁碼: {Page}, 每頁大小: {PageSize}", 
                    userId, page, pageSize);
                
                var products = await _playerMarketRepository.GetUserProductsAsync(userId, page, pageSize);
                
                _logger.LogInformation("成功獲取用戶發布的商品，用戶ID: {UserId}, 共 {TotalCount} 件商品", 
                    userId, products.TotalCount);
                return products;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "獲取用戶發布的商品時發生錯誤，用戶ID: {UserId}", userId);
                throw;
            }
        }

        /// <summary>
        /// 創建新商品
        /// </summary>
        /// <param name="product">商品資料</param>
        /// <returns>創建結果</returns>
        public async Task<bool> CreateProductAsync(PlayerMarket product)
        {
            try
            {
                _logger.LogInformation("創建新玩家市場商品，商品名稱: {ProductName}, 賣家ID: {SellerId}", product.Name, product.SellerId);
                
                // 檢查賣家是否存在
                var seller = await _userRepository.GetByIdAsync(product.SellerId);
                if (seller == null)
                {
                    _logger.LogWarning("賣家不存在，無法創建商品，賣家ID: {SellerId}", product.SellerId);
                    return false;
                }
                
                // 設置創建時間
                product.CreatedAt = DateTime.UtcNow;
                product.UpdatedAt = DateTime.UtcNow;
                product.IsActive = true;
                
                var result = await _playerMarketRepository.CreateProductAsync(product);
                
                if (result)
                {
                    _logger.LogInformation("成功創建新玩家市場商品，商品ID: {ProductId}", product.ProductId);
                }
                else
                {
                    _logger.LogWarning("創建新玩家市場商品失敗，商品名稱: {ProductName}", product.Name);
                }
                
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "創建新玩家市場商品時發生錯誤，商品名稱: {ProductName}", product.Name);
                throw;
            }
        }

        /// <summary>
        /// 更新商品
        /// </summary>
        /// <param name="product">商品資料</param>
        /// <param name="userId">操作者ID</param>
        /// <returns>更新結果</returns>
        public async Task<bool> UpdateProductAsync(PlayerMarket product, int userId)
        {
            try
            {
                _logger.LogInformation("更新玩家市場商品，商品ID: {ProductId}, 操作者ID: {UserId}", product.ProductId, userId);
                
                // 檢查權限
                var existingProduct = await _playerMarketRepository.GetProductByIdAsync(product.ProductId);
                if (existingProduct == null)
                {
                    _logger.LogWarning("商品不存在，無法更新，商品ID: {ProductId}", product.ProductId);
                    return false;
                }
                
                if (existingProduct.SellerId != userId)
                {
                    _logger.LogWarning("用戶無權限更新此商品，商品ID: {ProductId}, 用戶ID: {UserId}", product.ProductId, userId);
                    return false;
                }
                
                // 設置更新時間
                product.UpdatedAt = DateTime.UtcNow;
                
                var result = await _playerMarketRepository.UpdateProductAsync(product);
                
                if (result)
                {
                    _logger.LogInformation("成功更新玩家市場商品，商品ID: {ProductId}", product.ProductId);
                }
                else
                {
                    _logger.LogWarning("更新玩家市場商品失敗，商品ID: {ProductId}", product.ProductId);
                }
                
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "更新玩家市場商品時發生錯誤，商品ID: {ProductId}", product.ProductId);
                throw;
            }
        }

        /// <summary>
        /// 刪除商品
        /// </summary>
        /// <param name="productId">商品ID</param>
        /// <param name="userId">操作者ID</param>
        /// <returns>刪除結果</returns>
        public async Task<bool> DeleteProductAsync(int productId, int userId)
        {
            try
            {
                _logger.LogInformation("刪除玩家市場商品，商品ID: {ProductId}, 操作者ID: {UserId}", productId, userId);
                
                // 檢查權限
                var product = await _playerMarketRepository.GetProductByIdAsync(productId);
                if (product == null)
                {
                    _logger.LogWarning("商品不存在，無法刪除，商品ID: {ProductId}", productId);
                    return false;
                }
                
                var user = await _userRepository.GetByIdAsync(userId);
                if (user == null)
                {
                    _logger.LogWarning("用戶不存在，無法刪除商品，用戶ID: {UserId}", userId);
                    return false;
                }
                
                // 檢查是否有權限刪除（賣家或管理員）
                if (product.SellerId != userId && user.Role != "Admin" && user.Role != "Manager")
                {
                    _logger.LogWarning("用戶無權限刪除此商品，商品ID: {ProductId}, 用戶ID: {UserId}", productId, userId);
                    return false;
                }
                
                var result = await _playerMarketRepository.DeleteProductAsync(productId);
                
                if (result)
                {
                    _logger.LogInformation("成功刪除玩家市場商品，商品ID: {ProductId}", productId);
                }
                else
                {
                    _logger.LogWarning("刪除玩家市場商品失敗，商品ID: {ProductId}", productId);
                }
                
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "刪除玩家市場商品時發生錯誤，商品ID: {ProductId}", productId);
                throw;
            }
        }

        /// <summary>
        /// 購買商品
        /// </summary>
        /// <param name="productId">商品ID</param>
        /// <param name="buyerId">買家ID</param>
        /// <param name="quantity">購買數量</param>
        /// <returns>購買結果</returns>
        public async Task<bool> PurchaseProductAsync(int productId, int buyerId, int quantity = 1)
        {
            try
            {
                _logger.LogInformation("購買玩家市場商品，商品ID: {ProductId}, 買家ID: {BuyerId}, 數量: {Quantity}", 
                    productId, buyerId, quantity);
                
                // 檢查商品是否存在且有效
                var product = await _playerMarketRepository.GetProductByIdAsync(productId);
                if (product == null || !product.IsActive)
                {
                    _logger.LogWarning("商品不存在或已下架，無法購買，商品ID: {ProductId}", productId);
                    return false;
                }
                
                // 檢查庫存
                if (product.Quantity < quantity)
                {
                    _logger.LogWarning("商品庫存不足，無法購買，商品ID: {ProductId}, 需求數量: {Quantity}, 庫存: {Stock}", 
                        productId, quantity, product.Quantity);
                    return false;
                }
                
                // 檢查買家是否存在
                var buyer = await _userRepository.GetByIdAsync(buyerId);
                if (buyer == null)
                {
                    _logger.LogWarning("買家不存在，無法購買商品，買家ID: {BuyerId}", buyerId);
                    return false;
                }
                
                // 檢查是否購買自己的商品
                if (product.SellerId == buyerId)
                {
                    _logger.LogWarning("不能購買自己的商品，商品ID: {ProductId}, 買家ID: {BuyerId}", productId, buyerId);
                    return false;
                }
                
                // 執行購買邏輯
                var result = await _playerMarketRepository.PurchaseProductAsync(productId, buyerId, quantity);
                
                if (result)
                {
                    // 發送通知給賣家
                    await _notificationService.CreateNotificationAsync(
                        product.SellerId,
                        "商品售出通知",
                        $"您的商品「{product.Name}」已售出 {quantity} 件，總價 {product.Price * quantity} 元。",
                        "market_sale"
                    );
                    
                    // 發送通知給買家
                    await _notificationService.CreateNotificationAsync(
                        buyerId,
                        "購買成功通知",
                        $"您已成功購買商品「{product.Name}」{quantity} 件，總價 {product.Price * quantity} 元。",
                        "market_purchase"
                    );
                    
                    _logger.LogInformation("成功購買玩家市場商品，商品ID: {ProductId}, 買家ID: {BuyerId}", productId, buyerId);
                }
                else
                {
                    _logger.LogWarning("購買玩家市場商品失敗，商品ID: {ProductId}, 買家ID: {BuyerId}", productId, buyerId);
                }
                
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "購買玩家市場商品時發生錯誤，商品ID: {ProductId}, 買家ID: {BuyerId}", productId, buyerId);
                throw;
            }
        }

        /// <summary>
        /// 獲取商品分類列表
        /// </summary>
        /// <returns>商品分類列表</returns>
        public async Task<List<string>> GetCategoriesAsync()
        {
            try
            {
                _logger.LogInformation("獲取玩家市場商品分類列表");
                
                var categories = await _playerMarketRepository.GetCategoriesAsync();
                
                _logger.LogInformation("成功獲取玩家市場商品分類列表，共 {Count} 個分類", categories.Count);
                return categories;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "獲取玩家市場商品分類列表時發生錯誤");
                throw;
            }
        }

        /// <summary>
        /// 獲取熱門商品
        /// </summary>
        /// <param name="category">商品分類</param>
        /// <param name="limit">限制數量</param>
        /// <returns>熱門商品列表</returns>
        public async Task<List<PlayerMarket>> GetPopularProductsAsync(string category = null, int limit = 10)
        {
            try
            {
                _logger.LogInformation("獲取玩家市場熱門商品，分類: {Category}, 限制: {Limit}", category, limit);
                
                var products = await _playerMarketRepository.GetPopularProductsAsync(category, limit);
                
                _logger.LogInformation("成功獲取玩家市場熱門商品，共 {Count} 件商品", products.Count);
                return products;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "獲取玩家市場熱門商品時發生錯誤");
                throw;
            }
        }
    }
}
