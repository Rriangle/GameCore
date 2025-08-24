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
    /// 虛擬寵物控制器測試類別
    /// 測試所有寵物相關API的功能和邊界條件
    /// 驗證5維屬性管理、互動行為、等級系統、換色功能等完整邏輯
    /// </summary>
    public class VirtualPetControllerTests
    {
        private readonly Mock<IPetService> _mockPetService;
        private readonly Mock<ILogger<VirtualPetController>> _mockLogger;
        private readonly VirtualPetController _controller;

        public VirtualPetControllerTests()
        {
            _mockPetService = new Mock<IPetService>();
            _mockLogger = new Mock<ILogger<VirtualPetController>>();
            _controller = new VirtualPetController(_mockPetService.Object, _mockLogger.Object);

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

        #region 寵物基本管理測試

        [Fact]
        public async Task GetPet_ShouldReturnPetData_WhenUserHasPet()
        {
            // Arrange
            var expectedPet = new PetDto
            {
                PetId = 1,
                UserId = 123,
                PetName = "小可愛",
                Level = 5,
                Experience = 500,
                Hunger = 80,
                Mood = 75,
                Stamina = 90,
                Cleanliness = 85,
                Health = 82,
                SkinColor = "#ADD8E6",
                BackgroundColor = "粉藍",
                CanAdventure = true,
                PetStatus = "快樂"
            };

            _mockPetService
                .Setup(s => s.GetUserPetAsync(123))
                .ReturnsAsync(expectedPet);

            // Act
            var result = await _controller.GetPet();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = okResult.Value;
            
            Assert.NotNull(response);
            
            var responseType = response.GetType();
            var successProperty = responseType.GetProperty("success");
            var hasPetProperty = responseType.GetProperty("hasPet");
            var dataProperty = responseType.GetProperty("data");
            
            Assert.True((bool)successProperty.GetValue(response));
            Assert.True((bool)hasPetProperty.GetValue(response));
            Assert.NotNull(dataProperty.GetValue(response));
        }

        [Fact]
        public async Task GetPet_ShouldReturnNoPetMessage_WhenUserHasNoPet()
        {
            // Arrange
            _mockPetService
                .Setup(s => s.GetUserPetAsync(123))
                .ReturnsAsync((PetDto?)null);

            // Act
            var result = await _controller.GetPet();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = okResult.Value;
            
            var responseType = response.GetType();
            var successProperty = responseType.GetProperty("success");
            var hasPetProperty = responseType.GetProperty("hasPet");
            var messageProperty = responseType.GetProperty("message");
            
            Assert.True((bool)successProperty.GetValue(response));
            Assert.False((bool)hasPetProperty.GetValue(response));
            Assert.Contains("還沒有寵物", messageProperty.GetValue(response).ToString());
        }

        [Fact]
        public async Task CreatePet_ShouldReturnSuccess_WhenValidRequest()
        {
            // Arrange
            var createRequest = new CreatePetRequest { PetName = "史萊姆王" };
            var expectedPet = new PetDto
            {
                PetId = 1,
                UserId = 123,
                PetName = "史萊姆王",
                Level = 1,
                Experience = 0,
                Hunger = 100,
                Mood = 100,
                Stamina = 100,
                Cleanliness = 100,
                Health = 100,
                SkinColor = "#ADD8E6",
                BackgroundColor = "粉藍"
            };

            _mockPetService
                .Setup(s => s.CreatePetAsync(123, "史萊姆王"))
                .ReturnsAsync(expectedPet);

            // Act
            var result = await _controller.CreatePet(createRequest);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = okResult.Value;
            
            var responseType = response.GetType();
            var successProperty = responseType.GetProperty("success");
            var dataProperty = responseType.GetProperty("data");
            var messageProperty = responseType.GetProperty("message");
            
            Assert.True((bool)successProperty.GetValue(response));
            Assert.NotNull(dataProperty.GetValue(response));
            Assert.Contains("建立成功", messageProperty.GetValue(response).ToString());
        }

        [Fact]
        public async Task CreatePet_ShouldReturnBadRequest_WhenUserAlreadyHasPet()
        {
            // Arrange
            var createRequest = new CreatePetRequest { PetName = "史萊姆王" };

            _mockPetService
                .Setup(s => s.CreatePetAsync(123, "史萊姆王"))
                .ThrowsAsync(new InvalidOperationException("每位會員僅可擁有一隻寵物"));

            // Act
            var result = await _controller.CreatePet(createRequest);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var response = badRequestResult.Value;
            
            var responseType = response.GetType();
            var successProperty = responseType.GetProperty("success");
            var messageProperty = responseType.GetProperty("message");
            
            Assert.False((bool)successProperty.GetValue(response));
            Assert.Equal("每位會員僅可擁有一隻寵物", messageProperty.GetValue(response));
        }

        [Fact]
        public async Task UpdatePetProfile_ShouldReturnSuccess_WhenValidRequest()
        {
            // Arrange
            var updateRequest = new UpdatePetProfileDto { PetName = "新名字" };
            var expectedResult = new ServiceResult<PetDto>
            {
                Success = true,
                Message = "寵物資料更新成功",
                Data = new PetDto { PetName = "新名字" }
            };

            _mockPetService
                .Setup(s => s.UpdatePetProfileAsync(123, updateRequest))
                .ReturnsAsync(expectedResult);

            // Act
            var result = await _controller.UpdatePetProfile(updateRequest);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = okResult.Value;
            
            var responseType = response.GetType();
            var successProperty = responseType.GetProperty("success");
            var messageProperty = responseType.GetProperty("message");
            
            Assert.True((bool)successProperty.GetValue(response));
            Assert.Equal("寵物資料更新成功", messageProperty.GetValue(response));
        }

        #endregion

        #region 寵物互動行為測試

        [Fact]
        public async Task FeedPet_ShouldReturnInteractionResult_WhenSuccessful()
        {
            // Arrange
            var expectedResult = new PetInteractionResultDto
            {
                Success = true,
                Message = "成功餵食寵物！",
                InteractionType = "feed",
                BeforeStats = new PetStatsSnapshot { Hunger = 70 },
                AfterStats = new PetStatsSnapshot { Hunger = 80 },
                StatsChange = new PetStatsChange { HungerChange = 10 },
                ExperienceGained = 5,
                LeveledUp = false,
                PerfectCondition = false
            };

            _mockPetService
                .Setup(s => s.FeedPetAsync(123))
                .ReturnsAsync(expectedResult);

            // Act
            var result = await _controller.FeedPet();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = okResult.Value;
            
            var responseType = response.GetType();
            var successProperty = responseType.GetProperty("success");
            var dataProperty = responseType.GetProperty("data");
            
            Assert.True((bool)successProperty.GetValue(response));
            Assert.NotNull(dataProperty.GetValue(response));
            
            var data = dataProperty.GetValue(response) as PetInteractionResultDto;
            Assert.NotNull(data);
            Assert.Equal("feed", data.InteractionType);
            Assert.Equal(10, data.StatsChange.HungerChange);
        }

        [Fact]
        public async Task BathePet_ShouldReturnInteractionResult_WhenSuccessful()
        {
            // Arrange
            var expectedResult = new PetInteractionResultDto
            {
                Success = true,
                Message = "成功幫寵物洗澡！",
                InteractionType = "bathe",
                StatsChange = new PetStatsChange { CleanlinessChange = 10 }
            };

            _mockPetService
                .Setup(s => s.BathePetAsync(123))
                .ReturnsAsync(expectedResult);

            // Act
            var result = await _controller.BathePet();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var data = okResult.Value.GetType().GetProperty("data").GetValue(okResult.Value) as PetInteractionResultDto;
            
            Assert.NotNull(data);
            Assert.Equal("bathe", data.InteractionType);
            Assert.Equal(10, data.StatsChange.CleanlinessChange);
        }

        [Fact]
        public async Task PlayWithPet_ShouldReturnInteractionResult_WhenSuccessful()
        {
            // Arrange
            var expectedResult = new PetInteractionResultDto
            {
                Success = true,
                Message = "與寵物玩耍很開心！",
                InteractionType = "play",
                StatsChange = new PetStatsChange { MoodChange = 10 }
            };

            _mockPetService
                .Setup(s => s.PlayWithPetAsync(123))
                .ReturnsAsync(expectedResult);

            // Act
            var result = await _controller.PlayWithPet();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var data = okResult.Value.GetType().GetProperty("data").GetValue(okResult.Value) as PetInteractionResultDto;
            
            Assert.NotNull(data);
            Assert.Equal("play", data.InteractionType);
            Assert.Equal(10, data.StatsChange.MoodChange);
        }

        [Fact]
        public async Task RestPet_ShouldReturnInteractionResult_WhenSuccessful()
        {
            // Arrange
            var expectedResult = new PetInteractionResultDto
            {
                Success = true,
                Message = "寵物好好休息了！",
                InteractionType = "rest",
                StatsChange = new PetStatsChange { StaminaChange = 10 }
            };

            _mockPetService
                .Setup(s => s.RestPetAsync(123))
                .ReturnsAsync(expectedResult);

            // Act
            var result = await _controller.RestPet();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var data = okResult.Value.GetType().GetProperty("data").GetValue(okResult.Value) as PetInteractionResultDto;
            
            Assert.NotNull(data);
            Assert.Equal("rest", data.InteractionType);
            Assert.Equal(10, data.StatsChange.StaminaChange);
        }

        [Fact]
        public async Task FeedPet_ShouldTriggerLevelUp_WhenExperienceIsEnough()
        {
            // Arrange
            var expectedResult = new PetInteractionResultDto
            {
                Success = true,
                Message = "成功餵食寵物！恭喜升級！",
                InteractionType = "feed",
                LeveledUp = true,
                LevelUpInfo = new PetLevelUpInfo
                {
                    OldLevel = 4,
                    NewLevel = 5,
                    PointsReward = 0,
                    UpgradeMessage = "恭喜！寵物升級到 5 級！"
                }
            };

            _mockPetService
                .Setup(s => s.FeedPetAsync(123))
                .ReturnsAsync(expectedResult);

            // Act
            var result = await _controller.FeedPet();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var data = okResult.Value.GetType().GetProperty("data").GetValue(okResult.Value) as PetInteractionResultDto;
            
            Assert.NotNull(data);
            Assert.True(data.LeveledUp);
            Assert.NotNull(data.LevelUpInfo);
            Assert.Equal(4, data.LevelUpInfo.OldLevel);
            Assert.Equal(5, data.LevelUpInfo.NewLevel);
        }

        [Fact]
        public async Task FeedPet_ShouldReturnBadRequest_WhenPetNotFound()
        {
            // Arrange
            var expectedResult = new PetInteractionResultDto
            {
                Success = false,
                Message = "找不到您的寵物"
            };

            _mockPetService
                .Setup(s => s.FeedPetAsync(123))
                .ReturnsAsync(expectedResult);

            // Act
            var result = await _controller.FeedPet();

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var response = badRequestResult.Value;
            
            var responseType = response.GetType();
            var successProperty = responseType.GetProperty("success");
            var messageProperty = responseType.GetProperty("message");
            
            Assert.False((bool)successProperty.GetValue(response));
            Assert.Equal("找不到您的寵物", messageProperty.GetValue(response));
        }

        #endregion

        #region 寵物顏色系統測試

        [Fact]
        public async Task GetAvailableColors_ShouldReturnColorOptions()
        {
            // Arrange
            var expectedColors = new List<PetColorOptionDto>
            {
                new() { ColorId = "default", ColorName = "預設淺藍", SkinColor = "#ADD8E6", BackgroundColor = "粉藍", IsDefault = true, RequiredLevel = 1 },
                new() { ColorId = "pink", ColorName = "櫻花粉", SkinColor = "#FFB6C1", BackgroundColor = "粉紅", IsDefault = false, RequiredLevel = 1 },
                new() { ColorId = "gold", ColorName = "黃金色", SkinColor = "#FFD700", BackgroundColor = "金色", IsSpecial = true, RequiredLevel = 50 }
            };

            _mockPetService
                .Setup(s => s.GetAvailableColorsAsync())
                .ReturnsAsync(expectedColors);

            // Act
            var result = await _controller.GetAvailableColors();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = okResult.Value;
            
            var responseType = response.GetType();
            var successProperty = responseType.GetProperty("success");
            var dataProperty = responseType.GetProperty("data");
            
            Assert.True((bool)successProperty.GetValue(response));
            Assert.NotNull(dataProperty.GetValue(response));
            
            var data = dataProperty.GetValue(response) as List<PetColorOptionDto>;
            Assert.NotNull(data);
            Assert.Equal(3, data.Count);
            Assert.True(data.Any(c => c.IsDefault));
            Assert.True(data.Any(c => c.IsSpecial));
        }

        [Fact]
        public async Task RecolorPet_ShouldReturnSuccess_WhenValidRequest()
        {
            // Arrange
            var recolorRequest = new PetRecolorDto
            {
                SkinColor = "#FFB6C1",
                BackgroundColor = "粉紅",
                ConfirmPayment = true
            };

            var expectedResult = new ServiceResult<PetDto>
            {
                Success = true,
                Message = "寵物換色成功！",
                Data = new PetDto { SkinColor = "#FFB6C1", BackgroundColor = "粉紅" }
            };

            _mockPetService
                .Setup(s => s.RecolorPetAsync(123, recolorRequest))
                .ReturnsAsync(expectedResult);

            // Act
            var result = await _controller.RecolorPet(recolorRequest);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = okResult.Value;
            
            var responseType = response.GetType();
            var successProperty = responseType.GetProperty("success");
            var messageProperty = responseType.GetProperty("message");
            
            Assert.True((bool)successProperty.GetValue(response));
            Assert.Equal("寵物換色成功！", messageProperty.GetValue(response));
        }

        [Fact]
        public async Task RecolorPet_ShouldReturnBadRequest_WhenInsufficientPoints()
        {
            // Arrange
            var recolorRequest = new PetRecolorDto
            {
                SkinColor = "#FFB6C1",
                BackgroundColor = "粉紅",
                ConfirmPayment = true
            };

            var expectedResult = new ServiceResult<PetDto>
            {
                Success = false,
                Message = "點數不足，需要2000點數"
            };

            _mockPetService
                .Setup(s => s.RecolorPetAsync(123, recolorRequest))
                .ReturnsAsync(expectedResult);

            // Act
            var result = await _controller.RecolorPet(recolorRequest);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var response = badRequestResult.Value;
            
            var responseType = response.GetType();
            var successProperty = responseType.GetProperty("success");
            var messageProperty = responseType.GetProperty("message");
            
            Assert.False((bool)successProperty.GetValue(response));
            Assert.Contains("點數不足", messageProperty.GetValue(response).ToString());
        }

        [Fact]
        public async Task GetColorHistory_ShouldReturnHistory()
        {
            // Arrange
            var expectedHistory = new List<PetColorHistoryDto>
            {
                new() { ChangeTime = DateTime.Now.AddDays(-1), PointsCost = 2000, Reason = "使用者主動換色" },
                new() { ChangeTime = DateTime.Now.AddDays(-7), PointsCost = 2000, Reason = "使用者主動換色" }
            };

            _mockPetService
                .Setup(s => s.GetColorHistoryAsync(123))
                .ReturnsAsync(expectedHistory);

            // Act
            var result = await _controller.GetColorHistory();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var data = okResult.Value.GetType().GetProperty("data").GetValue(okResult.Value) as List<PetColorHistoryDto>;
            
            Assert.NotNull(data);
            Assert.Equal(2, data.Count);
            Assert.All(data, h => Assert.Equal(2000, h.PointsCost));
        }

        #endregion

        #region 寵物等級與經驗測試

        [Fact]
        public async Task GetLevelStats_ShouldReturnLevelInformation()
        {
            // Arrange
            var expectedStats = new PetLevelStatsDto
            {
                CurrentLevel = 10,
                CurrentExperience = 1500,
                NextLevelRequiredExp = 460,
                ExperienceToNextLevel = 160,
                LevelProgress = 65.22,
                IsMaxLevel = false
            };

            _mockPetService
                .Setup(s => s.GetLevelStatsAsync(123))
                .ReturnsAsync(expectedStats);

            // Act
            var result = await _controller.GetLevelStats();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var data = okResult.Value.GetType().GetProperty("data").GetValue(okResult.Value) as PetLevelStatsDto;
            
            Assert.NotNull(data);
            Assert.Equal(10, data.CurrentLevel);
            Assert.Equal(1500, data.CurrentExperience);
            Assert.False(data.IsMaxLevel);
        }

        [Fact]
        public async Task AddExperience_ShouldReturnExperienceResult()
        {
            // Arrange
            var addRequest = new AddExperienceRequest { Experience = 100, Source = "測試" };
            var expectedResult = new PetExperienceResultDto
            {
                Success = true,
                Message = "獲得了100經驗值",
                ExperienceGained = 100,
                Source = "測試",
                LeveledUp = false
            };

            _mockPetService
                .Setup(s => s.AddExperienceAsync(123, 100, "測試"))
                .ReturnsAsync(expectedResult);

            // Act
            var result = await _controller.AddExperience(addRequest);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var data = okResult.Value.GetType().GetProperty("data").GetValue(okResult.Value) as PetExperienceResultDto;
            
            Assert.NotNull(data);
            Assert.Equal(100, data.ExperienceGained);
            Assert.Equal("測試", data.Source);
            Assert.False(data.LeveledUp);
        }

        #endregion

        #region 寵物狀態檢查測試

        [Fact]
        public async Task CheckAdventureReadiness_ShouldReturnReadyStatus_WhenPetIsHealthy()
        {
            // Arrange
            var expectedReadiness = new PetAdventureReadinessDto
            {
                CanAdventure = true,
                Message = "寵物狀態良好，可以開始冒險！",
                CurrentHealth = 85,
                BlockingReasons = new List<string>(),
                SuggestedActions = new List<string>()
            };

            _mockPetService
                .Setup(s => s.CheckAdventureReadinessAsync(123))
                .ReturnsAsync(expectedReadiness);

            // Act
            var result = await _controller.CheckAdventureReadiness();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var data = okResult.Value.GetType().GetProperty("data").GetValue(okResult.Value) as PetAdventureReadinessDto;
            
            Assert.NotNull(data);
            Assert.True(data.CanAdventure);
            Assert.Empty(data.BlockingReasons);
            Assert.Contains("可以開始冒險", data.Message);
        }

        [Fact]
        public async Task CheckAdventureReadiness_ShouldReturnBlockedStatus_WhenPetIsUnhealthy()
        {
            // Arrange
            var expectedReadiness = new PetAdventureReadinessDto
            {
                CanAdventure = false,
                Message = "寵物狀態不佳，需要先照料",
                CurrentHealth = 0,
                BlockingReasons = new List<string> { "寵物健康度為0", "寵物飢餓值為0" },
                SuggestedActions = new List<string> { "與寵物互動提升健康度", "餵食寵物" }
            };

            _mockPetService
                .Setup(s => s.CheckAdventureReadinessAsync(123))
                .ReturnsAsync(expectedReadiness);

            // Act
            var result = await _controller.CheckAdventureReadiness();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var data = okResult.Value.GetType().GetProperty("data").GetValue(okResult.Value) as PetAdventureReadinessDto;
            
            Assert.NotNull(data);
            Assert.False(data.CanAdventure);
            Assert.NotEmpty(data.BlockingReasons);
            Assert.NotEmpty(data.SuggestedActions);
            Assert.Contains("需要先照料", data.Message);
        }

        #endregion

        #region 寵物統計與排行測試

        [Fact]
        public async Task GetPetStatistics_ShouldReturnStatistics()
        {
            // Arrange
            var expectedStats = new PetStatsDto
            {
                Pet = new PetDto { PetName = "測試寵物", Level = 15 },
                PetAge = 30,
                TotalPointsSpent = 4000,
                HighestHealth = 100
            };

            _mockPetService
                .Setup(s => s.GetPetStatisticsAsync(123))
                .ReturnsAsync(expectedStats);

            // Act
            var result = await _controller.GetPetStatistics();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var data = okResult.Value.GetType().GetProperty("data").GetValue(okResult.Value) as PetStatsDto;
            
            Assert.NotNull(data);
            Assert.Equal("測試寵物", data.Pet.PetName);
            Assert.Equal(30, data.PetAge);
            Assert.Equal(4000, data.TotalPointsSpent);
        }

        [Fact]
        public async Task GetPetRankings_ShouldReturnRankings()
        {
            // Arrange
            var expectedRankings = new List<PetRankingDto>
            {
                new() { Rank = 1, PetName = "第一名", OwnerName = "玩家1", Level = 50, RankingType = PetRankingType.Level },
                new() { Rank = 2, PetName = "第二名", OwnerName = "玩家2", Level = 45, RankingType = PetRankingType.Level },
                new() { Rank = 3, PetName = "第三名", OwnerName = "玩家3", Level = 40, RankingType = PetRankingType.Level }
            };

            _mockPetService
                .Setup(s => s.GetPetRankingsAsync(PetRankingType.Level, 50))
                .ReturnsAsync(expectedRankings);

            // Act
            var result = await _controller.GetPetRankings("level", 50);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var data = okResult.Value.GetType().GetProperty("data").GetValue(okResult.Value) as List<PetRankingDto>;
            
            Assert.NotNull(data);
            Assert.Equal(3, data.Count);
            Assert.Equal(1, data[0].Rank);
            Assert.Equal("第一名", data[0].PetName);
            Assert.True(data.All(r => r.RankingType == PetRankingType.Level));
        }

        [Fact]
        public async Task GetPetRankings_ShouldReturnBadRequest_WhenInvalidRankingType()
        {
            // Act
            var result = await _controller.GetPetRankings("invalid", 50);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var response = badRequestResult.Value;
            
            var responseType = response.GetType();
            var successProperty = responseType.GetProperty("success");
            var messageProperty = responseType.GetProperty("message");
            
            Assert.False((bool)successProperty.GetValue(response));
            Assert.Equal("不支援的排行類型", messageProperty.GetValue(response));
        }

        #endregion

        #region 管理員功能測試

        [Fact]
        public async Task GetSystemConfig_ShouldReturnConfig_WhenUserIsAdmin()
        {
            // Arrange
            var expectedConfig = new PetSystemConfigDto
            {
                RecolorCost = 2000,
                MaxLevel = 250,
                EnableAutoDailyDecay = true
            };

            _mockPetService
                .Setup(s => s.GetSystemConfigAsync())
                .ReturnsAsync(expectedConfig);

            // 設定管理員身份
            var adminClaims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, "999"),
                new Claim(ClaimTypes.Role, "Admin")
            };
            var adminIdentity = new ClaimsIdentity(adminClaims, "Test");
            var adminPrincipal = new ClaimsPrincipal(adminIdentity);

            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = adminPrincipal
                }
            };

            // Act
            var result = await _controller.GetSystemConfig();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var data = okResult.Value.GetType().GetProperty("data").GetValue(okResult.Value) as PetSystemConfigDto;
            
            Assert.NotNull(data);
            Assert.Equal(2000, data.RecolorCost);
            Assert.Equal(250, data.MaxLevel);
            Assert.True(data.EnableAutoDailyDecay);
        }

        [Fact]
        public async Task AdminResetPet_ShouldReturnSuccess_WhenValidRequest()
        {
            // Arrange
            var resetRequest = new PetAdminResetDto
            {
                ResetType = "stats",
                Reason = "測試重置",
                SendNotification = true
            };

            var expectedResult = new ServiceResult<PetDto>
            {
                Success = true,
                Message = "寵物重置成功",
                Data = new PetDto { Hunger = 100, Mood = 100, Stamina = 100, Cleanliness = 100, Health = 100 }
            };

            _mockPetService
                .Setup(s => s.AdminResetPetAsync(456, resetRequest))
                .ReturnsAsync(expectedResult);

            // 設定管理員身份
            var adminClaims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, "999"),
                new Claim(ClaimTypes.Role, "Admin")
            };
            var adminIdentity = new ClaimsIdentity(adminClaims, "Test");
            var adminPrincipal = new ClaimsPrincipal(adminIdentity);

            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = adminPrincipal
                }
            };

            // Act
            var result = await _controller.AdminResetPet(456, resetRequest);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = okResult.Value;
            
            var responseType = response.GetType();
            var successProperty = responseType.GetProperty("success");
            var messageProperty = responseType.GetProperty("message");
            
            Assert.True((bool)successProperty.GetValue(response));
            Assert.Equal("寵物重置成功", messageProperty.GetValue(response));
        }

        #endregion

        #region 錯誤處理測試

        [Fact]
        public async Task GetPet_ShouldReturnServerError_WhenServiceThrows()
        {
            // Arrange
            _mockPetService
                .Setup(s => s.GetUserPetAsync(123))
                .ThrowsAsync(new Exception("Database connection failed"));

            // Act
            var result = await _controller.GetPet();

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, statusCodeResult.StatusCode);
        }

        [Fact]
        public async Task FeedPet_ShouldReturnServerError_WhenServiceThrows()
        {
            // Arrange
            _mockPetService
                .Setup(s => s.FeedPetAsync(123))
                .ThrowsAsync(new Exception("Service error"));

            // Act
            var result = await _controller.FeedPet();

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, statusCodeResult.StatusCode);
        }

        #endregion

        #region 邊界條件測試

        [Fact]
        public async Task CreatePet_ShouldUseDefaultName_WhenEmptyNameProvided()
        {
            // Arrange
            var createRequest = new CreatePetRequest { PetName = "" };
            var expectedPet = new PetDto { PetName = "小可愛" };

            _mockPetService
                .Setup(s => s.CreatePetAsync(123, "小可愛"))
                .ReturnsAsync(expectedPet);

            // Act
            var result = await _controller.CreatePet(createRequest);

            // Assert
            _mockPetService.Verify(s => s.CreatePetAsync(123, "小可愛"), Times.Once);
        }

        [Fact]
        public async Task GetPetRankings_ShouldLimitResults_WhenLargeLimitRequested()
        {
            // Arrange
            var expectedRankings = new List<PetRankingDto>();

            _mockPetService
                .Setup(s => s.GetPetRankingsAsync(PetRankingType.Level, 100))
                .ReturnsAsync(expectedRankings);

            // Act
            var result = await _controller.GetPetRankings("level", 200); // 超過限制

            // Assert
            _mockPetService.Verify(s => s.GetPetRankingsAsync(PetRankingType.Level, 100), Times.Once);
        }

        #endregion
    }
}