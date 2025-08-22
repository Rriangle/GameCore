using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using GameCore.Core.Interfaces;
using GameCore.Core.Entities;

namespace GameCore.Web.Controllers
{
    /// <summary>
    /// 官方商城控制器
    /// 處理商品瀏覽、購買、訂單管理等功能
    /// </summary>
    [Authorize]
    public class StoreController : Controller
    {
        private readonly IStoreService _storeService;
        private readonly ILogger<StoreController> _logger;

        public StoreController(IStoreService storeService, ILogger<StoreController> logger)
        {
            _storeService = storeService;
            _logger = logger;
        }

        /// <summary>
        /// 商城首頁
        /// 顯示熱門商品、推薦商品等
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> Index()
        {
            try
            {
                var viewModel = new StoreIndexViewModel
                {
                    FeaturedProducts = await _storeService.GetFeaturedProductsAsync(),
                    NewProducts = await _storeService.GetNewProductsAsync(),
                    PopularProducts = await _storeService.GetPopularProductsAsync(),
                    Categories = await _storeService.GetCategoriesAsync()
                };

                return View(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "載入商城首頁時發生錯誤");
                return View("Error");
            }
        }

        /// <summary>
        /// 商品列表頁面
        /// 支援分類篩選、搜尋、排序等功能
        /// </summary>
        /// <param name="category">商品分類</param>
        /// <param name="search">搜尋關鍵字</param>
        /// <param name="sort">排序方式</param>
        /// <param name="page">頁碼</param>
        /// <returns></returns>
        public async Task<IActionResult> Products(string? category, string? search, string sort = "newest", int page = 1)
        {
            try
            {
                var viewModel = new ProductListViewModel
                {
                    Products = await _storeService.GetProductsAsync(category, search, sort, page),
                    Categories = await _storeService.GetCategoriesAsync(),
                    CurrentCategory = category,
                    CurrentSearch = search,
                    CurrentSort = sort,
                    CurrentPage = page
                };

                return View(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "載入商品列表時發生錯誤");
                return View("Error");
            }
        }

        /// <summary>
        /// 商品詳情頁面
        /// </summary>
        /// <param name="id">商品ID</param>
        /// <returns></returns>
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var product = await _storeService.GetProductDetailsAsync(id);
                if (product == null)
                {
                    return NotFound();
                }

                var viewModel = new ProductDetailsViewModel
                {
                    Product = product,
                    RelatedProducts = await _storeService.GetRelatedProductsAsync(id),
                    Reviews = await _storeService.GetProductReviewsAsync(id)
                };

