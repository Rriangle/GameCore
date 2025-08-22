using GameCore.Core.Entities;
using GameCore.Core.Interfaces;
using GameCore.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace GameCore.Infrastructure.Repositories
{
    /// <summary>
    /// 聊天 Repository 實作
    /// </summary>
    public class ChatRepository : Repository<ChatMessage>, IChatRepository
    {
        public ChatRepository(GameCoreDbContext context) : base(context)
        {
        }

        /// <summary>
        /// 取得兩個使用者之間的聊天記錄
        /// </summary>
        public async Task<List<ChatMessage>> GetChatHistoryAsync(int userId1, int userId2)
        {
            return await _context.ChatMessages
                .Where(cm => (cm.SenderId == userId1 && cm.ReceiverId == userId2) ||
                            (cm.SenderId == userId2 && cm.ReceiverId == userId1))
                .OrderBy(cm => cm.SentAt)
                .ToListAsync();
        }

        /// <summary>
        /// 取得使用者的最新聊天記錄
        /// </summary>
        public async Task<List<ChatMessage>> GetRecentChatsAsync(int userId)
        {
            return await _context.ChatMessages
                .Where(cm => cm.SenderId == userId || cm.ReceiverId == userId)
                .OrderByDescending(cm => cm.SentAt)
                .Take(20)
                .ToListAsync();
        }

        /// <summary>
        /// 獲取聊天歷史記錄（分頁）
        /// </summary>
        public async Task<IEnumerable<ChatMessage>> GetChatHistoryAsync(int userId1, int userId2, int page, int pageSize)
        {
            return await _context.ChatMessages
                .Where(m => (m.SenderId == userId1 && m.ReceiverId == userId2) ||
                           (m.SenderId == userId2 && m.ReceiverId == userId1))
                .OrderByDescending(m => m.SentAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        /// <summary>
        /// 獲取私人訊息
        /// </summary>
        public async Task<IEnumerable<ChatMessage>> GetPrivateMessagesAsync(int senderId, int receiverId, int page, int pageSize)
        {
            return await _context.ChatMessages
                .Where(m => m.SenderId == senderId && m.ReceiverId == receiverId)
                .OrderByDescending(m => m.SentAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        /// <summary>
        /// 獲取未讀訊息數量
        /// </summary>
        public async Task<int> GetUnreadCountAsync(int userId)
        {
            return await _context.ChatMessages
                .CountAsync(m => m.ReceiverId == userId && !m.IsRead);
        }

        /// <summary>
        /// 標記訊息為已讀
        /// </summary>
        public async Task MarkMessagesAsReadAsync(int senderId, int receiverId)
        {
            var messages = await _context.ChatMessages
                .Where(m => m.SenderId == senderId && m.ReceiverId == receiverId && !m.IsRead)
                .ToListAsync();

            foreach (var message in messages)
            {
                message.IsRead = true;
            }

            await _context.SaveChangesAsync();
        }
    }
}