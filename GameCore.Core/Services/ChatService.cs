using GameCore.Core.Entities;
using GameCore.Core.Interfaces;
using GameCore.Core.Services;
using GameCore.Core.DTOs;
using Microsoft.Extensions.Logging;

namespace GameCore.Core.Services
{
    public class ChatService : IChatService
    {
        private readonly IChatRepository _chatRepository;
        private readonly IChatMessageRepository _chatMessageRepository;
        private readonly IPrivateChatRepository _privateChatRepository;
        private readonly IPrivateMessageRepository _privateMessageRepository;
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<ChatService> _logger;

        public ChatService(
            IChatRepository chatRepository,
            IChatMessageRepository chatMessageRepository,
            IPrivateChatRepository privateChatRepository,
            IPrivateMessageRepository privateMessageRepository,
            IUserRepository userRepository,
            IUnitOfWork unitOfWork,
            ILogger<ChatService> logger)
        {
            _chatRepository = chatRepository;
            _chatMessageRepository = chatMessageRepository;
            _privateChatRepository = privateChatRepository;
            _privateMessageRepository = privateMessageRepository;
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<IEnumerable<ChatRoomDto>> GetUserChatRoomsAsync(int userId)
        {
            try
            {
                var chatRooms = await _chatRepository.GetChatRoomsByUserIdAsync(userId);
                return chatRooms.Select(cr => new ChatRoomDto
                {
                    Id = cr.Id,
                    Name = cr.Name,
                    Description = cr.Description,
                    Type = cr.Type,
                    CreatedBy = cr.CreatedBy,
                    CreatedAt = cr.CreatedAt,
                    IsActive = cr.IsActive,
                    MemberCount = cr.Members?.Count ?? 0
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "獲取用戶聊天室失敗: {UserId}", userId);
                return Enumerable.Empty<ChatRoomDto>();
            }
        }

        public async Task<ChatRoomDto> GetChatRoomByIdAsync(int chatRoomId, int userId)
        {
            try
            {
                var chatRoom = await _chatRepository.GetByIdAsync(chatRoomId);
                if (chatRoom == null) return null;

                // 檢查用戶是否為聊天室成員
                var isMember = chatRoom.Members?.Any(m => m.UserId == userId) ?? false;
                if (!isMember)
                {
                    return null;
                }

                return new ChatRoomDto
                {
                    Id = chatRoom.Id,
                    Name = chatRoom.Name,
                    Description = chatRoom.Description,
                    Type = chatRoom.Type,
                    CreatedBy = chatRoom.CreatedBy,
                    CreatedAt = chatRoom.CreatedAt,
                    IsActive = chatRoom.IsActive,
                    MemberCount = chatRoom.Members?.Count ?? 0
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "獲取聊天室失敗: {ChatRoomId}", chatRoomId);
                return null;
            }
        }

        public async Task<ChatRoomCreateResult> CreateChatRoomAsync(ChatRoomCreateDto createDto)
        {
            try
            {
                var chatRoom = new ChatRoom
                {
                    Name = createDto.Name,
                    Description = createDto.Description,
                    Type = createDto.Type,
                    CreatedBy = createDto.CreatedBy,
                    CreatedAt = DateTime.UtcNow,
                    IsActive = true
                };

                _chatRepository.Add(chatRoom);
                await _unitOfWork.SaveChangesAsync();

                // 創建者自動成為成員
                var member = new ChatRoomMember
                {
                    ChatRoomId = chatRoom.Id,
                    UserId = createDto.CreatedBy,
                    Role = ChatMemberRole.Admin,
                    JoinedAt = DateTime.UtcNow
                };

                _chatRepository.AddMember(member);
                await _unitOfWork.SaveChangesAsync();

                _logger.LogInformation("聊天室創建成功: {ChatRoomId}, 創建者: {CreatedBy}", chatRoom.Id, createDto.CreatedBy);

                return new ChatRoomCreateResult
                {
                    Success = true,
                    Message = "聊天室創建成功",
                    ChatRoomId = chatRoom.Id
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "創建聊天室失敗: 創建者 {CreatedBy}", createDto.CreatedBy);
                return new ChatRoomCreateResult
                {
                    Success = false,
                    Message = "創建失敗"
                };
            }
        }

        public async Task<ChatRoomUpdateResult> UpdateChatRoomAsync(int chatRoomId, int userId, ChatRoomUpdateDto updateDto)
        {
            try
            {
                var chatRoom = await _chatRepository.GetByIdAsync(chatRoomId);
                if (chatRoom == null)
                {
                    return new ChatRoomUpdateResult
                    {
                        Success = false,
                        Message = "聊天室不存在"
                    };
                }

                // 檢查權限
                var member = chatRoom.Members?.FirstOrDefault(m => m.UserId == userId);
                if (member == null || member.Role != ChatMemberRole.Admin)
                {
                    return new ChatRoomUpdateResult
                    {
                        Success = false,
                        Message = "無權限修改此聊天室"
                    };
                }

                chatRoom.Name = updateDto.Name;
                chatRoom.Description = updateDto.Description;

                _chatRepository.Update(chatRoom);
                await _unitOfWork.SaveChangesAsync();

                _logger.LogInformation("聊天室更新成功: {ChatRoomId}", chatRoomId);

                return new ChatRoomUpdateResult
                {
                    Success = true,
                    Message = "聊天室更新成功"
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "更新聊天室失敗: {ChatRoomId}", chatRoomId);
                return new ChatRoomUpdateResult
                {
                    Success = false,
                    Message = "更新失敗"
                };
            }
        }

        public async Task<bool> AddMemberToChatRoomAsync(int chatRoomId, int userId, int targetUserId)
        {
            try
            {
                var chatRoom = await _chatRepository.GetByIdAsync(chatRoomId);
                if (chatRoom == null) return false;

                // 檢查權限
                var member = chatRoom.Members?.FirstOrDefault(m => m.UserId == userId);
                if (member == null || member.Role != ChatMemberRole.Admin)
                {
                    return false;
                }

                // 檢查目標用戶是否已經是成員
                var existingMember = chatRoom.Members?.FirstOrDefault(m => m.UserId == targetUserId);
                if (existingMember != null)
                {
                    return false;
                }

                var newMember = new ChatRoomMember
                {
                    ChatRoomId = chatRoomId,
                    UserId = targetUserId,
                    Role = ChatMemberRole.Member,
                    JoinedAt = DateTime.UtcNow
                };

                _chatRepository.AddMember(newMember);
                await _unitOfWork.SaveChangesAsync();

                _logger.LogInformation("用戶加入聊天室成功: 聊天室 {ChatRoomId}, 用戶 {TargetUserId}", chatRoomId, targetUserId);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "添加聊天室成員失敗: 聊天室 {ChatRoomId}, 用戶 {TargetUserId}", chatRoomId, targetUserId);
                return false;
            }
        }

        public async Task<bool> RemoveMemberFromChatRoomAsync(int chatRoomId, int userId, int targetUserId)
        {
            try
            {
                var chatRoom = await _chatRepository.GetByIdAsync(chatRoomId);
                if (chatRoom == null) return false;

                // 檢查權限
                var member = chatRoom.Members?.FirstOrDefault(m => m.UserId == userId);
                if (member == null || member.Role != ChatMemberRole.Admin)
                {
                    return false;
                }

                // 不能移除自己
                if (userId == targetUserId)
                {
                    return false;
                }

                var targetMember = chatRoom.Members?.FirstOrDefault(m => m.UserId == targetUserId);
                if (targetMember == null)
                {
                    return false;
                }

                _chatRepository.RemoveMember(targetMember);
                await _unitOfWork.SaveChangesAsync();

                _logger.LogInformation("用戶離開聊天室成功: 聊天室 {ChatRoomId}, 用戶 {TargetUserId}", chatRoomId, targetUserId);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "移除聊天室成員失敗: 聊天室 {ChatRoomId}, 用戶 {TargetUserId}", chatRoomId, targetUserId);
                return false;
            }
        }

