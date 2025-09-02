using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using GameCore.Core.Services;
using GameCore.Core.Entities;
using GameCore.Core.DTOs;
using Microsoft.Extensions.Logging;

namespace GameCore.Web.Controllers
{
    /// <summary>
    /// 商城前端頁面控制器
    /// </summary>
    [Authorize]
    public class StoreMvcController : Controller
    {
        private readonly IStoreService _storeService;
        private readonly ILogger<StoreMvcController> _logger;

        public StoreMvcController(IStoreService storeService, ILogger<StoreMvcController> logger)
        {
            _storeService = storeService;
            _logger = logger;
        }

        /// <summary>
        /// 商城首頁 - 顯示商品列表
        /// </summary>
        [AllowAnonymous]
        public async Task<IActionResult> Index(string? category = null, string? search = null, 
            decimal? minPrice = null, decimal? maxPrice = null, int page = 1)
        {
            try
            {
                var pageSize = 12; // 每頁顯示 12 個商品
                
                // 根據搜尋條件獲取商品
                IEnumerable<StoreProduct> products;
                if (!string.IsNullOrEmpty(search))
                {
                    products = await _storeService.SearchProductsAsync(search, category, minPrice, maxPrice, page, pageSize);
                }
                else
                {
                    products = await _storeService.GetActiveProductsAsync(category, page, pageSize);
                }

                // 獲取商品分類
                var categories = await _storeService.GetProductCategoriesAsync();
                
                // 獲取熱門商品
                var popularProducts = await _storeService.GetPopularProductsAsync(6);
                
                // 獲取銷售排行榜
                var salesRanking = await _storeService.GetSalesRankingAsync("weekly", 10);

                var viewModel = new StoreIndexViewModel
                {
                    Products = products.ToList(),
                    Categories = categories.ToList(),
                    PopularProducts = popularProducts.ToList(),
                    SalesRanking = salesRanking.ToList(),
                    CurrentCategory = category,
                    CurrentSearch = search,
                    CurrentMinPrice = minPrice,
                    CurrentMaxPrice = maxPrice,
                    CurrentPage = page,
                    TotalPages = (int)Math.Ceiling(products.Count() / (double)pageSize)
                };

                return View(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "獲取商城首頁資料失敗");
                return View("Error");
            }
        }

        /// <summary>
        /// 商品詳情頁面
        /// </summary>
        [AllowAnonymous]
        public async Task<IActionResult> Product(int id)
        {
            try
            {
                var product = await _storeService.GetProductAsync(id);
                if (product == null)
                {
                    return NotFound();
                }

                // 獲取相關商品推薦
                var relatedProducts = await _storeService.GetRelatedProductsAsync(id, 4);

                var viewModel = new ProductDetailViewModel
                {
                    Product = product,
                    RelatedProducts = relatedProducts.ToList()
                };

                return View(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "獲取商品詳情失敗: {ProductId}", id);
                return View("Error");
            }
        }

        /// <summary>
        /// 購物車頁面
        /// </summary>
        public async Task<IActionResult> Cart()
        {
            try
            {
                var userId = GetCurrentUserId();
                var cartItems = await _storeService.GetUserCartAsync(userId);
                
                var viewModel = new CartViewModel
                {
                    CartItems = cartItems.ToList(),
                    TotalAmount = cartItems.Sum(item => item.Price * item.Quantity)
                };

                return View(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "獲取購物車失敗");
                return View("Error");
            }
        }

        /// <summary>
        /// 結帳頁面
        /// </summary>
        public async Task<IActionResult> Checkout()
        {
            try
            {
                var userId = GetCurrentUserId();
                var cartItems = await _storeService.GetUserCartAsync(userId);
                
                if (!cartItems.Any())
                {
                    return RedirectToAction(nameof(Cart));
                }

                var viewModel = new CheckoutViewModel
                {
                    CartItems = cartItems.ToList(),
                    TotalAmount = cartItems.Sum(item => item.Price * item.Quantity),
                    ShippingAddress = "", // 從用戶資料獲取
                    PaymentMethod = "credit_card" // 預設支付方式
                };

                return View(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "獲取結帳頁面失敗");
                return View("Error");
            }
        }

        /// <summary>
        /// 訂單確認頁面
        /// </summary>
        public async Task<IActionResult> OrderConfirmation(int orderId)
        {
            try
            {
                var userId = GetCurrentUserId();
                var order = await _storeService.GetOrderAsync(orderId);
                
                if (order == null || order.UserId != userId)
                {
                    return NotFound();
                }

                return View(order);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "獲取訂單確認頁面失敗: {OrderId}", orderId);
                return View("Error");
            }
        }

        /// <summary>
        /// 我的訂單頁面
        /// </summary>
        public async Task<IActionResult> MyOrders(int page = 1)
        {
            try
            {
                var userId = GetCurrentUserId();
                var pageSize = 10;
                var orders = await _storeService.GetUserOrdersAsync(userId, page, pageSize);
                
                var viewModel = new MyOrdersViewModel
                {
                    Orders = orders.ToList(),
                    CurrentPage = page,
                    TotalPages = (int)Math.Ceiling(orders.Count() / (double)pageSize)
                };

                return View(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "獲取用戶訂單失敗");
                return View("Error");
            }
        }

