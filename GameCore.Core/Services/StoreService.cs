using GameCore.Core.Entities;
using GameCore.Core.Interfaces;

namespace GameCore.Core.Services
{
    public class StoreService : IStoreService
    {
        private readonly IUnitOfWork _unitOfWork;

        public StoreService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ProductInfo> CreateProductAsync(ProductInfo product)
        {
            product.ProductCreatedAt = DateTime.UtcNow;
            product.ProductUpdatedAt = DateTime.UtcNow;

            await _unitOfWork.StoreRepository.AddAsync(product);
            await _unitOfWork.SaveChangesAsync();

            return product;
        }

        public async Task<ProductInfo?> GetProductAsync(int productId)
        {
            return await _unitOfWork.StoreRepository.GetByIdAsync(productId);
        }

        public async Task<IEnumerable<ProductInfo>> GetProductsByCategoryAsync(string category)
        {
            var products = await _unitOfWork.StoreRepository.GetAllAsync();
            return products.Where(p => p.ProductType == category);
        }

        public async Task<bool> UpdateProductAsync(ProductInfo product)
        {
            var existingProduct = await _unitOfWork.StoreRepository.GetByIdAsync(product.ProductId);
            if (existingProduct == null) return false;

            existingProduct.ProductName = product.ProductName;
            existingProduct.ProductType = product.ProductType;
            existingProduct.Price = product.Price;
            existingProduct.ProductUpdatedAt = DateTime.UtcNow;

            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteProductAsync(int productId)
        {
            var product = await _unitOfWork.StoreRepository.GetByIdAsync(productId);
            if (product == null) return false;

            await _unitOfWork.StoreRepository.DeleteByIdAsync(productId);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<ProductInfo>> SearchProductsAsync(string searchTerm)
        {
            var products = await _unitOfWork.StoreRepository.GetAllAsync();
            return products.Where(p => 
                (p.ProductName?.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ?? false));
        }
    }
}