        public async Task<IEnumerable<ChatMessageDto>> GetChatRoomMessagesAsync(int chatRoomId, int userId, int page = 1, int pageSize = 50)
        {
            try
            {
                // 檢查用戶是否為聊天室成員
                var chatRoom = await _chatRepository.GetByIdAsync(chatRoomId);
                if (chatRoom == null) return Enumerable.Empty<ChatMessageDto>();

                var isMember = chatRoom.Members?.Any(m => m.UserId == userId) ?? false;
                if (!isMember)
                {
                    return Enumerable.Empty<ChatMessageDto>();
                }

                var messages = await _chatMessageRepository.GetByChatRoomIdAsync(chatRoomId, page, pageSize);
                return messages.Select(m => new ChatMessageDto
                {
                    Id = m.Id,
                    ChatRoomId = m.ChatRoomId,
                    SenderId = m.SenderId,
                    SenderName = m.Sender?.Username ?? "未知用戶",
                    Content = m.Content,
                    Type = m.Type,
                    CreatedAt = m.CreatedAt
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "獲取聊天室消息失敗: 聊天室 {ChatRoomId}", chatRoomId);
                return Enumerable.Empty<ChatMessageDto>();
            }
        }

        public async Task<ChatMessageCreateResult> SendChatMessageAsync(ChatMessageCreateDto createDto)
        {
            try
            {
                // 檢查用戶是否為聊天室成員
                var chatRoom = await _chatRepository.GetByIdAsync(createDto.ChatRoomId);
                if (chatRoom == null)
                {
                    return new ChatMessageCreateResult
                    {
                        Success = false,
                        Message = "聊天室不存在"
                    };
                }

                var isMember = chatRoom.Members?.Any(m => m.UserId == createDto.SenderId) ?? false;
                if (!isMember)
                {
                    return new ChatMessageCreateResult
                    {
                        Success = false,
                        Message = "您不是此聊天室的成員"
                    };
                }

                var message = new ChatMessage
                {
                    ChatRoomId = createDto.ChatRoomId,
                    SenderId = createDto.SenderId,
                    Content = createDto.Content,
                    Type = createDto.Type,
                    CreatedAt = DateTime.UtcNow
                };

                _chatMessageRepository.Add(message);
                await _unitOfWork.SaveChangesAsync();

                _logger.LogInformation("聊天消息發送成功: 聊天室 {ChatRoomId}, 發送者: {SenderId}", 
                    createDto.ChatRoomId, createDto.SenderId);

                return new ChatMessageCreateResult
                {
                    Success = true,
                    Message = "消息發送成功",
                    MessageId = message.Id
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "發送聊天消息失敗: 聊天室 {ChatRoomId}, 發送者: {SenderId}", 
                    createDto.ChatRoomId, createDto.SenderId);
                return new ChatMessageCreateResult
                {
                    Success = false,
                    Message = "發送失敗"
                };
            }
        }

        public async Task<IEnumerable<PrivateChatDto>> GetUserPrivateChatsAsync(int userId)
        {
            try
            {
                var privateChats = await _privateChatRepository.GetByUserIdAsync(userId);
                return privateChats.Select(pc => new PrivateChatDto
                {
                    Id = pc.Id,
                    User1Id = pc.User1Id,
                    User1Name = pc.User1?.Username ?? "未知用戶",
                    User2Id = pc.User2Id,
                    User2Name = pc.User2?.Username ?? "未知用戶",
                    CreatedAt = pc.CreatedAt,
                    LastMessageAt = pc.LastMessageAt
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "獲取用戶私聊列表失敗: {UserId}", userId);
                return Enumerable.Empty<PrivateChatDto>();
            }
        }

        public async Task<PrivateChatDto> GetPrivateChatAsync(int user1Id, int user2Id)
        {
            try
            {
                var privateChat = await _privateChatRepository.GetByUsersAsync(user1Id, user2Id);
                if (privateChat == null) return null;

                return new PrivateChatDto
                {
                    Id = privateChat.Id,
                    User1Id = privateChat.User1Id,
                    User1Name = privateChat.User1?.Username ?? "未知用戶",
                    User2Id = privateChat.User2Id,
                    User2Name = privateChat.User2?.Username ?? "未知用戶",
                    CreatedAt = privateChat.CreatedAt,
                    LastMessageAt = privateChat.LastMessageAt
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "獲取私聊失敗: 用戶1 {User1Id}, 用戶2 {User2Id}", user1Id, user2Id);
                return null;
            }
        }

        public async Task<PrivateChatCreateResult> CreatePrivateChatAsync(int user1Id, int user2Id)
        {
            try
            {
                // 檢查是否已存在私聊
                var existingChat = await _privateChatRepository.GetByUsersAsync(user1Id, user2Id);
                if (existingChat != null)
                {
                    return new PrivateChatCreateResult
                    {
                        Success = true,
                        Message = "私聊已存在",
                        PrivateChatId = existingChat.Id
                    };
                }

                var privateChat = new PrivateChat
                {
                    User1Id = user1Id,
                    User2Id = user2Id,
                    CreatedAt = DateTime.UtcNow
                };

                _privateChatRepository.Add(privateChat);
                await _unitOfWork.SaveChangesAsync();

                _logger.LogInformation("私聊創建成功: {PrivateChatId}, 用戶1: {User1Id}, 用戶2: {User2Id}", 
                    privateChat.Id, user1Id, user2Id);

                return new PrivateChatCreateResult
                {
                    Success = true,
                    Message = "私聊創建成功",
                    PrivateChatId = privateChat.Id
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "創建私聊失敗: 用戶1 {User1Id}, 用戶2 {User2Id}", user1Id, user2Id);
                return new PrivateChatCreateResult
                {
                    Success = false,
                    Message = "創建失敗"
                };
            }
        }

        public async Task<IEnumerable<PrivateMessageDto>> GetPrivateMessagesAsync(int privateChatId, int userId, int page = 1, int pageSize = 50)
        {
            try
            {
                // 檢查用戶是否為私聊參與者
                var privateChat = await _privateChatRepository.GetByIdAsync(privateChatId);
                if (privateChat == null) return Enumerable.Empty<PrivateMessageDto>();

                if (privateChat.User1Id != userId && privateChat.User2Id != userId)
                {
                    return Enumerable.Empty<PrivateMessageDto>();
                }

                var messages = await _privateMessageRepository.GetByPrivateChatIdAsync(privateChatId, page, pageSize);
                return messages.Select(m => new PrivateMessageDto
                {
                    Id = m.Id,
                    PrivateChatId = m.PrivateChatId,
                    SenderId = m.SenderId,
                    SenderName = m.Sender?.Username ?? "未知用戶",
                    Content = m.Content,
                    Type = m.Type,
                    IsRead = m.IsRead,
                    CreatedAt = m.CreatedAt,
                    ReadAt = m.ReadAt
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "獲取私聊消息失敗: 私聊 {PrivateChatId}", privateChatId);
                return Enumerable.Empty<PrivateMessageDto>();
            }
        }

        public async Task<PrivateMessageCreateResult> SendPrivateMessageAsync(PrivateMessageCreateDto createDto)
        {
            try
            {
                // 檢查用戶是否為私聊參與者
                var privateChat = await _privateChatRepository.GetByIdAsync(createDto.PrivateChatId);
                if (privateChat == null)
                {
                    return new PrivateMessageCreateResult
                    {
                        Success = false,
                        Message = "私聊不存在"
                    };
                }

                if (privateChat.User1Id != createDto.SenderId && privateChat.User2Id != createDto.SenderId)
                {
                    return new PrivateMessageCreateResult
                    {
                        Success = false,
                        Message = "您不是此私聊的參與者"
                    };
                }

                var message = new PrivateMessage
                {
                    PrivateChatId = createDto.PrivateChatId,
                    SenderId = createDto.SenderId,
                    Content = createDto.Content,
                    Type = createDto.Type,
                    IsRead = false,
                    CreatedAt = DateTime.UtcNow
                };

                _privateMessageRepository.Add(message);

                // 更新私聊的最後消息時間
                privateChat.LastMessageAt = DateTime.UtcNow;
                _privateChatRepository.Update(privateChat);

                await _unitOfWork.SaveChangesAsync();

                _logger.LogInformation("私聊消息發送成功: 私聊 {PrivateChatId}, 發送者: {SenderId}", 
                    createDto.PrivateChatId, createDto.SenderId);

                return new PrivateMessageCreateResult
                {
                    Success = true,
                    Message = "消息發送成功",
                    MessageId = message.Id
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "發送私聊消息失敗: 私聊 {PrivateChatId}, 發送者: {SenderId}", 
                    createDto.PrivateChatId, createDto.SenderId);
                return new PrivateMessageCreateResult
                {
                    Success = false,
                    Message = "發送失敗"
                };
            }
        }

        public async Task<bool> MarkPrivateMessageAsReadAsync(int messageId, int userId)
        {
            try
            {
                var message = await _privateMessageRepository.GetByIdAsync(messageId);
                if (message == null) return false;

                // 檢查用戶是否為消息接收者
                var privateChat = await _privateChatRepository.GetByIdAsync(message.PrivateChatId);
                if (privateChat == null) return false;

                if (privateChat.User1Id != userId && privateChat.User2Id != userId)
                {
                    return false;
                }

                if (message.SenderId == userId)
                {
                    return false; // 發送者不能標記自己的消息為已讀
                }

                message.IsRead = true;
                message.ReadAt = DateTime.UtcNow;

                _privateMessageRepository.Update(message);
                await _unitOfWork.SaveChangesAsync();

                _logger.LogInformation("私聊消息標記為已讀: {MessageId}, 用戶: {UserId}", messageId, userId);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "標記私聊消息為已讀失敗: {MessageId}", messageId);
                return false;
            }
        }

        public async Task<int> GetUnreadMessageCountAsync(int userId)
        {
            try
            {
                return await _privateMessageRepository.GetUnreadCountAsync(userId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "獲取未讀消息數量失敗: {UserId}", userId);
                return 0;
            }
        }

        public async Task<bool> DeletePrivateMessageAsync(int messageId, int userId)
        {
            try
            {
                var message = await _privateMessageRepository.GetByIdAsync(messageId);
                if (message == null) return false;

                if (message.SenderId != userId)
                {
                    return false; // 只能刪除自己的消息
                }

                _privateMessageRepository.Delete(message);
                await _unitOfWork.SaveChangesAsync();

                _logger.LogInformation("私聊消息刪除成功: {MessageId}, 用戶: {UserId}", messageId, userId);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "刪除私聊消息失敗: {MessageId}", messageId);
                return false;
            }
        }

        public async Task<bool> LeaveChatRoomAsync(int chatRoomId, int userId)
        {
            try
            {
                var chatRoom = await _chatRepository.GetByIdAsync(chatRoomId);
                if (chatRoom == null) return false;

                var member = chatRoom.Members?.FirstOrDefault(m => m.UserId == userId);
                if (member == null)
                {
                    return false;
                }

                // 如果是聊天室創建者，不能離開
                if (chatRoom.CreatedBy == userId)
                {
                    return false;
                }

                _chatRepository.RemoveMember(member);
                await _unitOfWork.SaveChangesAsync();

                _logger.LogInformation("用戶離開聊天室成功: 聊天室 {ChatRoomId}, 用戶 {UserId}", chatRoomId, userId);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "離開聊天室失敗: 聊天室 {ChatRoomId}, 用戶 {UserId}", chatRoomId, userId);
                return false;
            }
        }
    }
}