        /// <summary>
        /// 商品分類頁面
        /// </summary>
        [AllowAnonymous]
        public async Task<IActionResult> Category(string category, int page = 1)
        {
            try
            {
                var pageSize = 12;
                var products = await _storeService.GetActiveProductsAsync(category, page, pageSize);
                var categories = await _storeService.GetProductCategoriesAsync();

                var viewModel = new CategoryViewModel
                {
                    Products = products.ToList(),
                    Categories = categories.ToList(),
                    CurrentCategory = category,
                    CurrentPage = page,
                    TotalPages = (int)Math.Ceiling(products.Count() / (double)pageSize)
                };

                return View(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "獲取分類頁面失敗: {Category}", category);
                return View("Error");
            }
        }

        /// <summary>
        /// 搜尋結果頁面
        /// </summary>
        [AllowAnonymous]
        public async Task<IActionResult> Search(string q, string? category = null, 
            decimal? minPrice = null, decimal? maxPrice = null, int page = 1)
        {
            try
            {
                if (string.IsNullOrEmpty(q))
                {
                    return RedirectToAction(nameof(Index));
                }

                var pageSize = 12;
                var products = await _storeService.SearchProductsAsync(q, category, minPrice, maxPrice, page, pageSize);
                var categories = await _storeService.GetProductCategoriesAsync();

                var viewModel = new SearchResultViewModel
                {
                    Products = products.ToList(),
                    Categories = categories.ToList(),
                    SearchQuery = q,
                    CurrentCategory = category,
                    CurrentMinPrice = minPrice,
                    CurrentMaxPrice = maxPrice,
                    CurrentPage = page,
                    TotalPages = (int)Math.Ceiling(products.Count() / (double)pageSize),
                    TotalResults = products.Count()
                };

                return View(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "搜尋商品失敗: {Query}", q);
                return View("Error");
            }
        }

        /// <summary>
        /// 獲取當前用戶 ID
        /// </summary>
        private int GetCurrentUserId()
        {
            // 從 JWT Token 或 Session 獲取用戶 ID
            // 這裡需要根據實際的認證機制來實作
            var userIdClaim = User.FindFirst("UserId");
            if (userIdClaim != null && int.TryParse(userIdClaim.Value, out int userId))
            {
                return userId;
            }
            
            // 如果無法獲取，返回預設值（實際應用中應該拋出異常）
            return 1;
        }
    }

    #region View Models

    /// <summary>
    /// 商城首頁視圖模型
    /// </summary>
    public class StoreIndexViewModel
    {
        public List<StoreProduct> Products { get; set; } = new();
        public List<string> Categories { get; set; } = new();
        public List<StoreProduct> PopularProducts { get; set; } = new();
        public List<StoreProduct> SalesRanking { get; set; } = new();
        public string? CurrentCategory { get; set; }
        public string? CurrentSearch { get; set; }
        public decimal? CurrentMinPrice { get; set; }
        public decimal? CurrentMaxPrice { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
    }

    /// <summary>
    /// 商品詳情視圖模型
    /// </summary>
    public class ProductDetailViewModel
    {
        public StoreProduct Product { get; set; } = null!;
        public List<StoreProduct> RelatedProducts { get; set; } = new();
    }

    /// <summary>
    /// 購物車視圖模型
    /// </summary>
    public class CartViewModel
    {
        public List<ShoppingCart> CartItems { get; set; } = new();
        public decimal TotalAmount { get; set; }
    }

    /// <summary>
    /// 結帳視圖模型
    /// </summary>
    public class CheckoutViewModel
    {
        public List<ShoppingCart> CartItems { get; set; } = new();
        public decimal TotalAmount { get; set; }
        public string ShippingAddress { get; set; } = string.Empty;
        public string PaymentMethod { get; set; } = string.Empty;
    }

    /// <summary>
    /// 我的訂單視圖模型
    /// </summary>
    public class MyOrdersViewModel
    {
        public List<StoreOrder> Orders { get; set; } = new();
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
    }

    /// <summary>
    /// 分類頁面視圖模型
    /// </summary>
    public class CategoryViewModel
    {
        public List<StoreProduct> Products { get; set; } = new();
        public List<string> Categories { get; set; } = new();
        public string CurrentCategory { get; set; } = string.Empty;
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
    }

    /// <summary>
    /// 搜尋結果視圖模型
    /// </summary>
    public class SearchResultViewModel
    {
        public List<StoreProduct> Products { get; set; } = new();
        public List<string> Categories { get; set; } = new();
        public string SearchQuery { get; set; } = string.Empty;
        public string? CurrentCategory { get; set; }
        public decimal? CurrentMinPrice { get; set; }
        public decimal? CurrentMaxPrice { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int TotalResults { get; set; }
    }

    #endregion
} 