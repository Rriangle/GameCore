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
    /// 官方商店服務實現類
    /// 提供官方商店相關的業務邏輯處理
    /// </summary>
    public class StoreService : IStoreService
    {
        private readonly IStoreRepository _storeRepository;
        private readonly IUserRepository _userRepository;
        private readonly INotificationService _notificationService;
        private readonly ILogger<StoreService> _logger;

        public StoreService(
            IStoreRepository storeRepository,
            IUserRepository userRepository,
            INotificationService notificationService,
            ILogger<StoreService> logger)
        {
            _storeRepository = storeRepository;
            _userRepository = userRepository;
            _notificationService = notificationService;
            _logger = logger;
        }

        /// <summary>
        /// 獲取商品列表
        /// </summary>
        /// <param name="category">商品分類</param>
        /// <param name="page">頁碼</param>
        /// <param name="pageSize">每頁大小</param>
        /// <param name="sortBy">排序方式</param>
        /// <returns>商品列表</returns>
        public async Task<PagedResult<Store>> GetProductsAsync(string category = null, int page = 1, int pageSize = 20, string sortBy = "name")
        {
            try
            {
                _logger.LogInformation("獲取商品列表，分類: {Category}, 頁碼: {Page}, 每頁大小: {PageSize}, 排序: {SortBy}", 
                    category, page, pageSize, sortBy);
                
                var products = await _storeRepository.GetProductsAsync(category, page, pageSize, sortBy);
                
                _logger.LogInformation("成功獲取商品列表，共 {TotalCount} 件商品", products.TotalCount);
                return products;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "獲取商品列表時發生錯誤");
                throw;
            }
        }

        /// <summary>
        /// 獲取商品詳情
        /// </summary>
        /// <param name="productId">商品ID</param>
        /// <returns>商品詳情</returns>
        public async Task<Store> GetProductByIdAsync(int productId)
        {
            try
            {
                _logger.LogInformation("獲取商品詳情，商品ID: {ProductId}", productId);
                
                var product = await _storeRepository.GetProductByIdAsync(productId);
                
                if (product == null)
                {
                    _logger.LogWarning("商品不存在，商品ID: {ProductId}", productId);
                    return null;
                }
                
                _logger.LogInformation("成功獲取商品詳情，商品ID: {ProductId}", productId);
                return product;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "獲取商品詳情時發生錯誤，商品ID: {ProductId}", productId);
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
        public async Task<PagedResult<Store>> SearchProductsAsync(string searchTerm, string category = null, 
            decimal? minPrice = null, decimal? maxPrice = null, int page = 1, int pageSize = 20)
        {
            try
            {
                _logger.LogInformation("搜尋商品，關鍵字: {SearchTerm}, 分類: {Category}, 價格範圍: {MinPrice}-{MaxPrice}, 頁碼: {Page}", 
                    searchTerm, category, minPrice, maxPrice, page);
                
                var result = await _storeRepository.SearchProductsAsync(searchTerm, category, minPrice, maxPrice, page, pageSize);
                
                _logger.LogInformation("成功搜尋商品，關鍵字: {SearchTerm}, 共 {TotalCount} 件商品", 
                    searchTerm, result.TotalCount);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "搜尋商品時發生錯誤，關鍵字: {SearchTerm}", searchTerm);
                throw;
            }
        }

        /// <summary>
        /// 獲取熱門商品
        /// </summary>
        /// <param name="category">商品分類</param>
        /// <param name="limit">限制數量</param>
        /// <returns>熱門商品列表</returns>
        public async Task<List<Store>> GetPopularProductsAsync(string category = null, int limit = 10)
        {
            try
            {
                _logger.LogInformation("獲取熱門商品，分類: {Category}, 限制: {Limit}", category, limit);
                
                var products = await _storeRepository.GetPopularProductsAsync(category, limit);
                
                _logger.LogInformation("成功獲取熱門商品，共 {Count} 件商品", products.Count);
                return products;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "獲取熱門商品時發生錯誤");
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
                _logger.LogInformation("獲取商品分類列表");
                
                var categories = await _storeRepository.GetCategoriesAsync();
                
                _logger.LogInformation("成功獲取商品分類列表，共 {Count} 個分類", categories.Count);
                return categories;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "獲取商品分類列表時發生錯誤");
                throw;
            }
        }

        /// <summary>
        /// 創建新商品
        /// </summary>
        /// <param name="product">商品資料</param>
        /// <param name="userId">創建者ID</param>
        /// <returns>創建結果</returns>
        public async Task<bool> CreateProductAsync(Store product, int userId)
        {
            try
            {
                _logger.LogInformation("創建新商品，商品名稱: {ProductName}, 創建者ID: {UserId}", product.Name, userId);
                
                // 檢查權限
                var user = await _userRepository.GetByIdAsync(userId);
                if (user == null || (user.Role != "Admin" && user.Role != "Manager"))
                {
                    _logger.LogWarning("用戶無權限創建商品，用戶ID: {UserId}", userId);
                    return false;
                }
                
                // 設置創建時間和創建者
                product.CreatedAt = DateTime.UtcNow;
                product.UpdatedAt = DateTime.UtcNow;
                product.CreatedBy = userId;
                
                var result = await _storeRepository.CreateProductAsync(product);
                
                if (result)
                {
                    _logger.LogInformation("成功創建新商品，商品ID: {ProductId}", product.ProductId);
                }
                else
                {
                    _logger.LogWarning("創建新商品失敗，商品名稱: {ProductName}", product.Name);
                }
                
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "創建新商品時發生錯誤，商品名稱: {ProductName}", product.Name);
                throw;
            }
        }

        /// <summary>
        /// 更新商品
        /// </summary>
        /// <param name="product">商品資料</param>
        /// <param name="userId">更新者ID</param>
        /// <returns>更新結果</returns>
        public async Task<bool> UpdateProductAsync(Store product, int userId)
        {
            try
            {
                _logger.LogInformation("更新商品，商品ID: {ProductId}, 更新者ID: {UserId}", product.ProductId, userId);
                
                // 檢查權限
                var user = await _userRepository.GetByIdAsync(userId);
                if (user == null || (user.Role != "Admin" && user.Role != "Manager"))
                {
                    _logger.LogWarning("用戶無權限更新商品，用戶ID: {UserId}", userId);
                    return false;
                }
                
                // 設置更新時間
                product.UpdatedAt = DateTime.UtcNow;
                
                var result = await _storeRepository.UpdateProductAsync(product);
                
                if (result)
                {
                    _logger.LogInformation("成功更新商品，商品ID: {ProductId}", product.ProductId);
                }
                else
                {
                    _logger.LogWarning("更新商品失敗，商品ID: {ProductId}", product.ProductId);
                }
                
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "更新商品時發生錯誤，商品ID: {ProductId}", product.ProductId);
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
                _logger.LogInformation("刪除商品，商品ID: {ProductId}, 操作者ID: {UserId}", productId, userId);
                
                // 檢查權限
                var user = await _userRepository.GetByIdAsync(userId);
                if (user == null || (user.Role != "Admin" && user.Role != "Manager"))
                {
                    _logger.LogWarning("用戶無權限刪除商品，用戶ID: {UserId}", userId);
                    return false;
                }
                
                var result = await _storeRepository.DeleteProductAsync(productId);
                
                if (result)
                {
                    _logger.LogInformation("成功刪除商品，商品ID: {ProductId}", productId);
                }
                else
                {
                    _logger.LogWarning("刪除商品失敗，商品ID: {ProductId}", productId);
                }
                
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "刪除商品時發生錯誤，商品ID: {ProductId}", productId);
                throw;
            }
        }

        /// <summary>
        /// 更新商品庫存
        /// </summary>
        /// <param name="productId">商品ID</param>
        /// <param name="quantity">數量變化</param>
        /// <param name="userId">操作者ID</param>
        /// <returns>更新結果</returns>
        public async Task<bool> UpdateStockAsync(int productId, int quantity, int userId)
        {
            try
            {
                _logger.LogInformation("更新商品庫存，商品ID: {ProductId}, 數量變化: {Quantity}, 操作者ID: {UserId}", 
                    productId, quantity, userId);
                
                // 檢查權限
                var user = await _userRepository.GetByIdAsync(userId);
                if (user == null || (user.Role != "Admin" && user.Role != "Manager"))
                {
                    _logger.LogWarning("用戶無權限更新庫存，用戶ID: {UserId}", userId);
                    return false;
                }
                
                var result = await _storeRepository.UpdateStockAsync(productId, quantity);
                
                if (result)
                {
                    _logger.LogInformation("成功更新商品庫存，商品ID: {ProductId}", productId);
                }
                else
                {
                    _logger.LogWarning("更新商品庫存失敗，商品ID: {ProductId}", productId);
                }
                
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "更新商品庫存時發生錯誤，商品ID: {ProductId}", productId);
                throw;
            }
        }

        /// <summary>
        /// 獲取商品統計資料
        /// </summary>
        /// <returns>商品統計資料</returns>
        public async Task<StoreStats> GetStoreStatsAsync()
        {
            try
            {
                _logger.LogInformation("獲取商品統計資料");
                
                var stats = await _storeRepository.GetStoreStatsAsync();
                
                _logger.LogInformation("成功獲取商品統計資料");
                return stats;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "獲取商品統計資料時發生錯誤");
                throw;
            }
        }
    }

    /// <summary>
    /// 商店統計資料 DTO
    /// </summary>
    public class StoreStats
    {
        public int TotalProducts { get; set; }
        public int ActiveProducts { get; set; }
        public int LowStockProducts { get; set; }
        public int OutOfStockProducts { get; set; }
        public decimal TotalValue { get; set; }
        public List<string> TopCategories { get; set; }
    }
}
