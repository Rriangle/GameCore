using Xunit;
using Moq;
using FluentAssertions;
using GameCore.Core.Entities;
using GameCore.Core.Interfaces;
using GameCore.Core.Services;

namespace GameCore.Tests.UnitTests
{
    public class PetServiceTests
    {
        private readonly Mock<IPetRepository> _mockPetRepository;
        private readonly Mock<IUserRepository> _mockUserRepository;
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly PetService _petService;

        public PetServiceTests()
        {
            _mockPetRepository = new Mock<IPetRepository>();
            _mockUserRepository = new Mock<IUserRepository>();
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _petService = new PetService(_mockPetRepository.Object, _mockUserRepository.Object, _mockUnitOfWork.Object);
        }

        [Fact]
        public async Task FeedPet_WithValidPet_ShouldIncreaseHungerAndDecreasePoints()
        {
            // Arrange
            var pet = new Pet
            {
                PetId = 1,
                UserId = 1,
                Name = "測試寵物",
                Hunger = 50,
                Health = 80,
                Cleanliness = 70,
                Happiness = 60,
                Energy = 90
            };

            var user = new User
            {
                UserId = 1,
                Username = "testuser",
                Points = 1000
            };

            _mockPetRepository.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(pet);
            _mockUserRepository.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(user);
            _mockUnitOfWork.Setup(u => u.SaveChangesAsync()).ReturnsAsync(1);

            // Act
            var result = await _petService.FeedPetAsync(1, 1);

            // Assert
            result.Should().NotBeNull();
            result.IsSuccess.Should().BeTrue();
            pet.Hunger.Should().BeGreaterThan(50);
            user.Points.Should().BeLessThan(1000);
        }

        [Fact]
        public async Task FeedPet_WithInsufficientPoints_ShouldReturnFailure()
        {
            // Arrange
            var pet = new Pet
            {
                PetId = 1,
                UserId = 1,
                Name = "測試寵物",
                Hunger = 50
            };

            var user = new User
            {
                UserId = 1,
                Username = "testuser",
                Points = 50 // 不足的點數
            };

            _mockPetRepository.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(pet);
            _mockUserRepository.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(user);

            // Act
            var result = await _petService.FeedPetAsync(1, 1);

            // Assert
            result.Should().NotBeNull();
            result.IsSuccess.Should().BeFalse();
            result.Message.Should().Contain("點數不足");
        }

        [Fact]
        public async Task CleanPet_WithValidPet_ShouldIncreaseCleanlinessAndDecreasePoints()
        {
            // Arrange
            var pet = new Pet
            {
                PetId = 1,
                UserId = 1,
                Name = "測試寵物",
                Hunger = 60,
                Health = 80,
                Cleanliness = 30, // 低清潔度
                Happiness = 60,
                Energy = 90
            };

            var user = new User
            {
                UserId = 1,
                Username = "testuser",
                Points = 1000
            };

            _mockPetRepository.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(pet);
            _mockUserRepository.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(user);
            _mockUnitOfWork.Setup(u => u.SaveChangesAsync()).ReturnsAsync(1);

            // Act
            var result = await _petService.CleanPetAsync(1, 1);

            // Assert
            result.Should().NotBeNull();
            result.IsSuccess.Should().BeTrue();
            pet.Cleanliness.Should().BeGreaterThan(30);
            user.Points.Should().BeLessThan(1000);
        }

        [Fact]
        public async Task PlayWithPet_WithValidPet_ShouldIncreaseHappinessAndDecreaseEnergy()
        {
            // Arrange
            var pet = new Pet
            {
                PetId = 1,
                UserId = 1,
                Name = "測試寵物",
                Hunger = 60,
                Health = 80,
                Cleanliness = 70,
                Happiness = 40, // 低心情
                Energy = 90
            };

            var user = new User
            {
                UserId = 1,
                Username = "testuser",
                Points = 1000
            };

            _mockPetRepository.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(pet);
            _mockUserRepository.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(user);
            _mockUnitOfWork.Setup(u => u.SaveChangesAsync()).ReturnsAsync(1);

            // Act
            var result = await _petService.PlayWithPetAsync(1, 1);

            // Assert
            result.Should().NotBeNull();
            result.IsSuccess.Should().BeTrue();
            pet.Happiness.Should().BeGreaterThan(40);
            pet.Energy.Should().BeLessThan(90);
        }

