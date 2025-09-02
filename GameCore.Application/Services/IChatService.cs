using GameCore.Application.Common;
using GameCore.Application.DTOs;

namespace GameCore.Application.Services
{
    /// <summary>
    /// 聊天服務介面
    /// </summary>
    public interface IChatService
    {
        /// <summary>
        /// 取得用戶的聊天室列表
        /// </summary>
        /// <param name="userId">用戶 ID</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>聊天室列表</returns>
        Task<Result<IEnumerable<ChatRoomResponse>>> GetUserChatRoomsAsync(int userId, CancellationToken cancellationToken = default);

        /// <summary>
        /// 建立聊天室
        /// </summary>
        /// <param name="request">建立聊天室請求</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>聊天室資訊</returns>
        Task<Result<ChatRoomResponse>> CreateChatRoomAsync(CreateChatRoomRequest request, CancellationToken cancellationToken = default);

        /// <summary>
        /// 加入聊天室
        /// </summary>
        /// <param name="roomId">聊天室 ID</param>
        /// <param name="userId">用戶 ID</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>加入結果</returns>
        Task<OperationResult> JoinChatRoomAsync(int roomId, int userId, CancellationToken cancellationToken = default);

        /// <summary>
        /// 離開聊天室
        /// </summary>
        /// <param name="roomId">聊天室 ID</param>
        /// <param name="userId">用戶 ID</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>離開結果</returns>
        Task<OperationResult> LeaveChatRoomAsync(int roomId, int userId, CancellationToken cancellationToken = default);

        /// <summary>
        /// 發送訊息
        /// </summary>
        /// <param name="request">發送訊息請求</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>訊息資訊</returns>
        Task<Result<ChatMessageResponse>> SendMessageAsync(SendMessageRequest request, CancellationToken cancellationToken = default);

        /// <summary>
        /// 取得聊天室訊息
        /// </summary>
        /// <param name="roomId">聊天室 ID</param>
        /// <param name="pageNumber">頁碼</param>
        /// <param name="pageSize">每頁大小</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>訊息列表</returns>
        Task<Result<PagedResult<ChatMessageResponse>>> GetChatRoomMessagesAsync(int roomId, int pageNumber, int pageSize, CancellationToken cancellationToken = default);

        /// <summary>
        /// 取得聊天室成員
        /// </summary>
        /// <param name="roomId">聊天室 ID</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>成員列表</returns>
        Task<Result<IEnumerable<ChatRoomMemberResponse>>> GetChatRoomMembersAsync(int roomId, CancellationToken cancellationToken = default);
    }

    /// <summary>
    /// 聊天室回應
    /// </summary>
    public class ChatRoomResponse
    {
        /// <summary>
        /// 聊天室 ID
        /// </summary>
        public int RoomId { get; set; }

        /// <summary>
        /// 聊天室名稱
        /// </summary>
        public string RoomName { get; set; } = string.Empty;

        /// <summary>
        /// 聊天室類型
        /// </summary>
        public string RoomType { get; set; } = string.Empty;

        /// <summary>
        /// 建立者 ID
        /// </summary>
        public int CreatedBy { get; set; }

        /// <summary>
        /// 建立時間
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// 最後訊息時間
        /// </summary>
        public DateTime? LastMessageAt { get; set; }

        /// <summary>
        /// 成員數量
        /// </summary>
        public int MemberCount { get; set; }
    }

    /// <summary>
    /// 建立聊天室請求
    /// </summary>
    public class CreateChatRoomRequest
    {
        /// <summary>
        /// 聊天室名稱
        /// </summary>
        public string RoomName { get; set; } = string.Empty;

        /// <summary>
        /// 聊天室類型
        /// </summary>
        public string RoomType { get; set; } = string.Empty;

        /// <summary>
        /// 初始成員 ID 列表
        /// </summary>
        public List<int> InitialMemberIds { get; set; } = new List<int>();
    }

    /// <summary>
    /// 聊天訊息回應
    /// </summary>
    public class ChatMessageResponse
    {
        /// <summary>
        /// 訊息 ID
        /// </summary>
        public int MessageId { get; set; }

        /// <summary>
        /// 聊天室 ID
        /// </summary>
        public int RoomId { get; set; }

        /// <summary>
        /// 發送者 ID
        /// </summary>
        public int SenderId { get; set; }

        /// <summary>
        /// 發送者名稱
        /// </summary>
        public string SenderName { get; set; } = string.Empty;

        /// <summary>
        /// 訊息內容
        /// </summary>
        public string Content { get; set; } = string.Empty;

        /// <summary>
        /// 訊息類型
        /// </summary>
        public string MessageType { get; set; } = string.Empty;

        /// <summary>
        /// 發送時間
        /// </summary>
        public DateTime SentAt { get; set; }
    }

    /// <summary>
    /// 發送訊息請求
    /// </summary>
    public class SendMessageRequest
    {
        /// <summary>
        /// 聊天室 ID
        /// </summary>
        public int RoomId { get; set; }

        /// <summary>
        /// 發送者 ID
        /// </summary>
        public int SenderId { get; set; }

        /// <summary>
        /// 訊息內容
        /// </summary>
        public string Content { get; set; } = string.Empty;

        /// <summary>
        /// 訊息類型
        /// </summary>
        public string MessageType { get; set; } = "text";
    }

    /// <summary>
    /// 聊天室成員回應
    /// </summary>
    public class ChatRoomMemberResponse
    {
        /// <summary>
        /// 用戶 ID
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// 用戶名稱
        /// </summary>
        public string UserName { get; set; } = string.Empty;

        /// <summary>
        /// 加入時間
        /// </summary>
        public DateTime JoinedAt { get; set; }

        /// <summary>
        /// 是否為管理員
        /// </summary>
        public bool IsAdmin { get; set; }

        /// <summary>
        /// 角色
        /// </summary>
        public string Role { get; set; } = string.Empty;
    }
} 