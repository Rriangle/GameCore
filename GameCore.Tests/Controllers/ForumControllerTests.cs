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
    /// 論壇控制器測試類別
    /// 測試所有論壇相關API的功能和邊界條件
    /// 驗證版面管理、主題討論、回覆互動、反應收藏等完整論壇功能
    /// </summary>
    public class ForumControllerTests
    {
        private readonly Mock<IForumService> _mockForumService;
        private readonly Mock<ILogger<ForumController>> _mockLogger;
        private readonly ForumController _controller;

        public ForumControllerTests()
        {
            _mockForumService = new Mock<IForumService>();
            _mockLogger = new Mock<ILogger<ForumController>>();
            _controller = new ForumController(_mockForumService.Object, _mockLogger.Object);

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

        #region 論壇版面測試

        [Fact]
        public async Task GetForums_ShouldReturnForumList_WhenValidRequest()
        {
            // Arrange
            var expectedForums = new List<ForumDto>
            {
                new() { ForumId = 1, Name = "遊戲討論區", GameName = "測試遊戲1", ThreadCount = 10, PostCount = 50, IsActive = true },
                new() { ForumId = 2, Name = "新手指南", GameName = "測試遊戲2", ThreadCount = 8, PostCount = 30, IsActive = true }
            };

            _mockForumService
                .Setup(s => s.GetForumsAsync(null, true))
                .ReturnsAsync(expectedForums);

            // Act
            var result = await _controller.GetForums();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = okResult.Value;
            
            Assert.NotNull(response);
            
            var responseType = response.GetType();
            var successProperty = responseType.GetProperty("Success");
            var dataProperty = responseType.GetProperty("Data");
            
            Assert.True((bool)successProperty.GetValue(response));
            Assert.NotNull(dataProperty.GetValue(response));
        }

        [Fact]
        public async Task GetForum_ShouldReturnForumDetail_WhenForumExists()
        {
            // Arrange
            var expectedForum = new ForumDto
            {
                ForumId = 1,
                Name = "遊戲討論區",
                Description = "歡迎討論遊戲相關話題",
                GameName = "測試遊戲",
                ThreadCount = 15,
                PostCount = 75,
                ViewCount = 1200,
                IsActive = true,
                Moderators = new List<ModeratorDto>
                {
                    new() { UserId = 1, UserName = "版主1", AssignedAt = DateTime.UtcNow }
                }
            };

            _mockForumService
                .Setup(s => s.GetForumDetailAsync(1))
                .ReturnsAsync(expectedForum);

            // Act
            var result = await _controller.GetForum(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = okResult.Value;
            
            var responseType = response.GetType();
            var successProperty = responseType.GetProperty("Success");
            var dataProperty = responseType.GetProperty("Data");
            
            Assert.True((bool)successProperty.GetValue(response));
            
            var data = dataProperty.GetValue(response) as ForumDto;
            Assert.NotNull(data);
            Assert.Equal("遊戲討論區", data.Name);
            Assert.Equal(15, data.ThreadCount);
            Assert.Single(data.Moderators);
        }

        [Fact]
        public async Task GetForum_ShouldReturnNotFound_WhenForumDoesNotExist()
        {
            // Arrange
            _mockForumService
                .Setup(s => s.GetForumDetailAsync(999))
                .ReturnsAsync((ForumDto?)null);

            // Act
            var result = await _controller.GetForum(999);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            var response = notFoundResult.Value;
            
            var responseType = response.GetType();
            var successProperty = responseType.GetProperty("Success");
            var messageProperty = responseType.GetProperty("Message");
            
            Assert.False((bool)successProperty.GetValue(response));
            Assert.Equal("論壇不存在", messageProperty.GetValue(response));
        }

        #endregion

        #region 主題管理測試

        [Fact]
        public async Task GetThreads_ShouldReturnThreadList_WhenValidRequest()
        {
            // Arrange
            var expectedThreads = new ForumPagedResult<ThreadDto>
            {
                Page = 1,
                PageSize = 20,
                TotalCount = 50,
                Data = new List<ThreadDto>
                {
                    new() { ThreadId = 1, Title = "測試主題1", AuthorName = "作者1", PostCount = 5, ViewCount = 100, IsPinned = false },
                    new() { ThreadId = 2, Title = "測試主題2", AuthorName = "作者2", PostCount = 8, ViewCount = 150, IsPinned = true }
                }
            };

            _mockForumService
                .Setup(s => s.GetThreadsAsync(It.IsAny<ThreadSearchDto>(), 123))
                .ReturnsAsync(expectedThreads);

            // Act
            var result = await _controller.GetThreads(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = okResult.Value;
            
            var responseType = response.GetType();
            var dataProperty = responseType.GetProperty("data");
            
            var data = dataProperty.GetValue(response) as ForumPagedResult<ThreadDto>;
            Assert.NotNull(data);
            Assert.Equal(50, data.TotalCount);
            Assert.Equal(2, data.Data.Count);
        }

        [Fact]
        public async Task CreateThread_ShouldReturnSuccess_WhenValidRequest()
        {
            // Arrange
            var createDto = new CreateThreadDto
            {
                ForumId = 1,
                Title = "新主題",
                Content = "這是新主題的內容",
                Tags = new List<string> { "攻略", "心得" }
            };

            var expectedThread = new ThreadDto
            {
                ThreadId = 100,
                Title = "新主題",
                Content = "這是新主題的內容",
                ForumId = 1,
                AuthorId = 123,
                Status = "normal"
            };

            var expectedResult = ForumServiceResult<ThreadDto>.CreateSuccess(
                expectedThread, "主題建立成功");

            _mockForumService
                .Setup(s => s.CreateThreadAsync(123, createDto))
                .ReturnsAsync(expectedResult);

            // Act
            var result = await _controller.CreateThread(createDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = okResult.Value;
            
            var responseType = response.GetType();
            var successProperty = responseType.GetProperty("success");
            var dataProperty = responseType.GetProperty("data");
            
            Assert.True((bool)successProperty.GetValue(response));
            
            var data = dataProperty.GetValue(response) as ThreadDto;
            Assert.NotNull(data);
            Assert.Equal("新主題", data.Title);
        }

        #endregion

        #region 回覆管理測試

        [Fact]
        public async Task CreateThreadPost_ShouldReturnSuccess_WhenValidRequest()
        {
            // Arrange
            var createDto = new CreateThreadPostDto
            {
                ThreadId = 1,
                Content = "這是新的回覆內容",
                ParentPostId = null
            };

            var expectedPost = new ThreadPostDto
            {
                PostId = 100,
                ThreadId = 1,
                AuthorId = 123,
                Content = "這是新的回覆內容",
                FloorNumber = 5,
                Status = "normal"
            };

            var expectedResult = ForumServiceResult<ThreadPostDto>.CreateSuccess(
                expectedPost, "回覆建立成功");

            _mockForumService
                .Setup(s => s.CreateThreadPostAsync(123, createDto))
                .ReturnsAsync(expectedResult);

            // Act
            var result = await _controller.CreateThreadPost(createDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = okResult.Value;
            
            var responseType = response.GetType();
            var successProperty = responseType.GetProperty("success");
            var dataProperty = responseType.GetProperty("data");
            
            Assert.True((bool)successProperty.GetValue(response));
            
            var data = dataProperty.GetValue(response) as ThreadPostDto;
            Assert.NotNull(data);
            Assert.Equal("這是新的回覆內容", data.Content);
            Assert.Equal(5, data.FloorNumber);
        }

        #endregion

        #region 互動管理測試

        [Fact]
        public async Task AddReaction_ShouldReturnSuccess_WhenValidRequest()
        {
            // Arrange
            var reactionDto = new AddReactionDto
            {
                TargetType = "thread",
                TargetId = 1,
                ReactionType = "like"
            };

            var expectedReaction = new ReactionDto
            {
                ReactionId = 100,
                TargetType = "thread",
                TargetId = 1,
                UserId = 123,
                ReactionType = "like",
                CreatedAt = DateTime.UtcNow
            };

            var expectedResult = ForumServiceResult<ReactionDto>.CreateSuccess(
                expectedReaction, "反應新增成功");

            _mockForumService
                .Setup(s => s.AddReactionAsync(123, reactionDto))
                .ReturnsAsync(expectedResult);

            // Act
            var result = await _controller.AddReaction(reactionDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = okResult.Value;
            
            var responseType = response.GetType();
            var dataProperty = responseType.GetProperty("data");
            
            var data = dataProperty.GetValue(response) as ReactionDto;
            Assert.NotNull(data);
            Assert.Equal("thread", data.TargetType);
            Assert.Equal("like", data.ReactionType);
        }

        [Fact]
        public async Task AddBookmark_ShouldReturnSuccess_WhenValidRequest()
        {
            // Arrange
            var bookmarkDto = new AddBookmarkDto
            {
                TargetType = "thread",
                TargetId = 1,
                Notes = "很有價值的討論"
            };

            var expectedBookmark = new BookmarkDto
            {
                BookmarkId = 100,
                TargetType = "thread",
                TargetId = 1,
                UserId = 123,
                Title = "測試主題",
                CreatedAt = DateTime.UtcNow
            };

            var expectedResult = ForumServiceResult<BookmarkDto>.CreateSuccess(
                expectedBookmark, "收藏新增成功");

            _mockForumService
                .Setup(s => s.AddBookmarkAsync(123, bookmarkDto))
                .ReturnsAsync(expectedResult);

            // Act
            var result = await _controller.AddBookmark(bookmarkDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = okResult.Value;
            
            var responseType = response.GetType();
            var dataProperty = responseType.GetProperty("data");
            
            var data = dataProperty.GetValue(response) as BookmarkDto;
            Assert.NotNull(data);
            Assert.Equal("thread", data.TargetType);
            Assert.Equal(1, data.TargetId);
        }

        #endregion

        #region 統計分析測試

        [Fact]
        public async Task GetStatistics_ShouldReturnStatistics()
        {
            // Arrange
            var expectedStats = new ForumStatisticsDto
            {
                TotalForums = 20,
                ActiveForums = 18,
                TotalThreads = 500,
                TotalPosts = 2500,
                TodayThreads = 15,
                TodayPosts = 80,
                ActiveUsers = 150,
                TotalViews = 50000,
                ForumActivities = new List<ForumActivityDto>
                {
                    new() { ForumId = 1, ForumName = "熱門論壇", ThreadCount = 50, PostCount = 300, ViewCount = 5000 }
                },
                PopularThreads = new List<ThreadSummaryDto>
                {
                    new() { ThreadId = 1, Title = "熱門主題", PostCount = 25, ViewCount = 800, LikeCount = 15 }
                }
            };

            _mockForumService
                .Setup(s => s.GetStatisticsAsync())
                .ReturnsAsync(expectedStats);

            // Act
            var result = await _controller.GetStatistics();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = okResult.Value;
            
            var responseType = response.GetType();
            var dataProperty = responseType.GetProperty("data");
            
            var data = dataProperty.GetValue(response) as ForumStatisticsDto;
            Assert.NotNull(data);
            Assert.Equal(20, data.TotalForums);
            Assert.Equal(500, data.TotalThreads);
            Assert.Single(data.ForumActivities);
            Assert.Single(data.PopularThreads);
        }

        #endregion

        #region 錯誤處理測試

        [Fact]
        public async Task GetForums_ShouldReturnServerError_WhenServiceThrows()
        {
            // Arrange
            _mockForumService
                .Setup(s => s.GetForumsAsync(It.IsAny<int?>(), It.IsAny<bool>()))
                .ThrowsAsync(new Exception("Database connection failed"));

            // Act
            var result = await _controller.GetForums();

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, statusCodeResult.StatusCode);
        }

        [Fact]
        public async Task CreateThread_ShouldReturnBadRequest_WhenInvalidModel()
        {
            // Arrange
            var createDto = new CreateThreadDto(); // 空的DTO，缺少必填欄位

            _controller.ModelState.AddModelError("Title", "主題標題為必填");

            // Act
            var result = await _controller.CreateThread(createDto);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var response = badRequestResult.Value;
            
            var responseType = response.GetType();
            var successProperty = responseType.GetProperty("success");
            var messageProperty = responseType.GetProperty("message");
            
            Assert.False((bool)successProperty.GetValue(response));
            Assert.Equal("主題資料格式錯誤", messageProperty.GetValue(response));
        }

        #endregion

        #region 邊界條件測試

        [Fact]
        public async Task GetThreads_ShouldLimitPageSize_WhenLargePageSizeRequested()
        {
            // Arrange
            _mockForumService
                .Setup(s => s.GetThreadsAsync(It.Is<ThreadSearchDto>(dto => dto.PageSize == 100), It.IsAny<int?>()))
                .ReturnsAsync(new ForumPagedResult<ThreadDto>());

            // Act
            await _controller.GetThreads(1, pageSize: 200); // 超過限制

            // Assert
            _mockForumService.Verify(s => s.GetThreadsAsync(It.Is<ThreadSearchDto>(dto => dto.PageSize == 100), It.IsAny<int?>()), Times.Once);
        }

        #endregion

        #region 權限測試

        [Fact]
        public async Task CreateForum_ShouldRequireAdminRole()
        {
            // Arrange
            var createDto = new CreateForumDto
            {
                GameId = 1,
                Name = "新討論區",
                Description = "測試描述"
            };

            // 設定管理員身份
            var adminClaims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, "1"),
                new Claim(ClaimTypes.Role, "Admin")
            };
            var adminIdentity = new ClaimsIdentity(adminClaims, "Test");
            var adminPrincipal = new ClaimsPrincipal(adminIdentity);

            _controller.ControllerContext.HttpContext.User = adminPrincipal;

            var expectedResult = ForumServiceResult<ForumDto>.CreateSuccess(
                new ForumDto { ForumId = 100, Name = "新討論區" }, "論壇建立成功");

            _mockForumService
                .Setup(s => s.CreateForumAsync(createDto))
                .ReturnsAsync(expectedResult);

            // Act
            var result = await _controller.CreateForum(createDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            _mockForumService.Verify(s => s.CreateForumAsync(createDto), Times.Once);
        }

        #endregion
    }

    #region 測試用 DTO 類別

    // 這些 DTO 類別用於測試，對應 ForumDTOs.cs 中的定義
    public class ForumDto
    {
        public int ForumId { get; set; }
        public int GameId { get; set; }
        public string? GameName { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int ThreadCount { get; set; }
        public int PostCount { get; set; }
        public int ViewCount { get; set; }
        public bool IsActive { get; set; }
        public List<ModeratorDto> Moderators { get; set; } = new();
    }

    public class ModeratorDto
    {
        public int UserId { get; set; }
        public string? UserName { get; set; }
        public DateTime AssignedAt { get; set; }
    }

    public class ThreadDto
    {
        public int ThreadId { get; set; }
        public int ForumId { get; set; }
        public int AuthorId { get; set; }
        public string? AuthorName { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public int PostCount { get; set; }
        public int ViewCount { get; set; }
        public int LikeCount { get; set; }
        public bool IsPinned { get; set; }
        public bool IsLocked { get; set; }
    }

    public class ThreadSummaryDto
    {
        public int ThreadId { get; set; }
        public string Title { get; set; } = string.Empty;
        public int PostCount { get; set; }
        public int ViewCount { get; set; }
        public int LikeCount { get; set; }
    }

    public class ThreadPostDto
    {
        public int PostId { get; set; }
        public int ThreadId { get; set; }
        public int AuthorId { get; set; }
        public string Content { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public int FloorNumber { get; set; }
    }

    public class ReactionDto
    {
        public int ReactionId { get; set; }
        public string TargetType { get; set; } = string.Empty;
        public int TargetId { get; set; }
        public int UserId { get; set; }
        public string ReactionType { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }

    public class BookmarkDto
    {
        public int BookmarkId { get; set; }
        public string TargetType { get; set; } = string.Empty;
        public int TargetId { get; set; }
        public int UserId { get; set; }
        public string? Title { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class CreateForumDto
    {
        public int GameId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }

    public class CreateThreadDto
    {
        public int ForumId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public List<string> Tags { get; set; } = new();
    }

    public class CreateThreadPostDto
    {
        public int ThreadId { get; set; }
        public string Content { get; set; } = string.Empty;
        public int? ParentPostId { get; set; }
    }

    public class AddReactionDto
    {
        public string TargetType { get; set; } = string.Empty;
        public int TargetId { get; set; }
        public string ReactionType { get; set; } = string.Empty;
    }

    public class AddBookmarkDto
    {
        public string TargetType { get; set; } = string.Empty;
        public int TargetId { get; set; }
        public string? Notes { get; set; }
    }

    public class ThreadSearchDto
    {
        public int? ForumId { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 20;
    }

    public class ForumPagedResult<T>
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public List<T> Data { get; set; } = new();
    }

    public class ForumStatisticsDto
    {
        public int TotalForums { get; set; }
        public int ActiveForums { get; set; }
        public int TotalThreads { get; set; }
        public int TotalPosts { get; set; }
        public int TodayThreads { get; set; }
        public int TodayPosts { get; set; }
        public int ActiveUsers { get; set; }
        public long TotalViews { get; set; }
        public List<ForumActivityDto> ForumActivities { get; set; } = new();
        public List<ThreadSummaryDto> PopularThreads { get; set; } = new();
    }

    public class ForumActivityDto
    {
        public int ForumId { get; set; }
        public string ForumName { get; set; } = string.Empty;
        public int ThreadCount { get; set; }
        public int PostCount { get; set; }
        public int ViewCount { get; set; }
    }

    public class ForumServiceResult
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public List<string> Errors { get; set; } = new();

        public static ForumServiceResult CreateSuccess(string message = "操作成功")
        {
            return new ForumServiceResult { Success = true, Message = message };
        }
    }

    public class ForumServiceResult<T> : ForumServiceResult
    {
        public T? Data { get; set; }

        public static ForumServiceResult<T> CreateSuccess(T data, string message = "操作成功")
        {
            return new ForumServiceResult<T> { Success = true, Message = message, Data = data };
        }
    }

    #endregion
}