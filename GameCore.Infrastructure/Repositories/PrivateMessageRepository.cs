using GameCore.Domain.Entities;
using GameCore.Domain.Interfaces;
using GameCore.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace GameCore.Infrastructure.Repositories
{
    public class PrivateMessageRepository : IPrivateMessageRepository
    {
        private readonly GameCoreDbContext _context;
        private readonly ILogger<PrivateMessageRepository> _logger;

        public PrivateMessageRepository(GameCoreDbContext context, ILogger<PrivateMessageRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<PrivateMessage?> GetByIdAsync(int id)
        {
            return await _context.PrivateMessages
                .Include(pm => pm.Sender)
                .Include(pm => pm.Receiver)
                .FirstOrDefaultAsync(pm => pm.Id == id);
        }

        public async Task<IEnumerable<PrivateMessage>> GetMessagesBetweenUsersAsync(int user1Id, int user2Id)
        {
            return await _context.PrivateMessages
                .Include(pm => pm.Sender)
                .Include(pm => pm.Receiver)
                .Where(pm => (pm.SenderId == user1Id && pm.ReceiverId == user2Id) ||
                           (pm.SenderId == user2Id && pm.ReceiverId == user1Id))
                .OrderBy(pm => pm.CreatedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<PrivateMessage>> GetUserMessagesAsync(int userId, int skip = 0, int take = 20)
        {
            return await _context.PrivateMessages
                .Include(pm => pm.Sender)
                .Include(pm => pm.Receiver)
                .Where(pm => pm.SenderId == userId || pm.ReceiverId == userId)
                .OrderByDescending(pm => pm.CreatedAt)
                .Skip(skip)
                .Take(take)
                .ToListAsync();
        }

        public async Task<PrivateMessage> AddAsync(PrivateMessage message)
        {
            _context.PrivateMessages.Add(message);
            await _context.SaveChangesAsync();
            return message;
        }

        public async Task<bool> UpdateAsync(PrivateMessage message)
        {
            _context.PrivateMessages.Update(message);
            var result = await _context.SaveChangesAsync();
            return result > 0;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var message = await _context.PrivateMessages.FindAsync(id);
            if (message == null) return false;

            _context.PrivateMessages.Remove(message);
            var result = await _context.SaveChangesAsync();
            return result > 0;
        }

        public async Task<IEnumerable<PrivateMessage>> GetByPrivateChatIdAsync(int privateChatId, int page, int pageSize)
        {
            var skip = (page - 1) * pageSize;
            return await _context.PrivateMessages
                .Include(pm => pm.Sender)
                .Include(pm => pm.Receiver)
                .Where(pm => pm.PrivateChatId == privateChatId)
                .OrderByDescending(pm => pm.CreatedAt)
                .Skip(skip)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task Delete(PrivateMessage message)
        {
            _context.PrivateMessages.Remove(message);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<PrivateMessage>> GetMessagesByChatIdAsync(int chatId, int page, int pageSize)
        {
            var skip = (page - 1) * pageSize;
            return await _context.PrivateMessages
                .Include(pm => pm.Sender)
                .Include(pm => pm.Receiver)
                .Where(pm => pm.PrivateChatId == chatId)
                .OrderByDescending(pm => pm.CreatedAt)
                .Skip(skip)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<int> GetUnreadCountAsync(int userId, int privateChatId)
        {
            return await _context.PrivateMessages
                .CountAsync(pm => pm.ReceiverId == userId && pm.PrivateChatId == privateChatId && !pm.IsRead);
        }

        public async Task<bool> MarkAsReadAsync(int messageId)
        {
            var message = await _context.PrivateMessages.FindAsync(messageId);
            if (message == null) return false;

            message.IsRead = true;
            message.ReadAt = DateTime.UtcNow;
            var result = await _context.SaveChangesAsync();
            return result > 0;
        }

        public async Task<bool> MarkAllAsReadAsync(int userId, int senderId)
        {
            var unreadMessages = await _context.PrivateMessages
                .Where(pm => pm.ReceiverId == userId && pm.SenderId == senderId && !pm.IsRead)
                .ToListAsync();

            foreach (var message in unreadMessages)
            {
                message.IsRead = true;
                message.ReadAt = DateTime.UtcNow;
            }

            var result = await _context.SaveChangesAsync();
            return result > 0;
        }
    }
} 