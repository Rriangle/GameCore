using GameCore.Application.Common;
using GameCore.Application.DTOs;

namespace GameCore.Application.Services
{
    /// <summary>
    /// 玩家市場服務介面
    /// </summary>
    public interface IPlayerMarketService
    {
        /// <summary>
        /// 取得活躍商品列表
        /// </summary>
        /// <param name="category">分類</param>
        /// <param name="minPrice">最低價格</param>
        /// <param name="maxPrice">最高價格</param>
        /// <param name="pageNumber">頁碼</param>
        /// <param name="pageSize">每頁大小</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>商品列表</returns>
        Task<Result<IEnumerable<PlayerMarketItemResponse>>> GetActiveItemsAsync(string? category, decimal? minPrice, decimal? maxPrice, int pageNumber, int pageSize, CancellationToken cancellationToken = default);

        /// <summary>
        /// 搜尋商品
        /// </summary>
        /// <param name="searchTerm">搜尋關鍵字</param>
        /// <param name="category">分類</param>
        /// <param name="minPrice">最低價格</param>
        /// <param name="maxPrice">最高價格</param>
        /// <param name="pageNumber">頁碼</param>
        /// <param name="pageSize">每頁大小</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>商品列表</returns>
        Task<Result<IEnumerable<PlayerMarketItemResponse>>> SearchItemsAsync(string searchTerm, string? category, decimal? minPrice, decimal? maxPrice, int pageNumber, int pageSize, CancellationToken cancellationToken = default);

        /// <summary>
        /// 取得商品詳情
        /// </summary>
        /// <param name="itemId">商品 ID</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>商品詳情</returns>
        Task<Result<PlayerMarketItemResponse>> GetItemAsync(int itemId, CancellationToken cancellationToken = default);

        /// <summary>
        /// 取得用戶的商品列表
        /// </summary>
        /// <param name="userId">用戶 ID</param>
        /// <param name="pageNumber">頁碼</param>
        /// <param name="pageSize">每頁大小</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>商品列表</returns>
        Task<Result<IEnumerable<PlayerMarketItemResponse>>> GetItemsByUserAsync(int userId, int pageNumber, int pageSize, CancellationToken cancellationToken = default);

        /// <summary>
        /// 上架商品
        /// </summary>
        /// <param name="request">上架商品請求</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>商品資訊</returns>
        Task<Result<PlayerMarketItemResponse>> ListItemAsync(ListItemRequest request, CancellationToken cancellationToken = default);

        /// <summary>
        /// 下架商品
        /// </summary>
        /// <param name="itemId">商品 ID</param>
        /// <param name="userId">用戶 ID</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>下架結果</returns>
        Task<OperationResult> UnlistItemAsync(int itemId, int userId, CancellationToken cancellationToken = default);

        /// <summary>
        /// 購買商品
        /// </summary>
        /// <param name="request">購買商品請求</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>購買結果</returns>
        Task<Result<MarketTransactionResponse>> PurchaseItemAsync(PurchaseItemRequest request, CancellationToken cancellationToken = default);

        /// <summary>
        /// 取得用戶的交易記錄
        /// </summary>
        /// <param name="userId">用戶 ID</param>
        /// <param name="pageNumber">頁碼</param>
        /// <param name="pageSize">每頁大小</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>交易記錄</returns>
        Task<Result<PagedResult<MarketTransactionResponse>>> GetUserTransactionsAsync(int userId, int pageNumber, int pageSize, CancellationToken cancellationToken = default);
    }

    /// <summary>
    /// 玩家市場商品回應
    /// </summary>
    public class PlayerMarketItemResponse
    {
        /// <summary>
        /// 商品 ID
        /// </summary>
        public int ItemId { get; set; }

        /// <summary>
        /// 賣家 ID
        /// </summary>
        public int SellerId { get; set; }

        /// <summary>
        /// 賣家名稱
        /// </summary>
        public string SellerName { get; set; } = string.Empty;

        /// <summary>
        /// 商品名稱
        /// </summary>
        public string ItemName { get; set; } = string.Empty;

        /// <summary>
        /// 商品描述
        /// </summary>
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// 商品類型
        /// </summary>
        public string ItemType { get; set; } = string.Empty;

        /// <summary>
        /// 分類
        /// </summary>
        public string Category { get; set; } = string.Empty;

        /// <summary>
        /// 價格
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// 數量
        /// </summary>
        public int Quantity { get; set; }

        /// <summary>
        /// 品質
        /// </summary>
        public string Quality { get; set; } = string.Empty;

        /// <summary>
        /// 上架時間
        /// </summary>
        public DateTime ListedAt { get; set; }

        /// <summary>
        /// 狀態
        /// </summary>
        public string Status { get; set; } = string.Empty;
    }

    /// <summary>
    /// 上架商品請求
    /// </summary>
    public class ListItemRequest
    {
        /// <summary>
        /// 賣家 ID
        /// </summary>
        public int SellerId { get; set; }

        /// <summary>
        /// 商品名稱
        /// </summary>
        public string ItemName { get; set; } = string.Empty;

        /// <summary>
        /// 商品描述
        /// </summary>
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// 商品類型
        /// </summary>
        public string ItemType { get; set; } = string.Empty;

        /// <summary>
        /// 分類
        /// </summary>
        public string Category { get; set; } = string.Empty;

        /// <summary>
        /// 價格
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// 數量
        /// </summary>
        public int Quantity { get; set; }

        /// <summary>
        /// 品質
        /// </summary>
        public string Quality { get; set; } = string.Empty;
    }

    /// <summary>
    /// 購買商品請求
    /// </summary>
    public class PurchaseItemRequest
    {
        /// <summary>
        /// 買家 ID
        /// </summary>
        public int BuyerId { get; set; }

        /// <summary>
        /// 商品 ID
        /// </summary>
        public int ItemId { get; set; }

        /// <summary>
        /// 購買數量
        /// </summary>
        public int Quantity { get; set; }
    }

    /// <summary>
    /// 市場交易回應
    /// </summary>
    public class MarketTransactionResponse
    {
        /// <summary>
        /// 交易 ID
        /// </summary>
        public int TransactionId { get; set; }

        /// <summary>
        /// 商品 ID
        /// </summary>
        public int ItemId { get; set; }

        /// <summary>
        /// 商品名稱
        /// </summary>
        public string ItemName { get; set; } = string.Empty;

        /// <summary>
        /// 賣家 ID
        /// </summary>
        public int SellerId { get; set; }

        /// <summary>
        /// 賣家名稱
        /// </summary>
        public string SellerName { get; set; } = string.Empty;

        /// <summary>
        /// 買家 ID
        /// </summary>
        public int BuyerId { get; set; }

        /// <summary>
        /// 買家名稱
        /// </summary>
        public string BuyerName { get; set; } = string.Empty;

        /// <summary>
        /// 數量
        /// </summary>
        public int Quantity { get; set; }

        /// <summary>
        /// 單價
        /// </summary>
        public decimal UnitPrice { get; set; }

        /// <summary>
        /// 總價
        /// </summary>
        public decimal TotalPrice { get; set; }

        /// <summary>
        /// 交易時間
        /// </summary>
        public DateTime TransactionTime { get; set; }

        /// <summary>
        /// 狀態
        /// </summary>
        public string Status { get; set; } = string.Empty;
    }
} 