using GameCore.Core.DTOs;

namespace GameCore.Core.Services
{
    /// <summary>
    /// 官方商城服務介面 - 完整實現B2C電商功能
    /// 提供商品管理、購物車操作、訂單流程、排行榜分析等完整商城服務
    /// 嚴格按照規格要求實現商城業務邏輯和狀態機管理
    /// </summary>
    public interface IStoreService
    {
        #region 商品管理

        /// <summary>
        /// 搜尋商品
        /// 支援關鍵字、分類、價格區間、庫存狀態等多維度搜尋
        /// </summary>
        /// <param name="searchDto">搜尋條件</param>
        /// <returns>分頁搜尋結果</returns>
        Task<PagedResult<ProductListDto>> SearchProductsAsync(ProductSearchDto searchDto);

        /// <summary>
        /// 取得商品詳細資訊
        /// 包含基本資訊、供應商資料、遊戲/其他商品詳細資訊
        /// </summary>
        /// <param name="productId">商品ID</param>
        /// <returns>商品詳細資訊</returns>
        Task<ProductDetailDto?> GetProductDetailAsync(int productId);

        /// <summary>
        /// 取得商品分類列表
        /// 從ProductInfo表的product_type欄位統計所有分類
        /// </summary>
        /// <returns>分類列表</returns>
        Task<List<string>> GetProductCategoriesAsync();

        /// <summary>
        /// 取得熱門商品
        /// 基於銷售量、交易額或排行榜資料計算熱門度
        /// </summary>
        /// <param name="limit">限制筆數</param>
        /// <returns>熱門商品列表</returns>
        Task<List<ProductListDto>> GetPopularProductsAsync(int limit = 10);

        #endregion

        #region 購物車管理

        /// <summary>
        /// 取得使用者購物車
        /// 從Session或資料庫載入購物車資料，包含商品資訊和庫存檢查
        /// </summary>
        /// <param name="userId">使用者ID</param>
        /// <returns>購物車資訊</returns>
        Task<ShoppingCartDto> GetCartAsync(int userId);

        /// <summary>
        /// 加入商品到購物車
        /// 檢查商品存在、庫存充足、購物權限等條件
        /// </summary>
        /// <param name="userId">使用者ID</param>
        /// <param name="addToCartDto">加入購物車請求</param>
        /// <returns>操作結果和更新後的購物車</returns>
        Task<ServiceResult<ShoppingCartDto>> AddToCartAsync(int userId, AddToCartDto addToCartDto);

        /// <summary>
        /// 更新購物車項目數量
        /// 若數量為0則移除該項目，檢查庫存充足性
        /// </summary>
        /// <param name="userId">使用者ID</param>
        /// <param name="updateCartItemDto">更新購物車項目請求</param>
        /// <returns>操作結果和更新後的購物車</returns>
        Task<ServiceResult<ShoppingCartDto>> UpdateCartItemAsync(int userId, UpdateCartItemDto updateCartItemDto);

        /// <summary>
        /// 移除購物車項目
        /// 從購物車中完全移除指定商品
        /// </summary>
        /// <param name="userId">使用者ID</param>
        /// <param name="productId">商品ID</param>
        /// <returns>操作結果和更新後的購物車</returns>
        Task<ServiceResult<ShoppingCartDto>> RemoveFromCartAsync(int userId, int productId);

        /// <summary>
        /// 清空購物車
        /// 移除購物車中的所有項目
        /// </summary>
        /// <param name="userId">使用者ID</param>
        /// <returns>操作結果</returns>
        Task<ServiceResult> ClearCartAsync(int userId);

        #endregion

        #region 訂單管理

        /// <summary>
        /// 建立訂單
        /// 從指定商品項目建立訂單，檢查庫存、計算金額、建立OrderInfo和OrderItems記錄
        /// </summary>
        /// <param name="userId">使用者ID</param>
        /// <param name="createOrderDto">建立訂單請求</param>
        /// <returns>操作結果和訂單資訊</returns>
        Task<ServiceResult<OrderDto>> CreateOrderAsync(int userId, CreateOrderDto createOrderDto);

