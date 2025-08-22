using GameCore.Core.DTOs;
using GameCore.Core.Entities;
using GameCore.Core.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GameCore.Core.Services
{
    /// <summary>
    /// 聊天服務實現類
    /// 提供聊天相關的業務邏輯處理
    /// </summary>
    public class ChatService : IChatService
    {
        private readonly IChatRepository _chatRepository;
        private readonly IUserRepository _userRepository;
        private readonly INotificationService _notificationService;
        private readonly ILogger<ChatService> _logger;

        public ChatService(
            IChatRepository chatRepository,
            IUserRepository userRepository,
            INotificationService notificationService,
            ILogger<ChatService> logger)
        {
            _chatRepository = chatRepository;
            _userRepository = userRepository;
            _notificationService = notificationService;
            _logger = logger;
        }

        /// <summary>
        /// 獲取聊天歷史記錄
        /// </summary>
        /// <param name="userId1">用戶1 ID</param>
        /// <param name="userId2">用戶2 ID</param>
        /// <param name="page">頁碼</param>
        /// <param name="pageSize">每頁大小</param>
        /// <returns>聊天記錄</returns>
        public async Task<PagedResult<ChatMessage>> GetChatHistoryAsync(int userId1, int userId2, int page = 1, int pageSize = 50)
        {
            try
            {
                _logger.LogInformation("獲取聊天歷史記錄，用戶1: {UserId1}, 用戶2: {UserId2}, 頁碼: {Page}, 每頁大小: {PageSize}", 
                    userId1, userId2, page, pageSize);
                
                var messages = await _chatRepository.GetChatHistoryAsync(userId1, userId2, page, pageSize);
                
                _logger.LogInformation("成功獲取聊天歷史記錄，用戶1: {UserId1}, 用戶2: {UserId2}, 共 {TotalCount} 條消息", 
                    userId1, userId2, messages.TotalCount);
                return messages;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "獲取聊天歷史記錄時發生錯誤，用戶1: {UserId1}, 用戶2: {UserId2}", userId1, userId2);
                throw;
            }
        }

        /// <summary>
        /// 發送私聊消息
        /// </summary>
        /// <param name="senderId">發送者ID</param>
        /// <param name="receiverId">接收者ID</param>
        /// <param name="message">消息內容</param>
        /// <param name="messageType">消息類型</param>
        /// <returns>發送結果</returns>
        public async Task<bool> SendPrivateMessageAsync(int senderId, int receiverId, string message, string messageType = "text")
        {
            try
            {
                _logger.LogInformation("發送私聊消息，發送者: {SenderId}, 接收者: {ReceiverId}, 消息類型: {MessageType}", 
                    senderId, receiverId, messageType);
                
                // 檢查發送者和接收者是否存在
                var sender = await _userRepository.GetByIdAsync(senderId);
                var receiver = await _userRepository.GetByIdAsync(receiverId);
                
                if (sender == null || receiver == null)
                {
                    _logger.LogWarning("發送者或接收者不存在，發送者: {SenderId}, 接收者: {ReceiverId}", senderId, receiverId);
                    return false;
                }
                
                // 檢查是否發送給自己
                if (senderId == receiverId)
                {
                    _logger.LogWarning("不能發送消息給自己，用戶ID: {UserId}", senderId);
                    return false;
                }
                
                var result = await _chatRepository.SendPrivateMessageAsync(senderId, receiverId, message, messageType);
                
                if (result)
                {
                    // 發送通知給接收者
                    await _notificationService.CreateNotificationAsync(
                        receiverId,
                        "新私聊消息",
                        $"您收到來自 {sender.Username} 的新消息",
                        "chat_message"
                    );
                    
                    _logger.LogInformation("成功發送私聊消息，發送者: {SenderId}, 接收者: {ReceiverId}", senderId, receiverId);
                }
                else
                {
                    _logger.LogWarning("發送私聊消息失敗，發送者: {SenderId}, 接收者: {ReceiverId}", senderId, receiverId);
                }
                
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "發送私聊消息時發生錯誤，發送者: {SenderId}, 接收者: {ReceiverId}", senderId, receiverId);
                throw;
            }
        }

        /// <summary>
        /// 獲取用戶的私聊列表
        /// </summary>
        /// <param name="userId">用戶ID</param>
        /// <returns>私聊列表</returns>
        public async Task<List<ChatContact>> GetPrivateChatsAsync(int userId)
        {
            try
            {
                _logger.LogInformation("獲取用戶私聊列表，用戶ID: {UserId}", userId);
                
                var contacts = await _chatRepository.GetPrivateMessagesAsync(userId);
                
                _logger.LogInformation("成功獲取用戶私聊列表，用戶ID: {UserId}, 共 {Count} 個聯繫人", userId, contacts.Count);
                return contacts;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "獲取用戶私聊列表時發生錯誤，用戶ID: {UserId}", userId);
                throw;
            }
        }

        /// <summary>
        /// 標記消息為已讀
        /// </summary>
        /// <param name="messageId">消息ID</param>
        /// <param name="userId">用戶ID</param>
        /// <returns>更新結果</returns>
        public async Task<bool> MarkMessageAsReadAsync(int messageId, int userId)
        {
            try
            {
                _logger.LogInformation("標記消息為已讀，消息ID: {MessageId}, 用戶ID: {UserId}", messageId, userId);
                
                var result = await _chatRepository.MarkMessagesAsReadAsync(messageId, userId);
                
                if (result)
                {
                    _logger.LogInformation("成功標記消息為已讀，消息ID: {MessageId}, 用戶ID: {UserId}", messageId, userId);
                }
                else
                {
                    _logger.LogWarning("標記消息為已讀失敗，消息ID: {MessageId}, 用戶ID: {UserId}", messageId, userId);
                }
                
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "標記消息為已讀時發生錯誤，消息ID: {MessageId}, 用戶ID: {UserId}", messageId, userId);
                throw;
            }
        }

        /// <summary>
        /// 獲取未讀消息數量
        /// </summary>
        /// <param name="userId">用戶ID</param>
        /// <returns>未讀消息數量</returns>
        public async Task<int> GetUnreadMessageCountAsync(int userId)
        {
            try
            {
                _logger.LogInformation("獲取未讀消息數量，用戶ID: {UserId}", userId);
                
                var count = await _chatRepository.GetUnreadCountAsync(userId);
                
                _logger.LogInformation("成功獲取未讀消息數量，用戶ID: {UserId}, 數量: {Count}", userId, count);
                return count;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "獲取未讀消息數量時發生錯誤，用戶ID: {UserId}", userId);
                throw;
            }
        }

        /// <summary>
        /// 刪除聊天記錄
        /// </summary>
        /// <param name="messageId">消息ID</param>
        /// <param name="userId">用戶ID</param>
        /// <returns>刪除結果</returns>
        public async Task<bool> DeleteMessageAsync(int messageId, int userId)
        {
            try
            {
                _logger.LogInformation("刪除聊天記錄，消息ID: {MessageId}, 用戶ID: {UserId}", messageId, userId);
                
                var result = await _chatRepository.DeleteMessageAsync(messageId, userId);
                
                if (result)
                {
                    _logger.LogInformation("成功刪除聊天記錄，消息ID: {MessageId}, 用戶ID: {UserId}", messageId, userId);
                }
                else
                {
                    _logger.LogWarning("刪除聊天記錄失敗，消息ID: {MessageId}, 用戶ID: {UserId}", messageId, userId);
                }
                
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "刪除聊天記錄時發生錯誤，消息ID: {MessageId}, 用戶ID: {UserId}", messageId, userId);
                throw;
            }
        }

        /// <summary>
        /// 獲取聊天統計資料
        /// </summary>
        /// <param name="userId">用戶ID</param>
        /// <returns>聊天統計資料</returns>
        public async Task<ChatStats> GetChatStatsAsync(int userId)
        {
            try
            {
                _logger.LogInformation("獲取聊天統計資料，用戶ID: {UserId}", userId);
                
                var unreadCount = await _chatRepository.GetUnreadCountAsync(userId);
                var totalMessages = await _chatRepository.GetTotalMessageCountAsync(userId);
                var chatContacts = await _chatRepository.GetPrivateMessagesAsync(userId);
                
                var stats = new ChatStats
                {
                    UserId = userId,
                    UnreadMessageCount = unreadCount,
                    TotalMessageCount = totalMessages,
                    ChatContactCount = chatContacts.Count
                };
                
                _logger.LogInformation("成功獲取聊天統計資料，用戶ID: {UserId}, 未讀消息: {UnreadCount}, 總消息: {TotalCount}, 聯繫人: {ContactCount}", 
                    userId, unreadCount, totalMessages, chatContacts.Count);
                return stats;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "獲取聊天統計資料時發生錯誤，用戶ID: {UserId}", userId);
                throw;
            }
        }

        /// <summary>
        /// 清理過期聊天記錄
        /// </summary>
        /// <param name="daysToKeep">保留天數</param>
        /// <returns>清理結果</returns>
        public async Task<bool> CleanupExpiredMessagesAsync(int daysToKeep = 90)
        {
            try
            {
                _logger.LogInformation("清理過期聊天記錄，保留天數: {DaysToKeep}", daysToKeep);
                
                var result = await _chatRepository.CleanupExpiredMessagesAsync(daysToKeep);
                
                if (result)
                {
                    _logger.LogInformation("成功清理過期聊天記錄，保留天數: {DaysToKeep}", daysToKeep);
                }
                else
                {
                    _logger.LogWarning("清理過期聊天記錄失敗，保留天數: {DaysToKeep}", daysToKeep);
                }
                
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "清理過期聊天記錄時發生錯誤，保留天數: {DaysToKeep}", daysToKeep);
                throw;
            }
        }

        /// <summary>
        /// 檢查用戶是否在線
        /// </summary>
        /// <param name="userId">用戶ID</param>
        /// <returns>是否在線</returns>
        public async Task<bool> IsUserOnlineAsync(int userId)
        {
            try
            {
                _logger.LogInformation("檢查用戶在線狀態，用戶ID: {UserId}", userId);
                
                // 這裡可以實現檢查用戶在線狀態的邏輯
                // 例如：檢查用戶最後活動時間、SignalR 連接狀態等
                var user = await _userRepository.GetByIdAsync(userId);
                if (user == null)
                {
                    _logger.LogWarning("用戶不存在，無法檢查在線狀態，用戶ID: {UserId}", userId);
                    return false;
                }
                
                // 簡單的檢查邏輯：如果最後登入時間在5分鐘內，認為在線
                var isOnline = user.LastLoginAt.HasValue && 
                              (DateTime.UtcNow - user.LastLoginAt.Value).TotalMinutes <= 5;
                
                _logger.LogInformation("用戶 {UserId} 在線狀態檢查結果: {IsOnline}", userId, isOnline);
                return isOnline;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "檢查用戶在線狀態時發生錯誤，用戶ID: {UserId}", userId);
                throw;
            }
        }
    }

    /// <summary>
    /// 聊天聯繫人 DTO
    /// </summary>
    public class ChatContact
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public string Avatar { get; set; }
        public bool IsOnline { get; set; }
        public DateTime LastMessageTime { get; set; }
        public string LastMessage { get; set; }
        public int UnreadCount { get; set; }
    }

    /// <summary>
    /// 聊天消息 DTO
    /// </summary>
    public class ChatMessage
    {
        public int MessageId { get; set; }
        public int SenderId { get; set; }
        public int ReceiverId { get; set; }
        public string Message { get; set; }
        public string MessageType { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsRead { get; set; }
        public string SenderUsername { get; set; }
        public string ReceiverUsername { get; set; }
    }

    /// <summary>
    /// 聊天統計資料 DTO
    /// </summary>
    public class ChatStats
    {
        public int UserId { get; set; }
        public int UnreadMessageCount { get; set; }
        public int TotalMessageCount { get; set; }
        public int ChatContactCount { get; set; }
    }
}
