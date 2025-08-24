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
    /// 錢包控制器測試類別
    /// 測試錢包相關API的功能和安全性
    /// </summary>
    public class WalletControllerTests
    {
        private readonly Mock<IWalletService> _mockWalletService;
        private readonly Mock<ILogger<WalletController>> _mockLogger;
        private readonly WalletController _controller;

        public WalletControllerTests()
        {
            _mockWalletService = new Mock<IWalletService>();
            _mockLogger = new Mock<ILogger<WalletController>>();
            _controller = new WalletController(_mockWalletService.Object, _mockLogger.Object);

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

        #region 基本錢包功能測試

        [Fact]
        public async Task GetWallet_ShouldReturnWalletInfo_WhenUserExists()
        {
            // Arrange
            var expectedWallet = new WalletDto
            {
                UserId = 123,
                CurrentPoints = 1500,
                CouponNumber = "COUPON123",
                HasSalesAuthority = false,
                SalesWalletBalance = null
            };

            _mockWalletService
                .Setup(s => s.GetWalletAsync(123))
                .ReturnsAsync(expectedWallet);

            // Act
            var result = await _controller.GetWallet();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = okResult.Value;
            
            Assert.NotNull(response);
            
            // 驗證回應結構
            var responseType = response.GetType();
            var successProperty = responseType.GetProperty("success");
            var dataProperty = responseType.GetProperty("data");
            var messageProperty = responseType.GetProperty("message");
            
            Assert.NotNull(successProperty);
            Assert.NotNull(dataProperty);
            Assert.NotNull(messageProperty);
            
            Assert.True((bool)successProperty.GetValue(response));
            Assert.Equal("錢包資訊取得成功", messageProperty.GetValue(response));
        }

        [Fact]
        public async Task GetBalance_ShouldReturnBalance_WhenUserExists()
        {
            // Arrange
            var expectedWallet = new WalletDto
            {
                UserId = 123,
                CurrentPoints = 2500
            };

            _mockWalletService
                .Setup(s => s.GetWalletAsync(123))
                .ReturnsAsync(expectedWallet);

            // Act
            var result = await _controller.GetBalance();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = okResult.Value;
            
            Assert.NotNull(response);
            
            var responseType = response.GetType();
            var balanceProperty = responseType.GetProperty("balance");
            var successProperty = responseType.GetProperty("success");
            
            Assert.Equal(2500, balanceProperty.GetValue(response));
            Assert.True((bool)successProperty.GetValue(response));
        }

        [Fact]
        public async Task CheckSufficientPoints_ShouldReturnTrue_WhenUserHasEnoughPoints()
        {
            // Arrange
            _mockWalletService
                .Setup(s => s.HasSufficientPointsAsync(123, 100))
                .ReturnsAsync(true);

            // Act
            var result = await _controller.CheckSufficientPoints(100);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = okResult.Value;
            
            var responseType = response.GetType();
            var sufficientProperty = responseType.GetProperty("sufficient");
            var requiredPointsProperty = responseType.GetProperty("requiredPoints");
            
            Assert.True((bool)sufficientProperty.GetValue(response));
            Assert.Equal(100, requiredPointsProperty.GetValue(response));
        }

        [Fact]
        public async Task CheckSufficientPoints_ShouldReturnBadRequest_WhenPointsIsZeroOrNegative()
        {
            // Act
            var result = await _controller.CheckSufficientPoints(0);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var response = badRequestResult.Value;
            
            var responseType = response.GetType();
            var successProperty = responseType.GetProperty("success");
            var messageProperty = responseType.GetProperty("message");
            
            Assert.False((bool)successProperty.GetValue(response));
            Assert.Equal("檢查點數必須大於 0", messageProperty.GetValue(response));
        }

        #endregion

        #region 收支明細測試

        [Fact]
        public async Task GetLedgerHistory_ShouldReturnPagedResult_WhenValidRequest()
        {
            // Arrange
            var expectedResult = new PagedResult<LedgerEntryDto>
            {
                Items = new List<LedgerEntryDto>
                {
                    new LedgerEntryDto
                    {
                        Id = "signin_1",
                        Timestamp = DateTime.UtcNow.AddDays(-1),
                        Type = "signin",
                        PointsDelta = 20,
                        Description = "每日簽到獲得 20 點數"
                    }
                },
                TotalCount = 1,
                CurrentPage = 1,
                PageSize = 20
            };

            _mockWalletService
                .Setup(s => s.GetLedgerHistoryAsync(123, It.IsAny<LedgerQueryDto>()))
                .ReturnsAsync(expectedResult);

            // Act
            var result = await _controller.GetLedgerHistory();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = okResult.Value;
            
            var responseType = response.GetType();
            var successProperty = responseType.GetProperty("success");
            var dataProperty = responseType.GetProperty("data");
            var paginationProperty = responseType.GetProperty("pagination");
            
            Assert.True((bool)successProperty.GetValue(response));
            Assert.NotNull(dataProperty.GetValue(response));
            Assert.NotNull(paginationProperty.GetValue(response));
        }

        [Fact]
        public async Task GetPointsStatistics_ShouldReturnStatistics_WhenUserExists()
        {
            // Arrange
            var expectedStats = new PointsStatisticsDto
            {
                TotalPoints = 1500,
                TodayEarned = 50,
                TodaySpent = 20,
                WeekEarned = 200,
                WeekSpent = 100,
                MonthEarned = 800,
                MonthSpent = 300
            };

            _mockWalletService
                .Setup(s => s.GetPointsStatisticsAsync(123))
                .ReturnsAsync(expectedStats);

            // Act
            var result = await _controller.GetPointsStatistics();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = okResult.Value;
            
            var responseType = response.GetType();
            var successProperty = responseType.GetProperty("success");
            var dataProperty = responseType.GetProperty("data");
            
            Assert.True((bool)successProperty.GetValue(response));
            Assert.NotNull(dataProperty.GetValue(response));
        }

        #endregion

        #region 銷售功能測試

        [Fact]
        public async Task ApplySalesProfile_ShouldReturnSuccess_WhenValidRequest()
        {
            // Arrange
            var request = new CreateSalesProfileDto
            {
                BankCode = 1,
                BankAccountNumber = "12345678901234",
                ApplicationReason = "需要銷售遊戲道具"
            };

            var expectedResult = new MemberSalesProfileDto
            {
                UserId = 123,
                BankCode = 1,
                BankName = "台灣銀行",
                MaskedBankAccountNumber = "**********1234",
                SalesAuthorityEnabled = false,
                ReviewStatus = "pending"
            };

            _mockWalletService
                .Setup(s => s.ApplySalesProfileAsync(123, request))
                .ReturnsAsync(ServiceResult<MemberSalesProfileDto>.Success(expectedResult, "申請已提交"));

            // Act
            var result = await _controller.ApplySalesProfile(request);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = okResult.Value;
            
            var responseType = response.GetType();
            var successProperty = responseType.GetProperty("success");
            var messageProperty = responseType.GetProperty("message");
            
            Assert.True((bool)successProperty.GetValue(response));
            Assert.Equal("申請已提交", messageProperty.GetValue(response));
        }

        [Fact]
        public async Task ApplySalesProfile_ShouldReturnBadRequest_WhenServiceFails()
        {
            // Arrange
            var request = new CreateSalesProfileDto
            {
                BankCode = 1,
                BankAccountNumber = "12345678901234"
            };

            _mockWalletService
                .Setup(s => s.ApplySalesProfileAsync(123, request))
                .ReturnsAsync(ServiceResult<MemberSalesProfileDto>.Failure("您已經申請過銷售功能"));

            // Act
            var result = await _controller.ApplySalesProfile(request);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var response = badRequestResult.Value;
            
            var responseType = response.GetType();
            var successProperty = responseType.GetProperty("success");
            var messageProperty = responseType.GetProperty("message");
            
            Assert.False((bool)successProperty.GetValue(response));
            Assert.Equal("您已經申請過銷售功能", messageProperty.GetValue(response));
        }

        [Fact]
        public async Task GetSalesProfile_ShouldReturnNotFound_WhenProfileNotExists()
        {
            // Arrange
            _mockWalletService
                .Setup(s => s.GetSalesProfileAsync(123))
                .ReturnsAsync((MemberSalesProfileDto)null);

            // Act
            var result = await _controller.GetSalesProfile();

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            var response = notFoundResult.Value;
            
            var responseType = response.GetType();
            var successProperty = responseType.GetProperty("success");
            var messageProperty = responseType.GetProperty("message");
            
            Assert.False((bool)successProperty.GetValue(response));
            Assert.Equal("尚未申請銷售功能", messageProperty.GetValue(response));
        }

        #endregion

        #region 交易處理測試

        [Fact]
        public async Task SpendPoints_ShouldReturnSuccess_WhenValidRequest()
        {
            // Arrange
            var request = new SpendPointsRequestDto
            {
                Points = 100,
                Purpose = "寵物換色",
                ReferenceId = "pet_123"
            };

            var expectedTransaction = new WalletTransactionDto
            {
                TransactionId = "txn_123",
                UserId = 123,
                TransactionType = "spend",
                PointsDelta = -100,
                BalanceBefore = 500,
                BalanceAfter = 400,
                Description = "寵物換色",
                Status = "success"
            };

            _mockWalletService
                .Setup(s => s.SpendPointsAsync(123, 100, "寵物換色", "pet_123"))
                .ReturnsAsync(ServiceResult<WalletTransactionDto>.Success(expectedTransaction, "點數消費成功"));

            // Act
            var result = await _controller.SpendPoints(request);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = okResult.Value;
            
            var responseType = response.GetType();
            var successProperty = responseType.GetProperty("success");
            var messageProperty = responseType.GetProperty("message");
            
            Assert.True((bool)successProperty.GetValue(response));
            Assert.Equal("點數消費成功", messageProperty.GetValue(response));
        }

        [Fact]
        public async Task EarnPoints_ShouldReturnSuccess_WhenValidRequest()
        {
            // Arrange
            var request = new EarnPointsRequestDto
            {
                Points = 50,
                Source = "每日簽到",
                ReferenceId = "signin_123"
            };

            var expectedTransaction = new WalletTransactionDto
            {
                TransactionId = "txn_124",
                UserId = 123,
                TransactionType = "earn",
                PointsDelta = 50,
                BalanceBefore = 400,
                BalanceAfter = 450,
                Description = "每日簽到",
                Status = "success"
            };

            _mockWalletService
                .Setup(s => s.EarnPointsAsync(123, 50, "每日簽到", "signin_123"))
                .ReturnsAsync(ServiceResult<WalletTransactionDto>.Success(expectedTransaction, "點數獲得成功"));

            // Act
            var result = await _controller.EarnPoints(request);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = okResult.Value;
            
            var responseType = response.GetType();
            var successProperty = responseType.GetProperty("success");
            var messageProperty = responseType.GetProperty("message");
            
            Assert.True((bool)successProperty.GetValue(response));
            Assert.Equal("點數獲得成功", messageProperty.GetValue(response));
        }

        #endregion

        #region 錯誤處理測試

        [Fact]
        public async Task GetWallet_ShouldReturnInternalServerError_WhenServiceThrows()
        {
            // Arrange
            _mockWalletService
                .Setup(s => s.GetWalletAsync(123))
                .ThrowsAsync(new Exception("Database connection failed"));

            // Act
            var result = await _controller.GetWallet();

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, statusCodeResult.StatusCode);
            
            var response = statusCodeResult.Value;
            var responseType = response.GetType();
            var successProperty = responseType.GetProperty("success");
            var messageProperty = responseType.GetProperty("message");
            
            Assert.False((bool)successProperty.GetValue(response));
            Assert.Equal("取得錢包資訊時發生錯誤", messageProperty.GetValue(response));
        }

        #endregion

        #region 驗證測試

        [Fact]
        public async Task SpendPoints_ShouldReturnBadRequest_WhenModelStateInvalid()
        {
            // Arrange
            var request = new SpendPointsRequestDto
            {
                Points = 0, // 無效值
                Purpose = "", // 空字串
                ReferenceId = null
            };

            _controller.ModelState.AddModelError("Points", "消費點數必須大於 0");
            _controller.ModelState.AddModelError("Purpose", "消費目的必填");

            // Act
            var result = await _controller.SpendPoints(request);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var response = badRequestResult.Value;
            
            var responseType = response.GetType();
            var successProperty = responseType.GetProperty("success");
            var messageProperty = responseType.GetProperty("message");
            
            Assert.False((bool)successProperty.GetValue(response));
            Assert.Equal("消費資料格式錯誤", messageProperty.GetValue(response));
        }

        #endregion
    }
}

/// <summary>
/// 服務結果包裝類別 (用於測試)
/// </summary>
/// <typeparam name="T">結果資料類型</typeparam>
public class ServiceResult<T>
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public T? Data { get; set; }

    public static ServiceResult<T> Success(T data, string message = "")
    {
        return new ServiceResult<T>
        {
            Success = true,
            Data = data,
            Message = message
        };
    }

    public static ServiceResult<T> Failure(string message)
    {
        return new ServiceResult<T>
        {
            Success = false,
            Message = message,
            Data = default(T)
        };
    }
}