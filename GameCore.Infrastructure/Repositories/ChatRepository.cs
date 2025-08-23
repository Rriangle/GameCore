using GameCore.Core.Entities;
using GameCore.Core.Interfaces;
using GameCore.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace GameCore.Infrastructure.Repositories
{
    public class ChatRepository : Repository<ChatRoom>, IChatRepository
    {
        public ChatRepository(GameCoreDbContext context) : base(context)
        {
        }

        // ChatRoom methods
        public async Task<IEnumerable<ChatRoom>> GetUserChatRoomsAsync(int userId)
        {
            return await _context.ChatRooms
                .Include(cr => cr.Members.Where(m => m.UserId == userId))
                .Include(cr => cr.Messages.OrderByDescending(m => m.CreateTime).Take(1))
                .Where(cr => cr.Members.Any(m => m.UserId == userId))
                .OrderByDescending(cr => cr.UpdateTime)
                .ToListAsync();
        }

        public async Task<ChatRoom?> GetChatRoomWithMembersAsync(int roomId)
        {
            return await _context.ChatRooms
                .Include(cr => cr.Members)
                .ThenInclude(m => m.User)
                .FirstOrDefaultAsync(cr => cr.Id == roomId);
        }

        public async Task<ChatRoom?> GetPrivateChatRoomAsync(int userId1, int userId2)
        {
            return await _context.ChatRooms
                .Include(cr => cr.Members)
                .Where(cr => cr.Type == ChatRoomType.Private &&
                           cr.Members.Count == 2 &&
                           cr.Members.Any(m => m.UserId == userId1) &&
                           cr.Members.Any(m => m.UserId == userId2))
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<ChatRoom>> GetGroupChatRoomsAsync(int userId)
        {
            return await _context.ChatRooms
                .Include(cr => cr.Members)
                .Where(cr => cr.Type == ChatRoomType.Group &&
                           cr.Members.Any(m => m.UserId == userId))
                .OrderByDescending(cr => cr.UpdateTime)
                .ToListAsync();
        }

        // ChatMessage methods
        public async Task<IEnumerable<ChatMessage>> GetRoomMessagesAsync(int roomId, int page = 1, int pageSize = 50)
        {
            return await _context.ChatMessages
                .Include(m => m.User)
                .Where(m => m.ChatRoomId == roomId)
                .OrderByDescending(m => m.CreateTime)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<ChatMessage?> GetMessageByIdAsync(int messageId)
        {
            return await _context.ChatMessages
                .Include(m => m.User)
                .Include(m => m.ChatRoom)
                .FirstOrDefaultAsync(m => m.Id == messageId);
        }

        public async Task<IEnumerable<ChatMessage>> GetUnreadMessagesAsync(int userId, int roomId)
        {
            // Get user's last read time for this room
            var member = await _context.ChatRoomMembers
                .FirstOrDefaultAsync(m => m.UserId == userId && m.ChatRoomId == roomId);

            if (member == null) return new List<ChatMessage>();

            return await _context.ChatMessages
                .Include(m => m.User)
                .Where(m => m.ChatRoomId == roomId && 
                           m.CreateTime > member.LastReadTime && 
                           m.UserId != userId)
                .OrderBy(m => m.CreateTime)
                .ToListAsync();
        }

        public async Task<int> GetUnreadMessageCountAsync(int userId, int roomId)
        {
            var member = await _context.ChatRoomMembers
                .FirstOrDefaultAsync(m => m.UserId == userId && m.ChatRoomId == roomId);

            if (member == null) return 0;

            return await _context.ChatMessages
                .CountAsync(m => m.ChatRoomId == roomId && 
                               m.CreateTime > member.LastReadTime && 
                               m.UserId != userId);
        }

        // ChatRoomMember methods
        public async Task<ChatRoomMember?> GetRoomMemberAsync(int roomId, int userId)
        {
            return await _context.ChatRoomMembers
                .Include(m => m.User)
                .Include(m => m.ChatRoom)
                .FirstOrDefaultAsync(m => m.ChatRoomId == roomId && m.UserId == userId);
        }

        public async Task<IEnumerable<ChatRoomMember>> GetRoomMembersAsync(int roomId)
        {
            return await _context.ChatRoomMembers
                .Include(m => m.User)
                .Where(m => m.ChatRoomId == roomId)
                .OrderBy(m => m.JoinTime)
                .ToListAsync();
        }

        public async Task UpdateLastReadTimeAsync(int userId, int roomId)
        {
            var member = await _context.ChatRoomMembers
                .FirstOrDefaultAsync(m => m.UserId == userId && m.ChatRoomId == roomId);

            if (member != null)
            {
                member.LastReadTime = DateTime.UtcNow;
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> IsUserInRoomAsync(int userId, int roomId)
        {
            return await _context.ChatRoomMembers
                .AnyAsync(m => m.UserId == userId && m.ChatRoomId == roomId);
        }

        // PrivateChat methods
        public async Task<PrivateChat?> GetPrivateChatAsync(int userId1, int userId2)
        {
            return await _context.PrivateChats
                .Include(pc => pc.User1)
                .Include(pc => pc.User2)
                .Include(pc => pc.Messages.OrderByDescending(m => m.CreateTime).Take(1))
                .FirstOrDefaultAsync(pc => 
                    (pc.User1Id == userId1 && pc.User2Id == userId2) ||
                    (pc.User1Id == userId2 && pc.User2Id == userId1));
        }

        public async Task<IEnumerable<PrivateChat>> GetUserPrivateChatsAsync(int userId)
        {
            return await _context.PrivateChats
                .Include(pc => pc.User1)
                .Include(pc => pc.User2)
                .Include(pc => pc.Messages.OrderByDescending(m => m.CreateTime).Take(1))
                .Where(pc => pc.User1Id == userId || pc.User2Id == userId)
                .OrderByDescending(pc => pc.UpdateTime)
                .ToListAsync();
        }

        // PrivateMessage methods
        public async Task<IEnumerable<PrivateMessage>> GetPrivateMessagesAsync(int privateChatId, int page = 1, int pageSize = 50)
        {
            return await _context.PrivateMessages
                .Include(m => m.Sender)
                .Where(m => m.PrivateChatId == privateChatId)
                .OrderByDescending(m => m.CreateTime)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<int> GetPrivateUnreadCountAsync(int userId, int privateChatId)
        {
            return await _context.PrivateMessages
                .CountAsync(m => m.PrivateChatId == privateChatId && 
                               m.ReceiverId == userId && 
                               !m.IsRead);
        }

        public async Task MarkPrivateMessagesAsReadAsync(int userId, int privateChatId)
        {
            var unreadMessages = await _context.PrivateMessages
                .Where(m => m.PrivateChatId == privateChatId && 
                           m.ReceiverId == userId && 
                           !m.IsRead)
                .ToListAsync();

            foreach (var message in unreadMessages)
            {
                message.IsRead = true;
                message.ReadTime = DateTime.UtcNow;
            }

            await _context.SaveChangesAsync();
        }
    }
}