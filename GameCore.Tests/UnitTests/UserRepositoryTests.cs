using Xunit;
using FluentAssertions;
using GameCore.Infrastructure.Repositories;
using GameCore.Infrastructure.Data;
using GameCore.Core.Entities;
using GameCore.Tests.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace GameCore.Tests.UnitTests
{
    /// <summary>
    /// 使用者 Repository 單元測試
    /// 測試使用者資料存取的各種操作
    /// </summary>
    public class UserRepositoryTests : IDisposable
    {
        private readonly GameCoreDbContext _context;
        private readonly UserRepository _userRepository;

        public UserRepositoryTests()
        {
            _context = TestDbContextFactory.CreateInMemoryDatabase();
            _userRepository = new UserRepository(_context);
        }

        public void Dispose()
        {
            TestDbContextFactory.CleanupDatabase(_context);
        }

        [Fact]
        public async Task AddAsync_新增使用者_應該成功保存()
        {
            // Arrange
            var user = CreateTestUser();

            // Act
            await _userRepository.AddAsync(user);
            await _context.SaveChangesAsync();

            // Assert
            var savedUser = await _context.Users.FindAsync(user.UserId);
            savedUser.Should().NotBeNull();
            savedUser!.UserAccount.Should().Be(user.UserAccount);
            savedUser.Email.Should().Be(user.Email);
        }

        [Fact]
        public async Task GetByAccountAsync_根據帳號查詢_應該返回正確使用者()
        {
            // Arrange
            var user = CreateTestUser();
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            // Act
            var result = await _userRepository.GetByAccountAsync(user.UserAccount!);

            // Assert
            result.Should().NotBeNull();
            result!.UserId.Should().Be(user.UserId);
            result.UserAccount.Should().Be(user.UserAccount);
        }

        [Fact]
        public async Task GetByEmailAsync_根據Email查詢_應該返回正確使用者()
        {
            // Arrange
            var user = CreateTestUser();
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            // Act
            var result = await _userRepository.GetByEmailAsync(user.Email!);

            // Assert
            result.Should().NotBeNull();
            result!.UserId.Should().Be(user.UserId);
            result.Email.Should().Be(user.Email);
        }

        [Fact]
        public async Task GetUserWithAllDataAsync_查詢完整使用者資料_應該包含所有關聯資料()
        {
            // Arrange
            var user = CreateTestUserWithRelatedData();
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            // Act
            var result = await _userRepository.GetUserWithAllDataAsync(user.UserId);

            // Assert
            result.Should().NotBeNull();
            result!.UserIntroduce.Should().NotBeNull();
            result.UserWallet.Should().NotBeNull();
            result.UserRights.Should().NotBeNull();
        }

        [Fact]
        public async Task AddPointsAsync_增加使用者點數_應該正確更新錢包()
        {
            // Arrange
            var user = CreateTestUserWithWallet();
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            
            var initialPoints = user.UserWallet!.UserPoint;
            var pointsToAdd = 100;

            // Act
            await _userRepository.AddPointsAsync(user.UserId, pointsToAdd, "測試增加點數");

            // Assert
            var updatedWallet = await _context.UserWallets.FindAsync(user.UserWallet.UserId);
            updatedWallet.Should().NotBeNull();
            updatedWallet!.UserPoint.Should().Be(initialPoints + pointsToAdd);
        }

        [Fact]
        public async Task DeductPointsAsync_扣除點數足夠_應該成功扣除並返回true()
        {
            // Arrange
            var user = CreateTestUserWithWallet(1000); // 錢包有1000點
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            // Act
            var result = await _userRepository.DeductPointsAsync(user.UserId, 500, "測試扣除點數");

            // Assert
            result.Should().BeTrue();
            var updatedWallet = await _context.UserWallets.FindAsync(user.UserWallet!.UserId);
            updatedWallet!.UserPoint.Should().Be(500);
        }

        [Fact]
        public async Task DeductPointsAsync_扣除點數不足_應該失敗並返回false()
        {
            // Arrange
            var user = CreateTestUserWithWallet(100); // 錢包只有100點
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            // Act
            var result = await _userRepository.DeductPointsAsync(user.UserId, 500, "測試扣除點數");

            // Assert
            result.Should().BeFalse();
            var wallet = await _context.UserWallets.FindAsync(user.UserWallet!.UserId);
            wallet!.UserPoint.Should().Be(100); // 點數不應該改變
        }

        [Fact]
        public async Task AccountExistsAsync_帳號存在_應該返回true()
        {
            // Arrange
            var user = CreateTestUser();
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            // Act
            var result = await _userRepository.AccountExistsAsync(user.UserAccount!);

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public async Task AccountExistsAsync_帳號不存在_應該返回false()
        {
            // Act
            var result = await _userRepository.AccountExistsAsync("不存在的帳號");

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public async Task SearchUsersAsync_搜尋使用者_應該返回符合條件的結果()
        {
            // Arrange
            var users = new[]
            {
                CreateTestUser("testuser1", "測試使用者1", "test1@example.com"),
                CreateTestUser("testuser2", "測試使用者2", "test2@example.com"),
                CreateTestUser("anotheruser", "另一個使用者", "another@example.com")
            };

            foreach (var user in users)
            {
                user.UserIntroduce = new UserIntroduce
                {
                    User = user,
                    UserNickName = user.UserName + "_暱稱",
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };
            }

            await _context.Users.AddRangeAsync(users);
            await _context.SaveChangesAsync();

            // Act
            var result = await _userRepository.SearchUsersAsync("測試", 1, 10);

            // Assert
            result.Should().NotBeNull();
            result.Items.Should().HaveCount(2);
            result.Items.Should().OnlyContain(u => u.UserName!.Contains("測試"));
            result.TotalCount.Should().Be(2);
        }

        [Fact]
        public async Task GetPointsLeaderboardAsync_獲取點數排行榜_應該按點數降序排列()
        {
            // Arrange
            var users = new[]
            {
                CreateTestUserWithWallet(1000),
                CreateTestUserWithWallet(2000),
                CreateTestUserWithWallet(500)
            };

            await _context.Users.AddRangeAsync(users);
            await _context.SaveChangesAsync();

            // Act
            var result = await _userRepository.GetPointsLeaderboardAsync(3);

            // Assert
            result.Should().NotBeNull();
            var leaderboard = result.ToList();
            leaderboard.Should().HaveCount(3);
            leaderboard[0].UserPoint.Should().Be(2000);
            leaderboard[1].UserPoint.Should().Be(1000);
            leaderboard[2].UserPoint.Should().Be(500);
        }

        #region Helper Methods

        /// <summary>
        /// 創建測試使用者
        /// </summary>
        private static User CreateTestUser(string account = "testuser", string name = "測試使用者", string email = "test@example.com")
        {
            return new User
            {
                UserAccount = account,
                UserName = name,
                Email = email,
                UserLevel = 1,
                Points = 0,
                Experience = 0,
                DisplayName = name,
                RegistrationTime = DateTime.UtcNow,
                LastLoginTime = DateTime.UtcNow,
                IsOnline = false,
                Status = "Active"
            };
        }

        /// <summary>
        /// 創建包含關聯資料的測試使用者
        /// </summary>
        private static User CreateTestUserWithRelatedData()
        {
            var user = CreateTestUser();
            
            user.UserIntroduce = new UserIntroduce
            {
                User = user,
                UserNickName = "測試暱稱",
                UserSelfIntroduction = "這是測試用的自我介紹",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            user.UserWallet = new UserWallet
            {
                User = user,
                UserPoint = 100,
                UserCash = 50.5m,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            user.UserRights = new UserRights
            {
                User = user,
                CanPost = true,
                CanComment = true,
                CanUpload = true,
                CanTrade = true,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            return user;
        }

        /// <summary>
        /// 創建包含錢包的測試使用者
        /// </summary>
        private static User CreateTestUserWithWallet(int points = 100)
        {
            var user = CreateTestUser();
            
            user.UserWallet = new UserWallet
            {
                User = user,
                UserPoint = points,
                UserCash = 0,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            return user;
        }

        #endregion
    }
}
