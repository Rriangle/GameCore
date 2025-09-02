using GameCore.Domain.Entities;
using GameCore.Domain.Interfaces;
using GameCore.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace GameCore.Infrastructure.Repositories
{
    public class NotificationSourceRepository : Repository<NotificationSource>, INotificationSourceRepository
    {
        private readonly ILogger<NotificationSourceRepository> _logger;

        public NotificationSourceRepository(GameCoreDbContext context, ILogger<NotificationSourceRepository> logger) : base(context)
        {
            _logger = logger;
        }

        public async Task<IEnumerable<NotificationSource>> GetAllSourcesAsync()
        {
            return await _dbSet
                .OrderBy(s => s.Name)
                .ToListAsync();
        }

        public async Task<NotificationSource?> GetSourceByNameAsync(string name)
        {
            return await _dbSet
                .FirstOrDefaultAsync(s => s.Name == name);
        }
    }
} 