        /// <summary>
        /// 從購物車建立訂單
        /// 將當前購物車內容轉為訂單，成功後清空購物車
        /// </summary>
        /// <param name="userId">使用者ID</param>
        /// <returns>操作結果和訂單資訊</returns>
        Task<ServiceResult<OrderDto>> CreateOrderFromCartAsync(int userId);

        /// <summary>
        /// 取得訂單詳細資訊
        /// 檢查使用者權限，返回訂單基本資訊和項目明細
        /// </summary>
        /// <param name="userId">使用者ID</param>
        /// <param name="orderId">訂單ID</param>
        /// <returns>訂單詳細資訊</returns>
        Task<OrderDto?> GetOrderAsync(int userId, int orderId);

        /// <summary>
        /// 取得使用者訂單列表
        /// 支援狀態篩選、日期範圍、分頁排序
        /// </summary>
        /// <param name="userId">使用者ID</param>
        /// <param name="queryDto">查詢條件</param>
        /// <returns>分頁訂單列表</returns>
        Task<PagedResult<OrderDto>> GetOrdersAsync(int userId, OrderQueryDto queryDto);

        /// <summary>
        /// 處理訂單付款 (模擬)
        /// 更新付款狀態、檢核庫存、扣減庫存、更新訂單狀態至ToShip
        /// </summary>
        /// <param name="userId">使用者ID</param>
        /// <param name="orderId">訂單ID</param>
        /// <returns>操作結果和更新後的訂單</returns>
        Task<ServiceResult<OrderDto>> ProcessPaymentAsync(int userId, int orderId);

        #endregion

        #region 排行榜

        /// <summary>
        /// 取得商城排行榜
        /// 從Official_Store_Ranking表查詢指定期間和指標的排行榜
        /// </summary>
        /// <param name="queryDto">查詢條件</param>
        /// <returns>排行榜列表</returns>
        Task<List<StoreRankingDto>> GetRankingsAsync(RankingQueryDto queryDto);

        /// <summary>
        /// 更新商城排行榜
        /// 依日/月/季/年計算商品交易額、交易量，寫入Official_Store_Ranking表
        /// </summary>
        /// <param name="periodType">榜單型態</param>
        /// <param name="date">計算日期</param>
        /// <returns>更新結果</returns>
        Task<ServiceResult> UpdateRankingsAsync(string periodType, DateTime date);

        #endregion

        #region 統計分析

        /// <summary>
        /// 取得商城統計資訊
        /// 計算商品數量、訂單統計、交易額、分類分析等綜合指標
        /// </summary>
        /// <returns>商城統計資訊</returns>
        Task<StoreStatisticsDto> GetStatisticsAsync();

        #endregion

        #region 管理員功能

        /// <summary>
        /// 管理員取得所有訂單
        /// 跨使用者查詢訂單，支援複雜篩選條件
        /// </summary>
        /// <param name="queryDto">查詢條件</param>
        /// <returns>分頁訂單列表</returns>
        Task<PagedResult<OrderDto>> GetAllOrdersAsync(OrderQueryDto queryDto);

        /// <summary>
        /// 管理員更新訂單狀態
        /// 更新訂單狀態和付款狀態，記錄異動日誌
        /// </summary>
        /// <param name="orderId">訂單ID</param>
        /// <param name="statusDto">狀態更新請求</param>
        /// <returns>操作結果和更新後的訂單</returns>
        Task<ServiceResult<OrderDto>> UpdateOrderStatusAsync(int orderId, UpdateOrderStatusDto statusDto);

        /// <summary>
        /// 管理員完成訂單出貨
        /// 更新訂單狀態為Shipped，填入shipped_at時間
        /// </summary>
        /// <param name="orderId">訂單ID</param>
        /// <returns>操作結果</returns>
        Task<ServiceResult<OrderDto>> ShipOrderAsync(int orderId);

