using Xunit;
using Moq;
using FluentAssertions;
using GameCore.Core.Services;
using GameCore.Core.Interfaces;
using GameCore.Core.Entities;
using GameCore.Core.DTOs;
using Microsoft.Extensions.Logging;

namespace GameCore.Tests.UnitTests
{
    /// <summary>
    /// 簽到服務單元測試
    /// 測試簽到邏輯、獎勵計算、連續簽到等功能
    /// </summary>
    public class SignInServiceTests
    {
        private readonly Mock<IUserRepository> _mockUserRepository;
        private readonly Mock<ISignInRepository> _mockSignInRepository;
        private readonly Mock<ILogger<SignInService>> _mockLogger;
        private readonly SignInService _signInService;

        public SignInServiceTests()
        {
            _mockUserRepository = new Mock<IUserRepository>();
            _mockSignInRepository = new Mock<ISignInRepository>();
            _mockLogger = new Mock<ILogger<SignInService>>();
            
            _signInService = new SignInService(
                _mockUserRepository.Object,
                _mockSignInRepository.Object,
                _mockLogger.Object);
        }

        [Fact]
        public async Task DailySignInAsync_新用戶首次簽到_應該成功並獲得基礎獎勵()
        {
            // Arrange
            var userId = 1;
            var user = new User { UserId = userId, Points = 100, Experience = 500 };
            var today = DateTime.Now.Date;
            
            _mockUserRepository.Setup(x => x.GetByIdAsync(userId))
                .ReturnsAsync(user);
            
            _mockSignInRepository.Setup(x => x.GetSignInByDateAsync(userId, today))
                .ReturnsAsync((UserSignInStats?)null); // 今天還沒簽到
            
            _mockSignInRepository.Setup(x => x.GetStreakDaysAsync(userId))
                .ReturnsAsync(0); // 沒有連續簽到記錄

            // Act
            var result = await _signInService.DailySignInAsync(userId);

            // Assert
            result.Should().NotBeNull();
            result.Success.Should().BeTrue();
            result.Points.Should().Be(20); // 平日基礎獎勵
            result.Experience.Should().Be(10);
            result.StreakDays.Should().Be(1);
            
            // 驗證方法調用
            _mockUserRepository.Verify(x => x.AddPointsAsync(userId, 20, "每日簽到獎勵"), Times.Once);
            _mockSignInRepository.Verify(x => x.AddAsync(It.IsAny<UserSignInStats>()), Times.Once);
        }

        [Fact]
        public async Task DailySignInAsync_週末簽到_應該獲得額外獎勵()
        {
            // Arrange
            var userId = 1;
            var user = new User { UserId = userId, Points = 100, Experience = 500 };
            var saturday = GetNextSaturday();
            
            _mockUserRepository.Setup(x => x.GetByIdAsync(userId))
                .ReturnsAsync(user);
            
            _mockSignInRepository.Setup(x => x.GetSignInByDateAsync(userId, saturday))
                .ReturnsAsync((UserSignInStats?)null);
            
            _mockSignInRepository.Setup(x => x.GetStreakDaysAsync(userId))
                .ReturnsAsync(2);

            // Act
            var result = await _signInService.DailySignInAsync(userId);

            // Assert
            result.Should().NotBeNull();
            result.Success.Should().BeTrue();
            result.Points.Should().Be(30); // 週末額外獎勵
            result.Experience.Should().Be(15);
            result.StreakDays.Should().Be(3);
        }

        [Fact]
        public async Task DailySignInAsync_連續簽到7天_應該獲得大獎勵()
        {
            // Arrange
            var userId = 1;
            var user = new User { UserId = userId, Points = 100, Experience = 500 };
            var today = DateTime.Now.Date;
            
            _mockUserRepository.Setup(x => x.GetByIdAsync(userId))
                .ReturnsAsync(user);
            
            _mockSignInRepository.Setup(x => x.GetSignInByDateAsync(userId, today))
                .ReturnsAsync((UserSignInStats?)null);
            
            _mockSignInRepository.Setup(x => x.GetStreakDaysAsync(userId))
                .ReturnsAsync(6); // 已經連續6天

            // Act
            var result = await _signInService.DailySignInAsync(userId);

            // Assert
            result.Should().NotBeNull();
            result.Success.Should().BeTrue();
            result.StreakDays.Should().Be(7);
            result.Points.Should().BeGreaterThan(20); // 應該有額外獎勵
            result.IsStreakBonus.Should().BeTrue();
        }

