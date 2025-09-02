using GameCore.Core.DTOs;
using GameCore.Core.Interfaces;
using GameCore.Web.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System.Security.Claims;

namespace GameCore.Tests.Controllers
{
    /// <summary>
    /// 論壇控制器測試
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
        }

        /// <summary>
        /// 測試取得論壇版面列表 - 成功
        /// </summary>
        [Fact]
        public async Task GetForums_ShouldReturnOkResult_WhenSuccessful()
        {
            // Arrange
            var expectedForums = new PagedResponse<ForumInfo>
            {
                Items = new List<ForumInfo>
                {
                    new ForumInfo
                    {
                        ForumId = 1,
                        GameId = 1,
                        GameName = "英雄聯盟",
                        Name = "LOL 討論區",
                        Description = "英雄聯盟相關討論",
                        ThreadCount = 100,
                        TodayThreadCount = 5,
                        ActiveUserCount = 50
                    }
                },
                TotalCount = 1,
                Page = 1,
                PageSize = 20,
                TotalPages = 1,
                HasPreviousPage = false,
                HasNextPage = false
            };

            _mockForumService.Setup(x => x.GetForumsAsync(It.IsAny<ForumQueryRequest>()))
                .ReturnsAsync(expectedForums);

            // Act
            var result = await _controller.GetForums();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedForums = Assert.IsType<PagedResponse<ForumInfo>>(okResult.Value);
            Assert.Equal(expectedForums.Items.Count, returnedForums.Items.Count);
            Assert.Equal(expectedForums.TotalCount, returnedForums.TotalCount);
        }

        /// <summary>
        /// 測試取得論壇版面詳情 - 成功
        /// </summary>
        [Fact]
        public async Task GetForum_ShouldReturnOkResult_WhenForumExists()
        {
            // Arrange
            var forumId = 1;
            var expectedForum = new ForumInfo
            {
                ForumId = forumId,
                GameId = 1,
                GameName = "英雄聯盟",
                Name = "LOL 討論區",
                Description = "英雄聯盟相關討論",
                ThreadCount = 100,
                TodayThreadCount = 5,
                ActiveUserCount = 50
            };

            _mockForumService.Setup(x => x.GetForumByIdAsync(forumId))
                .ReturnsAsync(expectedForum);

            // Act
            var result = await _controller.GetForum(forumId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedForum = Assert.IsType<ForumInfo>(okResult.Value);
            Assert.Equal(expectedForum.ForumId, returnedForum.ForumId);
            Assert.Equal(expectedForum.Name, returnedForum.Name);
        }

        /// <summary>
        /// 測試取得論壇版面詳情 - 不存在
        /// </summary>
        [Fact]
        public async Task GetForum_ShouldReturnNotFound_WhenForumDoesNotExist()
        {
            // Arrange
            var forumId = 999;
            _mockForumService.Setup(x => x.GetForumByIdAsync(forumId))
                .ReturnsAsync((ForumInfo?)null);

            // Act
            var result = await _controller.GetForum(forumId);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result.Result);
        }

        /// <summary>
        /// 測試取得主題列表 - 成功
        /// </summary>
        [Fact]
        public async Task GetThreads_ShouldReturnOkResult_WhenSuccessful()
        {
            // Arrange
            var forumId = 1;
            var expectedThreads = new PagedResponse<ThreadListItem>
            {
                Items = new List<ThreadListItem>
                {
                    new ThreadListItem
                    {
                        ThreadId = 1,
                        Title = "測試主題",
                        AuthorUserId = 1,
                        AuthorNickname = "測試用戶",
                        ReplyCount = 5,
                        ViewCount = 100,
                        LikeCount = 10,
                        CreatedAt = DateTime.UtcNow,
                        Status = "normal"
                    }
                },
                TotalCount = 1,
                Page = 1,
                PageSize = 20,
                TotalPages = 1,
                HasPreviousPage = false,
                HasNextPage = false
            };

            _mockForumService.Setup(x => x.GetThreadsAsync(It.IsAny<ThreadQueryRequest>()))
                .ReturnsAsync(expectedThreads);

            // Act
            var result = await _controller.GetThreads(forumId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedThreads = Assert.IsType<PagedResponse<ThreadListItem>>(okResult.Value);
            Assert.Equal(expectedThreads.Items.Count, returnedThreads.Items.Count);
        }

        /// <summary>
        /// 測試取得主題詳情 - 成功
        /// </summary>
        [Fact]
        public async Task GetThread_ShouldReturnOkResult_WhenThreadExists()
        {
            // Arrange
            var threadId = 1L;
            var expectedThread = new ThreadDetail
            {
                ThreadId = threadId,
                Title = "測試主題",
                AuthorUserId = 1,
                AuthorNickname = "測試用戶",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                ViewCount = 100,
                LikeCount = 10,
                BookmarkCount = 5,
                Status = "normal",
                IsPinned = false,
                IsLikedByCurrentUser = false,
                IsBookmarkedByCurrentUser = false,
                Posts = new List<ThreadPostDetail>()
            };

            _mockForumService.Setup(x => x.GetThreadByIdAsync(threadId, It.IsAny<int>()))
                .ReturnsAsync(expectedThread);

            // Act
            var result = await _controller.GetThread(threadId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedThread = Assert.IsType<ThreadDetail>(okResult.Value);
            Assert.Equal(expectedThread.ThreadId, returnedThread.ThreadId);
            Assert.Equal(expectedThread.Title, returnedThread.Title);
        }

        /// <summary>
        /// 測試建立新主題 - 成功
        /// </summary>
        [Fact]
        public async Task CreateThread_ShouldReturnCreatedResult_WhenSuccessful()
        {
            // Arrange
            var request = new CreateThreadRequest
            {
                ForumId = 1,
                Title = "新主題",
                Content = "主題內容"
            };

            var threadId = 1L;
            _mockForumService.Setup(x => x.CreateThreadAsync(request, It.IsAny<int>()))
                .ReturnsAsync(threadId);

            // Act
            var result = await _controller.CreateThread(request);

            // Assert
            var createdResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            Assert.Equal(threadId, createdResult.Value);
        }

        /// <summary>
        /// 測試建立新主題 - 無效請求
        /// </summary>
        [Fact]
        public async Task CreateThread_ShouldReturnBadRequest_WhenRequestIsInvalid()
        {
            // Arrange
            var request = new CreateThreadRequest
            {
                ForumId = 0, // 無效的論壇ID
                Title = "",
                Content = ""
            };

            _mockForumService.Setup(x => x.CreateThreadAsync(request, It.IsAny<int>()))
                .ThrowsAsync(new ArgumentException("論壇版不存在"));

            // Act
            var result = await _controller.CreateThread(request);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result.Result);
        }

        /// <summary>
        /// 測試建立新回覆 - 成功
        /// </summary>
        [Fact]
        public async Task CreatePost_ShouldReturnOkResult_WhenSuccessful()
        {
            // Arrange
            var request = new CreatePostRequest
            {
                ThreadId = 1,
                Content = "回覆內容"
            };

            var postId = 1L;
            _mockForumService.Setup(x => x.CreatePostAsync(request, It.IsAny<int>()))
                .ReturnsAsync(postId);

            // Act
            var result = await _controller.CreatePost(request);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedPostId = Assert.IsType<long>(okResult.Value);
            Assert.Equal(postId, returnedPostId);
        }

        /// <summary>
        /// 測試更新主題 - 成功
        /// </summary>
        [Fact]
        public async Task UpdateThread_ShouldReturnOkResult_WhenSuccessful()
        {
            // Arrange
            var threadId = 1L;
            var newTitle = "更新後的主題標題";

            _mockForumService.Setup(x => x.UpdateThreadAsync(threadId, newTitle, It.IsAny<int>()))
                .ReturnsAsync(true);

            // Act
            var result = await _controller.UpdateThread(threadId, newTitle);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var success = Assert.IsType<bool>(okResult.Value);
            Assert.True(success);
        }

        /// <summary>
        /// 測試更新主題 - 標題為空
        /// </summary>
        [Fact]
        public async Task UpdateThread_ShouldReturnBadRequest_WhenTitleIsEmpty()
        {
            // Arrange
            var threadId = 1L;
            var emptyTitle = "";

            // Act
            var result = await _controller.UpdateThread(threadId, emptyTitle);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result.Result);
        }

        /// <summary>
        /// 測試刪除主題 - 成功
        /// </summary>
        [Fact]
        public async Task DeleteThread_ShouldReturnOkResult_WhenSuccessful()
        {
            // Arrange
            var threadId = 1L;

            _mockForumService.Setup(x => x.DeleteThreadAsync(threadId, It.IsAny<int>()))
                .ReturnsAsync(true);

            // Act
            var result = await _controller.DeleteThread(threadId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var success = Assert.IsType<bool>(okResult.Value);
            Assert.True(success);
        }

        /// <summary>
        /// 測試新增反應 - 成功
        /// </summary>
        [Fact]
        public async Task AddReaction_ShouldReturnOkResult_WhenSuccessful()
        {
            // Arrange
            var request = new ReactionRequest
            {
                TargetType = "thread",
                TargetId = 1,
                Kind = "like"
            };

            _mockForumService.Setup(x => x.AddReactionAsync(request, It.IsAny<int>()))
                .ReturnsAsync(true);

            // Act
            var result = await _controller.AddReaction(request);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var success = Assert.IsType<bool>(okResult.Value);
            Assert.True(success);
        }

        /// <summary>
        /// 測試移除反應 - 成功
        /// </summary>
        [Fact]
        public async Task RemoveReaction_ShouldReturnOkResult_WhenSuccessful()
        {
            // Arrange
            var request = new ReactionRequest
            {
                TargetType = "thread",
                TargetId = 1,
                Kind = "like"
            };

            _mockForumService.Setup(x => x.RemoveReactionAsync(request, It.IsAny<int>()))
                .ReturnsAsync(true);

            // Act
            var result = await _controller.RemoveReaction(request);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var success = Assert.IsType<bool>(okResult.Value);
            Assert.True(success);
        }

        /// <summary>
        /// 測試新增收藏 - 成功
        /// </summary>
        [Fact]
        public async Task AddBookmark_ShouldReturnOkResult_WhenSuccessful()
        {
            // Arrange
            var request = new BookmarkRequest
            {
                TargetType = "thread",
                TargetId = 1
            };

            _mockForumService.Setup(x => x.AddBookmarkAsync(request, It.IsAny<int>()))
                .ReturnsAsync(true);

            // Act
            var result = await _controller.AddBookmark(request);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var success = Assert.IsType<bool>(okResult.Value);
            Assert.True(success);
        }

        /// <summary>
        /// 測試移除收藏 - 成功
        /// </summary>
        [Fact]
        public async Task RemoveBookmark_ShouldReturnOkResult_WhenSuccessful()
        {
            // Arrange
            var request = new BookmarkRequest
            {
                TargetType = "thread",
                TargetId = 1
            };

            _mockForumService.Setup(x => x.RemoveBookmarkAsync(request, It.IsAny<int>()))
                .ReturnsAsync(true);

            // Act
            var result = await _controller.RemoveBookmark(request);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var success = Assert.IsType<bool>(okResult.Value);
            Assert.True(success);
        }

        /// <summary>
        /// 測試搜尋 - 成功
        /// </summary>
        [Fact]
        public async Task Search_ShouldReturnOkResult_WhenSuccessful()
        {
            // Arrange
            var keyword = "測試";
            var expectedResults = new PagedResponse<object>
            {
                Items = new List<object>
                {
                    new { Type = "thread", Id = 1, Title = "測試主題" }
                },
                TotalCount = 1,
                Page = 1,
                PageSize = 20,
                TotalPages = 1,
                HasPreviousPage = false,
                HasNextPage = false
            };

            _mockForumService.Setup(x => x.SearchAsync(keyword, null, 1, 20))
                .ReturnsAsync(expectedResults);

            // Act
            var result = await _controller.Search(keyword);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedResults = Assert.IsType<PagedResponse<object>>(okResult.Value);
            Assert.Equal(expectedResults.Items.Count, returnedResults.Items.Count);
        }

        /// <summary>
        /// 測試搜尋 - 關鍵字為空
        /// </summary>
        [Fact]
        public async Task Search_ShouldReturnBadRequest_WhenKeywordIsEmpty()
        {
            // Arrange
            var emptyKeyword = "";

            // Act
            var result = await _controller.Search(emptyKeyword);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result.Result);
        }

        /// <summary>
        /// 測試取得熱門主題 - 成功
        /// </summary>
        [Fact]
        public async Task GetPopularThreads_ShouldReturnOkResult_WhenSuccessful()
        {
            // Arrange
            var expectedThreads = new List<ThreadListItem>
            {
                new ThreadListItem
                {
                    ThreadId = 1,
                    Title = "熱門主題",
                    AuthorUserId = 1,
                    AuthorNickname = "測試用戶",
                    ReplyCount = 10,
                    ViewCount = 200,
                    LikeCount = 20,
                    CreatedAt = DateTime.UtcNow,
                    Status = "normal"
                }
            };

            _mockForumService.Setup(x => x.GetPopularThreadsAsync(null, 10))
                .ReturnsAsync(expectedThreads);

            // Act
            var result = await _controller.GetPopularThreads();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedThreads = Assert.IsType<List<ThreadListItem>>(okResult.Value);
            Assert.Equal(expectedThreads.Count, returnedThreads.Count);
        }

        /// <summary>
        /// 測試取得最新主題 - 成功
        /// </summary>
        [Fact]
        public async Task GetLatestThreads_ShouldReturnOkResult_WhenSuccessful()
        {
            // Arrange
            var expectedThreads = new List<ThreadListItem>
            {
                new ThreadListItem
                {
                    ThreadId = 1,
                    Title = "最新主題",
                    AuthorUserId = 1,
                    AuthorNickname = "測試用戶",
                    ReplyCount = 5,
                    ViewCount = 100,
                    LikeCount = 10,
                    CreatedAt = DateTime.UtcNow,
                    Status = "normal"
                }
            };

            _mockForumService.Setup(x => x.GetLatestThreadsAsync(null, 10))
                .ReturnsAsync(expectedThreads);

            // Act
            var result = await _controller.GetLatestThreads();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedThreads = Assert.IsType<List<ThreadListItem>>(okResult.Value);
            Assert.Equal(expectedThreads.Count, returnedThreads.Count);
        }

        /// <summary>
        /// 測試服務異常處理
        /// </summary>
        [Fact]
        public async Task GetForums_ShouldReturnInternalServerError_WhenServiceThrowsException()
        {
            // Arrange
            _mockForumService.Setup(x => x.GetForumsAsync(It.IsAny<ForumQueryRequest>()))
                .ThrowsAsync(new Exception("服務異常"));

            // Act
            var result = await _controller.GetForums();

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result.Result);
            Assert.Equal(500, statusCodeResult.StatusCode);
        }

        /// <summary>
        /// 測試未授權訪問
        /// </summary>
        [Fact]
        public async Task CreateThread_ShouldReturnForbid_WhenUserNotAuthorized()
        {
            // Arrange
            var request = new CreateThreadRequest
            {
                ForumId = 1,
                Title = "測試主題",
                Content = "測試內容"
            };

            _mockForumService.Setup(x => x.CreateThreadAsync(request, It.IsAny<int>()))
                .ThrowsAsync(new UnauthorizedAccessException("用戶未授權"));

            // Act
            var result = await _controller.CreateThread(request);

            // Assert
            Assert.IsType<ForbidResult>(result.Result);
        }
    }
} 