using GameCore.Core.DTOs;
using GameCore.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace GameCore.Web.Controllers
{
    /// <summary>
    /// 管理員控制器 - 提供完整的後台管理API端點
    /// 實現管理員認證、角色權限管理、使用者治理、內容管理、系統監控等功能
    /// 嚴格按照規格要求實現後台頁面授權和操作稽核
    /// </summary>
    [ApiController]
    [Route("api/admin")]
    [Authorize(Roles = "Admin")]
    public class AdminController : ControllerBase
    {
        private readonly IAdminService _adminService;
        private readonly ILogger<AdminController> _logger;

        public AdminController(IAdminService adminService, ILogger<AdminController> logger)
        {
            _adminService = adminService;
            _logger = logger;
        }

        #region 管理員認證

        /// <summary>
        /// 管理員登入
        /// 驗證帳號密碼，更新last_login，產生JWT Token
        /// </summary>
        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] ManagerLoginDto loginDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new { success = false, message = "登入資料格式錯誤", errors = ModelState });
                }

                var result = await _adminService.LoginAsync(loginDto);
                
                if (result.Success)
                {
                    _logger.LogInformation("管理員登入成功: {Account}", loginDto.ManagerAccount);
                    return Ok(new { success = true, data = result.Result, message = "登入成功" });
                }
                else
                {
                    _logger.LogWarning("管理員登入失敗: {Account}, 原因: {Message}", 
                        loginDto.ManagerAccount, result.Message);
                    return BadRequest(new { success = false, message = result.Message, errors = result.Errors });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "管理員登入時發生錯誤: {Account}", loginDto.ManagerAccount);
                return StatusCode(500, new { success = false, message = "登入過程中發生錯誤" });
            }
        }

        /// <summary>
        /// 取得當前管理員資訊
        /// 驗證Token並返回管理員詳細資訊
        /// </summary>
        [HttpGet("me")]
        public async Task<IActionResult> GetCurrentManager()
        {
            try
            {
                var managerIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (!int.TryParse(managerIdClaim, out int managerId))
                {
                    return Unauthorized(new { success = false, message = "無效的管理員身份" });
                }

                var manager = await _adminService.GetManagerDetailAsync(managerId);
                if (manager == null)
                {
                    return NotFound(new { success = false, message = "管理員不存在" });
                }

                return Ok(new { success = true, data = manager, message = "管理員資訊取得成功" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "取得當前管理員資訊時發生錯誤");
                return StatusCode(500, new { success = false, message = "取得管理員資訊時發生錯誤" });
            }
        }

        #endregion

        #region 管理員管理

        /// <summary>
        /// 取得管理員列表
        /// 需要管理者權限管理權限
        /// </summary>
        [HttpGet("managers")]
        [Authorize(Policy = "AdministratorPrivilegesManagement")]
        public async Task<IActionResult> GetManagers(
            [FromQuery] int page = 1, 
            [FromQuery] int pageSize = 20, 
            [FromQuery] bool activeOnly = true)
        {
            try
            {
                var result = await _adminService.GetManagersAsync(page, pageSize, activeOnly);
                return Ok(new { success = true, data = result, message = "管理員列表取得成功" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "取得管理員列表時發生錯誤");
                return StatusCode(500, new { success = false, message = "取得管理員列表時發生錯誤" });
            }
        }

        /// <summary>
        /// 取得管理員詳細資訊
        /// 需要管理者權限管理權限
        /// </summary>
        [HttpGet("managers/{id}")]
        [Authorize(Policy = "AdministratorPrivilegesManagement")]
        public async Task<IActionResult> GetManager(int id)
        {
            try
            {
                var manager = await _adminService.GetManagerDetailAsync(id);
                if (manager == null)
                {
                    return NotFound(new { success = false, message = "管理員不存在" });
                }

                return Ok(new { success = true, data = manager, message = "管理員資訊取得成功" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "取得管理員詳細資訊時發生錯誤: {ManagerId}", id);
                return StatusCode(500, new { success = false, message = "取得管理員資訊時發生錯誤" });
            }
        }

        /// <summary>
        /// 建立管理員
        /// 需要管理者權限管理權限
        /// </summary>
        [HttpPost("managers")]
        [Authorize(Policy = "AdministratorPrivilegesManagement")]
        public async Task<IActionResult> CreateManager([FromBody] CreateManagerDto createDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new { success = false, message = "管理員資料格式錯誤", errors = ModelState });
                }

                var result = await _adminService.CreateManagerAsync(createDto);
                
                if (result.Success)
                {
                    // 記錄操作日誌
                    var operatorId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
                    await _adminService.LogOperationAsync(
                        "CREATE", "Manager", operatorId, "ManagerData", 
                        result.Result?.ManagerId.ToString(), "建立管理員", createDto, "success");

                    return Ok(new { success = true, data = result.Result, message = result.Message });
                }
                else
                {
                    return BadRequest(new { success = false, message = result.Message, errors = result.Errors });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "建立管理員時發生錯誤");
                return StatusCode(500, new { success = false, message = "建立管理員時發生錯誤" });
            }
        }

        #endregion

        #region 角色權限管理

        /// <summary>
        /// 取得角色權限列表
        /// 需要管理者權限管理權限
        /// </summary>
        [HttpGet("roles")]
        [Authorize(Policy = "AdministratorPrivilegesManagement")]
        public async Task<IActionResult> GetRoles()
        {
            try
            {
                var roles = await _adminService.GetRolePermissionsAsync();
                return Ok(new { success = true, data = roles, message = "角色權限列表取得成功" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "取得角色權限列表時發生錯誤");
                return StatusCode(500, new { success = false, message = "取得角色權限列表時發生錯誤" });
            }
        }

        /// <summary>
        /// 建立角色權限
        /// 需要管理者權限管理權限
        /// </summary>
        [HttpPost("roles")]
        [Authorize(Policy = "AdministratorPrivilegesManagement")]
        public async Task<IActionResult> CreateRole([FromBody] CreateManagerRolePermissionDto createDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new { success = false, message = "角色權限資料格式錯誤", errors = ModelState });
                }

                var result = await _adminService.CreateRolePermissionAsync(createDto);
                
                if (result.Success)
                {
                    // 記錄操作日誌
                    var operatorId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
                    await _adminService.LogOperationAsync(
                        "CREATE", "Role", operatorId, "ManagerRolePermission", 
                        result.Result?.ManagerRoleId.ToString(), "建立角色權限", createDto, "success");

                    return Ok(new { success = true, data = result.Result, message = result.Message });
                }
                else
                {
                    return BadRequest(new { success = false, message = result.Message, errors = result.Errors });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "建立角色權限時發生錯誤");
                return StatusCode(500, new { success = false, message = "建立角色權限時發生錯誤" });
            }
        }

        #endregion

        #region 使用者治理

        /// <summary>
        /// 取得使用者管理列表
        /// 需要使用者狀態管理權限
        /// </summary>
        [HttpGet("users")]
        [Authorize(Policy = "UserStatusManagement")]
        public async Task<IActionResult> GetUsers(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 20,
            [FromQuery] string? search = null,
            [FromQuery] int? accountStatus = null)
        {
            try
            {
                var result = await _adminService.GetUsersForManagementAsync(page, pageSize, search, accountStatus);
                return Ok(new { success = true, data = result, message = "使用者列表取得成功" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "取得使用者管理列表時發生錯誤");
                return StatusCode(500, new { success = false, message = "取得使用者列表時發生錯誤" });
            }
        }

        /// <summary>
        /// 取得使用者詳細資訊
        /// 需要使用者狀態管理權限
        /// </summary>
        [HttpGet("users/{id}")]
        [Authorize(Policy = "UserStatusManagement")]
        public async Task<IActionResult> GetUser(int id)
        {
            try
            {
                var user = await _adminService.GetUserDetailForManagementAsync(id);
                if (user == null)
                {
                    return NotFound(new { success = false, message = "使用者不存在" });
                }

                return Ok(new { success = true, data = user, message = "使用者資訊取得成功" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "取得使用者詳細資訊時發生錯誤: {UserId}", id);
                return StatusCode(500, new { success = false, message = "取得使用者資訊時發生錯誤" });
            }
        }

        /// <summary>
        /// 調整使用者權限
        /// 需要使用者狀態管理權限
        /// </summary>
        [HttpPut("users/{id}/rights")]
        [Authorize(Policy = "UserStatusManagement")]
        public async Task<IActionResult> UpdateUserRights(int id, [FromBody] UpdateUserRightsDto updateDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new { success = false, message = "權限調整資料格式錯誤", errors = ModelState });
                }

                var operatorId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
                var result = await _adminService.UpdateUserRightsAsync(id, updateDto, operatorId);
                
                if (result.Success)
                {
                    return Ok(new { success = true, message = result.Message });
                }
                else
                {
                    return BadRequest(new { success = false, message = result.Message, errors = result.Errors });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "調整使用者權限時發生錯誤: {UserId}", id);
                return StatusCode(500, new { success = false, message = "調整使用者權限時發生錯誤" });
            }
        }

        /// <summary>
        /// 調整使用者點數
        /// 需要使用者狀態管理權限
        /// </summary>
        [HttpPost("users/{id}/points/adjust")]
        [Authorize(Policy = "UserStatusManagement")]
        public async Task<IActionResult> AdjustUserPoints(int id, [FromBody] AdjustUserPointsDto adjustDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new { success = false, message = "點數調整資料格式錯誤", errors = ModelState });
                }

                var operatorId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
                var result = await _adminService.AdjustUserPointsAsync(id, adjustDto, operatorId);
                
                if (result.Success)
                {
                    return Ok(new { success = true, message = result.Message });
                }
                else
                {
                    return BadRequest(new { success = false, message = result.Message, errors = result.Errors });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "調整使用者點數時發生錯誤: {UserId}", id);
                return StatusCode(500, new { success = false, message = "調整使用者點數時發生錯誤" });
            }
        }

        #endregion

        #region 儀表板統計

        /// <summary>
        /// 取得管理員儀表板
        /// 顯示系統綜合統計資訊
        /// </summary>
        [HttpGet("dashboard")]
        public async Task<IActionResult> GetDashboard()
        {
            try
            {
                var dashboard = await _adminService.GetAdminDashboardAsync();
                return Ok(new { success = true, data = dashboard, message = "儀表板資料取得成功" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "取得管理員儀表板時發生錯誤");
                return StatusCode(500, new { success = false, message = "取得儀表板資料時發生錯誤" });
            }
        }

        /// <summary>
        /// 取得系統健康狀態
        /// 監控系統運行情況
        /// </summary>
        [HttpGet("system/health")]
        public async Task<IActionResult> GetSystemHealth()
        {
            try
            {
                var health = await _adminService.GetSystemHealthAsync();
                return Ok(new { success = true, data = health, message = "系統健康狀態取得成功" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "取得系統健康狀態時發生錯誤");
                return StatusCode(500, new { success = false, message = "取得系統健康狀態時發生錯誤" });
            }
        }

        #endregion

        #region 操作日誌

        /// <summary>
        /// 查詢操作日誌
        /// 需要管理者權限管理權限
        /// </summary>
        [HttpPost("logs/search")]
        [Authorize(Policy = "AdministratorPrivilegesManagement")]
        public async Task<IActionResult> SearchOperationLogs([FromBody] OperationLogQueryDto queryDto)
        {
            try
            {
                var result = await _adminService.GetOperationLogsAsync(queryDto);
                return Ok(new { success = true, data = result, message = "操作日誌查詢成功" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "查詢操作日誌時發生錯誤");
                return StatusCode(500, new { success = false, message = "查詢操作日誌時發生錯誤" });
            }
        }

        #endregion

        #region 權限檢查輔助方法

        /// <summary>
        /// 檢查當前管理員權限
        /// </summary>
        private async Task<bool> CheckCurrentManagerPermission(string permission)
        {
            var managerIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!int.TryParse(managerIdClaim, out int managerId))
            {
                return false;
            }

            return await _adminService.CheckManagerPermissionAsync(managerId, permission);
        }

        #endregion
    }
}