                return View(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "載入商品詳情時發生錯誤，商品ID: {ProductId}", id);
                return View("Error");
            }
        }

        /// <summary>
        /// 購物車頁面
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> Cart()
        {
            try
            {
                var userId = GetCurrentUserId();
                var cartItems = await _storeService.GetCartItemsAsync(userId);
                
                var viewModel = new CartViewModel
                {
                    Items = cartItems,
                    Total = cartItems.Sum(item => item.Price * item.Quantity)
                };

                return View(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "載入購物車時發生錯誤");
                return View("Error");
            }
        }

        /// <summary>
        /// 添加商品到購物車
        /// </summary>
        /// <param name="productId">商品ID</param>
        /// <param name="quantity">數量</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> AddToCart(int productId, int quantity = 1)
        {
            try
            {
                var userId = GetCurrentUserId();
                await _storeService.AddToCartAsync(userId, productId, quantity);
                
                return Json(new { success = true, message = "商品已添加到購物車" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "添加商品到購物車時發生錯誤，商品ID: {ProductId}", productId);
                return Json(new { success = false, message = "添加失敗，請稍後再試" });
            }
        }

        /// <summary>
        /// 結帳頁面
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> Checkout()
        {
            try
            {
                var userId = GetCurrentUserId();
                var cartItems = await _storeService.GetCartItemsAsync(userId);
                
                if (!cartItems.Any())
                {
                    return RedirectToAction("Cart");
                }

                var viewModel = new CheckoutViewModel
                {
                    Items = cartItems,
                    Total = cartItems.Sum(item => item.Price * item.Quantity),
                    UserAddress = await _storeService.GetUserAddressAsync(userId)
                };

                return View(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "載入結帳頁面時發生錯誤");
                return View("Error");
            }
        }

        /// <summary>
        /// 處理訂單提交
        /// </summary>
        /// <param name="model">結帳資訊</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> PlaceOrder(CheckoutViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View("Checkout", model);
                }

                var userId = GetCurrentUserId();
                var orderId = await _storeService.PlaceOrderAsync(userId, model);
                
                return RedirectToAction("OrderSuccess", new { id = orderId });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "提交訂單時發生錯誤");
                ModelState.AddModelError("", "訂單提交失敗，請稍後再試");
                return View("Checkout", model);
            }
        }

        /// <summary>
        /// 訂單成功頁面
        /// </summary>
        /// <param name="id">訂單ID</param>
        /// <returns></returns>
        public async Task<IActionResult> OrderSuccess(int id)
        {
            try
            {
                var userId = GetCurrentUserId();
                var order = await _storeService.GetOrderAsync(userId, id);
                
                if (order == null)
                {
                    return NotFound();
                }

                return View(order);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "載入訂單成功頁面時發生錯誤，訂單ID: {OrderId}", id);
                return View("Error");
            }
        }

        /// <summary>
        /// 我的訂單列表
        /// </summary>
        /// <param name="status">訂單狀態</param>
        /// <param name="page">頁碼</param>
        /// <returns></returns>
        public async Task<IActionResult> Orders(string? status, int page = 1)
        {
            try
            {
                var userId = GetCurrentUserId();
                var viewModel = new OrderListViewModel
                {
                    Orders = await _storeService.GetUserOrdersAsync(userId, status, page),
                    CurrentStatus = status,
                    CurrentPage = page
                };

                return View(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "載入訂單列表時發生錯誤");
                return View("Error");
            }
        }

        /// <summary>
        /// 獲取當前使用者ID
        /// </summary>
        /// <returns></returns>
        private int GetCurrentUserId()
        {
            // 這裡應該從 Claims 中獲取真實的使用者ID
            // 暫時返回示例ID
            return 1;
        }
    }

    #region ViewModels

    /// <summary>
    /// 商城首頁視圖模型
    /// </summary>
    public class StoreIndexViewModel
    {
        public List<ProductInfo> FeaturedProducts { get; set; } = new();
        public List<ProductInfo> NewProducts { get; set; } = new();
        public List<ProductInfo> PopularProducts { get; set; } = new();
        public List<string> Categories { get; set; } = new();
    }

    /// <summary>
    /// 商品列表視圖模型
    /// </summary>
    public class ProductListViewModel
    {
        public List<ProductInfo> Products { get; set; } = new();
        public List<string> Categories { get; set; } = new();
        public string? CurrentCategory { get; set; }
        public string? CurrentSearch { get; set; }
        public string CurrentSort { get; set; } = "newest";
        public int CurrentPage { get; set; } = 1;
        public int TotalPages { get; set; }
    }

    /// <summary>
    /// 商品詳情視圖模型
    /// </summary>
    public class ProductDetailsViewModel
    {
        public ProductInfo Product { get; set; } = null!;
        public List<ProductInfo> RelatedProducts { get; set; } = new();
        public List<ProductReview> Reviews { get; set; } = new();
    }

    /// <summary>
    /// 購物車視圖模型
    /// </summary>
    public class CartViewModel
    {
        public List<CartItem> Items { get; set; } = new();
        public decimal Total { get; set; }
    }

    /// <summary>
    /// 結帳視圖模型
    /// </summary>
    public class CheckoutViewModel
    {
        public List<CartItem> Items { get; set; } = new();
        public decimal Total { get; set; }
        public string UserAddress { get; set; } = string.Empty;
        public string PaymentMethod { get; set; } = string.Empty;
        public string DeliveryMethod { get; set; } = string.Empty;
        public string? Notes { get; set; }
    }

    /// <summary>
    /// 訂單列表視圖模型
    /// </summary>
    public class OrderListViewModel
    {
        public List<OrderInfo> Orders { get; set; } = new();
        public string? CurrentStatus { get; set; }
        public int CurrentPage { get; set; } = 1;
        public int TotalPages { get; set; }
    }

    /// <summary>
    /// 購物車項目
    /// </summary>
    public class CartItem
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public string ImageUrl { get; set; } = string.Empty;
    }

    /// <summary>
    /// 商品評價
    /// </summary>
    public class ProductReview
    {
        public string UserName { get; set; } = string.Empty;
        public int Rating { get; set; }
        public string Comment { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }

    #endregion
}
