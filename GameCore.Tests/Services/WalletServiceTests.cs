using GameCore.Core.DTOs;
using GameCore.Core.Interfaces;
using GameCore.Core.Services;
using GameCore.Infrastructure.Data;
using GameCore.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace GameCore.Tests.Services
{
    /// <summary>
    /// 錢包服務測試
    /// </summary>
    public class WalletServiceTests
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<INotificationService> _mockNotificationService;
        private readonly Mock<ILogger<WalletService>> _mockLogger;
        private readonly WalletService _walletService;

        public WalletServiceTests()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockNotificationService = new Mock<INotificationService>();
            _mockLogger = new Mock<ILogger<WalletService>>();
            _walletService = new WalletService(_mockUnitOfWork.Object, _mockNotificationService.Object, _mockLogger.Object);
        }

        /// <summary>
        /// 測試取得錢包餘額 - 錢包存在
        /// </summary>
        [Fact]
        public async Task GetWalletBalanceAsync_ExistingWallet_ReturnsBalance()
        {
            // Arrange
            var userId = 1;
            var mockWalletRepo = new Mock<IUserWalletRepository>();
            var existingWallet = new Infrastructure.Data.Entities.UserWallet
            {
                UserId = userId,
                UserPoint = 1000,
                CouponNumber = "COUPON123"
            };

            mockWalletRepo.Setup(r => r.GetByUserIdAsync(userId))
                .ReturnsAsync(existingWallet);

            _mockUnitOfWork.Setup(u => u.UserWalletRepository)
                .Returns(mockWalletRepo.Object);

            // Act
            var result = await _walletService.GetWalletBalanceAsync(userId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(userId, result.UserId);
            Assert.Equal(1000, result.UserPoint);
            Assert.Equal("COUPON123", result.CouponNumber);
        }

        /// <summary>
        /// 測試取得錢包餘額 - 錢包不存在，自動建立
        /// </summary>
        [Fact]
        public async Task GetWalletBalanceAsync_NonExistingWallet_CreatesNewWallet()
        {
            // Arrange
            var userId = 1;
            var mockWalletRepo = new Mock<IUserWalletRepository>();
            var mockUserRightsRepo = new Mock<IUserRightsRepository>();

            mockWalletRepo.Setup(r => r.GetByUserIdAsync(userId))
                .ReturnsAsync((Infrastructure.Data.Entities.UserWallet?)null);

            mockWalletRepo.Setup(r => r.AddAsync(It.IsAny<Infrastructure.Data.Entities.UserWallet>()))
                .Returns(Task.CompletedTask);

            _mockUnitOfWork.Setup(u => u.UserWalletRepository)
                .Returns(mockWalletRepo.Object);

            _mockUnitOfWork.Setup(u => u.SaveChangesAsync())
                .ReturnsAsync(1);

            // Act
            var result = await _walletService.GetWalletBalanceAsync(userId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(userId, result.UserId);
            Assert.Equal(0, result.UserPoint);
            Assert.Null(result.CouponNumber);

            mockWalletRepo.Verify(r => r.AddAsync(It.Is<Infrastructure.Data.Entities.UserWallet>(w => 
                w.UserId == userId && w.UserPoint == 0)), Times.Once);
        }

        /// <summary>
        /// 測試查詢點數流水記錄
        /// </summary>
        [Fact]
        public async Task GetPointTransactionsAsync_ValidRequest_ReturnsTransactions()
        {
            // Arrange
            var userId = 1;
            var request = new PointTransactionQueryRequest
            {
                Page = 1,
                PageSize = 10
            };

            var mockSignInRepo = new Mock<ISignInRepository>();
            var mockMiniGameRepo = new Mock<IMiniGameRepository>();
            var mockNotificationRepo = new Mock<INotificationRepository>();

            var signInRecords = new List<Infrastructure.Data.Entities.UserSignInStats>
            {
                new Infrastructure.Data.Entities.UserSignInStats
                {
                    LogID = 1,
                    UserID = userId,
                    SignTime = DateTime.UtcNow,
                    PointsChanged = 20,
                    ExpGained = 0
                }
            };

            var miniGameRecords = new List<Infrastructure.Data.Entities.MiniGame>
            {
                new Infrastructure.Data.Entities.MiniGame
                {
                    PlayID = 1,
                    UserID = userId,
                    StartTime = DateTime.UtcNow,
                    PointsChanged = 100,
                    Result = "Win"
                }
            };

            mockSignInRepo.Setup(r => r.GetByUserIdAsync(userId))
                .ReturnsAsync(signInRecords);

            mockMiniGameRepo.Setup(r => r.GetByUserIdAsync(userId))
                .ReturnsAsync(miniGameRecords);

            mockNotificationRepo.Setup(r => r.GetByUserAndActionAsync(userId, "pet_color_change"))
                .ReturnsAsync(new List<Infrastructure.Data.Entities.Notifications>());

            mockNotificationRepo.Setup(r => r.GetByUserAndActionAsync(userId, "points_adjustment"))
                .ReturnsAsync(new List<Infrastructure.Data.Entities.Notifications>());

            _mockUnitOfWork.Setup(u => u.SignInRepository)
                .Returns(mockSignInRepo.Object);

            _mockUnitOfWork.Setup(u => u.MiniGameRepository)
                .Returns(mockMiniGameRepo.Object);

            _mockUnitOfWork.Setup(u => u.NotificationRepository)
                .Returns(mockNotificationRepo.Object);

            // Act
            var result = await _walletService.GetPointTransactionsAsync(userId, request);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Transactions.Count);
            Assert.Equal(2, result.TotalCount);
            Assert.Equal(1, result.TotalPages);
            Assert.Equal(1, result.CurrentPage);
            Assert.Equal(10, result.PageSize);

            var signinTransaction = result.Transactions.First(t => t.TransactionType == "signin");
            Assert.Equal(20, signinTransaction.PointsChanged);
            Assert.Equal("每日簽到獎勵", signinTransaction.Description);

            var minigameTransaction = result.Transactions.First(t => t.TransactionType == "minigame");
            Assert.Equal(100, minigameTransaction.PointsChanged);
            Assert.Contains("Win", minigameTransaction.Description);
        }

        /// <summary>
        /// 測試申請銷售權限
        /// </summary>
        [Fact]
        public async Task ApplySalesPermissionAsync_ValidRequest_ReturnsApplication()
        {
            // Arrange
            var userId = 1;
            var request = new SalesPermissionRequest
            {
                BankCode = 004,
                BankAccountNumber = "1234567890",
                AccountCoverPhoto = "base64encodedimage",
                ApplicationNote = "想要銷售遊戲道具"
            };

            var mockMemberSalesProfileRepo = new Mock<IMemberSalesProfileRepository>();
            var mockUserSalesInfoRepo = new Mock<IUserSalesInformationRepository>();

            mockMemberSalesProfileRepo.Setup(r => r.GetByUserIdAsync(userId))
                .ReturnsAsync((Infrastructure.Data.Entities.MemberSalesProfile?)null);

            mockMemberSalesProfileRepo.Setup(r => r.AddAsync(It.IsAny<Infrastructure.Data.Entities.MemberSalesProfile>()))
                .Returns(Task.CompletedTask);

            mockUserSalesInfoRepo.Setup(r => r.AddAsync(It.IsAny<Infrastructure.Data.Entities.UserSalesInformation>()))
                .Returns(Task.CompletedTask);

            _mockUnitOfWork.Setup(u => u.MemberSalesProfileRepository)
                .Returns(mockMemberSalesProfileRepo.Object);

            _mockUnitOfWork.Setup(u => u.UserSalesInformationRepository)
                .Returns(mockUserSalesInfoRepo.Object);

            _mockUnitOfWork.Setup(u => u.SaveChangesAsync())
                .ReturnsAsync(1);

            // Act
            var result = await _walletService.ApplySalesPermissionAsync(userId, request);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(userId, result.ApplicationId);
            Assert.Equal("pending", result.Status);

            mockMemberSalesProfileRepo.Verify(r => r.AddAsync(It.Is<Infrastructure.Data.Entities.MemberSalesProfile>(p => 
                p.UserId == userId && p.BankCode == 004)), Times.Once);

            mockUserSalesInfoRepo.Verify(r => r.AddAsync(It.Is<Infrastructure.Data.Entities.UserSalesInformation>(s => 
                s.UserId == userId)), Times.Once);
        }

        /// <summary>
        /// 測試申請銷售權限 - 已有申請記錄
        /// </summary>
        [Fact]
        public async Task ApplySalesPermissionAsync_ExistingApplication_ThrowsException()
        {
            // Arrange
            var userId = 1;
            var request = new SalesPermissionRequest
            {
                BankCode = 004,
                BankAccountNumber = "1234567890",
                AccountCoverPhoto = "base64encodedimage"
            };

            var mockMemberSalesProfileRepo = new Mock<IMemberSalesProfileRepository>();
            var existingProfile = new Infrastructure.Data.Entities.MemberSalesProfile
            {
                UserId = userId,
                BankCode = 004,
                BankAccountNumber = "1234567890"
            };

            mockMemberSalesProfileRepo.Setup(r => r.GetByUserIdAsync(userId))
                .ReturnsAsync(existingProfile);

            _mockUnitOfWork.Setup(u => u.MemberSalesProfileRepository)
                .Returns(mockMemberSalesProfileRepo.Object);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<InvalidOperationException>(
                () => _walletService.ApplySalesPermissionAsync(userId, request));

            Assert.Contains("已經有銷售權限申請記錄", exception.Message);
        }

        /// <summary>
        /// 測試檢查銷售權限
        /// </summary>
        [Fact]
        public async Task HasSalesAuthorityAsync_UserHasAuthority_ReturnsTrue()
        {
            // Arrange
            var userId = 1;
            var mockUserRightsRepo = new Mock<IUserRightsRepository>();
            var userRights = new Infrastructure.Data.Entities.UserRights
            {
                UserId = userId,
                SalesAuthority = true
            };

            mockUserRightsRepo.Setup(r => r.GetByUserIdAsync(userId))
                .ReturnsAsync(userRights);

            _mockUnitOfWork.Setup(u => u.UserRightsRepository)
                .Returns(mockUserRightsRepo.Object);

            // Act
            var result = await _walletService.HasSalesAuthorityAsync(userId);

            // Assert
            Assert.True(result);
        }

        /// <summary>
        /// 測試檢查銷售權限 - 用戶無權限
        /// </summary>
        [Fact]
        public async Task HasSalesAuthorityAsync_UserNoAuthority_ReturnsFalse()
        {
            // Arrange
            var userId = 1;
            var mockUserRightsRepo = new Mock<IUserRightsRepository>();
            var userRights = new Infrastructure.Data.Entities.UserRights
            {
                UserId = userId,
                SalesAuthority = false
            };

            mockUserRightsRepo.Setup(r => r.GetByUserIdAsync(userId))
                .ReturnsAsync(userRights);

            _mockUnitOfWork.Setup(u => u.UserRightsRepository)
                .Returns(mockUserRightsRepo.Object);

            // Act
            var result = await _walletService.HasSalesAuthorityAsync(userId);

            // Assert
            Assert.False(result);
        }

        /// <summary>
        /// 測試檢查銷售權限 - 權限記錄不存在
        /// </summary>
        [Fact]
        public async Task HasSalesAuthorityAsync_NoRightsRecord_ReturnsFalse()
        {
            // Arrange
            var userId = 1;
            var mockUserRightsRepo = new Mock<IUserRightsRepository>();

            mockUserRightsRepo.Setup(r => r.GetByUserIdAsync(userId))
                .ReturnsAsync((Infrastructure.Data.Entities.UserRights?)null);

            _mockUnitOfWork.Setup(u => u.UserRightsRepository)
                .Returns(mockUserRightsRepo.Object);

            // Act
            var result = await _walletService.HasSalesAuthorityAsync(userId);

            // Assert
            Assert.False(result);
        }
    }
} 