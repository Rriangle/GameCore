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
    /// 小遊戲控制器測試類別
    /// 測試所有小遊戲相關API的功能和邊界條件
    /// 驗證每日次數限制、關卡進度、寵物整合、獎勵發放等完整邏輯
    /// </summary>
    public class MiniGameControllerTests
    {
        private readonly Mock<IMiniGameService> _mockMiniGameService;
        private readonly Mock<ILogger<MiniGameController>> _mockLogger;
        private readonly MiniGameController _controller;

        public MiniGameControllerTests()
        {
            _mockMiniGameService = new Mock<IMiniGameService>();
            _mockLogger = new Mock<ILogger<MiniGameController>>();
            _controller = new MiniGameController(_mockMiniGameService.Object, _mockLogger.Object);

            // 設定模擬的使用者身份
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, "123"),
                new Claim(ClaimTypes.Name, "testuser")
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

        #region 遊戲資格檢查測試

        [Fact]
        public async Task CheckEligibility_ShouldReturnCanPlay_WhenUserMeetsAllConditions()
        {
            // Arrange
            var expectedEligibility = new MiniGameEligibilityDto
            {
                CanPlay = true,
                Message = "可以開始冒險！",
                TodayPlayCount = 1,
                DailyLimit = 3,
                PetHealthy = true,
                BlockingReasons = new List<string>(),
                SuggestedActions = new List<string>()
            };

            _mockMiniGameService
                .Setup(s => s.CheckGameEligibilityAsync(123))
                .ReturnsAsync(expectedEligibility);

            // Act
            var result = await _controller.CheckEligibility();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = okResult.Value;
            
            Assert.NotNull(response);
            
            var responseType = response.GetType();
            var successProperty = responseType.GetProperty("success");
            var dataProperty = responseType.GetProperty("data");
            var messageProperty = responseType.GetProperty("message");
            
            Assert.True((bool)successProperty.GetValue(response));
            Assert.NotNull(dataProperty.GetValue(response));
            Assert.Equal("可以開始冒險！", messageProperty.GetValue(response));
        }

        [Fact]
        public async Task CheckEligibility_ShouldReturnCannotPlay_WhenDailyLimitReached()
        {
            // Arrange
            var expectedEligibility = new MiniGameEligibilityDto
            {
                CanPlay = false,
                Message = "今日遊戲次數已達上限 (3 次)",
                TodayPlayCount = 3,
                DailyLimit = 3,
                PetHealthy = true,
                BlockingReasons = new List<string> { "每日遊戲次數已用完" },
                NextPlayTime = DateTime.UtcNow.AddHours(8)
            };

            _mockMiniGameService
                .Setup(s => s.CheckGameEligibilityAsync(123))
                .ReturnsAsync(expectedEligibility);

            // Act
            var result = await _controller.CheckEligibility();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = okResult.Value;
            
            var responseType = response.GetType();
            var dataProperty = responseType.GetProperty("data");
            var data = dataProperty.GetValue(response) as MiniGameEligibilityDto;
            
            Assert.NotNull(data);
            Assert.False(data.CanPlay);
            Assert.Equal(3, data.TodayPlayCount);
            Assert.Contains("每日遊戲次數已用完", data.BlockingReasons);
        }

        [Fact]
        public async Task CheckEligibility_ShouldReturnCannotPlay_WhenPetUnhealthy()
        {
            // Arrange
            var expectedEligibility = new MiniGameEligibilityDto
            {
                CanPlay = false,
                Message = "寵物狀態不佳，無法進行冒險",
                TodayPlayCount = 1,
                DailyLimit = 3,
                PetHealthy = false,
                BlockingReasons = new List<string> { "寵物健康度為0", "寵物飢餓值為0" },
                SuggestedActions = new List<string> { "與寵物互動提升健康度", "餵食寵物" }
            };

            _mockMiniGameService
                .Setup(s => s.CheckGameEligibilityAsync(123))
                .ReturnsAsync(expectedEligibility);

            // Act
            var result = await _controller.CheckEligibility();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = okResult.Value;
            
            var responseType = response.GetType();
            var dataProperty = responseType.GetProperty("data");
            var data = dataProperty.GetValue(response) as MiniGameEligibilityDto;
            
            Assert.NotNull(data);
            Assert.False(data.CanPlay);
            Assert.False(data.PetHealthy);
            Assert.Contains("寵物健康度為0", data.BlockingReasons);
            Assert.Contains("餵食寵物", data.SuggestedActions);
        }

        #endregion

        #region 遊戲開始測試

        [Fact]
        public async Task StartGame_ShouldReturnSuccess_WhenAllConditionsMet()
        {
            // Arrange
            var expectedSession = new MiniGameSessionDto
            {
                GameId = 456,
                UserId = 123,
                Level = 2,
                MonsterCount = 8,
                SpeedMultiplier = 1.2m,
                StartTime = DateTime.UtcNow,
                Status = "進行中",
                GameTips = "速度加快了，保持專注！"
            };

            var expectedResult = new ServiceResult<MiniGameSessionDto>
            {
                Success = true,
                Message = "冒險開始！關卡 2",
                Data = expectedSession
            };

            _mockMiniGameService
                .Setup(s => s.StartGameAsync(123))
                .ReturnsAsync(expectedResult);

            // Act
            var result = await _controller.StartGame();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = okResult.Value;
            
            var responseType = response.GetType();
            var successProperty = responseType.GetProperty("success");
            var dataProperty = responseType.GetProperty("data");
            var messageProperty = responseType.GetProperty("message");
            
            Assert.True((bool)successProperty.GetValue(response));
            Assert.NotNull(dataProperty.GetValue(response));
            Assert.Equal("冒險開始！關卡 2", messageProperty.GetValue(response));
            
            var data = dataProperty.GetValue(response) as MiniGameSessionDto;
            Assert.NotNull(data);
            Assert.Equal(456, data.GameId);
            Assert.Equal(2, data.Level);
            Assert.Equal(8, data.MonsterCount);
        }

        [Fact]
        public async Task StartGame_ShouldReturnBadRequest_WhenConditionsNotMet()
        {
            // Arrange
            var expectedResult = new ServiceResult<MiniGameSessionDto>
            {
                Success = false,
                Message = "今日遊戲次數已達上限",
                Errors = new List<string> { "每日遊戲次數已用完", "請明日再來" }
            };

            _mockMiniGameService
                .Setup(s => s.StartGameAsync(123))
                .ReturnsAsync(expectedResult);

            // Act
            var result = await _controller.StartGame();

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var response = badRequestResult.Value;
            
            var responseType = response.GetType();
            var successProperty = responseType.GetProperty("success");
            var messageProperty = responseType.GetProperty("message");
            var errorsProperty = responseType.GetProperty("errors");
            
            Assert.False((bool)successProperty.GetValue(response));
            Assert.Equal("今日遊戲次數已達上限", messageProperty.GetValue(response));
            
            var errors = errorsProperty.GetValue(response) as List<string>;
            Assert.NotNull(errors);
            Assert.Contains("每日遊戲次數已用完", errors);
        }

        #endregion

        #region 遊戲完成測試

        [Fact]
        public async Task FinishGame_ShouldReturnSuccess_WhenGameCompletedSuccessfully()
        {
            // Arrange
            var finishRequest = new FinishGameDto
            {
                IsVictory = true,
                DurationSeconds = 300,
                MonstersDefeated = 8,
                FinalScore = 1200,
                GameNotes = "完美通關！"
            };

            var expectedResult = new ServiceResult<MiniGameResultDto>
            {
                Success = true,
                Message = "恭喜！成功通過關卡 2！",
                Data = new MiniGameResultDto
                {
                    GameId = 456,
                    IsVictory = true,
                    ResultMessage = "恭喜！成功通過關卡 2！",
                    Reward = new GameRewardDto { Experience = 200, Points = 20 },
                    NextLevel = 3,
                    CanContinue = true,
                    RemainingPlaysToday = 2
                }
            };

            _mockMiniGameService
                .Setup(s => s.FinishGameAsync(123, 456, finishRequest))
                .ReturnsAsync(expectedResult);

            // Act
            var result = await _controller.FinishGame(456, finishRequest);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = okResult.Value;
            
            var responseType = response.GetType();
            var successProperty = responseType.GetProperty("success");
            var dataProperty = responseType.GetProperty("data");
            
            Assert.True((bool)successProperty.GetValue(response));
            
            var data = dataProperty.GetValue(response) as MiniGameResultDto;
            Assert.NotNull(data);
            Assert.True(data.IsVictory);
            Assert.Equal(3, data.NextLevel);
            Assert.True(data.CanContinue);
            Assert.Equal(200, data.Reward.Experience);
        }

        [Fact]
        public async Task FinishGame_ShouldReturnSuccess_WhenGameLost()
        {
            // Arrange
            var finishRequest = new FinishGameDto
            {
                IsVictory = false,
                DurationSeconds = 180,
                MonstersDefeated = 3,
                FinalScore = 450
            };

            var expectedResult = new ServiceResult<MiniGameResultDto>
            {
                Success = true,
                Message = "很可惜，關卡 2 失敗了，再接再厲！",
                Data = new MiniGameResultDto
                {
                    GameId = 456,
                    IsVictory = false,
                    NextLevel = 2, // 失敗保持原關卡
                    Reward = new GameRewardDto { Experience = 0, Points = 0 } // 失敗無獎勵
                }
            };

            _mockMiniGameService
                .Setup(s => s.FinishGameAsync(123, 456, finishRequest))
                .ReturnsAsync(expectedResult);

            // Act
            var result = await _controller.FinishGame(456, finishRequest);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = okResult.Value;
            
            var responseType = response.GetType();
            var dataProperty = responseType.GetProperty("data");
            
            var data = dataProperty.GetValue(response) as MiniGameResultDto;
            Assert.NotNull(data);
            Assert.False(data.IsVictory);
            Assert.Equal(2, data.NextLevel); // 失敗時關卡不變
            Assert.Equal(0, data.Reward.Experience); // 失敗無獎勵
        }

        [Fact]
        public async Task FinishGame_ShouldReturnBadRequest_WhenInvalidModel()
        {
            // Arrange
            var finishRequest = new FinishGameDto
            {
                DurationSeconds = -1, // 無效時間
                MonstersDefeated = -5 // 無效數量
            };

            _controller.ModelState.AddModelError("DurationSeconds", "Duration must be positive");

            // Act
            var result = await _controller.FinishGame(456, finishRequest);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var response = badRequestResult.Value;
            
            var responseType = response.GetType();
            var successProperty = responseType.GetProperty("success");
            var messageProperty = responseType.GetProperty("message");
            
            Assert.False((bool)successProperty.GetValue(response));
            Assert.Equal("資料格式錯誤", messageProperty.GetValue(response));
        }

        #endregion

        #region 遊戲中斷測試

        [Fact]
        public async Task AbortGame_ShouldReturnSuccess_WhenGameAbortedSuccessfully()
        {
            // Arrange
            var abortRequest = new AbortGameRequest { Reason = "網路連線中斷" };
            var expectedResult = new ServiceResult
            {
                Success = true,
                Message = "遊戲已中斷"
            };

            _mockMiniGameService
                .Setup(s => s.AbortGameAsync(123, 456, "網路連線中斷"))
                .ReturnsAsync(expectedResult);

            // Act
            var result = await _controller.AbortGame(456, abortRequest);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = okResult.Value;
            
            var responseType = response.GetType();
            var successProperty = responseType.GetProperty("success");
            var messageProperty = responseType.GetProperty("message");
            
            Assert.True((bool)successProperty.GetValue(response));
            Assert.Equal("遊戲已中斷", messageProperty.GetValue(response));
        }

        [Fact]
        public async Task AbortGame_ShouldUseDefaultReason_WhenNoReasonProvided()
        {
            // Arrange
            var expectedResult = new ServiceResult { Success = true, Message = "遊戲已中斷" };

            _mockMiniGameService
                .Setup(s => s.AbortGameAsync(123, 456, "使用者中斷"))
                .ReturnsAsync(expectedResult);

            // Act
            var result = await _controller.AbortGame(456, null);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            _mockMiniGameService.Verify(s => s.AbortGameAsync(123, 456, "使用者中斷"), Times.Once);
        }

        #endregion

        #region 遊戲記錄測試

        [Fact]
        public async Task GetRecords_ShouldReturnPagedRecords_WhenValidRequest()
        {
            // Arrange
            var expectedRecords = new PagedResult<MiniGameRecordDto>
            {
                Page = 1,
                PageSize = 20,
                TotalCount = 45,
                Data = new List<MiniGameRecordDto>
                {
                    new() { GameId = 1, Level = 1, IsVictory = true, ExpGained = 100, PointsGained = 10 },
                    new() { GameId = 2, Level = 2, IsVictory = false, ExpGained = 0, PointsGained = 0 },
                    new() { GameId = 3, Level = 1, IsVictory = true, ExpGained = 100, PointsGained = 10 }
                }
            };

            _mockMiniGameService
                .Setup(s => s.GetGameRecordsAsync(123, It.IsAny<GetGameRecordsDto>()))
                .ReturnsAsync(expectedRecords);

            // Act
            var result = await _controller.GetRecords();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = okResult.Value;
            
            var responseType = response.GetType();
            var dataProperty = responseType.GetProperty("data");
            
            var data = dataProperty.GetValue(response) as PagedResult<MiniGameRecordDto>;
            Assert.NotNull(data);
            Assert.Equal(45, data.TotalCount);
            Assert.Equal(3, data.Data.Count);
        }

        [Fact]
        public async Task GetRecords_ShouldApplyFilters_WhenParametersProvided()
        {
            // Arrange
            var from = DateTime.Now.AddDays(-7);
            var to = DateTime.Now;
            var level = 2;
            var result = true;

            _mockMiniGameService
                .Setup(s => s.GetGameRecordsAsync(123, It.Is<GetGameRecordsDto>(req => 
                    req.FromDate == from && 
                    req.ToDate == to && 
                    req.Level == level && 
                    req.IsVictory == result)))
                .ReturnsAsync(new PagedResult<MiniGameRecordDto>());

            // Act
            await _controller.GetRecords(from, to, result, level);

            // Assert
            _mockMiniGameService.Verify(s => s.GetGameRecordsAsync(123, It.Is<GetGameRecordsDto>(req => 
                req.FromDate == from && 
                req.ToDate == to && 
                req.Level == level && 
                req.IsVictory == result)), Times.Once);
        }

        #endregion

        #region 統計測試

        [Fact]
        public async Task GetStatistics_ShouldReturnComprehensiveStats()
        {
            // Arrange
            var expectedStats = new MiniGameStatsDto
            {
                TotalGames = 25,
                TotalVictories = 18,
                TotalDefeats = 7,
                HighestLevel = 5,
                CurrentLevel = 3,
                TotalExperienceEarned = 2500,
                TotalPointsEarned = 250,
                BestScore = 1800,
                AverageScore = 1200,
                LongestWinStreak = 5,
                CurrentWinStreak = 2
            };

            _mockMiniGameService
                .Setup(s => s.GetGameStatisticsAsync(123))
                .ReturnsAsync(expectedStats);

            // Act
            var result = await _controller.GetStatistics();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = okResult.Value;
            
            var responseType = response.GetType();
            var dataProperty = responseType.GetProperty("data");
            
            var data = dataProperty.GetValue(response) as MiniGameStatsDto;
            Assert.NotNull(data);
            Assert.Equal(25, data.TotalGames);
            Assert.Equal(72.0, data.WinRate); // 18/25 * 100
            Assert.Equal(5, data.HighestLevel);
            Assert.Equal(3, data.CurrentLevel);
        }

        [Fact]
        public async Task GetDailyStatus_ShouldReturnTodayGameInfo()
        {
            // Arrange
            var expectedStatus = new DailyGameStatusDto
            {
                Date = DateTime.Today,
                TodayPlayCount = 2,
                DailyLimit = 3,
                CanPlay = true,
                TodayTotalExp = 300,
                TodayTotalPoints = 30,
                TodayVictories = 2,
                NextResetTime = DateTime.Today.AddDays(1)
            };

            _mockMiniGameService
                .Setup(s => s.GetDailyGameStatusAsync(123))
                .ReturnsAsync(expectedStatus);

            // Act
            var result = await _controller.GetDailyStatus();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = okResult.Value;
            
            var responseType = response.GetType();
            var dataProperty = responseType.GetProperty("data");
            
            var data = dataProperty.GetValue(response) as DailyGameStatusDto;
            Assert.NotNull(data);
            Assert.Equal(2, data.TodayPlayCount);
            Assert.Equal(1, data.RemainingPlays);
            Assert.True(data.CanPlay);
            Assert.Equal(300, data.TodayTotalExp);
        }

        #endregion

        #region 關卡設定測試

        [Fact]
        public async Task GetLevels_ShouldReturnLevelConfigurations()
        {
            // Arrange
            var expectedLevels = new List<GameLevelConfigDto>
            {
                new() { Level = 1, MonsterCount = 6, SpeedMultiplier = 1.0m, 
                        VictoryReward = new GameRewardDto { Experience = 100, Points = 10 } },
                new() { Level = 2, MonsterCount = 8, SpeedMultiplier = 1.2m, 
                        VictoryReward = new GameRewardDto { Experience = 200, Points = 20 } },
                new() { Level = 3, MonsterCount = 10, SpeedMultiplier = 1.5m, 
                        VictoryReward = new GameRewardDto { Experience = 300, Points = 30 } }
            };

            _mockMiniGameService
                .Setup(s => s.GetLevelConfigsAsync())
                .ReturnsAsync(expectedLevels);

            // Act
            var result = await _controller.GetLevels();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = okResult.Value;
            
            var responseType = response.GetType();
            var dataProperty = responseType.GetProperty("data");
            
            var data = dataProperty.GetValue(response) as List<GameLevelConfigDto>;
            Assert.NotNull(data);
            Assert.Equal(3, data.Count);
            Assert.Equal(6, data[0].MonsterCount);
            Assert.Equal(1.2m, data[1].SpeedMultiplier);
            Assert.Equal(300, data[2].VictoryReward.Experience);
        }

        [Fact]
        public async Task GetCurrentLevel_ShouldReturnUserCurrentLevel()
        {
            // Arrange
            _mockMiniGameService
                .Setup(s => s.GetUserCurrentLevelAsync(123))
                .ReturnsAsync(5);

            // Act
            var result = await _controller.GetCurrentLevel();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = okResult.Value;
            
            var responseType = response.GetType();
            var dataProperty = responseType.GetProperty("data");
            var data = dataProperty.GetValue(response);
            
            var currentLevelProperty = data.GetType().GetProperty("currentLevel");
            Assert.Equal(5, currentLevelProperty.GetValue(data));
        }

        #endregion

        #region 排行榜測試

        [Fact]
        public async Task GetRankings_ShouldReturnRankings_WhenValidType()
        {
            // Arrange
            var expectedRankings = new List<MiniGameRankingDto>
            {
                new() { Rank = 1, Username = "Player1", RankingValue = 20, RankingDescription = "最高關卡 20" },
                new() { Rank = 2, Username = "Player2", RankingValue = 18, RankingDescription = "最高關卡 18" },
                new() { Rank = 3, Username = "Player3", RankingValue = 15, RankingDescription = "最高關卡 15" }
            };

            _mockMiniGameService
                .Setup(s => s.GetGameRankingsAsync(GameRankingType.HighestLevel, 50))
                .ReturnsAsync(expectedRankings);

            // Act
            var result = await _controller.GetRankings("HighestLevel", 50);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = okResult.Value;
            
            var responseType = response.GetType();
            var dataProperty = responseType.GetProperty("data");
            
            var data = dataProperty.GetValue(response) as List<MiniGameRankingDto>;
            Assert.NotNull(data);
            Assert.Equal(3, data.Count);
            Assert.Equal("Player1", data[0].Username);
            Assert.Equal(20, data[0].RankingValue);
        }

        [Fact]
        public async Task GetRankings_ShouldReturnBadRequest_WhenInvalidType()
        {
            // Act
            var result = await _controller.GetRankings("InvalidType", 50);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var response = badRequestResult.Value;
            
            var responseType = response.GetType();
            var messageProperty = responseType.GetProperty("message");
            
            Assert.Equal("不支援的排行類型", messageProperty.GetValue(response));
        }

        #endregion

        #region 錯誤處理測試

        [Fact]
        public async Task CheckEligibility_ShouldReturnServerError_WhenServiceThrows()
        {
            // Arrange
            _mockMiniGameService
                .Setup(s => s.CheckGameEligibilityAsync(123))
                .ThrowsAsync(new Exception("Database connection failed"));

            // Act
            var result = await _controller.CheckEligibility();

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, statusCodeResult.StatusCode);
        }

        [Fact]
        public async Task StartGame_ShouldReturnServerError_WhenServiceThrows()
        {
            // Arrange
            _mockMiniGameService
                .Setup(s => s.StartGameAsync(123))
                .ThrowsAsync(new Exception("Service error"));

            // Act
            var result = await _controller.StartGame();

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, statusCodeResult.StatusCode);
        }

        #endregion

        #region 邊界條件測試

        [Fact]
        public async Task GetRecords_ShouldLimitPageSize_WhenLargePageSizeRequested()
        {
            // Arrange
            _mockMiniGameService
                .Setup(s => s.GetGameRecordsAsync(123, It.Is<GetGameRecordsDto>(req => req.PageSize == 100)))
                .ReturnsAsync(new PagedResult<MiniGameRecordDto>());

            // Act
            await _controller.GetRecords(pageSize: 200); // 超過限制

            // Assert
            _mockMiniGameService.Verify(s => s.GetGameRecordsAsync(123, It.Is<GetGameRecordsDto>(req => req.PageSize == 100)), Times.Once);
        }

        [Fact]
        public async Task GetRankings_ShouldLimitResults_WhenLargeLimitRequested()
        {
            // Arrange
            _mockMiniGameService
                .Setup(s => s.GetGameRankingsAsync(GameRankingType.Level, 100))
                .ReturnsAsync(new List<MiniGameRankingDto>());

            // Act
            await _controller.GetRankings("Level", 200); // 超過限制

            // Assert
            _mockMiniGameService.Verify(s => s.GetGameRankingsAsync(GameRankingType.Level, 100), Times.Once);
        }

        #endregion
    }
}