using GameCore.Domain.Entities;

namespace GameCore.Domain.Interfaces
{
    /// <summary>
    /// 聊天室 Repository 介面
    /// </summary>
    public interface IChatRepository
    {
        /// <summary>
        /// 根據ID取得聊天室
        /// </summary>
        Task<ChatRoom?> GetByIdAsync(int id);

        /// <summary>
        /// 取得聊天室成員
        /// </summary>
        Task<ChatRoomMember?> GetRoomMemberAsync(int chatRoomId, int userId);

        /// <summary>
        /// 取得聊天室所有成員
        /// </summary>
        Task<IEnumerable<ChatRoomMember>> GetRoomMembersAsync(int chatRoomId);

        /// <summary>
        /// 移除聊天室成員
        /// </summary>
        Task RemoveMember(int chatRoomId, int userId);

        /// <summary>
        /// 根據聊天室ID取得訊息
        /// </summary>
        Task<IEnumerable<ChatMessage>> GetByChatRoomIdAsync(int chatRoomId, int page, int pageSize);

        /// <summary>
        /// 新增聊天室
        /// </summary>
        Task<ChatRoom> AddAsync(ChatRoom chatRoom);

        /// <summary>
        /// 新增聊天室成員
        /// </summary>
        Task<ChatRoomMember> AddMember(ChatRoomMember member);

        /// <summary>
        /// 更新聊天室成員
        /// </summary>
        Task<ChatRoomMember> UpdateRoomMemberAsync(ChatRoomMember member);

        /// <summary>
        /// 更新聊天室
        /// </summary>
        Task<ChatRoom> Update(ChatRoom chatRoom);

        /// <summary>
        /// 取得聊天室成員
        /// </summary>
        Task<ChatRoomMember?> GetChatRoomMemberAsync(int chatRoomId, int userId);

        /// <summary>
        /// 根據用戶ID取得聊天室
        /// </summary>
        Task<IEnumerable<ChatRoom>> GetChatRoomsByUserIdAsync(int userId);

        /// <summary>
        /// 新增聊天室
        /// </summary>
        Task<ChatRoom> Add(ChatRoom chatRoom);
    }
}
