using GameCore.Domain.Entities;
using GameCore.Domain.Interfaces;
using GameCore.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace GameCore.Infrastructure.Repositories
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        private readonly ILogger<ProductRepository> _logger;

        public ProductRepository(GameCoreDbContext context, ILogger<ProductRepository> logger) : base(context)
        {
            _logger = logger;
        }

        public async Task<IEnumerable<Product>> GetByCategoryAsync(string category)
        {
            return await _dbSet
                .Include(p => p.Category)
                .Where(p => p.Category == category)
                .OrderBy(p => p.Name)
                .ToListAsync();
        }

        public async Task<IEnumerable<Product>> SearchAsync(string keyword)
        {
            return await _dbSet
                .Include(p => p.Category)
                .Where(p => p.Name.Contains(keyword) || p.Description.Contains(keyword))
                .OrderBy(p => p.Name)
                .ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetPopularAsync(int limit)
        {
            return await _dbSet
                .Include(p => p.Category)
                .OrderByDescending(p => p.SalesCount)
                .Take(limit)
                .ToListAsync();
        }
    }
} 
