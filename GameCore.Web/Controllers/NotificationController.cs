using Microsoft.AspNetCore.Mvc;
using GameCore.Core.Services;
using GameCore.Core.Entities;
using Microsoft.AspNetCore.Authorization;

namespace GameCore.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationService _notificationService;
        private readonly ILogger<NotificationController> _logger;

        public NotificationController(INotificationService notificationService, ILogger<NotificationController> logger)
        {
            _notificationService = notificationService;
            _logger = logger;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetNotifications([FromQuery] int page = 1, [FromQuery] int pageSize = 20)
        {
            try
            {
                var userId = GetCurrentUserId();
                var notifications = await _notificationService.GetUserNotificationsAsync(userId, page, pageSize);
                return Ok(new { Success = true, Data = notifications });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting notifications");
                return StatusCode(500, new { Success = false, Message = "取得通知清單失敗" });
            }
        }

        [HttpGet("unread")]
        [Authorize]
        public async Task<IActionResult> GetUnreadNotifications()
        {
            try
            {
                var userId = GetCurrentUserId();
                var notifications = await _notificationService.GetUnreadNotificationsAsync(userId);
                return Ok(new { Success = true, Data = notifications });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting unread notifications");
                return StatusCode(500, new { Success = false, Message = "取得未讀通知失敗" });
            }
        }

        [HttpGet("unread-count")]
        [Authorize]
        public async Task<IActionResult> GetUnreadCount()
        {
            try
            {
                var userId = GetCurrentUserId();
                var count = await _notificationService.GetUnreadCountAsync(userId);
                return Ok(new { Success = true, Data = new { UnreadCount = count } });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting unread count");
                return StatusCode(500, new { Success = false, Message = "取得未讀數量失敗" });
            }
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetNotification(int id)
        {
            try
            {
                var userId = GetCurrentUserId();
                var notification = await _notificationService.GetNotificationByIdAsync(id);
                if (notification == null || notification.UserId != userId)
                {
                    return NotFound(new { Success = false, Message = "通知不存在" });
                }
                return Ok(new { Success = true, Data = notification });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting notification {NotificationId}", id);
                return StatusCode(500, new { Success = false, Message = "取得通知資訊失敗" });
            }
        }

        [HttpPost("{id}/read")]
        [Authorize]
        public async Task<IActionResult> MarkAsRead(int id)
        {
            try
            {
                var userId = GetCurrentUserId();
                await _notificationService.MarkAsReadAsync(id, userId);
                return Ok(new { Success = true, Message = "通知標記為已讀" });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { Success = false, Message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error marking notification as read {NotificationId}", id);
                return StatusCode(500, new { Success = false, Message = "標記通知失敗" });
            }
        }

        [HttpPost("mark-all-read")]
        [Authorize]
        public async Task<IActionResult> MarkAllAsRead()
        {
            try
            {
                var userId = GetCurrentUserId();
                await _notificationService.MarkAllAsReadAsync(userId);
                return Ok(new { Success = true, Message = "所有通知標記為已讀" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error marking all notifications as read");
                return StatusCode(500, new { Success = false, Message = "標記所有通知失敗" });
            }
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteNotification(int id)
        {
            try
            {
                var userId = GetCurrentUserId();
                await _notificationService.DeleteNotificationAsync(id, userId);
                return Ok(new { Success = true, Message = "通知刪除成功" });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { Success = false, Message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting notification {NotificationId}", id);
                return StatusCode(500, new { Success = false, Message = "刪除通知失敗" });
            }
        }

        [HttpGet("by-type")]
        [Authorize]
        public async Task<IActionResult> GetNotificationsByType([FromQuery] string type, [FromQuery] int page = 1, [FromQuery] int pageSize = 20)
        {
            try
            {
                var userId = GetCurrentUserId();
                var notifications = await _notificationService.GetNotificationsByTypeAsync(userId, type, page, pageSize);
                return Ok(new { Success = true, Data = notifications });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting notifications by type {Type}", type);
                return StatusCode(500, new { Success = false, Message = "取得分類通知失敗" });
            }
        }

        [HttpGet("by-source")]
        [Authorize]
        public async Task<IActionResult> GetNotificationsBySource([FromQuery] int sourceId, [FromQuery] string sourceType)
        {
            try
            {
                var userId = GetCurrentUserId();
                var notifications = await _notificationService.GetNotificationsBySourceAsync(userId, sourceId, sourceType);
                return Ok(new { Success = true, Data = notifications });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting notifications by source {SourceId} {SourceType}", sourceId, sourceType);
                return StatusCode(500, new { Success = false, Message = "取得來源通知失敗" });
            }
        }

        [HttpGet("system")]
        public async Task<IActionResult> GetSystemNotifications([FromQuery] int page = 1, [FromQuery] int pageSize = 20)
        {
            try
            {
                var notifications = await _notificationService.GetSystemNotificationsAsync(page, pageSize);
                return Ok(new { Success = true, Data = notifications });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting system notifications");
                return StatusCode(500, new { Success = false, Message = "取得系統通知失敗" });
            }
        }

        [HttpGet("counts-by-type")]
        [Authorize]
        public async Task<IActionResult> GetNotificationCountsByType()
        {
            try
            {
                var userId = GetCurrentUserId();
                var counts = await _notificationService.GetNotificationCountsByTypeAsync(userId);
                return Ok(new { Success = true, Data = counts });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting notification counts by type");
                return StatusCode(500, new { Success = false, Message = "取得通知分類統計失敗" });
            }
        }

        [HttpGet("recent")]
        [Authorize]
        public async Task<IActionResult> GetRecentNotifications([FromQuery] int hours = 24)
        {
            try
            {
                var userId = GetCurrentUserId();
                var timeSpan = TimeSpan.FromHours(hours);
                var notifications = await _notificationService.GetRecentNotificationsAsync(userId, timeSpan);
                return Ok(new { Success = true, Data = notifications });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting recent notifications");
                return StatusCode(500, new { Success = false, Message = "取得最近通知失敗" });
            }
        }

        [HttpPost("send")]
        [Authorize(Roles = "Admin,Moderator")]
        public async Task<IActionResult> SendNotification([FromBody] SendNotificationRequest request)
        {
            try
            {
                var notification = await _notificationService.SendNotificationAsync(
                    request.UserId, 
                    request.Type, 
                    request.Title, 
                    request.Message, 
                    request.SourceId, 
                    request.SourceType);
                return Ok(new { Success = true, Data = notification, Message = "通知發送成功" });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { Success = false, Message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending notification");
                return StatusCode(500, new { Success = false, Message = "發送通知失敗" });
            }
        }

        [HttpPost("send-bulk")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> SendBulkNotification([FromBody] SendBulkNotificationRequest request)
        {
            try
            {
                var count = await _notificationService.SendBulkNotificationAsync(
                    request.UserIds, 
                    request.Type, 
                    request.Title, 
                    request.Message, 
                    request.SourceId, 
                    request.SourceType);
                return Ok(new { Success = true, Data = new { SentCount = count }, Message = $"成功發送 {count} 個通知" });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { Success = false, Message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending bulk notification");
                return StatusCode(500, new { Success = false, Message = "發送批量通知失敗" });
            }
        }

        [HttpPost("cleanup")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CleanupOldNotifications([FromQuery] int days = 30)
        {
            try
            {
                var count = await _notificationService.CleanupOldNotificationsAsync(days);
                return Ok(new { Success = true, Data = new { CleanedCount = count }, Message = $"成功清理 {count} 個舊通知" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error cleaning up old notifications");
                return StatusCode(500, new { Success = false, Message = "清理舊通知失敗" });
            }
        }

        private int GetCurrentUserId()
        {
            var userIdClaim = User.FindFirst("UserId")?.Value;
            return int.TryParse(userIdClaim, out var userId) ? userId : 0;
        }
    }

    public class SendNotificationRequest
    {
        public int UserId { get; set; }
        public string Type { get; set; } = "";
        public string Title { get; set; } = "";
        public string Message { get; set; } = "";
        public int? SourceId { get; set; }
        public string SourceType { get; set; } = "";
    }

    public class SendBulkNotificationRequest
    {
        public int[] UserIds { get; set; } = Array.Empty<int>();
        public string Type { get; set; } = "";
        public string Title { get; set; } = "";
        public string Message { get; set; } = "";
        public int? SourceId { get; set; }
        public string SourceType { get; set; } = "";
    }
}