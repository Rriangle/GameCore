using GameCore.Domain.Entities;
using GameCore.Domain.Interfaces;
using GameCore.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace GameCore.Infrastructure.Repositories
{
    public class ChatMessageRepository : Repository<ChatMessage>, IChatMessageRepository
    {
        private readonly ILogger<ChatMessageRepository> _logger;

        public ChatMessageRepository(GameCoreDbContext context, ILogger<ChatMessageRepository> logger) : base(context)
        {
            _logger = logger;
        }

        public async Task<IEnumerable<ChatMessage>> GetByChatRoomIdAsync(int chatRoomId, int page, int pageSize)
        {
            return await _dbSet
                .Include(m => m.Sender)
                .Where(m => m.ChatRoomId == chatRoomId)
                .OrderByDescending(m => m.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<ChatMessage> AddAsync(ChatMessage message)
        {
            var result = await _dbSet.AddAsync(message);
            return result.Entity;
        }

        public async Task<IEnumerable<ChatMessage>> GetMessagesByRoomAsync(int roomId)
        {
            return await _dbSet
                .Include(m => m.Sender)
                .Where(m => m.ChatRoomId == roomId)
                .OrderBy(m => m.CreatedAt)
                .ToListAsync();
        }

        public async Task<int> GetUnreadCountAsync(int roomId, int userId)
        {
            return await _dbSet
                .CountAsync(m => m.ChatRoomId == roomId && 
                                m.SenderId != userId && 
                                !m.IsRead);
        }

        public async Task MarkAsReadAsync(int messageId)
        {
            var message = await _dbSet.FindAsync(messageId);
            if (message != null)
            {
                message.IsRead = true;
                _dbSet.Update(message);
            }
        }

        public async Task<ChatMessage> Add(ChatMessage message)
        {
            var result = await _dbSet.AddAsync(message);
            return result.Entity;
        }
    }
} 