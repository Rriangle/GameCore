using GameCore.Core.Entities;

namespace GameCore.Core.Interfaces
{
    /// <summary>
    /// 聊天 Repository 介面
    /// </summary>
    public interface IChatRepository : IRepository<ChatMessage>
    {
        /// <summary>
        /// 取得聊天歷史
        /// </summary>
        /// <param name="groupId">群組ID</param>
        /// <param name="userId">用戶ID</param>
        /// <param name="page">頁碼</param>
        /// <param name="pageSize">每頁數量</param>
        /// <returns></returns>
        Task<IEnumerable<ChatMessage>> GetChatHistoryAsync(int groupId, int userId, int page, int pageSize);

        /// <summary>
        /// 取得私訊歷史
        /// </summary>
        /// <param name="userId1">用戶1 ID</param>
        /// <param name="userId2">用戶2 ID</param>
        /// <param name="page">頁碼</param>
        /// <param name="pageSize">每頁數量</param>
        /// <returns></returns>
        Task<IEnumerable<ChatMessage>> GetPrivateMessagesAsync(int userId1, int userId2, int page, int pageSize);

        /// <summary>
        /// 取得未讀訊息數量
        /// </summary>
        /// <param name="userId">用戶ID</param>
        /// <returns></returns>
        Task<int> GetUnreadCountAsync(int userId);

        /// <summary>
        /// 標記訊息為已讀
        /// </summary>
        /// <param name="userId">用戶ID</param>
        /// <param name="groupId">群組ID</param>
        /// <returns></returns>
        Task MarkMessagesAsReadAsync(int userId, int groupId);
    }
}
