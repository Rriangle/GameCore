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
    /// 每日簽到控制器測試類別
    /// 測試所有簽到相關API的功能和邊界條件
    /// 驗證Asia/Taipei時區處理和獎勵計算邏輯
    /// </summary>
    public class DailySignInControllerTests
    {
        private readonly Mock<IDailySignInService> _mockDailySignInService;
        private readonly Mock<ILogger<DailySignInController>> _mockLogger;
        private readonly DailySignInController _controller;

        public DailySignInControllerTests()
        {
            _mockDailySignInService = new Mock<IDailySignInService>();
            _mockLogger = new Mock<ILogger<DailySignInController>>();
            _controller = new DailySignInController(_mockDailySignInService.Object, _mockLogger.Object);

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

        #region 簽到狀態查詢測試

        [Fact]
        public async Task GetSignInStatus_ShouldReturnStatus_WhenUserExists()
        {
            // Arrange
            var expectedStatus = new SignInStatusDto
            {
                UserId = 123,
                TodaySigned = false,
                CurrentStreak = 5,
                TaipeiDate = DateTime.Today,
                TaipeiDateTime = DateTime.Now,
                IsWeekend = false,
                CanSignToday = true,
                TodayPotentialRewards = new SignInRewards
                {
                    Points = 20,
                    Experience = 0
                }
            };

            _mockDailySignInService
                .Setup(s => s.GetSignInStatusAsync(123))
                .ReturnsAsync(expectedStatus);

            // Act
            var result = await _controller.GetSignInStatus();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = okResult.Value;
            
            Assert.NotNull(response);
            
            var responseType = response.GetType();
            var successProperty = responseType.GetProperty("success");
            var dataProperty = responseType.GetProperty("data");
            var messageProperty = responseType.GetProperty("message");
            
            Assert.True((bool)successProperty.GetValue(response));
            Assert.Equal("簽到狀態取得成功", messageProperty.GetValue(response));
            Assert.NotNull(dataProperty.GetValue(response));
        }

        [Fact]
        public async Task GetSignInStatus_ShouldReturnServerError_WhenServiceThrows()
        {
            // Arrange
            _mockDailySignInService
                .Setup(s => s.GetSignInStatusAsync(123))
                .ThrowsAsync(new Exception("Database connection failed"));

            // Act
            var result = await _controller.GetSignInStatus();

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, statusCodeResult.StatusCode);
            
            var response = statusCodeResult.Value;
            var responseType = response.GetType();
            var successProperty = responseType.GetProperty("success");
            var messageProperty = responseType.GetProperty("message");
            
            Assert.False((bool)successProperty.GetValue(response));
            Assert.Equal("取得簽到狀態時發生錯誤", messageProperty.GetValue(response));
        }

        #endregion

        #region 簽到執行測試

        [Fact]
        public async Task PerformSignIn_ShouldReturnSuccess_WhenSignInSucceeds()
        {
            // Arrange
            var expectedResult = new SignInResultDto
            {
                Success = true,
                Message = "簽到成功！",
                PointsEarned = 20,
                ExperienceGained = 0,
                StreakBefore = 4,
                StreakAfter = 5,
                IsWeekend = false,
                HasSevenDayBonus = false,
                HasMonthlyBonus = false,
                SignInTime = DateTime.Now
            };

            _mockDailySignInService
                .Setup(s => s.PerformSignInAsync(123))
                .ReturnsAsync(expectedResult);

            // Act
            var result = await _controller.PerformSignIn();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = okResult.Value;
            
            var responseType = response.GetType();
            var successProperty = responseType.GetProperty("success");
            var dataProperty = responseType.GetProperty("data");
            var messageProperty = responseType.GetProperty("message");
            
            Assert.True((bool)successProperty.GetValue(response));
            Assert.Equal("簽到成功！", messageProperty.GetValue(response));
            Assert.NotNull(dataProperty.GetValue(response));
        }

        [Fact]
        public async Task PerformSignIn_ShouldReturnBadRequest_WhenAlreadySignedIn()
        {
            // Arrange
            var expectedResult = new SignInResultDto
            {
                Success = false,
                Message = "今日已經簽到過了"
            };

            _mockDailySignInService
                .Setup(s => s.PerformSignInAsync(123))
                .ReturnsAsync(expectedResult);

            // Act
            var result = await _controller.PerformSignIn();

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var response = badRequestResult.Value;
            
            var responseType = response.GetType();
            var successProperty = responseType.GetProperty("success");
            var messageProperty = responseType.GetProperty("message");
            
            Assert.False((bool)successProperty.GetValue(response));
            Assert.Equal("今日已經簽到過了", messageProperty.GetValue(response));
        }

        [Fact]
        public async Task PerformSignIn_ShouldReturnSuccessWithBonuses_WhenSevenDayStreak()
        {
            // Arrange
            var expectedResult = new SignInResultDto
            {
                Success = true,
                Message = "簽到成功！",
                PointsEarned = 60, // 20基礎 + 40獎勵
                ExperienceGained = 300,
                StreakBefore = 6,
                StreakAfter = 7,
                IsWeekend = false,
                HasSevenDayBonus = true,
                HasMonthlyBonus = false,
                BonusMessages = new List<string> { "連續簽到7天獎勵：+40點數 +300經驗" }
            };

            _mockDailySignInService
                .Setup(s => s.PerformSignInAsync(123))
                .ReturnsAsync(expectedResult);

            // Act
            var result = await _controller.PerformSignIn();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = okResult.Value;
            
            Assert.NotNull(response);
            
            // 驗證獲得了連續7天獎勵
            var data = response.GetType().GetProperty("data").GetValue(response) as SignInResultDto;
            Assert.NotNull(data);
            Assert.Equal(60, data.PointsEarned);
            Assert.Equal(300, data.ExperienceGained);
            Assert.True(data.HasSevenDayBonus);
        }

        [Fact]
        public async Task PerformSignIn_ShouldReturnSuccessWithWeekendBonus_WhenWeekend()
        {
            // Arrange
            var expectedResult = new SignInResultDto
            {
                Success = true,
                Message = "簽到成功！",
                PointsEarned = 30, // 週末獎勵
                ExperienceGained = 200, // 週末經驗
                StreakBefore = 2,
                StreakAfter = 3,
                IsWeekend = true,
                HasSevenDayBonus = false,
                HasMonthlyBonus = false
            };

            _mockDailySignInService
                .Setup(s => s.PerformSignInAsync(123))
                .ReturnsAsync(expectedResult);

            // Act
            var result = await _controller.PerformSignIn();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var data = okResult.Value.GetType().GetProperty("data").GetValue(okResult.Value) as SignInResultDto;
            
            Assert.NotNull(data);
            Assert.Equal(30, data.PointsEarned); // 週末點數
            Assert.Equal(200, data.ExperienceGained); // 週末經驗
            Assert.True(data.IsWeekend);
        }

        #endregion

        #region 月度統計測試

        [Fact]
        public async Task GetMonthlyAttendance_ShouldReturnCurrentMonth_WhenNoParameters()
        {
            // Arrange
            var expectedAttendance = new MonthlyAttendanceDto
            {
                Year = 2024,
                Month = 8,
                TotalSignedDays = 15,
                TotalDaysInMonth = 31,
                AttendanceRate = 48.39,
                TotalPointsEarned = 340,
                TotalExperienceGained = 800,
                IsPerfectAttendance = false
            };

            _mockDailySignInService
                .Setup(s => s.GetMonthAttendanceAsync(123, It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(expectedAttendance);

            // Act
            var result = await _controller.GetMonthlyAttendance();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = okResult.Value;
            
            var responseType = response.GetType();
            var successProperty = responseType.GetProperty("success");
            var dataProperty = responseType.GetProperty("data");
            
            Assert.True((bool)successProperty.GetValue(response));
            Assert.NotNull(dataProperty.GetValue(response));
        }

        [Fact]
        public async Task GetMonthlyAttendance_ShouldReturnSpecificMonth_WhenParametersProvided()
        {
            // Arrange
            var expectedAttendance = new MonthlyAttendanceDto
            {
                Year = 2024,
                Month = 7,
                TotalSignedDays = 31,
                TotalDaysInMonth = 31,
                AttendanceRate = 100,
                IsPerfectAttendance = true
            };

            _mockDailySignInService
                .Setup(s => s.GetMonthAttendanceAsync(123, 2024, 7))
                .ReturnsAsync(expectedAttendance);

            // Act
            var result = await _controller.GetMonthlyAttendance(2024, 7);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var data = okResult.Value.GetType().GetProperty("data").GetValue(okResult.Value) as MonthlyAttendanceDto;
            
            Assert.NotNull(data);
            Assert.Equal(2024, data.Year);
            Assert.Equal(7, data.Month);
            Assert.True(data.IsPerfectAttendance);
        }

        #endregion

        #region 簽到歷史測試

        [Fact]
        public async Task GetSignInHistory_ShouldReturnPagedHistory_WhenValidRequest()
        {
            // Arrange
            var expectedResult = new PagedResult<SignInRecordDto>
            {
                Items = new List<SignInRecordDto>
                {
                    new SignInRecordDto
                    {
                        LogId = 1,
                        SignTime = DateTime.Now.AddDays(-1),
                        PointsChanged = 20,
                        ExpGained = 0,
                        IsWeekend = false,
                        DayOfWeek = "Monday"
                    }
                },
                TotalCount = 50,
                CurrentPage = 1,
                PageSize = 20
            };

            _mockDailySignInService
                .Setup(s => s.GetSignInHistoryAsync(123, It.IsAny<SignInHistoryQueryDto>()))
                .ReturnsAsync(expectedResult);

            // Act
            var result = await _controller.GetSignInHistory();

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
        public async Task GetSignInHistory_ShouldLimitPageSize_WhenLargePageSizeRequested()
        {
            // Arrange
            var expectedResult = new PagedResult<SignInRecordDto>
            {
                Items = new List<SignInRecordDto>(),
                TotalCount = 0,
                CurrentPage = 1,
                PageSize = 100 // 限制在最大值
            };

            _mockDailySignInService
                .Setup(s => s.GetSignInHistoryAsync(123, It.Is<SignInHistoryQueryDto>(q => q.PageSize == 100)))
                .ReturnsAsync(expectedResult);

            // Act
            var result = await _controller.GetSignInHistory(pageSize: 200); // 請求超過限制

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            
            // 驗證服務被呼叫時PageSize被限制為100
            _mockDailySignInService.Verify(s => s.GetSignInHistoryAsync(
                123, 
                It.Is<SignInHistoryQueryDto>(q => q.PageSize == 100)
            ), Times.Once);
        }

        #endregion

        #region 簽到日曆測試

        [Fact]
        public async Task GetSignInCalendar_ShouldReturnCalendarData_WhenValidRequest()
        {
            // Arrange
            var expectedAttendance = new MonthlyAttendanceDto
            {
                Year = 2024,
                Month = 8,
                SignedDays = new List<DateTime>
                {
                    new DateTime(2024, 8, 1),
                    new DateTime(2024, 8, 2),
                    new DateTime(2024, 8, 3)
                },
                Records = new List<SignInRecordDto>
                {
                    new SignInRecordDto
                    {
                        LogId = 1,
                        SignTime = new DateTime(2024, 8, 1),
                        PointsChanged = 20,
                        ExpGained = 0
                    }
                }
            };

            _mockDailySignInService
                .Setup(s => s.GetMonthAttendanceAsync(123, 2024, 8))
                .ReturnsAsync(expectedAttendance);

            // Act
            var result = await _controller.GetSignInCalendar(2024, 8);

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

        #region 簽到統計測試

        [Fact]
        public async Task GetSignInSummary_ShouldReturnComprehensiveStats_WhenValidRequest()
        {
            // Arrange
            var currentMonth = new MonthlyAttendanceDto
            {
                Year = 2024,
                Month = 8,
                TotalSignedDays = 20,
                TotalPointsEarned = 500,
                TotalExperienceGained = 1000
            };

            var pastMonth = new MonthlyAttendanceDto
            {
                Year = 2024,
                Month = 7,
                TotalSignedDays = 31,
                TotalPointsEarned = 800,
                TotalExperienceGained = 2000
            };

            _mockDailySignInService
                .Setup(s => s.GetMonthAttendanceAsync(123, It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync((int userId, int year, int month) =>
                {
                    if (month == 8) return currentMonth;
                    if (month == 7) return pastMonth;
                    return new MonthlyAttendanceDto();
                });

            // Act
            var result = await _controller.GetSignInSummary();

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

        #region 管理員功能測試

        [Fact]
        public async Task AdminAdjustSignIn_ShouldReturnSuccess_WhenValidAdjustment()
        {
            // Arrange
            var adjustment = new AdminSignInAdjustmentDto
            {
                UserId = 456,
                AdjustmentDate = DateTime.Today,
                AdjustmentType = "add",
                Reason = "補簽系統維護期間",
                SendNotification = true
            };

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
            var result = await _controller.AdminAdjustSignIn(adjustment);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = okResult.Value;
            
            var responseType = response.GetType();
            var successProperty = responseType.GetProperty("success");
            var messageProperty = responseType.GetProperty("message");
            
            Assert.True((bool)successProperty.GetValue(response));
            Assert.Contains("簽到記錄調整成功", messageProperty.GetValue(response).ToString());
        }

        [Fact]
        public async Task AdminAdjustSignIn_ShouldReturnBadRequest_WhenInvalidModel()
        {
            // Arrange
            var adjustment = new AdminSignInAdjustmentDto
            {
                // 缺少必要欄位
                AdjustmentType = "",
                Reason = ""
            };

            _controller.ModelState.AddModelError("UserId", "使用者ID必填");
            _controller.ModelState.AddModelError("Reason", "調整原因必填");

            // Act
            var result = await _controller.AdminAdjustSignIn(adjustment);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var response = badRequestResult.Value;
            
            var responseType = response.GetType();
            var successProperty = responseType.GetProperty("success");
            var messageProperty = responseType.GetProperty("message");
            
            Assert.False((bool)successProperty.GetValue(response));
            Assert.Equal("調整資料格式錯誤", messageProperty.GetValue(response));
        }

        #endregion

        #region 錯誤處理測試

        [Fact]
        public async Task PerformSignIn_ShouldReturnServerError_WhenServiceThrows()
        {
            // Arrange
            _mockDailySignInService
                .Setup(s => s.PerformSignInAsync(123))
                .ThrowsAsync(new Exception("Database connection failed"));

            // Act
            var result = await _controller.PerformSignIn();

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, statusCodeResult.StatusCode);
        }

        [Fact]
        public async Task GetMonthlyAttendance_ShouldReturnServerError_WhenServiceThrows()
        {
            // Arrange
            _mockDailySignInService
                .Setup(s => s.GetMonthAttendanceAsync(123, It.IsAny<int>(), It.IsAny<int>()))
                .ThrowsAsync(new Exception("Database connection failed"));

            // Act
            var result = await _controller.GetMonthlyAttendance();

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, statusCodeResult.StatusCode);
        }

        #endregion

        #region 邊界條件測試

        [Fact]
        public async Task GetSignInHistory_ShouldHandleEmptyResult_WhenNoRecords()
        {
            // Arrange
            var emptyResult = new PagedResult<SignInRecordDto>
            {
                Items = new List<SignInRecordDto>(),
                TotalCount = 0,
                CurrentPage = 1,
                PageSize = 20
            };

            _mockDailySignInService
                .Setup(s => s.GetSignInHistoryAsync(123, It.IsAny<SignInHistoryQueryDto>()))
                .ReturnsAsync(emptyResult);

            // Act
            var result = await _controller.GetSignInHistory();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = okResult.Value;
            
            var dataProperty = response.GetType().GetProperty("data");
            var data = dataProperty.GetValue(response) as List<SignInRecordDto>;
            
            Assert.NotNull(data);
            Assert.Empty(data);
        }

        [Fact]
        public async Task GetMonthlyAttendance_ShouldHandleFutureMonth_WhenRequestingFutureDate()
        {
            // Arrange
            var futureYear = DateTime.Now.Year + 1;
            var futureMonth = 1;

            var emptyAttendance = new MonthlyAttendanceDto
            {
                Year = futureYear,
                Month = futureMonth,
                TotalSignedDays = 0,
                AttendanceRate = 0
            };

            _mockDailySignInService
                .Setup(s => s.GetMonthAttendanceAsync(123, futureYear, futureMonth))
                .ReturnsAsync(emptyAttendance);

            // Act
            var result = await _controller.GetMonthlyAttendance(futureYear, futureMonth);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = okResult.Value;
            
            Assert.NotNull(response);
            
            // 驗證服務被正確呼叫
            _mockDailySignInService.Verify(s => s.GetMonthAttendanceAsync(123, futureYear, futureMonth), Times.Once);
        }

        #endregion
    }
}