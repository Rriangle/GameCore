using GameCore.Core.Entities;

namespace GameCore.Core.Interfaces
{
    /// <summary>
    /// 通用 Repository 接口
    /// </summary>
    /// <typeparam name="T">實體類型</typeparam>
    public interface IRepository<T> where T : class
    {
        /// <summary>
        /// 獲取所有實體
        /// </summary>
        /// <returns>實體列表</returns>
        Task<IEnumerable<T>> GetAllAsync();

        /// <summary>
        /// 根據 ID 獲取實體
        /// </summary>
        /// <param name="id">實體 ID</param>
        /// <returns>實體</returns>
        Task<T?> GetByIdAsync(int id);

        /// <summary>
        /// 添加實體
        /// </summary>
        /// <param name="entity">實體</param>
        /// <returns>添加的實體</returns>
        Task<T> AddAsync(T entity);

        /// <summary>
        /// 更新實體
        /// </summary>
        /// <param name="entity">實體</param>
        /// <returns>更新結果</returns>
        Task<bool> UpdateAsync(T entity);

        /// <summary>
        /// 刪除實體
        /// </summary>
        /// <param name="id">實體 ID</param>
        /// <returns>刪除結果</returns>
        Task<bool> DeleteAsync(int id);

        /// <summary>
        /// 檢查實體是否存在
        /// </summary>
        /// <param name="id">實體 ID</param>
        /// <returns>是否存在</returns>
        Task<bool> ExistsAsync(int id);
    }

    /// <summary>
    /// 用戶 Repository 接口
    /// </summary>
    public interface IUserRepository : IRepository<User>
    {
        /// <summary>
        /// 根據用戶名獲取用戶
        /// </summary>
        /// <param name="username">用戶名</param>
        /// <returns>用戶</returns>
        Task<User?> GetByUsernameAsync(string username);

        /// <summary>
        /// 根據郵箱獲取用戶
        /// </summary>
        /// <param name="email">郵箱</param>
        /// <returns>用戶</returns>
        Task<User?> GetByEmailAsync(string email);

        /// <summary>
        /// 檢查用戶名是否存在
        /// </summary>
        /// <param name="username">用戶名</param>
        /// <returns>是否存在</returns>
        Task<bool> UsernameExistsAsync(string username);

        /// <summary>
        /// 檢查郵箱是否存在
        /// </summary>
        /// <param name="email">郵箱</param>
        /// <returns>是否存在</returns>
        Task<bool> EmailExistsAsync(string email);

        /// <summary>
        /// 獲取用戶統計信息
        /// </summary>
        /// <param name="userId">用戶 ID</param>
        /// <returns>統計信息</returns>
        Task<UserStatistics> GetUserStatisticsAsync(int userId);
    }

    /// <summary>
    /// 產品 Repository 接口
    /// </summary>
    public interface IProductRepository : IRepository<Product>
    {
        /// <summary>
        /// 根據分類獲取產品
        /// </summary>
        /// <param name="categoryId">分類 ID</param>
        /// <returns>產品列表</returns>
        Task<IEnumerable<Product>> GetByCategoryAsync(int categoryId);

        /// <summary>
        /// 搜索產品
        /// </summary>
        /// <param name="keyword">關鍵字</param>
        /// <returns>產品列表</returns>
        Task<IEnumerable<Product>> SearchAsync(string keyword);

        /// <summary>
        /// 獲取熱門產品
        /// </summary>
        /// <param name="limit">數量限制</param>
        /// <returns>產品列表</returns>
        Task<IEnumerable<Product>> GetPopularAsync(int limit);

        /// <summary>
        /// 更新庫存
        /// </summary>
        /// <param name="productId">產品 ID</param>
        /// <param name="quantity">數量變化</param>
        /// <returns>更新結果</returns>
        Task<bool> UpdateStockAsync(int productId, int quantity);
    }

    /// <summary>
    /// 訂單 Repository 接口
    /// </summary>
    public interface IOrderRepository : IRepository<Order>
    {
        /// <summary>
        /// 根據用戶獲取訂單
        /// </summary>
        /// <param name="userId">用戶 ID</param>
        /// <returns>訂單列表</returns>
        Task<IEnumerable<Order>> GetByUserAsync(int userId);

