using GameCore.Core.Entities;

namespace GameCore.Core.Services
{
    /// <summary>
    /// 玩家市場服務介面
    /// </summary>
    public interface IPlayerMarketService
    {
        /// <summary>
        /// 取得所有活躍的市場商品
        /// </summary>
        /// <param name="category">商品分類</param>
        /// <param name="minPrice">最低價格</param>
        /// <param name="maxPrice">最高價格</param>
        /// <param name="page">頁碼</param>
        /// <param name="pageSize">每頁大小</param>
        /// <returns>市場商品列表</returns>
        Task<IEnumerable<PlayerMarketItem>> GetActiveItemsAsync(string? category = null, decimal? minPrice = null, decimal? maxPrice = null, int page = 1, int pageSize = 20);

        /// <summary>
        /// 搜尋市場商品
        /// </summary>
        /// <param name="keyword">關鍵字</param>
        /// <param name="category">商品分類</param>
        /// <param name="minPrice">最低價格</param>
        /// <param name="maxPrice">最高價格</param>
        /// <param name="page">頁碼</param>
        /// <param name="pageSize">每頁大小</param>
        /// <returns>搜尋結果</returns>
        Task<IEnumerable<PlayerMarketItem>> SearchItemsAsync(string keyword, string? category = null, decimal? minPrice = null, decimal? maxPrice = null, int page = 1, int pageSize = 20);

        /// <summary>
        /// 取得商品詳情
        /// </summary>
        /// <param name="itemId">商品ID</param>
        /// <returns>商品詳情</returns>
        Task<PlayerMarketItem?> GetItemAsync(int itemId);

        /// <summary>
        /// 上架商品
        /// </summary>
        /// <param name="userId">使用者ID</param>
        /// <param name="itemCreate">商品建立</param>
        /// <returns>建立結果</returns>
        Task<ItemCreateResult> CreateItemAsync(int userId, ItemCreate itemCreate);

        /// <summary>
        /// 更新商品
        /// </summary>
        /// <param name="itemId">商品ID</param>
        /// <param name="userId">使用者ID</param>
        /// <param name="itemUpdate">商品更新</param>
        /// <returns>更新結果</returns>
        Task<bool> UpdateItemAsync(int itemId, int userId, ItemUpdate itemUpdate);

        /// <summary>
        /// 下架商品
        /// </summary>
        /// <param name="itemId">商品ID</param>
        /// <param name="userId">使用者ID</param>
        /// <returns>操作結果</returns>
        Task<bool> DeactivateItemAsync(int itemId, int userId);

        /// <summary>
        /// 取得使用者的市場商品
        /// </summary>
        /// <param name="userId">使用者ID</param>
        /// <param name="page">頁碼</param>
        /// <param name="pageSize">每頁大小</param>
        /// <returns>商品列表</returns>
        Task<IEnumerable<PlayerMarketItem>> GetItemsByUserAsync(int userId, int page, int pageSize);

        /// <summary>
        /// 取得商品分類
        /// </summary>
        /// <returns>分類列表</returns>
        Task<IEnumerable<string>> GetItemCategoriesAsync();

        /// <summary>
        /// 購買商品
        /// </summary>
        /// <param name="buyerId">買家ID</param>
        /// <param name="itemId">商品ID</param>
        /// <returns>購買結果</returns>
        Task<PurchaseResult> PurchaseItemAsync(int buyerId, int itemId);

        /// <summary>
        /// 取得使用者的交易記錄
        /// </summary>
        /// <param name="userId">使用者ID</param>
        /// <param name="page">頁碼</param>
        /// <param name="pageSize">每頁大小</param>
        /// <returns>交易記錄列表</returns>
        Task<IEnumerable<MarketTransaction>> GetTransactionsByUserAsync(int userId, int page, int pageSize);

        /// <summary>
        /// 確認交易
        /// </summary>
        /// <param name="transactionId">交易ID</param>
        /// <param name="userId">使用者ID</param>
        /// <returns>操作結果</returns>
        Task<bool> ConfirmTransactionAsync(int transactionId, int userId);

        /// <summary>
        /// 取消交易
        /// </summary>
        /// <param name="transactionId">交易ID</param>
        /// <param name="userId">使用者ID</param>
        /// <returns>操作結果</returns>
        Task<bool> CancelTransactionAsync(int transactionId, int userId);

        /// <summary>
        /// 評價交易
        /// </summary>
        /// <param name="transactionId">交易ID</param>
        /// <param name="userId">使用者ID</param>
        /// <param name="review">評價</param>
        /// <returns>操作結果</returns>
        Task<bool> ReviewTransactionAsync(int transactionId, int userId, TransactionReview review);
    }

    /// <summary>
    /// 商品建立模型
    /// </summary>
    public class ItemCreate
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public List<string> Images { get; set; } = new();
        public string? Condition { get; set; }
        public string? Brand { get; set; }
        public string? Model { get; set; }
    }

    /// <summary>
    /// 商品更新模型
    /// </summary>
    public class ItemUpdate
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public List<string> Images { get; set; } = new();
        public string? Condition { get; set; }
        public string? Brand { get; set; }
        public string? Model { get; set; }
    }

    /// <summary>
    /// 交易評價模型
    /// </summary>
    public class TransactionReview
    {
        public int Rating { get; set; }
        public string Comment { get; set; } = string.Empty;
        public bool IsPositive { get; set; }
    }

    /// <summary>
    /// 商品建立結果模型
    /// </summary>
    public class ItemCreateResult
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; } = string.Empty;
        public PlayerMarketItem? Item { get; set; }
        public List<string> Errors { get; set; } = new();
    }

    /// <summary>
    /// 購買結果模型
    /// </summary>
    public class PurchaseResult
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; } = string.Empty;
        public MarketTransaction? Transaction { get; set; }
        public List<string> Errors { get; set; } = new();
    }
}