using GameCore.Core.Entities;
using GameCore.Core.Enums;

namespace GameCore.Core.Interfaces
{
    /// <summary>
    /// 玩家市場倉庫介面
    /// </summary>
    public interface IPlayerMarketRepository : IRepository<PlayerMarketItem>
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
        /// 更新商品狀態
        /// </summary>
        /// <param name="itemId">商品ID</param>
        /// <param name="status">新狀態</param>
        /// <returns>操作結果</returns>
        Task<bool> UpdateItemStatusAsync(int itemId, MarketItemStatus status);
    }

    /// <summary>
    /// 市場交易倉庫介面
    /// </summary>
    public interface IMarketTransactionRepository : IRepository<MarketTransaction>
    {
        /// <summary>
        /// 取得使用者的交易記錄
        /// </summary>
        /// <param name="userId">使用者ID</param>
        /// <param name="page">頁碼</param>
        /// <param name="pageSize">每頁大小</param>
        /// <returns>交易記錄列表</returns>
        Task<IEnumerable<MarketTransaction>> GetTransactionsByUserAsync(int userId, int page, int pageSize);

        /// <summary>
        /// 建立交易記錄
        /// </summary>
        /// <param name="transaction">交易記錄</param>
        /// <returns>操作結果</returns>
        Task<bool> CreateTransactionAsync(MarketTransaction transaction);

        /// <summary>
        /// 更新交易狀態
        /// </summary>
        /// <param name="transactionId">交易ID</param>
        /// <param name="status">新狀態</param>
        /// <returns>操作結果</returns>
        Task<bool> UpdateTransactionStatusAsync(int transactionId, TransactionStatus status);

        /// <summary>
        /// 取得待處理的交易
        /// </summary>
        /// <param name="page">頁碼</param>
        /// <param name="pageSize">每頁大小</param>
        /// <returns>交易列表</returns>
        Task<IEnumerable<MarketTransaction>> GetPendingTransactionsAsync(int page = 1, int pageSize = 20);
    }

    /// <summary>
    /// 市場評價倉庫介面
    /// </summary>
    public interface IMarketReviewRepository : IRepository<MarketReview>
    {
        /// <summary>
        /// 取得商品的評價
        /// </summary>
        /// <param name="itemId">商品ID</param>
        /// <param name="page">頁碼</param>
        /// <param name="pageSize">每頁大小</param>
        /// <returns>評價列表</returns>
        Task<IEnumerable<MarketReview>> GetReviewsByItemAsync(int itemId, int page, int pageSize);

        /// <summary>
        /// 取得使用者的評價
        /// </summary>
        /// <param name="userId">使用者ID</param>
        /// <param name="page">頁碼</param>
        /// <param name="pageSize">每頁大小</param>
        /// <returns>評價列表</returns>
        Task<IEnumerable<MarketReview>> GetReviewsByUserAsync(int userId, int page, int pageSize);

        /// <summary>
        /// 檢查使用者是否已評價該商品
        /// </summary>
        /// <param name="userId">使用者ID</param>
        /// <param name="itemId">商品ID</param>
        /// <returns>是否已評價</returns>
        Task<bool> HasUserReviewedAsync(int userId, int itemId);
    }
}