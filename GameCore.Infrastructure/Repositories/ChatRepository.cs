using GameCore.Core.Entities;
using GameCore.Core.Interfaces;
using GameCore.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace GameCore.Infrastructure.Repositories
{
    public class ChatRepository : Repository<ChatMessage>, IChatRepository
    {
        public ChatRepository(GameCoreDbContext context) : base(context)
        {
        }

        public async Task<PagedResult<ChatMessage>> GetChatHistoryAsync(int userId1, int userId2, int page = 1, int pageSize = 50)
        {
            var query = _context.Set<ChatMessage>()
                .Where(c => (c.SenderId == userId1 && c.ReceiverId == userId2) || 
                           (c.SenderId == userId2 && c.ReceiverId == userId1))
                .OrderByDescending(c => c.SentAt)
                .Include(c => c.SenderUser)
                .Include(c => c.ReceiverUser);

            var totalCount = await query.CountAsync();
            var messages = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedResult<ChatMessage>
            {
                Items = messages,
                TotalCount = totalCount,
                Page = page,
                PageSize = pageSize
            };
        }
    }
}