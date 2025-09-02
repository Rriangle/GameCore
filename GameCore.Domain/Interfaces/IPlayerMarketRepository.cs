using GameCore.Domain.Entities;

namespace GameCore.Domain.Interfaces
{
    /// <summary>
    /// 玩家市場資料存取介面
    /// </summary>
    public interface IPlayerMarketRepository
    {
        /// <summary>
        /// 取得市場商品列表
        /// </summary>
        Task<IEnumerable<PlayerMarketProductInfo>> GetMarketItemsAsync();

        /// <summary>
        /// 根據ID取得市場商品
        /// </summary>
        Task<PlayerMarketProductInfo?> GetMarketItemByIdAsync(int id);

        /// <summary>
        /// 搜尋市場商品
        /// </summary>
        Task<IEnumerable<PlayerMarketProductInfo>> SearchMarketItemsAsync(string keyword);

        /// <summary>
        /// 根據類別取得市場商品
        /// </summary>
        Task<IEnumerable<PlayerMarketProductInfo>> GetMarketItemsByCategoryAsync(string category);

        /// <summary>
        /// 根據賣家ID取得商品
        /// </summary>
        Task<IEnumerable<PlayerMarketProductInfo>> GetBySellerIdAsync(int sellerId);

        /// <summary>
        /// 取得商品類別
        /// </summary>
        Task<IEnumerable<string>> GetCategoriesAsync();

        /// <summary>
        /// 新增市場商品
        /// </summary>
        Task<PlayerMarketProductInfo> AddAsync(PlayerMarketProductInfo item);

        /// <summary>
        /// 更新市場商品
        /// </summary>
        Task<PlayerMarketProductInfo> UpdateAsync(PlayerMarketProductInfo item);

        /// <summary>
        /// 刪除市場商品
        /// </summary>
        Task DeleteAsync(int id);

        /// <summary>
        /// 確認交易
        /// </summary>
        Task<bool> ConfirmTransactionAsync(int transactionId);

        /// <summary>
        /// 取消交易
        /// </summary>
        Task<bool> CancelTransactionAsync(int transactionId);

        /// <summary>
        /// 評價交易
        /// </summary>
        Task<bool> ReviewTransactionAsync(int transactionId, int rating, string comment);

        /// <summary>
        /// 取得市場商品列表（帶分頁）
        /// </summary>
        Task<IEnumerable<PlayerMarketProductInfo>> GetMarketItemsAsync(int page, int pageSize);

        /// <summary>
        /// 搜尋市場商品（帶分頁）
        /// </summary>
        Task<IEnumerable<PlayerMarketProductInfo>> SearchMarketItemsAsync(string keyword, int page, int pageSize);

        /// <summary>
        /// 根據類別取得市場商品（帶分頁）
        /// </summary>
        Task<IEnumerable<PlayerMarketProductInfo>> GetMarketItemsByCategoryAsync(string category, int page, int pageSize);

        /// <summary>
        /// 新增市場商品
        /// </summary>
        Task<PlayerMarketProductInfo> Add(PlayerMarketProductInfo item);

        /// <summary>
        /// 更新市場商品
        /// </summary>
        Task<PlayerMarketProductInfo> Update(PlayerMarketProductInfo item);

        /// <summary>
        /// 取得活躍商品
        /// </summary>
        Task<IEnumerable<PlayerMarketProductInfo>> GetActiveItemsAsync();

        /// <summary>
        /// 搜尋商品
        /// </summary>
        Task<IEnumerable<PlayerMarketProductInfo>> SearchItemsAsync(string keyword);

        /// <summary>
        /// 根據ID取得商品
        /// </summary>
        Task<PlayerMarketProductInfo?> GetByIdAsync(int id);

        /// <summary>
        /// 根據賣家ID取得商品（帶分頁）
        /// </summary>
        Task<IEnumerable<PlayerMarketProductInfo>> GetBySellerIdAsync(int sellerId, int page, int pageSize);

        /// <summary>
        /// 確認交易
        /// </summary>
        Task<bool> ConfirmTransactionAsync(int transactionId, int itemId);

        /// <summary>
        /// 取消交易
        /// </summary>
        Task<bool> CancelTransactionAsync(int transactionId, int itemId);
    }
}
