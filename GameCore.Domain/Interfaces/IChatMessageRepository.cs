using GameCore.Domain.Entities;

namespace GameCore.Domain.Interfaces
{
    /// <summary>
    /// 聊天訊息 Repository 介面
    /// </summary>
    public interface IChatMessageRepository
    {
        /// <summary>
        /// 根據聊天室ID取得訊息
        /// </summary>
        Task<IEnumerable<ChatMessage>> GetByChatRoomIdAsync(int chatRoomId, int page, int pageSize);

        /// <summary>
        /// 新增訊息
        /// </summary>
        Task<ChatMessage> AddAsync(ChatMessage message);

        /// <summary>
        /// 根據聊天室取得訊息
        /// </summary>
        Task<IEnumerable<ChatMessage>> GetMessagesByRoomAsync(int roomId);

        /// <summary>
        /// 取得未讀訊息數量
        /// </summary>
        Task<int> GetUnreadCountAsync(int roomId, int userId);

        /// <summary>
        /// 標記為已讀
        /// </summary>
        Task MarkAsReadAsync(int messageId);

        /// <summary>
        /// 新增訊息
        /// </summary>
        Task<ChatMessage> Add(ChatMessage message);
    }
} 
