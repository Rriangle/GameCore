using GameCore.Domain.Entities;

namespace GameCore.Domain.Interfaces
{
    /// <summary>
    /// 私聊訊息 Repository 介面
    /// </summary>
    public interface IPrivateMessageRepository
    {
        /// <summary>
        /// 根據私聊ID取得訊息
        /// </summary>
        Task<IEnumerable<PrivateMessage>> GetByPrivateChatIdAsync(int privateChatId, int page, int pageSize);

        /// <summary>
        /// 刪除訊息
        /// </summary>
        Task Delete(PrivateMessage message);

        /// <summary>
        /// 新增訊息
        /// </summary>
        Task<PrivateMessage> AddAsync(PrivateMessage message);

        /// <summary>
        /// 根據聊天ID取得訊息
        /// </summary>
        Task<IEnumerable<PrivateMessage>> GetMessagesByChatIdAsync(int chatId, int page, int pageSize);

        /// <summary>
        /// 根據ID取得訊息
        /// </summary>
        Task<PrivateMessage?> GetByIdAsync(int messageId);

        /// <summary>
        /// 取得未讀訊息數量
        /// </summary>
        Task<int> GetUnreadCountAsync(int userId, int privateChatId);
    }
} 
