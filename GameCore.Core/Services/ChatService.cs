using GameCore.Core.Entities;
using GameCore.Core.Interfaces;

namespace GameCore.Core.Services
{
    public class ChatService : IChatService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ChatService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ChatMessage> SendMessageAsync(int senderId, int? receiverId, string content)
        {
            var message = new ChatMessage
            {
                SenderId = senderId,
                ReceiverId = receiverId,
                ChatContent = content,
                SentAt = DateTime.UtcNow,
                IsRead = false,
                IsSent = true
            };

            await _unitOfWork.ChatRepository.AddAsync(message);
            await _unitOfWork.SaveChangesAsync();

            return message;
        }

        public async Task<IEnumerable<ChatMessage>> GetUserMessagesAsync(int userId)
        {
            var messages = await _unitOfWork.ChatRepository.GetAllAsync();
            return messages.Where(m => m.SenderId == userId || m.ReceiverId == userId);
        }

        public async Task<IEnumerable<ChatMessage>> GetChatHistoryAsync(int userId1, int userId2)
        {
            var messages = await _unitOfWork.ChatRepository.GetAllAsync();
            return messages.Where(m => 
                (m.SenderId == userId1 && m.ReceiverId == userId2) || 
                (m.SenderId == userId2 && m.ReceiverId == userId1))
                .OrderBy(m => m.SentAt);
        }

        public async Task<bool> MarkAsReadAsync(int messageId)
        {
            var message = await _unitOfWork.ChatRepository.GetByIdAsync(messageId);
            if (message == null) return false;

            message.IsRead = true;
            await _unitOfWork.SaveChangesAsync();
            return true;
        }
    }
}