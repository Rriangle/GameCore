using Microsoft.AspNetCore.Mvc;
using GameCore.Core.Services;
using GameCore.Core.Entities;
using Microsoft.AspNetCore.Authorization;

namespace GameCore.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ChatController : ControllerBase
    {
        private readonly IChatService _chatService;
        private readonly ILogger<ChatController> _logger;

        public ChatController(IChatService chatService, ILogger<ChatController> logger)
        {
            _chatService = chatService;
            _logger = logger;
        }

        [HttpGet("rooms")]
        [Authorize]
        public async Task<IActionResult> GetChatRooms()
        {
            try
            {
                var userId = GetCurrentUserId();
                var rooms = await _chatService.GetUserChatRoomsAsync(userId);
                return Ok(new { Success = true, Data = rooms });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting chat rooms");
                return StatusCode(500, new { Success = false, Message = "取得聊天室清單失敗" });
            }
        }

        [HttpGet("rooms/{id}")]
        [Authorize]
        public async Task<IActionResult> GetChatRoom(int id)
        {
            try
            {
                var room = await _chatService.GetChatRoomAsync(id);
                if (room == null)
                {
                    return NotFound(new { Success = false, Message = "聊天室不存在" });
                }
                return Ok(new { Success = true, Data = room });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting chat room {RoomId}", id);
                return StatusCode(500, new { Success = false, Message = "取得聊天室資訊失敗" });
            }
        }

        [HttpPost("rooms")]
        [Authorize]
        public async Task<IActionResult> CreateChatRoom([FromBody] CreateChatRoomRequest request)
        {
            try
            {
                var userId = GetCurrentUserId();
                var room = await _chatService.CreateChatRoomAsync(
                    userId, 
                    request.Name, 
                    request.Type, 
                    request.MemberIds);
                return Ok(new { Success = true, Data = room, Message = "聊天室創建成功" });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { Success = false, Message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating chat room");
                return StatusCode(500, new { Success = false, Message = "創建聊天室失敗" });
            }
        }

        [HttpPut("rooms/{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateChatRoom(int id, [FromBody] UpdateChatRoomRequest request)
        {
            try
            {
                var userId = GetCurrentUserId();
                await _chatService.UpdateChatRoomAsync(id, userId, request.Name, request.Description);
                return Ok(new { Success = true, Message = "聊天室更新成功" });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { Success = false, Message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating chat room {RoomId}", id);
                return StatusCode(500, new { Success = false, Message = "更新聊天室失敗" });
            }
        }

        [HttpDelete("rooms/{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteChatRoom(int id)
        {
            try
            {
                var userId = GetCurrentUserId();
                await _chatService.DeleteChatRoomAsync(id, userId);
                return Ok(new { Success = true, Message = "聊天室刪除成功" });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { Success = false, Message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting chat room {RoomId}", id);
                return StatusCode(500, new { Success = false, Message = "刪除聊天室失敗" });
            }
        }

        [HttpGet("rooms/{id}/messages")]
        [Authorize]
        public async Task<IActionResult> GetRoomMessages(int id, [FromQuery] int page = 1, [FromQuery] int pageSize = 50)
        {
            try
            {
                var messages = await _chatService.GetRoomMessagesAsync(id, page, pageSize);
                return Ok(new { Success = true, Data = messages });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting messages for room {RoomId}", id);
                return StatusCode(500, new { Success = false, Message = "取得訊息清單失敗" });
            }
        }

        [HttpPost("rooms/{id}/messages")]
        [Authorize]
        public async Task<IActionResult> SendMessage(int id, [FromBody] SendMessageRequest request)
        {
            try
            {
                var userId = GetCurrentUserId();
                var message = await _chatService.SendMessageAsync(
                    id, 
                    userId, 
                    request.Content, 
                    request.Type);
                return Ok(new { Success = true, Data = message, Message = "訊息發送成功" });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { Success = false, Message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending message to room {RoomId}", id);
                return StatusCode(500, new { Success = false, Message = "發送訊息失敗" });
            }
        }

        [HttpDelete("messages/{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteMessage(int id)
        {
            try
            {
                var userId = GetCurrentUserId();
                await _chatService.DeleteMessageAsync(id, userId);
                return Ok(new { Success = true, Message = "訊息刪除成功" });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { Success = false, Message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting message {MessageId}", id);
                return StatusCode(500, new { Success = false, Message = "刪除訊息失敗" });
            }
        }

        [HttpPost("rooms/{id}/join")]
        [Authorize]
        public async Task<IActionResult> JoinChatRoom(int id)
        {
            try
            {
                var userId = GetCurrentUserId();
                await _chatService.JoinChatRoomAsync(id, userId);
                return Ok(new { Success = true, Message = "加入聊天室成功" });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { Success = false, Message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error joining chat room {RoomId}", id);
                return StatusCode(500, new { Success = false, Message = "加入聊天室失敗" });
            }
        }

        [HttpDelete("rooms/{id}/leave")]
        [Authorize]
        public async Task<IActionResult> LeaveChatRoom(int id)
        {
            try
            {
                var userId = GetCurrentUserId();
                await _chatService.LeaveChatRoomAsync(id, userId);
                return Ok(new { Success = true, Message = "離開聊天室成功" });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { Success = false, Message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error leaving chat room {RoomId}", id);
                return StatusCode(500, new { Success = false, Message = "離開聊天室失敗" });
            }
        }

        [HttpGet("private-chats")]
        [Authorize]
        public async Task<IActionResult> GetPrivateChats()
        {
            try
            {
                var userId = GetCurrentUserId();
                var chats = await _chatService.GetUserPrivateChatsAsync(userId);
                return Ok(new { Success = true, Data = chats });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting private chats");
                return StatusCode(500, new { Success = false, Message = "取得私聊清單失敗" });
            }
        }

        [HttpPost("private-chats")]
        [Authorize]
        public async Task<IActionResult> CreatePrivateChat([FromBody] CreatePrivateChatRequest request)
        {
            try
            {
                var userId = GetCurrentUserId();
                var chat = await _chatService.CreatePrivateChatAsync(userId, request.OtherUserId);
                return Ok(new { Success = true, Data = chat, Message = "私聊創建成功" });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { Success = false, Message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating private chat");
                return StatusCode(500, new { Success = false, Message = "創建私聊失敗" });
            }
        }

        [HttpGet("private-chats/{id}/messages")]
        [Authorize]
        public async Task<IActionResult> GetPrivateChatMessages(int id, [FromQuery] int page = 1, [FromQuery] int pageSize = 50)
        {
            try
            {
                var messages = await _chatService.GetPrivateChatMessagesAsync(id, page, pageSize);
                return Ok(new { Success = true, Data = messages });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting private chat messages {ChatId}", id);
                return StatusCode(500, new { Success = false, Message = "取得私聊訊息失敗" });
            }
        }

        [HttpPost("private-chats/{id}/messages")]
        [Authorize]
        public async Task<IActionResult> SendPrivateMessage(int id, [FromBody] SendPrivateMessageRequest request)
        {
            try
            {
                var userId = GetCurrentUserId();
                var message = await _chatService.SendPrivateMessageAsync(
                    id, 
                    userId, 
                    request.ReceiverId, 
                    request.Content);
                return Ok(new { Success = true, Data = message, Message = "私聊訊息發送成功" });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { Success = false, Message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending private message {ChatId}", id);
                return StatusCode(500, new { Success = false, Message = "發送私聊訊息失敗" });
            }
        }

        [HttpPost("messages/{id}/read")]
        [Authorize]
        public async Task<IActionResult> MarkMessageAsRead(int id)
        {
            try
            {
                var userId = GetCurrentUserId();
                await _chatService.MarkMessageAsReadAsync(id, userId);
                return Ok(new { Success = true, Message = "訊息標記為已讀" });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { Success = false, Message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error marking message as read {MessageId}", id);
                return StatusCode(500, new { Success = false, Message = "標記訊息失敗" });
            }
        }

        [HttpGet("unread-count")]
        [Authorize]
        public async Task<IActionResult> GetUnreadCount()
        {
            try
            {
                var userId = GetCurrentUserId();
                var count = await _chatService.GetUnreadMessageCountAsync(userId);
                return Ok(new { Success = true, Data = new { UnreadCount = count } });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting unread count");
                return StatusCode(500, new { Success = false, Message = "取得未讀訊息數量失敗" });
            }
        }

        private int GetCurrentUserId()
        {
            var userIdClaim = User.FindFirst("UserId")?.Value;
            return int.TryParse(userIdClaim, out var userId) ? userId : 0;
        }
    }

    public class CreateChatRoomRequest
    {
        public string Name { get; set; } = "";
        public ChatRoomType Type { get; set; }
        public int[] MemberIds { get; set; } = Array.Empty<int>();
    }

    public class UpdateChatRoomRequest
    {
        public string Name { get; set; } = "";
        public string Description { get; set; } = "";
    }

    public class SendMessageRequest
    {
        public string Content { get; set; } = "";
        public ChatMessageType Type { get; set; } = ChatMessageType.Text;
    }

    public class CreatePrivateChatRequest
    {
        public int OtherUserId { get; set; }
    }

    public class SendPrivateMessageRequest
    {
        public int ReceiverId { get; set; }
        public string Content { get; set; } = "";
    }
}