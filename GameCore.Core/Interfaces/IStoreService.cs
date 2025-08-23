using GameCore.Core.Entities;

namespace GameCore.Core.Interfaces
{
    /// <summary>
    /// 商城服務介面
    /// </summary>
    public interface IStoreService
    {
        Task<ProductInfo> CreateProductAsync(ProductInfo product);
        Task<ProductInfo?> GetProductAsync(int productId);
        Task<IEnumerable<ProductInfo>> GetProductsByCategoryAsync(string category);
        Task<bool> UpdateProductAsync(ProductInfo product);
        Task<bool> DeleteProductAsync(int productId);
        Task<IEnumerable<ProductInfo>> SearchProductsAsync(string searchTerm);
    }
}