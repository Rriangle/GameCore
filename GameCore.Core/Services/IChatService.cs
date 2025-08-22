using GameCore.Core.Entities;

namespace GameCore.Core.Services
{
    /// <summary>
    /// 聊天服務介面
    /// </summary>
    public interface IChatService
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
        /// 建立群組聊天室
        /// </summary>
        /// <param name="creatorId">建立者ID</param>
        /// <param name="roomName">聊天室名稱</param>
        /// <param name="memberIds">成員ID列表</param>
        /// <returns>聊天室</returns>
        Task<ChatRoom?> CreateGroupRoomAsync(int creatorId, string roomName, List<int> memberIds);

        /// <summary>
        /// 加入群組聊天室
        /// </summary>
        /// <param name="roomId">聊天室ID</param>
        /// <param name="userId">使用者ID</param>
        /// <returns>操作結果</returns>
        Task<bool> JoinGroupRoomAsync(int roomId, int userId);

        /// <summary>
        /// 離開群組聊天室
        /// </summary>
        /// <param name="roomId">聊天室ID</param>
        /// <param name="userId">使用者ID</param>
        /// <returns>操作結果</returns>
        Task<bool> LeaveGroupRoomAsync(int roomId, int userId);

        /// <summary>
        /// 取得聊天室的訊息
        /// </summary>
        /// <param name="roomId">聊天室ID</param>
        /// <param name="page">頁碼</param>
        /// <param name="pageSize">每頁大小</param>
        /// <returns>訊息列表</returns>
        Task<IEnumerable<ChatMessage>> GetMessagesByRoomAsync(int roomId, int page, int pageSize);

        /// <summary>
        /// 發送聊天訊息
        /// </summary>
        /// <param name="roomId">聊天室ID</param>
        /// <param name="senderId">發送者ID</param>
        /// <param name="content">訊息內容</param>
        /// <param name="messageType">訊息類型</param>
        /// <returns>發送結果</returns>
        Task<bool> SendMessageAsync(int roomId, int senderId, string content, ChatMessageType messageType = ChatMessageType.Text);

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

        /// <summary>
        /// 取得私人聊天
        /// </summary>
        /// <param name="userId">使用者ID</param>
        /// <returns>私人聊天列表</returns>
        Task<IEnumerable<PrivateChat>> GetPrivateChatsAsync(int userId);

        /// <summary>
        /// 取得兩個使用者之間的私人聊天
        /// </summary>
        /// <param name="user1Id">使用者1 ID</param>
        /// <param name="user2Id">使用者2 ID</param>
        /// <returns>私人聊天</returns>
        Task<PrivateChat?> GetPrivateChatAsync(int user1Id, int user2Id);

        /// <summary>
        /// 發送私人訊息
        /// </summary>
        /// <param name="chatId">聊天ID</param>
        /// <param name="senderId">發送者ID</param>
        /// <param name="receiverId">接收者ID</param>
        /// <param name="content">訊息內容</param>
        /// <param name="messageType">訊息類型</param>
        /// <returns>發送結果</returns>
        Task<bool> SendPrivateMessageAsync(int chatId, int senderId, int receiverId, string content, ChatMessageType messageType = ChatMessageType.Text);

        /// <summary>
        /// 取得私人聊天的訊息
        /// </summary>
        /// <param name="chatId">聊天ID</param>
        /// <param name="page">頁碼</param>
        /// <param name="pageSize">每頁大小</param>
        /// <returns>訊息列表</returns>
        Task<IEnumerable<PrivateMessage>> GetPrivateMessagesAsync(int chatId, int page, int pageSize);

        /// <summary>
        /// 檢查使用者是否為聊天室成員
        /// </summary>
        /// <param name="roomId">聊天室ID</param>
        /// <param name="userId">使用者ID</param>
        /// <returns>是否為成員</returns>
        Task<bool> IsUserInRoomAsync(int roomId, int userId);

        /// <summary>
        /// 取得聊天室成員
        /// </summary>
        /// <param name="roomId">聊天室ID</param>
        /// <returns>成員列表</returns>
        Task<IEnumerable<ChatRoomMember>> GetRoomMembersAsync(int roomId);

        /// <summary>
        /// 更新聊天室成員角色
        /// </summary>
        /// <param name="roomId">聊天室ID</param>
        /// <param name="userId">使用者ID</param>
        /// <param name="role">新角色</param>
        /// <returns>操作結果</returns>
        Task<bool> UpdateMemberRoleAsync(int roomId, int userId, ChatMemberRole role);
    }
}