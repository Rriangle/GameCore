using GameCore.Domain.Entities;
using GameCore.Domain.Interfaces;
using GameCore.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace GameCore.Infrastructure.Repositories
{
    public class NotificationRepository : Repository<Notification>, INotificationRepository
    {
        private readonly ILogger<NotificationRepository> _logger;

        public NotificationRepository(GameCoreDbContext context, ILogger<NotificationRepository> logger) : base(context)
        {
            _logger = logger;
        }

        // 實現 INotificationRepository 接口的缺少方法
        public async Task<Notification?> GetByIdAsync(int id)
        {
            return await _context.Notifications
                .Include(n => n.NotificationSource)
                .Include(n => n.NotificationAction)
                .FirstOrDefaultAsync(n => n.Id == id);
        }

        public async Task<IEnumerable<Notification>> GetByUserIdAsync(int userId)
        {
            return await _context.Notifications
                .Include(n => n.NotificationSource)
                .Include(n => n.NotificationAction)
                .Where(n => n.UserId == userId)
                .OrderByDescending(n => n.CreateTime)
                .ToListAsync();
        }

        public async Task<Notification> AddAsync(Notification notification)
        {
            _context.Notifications.Add(notification);
            await _context.SaveChangesAsync();
            return notification;
        }

        public async Task<Notification> UpdateAsync(Notification notification)
        {
            _context.Notifications.Update(notification);
            await _context.SaveChangesAsync();
            return notification;
        }

        public async Task DeleteAsync(int id)
        {
            var notification = await _context.Notifications.FindAsync(id);
            if (notification != null)
            {
                _context.Notifications.Remove(notification);
                await _context.SaveChangesAsync();
            }
        }

        public async Task CreatePetColorChangeNotificationAsync(int userId, string oldColor, string newColor)
        {
            var notification = new Notification
            {
                UserId = userId,
                Type = "PetColorChange",
                Title = "寵物顏色變更通知",
                Content = $"您的寵物顏色已從 {oldColor} 變更為 {newColor}",
                IsRead = false,
                CreateTime = DateTime.UtcNow
            };

            await AddAsync(notification);
        }

        public async Task<Notification?> GetByUserAndActionAsync(int userId, string action)
        {
            return await _context.Notifications
                .Include(n => n.NotificationSource)
                .Include(n => n.NotificationAction)
                .FirstOrDefaultAsync(n => n.UserId == userId && n.Type == action);
        }

        public async Task<IEnumerable<Notification>> GetUserNotificationsAsync(int userId, int page = 1, int pageSize = 20)
        {
            return await _context.Notifications
                .Include(n => n.NotificationSource)
                .Include(n => n.NotificationAction)
                .Where(n => n.UserId == userId)
                .OrderByDescending(n => n.CreateTime)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<IEnumerable<Notification>> GetUnreadNotificationsAsync(int userId)
        {
            return await _context.Notifications
                .Include(n => n.NotificationSource)
                .Include(n => n.NotificationAction)
                .Where(n => n.UserId == userId && !n.IsRead)
                .OrderByDescending(n => n.CreateTime)
                .ToListAsync();
        }

        public async Task<int> GetUnreadCountAsync(int userId)
        {
            return await _context.Notifications
                .CountAsync(n => n.UserId == userId && !n.IsRead);
        }

        public async Task<IEnumerable<Notification>> GetNotificationsByTypeAsync(int userId, string type, int page = 1, int pageSize = 20)
        {
            return await _context.Notifications
                .Include(n => n.NotificationSource)
                .Include(n => n.NotificationAction)
                .Where(n => n.UserId == userId && n.Type == type)
                .OrderByDescending(n => n.CreateTime)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task MarkAsReadAsync(int notificationId)
        {
            var notification = await _context.Notifications.FindAsync(notificationId);
            if (notification != null && !notification.IsRead)
            {
                notification.IsRead = true;
                notification.ReadTime = DateTime.UtcNow;
                await _context.SaveChangesAsync();
            }
        }

        public async Task MarkAllAsReadAsync(int userId)
        {
            var unreadNotifications = await _context.Notifications
                .Where(n => n.UserId == userId && !n.IsRead)
                .ToListAsync();

            foreach (var notification in unreadNotifications)
            {
                notification.IsRead = true;
                notification.ReadTime = DateTime.UtcNow;
            }

            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Notification>> GetNotificationsBySourceAsync(int userId, int sourceId, string sourceType)
        {
            return await _context.Notifications
                .Include(n => n.NotificationSource)
                .Include(n => n.NotificationAction)
                .Where(n => n.UserId == userId && 
                           n.NotificationSource.SourceId == sourceId.ToString() && 
                           n.NotificationSource.SourceType == sourceType)
                .OrderByDescending(n => n.CreateTime)
                .ToListAsync();
        }

        public async Task DeleteNotificationAsync(int notificationId)
        {
            var notification = await _context.Notifications.FindAsync(notificationId);
            if (notification != null)
            {
                _context.Notifications.Remove(notification);
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteOldNotificationsAsync(int userId, DateTime olderThan)
        {
            var oldNotifications = await _context.Notifications
                .Where(n => n.UserId == userId && n.CreateTime < olderThan)
                .ToListAsync();

            _context.Notifications.RemoveRange(oldNotifications);
            await _context.SaveChangesAsync();
        }

        public async Task<NotificationSource?> GetNotificationSourceAsync(int sourceId, string sourceType)
        {
            return await _context.NotificationSources
                .FirstOrDefaultAsync(ns => ns.SourceId == sourceId && ns.SourceType == sourceType);
        }

        public async Task<NotificationAction?> GetNotificationActionAsync(int actionId, string actionType)
        {
            return await _context.NotificationActions
                .FirstOrDefaultAsync(na => na.ActionId == actionId && na.ActionType == actionType);
        }

        public async Task<IEnumerable<Notification>> GetSystemNotificationsAsync(int page = 1, int pageSize = 20)
        {
            return await _context.Notifications
                .Include(n => n.NotificationSource)
                .Include(n => n.NotificationAction)
                .Where(n => n.Type == "System")
                .OrderByDescending(n => n.CreateTime)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<Dictionary<string, int>> GetNotificationCountsByTypeAsync(int userId)
        {
            return await _context.Notifications
                .Where(n => n.UserId == userId && !n.IsRead)
                .GroupBy(n => n.Type)
                .ToDictionaryAsync(g => g.Key, g => g.Count());
        }

        public async Task<IEnumerable<Notification>> GetRecentNotificationsAsync(int userId, TimeSpan timeSpan)
        {
            var cutoffTime = DateTime.UtcNow.Subtract(timeSpan);
            return await _context.Notifications
                .Include(n => n.NotificationSource)
                .Include(n => n.NotificationAction)
                .Where(n => n.UserId == userId && n.CreateTime >= cutoffTime)
                .OrderByDescending(n => n.CreateTime)
                .ToListAsync();
        }
    }
}