        /// <summary>
        /// 根據狀態獲取訂單
        /// </summary>
        /// <param name="status">訂單狀態</param>
        /// <returns>訂單列表</returns>
        Task<IEnumerable<Order>> GetByStatusAsync(string status);

        /// <summary>
        /// 更新訂單狀態
        /// </summary>
        /// <param name="orderId">訂單 ID</param>
        /// <param name="status">新狀態</param>
        /// <returns>更新結果</returns>
        Task<bool> UpdateStatusAsync(int orderId, string status);
    }

    /// <summary>
    /// 寵物 Repository 接口
    /// </summary>
    public interface IPetRepository : IRepository<Pet>
    {
        /// <summary>
        /// 根據用戶獲取寵物
        /// </summary>
        /// <param name="userId">用戶 ID</param>
        /// <returns>寵物列表</returns>
        Task<IEnumerable<Pet>> GetByUserAsync(int userId);

        /// <summary>
        /// 根據類型獲取寵物
        /// </summary>
        /// <param name="petType">寵物類型</param>
        /// <returns>寵物列表</returns>
        Task<IEnumerable<Pet>> GetByTypeAsync(string petType);

        /// <summary>
        /// 更新寵物健康狀態
        /// </summary>
        /// <param name="petId">寵物 ID</param>
        /// <param name="health">健康值</param>
        /// <returns>更新結果</returns>
        Task<bool> UpdateHealthAsync(int petId, int health);
    }

    /// <summary>
    /// 論壇 Repository 接口
    /// </summary>
    public interface IForumRepository : IRepository<Forum>
    {
        /// <summary>
        /// 獲取活躍論壇
        /// </summary>
        /// <param name="limit">數量限制</param>
        /// <returns>論壇列表</returns>
        Task<IEnumerable<Forum>> GetActiveForumsAsync(int limit);

        /// <summary>
        /// 根據分類獲取論壇
        /// </summary>
        /// <param name="category">分類</param>
        /// <returns>論壇列表</returns>
        Task<IEnumerable<Forum>> GetByCategoryAsync(string category);
    }

    /// <summary>
    /// 貼文 Repository 接口
    /// </summary>
    public interface IPostRepository : IRepository<Post>
    {
        /// <summary>
        /// 根據論壇獲取貼文
        /// </summary>
        /// <param name="forumId">論壇 ID</param>
        /// <returns>貼文列表</returns>
        Task<IEnumerable<Post>> GetByForumAsync(int forumId);

        /// <summary>
        /// 根據作者獲取貼文
        /// </summary>
        /// <param name="authorId">作者 ID</param>
        /// <returns>貼文列表</returns>
        Task<IEnumerable<Post>> GetByAuthorAsync(int authorId);

        /// <summary>
        /// 搜索貼文
        /// </summary>
        /// <param name="keyword">關鍵字</param>
        /// <returns>貼文列表</returns>
        Task<IEnumerable<Post>> SearchAsync(string keyword);

        /// <summary>
        /// 獲取熱門貼文
        /// </summary>
        /// <param name="limit">數量限制</param>
        /// <returns>貼文列表</returns>
        Task<IEnumerable<Post>> GetPopularAsync(int limit);
    }

    /// <summary>
    /// 商店 Repository 接口
    /// </summary>
    public interface IStoreRepository : IRepository<Product>
    {
        /// <summary>
        /// 根據類別獲取活躍商品
        /// </summary>
        /// <param name="category">類別</param>
        /// <param name="page">頁數</param>
        /// <param name="pageSize">每頁數量</param>
        /// <returns>商品列表</returns>
        Task<IEnumerable<Product>> GetActiveProductsAsync(string? category = null, int page = 1, int pageSize = 20);

        /// <summary>
        /// 搜索商品
        /// </summary>
        /// <param name="keyword">關鍵字</param>
        /// <param name="category">類別</param>
        /// <param name="minPrice">最低價格</param>
        /// <param name="maxPrice">最高價格</param>
        /// <param name="page">頁數</param>
        /// <param name="pageSize">每頁數量</param>
        /// <returns>商品列表</returns>
        Task<IEnumerable<Product>> SearchProductsAsync(string keyword, string? category = null, decimal? minPrice = null, decimal? maxPrice = null, int page = 1, int pageSize = 20);

