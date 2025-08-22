using GameCore.Core.Entities;

namespace GameCore.Core.Interfaces
{
    /// <summary>
    /// 聊天室倉庫介面
    /// </summary>
    public interface IChatRepository : IRepository<ChatRoom>
    {
        /// <summary>
        /// 取得使用者的聊天室
        /// </summary>
        /// <param name="userId">使用者ID</param>
        /// <returns>聊天室列表</returns>
        Task<IEnumerable<ChatRoom>> GetRoomsByUserAsync(int userId);

        /// <summary>
        /// 建立私人聊天室
        /// </summary>
        /// <param name="user1Id">使用者1 ID</param>
        /// <param name="user2Id">使用者2 ID</param>
        /// <returns>聊天室</returns>
        Task<ChatRoom?> CreatePrivateRoomAsync(int user1Id, int user2Id);

        /// <summary>
        /// 取得私人聊天室
        /// </summary>
        /// <param name="user1Id">使用者1 ID</param>
        /// <param name="user2Id">使用者2 ID</param>
        /// <returns>聊天室</returns>
        Task<ChatRoom?> GetPrivateRoomAsync(int user1Id, int user2Id);

        /// <summary>
        /// 檢查使用者是否為聊天室成員
        /// </summary>
        /// <param name="roomId">聊天室ID</param>
        /// <param name="userId">使用者ID</param>
        /// <returns>是否為成員</returns>
        Task<bool> IsUserInRoomAsync(int roomId, int userId);
    }

    /// <summary>
    /// 聊天訊息倉庫介面
    /// </summary>
    public interface IChatMessageRepository : IRepository<ChatMessage>
    {
        /// <summary>
        /// 取得聊天室的訊息
        /// </summary>
        /// <param name="roomId">聊天室ID</param>
        /// <param name="page">頁碼</param>
        /// <param name="pageSize">每頁大小</param>
        /// <returns>訊息列表</returns>
        Task<IEnumerable<ChatMessage>> GetMessagesByRoomAsync(int roomId, int page, int pageSize);

        /// <summary>
        /// 取得未讀訊息數量
        /// </summary>
        /// <param name="roomId">聊天室ID</param>
        /// <param name="userId">使用者ID</param>
        /// <returns>未讀訊息數量</returns>
        Task<int> GetUnreadCountAsync(int roomId, int userId);

        /// <summary>
        /// 標記訊息為已讀
        /// </summary>
        /// <param name="roomId">聊天室ID</param>
        /// <param name="userId">使用者ID</param>
        /// <returns>操作結果</returns>
        Task<bool> MarkAsReadAsync(int roomId, int userId);
    }

    /// <summary>
    /// 私人聊天倉庫介面
    /// </summary>
    public interface IPrivateChatRepository : IRepository<PrivateChat>
    {
        /// <summary>
        /// 取得使用者的私人聊天
        /// </summary>
        /// <param name="userId">使用者ID</param>
        /// <returns>私人聊天列表</returns>
        Task<IEnumerable<PrivateChat>> GetChatsByUserAsync(int userId);

        /// <summary>
        /// 取得兩個使用者之間的私人聊天
        /// </summary>
        /// <param name="user1Id">使用者1 ID</param>
        /// <param name="user2Id">使用者2 ID</param>
        /// <returns>私人聊天</returns>
        Task<PrivateChat?> GetChatBetweenUsersAsync(int user1Id, int user2Id);

        /// <summary>
        /// 建立私人聊天
        /// </summary>
        /// <param name="user1Id">使用者1 ID</param>
        /// <param name="user2Id">使用者2 ID</param>
        /// <returns>私人聊天</returns>
        Task<PrivateChat?> CreateChatAsync(int user1Id, int user2Id);
    }

    /// <summary>
    /// 私人訊息倉庫介面
    /// </summary>
    public interface IPrivateMessageRepository : IRepository<PrivateMessage>
    {
        /// <summary>
        /// 取得私人聊天的訊息
        /// </summary>
        /// <param name="chatId">聊天ID</param>
        /// <param name="page">頁碼</param>
        /// <param name="pageSize">每頁大小</param>
        /// <returns>訊息列表</returns>
        Task<IEnumerable<PrivateMessage>> GetMessagesByChatAsync(int chatId, int page, int pageSize);

        /// <summary>
        /// 取得未讀訊息數量
        /// </summary>
        /// <param name="chatId">聊天ID</param>
        /// <param name="userId">使用者ID</param>
        /// <returns>未讀訊息數量</returns>
        Task<int> GetUnreadCountAsync(int chatId, int userId);

        /// <summary>
        /// 標記訊息為已讀
        /// </summary>
        /// <param name="chatId">聊天ID</param>
        /// <param name="userId">使用者ID</param>
        /// <returns>操作結果</returns>
        Task<bool> MarkAsReadAsync(int chatId, int userId);
    }
}