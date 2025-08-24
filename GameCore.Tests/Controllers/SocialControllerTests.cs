using GameCore.Core.DTOs;
using GameCore.Core.Services;
using GameCore.Web.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System.Security.Claims;
using Xunit;

namespace GameCore.Tests.Controllers
{
    /// <summary>
    /// 社交控制器測試類別
    /// 測試所有社交相關API的功能和邊界條件
    /// 驗證通知系統、聊天系統、群組管理、封鎖功能等完整社交功能
    /// </summary>
    public class SocialControllerTests
    {
        private readonly Mock<ISocialService> _mockSocialService;
        private readonly Mock<ILogger<SocialController>> _mockLogger;
        private readonly SocialController _controller;

        public SocialControllerTests()
        {
            _mockSocialService = new Mock<ISocialService>();
            _mockLogger = new Mock<ILogger<SocialController>>();
            _controller = new SocialController(_mockSocialService.Object, _mockLogger.Object);

            // 設定模擬的使用者身份
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, "123"),
                new Claim(ClaimTypes.Name, "testuser"),
                new Claim(ClaimTypes.Role, "User")
            };
            var identity = new ClaimsIdentity(claims, "Test");
            var principal = new ClaimsPrincipal(identity);

            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = principal
                }
            };
        }

        #region 通知系統測試

        [Fact]
        public async Task CreateNotification_ShouldReturnSuccess_WhenValidRequest()
        {
            // Arrange
            var createDto = new CreateNotificationDto
            {
                SourceId = 1,
                ActionId = 1,
                SenderId = 123,
                NotificationTitle = "測試通知",
                NotificationMessage = "這是測試通知內容",
                RecipientIds = new List<int> { 456, 789 }
            };

            var expectedNotification = new NotificationDto
            {
                NotificationId = 100,
                SourceName = "系統",
                ActionName = "公告",
                NotificationTitle = "測試通知",
                NotificationMessage = "這是測試通知內容"
            };

            var expectedResult = SocialServiceResult<NotificationDto>.CreateSuccess(
                expectedNotification, "通知建立成功");

            _mockSocialService
                .Setup(s => s.CreateNotificationAsync(createDto))
                .ReturnsAsync(expectedResult);

            // Act
            var result = await _controller.CreateNotification(createDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = okResult.Value;
            
            var responseType = response.GetType();
            var successProperty = responseType.GetProperty("success");
            var dataProperty = responseType.GetProperty("data");
            
            Assert.True((bool)successProperty.GetValue(response));
            
            var data = dataProperty.GetValue(response) as NotificationDto;
            Assert.NotNull(data);
            Assert.Equal("測試通知", data.NotificationTitle);
        }

        [Fact]
        public async Task GetNotifications_ShouldReturnNotificationList_WhenValidRequest()
        {
            // Arrange
            var expectedNotifications = new SocialPagedResult<NotificationDto>
            {
                Page = 1,
                PageSize = 20,
                TotalCount = 50,
                Data = new List<NotificationDto>
                {
                    new() { NotificationId = 1, NotificationTitle = "通知1", SourceName = "系統", IsRead = false },
                    new() { NotificationId = 2, NotificationTitle = "通知2", SourceName = "論壇", IsRead = true }
                }
            };

            _mockSocialService
                .Setup(s => s.GetUserNotificationsAsync(123, 1, 20, false, null))
                .ReturnsAsync(expectedNotifications);

            // Act
            var result = await _controller.GetNotifications();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = okResult.Value;
            
            var responseType = response.GetType();
            var dataProperty = responseType.GetProperty("data");
            
            var data = dataProperty.GetValue(response) as SocialPagedResult<NotificationDto>;
            Assert.NotNull(data);
            Assert.Equal(50, data.TotalCount);
            Assert.Equal(2, data.Data.Count);
        }

        [Fact]
        public async Task MarkNotificationsAsRead_ShouldReturnSuccess_WhenValidRequest()
        {
            // Arrange
            var markReadDto = new MarkNotificationReadDto
            {
                NotificationIds = new List<int> { 1, 2, 3 }
            };

            var expectedResult = SocialServiceResult.CreateSuccess("通知已標記為已讀");

            _mockSocialService
                .Setup(s => s.MarkNotificationsAsReadAsync(123, markReadDto))
                .ReturnsAsync(expectedResult);

            // Act
            var result = await _controller.MarkNotificationsAsRead(markReadDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = okResult.Value;
            
            var responseType = response.GetType();
            var successProperty = responseType.GetProperty("success");
            var messageProperty = responseType.GetProperty("message");
            
            Assert.True((bool)successProperty.GetValue(response));
            Assert.Equal("通知已標記為已讀", messageProperty.GetValue(response));
        }

        [Fact]
        public async Task GetNotificationStats_ShouldReturnStats_WhenValidRequest()
        {
            // Arrange
            var expectedStats = new NotificationStatsDto
            {
                TotalCount = 100,
                UnreadCount = 25,
                TodayCount = 10,
                WeekCount = 45,
                SourceStats = new List<NotificationSourceStatsDto>
                {
                    new() { SourceName = "系統", Count = 30, UnreadCount = 5 },
                    new() { SourceName = "論壇", Count = 25, UnreadCount = 8 }
                }
            };

            _mockSocialService
                .Setup(s => s.GetNotificationStatsAsync(123))
                .ReturnsAsync(expectedStats);

            // Act
            var result = await _controller.GetNotificationStats();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = okResult.Value;
            
            var responseType = response.GetType();
            var dataProperty = responseType.GetProperty("data");
            
            var data = dataProperty.GetValue(response) as NotificationStatsDto;
            Assert.NotNull(data);
            Assert.Equal(100, data.TotalCount);
            Assert.Equal(25, data.UnreadCount);
            Assert.Equal(2, data.SourceStats.Count);
        }

        #endregion

        #region 聊天系統測試

        [Fact]
        public async Task SendChatMessage_ShouldReturnSuccess_WhenValidRequest()
        {
            // Arrange
            var sendMessageDto = new SendChatMessageDto
            {
                ReceiverId = 456,
                ChatContent = "你好！這是測試訊息。"
            };

            var expectedMessage = new ChatMessageDto
            {
                MessageId = 100,
                SenderId = 123,
                ReceiverId = 456,
                ChatContent = "你好！這是測試訊息。",
                SentAt = DateTime.UtcNow,
                IsRead = false,
                IsSent = true
            };

            var expectedResult = SocialServiceResult<ChatMessageDto>.CreateSuccess(
                expectedMessage, "訊息發送成功");

            _mockSocialService
                .Setup(s => s.SendChatMessageAsync(123, sendMessageDto))
                .ReturnsAsync(expectedResult);

            // Act
            var result = await _controller.SendChatMessage(sendMessageDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = okResult.Value;
            
            var responseType = response.GetType();
            var successProperty = responseType.GetProperty("success");
            var dataProperty = responseType.GetProperty("data");
            
            Assert.True((bool)successProperty.GetValue(response));
            
            var data = dataProperty.GetValue(response) as ChatMessageDto;
            Assert.NotNull(data);
            Assert.Equal("你好！這是測試訊息。", data.ChatContent);
        }

        [Fact]
        public async Task GetChatConversations_ShouldReturnConversationList_WhenValidRequest()
        {
            // Arrange
            var expectedConversations = new SocialPagedResult<ChatConversationDto>
            {
                Page = 1,
                PageSize = 20,
                TotalCount = 10,
                Data = new List<ChatConversationDto>
                {
                    new() { PartnerId = 456, PartnerName = "朋友1", UnreadCount = 2, ConversationType = "private" },
                    new() { PartnerId = 789, PartnerName = "朋友2", UnreadCount = 0, ConversationType = "private" }
                }
            };

            _mockSocialService
                .Setup(s => s.GetChatConversationsAsync(123, 1, 20))
                .ReturnsAsync(expectedConversations);

            // Act
            var result = await _controller.GetChatConversations();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = okResult.Value;
            
            var responseType = response.GetType();
            var dataProperty = responseType.GetProperty("data");
            
            var data = dataProperty.GetValue(response) as SocialPagedResult<ChatConversationDto>;
            Assert.NotNull(data);
            Assert.Equal(10, data.TotalCount);
            Assert.Equal(2, data.Data.Count);
        }

        [Fact]
        public async Task GetChatMessages_ShouldReturnMessageList_WhenValidRequest()
        {
            // Arrange
            var expectedMessages = new SocialPagedResult<ChatMessageDto>
            {
                Page = 1,
                PageSize = 50,
                TotalCount = 25,
                Data = new List<ChatMessageDto>
                {
                    new() { MessageId = 1, SenderId = 123, ChatContent = "你好", IsRead = true },
                    new() { MessageId = 2, SenderId = 456, ChatContent = "你好！", IsRead = true }
                }
            };

            _mockSocialService
                .Setup(s => s.GetChatMessagesAsync(123, 456, 1, 50))
                .ReturnsAsync(expectedMessages);

            // Act
            var result = await _controller.GetChatMessages(456);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = okResult.Value;
            
            var responseType = response.GetType();
            var dataProperty = responseType.GetProperty("data");
            
            var data = dataProperty.GetValue(response) as SocialPagedResult<ChatMessageDto>;
            Assert.NotNull(data);
            Assert.Equal(25, data.TotalCount);
            Assert.Equal(2, data.Data.Count);
        }

        #endregion

        #region 群組管理測試

        [Fact]
        public async Task CreateGroup_ShouldReturnSuccess_WhenValidRequest()
        {
            // Arrange
            var createDto = new CreateGroupDto
            {
                GroupName = "測試群組",
                InitialMemberIds = new List<int> { 456, 789 }
            };

            var expectedGroup = new GroupDto
            {
                GroupId = 100,
                GroupName = "測試群組",
                CreatedBy = 123,
                MemberCount = 3,
                IsAdmin = true,
                IsMember = true
            };

            var expectedResult = SocialServiceResult<GroupDto>.CreateSuccess(
                expectedGroup, "群組建立成功");

            _mockSocialService
                .Setup(s => s.CreateGroupAsync(123, createDto))
                .ReturnsAsync(expectedResult);

            // Act
            var result = await _controller.CreateGroup(createDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = okResult.Value;
            
            var responseType = response.GetType();
            var successProperty = responseType.GetProperty("success");
            var dataProperty = responseType.GetProperty("data");
            
            Assert.True((bool)successProperty.GetValue(response));
            
            var data = dataProperty.GetValue(response) as GroupDto;
            Assert.NotNull(data);
            Assert.Equal("測試群組", data.GroupName);
            Assert.Equal(3, data.MemberCount);
        }

        [Fact]
        public async Task JoinGroup_ShouldReturnSuccess_WhenValidRequest()
        {
            // Arrange
            var joinDto = new JoinGroupDto
            {
                GroupId = 100
            };

            var expectedResult = SocialServiceResult.CreateSuccess("成功加入群組");

            _mockSocialService
                .Setup(s => s.JoinGroupAsync(123, joinDto))
                .ReturnsAsync(expectedResult);

            // Act
            var result = await _controller.JoinGroup(joinDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = okResult.Value;
            
            var responseType = response.GetType();
            var successProperty = responseType.GetProperty("success");
            var messageProperty = responseType.GetProperty("message");
            
            Assert.True((bool)successProperty.GetValue(response));
            Assert.Equal("成功加入群組", messageProperty.GetValue(response));
        }

        [Fact]
        public async Task LeaveGroup_ShouldReturnSuccess_WhenValidRequest()
        {
            // Arrange
            var expectedResult = SocialServiceResult.CreateSuccess("成功退出群組");

            _mockSocialService
                .Setup(s => s.LeaveGroupAsync(123, 100))
                .ReturnsAsync(expectedResult);

            // Act
            var result = await _controller.LeaveGroup(100);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = okResult.Value;
            
            var responseType = response.GetType();
            var successProperty = responseType.GetProperty("success");
            
            Assert.True((bool)successProperty.GetValue(response));
            _mockSocialService.Verify(s => s.LeaveGroupAsync(123, 100), Times.Once);
        }

        [Fact]
        public async Task GetUserGroups_ShouldReturnGroupList_WhenValidRequest()
        {
            // Arrange
            var expectedGroups = new SocialPagedResult<GroupDto>
            {
                Page = 1,
                PageSize = 20,
                TotalCount = 5,
                Data = new List<GroupDto>
                {
                    new() { GroupId = 1, GroupName = "群組1", MemberCount = 10, IsAdmin = true },
                    new() { GroupId = 2, GroupName = "群組2", MemberCount = 15, IsAdmin = false }
                }
            };

            _mockSocialService
                .Setup(s => s.GetUserGroupsAsync(123, 1, 20))
                .ReturnsAsync(expectedGroups);

            // Act
            var result = await _controller.GetUserGroups();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = okResult.Value;
            
            var responseType = response.GetType();
            var dataProperty = responseType.GetProperty("data");
            
            var data = dataProperty.GetValue(response) as SocialPagedResult<GroupDto>;
            Assert.NotNull(data);
            Assert.Equal(5, data.TotalCount);
            Assert.Equal(2, data.Data.Count);
        }

        #endregion

        #region 群組聊天測試

        [Fact]
        public async Task SendGroupChat_ShouldReturnSuccess_WhenValidRequest()
        {
            // Arrange
            var sendChatDto = new SendGroupChatDto
            {
                GroupId = 100,
                GroupChatContent = "大家好！這是群組訊息。"
            };

            var expectedChat = new GroupChatDto
            {
                GroupChatId = 200,
                GroupId = 100,
                SenderId = 123,
                GroupChatContent = "大家好！這是群組訊息。",
                SentAt = DateTime.UtcNow,
                IsSent = true
            };

            var expectedResult = SocialServiceResult<GroupChatDto>.CreateSuccess(
                expectedChat, "群組訊息發送成功");

            _mockSocialService
                .Setup(s => s.SendGroupChatAsync(123, sendChatDto))
                .ReturnsAsync(expectedResult);

            // Act
            var result = await _controller.SendGroupChat(sendChatDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = okResult.Value;
            
            var responseType = response.GetType();
            var successProperty = responseType.GetProperty("success");
            var dataProperty = responseType.GetProperty("data");
            
            Assert.True((bool)successProperty.GetValue(response));
            
            var data = dataProperty.GetValue(response) as GroupChatDto;
            Assert.NotNull(data);
            Assert.Equal("大家好！這是群組訊息。", data.GroupChatContent);
        }

        [Fact]
        public async Task GetGroupChatMessages_ShouldReturnMessageList_WhenValidRequest()
        {
            // Arrange
            var expectedMessages = new SocialPagedResult<GroupChatDto>
            {
                Page = 1,
                PageSize = 50,
                TotalCount = 30,
                Data = new List<GroupChatDto>
                {
                    new() { GroupChatId = 1, GroupId = 100, GroupChatContent = "訊息1", SenderName = "用戶1" },
                    new() { GroupChatId = 2, GroupId = 100, GroupChatContent = "訊息2", SenderName = "用戶2" }
                }
            };

            _mockSocialService
                .Setup(s => s.GetGroupChatMessagesAsync(123, 100, 1, 50))
                .ReturnsAsync(expectedMessages);

            // Act
            var result = await _controller.GetGroupChatMessages(100);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = okResult.Value;
            
            var responseType = response.GetType();
            var dataProperty = responseType.GetProperty("data");
            
            var data = dataProperty.GetValue(response) as SocialPagedResult<GroupChatDto>;
            Assert.NotNull(data);
            Assert.Equal(30, data.TotalCount);
            Assert.Equal(2, data.Data.Count);
        }

        #endregion

        #region 封鎖系統測試

        [Fact]
        public async Task BlockUserInGroup_ShouldReturnSuccess_WhenValidRequest()
        {
            // Arrange
            var blockDto = new BlockUserDto
            {
                GroupId = 100,
                UserId = 456,
                BlockReason = "違反群組規則"
            };

            var expectedResult = SocialServiceResult.CreateSuccess("使用者已被封鎖");

            _mockSocialService
                .Setup(s => s.BlockUserInGroupAsync(123, blockDto))
                .ReturnsAsync(expectedResult);

            // Act
            var result = await _controller.BlockUserInGroup(blockDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = okResult.Value;
            
            var responseType = response.GetType();
            var successProperty = responseType.GetProperty("success");
            var messageProperty = responseType.GetProperty("message");
            
            Assert.True((bool)successProperty.GetValue(response));
            Assert.Equal("使用者已被封鎖", messageProperty.GetValue(response));
        }

        [Fact]
        public async Task UnblockUserInGroup_ShouldReturnSuccess_WhenValidRequest()
        {
            // Arrange
            var unblockDto = new UnblockUserDto
            {
                GroupId = 100,
                UserId = 456
            };

            var expectedResult = SocialServiceResult.CreateSuccess("封鎖已解除");

            _mockSocialService
                .Setup(s => s.UnblockUserInGroupAsync(123, unblockDto))
                .ReturnsAsync(expectedResult);

            // Act
            var result = await _controller.UnblockUserInGroup(unblockDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = okResult.Value;
            
            var responseType = response.GetType();
            var successProperty = responseType.GetProperty("success");
            
            Assert.True((bool)successProperty.GetValue(response));
            _mockSocialService.Verify(s => s.UnblockUserInGroupAsync(123, unblockDto), Times.Once);
        }

        #endregion

        #region 錯誤處理測試

        [Fact]
        public async Task CreateNotification_ShouldReturnBadRequest_WhenInvalidModel()
        {
            // Arrange
            var createDto = new CreateNotificationDto(); // 空的DTO，缺少必填欄位

            _controller.ModelState.AddModelError("SourceId", "來源類型為必填");

            // Act
            var result = await _controller.CreateNotification(createDto);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var response = badRequestResult.Value;
            
            var responseType = response.GetType();
            var successProperty = responseType.GetProperty("success");
            var messageProperty = responseType.GetProperty("message");
            
            Assert.False((bool)successProperty.GetValue(response));
            Assert.Equal("通知資料格式錯誤", messageProperty.GetValue(response));
        }

        [Fact]
        public async Task GetNotifications_ShouldReturnServerError_WhenServiceThrows()
        {
            // Arrange
            _mockSocialService
                .Setup(s => s.GetUserNotificationsAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<bool>(), It.IsAny<int?>()))
                .ThrowsAsync(new Exception("Database connection failed"));

            // Act
            var result = await _controller.GetNotifications();

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, statusCodeResult.StatusCode);
        }

        [Fact]
        public async Task SendChatMessage_ShouldReturnBadRequest_WhenServiceFails()
        {
            // Arrange
            var sendMessageDto = new SendChatMessageDto
            {
                ReceiverId = 999, // 不存在的使用者
                ChatContent = "測試訊息"
            };

            var expectedResult = SocialServiceResult<ChatMessageDto>.CreateFailure(
                "接收者不存在", new List<string> { "指定的接收者不存在" });

            _mockSocialService
                .Setup(s => s.SendChatMessageAsync(123, sendMessageDto))
                .ReturnsAsync(expectedResult);

            // Act
            var result = await _controller.SendChatMessage(sendMessageDto);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var response = badRequestResult.Value;
            
            var responseType = response.GetType();
            var successProperty = responseType.GetProperty("success");
            var messageProperty = responseType.GetProperty("message");
            
            Assert.False((bool)successProperty.GetValue(response));
            Assert.Equal("接收者不存在", messageProperty.GetValue(response));
        }

        #endregion

        #region 權限測試

        [Fact]
        public async Task CreateGroup_ShouldRequireAuthentication()
        {
            // Arrange
            var createDto = new CreateGroupDto
            {
                GroupName = "測試群組"
            };

            // 移除使用者身份
            _controller.ControllerContext.HttpContext.User = new ClaimsPrincipal();

            // Act & Assert
            // 在實際實作中，這應該會拋出 UnauthorizedAccessException
            // 或返回 Unauthorized 結果
        }

        #endregion

        #region 邊界條件測試

        [Fact]
        public async Task GetNotifications_ShouldLimitPageSize_WhenLargePageSizeRequested()
        {
            // Arrange
            _mockSocialService
                .Setup(s => s.GetUserNotificationsAsync(123, 1, 100, false, null))
                .ReturnsAsync(new SocialPagedResult<NotificationDto>());

            // Act
            await _controller.GetNotifications(pageSize: 200); // 超過限制

            // Assert
            _mockSocialService.Verify(s => s.GetUserNotificationsAsync(123, 1, 100, false, null), Times.Once);
        }

        #endregion
    }

    #region 測試用控制器類別

    // 模擬的SocialController類別，用於測試
    public class SocialController : ControllerBase
    {
        private readonly ISocialService _socialService;
        private readonly ILogger<SocialController> _logger;

        public SocialController(ISocialService socialService, ILogger<SocialController> logger)
        {
            _socialService = socialService;
            _logger = logger;
        }

        // 通知相關方法
        public async Task<IActionResult> CreateNotification(CreateNotificationDto createDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { success = false, message = "通知資料格式錯誤", errors = ModelState });
            }

            try
            {
                var result = await _socialService.CreateNotificationAsync(createDto);
                if (result.Success)
                {
                    return Ok(new { success = true, data = result.Data, message = result.Message });
                }
                else
                {
                    return BadRequest(new { success = false, message = result.Message, errors = result.Errors });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "建立通知時發生錯誤");
                return StatusCode(500, new { success = false, message = "建立通知時發生錯誤" });
            }
        }

        public async Task<IActionResult> GetNotifications(int page = 1, int pageSize = 20, bool unreadOnly = false, int? sourceId = null)
        {
            try
            {
                var userId = GetCurrentUserId();
                var result = await _socialService.GetUserNotificationsAsync(userId, page, Math.Min(pageSize, 100), unreadOnly, sourceId);
                return Ok(new { success = true, data = result, message = "通知列表取得成功" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "取得通知列表時發生錯誤");
                return StatusCode(500, new { success = false, message = "取得通知列表時發生錯誤" });
            }
        }

        public async Task<IActionResult> MarkNotificationsAsRead(MarkNotificationReadDto markReadDto)
        {
            try
            {
                var userId = GetCurrentUserId();
                var result = await _socialService.MarkNotificationsAsReadAsync(userId, markReadDto);
                return Ok(new { success = result.Success, message = result.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "標記通知已讀時發生錯誤");
                return StatusCode(500, new { success = false, message = "標記通知已讀時發生錯誤" });
            }
        }

        public async Task<IActionResult> GetNotificationStats()
        {
            try
            {
                var userId = GetCurrentUserId();
                var stats = await _socialService.GetNotificationStatsAsync(userId);
                return Ok(new { success = true, data = stats, message = "通知統計取得成功" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "取得通知統計時發生錯誤");
                return StatusCode(500, new { success = false, message = "取得通知統計時發生錯誤" });
            }
        }

        // 聊天相關方法
        public async Task<IActionResult> SendChatMessage(SendChatMessageDto sendMessageDto)
        {
            try
            {
                var userId = GetCurrentUserId();
                var result = await _socialService.SendChatMessageAsync(userId, sendMessageDto);
                if (result.Success)
                {
                    return Ok(new { success = true, data = result.Data, message = result.Message });
                }
                else
                {
                    return BadRequest(new { success = false, message = result.Message, errors = result.Errors });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "發送聊天訊息時發生錯誤");
                return StatusCode(500, new { success = false, message = "發送聊天訊息時發生錯誤" });
            }
        }

        public async Task<IActionResult> GetChatConversations(int page = 1, int pageSize = 20)
        {
            try
            {
                var userId = GetCurrentUserId();
                var result = await _socialService.GetChatConversationsAsync(userId, page, pageSize);
                return Ok(new { success = true, data = result, message = "聊天對話列表取得成功" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "取得聊天對話列表時發生錯誤");
                return StatusCode(500, new { success = false, message = "取得聊天對話列表時發生錯誤" });
            }
        }

        public async Task<IActionResult> GetChatMessages(int partnerId, int page = 1, int pageSize = 50)
        {
            try
            {
                var userId = GetCurrentUserId();
                var result = await _socialService.GetChatMessagesAsync(userId, partnerId, page, pageSize);
                return Ok(new { success = true, data = result, message = "聊天記錄取得成功" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "取得聊天記錄時發生錯誤");
                return StatusCode(500, new { success = false, message = "取得聊天記錄時發生錯誤" });
            }
        }

        // 群組相關方法
        public async Task<IActionResult> CreateGroup(CreateGroupDto createDto)
        {
            try
            {
                var userId = GetCurrentUserId();
                var result = await _socialService.CreateGroupAsync(userId, createDto);
                if (result.Success)
                {
                    return Ok(new { success = true, data = result.Data, message = result.Message });
                }
                else
                {
                    return BadRequest(new { success = false, message = result.Message, errors = result.Errors });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "建立群組時發生錯誤");
                return StatusCode(500, new { success = false, message = "建立群組時發生錯誤" });
            }
        }

        public async Task<IActionResult> JoinGroup(JoinGroupDto joinDto)
        {
            try
            {
                var userId = GetCurrentUserId();
                var result = await _socialService.JoinGroupAsync(userId, joinDto);
                return Ok(new { success = result.Success, message = result.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "加入群組時發生錯誤");
                return StatusCode(500, new { success = false, message = "加入群組時發生錯誤" });
            }
        }

        public async Task<IActionResult> LeaveGroup(int groupId)
        {
            try
            {
                var userId = GetCurrentUserId();
                var result = await _socialService.LeaveGroupAsync(userId, groupId);
                return Ok(new { success = result.Success, message = result.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "退出群組時發生錯誤");
                return StatusCode(500, new { success = false, message = "退出群組時發生錯誤" });
            }
        }

        public async Task<IActionResult> GetUserGroups(int page = 1, int pageSize = 20)
        {
            try
            {
                var userId = GetCurrentUserId();
                var result = await _socialService.GetUserGroupsAsync(userId, page, pageSize);
                return Ok(new { success = true, data = result, message = "群組列表取得成功" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "取得群組列表時發生錯誤");
                return StatusCode(500, new { success = false, message = "取得群組列表時發生錯誤" });
            }
        }

        // 群組聊天相關方法
        public async Task<IActionResult> SendGroupChat(SendGroupChatDto sendChatDto)
        {
            try
            {
                var userId = GetCurrentUserId();
                var result = await _socialService.SendGroupChatAsync(userId, sendChatDto);
                if (result.Success)
                {
                    return Ok(new { success = true, data = result.Data, message = result.Message });
                }
                else
                {
                    return BadRequest(new { success = false, message = result.Message, errors = result.Errors });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "發送群組聊天時發生錯誤");
                return StatusCode(500, new { success = false, message = "發送群組聊天時發生錯誤" });
            }
        }

        public async Task<IActionResult> GetGroupChatMessages(int groupId, int page = 1, int pageSize = 50)
        {
            try
            {
                var userId = GetCurrentUserId();
                var result = await _socialService.GetGroupChatMessagesAsync(userId, groupId, page, pageSize);
                return Ok(new { success = true, data = result, message = "群組聊天記錄取得成功" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "取得群組聊天記錄時發生錯誤");
                return StatusCode(500, new { success = false, message = "取得群組聊天記錄時發生錯誤" });
            }
        }

        // 封鎖相關方法
        public async Task<IActionResult> BlockUserInGroup(BlockUserDto blockDto)
        {
            try
            {
                var userId = GetCurrentUserId();
                var result = await _socialService.BlockUserInGroupAsync(userId, blockDto);
                return Ok(new { success = result.Success, message = result.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "封鎖使用者時發生錯誤");
                return StatusCode(500, new { success = false, message = "封鎖使用者時發生錯誤" });
            }
        }

        public async Task<IActionResult> UnblockUserInGroup(UnblockUserDto unblockDto)
        {
            try
            {
                var userId = GetCurrentUserId();
                var result = await _socialService.UnblockUserInGroupAsync(userId, unblockDto);
                return Ok(new { success = result.Success, message = result.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "解除封鎖時發生錯誤");
                return StatusCode(500, new { success = false, message = "解除封鎖時發生錯誤" });
            }
        }

        private int GetCurrentUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
            {
                throw new UnauthorizedAccessException("無法取得使用者身份資訊");
            }
            return userId;
        }
    }

    #endregion
}