        /// <summary>
        /// 管理員完成訂單
        /// 更新訂單狀態為Completed，填入completed_at時間
        /// </summary>
        /// <param name="orderId">訂單ID</param>
        /// <returns>操作結果</returns>
        Task<ServiceResult<OrderDto>> CompleteOrderAsync(int orderId);

        #endregion

        #region 庫存管理

        /// <summary>
        /// 檢查商品庫存
        /// 驗證指定商品和數量的庫存充足性
        /// </summary>
        /// <param name="productId">商品ID</param>
        /// <param name="quantity">需求數量</param>
        /// <returns>庫存檢查結果</returns>
        Task<bool> CheckStockAsync(int productId, int quantity);

        /// <summary>
        /// 扣減商品庫存
        /// 原子性地扣減商品庫存，防止超賣
        /// </summary>
        /// <param name="productId">商品ID</param>
        /// <param name="quantity">扣減數量</param>
        /// <returns>扣減結果</returns>
        Task<ServiceResult> DeductStockAsync(int productId, int quantity);

        /// <summary>
        /// 歸還商品庫存
        /// 取消訂單時歸還已扣減的庫存
        /// </summary>
        /// <param name="productId">商品ID</param>
        /// <param name="quantity">歸還數量</param>
        /// <returns>歸還結果</returns>
        Task<ServiceResult> RestoreStockAsync(int productId, int quantity);

        #endregion

        #region 輔助方法

        /// <summary>
        /// 檢查使用者購物權限
        /// 從User_Rights表檢查ShoppingPermission
        /// </summary>
        /// <param name="userId">使用者ID</param>
        /// <returns>是否有購物權限</returns>
        Task<bool> CheckShoppingPermissionAsync(int userId);

        /// <summary>
        /// 驗證訂單狀態轉換
        /// 按照狀態機規則驗證狀態轉換的合法性
        /// </summary>
        /// <param name="currentStatus">當前狀態</param>
        /// <param name="newStatus">新狀態</param>
        /// <returns>是否允許轉換</returns>
        bool ValidateStatusTransition(string currentStatus, string newStatus);

        #endregion
    }

    #region 通用結果類別

    /// <summary>
    /// 服務執行結果
    /// </summary>
    public class ServiceResult
    {
        /// <summary>是否成功</summary>
        public bool Success { get; set; }

        /// <summary>訊息</summary>
        public string Message { get; set; } = string.Empty;

        /// <summary>錯誤清單</summary>
        public List<string> Errors { get; set; } = new();

        /// <summary>建立成功結果</summary>
        public static ServiceResult CreateSuccess(string message = "操作成功")
        {
            return new ServiceResult { Success = true, Message = message };
        }

        /// <summary>建立失敗結果</summary>
        public static ServiceResult CreateFailure(string message, List<string>? errors = null)
        {
            return new ServiceResult 
            { 
                Success = false, 
                Message = message, 
                Errors = errors ?? new List<string>() 
            };
        }
    }

    /// <summary>
    /// 帶資料的服務執行結果
    /// </summary>
    /// <typeparam name="T">資料類型</typeparam>
    public class ServiceResult<T> : ServiceResult
    {
        /// <summary>結果資料</summary>
        public T? Data { get; set; }

        /// <summary>建立成功結果</summary>
        public static ServiceResult<T> CreateSuccess(T data, string message = "操作成功")
        {
            return new ServiceResult<T> 
            { 
                Success = true, 
                Message = message, 
                Data = data 
            };
        }

        /// <summary>建立失敗結果</summary>
        public static new ServiceResult<T> CreateFailure(string message, List<string>? errors = null)
        {
            return new ServiceResult<T> 
            { 
                Success = false, 
                Message = message, 
                Errors = errors ?? new List<string>() 
            };
        }
    }

    #endregion
}