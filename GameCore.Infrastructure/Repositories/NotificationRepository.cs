using GameCore.Core.Entities;
using GameCore.Core.Interfaces;
using GameCore.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace GameCore.Infrastructure.Repositories
{
    public class NotificationRepository : Repository<Notification>, INotificationRepository
    {
        public NotificationRepository(GameCoreDbContext context) : base(context)
        {
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
                           n.NotificationSource.SourceId == sourceId && 
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