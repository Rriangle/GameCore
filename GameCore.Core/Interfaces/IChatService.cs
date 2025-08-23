using GameCore.Core.Entities;

namespace GameCore.Core.Interfaces
{
    /// <summary>
    /// 聊天服務介面
    /// </summary>
    public interface IChatService
    {
        Task<ChatMessage> SendMessageAsync(int senderId, int? receiverId, string content);
        Task<IEnumerable<ChatMessage>> GetUserMessagesAsync(int userId);
        Task<IEnumerable<ChatMessage>> GetChatHistoryAsync(int userId1, int userId2);
        Task<bool> MarkAsReadAsync(int messageId);
    }
}