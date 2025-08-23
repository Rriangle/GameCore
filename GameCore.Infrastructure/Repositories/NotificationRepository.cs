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

        public async Task CreatePetColorChangeNotificationAsync(int userId, string skinColor, string backgroundColor, int pointsCost)
        {
            var notification = new Notification
            {
                SourceId = 1, // 假設 1 代表寵物系統
                ActionId = 1, // 假設 1 代表換色動作
                SenderId = userId,
                NotificationTitle = "寵物換色成功",
                NotificationMessage = $"您的寵物已成功換色為 {skinColor} 膚色和 {backgroundColor} 背景色，消耗點數 {pointsCost}",
                CreatedAt = DateTime.UtcNow
            };

            await _context.Set<Notification>().AddAsync(notification);
        }

        public async Task<IEnumerable<Notification>> GetByUserIdAsync(int userId)
        {
            return await _context.Notifications
                .Where(n => n.UserId == userId)
                .OrderByDescending(n => n.CreatedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<Notification>> GetUnreadByUserIdAsync(int userId)
        {
            return await _context.Notifications
                .Where(n => n.UserId == userId && !n.IsRead)
                .OrderByDescending(n => n.CreatedAt)
                .ToListAsync();
        }

        public async Task<int> GetUnreadCountAsync(int userId)
        {
            // 這裡需要根據 NotificationRecipient 來計算未讀數量
            // 暫時返回 0，實際實現需要更複雜的查詢
            return 0;
        }

        public async Task MarkAsReadAsync(int notificationId)
        {
            var notification = await _context.Notifications.FindAsync(notificationId);
            if (notification != null)
            {
                notification.IsRead = true;
                await _context.SaveChangesAsync();
            }
        }
    }
}