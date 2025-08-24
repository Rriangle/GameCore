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
    /// 管理員控制器測試類別
    /// 測試所有管理員相關API的功能和邊界條件
    /// 驗證管理員認證、角色權限管理、使用者治理、系統監控等完整管理功能
    /// </summary>
    public class AdminControllerTests
    {
        private readonly Mock<IAdminService> _mockAdminService;
        private readonly Mock<ILogger<AdminController>> _mockLogger;
        private readonly AdminController _controller;

        public AdminControllerTests()
        {
            _mockAdminService = new Mock<IAdminService>();
            _mockLogger = new Mock<ILogger<AdminController>>();
            _controller = new AdminController(_mockAdminService.Object, _mockLogger.Object);

            // 設定模擬的管理員身份
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, "1"),
                new Claim(ClaimTypes.Name, "admin"),
                new Claim(ClaimTypes.Role, "Admin"),
                new Claim("AdministratorPrivilegesManagement", "true"),
                new Claim("UserStatusManagement", "true")
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

        #region 管理員認證測試

        [Fact]
        public async Task Login_ShouldReturnSuccess_WhenValidCredentials()
        {
            // Arrange
            var loginDto = new ManagerLoginDto
            {
                ManagerAccount = "admin001",
                ManagerPassword = "password123"
            };

            var expectedResponse = new ManagerLoginResponseDto
            {
                Token = "Bearer_1_20240824",
                Manager = new ManagerDataDto
                {
                    ManagerId = 1,
                    ManagerName = "系統管理員",
                    ManagerAccount = "admin001"
                },
                Permissions = new ManagerPermissionSummaryDto
                {
                    AdministratorPrivilegesManagement = true,
                    UserStatusManagement = true
                },
                TokenExpiry = DateTime.UtcNow.AddHours(8)
            };

            var expectedResult = AdminServiceResult<ManagerLoginResponseDto>.CreateSuccess(
                expectedResponse, "登入成功");

            _mockAdminService
                .Setup(s => s.LoginAsync(loginDto))
                .ReturnsAsync(expectedResult);

            // Act
            var result = await _controller.Login(loginDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = okResult.Value;
            
            var responseType = response.GetType();
            var successProperty = responseType.GetProperty("success");
            var dataProperty = responseType.GetProperty("data");
            
            Assert.True((bool)successProperty.GetValue(response));
            
            var data = dataProperty.GetValue(response) as ManagerLoginResponseDto;
            Assert.NotNull(data);
            Assert.Equal("admin001", data.Manager.ManagerAccount);
        }

        [Fact]
        public async Task Login_ShouldReturnFailure_WhenInvalidCredentials()
        {
            // Arrange
            var loginDto = new ManagerLoginDto
            {
                ManagerAccount = "admin001",
                ManagerPassword = "wrongpassword"
            };

            var expectedResult = AdminServiceResult<ManagerLoginResponseDto>.CreateFailure(
                "帳號或密碼錯誤");

            _mockAdminService
                .Setup(s => s.LoginAsync(loginDto))
                .ReturnsAsync(expectedResult);

            // Act
            var result = await _controller.Login(loginDto);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var response = badRequestResult.Value;
            
            var responseType = response.GetType();
            var successProperty = responseType.GetProperty("success");
            var messageProperty = responseType.GetProperty("message");
            
            Assert.False((bool)successProperty.GetValue(response));
            Assert.Equal("帳號或密碼錯誤", messageProperty.GetValue(response));
        }

        [Fact]
        public async Task GetCurrentManager_ShouldReturnManagerInfo_WhenAuthenticated()
        {
            // Arrange
            var expectedManager = new ManagerDataDto
            {
                ManagerId = 1,
                ManagerName = "系統管理員",
                ManagerAccount = "admin001",
                PermissionSummary = new ManagerPermissionSummaryDto
                {
                    AdministratorPrivilegesManagement = true,
                    UserStatusManagement = true
                }
            };

            _mockAdminService
                .Setup(s => s.GetManagerDetailAsync(1))
                .ReturnsAsync(expectedManager);

            // Act
            var result = await _controller.GetCurrentManager();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = okResult.Value;
            
            var responseType = response.GetType();
            var dataProperty = responseType.GetProperty("data");
            
            var data = dataProperty.GetValue(response) as ManagerDataDto;
            Assert.NotNull(data);
            Assert.Equal("admin001", data.ManagerAccount);
        }

        #endregion

        #region 管理員管理測試

        [Fact]
        public async Task GetManagers_ShouldReturnManagerList_WhenValidRequest()
        {
            // Arrange
            var expectedManagers = new AdminPagedResult<ManagerDataDto>
            {
                Page = 1,
                PageSize = 20,
                TotalCount = 5,
                Data = new List<ManagerDataDto>
                {
                    new() { ManagerId = 1, ManagerName = "系統管理員", ManagerAccount = "admin001" },
                    new() { ManagerId = 2, ManagerName = "商城管理員", ManagerAccount = "admin002" }
                }
            };

            _mockAdminService
                .Setup(s => s.GetManagersAsync(1, 20, true))
                .ReturnsAsync(expectedManagers);

            // Act
            var result = await _controller.GetManagers();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = okResult.Value;
            
            var responseType = response.GetType();
            var dataProperty = responseType.GetProperty("data");
            
            var data = dataProperty.GetValue(response) as AdminPagedResult<ManagerDataDto>;
            Assert.NotNull(data);
            Assert.Equal(5, data.TotalCount);
            Assert.Equal(2, data.Data.Count);
        }

        [Fact]
        public async Task CreateManager_ShouldReturnSuccess_WhenValidRequest()
        {
            // Arrange
            var createDto = new CreateManagerDto
            {
                ManagerName = "新管理員",
                ManagerAccount = "admin003",
                ManagerPassword = "password123",
                RoleIds = new List<int> { 1, 2 }
            };

            var expectedManager = new ManagerDataDto
            {
                ManagerId = 3,
                ManagerName = "新管理員",
                ManagerAccount = "admin003"
            };

            var expectedResult = AdminServiceResult<ManagerDataDto>.CreateSuccess(
                expectedManager, "管理員建立成功");

            _mockAdminService
                .Setup(s => s.CreateManagerAsync(createDto))
                .ReturnsAsync(expectedResult);

            _mockAdminService
                .Setup(s => s.LogOperationAsync(
                    It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(),
                    It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(),
                    It.IsAny<object>(), It.IsAny<string>()))
                .ReturnsAsync(AdminServiceResult.CreateSuccess("操作日誌記錄成功"));

            // Act
            var result = await _controller.CreateManager(createDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = okResult.Value;
            
            var responseType = response.GetType();
            var successProperty = responseType.GetProperty("success");
            var dataProperty = responseType.GetProperty("data");
            
            Assert.True((bool)successProperty.GetValue(response));
            
            var data = dataProperty.GetValue(response) as ManagerDataDto;
            Assert.NotNull(data);
            Assert.Equal("admin003", data.ManagerAccount);
        }

        #endregion

        #region 角色權限管理測試

        [Fact]
        public async Task GetRoles_ShouldReturnRoleList_WhenValidRequest()
        {
            // Arrange
            var expectedRoles = new List<ManagerRolePermissionDto>
            {
                new()
                {
                    ManagerRoleId = 1,
                    RoleName = "超級管理員",
                    AdministratorPrivilegesManagement = true,
                    UserStatusManagement = true,
                    ShoppingPermissionManagement = true
                },
                new()
                {
                    ManagerRoleId = 2,
                    RoleName = "使用者管理員",
                    UserStatusManagement = true
                }
            };

            _mockAdminService
                .Setup(s => s.GetRolePermissionsAsync())
                .ReturnsAsync(expectedRoles);

            // Act
            var result = await _controller.GetRoles();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = okResult.Value;
            
            var responseType = response.GetType();
            var dataProperty = responseType.GetProperty("data");
            
            var data = dataProperty.GetValue(response) as List<ManagerRolePermissionDto>;
            Assert.NotNull(data);
            Assert.Equal(2, data.Count);
            Assert.Equal("超級管理員", data[0].RoleName);
        }

        [Fact]
        public async Task CreateRole_ShouldReturnSuccess_WhenValidRequest()
        {
            // Arrange
            var createDto = new CreateManagerRolePermissionDto
            {
                RoleName = "論壇管理員",
                MessagePermissionManagement = true,
                CustomerService = true
            };

            var expectedRole = new ManagerRolePermissionDto
            {
                ManagerRoleId = 3,
                RoleName = "論壇管理員",
                MessagePermissionManagement = true,
                CustomerService = true
            };

            var expectedResult = AdminServiceResult<ManagerRolePermissionDto>.CreateSuccess(
                expectedRole, "角色權限建立成功");

            _mockAdminService
                .Setup(s => s.CreateRolePermissionAsync(createDto))
                .ReturnsAsync(expectedResult);

            _mockAdminService
                .Setup(s => s.LogOperationAsync(
                    It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(),
                    It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(),
                    It.IsAny<object>(), It.IsAny<string>()))
                .ReturnsAsync(AdminServiceResult.CreateSuccess("操作日誌記錄成功"));

            // Act
            var result = await _controller.CreateRole(createDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = okResult.Value;
            
            var responseType = response.GetType();
            var successProperty = responseType.GetProperty("success");
            var dataProperty = responseType.GetProperty("data");
            
            Assert.True((bool)successProperty.GetValue(response));
            
            var data = dataProperty.GetValue(response) as ManagerRolePermissionDto;
            Assert.NotNull(data);
            Assert.Equal("論壇管理員", data.RoleName);
        }

        #endregion

        #region 使用者治理測試

        [Fact]
        public async Task GetUsers_ShouldReturnUserList_WhenValidRequest()
        {
            // Arrange
            var expectedUsers = new AdminPagedResult<UserManagementDto>
            {
                Page = 1,
                PageSize = 20,
                TotalCount = 100,
                Data = new List<UserManagementDto>
                {
                    new()
                    {
                        UserId = 1,
                        UserAccount = "user001",
                        UserName = "測試用戶1",
                        UserRights = new UserRightsDto { AccountStatus = 1, CommentPermission = 1 }
                    },
                    new()
                    {
                        UserId = 2,
                        UserAccount = "user002",
                        UserName = "測試用戶2",
                        UserRights = new UserRightsDto { AccountStatus = 0, CommentPermission = 0 }
                    }
                }
            };

            _mockAdminService
                .Setup(s => s.GetUsersForManagementAsync(1, 20, null, null))
                .ReturnsAsync(expectedUsers);

            // Act
            var result = await _controller.GetUsers();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = okResult.Value;
            
            var responseType = response.GetType();
            var dataProperty = responseType.GetProperty("data");
            
            var data = dataProperty.GetValue(response) as AdminPagedResult<UserManagementDto>;
            Assert.NotNull(data);
            Assert.Equal(100, data.TotalCount);
            Assert.Equal(2, data.Data.Count);
        }

        [Fact]
        public async Task UpdateUserRights_ShouldReturnSuccess_WhenValidRequest()
        {
            // Arrange
            var updateDto = new UpdateUserRightsDto
            {
                AccountStatus = 0,
                CommentPermission = 0,
                Reason = "違反社群規範"
            };

            var expectedResult = AdminServiceResult.CreateSuccess("使用者權限調整成功");

            _mockAdminService
                .Setup(s => s.UpdateUserRightsAsync(1, updateDto, 1))
                .ReturnsAsync(expectedResult);

            // Act
            var result = await _controller.UpdateUserRights(1, updateDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = okResult.Value;
            
            var responseType = response.GetType();
            var successProperty = responseType.GetProperty("success");
            var messageProperty = responseType.GetProperty("message");
            
            Assert.True((bool)successProperty.GetValue(response));
            Assert.Equal("使用者權限調整成功", messageProperty.GetValue(response));
        }

        [Fact]
        public async Task AdjustUserPoints_ShouldReturnSuccess_WhenValidRequest()
        {
            // Arrange
            var adjustDto = new AdjustUserPointsDto
            {
                Delta = 1000,
                Reason = "活動獎勵"
            };

            var expectedResult = AdminServiceResult.CreateSuccess("使用者點數調整成功");

            _mockAdminService
                .Setup(s => s.AdjustUserPointsAsync(1, adjustDto, 1))
                .ReturnsAsync(expectedResult);

            // Act
            var result = await _controller.AdjustUserPoints(1, adjustDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = okResult.Value;
            
            var responseType = response.GetType();
            var successProperty = responseType.GetProperty("success");
            var messageProperty = responseType.GetProperty("message");
            
            Assert.True((bool)successProperty.GetValue(response));
            Assert.Equal("使用者點數調整成功", messageProperty.GetValue(response));
        }

        #endregion

        #region 儀表板統計測試

        [Fact]
        public async Task GetDashboard_ShouldReturnDashboardData_WhenValidRequest()
        {
            // Arrange
            var expectedDashboard = new AdminDashboardDto
            {
                TotalUsers = 1000,
                TodayNewUsers = 25,
                ActiveUsers = 450,
                TotalOrders = 5000,
                TodayOrders = 120,
                PendingAudits = 15,
                SystemAlerts = 3,
                TotalRevenue = 250000.00m,
                MonthlyRevenue = 45000.00m,
                SystemHealth = new SystemHealthDto
                {
                    OverallHealth = 95,
                    CpuUsage = 45.2,
                    MemoryUsage = 62.8,
                    DiskUsage = 35.1,
                    DatabaseConnected = true
                }
            };

            _mockAdminService
                .Setup(s => s.GetAdminDashboardAsync())
                .ReturnsAsync(expectedDashboard);

            // Act
            var result = await _controller.GetDashboard();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = okResult.Value;
            
            var responseType = response.GetType();
            var dataProperty = responseType.GetProperty("data");
            
            var data = dataProperty.GetValue(response) as AdminDashboardDto;
            Assert.NotNull(data);
            Assert.Equal(1000, data.TotalUsers);
            Assert.Equal(25, data.TodayNewUsers);
            Assert.True(data.SystemHealth.DatabaseConnected);
        }

        [Fact]
        public async Task GetSystemHealth_ShouldReturnHealthStatus_WhenValidRequest()
        {
            // Arrange
            var expectedHealth = new SystemHealthDto
            {
                OverallHealth = 98,
                CpuUsage = 35.5,
                MemoryUsage = 55.2,
                DiskUsage = 28.7,
                DatabaseConnected = true,
                ExternalServices = new Dictionary<string, bool>
                {
                    ["Database"] = true,
                    ["Redis"] = true,
                    ["Email"] = false
                },
                LastCheckTime = DateTime.UtcNow
            };

            _mockAdminService
                .Setup(s => s.GetSystemHealthAsync())
                .ReturnsAsync(expectedHealth);

            // Act
            var result = await _controller.GetSystemHealth();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = okResult.Value;
            
            var responseType = response.GetType();
            var dataProperty = responseType.GetProperty("data");
            
            var data = dataProperty.GetValue(response) as SystemHealthDto;
            Assert.NotNull(data);
            Assert.Equal(98, data.OverallHealth);
            Assert.True(data.DatabaseConnected);
            Assert.False(data.ExternalServices["Email"]);
        }

        #endregion

        #region 錯誤處理測試

        [Fact]
        public async Task Login_ShouldReturnBadRequest_WhenInvalidModel()
        {
            // Arrange
            var loginDto = new ManagerLoginDto(); // 空的DTO，缺少必填欄位

            _controller.ModelState.AddModelError("ManagerAccount", "管理員帳號為必填");

            // Act
            var result = await _controller.Login(loginDto);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var response = badRequestResult.Value;
            
            var responseType = response.GetType();
            var successProperty = responseType.GetProperty("success");
            var messageProperty = responseType.GetProperty("message");
            
            Assert.False((bool)successProperty.GetValue(response));
            Assert.Equal("登入資料格式錯誤", messageProperty.GetValue(response));
        }

        [Fact]
        public async Task GetManagers_ShouldReturnServerError_WhenServiceThrows()
        {
            // Arrange
            _mockAdminService
                .Setup(s => s.GetManagersAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<bool>()))
                .ThrowsAsync(new Exception("Database connection failed"));

            // Act
            var result = await _controller.GetManagers();

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, statusCodeResult.StatusCode);
        }

        #endregion
    }
}