using GameCore.Core.Entities;
using GameCore.Core.Interfaces;
using GameCore.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace GameCore.Infrastructure.Repositories
{
    /// <summary>
    /// 通知 Repository 實作
    /// </summary>
    public class NotificationRepository : Repository<Notification>, INotificationRepository
    {
        public NotificationRepository(GameCoreDbContext context) : base(context)
        {
        }

        /// <summary>
        /// 獲取未讀通知數量
        /// </summary>
        public async Task<int> GetUnreadCountAsync(int userId)
        {
            return await _context.NotificationRecipients
                .CountAsync(nr => nr.UserId == userId && !nr.IsRead);
        }

        /// <summary>
        /// 獲取使用者通知（分頁）
        /// </summary>
        public async Task<IEnumerable<Notification>> GetUserNotificationsAsync(int userId, int page, int pageSize)
        {
            return await _context.NotificationRecipients
                .Where(nr => nr.UserId == userId)
                .Include(nr => nr.Notification)
                .OrderByDescending(nr => nr.NotificationId)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(nr => nr.Notification)
                .ToListAsync();
        }

        /// <summary>
        /// 標記通知為已讀
        /// </summary>
        public async Task MarkAsReadAsync(int userId, int notificationId)
        {
            var recipient = await _context.NotificationRecipients
                .FirstOrDefaultAsync(nr => nr.UserId == userId && nr.NotificationId == notificationId);

            if (recipient != null)
            {
                recipient.IsRead = true;
                await _context.SaveChangesAsync();
            }
        }

        /// <summary>
        /// 標記所有通知為已讀
        /// </summary>
        public async Task MarkAllAsReadAsync(int userId)
        {
            var unreadNotifications = await _context.NotificationRecipients
                .Where(nr => nr.UserId == userId && !nr.IsRead)
                .ToListAsync();

            foreach (var notification in unreadNotifications)
            {
                notification.IsRead = true;
            }

            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// 創建寵物換色通知
        /// </summary>
        public async Task CreatePetColorChangeNotificationAsync(int userId, string oldColor, string newColor, int cost)
        {
            var notification = new Notification
            {
                SourceId = 1,
                ActionId = 1
            };

            await AddAsync(notification);
            await _context.SaveChangesAsync();

            var recipient = new NotificationRecipient
            {
                NotificationId = notification.NotificationId,
                UserId = userId,
                IsRead = false
            };

            _context.NotificationRecipients.Add(recipient);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// 創建簽到通知
        /// </summary>
        public async Task CreateSignInNotificationAsync(int userId, int points, int experience, int streak)
        {
            var notification = new Notification
            {
                SourceId = 1,
                ActionId = 1
            };

            await AddAsync(notification);
            await _context.SaveChangesAsync();

            var recipient = new NotificationRecipient
            {
                NotificationId = notification.NotificationId,
                UserId = userId,
                IsRead = false
            };

            _context.NotificationRecipients.Add(recipient);
            await _context.SaveChangesAsync();
        }
    }
}