        /// <summary>
        /// 獲取熱門商品
        /// </summary>
        /// <param name="count">數量</param>
        /// <returns>商品列表</returns>
        Task<IEnumerable<Product>> GetPopularProductsAsync(int count = 10);

        /// <summary>
        /// 獲取銷售排行
        /// </summary>
        /// <param name="category">類別</param>
        /// <param name="count">數量</param>
        /// <returns>商品列表</returns>
        Task<IEnumerable<Product>> GetSalesRankingAsync(string category, int count = 10);

        /// <summary>
        /// 更新商品庫存
        /// </summary>
        /// <param name="productId">商品ID</param>
        /// <param name="quantity">數量變化</param>
        /// <returns>更新結果</returns>
        Task<bool> UpdateStockAsync(int productId, int quantity);

        /// <summary>
        /// 檢查庫存是否足夠
        /// </summary>
        /// <param name="productId">商品ID</param>
        /// <param name="requiredQuantity">需要數量</param>
        /// <returns>是否足夠</returns>
        Task<bool> IsStockAvailableAsync(int productId, int requiredQuantity);
    }

    /// <summary>
    /// 購物車 Repository 接口
    /// </summary>
    public interface ICartRepository : IRepository<Cart>
    {
        /// <summary>
        /// 根據用戶獲取購物車
        /// </summary>
        /// <param name="userId">用戶ID</param>
        /// <returns>購物車</returns>
        Task<Cart?> GetByUserIdAsync(int userId);

        /// <summary>
        /// 添加商品到購物車
        /// </summary>
        /// <param name="userId">用戶ID</param>
        /// <param name="productId">商品ID</param>
        /// <param name="quantity">數量</param>
        /// <returns>添加結果</returns>
        Task<bool> AddItemAsync(int userId, int productId, int quantity);

        /// <summary>
        /// 更新購物車商品數量
        /// </summary>
        /// <param name="userId">用戶ID</param>
        /// <param name="productId">商品ID</param>
        /// <param name="quantity">新數量</param>
        /// <returns>更新結果</returns>
        Task<bool> UpdateItemQuantityAsync(int userId, int productId, int quantity);

        /// <summary>
        /// 從購物車移除商品
        /// </summary>
        /// <param name="userId">用戶ID</param>
        /// <param name="productId">商品ID</param>
        /// <returns>移除結果</returns>
        Task<bool> RemoveItemAsync(int userId, int productId);

        /// <summary>
        /// 清空購物車
        /// </summary>
        /// <param name="userId">用戶ID</param>
        /// <returns>清空結果</returns>
        Task<bool> ClearCartAsync(int userId);

        /// <summary>
        /// 獲取購物車商品數量
        /// </summary>
        /// <param name="userId">用戶ID</param>
        /// <returns>商品數量</returns>
        Task<int> GetItemCountAsync(int userId);

        /// <summary>
        /// 獲取購物車總金額
        /// </summary>
        /// <param name="userId">用戶ID</param>
        /// <returns>總金額</returns>
        Task<decimal> GetTotalAmountAsync(int userId);
    }

    /// <summary>
    /// 用戶統計信息
    /// </summary>
    public class UserStatistics
    {
        /// <summary>
        /// 用戶 ID
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// 訂單總數
        /// </summary>
        public int TotalOrders { get; set; }

        /// <summary>
        /// 總消費金額
        /// </summary>
        public decimal TotalSpent { get; set; }

        /// <summary>
        /// 寵物數量
        /// </summary>
        public int PetCount { get; set; }

        /// <summary>
        /// 貼文數量
        /// </summary>
        public int PostCount { get; set; }

        /// <summary>
        /// 註冊天數
        /// </summary>
        public int DaysRegistered { get; set; }

        /// <summary>
        /// 最後登入時間
        /// </summary>
        public DateTime? LastLoginAt { get; set; }
    }
} 