        [Fact]
        public async Task RestPet_WithValidPet_ShouldIncreaseEnergyAndDecreaseHappiness()
        {
            // Arrange
            var pet = new Pet
            {
                PetId = 1,
                UserId = 1,
                Name = "測試寵物",
                Hunger = 60,
                Health = 80,
                Cleanliness = 70,
                Happiness = 80,
                Energy = 30 // 低體力
            };

            var user = new User
            {
                UserId = 1,
                Username = "testuser",
                Points = 1000
            };

            _mockPetRepository.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(pet);
            _mockUserRepository.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(user);
            _mockUnitOfWork.Setup(u => u.SaveChangesAsync()).ReturnsAsync(1);

            // Act
            var result = await _petService.RestPetAsync(1, 1);

            // Assert
            result.Should().NotBeNull();
            result.IsSuccess.Should().BeTrue();
            pet.Energy.Should().BeGreaterThan(30);
            pet.Happiness.Should().BeLessThan(80);
        }

        [Fact]
        public async Task ChangePetColor_WithValidPetAndSufficientPoints_ShouldChangeColor()
        {
            // Arrange
            var pet = new Pet
            {
                PetId = 1,
                UserId = 1,
                Name = "測試寵物",
                Color = "#FF0000"
            };

            var user = new User
            {
                UserId = 1,
                Username = "testuser",
                Points = 2500 // 足夠的點數
            };

            _mockPetRepository.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(pet);
            _mockUserRepository.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(user);
            _mockUnitOfWork.Setup(u => u.SaveChangesAsync()).ReturnsAsync(1);

            var newColor = "#00FF00";

            // Act
            var result = await _petService.ChangePetColorAsync(1, 1, newColor);

            // Assert
            result.Should().NotBeNull();
            result.IsSuccess.Should().BeTrue();
            pet.Color.Should().Be(newColor);
            user.Points.Should().Be(500); // 2000 點數被扣除
        }

        [Fact]
        public async Task ChangePetColor_WithInsufficientPoints_ShouldReturnFailure()
        {
            // Arrange
            var pet = new Pet
            {
                PetId = 1,
                UserId = 1,
                Name = "測試寵物",
                Color = "#FF0000"
            };

            var user = new User
            {
                UserId = 1,
                Username = "testuser",
                Points = 1500 // 不足的點數
            };

            _mockPetRepository.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(pet);
            _mockUserRepository.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(user);

            var newColor = "#00FF00";

            // Act
            var result = await _petService.ChangePetColorAsync(1, 1, newColor);

            // Assert
            result.Should().NotBeNull();
            result.IsSuccess.Should().BeFalse();
            result.Message.Should().Contain("點數不足");
            pet.Color.Should().Be("#FF0000"); // 顏色不應改變
            user.Points.Should().Be(1500); // 點數不應改變
        }

        [Fact]
        public async Task GetPetStatus_WithValidPet_ShouldReturnCorrectStatus()
        {
            // Arrange
            var pet = new Pet
            {
                PetId = 1,
                UserId = 1,
                Name = "測試寵物",
                Hunger = 20, // 低飢餓度
                Health = 80,
                Cleanliness = 30, // 低清潔度
                Happiness = 60,
                Energy = 90
            };

            _mockPetRepository.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(pet);

            // Act
            var result = await _petService.GetPetStatusAsync(1, 1);

            // Assert
            result.Should().NotBeNull();
            result.IsSuccess.Should().BeTrue();
            result.Data.Should().NotBeNull();
            result.Data.Hunger.Should().Be(20);
            result.Data.Cleanliness.Should().Be(30);
        }

        [Fact]
        public async Task GetPetStatus_WithInvalidPet_ShouldReturnFailure()
        {
            // Arrange
            _mockPetRepository.Setup(r => r.GetByIdAsync(999)).ReturnsAsync((Pet?)null);

            // Act
            var result = await _petService.GetPetStatusAsync(1, 999);

            // Assert
            result.Should().NotBeNull();
            result.IsSuccess.Should().BeFalse();
            result.Message.Should().Contain("寵物不存在");
        }
    }
}