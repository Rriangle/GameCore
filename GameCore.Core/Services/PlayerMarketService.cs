using GameCore.Core.Entities;
using GameCore.Core.Interfaces;

namespace GameCore.Core.Services
{
    public class PlayerMarketService : IPlayerMarketService
    {
        private readonly IUnitOfWork _unitOfWork;

        public PlayerMarketService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<PlayerMarketProductInfo> CreateProductAsync(PlayerMarketProductInfo product)
        {
            product.CreatedAt = DateTime.UtcNow;
            product.UpdatedAt = DateTime.UtcNow;

            await _unitOfWork.PlayerMarketRepository.AddAsync(product);
            await _unitOfWork.SaveChangesAsync();

            return product;
        }

        public async Task<PlayerMarketProductInfo?> GetProductAsync(int productId)
        {
            return await _unitOfWork.PlayerMarketRepository.GetByIdAsync(productId);
        }

        public async Task<IEnumerable<PlayerMarketProductInfo>> GetUserProductsAsync(int userId)
        {
            var products = await _unitOfWork.PlayerMarketRepository.GetAllAsync();
            return products.Where(p => p.SellerId == userId);
        }

        public async Task<bool> UpdateProductAsync(PlayerMarketProductInfo product)
        {
            var existingProduct = await _unitOfWork.PlayerMarketRepository.GetByIdAsync(product.PProductId);
            if (existingProduct == null) return false;

            existingProduct.PProductName = product.PProductName;
            existingProduct.PProductDescription = product.PProductDescription;
            existingProduct.Price = product.Price;
            existingProduct.UpdatedAt = DateTime.UtcNow;

            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteProductAsync(int productId)
        {
            var product = await _unitOfWork.PlayerMarketRepository.GetByIdAsync(productId);
            if (product == null) return false;

            await _unitOfWork.PlayerMarketRepository.DeleteByIdAsync(productId);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<PlayerMarketProductInfo>> SearchProductsAsync(string searchTerm)
        {
            var products = await _unitOfWork.PlayerMarketRepository.GetAllAsync();
            return products.Where(p => 
                (p.PProductName?.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ?? false) ||
                (p.PProductDescription?.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ?? false));
        }
    }
}