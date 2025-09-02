using GameCore.Domain.Entities;
using GameCore.Domain.Interfaces;
using GameCore.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace GameCore.Infrastructure.Repositories
{
    public class OrderRepository : Repository<Order>, IOrderRepository
    {
        private readonly ILogger<OrderRepository> _logger;

        public OrderRepository(GameCoreDbContext context, ILogger<OrderRepository> logger) : base(context)
        {
            _logger = logger;
        }

        public async Task<IEnumerable<Order>> GetByUserIdAsync(int userId)
        {
            return await _dbSet
                .Include(o => o.Items)
                .ThenInclude(oi => oi.Product)
                .Where(o => o.UserId == userId)
                .OrderByDescending(o => o.CreatedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<Order>> GetByStatusAsync(string status)
        {
            return await _dbSet
                .Include(o => o.Items)
                .ThenInclude(oi => oi.Product)
                .Where(o => o.Status == status)
                .OrderByDescending(o => o.CreatedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<Order>> GetPendingOrdersAsync()
        {
            return await _dbSet
                .Include(o => o.Items)
                .ThenInclude(oi => oi.Product)
                .Where(o => o.Status == "Pending" || o.Status == "Processing")
                .OrderBy(o => o.CreatedAt)
                .ToListAsync();
        }
    }
} 