        [Fact]
        public async Task DailySignInAsync_已經簽到過_應該返回失敗()
        {
            // Arrange
            var userId = 1;
            var today = DateTime.Now.Date;
            var existingSignIn = new UserSignInStats
            {
                UserId = userId,
                SignInDate = today,
                Points = 20,
                Experience = 10
            };
            
            _mockSignInRepository.Setup(x => x.GetSignInByDateAsync(userId, today))
                .ReturnsAsync(existingSignIn);

            // Act
            var result = await _signInService.DailySignInAsync(userId);

            // Assert
            result.Should().NotBeNull();
            result.Success.Should().BeFalse();
            result.Message.Should().Contain("已經簽到");
            
            // 確保不會重複添加點數
            _mockUserRepository.Verify(x => x.AddPointsAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public async Task GetUserSignInStatsAsync_應該返回正確的統計資料()
        {
            // Arrange
            var userId = 1;
            var signInStats = new List<UserSignInStats>
            {
                new() { UserId = userId, SignInDate = DateTime.Now.Date.AddDays(-1), Points = 20 },
                new() { UserId = userId, SignInDate = DateTime.Now.Date.AddDays(-2), Points = 30 },
                new() { UserId = userId, SignInDate = DateTime.Now.Date.AddDays(-3), Points = 20 }
            };

            _mockSignInRepository.Setup(x => x.GetRecentSignInsAsync(userId, 30))
                .ReturnsAsync(signInStats);
            
            _mockSignInRepository.Setup(x => x.GetSignInCountAsync(userId))
                .ReturnsAsync(15);
            
            _mockSignInRepository.Setup(x => x.GetStreakDaysAsync(userId))
                .ReturnsAsync(3);

            // Act
            var result = await _signInService.GetUserSignInStatsAsync(userId);

            // Assert
            result.Should().NotBeNull();
            result.TotalSignIns.Should().Be(15);
            result.CurrentStreak.Should().Be(3);
            result.MonthlySignIns.Should().Be(3);
            result.TotalRewards.Should().Be(70); // 20 + 30 + 20
        }

        [Theory]
        [InlineData(1, 20, 10, false)] // 平日
        [InlineData(3, 20, 10, false)] // 週三
        [InlineData(5, 20, 10, false)] // 週五
        [InlineData(6, 30, 15, true)]  // 週六
        [InlineData(0, 30, 15, true)]  // 週日
        public void CalculateSignInReward_不同日期_應該返回正確獎勵(int dayOfWeek, int expectedPoints, int expectedExp, bool isWeekend)
        {
            // Arrange
            var testDate = GetDateWithDayOfWeek((DayOfWeek)dayOfWeek);
            var streakDays = 1;

            // Act
            var result = _signInService.CalculateSignInReward(testDate, streakDays);

            // Assert
            result.Should().NotBeNull();
            result.Points.Should().Be(expectedPoints);
            result.Experience.Should().Be(expectedExp);
            result.IsWeekendBonus.Should().Be(isWeekend);
        }

        [Fact]
        public async Task CheckCanSignInAsync_今天已簽到_應該返回false()
        {
            // Arrange
            var userId = 1;
            var today = DateTime.Now.Date;
            var existingSignIn = new UserSignInStats { UserId = userId, SignInDate = today };
            
            _mockSignInRepository.Setup(x => x.GetSignInByDateAsync(userId, today))
                .ReturnsAsync(existingSignIn);

            // Act
            var result = await _signInService.CheckCanSignInAsync(userId);

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public async Task CheckCanSignInAsync_今天未簽到_應該返回true()
        {
            // Arrange
            var userId = 1;
            var today = DateTime.Now.Date;
            
            _mockSignInRepository.Setup(x => x.GetSignInByDateAsync(userId, today))
                .ReturnsAsync((UserSignInStats?)null);

            // Act
            var result = await _signInService.CheckCanSignInAsync(userId);

            // Assert
            result.Should().BeTrue();
        }

        #region Helper Methods

        /// <summary>
        /// 獲取下一個週六的日期
        /// </summary>
        private static DateTime GetNextSaturday()
        {
            var today = DateTime.Now.Date;
            var daysUntilSaturday = ((int)DayOfWeek.Saturday - (int)today.DayOfWeek + 7) % 7;
            if (daysUntilSaturday == 0) // 今天就是週六
                return today;
            return today.AddDays(daysUntilSaturday);
        }

        /// <summary>
        /// 獲取指定星期幾的日期
        /// </summary>
        private static DateTime GetDateWithDayOfWeek(DayOfWeek targetDayOfWeek)
        {
            var today = DateTime.Now.Date;
            var diff = (int)targetDayOfWeek - (int)today.DayOfWeek;
            return today.AddDays(diff);
        }

        #endregion
    }
}
