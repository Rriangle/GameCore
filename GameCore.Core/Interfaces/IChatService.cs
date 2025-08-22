using GameCore.Core.Entities;

namespace GameCore.Core.Interfaces
{
    /// <summary>
    /// 聊天服務介面
    /// </summary>
    public interface IChatService
    {
        /// <summary>
        /// 發送私人訊息
        /// </summary>
        Task<ChatMessage> SendPrivateMessageAsync(int senderId, int receiverId, string content);

        /// <summary>
        /// 取得聊天記錄
        /// </summary>
        Task<List<ChatMessage>> GetChatHistoryAsync(int userId1, int userId2);

        /// <summary>
        /// 取得最新聊天記錄
        /// </summary>
        Task<List<ChatMessage>> GetRecentChatsAsync(int userId);

        /// <summary>
        /// 標記訊息為已讀
        /// </summary>
        Task MarkMessagesAsReadAsync(int senderId, int receiverId);
    }
}

