using Xunit;
using Moq;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using GameCore.Core.Services;
using GameCore.Core.Interfaces;
using GameCore.Core.Entities;

namespace GameCore.Tests.UnitTests
{
    /// <summary>
    /// 寵物服務單元測試
    /// 測試寵物系統的核心業務邏輯
    /// </summary>
    public class PetServiceTests
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<IPetRepository> _mockPetRepository;
        private readonly Mock<IUserRepository> _mockUserRepository;
        private readonly Mock<ILogger<PetService>> _mockLogger;
        private readonly PetService _petService;

        public PetServiceTests()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockPetRepository = new Mock<IPetRepository>();
            _mockUserRepository = new Mock<IUserRepository>();
            _mockLogger = new Mock<ILogger<PetService>>();

            // 設定 UnitOfWork 回傳對應的 Repository
            _mockUnitOfWork.Setup(u => u.PetRepository).Returns(_mockPetRepository.Object);
            _mockUnitOfWork.Setup(u => u.UserRepository).Returns(_mockUserRepository.Object);

            _petService = new PetService(_mockUnitOfWork.Object, _mockLogger.Object);
        }

        /// <summary>
        /// 測試取得或建立寵物 - 當寵物不存在時應建立新寵物
        /// </summary>
        [Fact]
        public async Task GetOrCreatePetAsync_WhenPetNotExists_ShouldCreateNewPet()
        {
            // Arrange
            var userId = 1;
            _mockPetRepository.Setup(r => r.GetByUserIdAsync(userId))
                .ReturnsAsync((Pet?)null);

            _mockPetRepository.Setup(r => r.AddAsync(It.IsAny<Pet>()))
                .ReturnsAsync((Pet pet) => pet);

            // Act
            var result = await _petService.GetOrCreatePetAsync(userId);

            // Assert
            result.Should().NotBeNull();
            result.UserId.Should().Be(userId);
            result.PetName.Should().Be("小可愛");
            result.Level.Should().Be(1);
            result.Hunger.Should().Be(100);
            result.Mood.Should().Be(100);
            result.Stamina.Should().Be(100);
            result.Cleanliness.Should().Be(100);
            result.Health.Should().Be(100);

            _mockPetRepository.Verify(r => r.AddAsync(It.IsAny<Pet>()), Times.Once);
            _mockUnitOfWork.Verify(u => u.SaveChangesAsync(), Times.Once);
        }

        /// <summary>
        /// 測試取得或建立寵物 - 當寵物已存在時應返回現有寵物
        /// </summary>
        [Fact]
        public async Task GetOrCreatePetAsync_WhenPetExists_ShouldReturnExistingPet()
        {
            // Arrange
            var userId = 1;
            var existingPet = new Pet
            {
                PetId = 1,
                UserId = userId,
                PetName = "史萊姆王",
                Level = 5,
                Hunger = 80,
                Mood = 90,
                Stamina = 75,
                Cleanliness = 85,
                Health = 95
            };

            _mockPetRepository.Setup(r => r.GetByUserIdAsync(userId))
                .ReturnsAsync(existingPet);

            // Act
            var result = await _petService.GetOrCreatePetAsync(userId);

            // Assert
            result.Should().BeEquivalentTo(existingPet);
            _mockPetRepository.Verify(r => r.AddAsync(It.IsAny<Pet>()), Times.Never);
        }

        /// <summary>
        /// 測試寵物互動 - 餵食應該增加飢餓值
        /// </summary>
        [Fact]
        public async Task InteractWithPetAsync_Feed_ShouldIncreaseHunger()
        {
            // Arrange
            var userId = 1;
            var pet = new Pet
            {
                PetId = 1,
                UserId = userId,
                Hunger = 50,
                Mood = 80,
                Stamina = 70,
                Cleanliness = 90,
                Health = 85
            };

            _mockPetRepository.Setup(r => r.GetByUserIdAsync(userId))
                .ReturnsAsync(pet);

            _mockPetRepository.Setup(r => r.GetLastInteractionTimeAsync(userId, PetInteractionType.Feed))
                .ReturnsAsync((DateTime?)null);

            // Act
            var result = await _petService.InteractWithPetAsync(userId, PetInteractionType.Feed);

            // Assert
            result.Should().NotBeNull();
            result.Success.Should().BeTrue();
            result.Pet?.Hunger.Should().Be(60); // 50 + 10
            result.Message.Should().Contain("餵食");

            _mockPetRepository.Verify(r => r.UpdateAsync(It.IsAny<Pet>()), Times.Once);
            _mockUnitOfWork.Verify(u => u.SaveChangesAsync(), Times.Once);
        }

        /// <summary>
        /// 測試寵物互動 - 當四維屬性全滿時應回復健康度
        /// </summary>
        [Fact]
        public async Task InteractWithPetAsync_WhenAllAttributesMax_ShouldRestoreHealth()
        {
            // Arrange
            var userId = 1;
            var pet = new Pet
            {
                PetId = 1,
                UserId = userId,
                Hunger = 90,  // 餵食後會變成 100
                Mood = 100,
                Stamina = 100,
                Cleanliness = 100,
                Health = 50   // 健康度較低
            };

            _mockPetRepository.Setup(r => r.GetByUserIdAsync(userId))
                .ReturnsAsync(pet);

            _mockPetRepository.Setup(r => r.GetLastInteractionTimeAsync(userId, PetInteractionType.Feed))
                .ReturnsAsync((DateTime?)null);

            // Act
            var result = await _petService.InteractWithPetAsync(userId, PetInteractionType.Feed);

            // Assert
            result.Should().NotBeNull();
            result.Success.Should().BeTrue();
            result.Pet?.Hunger.Should().Be(100);
            result.Pet?.Health.Should().Be(100); // 應該回復到 100
            result.HealthRestored.Should().BeTrue();
            result.Message.Should().Contain("四維屬性全滿");
        }

        /// <summary>
        /// 測試計算升級所需經驗值 - 不同等級範圍的公式
        /// </summary>
        [Theory]
        [InlineData(1, 100)]    // Level 1: 40*1+60 = 100
        [InlineData(5, 260)]    // Level 5: 40*5+60 = 260
        [InlineData(10, 460)]   // Level 10: 40*10+60 = 460
        [InlineData(15, 560)]   // Level 15: 0.8*15²+380 = 560
        [InlineData(50, 2380)]  // Level 50: 0.8*50²+380 = 2380
        [InlineData(100, 8380)] // Level 100: 0.8*100²+380 = 8380
        public void CalculateRequiredExperience_ShouldReturnCorrectValue(int level, int expectedExp)
        {
            // Act
            var result = _petService.CalculateRequiredExperience(level);

            // Assert
            result.Should().Be(expectedExp);
        }

        /// <summary>
        /// 測試寵物換色 - 點數不足時應該失敗
        /// </summary>
        [Fact]
        public async Task ChangePetColorAsync_InsufficientPoints_ShouldFail()
        {
            // Arrange
            var userId = 1;
            var pet = new Pet { PetId = 1, UserId = userId };
            var wallet = new UserWallet { UserId = userId, UserPoint = 1000 }; // 點數不足

            _mockPetRepository.Setup(r => r.GetByUserIdAsync(userId))
                .ReturnsAsync(pet);

            _mockUserRepository.Setup(r => r.GetWalletAsync(userId))
                .ReturnsAsync(wallet);

            // Act
            var result = await _petService.ChangePetColorAsync(userId, "#FF0000", "#00FF00");

            // Assert
            result.Should().NotBeNull();
            result.Success.Should().BeFalse();
            result.Message.Should().Contain("點數不足");
            result.RemainingPoints.Should().Be(1000);

            _mockUserRepository.Verify(r => r.UpdateWalletAsync(It.IsAny<UserWallet>()), Times.Never);
        }

        /// <summary>
        /// 測試寵物換色 - 點數足夠時應該成功
        /// </summary>
        [Fact]
        public async Task ChangePetColorAsync_SufficientPoints_ShouldSucceed()
        {
            // Arrange
            var userId = 1;
            var pet = new Pet 
            { 
                PetId = 1, 
                UserId = userId,
                SkinColor = "#ADD8E6",
                BackgroundColor = "粉藍"
            };
            var wallet = new UserWallet { UserId = userId, UserPoint = 5000 }; // 點數充足

            _mockPetRepository.Setup(r => r.GetByUserIdAsync(userId))
                .ReturnsAsync(pet);

            _mockUserRepository.Setup(r => r.GetWalletAsync(userId))
                .ReturnsAsync(wallet);

            // Act
            var result = await _petService.ChangePetColorAsync(userId, "#FF0000", "#00FF00");

            // Assert
            result.Should().NotBeNull();
            result.Success.Should().BeTrue();
            result.Message.Should().Contain("成功");
            result.PointsUsed.Should().Be(2000);
            result.RemainingPoints.Should().Be(3000);
            result.Pet?.SkinColor.Should().Be("#FF0000");
            result.Pet?.BackgroundColor.Should().Be("#00FF00");

            _mockUserRepository.Verify(r => r.UpdateWalletAsync(It.IsAny<UserWallet>()), Times.Once);
            _mockPetRepository.Verify(r => r.UpdateAsync(It.IsAny<Pet>()), Times.Once);
        }

        /// <summary>
        /// 測試檢查是否可以冒險 - 健康度為 0 時不能冒險
        /// </summary>
        [Fact]
        public async Task CanStartAdventureAsync_HealthZero_ShouldReturnFalse()
        {
            // Arrange
            var userId = 1;
            var pet = new Pet
            {
                PetId = 1,
                UserId = userId,
                Hunger = 100,
                Mood = 100,
                Stamina = 100,
                Cleanliness = 100,
                Health = 0  // 健康度為 0
            };

            _mockPetRepository.Setup(r => r.GetByUserIdAsync(userId))
                .ReturnsAsync(pet);

            // Act
            var result = await _petService.CanStartAdventureAsync(userId);

            // Assert
            result.Should().BeFalse();
        }

        /// <summary>
        /// 測試檢查是否可以冒險 - 任一屬性為 0 時不能冒險
        /// </summary>
        [Theory]
        [InlineData(0, 100, 100, 100, 100)]  // 飢餓值為 0
        [InlineData(100, 0, 100, 100, 100)]  // 心情值為 0
        [InlineData(100, 100, 0, 100, 100)]  // 體力值為 0
        [InlineData(100, 100, 100, 0, 100)]  // 清潔值為 0
        public async Task CanStartAdventureAsync_AnyAttributeZero_ShouldReturnFalse(
            int hunger, int mood, int stamina, int cleanliness, int health)
        {
            // Arrange
            var userId = 1;
            var pet = new Pet
            {
                PetId = 1,
                UserId = userId,
                Hunger = hunger,
                Mood = mood,
                Stamina = stamina,
                Cleanliness = cleanliness,
                Health = health
            };

            _mockPetRepository.Setup(r => r.GetByUserIdAsync(userId))
                .ReturnsAsync(pet);

            // Act
            var result = await _petService.CanStartAdventureAsync(userId);

            // Assert
            result.Should().BeFalse();
        }

        /// <summary>
        /// 測試檢查是否可以冒險 - 所有屬性都大於 0 時可以冒險
        /// </summary>
        [Fact]
        public async Task CanStartAdventureAsync_AllAttributesPositive_ShouldReturnTrue()
        {
            // Arrange
            var userId = 1;
            var pet = new Pet
            {
                PetId = 1,
                UserId = userId,
                Hunger = 80,
                Mood = 90,
                Stamina = 75,
                Cleanliness = 85,
                Health = 95
            };

            _mockPetRepository.Setup(r => r.GetByUserIdAsync(userId))
                .ReturnsAsync(pet);

            // Act
            var result = await _petService.CanStartAdventureAsync(userId);

            // Assert
            result.Should().BeTrue();
        }

        /// <summary>
        /// 測試寵物狀態描述 - 應該正確判斷整體狀態
        /// </summary>
        [Theory]
        [InlineData(100, 100, 100, 100, 100, "優秀")]
        [InlineData(80, 80, 80, 80, 80, "優秀")]
        [InlineData(70, 70, 70, 70, 70, "良好")]
        [InlineData(50, 50, 50, 50, 50, "普通")]
        [InlineData(30, 30, 30, 30, 30, "不佳")]
        [InlineData(10, 10, 10, 10, 10, "危險")]
        public void GetPetStatusDescription_ShouldReturnCorrectOverallStatus(
            int hunger, int mood, int stamina, int cleanliness, int health, string expectedStatus)
        {
            // Arrange
            var pet = new Pet
            {
                Hunger = hunger,
                Mood = mood,
                Stamina = stamina,
                Cleanliness = cleanliness,
                Health = health
            };

            // Act
            var result = _petService.GetPetStatusDescription(pet);

            // Assert
            result.OverallStatus.Should().Be(expectedStatus);
        }

        /// <summary>
        /// 測試增加寵物經驗值 - 應該正確處理升級邏輯
        /// </summary>
        [Fact]
        public async Task AddExperienceAsync_ShouldHandleLevelUp()
        {
            // Arrange
            var pet = new Pet
            {
                PetId = 1,
                UserId = 1,
                Level = 1,
                Experience = 50  // 距離升級還需要 50 經驗 (Level 1 需要 100 經驗)
            };

            _mockUserRepository.Setup(r => r.AddPointsAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _petService.AddExperienceAsync(pet, 100); // 給予 100 經驗

            // Assert
            result.Should().BeTrue(); // 應該有升級
            pet.Level.Should().Be(2); // 升到 2 級
            pet.Experience.Should().Be(50); // 剩餘 50 經驗 (150 - 100)

            _mockUserRepository.Verify(r => r.AddPointsAsync(1, It.IsAny<int>(), "寵物升級獎勵"), Times.Once);
            _mockPetRepository.Verify(r => r.UpdateAsync(pet), Times.Once);
        }

        /// <summary>
        /// 測試更新健康狀態 - 低屬性應該扣健康度
        /// </summary>
        [Fact]
        public async Task UpdateHealthStatusAsync_LowAttributes_ShouldReduceHealth()
        {
            // Arrange
            var pet = new Pet
            {
                PetId = 1,
                Hunger = 20,      // 低於 30，會扣健康度
                Mood = 80,
                Stamina = 25,     // 低於 30，會扣健康度
                Cleanliness = 90,
                Health = 100
            };

            // Act
            var result = await _petService.UpdateHealthStatusAsync(pet);

            // Assert
            result.Should().Be(60); // 100 - 20 - 20 = 60
            pet.Health.Should().Be(60);

            _mockPetRepository.Verify(r => r.UpdateAsync(pet), Times.Once);
        }

        /// <summary>
        /// 測試互動冷卻時間 - 沒有互動記錄時應該返回 0
        /// </summary>
        [Fact]
        public async Task GetInteractionCooldownAsync_NoLastInteraction_ShouldReturnZero()
        {
            // Arrange
            var userId = 1;
            _mockPetRepository.Setup(r => r.GetLastInteractionTimeAsync(userId, PetInteractionType.Feed))
                .ReturnsAsync((DateTime?)null);

            // Act
            var result = await _petService.GetInteractionCooldownAsync(userId, PetInteractionType.Feed);

            // Assert
            result.Should().Be(0);
        }
    }
}