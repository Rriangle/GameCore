using GameCore.Domain.Entities;

namespace GameCore.Domain.Interfaces
{
    /// <summary>
    /// 商店資料存取介面
    /// </summary>
    public interface IStoreRepository
    {
        /// <summary>
        /// 根據ID取得商品
        /// </summary>
        Task<Product?> GetProductByIdAsync(int id);

        /// <summary>
        /// 取得所有商品
        /// </summary>
        Task<IEnumerable<Product>> GetAllProductsAsync();

        /// <summary>
        /// 根據類別取得商品
        /// </summary>
        Task<IEnumerable<Product>> GetProductsByCategoryAsync(string category);

        /// <summary>
        /// 搜尋商品
        /// </summary>
        Task<IEnumerable<Product>> SearchProductsAsync(string keyword);

        /// <summary>
        /// 新增商品
        /// </summary>
        Task<Product> AddProductAsync(Product product);

        /// <summary>
        /// 更新商品
        /// </summary>
        Task<Product> UpdateProductAsync(Product product);

        /// <summary>
        /// 刪除商品
        /// </summary>
        Task DeleteProductAsync(int id);
    }
} 
