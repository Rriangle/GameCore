using GameCore.Domain.Entities;
using GameCore.Domain.Interfaces;
using GameCore.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace GameCore.Infrastructure.Repositories
{
    public class NotificationActionRepository : Repository<NotificationAction>, INotificationActionRepository
    {
        private readonly ILogger<NotificationActionRepository> _logger;

        public NotificationActionRepository(GameCoreDbContext context, ILogger<NotificationActionRepository> logger) : base(context)
        {
            _logger = logger;
        }

        public async Task<IEnumerable<NotificationAction>> GetAllActionsAsync()
        {
            return await _dbSet
                .OrderBy(a => a.Name)
                .ToListAsync();
        }

        public async Task<NotificationAction?> GetActionByNameAsync(string name)
        {
            return await _dbSet
                .FirstOrDefaultAsync(a => a.Name == name);
        }
    }
} 