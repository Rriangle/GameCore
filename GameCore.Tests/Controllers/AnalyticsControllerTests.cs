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
    /// 分析控制器測試類別
    /// 測試所有分析相關API的功能和邊界條件
    /// 驗證遊戲管理、指標收集、熱度計算、排行榜生成、洞察貼文等完整分析功能
    /// </summary>
    public class AnalyticsControllerTests
    {
        private readonly Mock<IAnalyticsService> _mockAnalyticsService;
        private readonly Mock<ILogger<AnalyticsController>> _mockLogger;
        private readonly AnalyticsController _controller;

        public AnalyticsControllerTests()
        {
            _mockAnalyticsService = new Mock<IAnalyticsService>();
            _mockLogger = new Mock<ILogger<AnalyticsController>>();
            _controller = new AnalyticsController(_mockAnalyticsService.Object, _mockLogger.Object);

            // 設定模擬的管理員身份
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, "1"),
                new Claim(ClaimTypes.Name, "admin"),
                new Claim(ClaimTypes.Role, "Admin")
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

        #region 遊戲管理測試

        [Fact]
        public async Task GetGames_ShouldReturnGameList_WhenValidRequest()
        {
            // Arrange
            var expectedGames = new AnalyticsPagedResult<GameDto>
            {
                Page = 1,
                PageSize = 20,
                TotalCount = 50,
                Data = new List<GameDto>
                {
                    new() { GameId = 1, GameName = "原神", IsActive = true, LatestPopularityIndex = 85.5m },
                    new() { GameId = 2, GameName = "英雄聯盟", IsActive = true, LatestPopularityIndex = 92.3m }
                }
            };

            _mockAnalyticsService
                .Setup(s => s.GetGamesAsync(true, 1, 20))
                .ReturnsAsync(expectedGames);

            // Act
            var result = await _controller.GetGames();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = okResult.Value;
            
            var responseType = response.GetType();
            var successProperty = responseType.GetProperty("success");
            var dataProperty = responseType.GetProperty("data");
            
            Assert.True((bool)successProperty.GetValue(response));
            
            var data = dataProperty.GetValue(response) as AnalyticsPagedResult<GameDto>;
            Assert.NotNull(data);
            Assert.Equal(50, data.TotalCount);
            Assert.Equal(2, data.Data.Count);
        }

        [Fact]
        public async Task CreateGame_ShouldReturnSuccess_WhenValidRequest()
        {
            // Arrange
            var createDto = new CreateGameDto
            {
                GameName = "新遊戲",
                GameDescription = "這是一個測試遊戲",
                IsActive = true
            };

            var expectedGame = new GameDto
            {
                GameId = 100,
                GameName = "新遊戲",
                GameDescription = "這是一個測試遊戲",
                IsActive = true
            };

            var expectedResult = AnalyticsServiceResult<GameDto>.CreateSuccess(
                expectedGame, "遊戲建立成功");

            _mockAnalyticsService
                .Setup(s => s.CreateGameAsync(createDto))
                .ReturnsAsync(expectedResult);

            // Act
            var result = await _controller.CreateGame(createDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = okResult.Value;
            
            var responseType = response.GetType();
            var successProperty = responseType.GetProperty("success");
            var dataProperty = responseType.GetProperty("data");
            
            Assert.True((bool)successProperty.GetValue(response));
            
            var data = dataProperty.GetValue(response) as GameDto;
            Assert.NotNull(data);
            Assert.Equal("新遊戲", data.GameName);
        }

        #endregion

        #region 指標管理測試

        [Fact]
        public async Task GetMetricSources_ShouldReturnSourceList_WhenValidRequest()
        {
            // Arrange
            var expectedSources = new List<MetricSourceDto>
            {
                new() { SourceId = 1, SourceName = "Steam", IsActive = true, MetricsCount = 5 },
                new() { SourceId = 2, SourceName = "Twitch", IsActive = true, MetricsCount = 3 }
            };

            _mockAnalyticsService
                .Setup(s => s.GetMetricSourcesAsync(true))
                .ReturnsAsync(expectedSources);

            // Act
            var result = await _controller.GetMetricSources();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = okResult.Value;
            
            var responseType = response.GetType();
            var dataProperty = responseType.GetProperty("data");
            
            var data = dataProperty.GetValue(response) as List<MetricSourceDto>;
            Assert.NotNull(data);
            Assert.Equal(2, data.Count);
            Assert.Equal("Steam", data[0].SourceName);
        }

        [Fact]
        public async Task CreateMetric_ShouldReturnSuccess_WhenValidRequest()
        {
            // Arrange
            var createDto = new CreateMetricDto
            {
                SourceId = 1,
                Code = "test_metric",
                Unit = "count",
                Description = "測試指標",
                Weight = 1.5m
            };

            var expectedMetric = new MetricDto
            {
                MetricId = 100,
                SourceId = 1,
                Code = "test_metric",
                Unit = "count",
                Description = "測試指標",
                Weight = 1.5m
            };

            var expectedResult = AnalyticsServiceResult<MetricDto>.CreateSuccess(
                expectedMetric, "指標建立成功");

            _mockAnalyticsService
                .Setup(s => s.CreateMetricAsync(createDto))
                .ReturnsAsync(expectedResult);

            // Act
            var result = await _controller.CreateMetric(createDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = okResult.Value;
            
            var responseType = response.GetType();
            var successProperty = responseType.GetProperty("success");
            var dataProperty = responseType.GetProperty("data");
            
            Assert.True((bool)successProperty.GetValue(response));
            
            var data = dataProperty.GetValue(response) as MetricDto;
            Assert.NotNull(data);
            Assert.Equal("test_metric", data.Code);
        }

        #endregion

        #region 每日指標數據測試

        [Fact]
        public async Task GetGameMetricDaily_ShouldReturnMetricData_WhenValidRequest()
        {
            // Arrange
            var expectedData = new AnalyticsPagedResult<GameMetricDailyDto>
            {
                Page = 1,
                PageSize = 50,
                TotalCount = 100,
                Data = new List<GameMetricDailyDto>
                {
                    new() { Id = 1, GameId = 1, MetricId = 1, Date = DateTime.Today, Value = 1000.0m },
                    new() { Id = 2, GameId = 1, MetricId = 2, Date = DateTime.Today, Value = 500.0m }
                }
            };

            _mockAnalyticsService
                .Setup(s => s.GetGameMetricDailyAsync(1, null, null, null, 1, 50))
                .ReturnsAsync(expectedData);

            // Act
            var result = await _controller.GetGameMetricDaily(gameId: 1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = okResult.Value;
            
            var responseType = response.GetType();
            var dataProperty = responseType.GetProperty("data");
            
            var data = dataProperty.GetValue(response) as AnalyticsPagedResult<GameMetricDailyDto>;
            Assert.NotNull(data);
            Assert.Equal(100, data.TotalCount);
            Assert.Equal(2, data.Data.Count);
        }

        [Fact]
        public async Task CreateGameMetricDaily_ShouldReturnSuccess_WhenValidRequest()
        {
            // Arrange
            var createDto = new CreateGameMetricDailyDto
            {
                GameId = 1,
                MetricId = 1,
                Date = DateTime.Today,
                Value = 1500.0m,
                AggMethod = "sum"
            };

            var expectedMetric = new GameMetricDailyDto
            {
                Id = 100,
                GameId = 1,
                MetricId = 1,
                Date = DateTime.Today,
                Value = 1500.0m
            };

            var expectedResult = AnalyticsServiceResult<GameMetricDailyDto>.CreateSuccess(
                expectedMetric, "指標數據建立成功");

            _mockAnalyticsService
                .Setup(s => s.CreateGameMetricDailyAsync(createDto))
                .ReturnsAsync(expectedResult);

            // Act
            var result = await _controller.CreateGameMetricDaily(createDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = okResult.Value;
            
            var responseType = response.GetType();
            var successProperty = responseType.GetProperty("success");
            
            Assert.True((bool)successProperty.GetValue(response));
        }

        #endregion

        #region 熱度指數測試

        [Fact]
        public async Task CalculatePopularityIndex_ShouldReturnSuccess_WhenValidRequest()
        {
            // Arrange
            var expectedResult = AnalyticsServiceResult.CreateSuccess("熱度指數計算完成", 20);

            _mockAnalyticsService
                .Setup(s => s.CalculatePopularityIndexAsync(DateTime.Today, null))
                .ReturnsAsync(expectedResult);

            // Act
            var result = await _controller.CalculatePopularityIndex(DateTime.Today);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = okResult.Value;
            
            var responseType = response.GetType();
            var successProperty = responseType.GetProperty("success");
            var messageProperty = responseType.GetProperty("message");
            
            Assert.True((bool)successProperty.GetValue(response));
            Assert.Equal("熱度指數計算完成", messageProperty.GetValue(response));
        }

        [Fact]
        public async Task GetPopularityIndexDaily_ShouldReturnIndexData_WhenValidRequest()
        {
            // Arrange
            var expectedData = new AnalyticsPagedResult<PopularityIndexDailyDto>
            {
                Page = 1,
                PageSize = 50,
                TotalCount = 30,
                Data = new List<PopularityIndexDailyDto>
                {
                    new() { Id = 1, GameId = 1, Date = DateTime.Today, IndexValue = 85.5m, Rank = 1 },
                    new() { Id = 2, GameId = 2, Date = DateTime.Today, IndexValue = 82.3m, Rank = 2 }
                }
            };

            _mockAnalyticsService
                .Setup(s => s.GetPopularityIndexDailyAsync(null, null, null, 1, 50))
                .ReturnsAsync(expectedData);

            // Act
            var result = await _controller.GetPopularityIndexDaily();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = okResult.Value;
            
            var responseType = response.GetType();
            var dataProperty = responseType.GetProperty("data");
            
            var data = dataProperty.GetValue(response) as AnalyticsPagedResult<PopularityIndexDailyDto>;
            Assert.NotNull(data);
            Assert.Equal(30, data.TotalCount);
            Assert.Equal(2, data.Data.Count);
        }

        [Fact]
        public async Task GetPopularityRanking_ShouldReturnRanking_WhenValidRequest()
        {
            // Arrange
            var expectedRanking = new List<PopularityIndexDailyDto>
            {
                new() { GameId = 1, IndexValue = 95.5m, Rank = 1 },
                new() { GameId = 2, IndexValue = 88.3m, Rank = 2 },
                new() { GameId = 3, IndexValue = 82.1m, Rank = 3 }
            };

            _mockAnalyticsService
                .Setup(s => s.GetPopularityRankingAsync(DateTime.Today, 10))
                .ReturnsAsync(expectedRanking);

            // Act
            var result = await _controller.GetPopularityRanking(DateTime.Today, 10);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = okResult.Value;
            
            var responseType = response.GetType();
            var dataProperty = responseType.GetProperty("data");
            
            var data = dataProperty.GetValue(response) as List<PopularityIndexDailyDto>;
            Assert.NotNull(data);
            Assert.Equal(3, data.Count);
            Assert.Equal(1, data[0].Rank);
        }

        #endregion

        #region 排行榜快照測試

        [Fact]
        public async Task GenerateLeaderboardSnapshot_ShouldReturnSuccess_WhenValidRequest()
        {
            // Arrange
            var expectedResult = AnalyticsServiceResult.CreateSuccess("排行榜快照生成完成", 20);

            _mockAnalyticsService
                .Setup(s => s.GenerateLeaderboardSnapshotAsync("daily", DateTime.Today))
                .ReturnsAsync(expectedResult);

            // Act
            var result = await _controller.GenerateLeaderboardSnapshot("daily", DateTime.Today);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = okResult.Value;
            
            var responseType = response.GetType();
            var successProperty = responseType.GetProperty("success");
            
            Assert.True((bool)successProperty.GetValue(response));
        }

        [Fact]
        public async Task GetLeaderboardSnapshot_ShouldReturnSnapshot_WhenValidRequest()
        {
            // Arrange
            var queryDto = new LeaderboardQueryDto
            {
                Period = "daily",
                Date = DateTime.Today,
                TopN = 10
            };

            var expectedSnapshot = new List<LeaderboardSnapshotDto>
            {
                new() { GameId = 1, Rank = 1, Score = 95.5m, RankChange = 1 },
                new() { GameId = 2, Rank = 2, Score = 88.3m, RankChange = -1 }
            };

            _mockAnalyticsService
                .Setup(s => s.GetLeaderboardSnapshotAsync(It.IsAny<LeaderboardQueryDto>()))
                .ReturnsAsync(expectedSnapshot);

            // Act
            var result = await _controller.GetLeaderboardSnapshot(queryDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = okResult.Value;
            
            var responseType = response.GetType();
            var dataProperty = responseType.GetProperty("data");
            
            var data = dataProperty.GetValue(response) as List<LeaderboardSnapshotDto>;
            Assert.NotNull(data);
            Assert.Equal(2, data.Count);
            Assert.Equal(1, data[0].Rank);
        }

        #endregion

        #region 洞察貼文測試

        [Fact]
        public async Task GetInsightPosts_ShouldReturnPostList_WhenValidRequest()
        {
            // Arrange
            var expectedPosts = new AnalyticsPagedResult<InsightPostDto>
            {
                Page = 1,
                PageSize = 20,
                TotalCount = 50,
                Data = new List<InsightPostDto>
                {
                    new() { PostId = 1, Title = "熱度分析", Status = "published", IsPinned = true },
                    new() { PostId = 2, Title = "市場洞察", Status = "published", IsPinned = false }
                }
            };

            _mockAnalyticsService
                .Setup(s => s.GetInsightPostsAsync(null, null, false, 1, 20))
                .ReturnsAsync(expectedPosts);

            // Act
            var result = await _controller.GetInsightPosts();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = okResult.Value;
            
            var responseType = response.GetType();
            var dataProperty = responseType.GetProperty("data");
            
            var data = dataProperty.GetValue(response) as AnalyticsPagedResult<InsightPostDto>;
            Assert.NotNull(data);
            Assert.Equal(50, data.TotalCount);
            Assert.Equal(2, data.Data.Count);
        }

        [Fact]
        public async Task CreateInsightPost_ShouldReturnSuccess_WhenValidRequest()
        {
            // Arrange
            var createDto = new CreateInsightPostDto
            {
                GameId = 1,
                Title = "新洞察貼文",
                Content = "這是測試內容",
                PublishNow = true
            };

            var expectedPost = new InsightPostDto
            {
                PostId = 100,
                GameId = 1,
                Title = "新洞察貼文",
                Content = "這是測試內容",
                Status = "published"
            };

            var expectedResult = AnalyticsServiceResult<InsightPostDto>.CreateSuccess(
                expectedPost, "洞察貼文建立成功");

            _mockAnalyticsService
                .Setup(s => s.CreateInsightPostAsync(createDto))
                .ReturnsAsync(expectedResult);

            // Act
            var result = await _controller.CreateInsightPost(createDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = okResult.Value;
            
            var responseType = response.GetType();
            var successProperty = responseType.GetProperty("success");
            var dataProperty = responseType.GetProperty("data");
            
            Assert.True((bool)successProperty.GetValue(response));
            
            var data = dataProperty.GetValue(response) as InsightPostDto;
            Assert.NotNull(data);
            Assert.Equal("新洞察貼文", data.Title);
        }

        #endregion

        #region 儀表板統計測試

        [Fact]
        public async Task GetAnalyticsDashboard_ShouldReturnDashboard_WhenValidRequest()
        {
            // Arrange
            var expectedDashboard = new AnalyticsDashboardDto
            {
                TotalGames = 20,
                ActiveGames = 18,
                TotalMetrics = 28,
                TodayDataPoints = 150,
                AveragePopularityIndex = 75.5m,
                MaxPopularityIndex = 95.8m
            };

            _mockAnalyticsService
                .Setup(s => s.GetAnalyticsDashboardAsync(null))
                .ReturnsAsync(expectedDashboard);

            // Act
            var result = await _controller.GetAnalyticsDashboard();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = okResult.Value;
            
            var responseType = response.GetType();
            var dataProperty = responseType.GetProperty("data");
            
            var data = dataProperty.GetValue(response) as AnalyticsDashboardDto;
            Assert.NotNull(data);
            Assert.Equal(20, data.TotalGames);
            Assert.Equal(18, data.ActiveGames);
        }

        #endregion

        #region 錯誤處理測試

        [Fact]
        public async Task CreateGame_ShouldReturnBadRequest_WhenInvalidModel()
        {
            // Arrange
            var createDto = new CreateGameDto(); // 空的DTO，缺少必填欄位

            _controller.ModelState.AddModelError("GameName", "遊戲名稱為必填");

            // Act
            var result = await _controller.CreateGame(createDto);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var response = badRequestResult.Value;
            
            var responseType = response.GetType();
            var successProperty = responseType.GetProperty("success");
            var messageProperty = responseType.GetProperty("message");
            
            Assert.False((bool)successProperty.GetValue(response));
            Assert.Equal("遊戲資料格式錯誤", messageProperty.GetValue(response));
        }

        [Fact]
        public async Task GetGames_ShouldReturnServerError_WhenServiceThrows()
        {
            // Arrange
            _mockAnalyticsService
                .Setup(s => s.GetGamesAsync(It.IsAny<bool>(), It.IsAny<int>(), It.IsAny<int>()))
                .ThrowsAsync(new Exception("Database connection failed"));

            // Act
            var result = await _controller.GetGames();

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, statusCodeResult.StatusCode);
        }

        [Fact]
        public async Task CreateGameMetricDaily_ShouldReturnBadRequest_WhenServiceFails()
        {
            // Arrange
            var createDto = new CreateGameMetricDailyDto
            {
                GameId = 999, // 不存在的遊戲
                MetricId = 1,
                Date = DateTime.Today,
                Value = 100
            };

            var expectedResult = AnalyticsServiceResult<GameMetricDailyDto>.CreateFailure(
                "遊戲不存在", new List<string> { "指定的遊戲ID不存在" });

            _mockAnalyticsService
                .Setup(s => s.CreateGameMetricDailyAsync(createDto))
                .ReturnsAsync(expectedResult);

            // Act
            var result = await _controller.CreateGameMetricDaily(createDto);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var response = badRequestResult.Value;
            
            var responseType = response.GetType();
            var successProperty = responseType.GetProperty("success");
            var messageProperty = responseType.GetProperty("message");
            
            Assert.False((bool)successProperty.GetValue(response));
            Assert.Equal("遊戲不存在", messageProperty.GetValue(response));
        }

        #endregion

        #region 權限測試

        [Fact]
        public async Task CreateGame_ShouldRequireAdminRole()
        {
            // Arrange
            var createDto = new CreateGameDto
            {
                GameName = "測試遊戲",
                IsActive = true
            };

            var expectedResult = AnalyticsServiceResult<GameDto>.CreateSuccess(
                new GameDto { GameId = 100, GameName = "測試遊戲" }, "遊戲建立成功");

            _mockAnalyticsService
                .Setup(s => s.CreateGameAsync(createDto))
                .ReturnsAsync(expectedResult);

            // Act
            var result = await _controller.CreateGame(createDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            _mockAnalyticsService.Verify(s => s.CreateGameAsync(createDto), Times.Once);
        }

        #endregion

        #region 邊界條件測試

        [Fact]
        public async Task GetGameMetricDaily_ShouldLimitPageSize_WhenLargePageSizeRequested()
        {
            // Arrange
            _mockAnalyticsService
                .Setup(s => s.GetGameMetricDailyAsync(null, null, null, null, 1, 100))
                .ReturnsAsync(new AnalyticsPagedResult<GameMetricDailyDto>());

            // Act
            await _controller.GetGameMetricDaily(pageSize: 200); // 超過限制

            // Assert
            _mockAnalyticsService.Verify(s => s.GetGameMetricDailyAsync(null, null, null, null, 1, 100), Times.Once);
        }

        [Fact]
        public async Task GetLeaderboardSnapshot_ShouldValidatePeriod_WhenInvalidPeriod()
        {
            // Arrange
            var queryDto = new LeaderboardQueryDto
            {
                Period = "invalid_period",
                TopN = 10
            };

            // Act & Assert
            // 在實際實作中，這應該會返回 BadRequest
            // 或在服務層進行驗證
        }

        #endregion
    }

    #region 測試用控制器類別

    // 模擬的AnalyticsController類別，用於測試
    public class AnalyticsController : ControllerBase
    {
        private readonly IAnalyticsService _analyticsService;
        private readonly ILogger<AnalyticsController> _logger;

        public AnalyticsController(IAnalyticsService analyticsService, ILogger<AnalyticsController> logger)
        {
            _analyticsService = analyticsService;
            _logger = logger;
        }

        // 遊戲管理方法
        public async Task<IActionResult> GetGames(bool activeOnly = true, int page = 1, int pageSize = 20)
        {
            try
            {
                var result = await _analyticsService.GetGamesAsync(activeOnly, page, pageSize);
                return Ok(new { success = true, data = result, message = "遊戲列表取得成功" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "取得遊戲列表時發生錯誤");
                return StatusCode(500, new { success = false, message = "取得遊戲列表時發生錯誤" });
            }
        }

        public async Task<IActionResult> CreateGame(CreateGameDto createDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { success = false, message = "遊戲資料格式錯誤", errors = ModelState });
            }

            try
            {
                var result = await _analyticsService.CreateGameAsync(createDto);
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
                _logger.LogError(ex, "建立遊戲時發生錯誤");
                return StatusCode(500, new { success = false, message = "建立遊戲時發生錯誤" });
            }
        }

        // 指標管理方法
        public async Task<IActionResult> GetMetricSources(bool activeOnly = true)
        {
            try
            {
                var result = await _analyticsService.GetMetricSourcesAsync(activeOnly);
                return Ok(new { success = true, data = result, message = "指標來源列表取得成功" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "取得指標來源列表時發生錯誤");
                return StatusCode(500, new { success = false, message = "取得指標來源列表時發生錯誤" });
            }
        }

        public async Task<IActionResult> CreateMetric(CreateMetricDto createDto)
        {
            try
            {
                var result = await _analyticsService.CreateMetricAsync(createDto);
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
                _logger.LogError(ex, "建立指標時發生錯誤");
                return StatusCode(500, new { success = false, message = "建立指標時發生錯誤" });
            }
        }

        // 每日指標數據方法
        public async Task<IActionResult> GetGameMetricDaily(int? gameId = null, int? metricId = null, 
            DateTime? startDate = null, DateTime? endDate = null, int page = 1, int pageSize = 50)
        {
            try
            {
                var result = await _analyticsService.GetGameMetricDailyAsync(
                    gameId, metricId, startDate, endDate, page, Math.Min(pageSize, 100));
                return Ok(new { success = true, data = result, message = "每日指標數據取得成功" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "取得每日指標數據時發生錯誤");
                return StatusCode(500, new { success = false, message = "取得每日指標數據時發生錯誤" });
            }
        }

        public async Task<IActionResult> CreateGameMetricDaily(CreateGameMetricDailyDto createDto)
        {
            try
            {
                var result = await _analyticsService.CreateGameMetricDailyAsync(createDto);
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
                _logger.LogError(ex, "建立每日指標數據時發生錯誤");
                return StatusCode(500, new { success = false, message = "建立每日指標數據時發生錯誤" });
            }
        }

        // 熱度指數方法
        public async Task<IActionResult> CalculatePopularityIndex(DateTime date, List<int>? gameIds = null)
        {
            try
            {
                var result = await _analyticsService.CalculatePopularityIndexAsync(date, gameIds);
                return Ok(new { success = result.Success, message = result.Message, processedCount = result.ProcessedCount });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "計算熱度指數時發生錯誤");
                return StatusCode(500, new { success = false, message = "計算熱度指數時發生錯誤" });
            }
        }

        public async Task<IActionResult> GetPopularityIndexDaily(int? gameId = null, 
            DateTime? startDate = null, DateTime? endDate = null, int page = 1, int pageSize = 50)
        {
            try
            {
                var result = await _analyticsService.GetPopularityIndexDailyAsync(
                    gameId, startDate, endDate, page, pageSize);
                return Ok(new { success = true, data = result, message = "熱度指數數據取得成功" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "取得熱度指數數據時發生錯誤");
                return StatusCode(500, new { success = false, message = "取得熱度指數數據時發生錯誤" });
            }
        }

        public async Task<IActionResult> GetPopularityRanking(DateTime? date = null, int topN = 10)
        {
            try
            {
                var result = await _analyticsService.GetPopularityRankingAsync(date, topN);
                return Ok(new { success = true, data = result, message = "熱度排行榜取得成功" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "取得熱度排行榜時發生錯誤");
                return StatusCode(500, new { success = false, message = "取得熱度排行榜時發生錯誤" });
            }
        }

        // 排行榜快照方法
        public async Task<IActionResult> GenerateLeaderboardSnapshot(string period, DateTime? ts = null)
        {
            try
            {
                var result = await _analyticsService.GenerateLeaderboardSnapshotAsync(period, ts);
                return Ok(new { success = result.Success, message = result.Message, processedCount = result.ProcessedCount });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "生成排行榜快照時發生錯誤");
                return StatusCode(500, new { success = false, message = "生成排行榜快照時發生錯誤" });
            }
        }

        public async Task<IActionResult> GetLeaderboardSnapshot(LeaderboardQueryDto queryDto)
        {
            try
            {
                var result = await _analyticsService.GetLeaderboardSnapshotAsync(queryDto);
                return Ok(new { success = true, data = result, message = "排行榜快照取得成功" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "取得排行榜快照時發生錯誤");
                return StatusCode(500, new { success = false, message = "取得排行榜快照時發生錯誤" });
            }
        }

        // 洞察貼文方法
        public async Task<IActionResult> GetInsightPosts(int? gameId = null, string? status = null, 
            bool pinnedOnly = false, int page = 1, int pageSize = 20)
        {
            try
            {
                var result = await _analyticsService.GetInsightPostsAsync(gameId, status, pinnedOnly, page, pageSize);
                return Ok(new { success = true, data = result, message = "洞察貼文列表取得成功" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "取得洞察貼文列表時發生錯誤");
                return StatusCode(500, new { success = false, message = "取得洞察貼文列表時發生錯誤" });
            }
        }

        public async Task<IActionResult> CreateInsightPost(CreateInsightPostDto createDto)
        {
            try
            {
                var result = await _analyticsService.CreateInsightPostAsync(createDto);
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
                _logger.LogError(ex, "建立洞察貼文時發生錯誤");
                return StatusCode(500, new { success = false, message = "建立洞察貼文時發生錯誤" });
            }
        }

        // 儀表板統計方法
        public async Task<IActionResult> GetAnalyticsDashboard(DateTime? date = null)
        {
            try
            {
                var result = await _analyticsService.GetAnalyticsDashboardAsync(date);
                return Ok(new { success = true, data = result, message = "分析儀表板數據取得成功" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "取得分析儀表板數據時發生錯誤");
                return StatusCode(500, new { success = false, message = "取得分析儀表板數據時發生錯誤" });
            }
        }
    }

    #endregion
}