using Microsoft.AspNetCore.Mvc;
using GameCore.Core.Services;
using GameCore.Core.Entities;
using Microsoft.AspNetCore.Authorization;

namespace GameCore.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin,Moderator")]
    public class ManagerController : ControllerBase
    {
        private readonly IManagerService _managerService;
        private readonly ILogger<ManagerController> _logger;

        public ManagerController(IManagerService managerService, ILogger<ManagerController> logger)
        {
            _managerService = managerService;
            _logger = logger;
        }

        [HttpGet("dashboard")]
        public async Task<IActionResult> GetDashboard()
        {
            try
            {
                var dashboard = await _managerService.GetDashboardAsync();
                return Ok(new { Success = true, Data = dashboard });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting dashboard");
                return StatusCode(500, new { Success = false, Message = "取得儀表板失敗" });
            }
        }

        [HttpGet("managers")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetManagers([FromQuery] int page = 1, [FromQuery] int pageSize = 20)
        {
            try
            {
                var managers = await _managerService.GetManagersAsync(page, pageSize);
                return Ok(new { Success = true, Data = managers });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting managers");
                return StatusCode(500, new { Success = false, Message = "取得管理員清單失敗" });
            }
        }

        [HttpGet("managers/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetManager(int id)
        {
            try
            {
                var manager = await _managerService.GetManagerByIdAsync(id);
                if (manager == null)
                {
                    return NotFound(new { Success = false, Message = "管理員不存在" });
                }
                return Ok(new { Success = true, Data = manager });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting manager {ManagerId}", id);
                return StatusCode(500, new { Success = false, Message = "取得管理員資訊失敗" });
            }
        }

        [HttpPost("managers")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateManager([FromBody] CreateManagerRequest request)
        {
            try
            {
                var manager = await _managerService.CreateManagerAsync(
                    request.Account, 
                    request.Password, 
                    request.Name, 
                    request.Email, 
                    request.RoleId);
                return Ok(new { Success = true, Data = manager, Message = "管理員創建成功" });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { Success = false, Message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating manager");
                return StatusCode(500, new { Success = false, Message = "創建管理員失敗" });
            }
        }

        [HttpPut("managers/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateManager(int id, [FromBody] UpdateManagerRequest request)
        {
            try
            {
                await _managerService.UpdateManagerAsync(
                    id, 
                    request.Name, 
                    request.Email, 
                    request.RoleId, 
                    request.IsActive);
                return Ok(new { Success = true, Message = "管理員更新成功" });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { Success = false, Message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating manager {ManagerId}", id);
                return StatusCode(500, new { Success = false, Message = "更新管理員失敗" });
            }
        }

        [HttpDelete("managers/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteManager(int id)
        {
            try
            {
                await _managerService.DeleteManagerAsync(id);
                return Ok(new { Success = true, Message = "管理員刪除成功" });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { Success = false, Message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting manager {ManagerId}", id);
                return StatusCode(500, new { Success = false, Message = "刪除管理員失敗" });
            }
        }

        [HttpGet("roles")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetRoles()
        {
            try
            {
                var roles = await _managerService.GetRolesAsync();
                return Ok(new { Success = true, Data = roles });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting roles");
                return StatusCode(500, new { Success = false, Message = "取得角色清單失敗" });
            }
        }

        [HttpGet("roles/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetRole(int id)
        {
            try
            {
                var role = await _managerService.GetRoleByIdAsync(id);
                if (role == null)
                {
                    return NotFound(new { Success = false, Message = "角色不存在" });
                }
                return Ok(new { Success = true, Data = role });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting role {RoleId}", id);
                return StatusCode(500, new { Success = false, Message = "取得角色資訊失敗" });
            }
        }

        [HttpPost("roles")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateRole([FromBody] CreateRoleRequest request)
        {
            try
            {
                var role = await _managerService.CreateRoleAsync(
                    request.Name, 
                    request.Description, 
                    request.PermissionIds);
                return Ok(new { Success = true, Data = role, Message = "角色創建成功" });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { Success = false, Message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating role");
                return StatusCode(500, new { Success = false, Message = "創建角色失敗" });
            }
        }

        [HttpPut("roles/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateRole(int id, [FromBody] UpdateRoleRequest request)
        {
            try
            {
                await _managerService.UpdateRoleAsync(
                    id, 
                    request.Name, 
                    request.Description, 
                    request.PermissionIds);
                return Ok(new { Success = true, Message = "角色更新成功" });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { Success = false, Message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating role {RoleId}", id);
                return StatusCode(500, new { Success = false, Message = "更新角色失敗" });
            }
        }

        [HttpDelete("roles/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteRole(int id)
        {
            try
            {
                await _managerService.DeleteRoleAsync(id);
                return Ok(new { Success = true, Message = "角色刪除成功" });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { Success = false, Message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting role {RoleId}", id);
                return StatusCode(500, new { Success = false, Message = "刪除角色失敗" });
            }
        }

        [HttpGet("permissions")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetPermissions()
        {
            try
            {
                var permissions = await _managerService.GetPermissionsAsync();
                return Ok(new { Success = true, Data = permissions });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting permissions");
                return StatusCode(500, new { Success = false, Message = "取得權限清單失敗" });
            }
        }

        [HttpGet("permissions/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetPermission(int id)
        {
            try
            {
                var permission = await _managerService.GetPermissionByIdAsync(id);
                if (permission == null)
                {
                    return NotFound(new { Success = false, Message = "權限不存在" });
                }
                return Ok(new { Success = true, Data = permission });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting permission {PermissionId}", id);
                return StatusCode(500, new { Success = false, Message = "取得權限資訊失敗" });
            }
        }

        [HttpPost("permissions")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreatePermission([FromBody] CreatePermissionRequest request)
        {
            try
            {
                var permission = await _managerService.CreatePermissionAsync(
                    request.Name, 
                    request.Code, 
                    request.Description, 
                    request.Category);
                return Ok(new { Success = true, Data = permission, Message = "權限創建成功" });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { Success = false, Message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating permission");
                return StatusCode(500, new { Success = false, Message = "創建權限失敗" });
            }
        }

        [HttpPut("permissions/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdatePermission(int id, [FromBody] UpdatePermissionRequest request)
        {
            try
            {
                await _managerService.UpdatePermissionAsync(
                    id, 
                    request.Name, 
                    request.Description, 
                    request.Category);
                return Ok(new { Success = true, Message = "權限更新成功" });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { Success = false, Message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating permission {PermissionId}", id);
                return StatusCode(500, new { Success = false, Message = "更新權限失敗" });
            }
        }

        [HttpDelete("permissions/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeletePermission(int id)
        {
            try
            {
                await _managerService.DeletePermissionAsync(id);
                return Ok(new { Success = true, Message = "權限刪除成功" });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { Success = false, Message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting permission {PermissionId}", id);
                return StatusCode(500, new { Success = false, Message = "刪除權限失敗" });
            }
        }

        [HttpGet("my-permissions")]
        public async Task<IActionResult> GetMyPermissions()
        {
            try
            {
                var managerId = GetCurrentManagerId();
                var permissions = await _managerService.GetManagerPermissionsAsync(managerId);
                return Ok(new { Success = true, Data = permissions });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting manager permissions");
                return StatusCode(500, new { Success = false, Message = "取得我的權限失敗" });
            }
        }

        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest request)
        {
            try
            {
                var managerId = GetCurrentManagerId();
                await _managerService.ChangePasswordAsync(managerId, request.OldPassword, request.NewPassword);
                return Ok(new { Success = true, Message = "密碼修改成功" });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { Success = false, Message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error changing password");
                return StatusCode(500, new { Success = false, Message = "修改密碼失敗" });
            }
        }

        [HttpGet("activity-logs")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetActivityLogs([FromQuery] int page = 1, [FromQuery] int pageSize = 20)
        {
            try
            {
                var logs = await _managerService.GetActivityLogsAsync(page, pageSize);
                return Ok(new { Success = true, Data = logs });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting activity logs");
                return StatusCode(500, new { Success = false, Message = "取得活動日誌失敗" });
            }
        }

        private int GetCurrentManagerId()
        {
            var managerIdClaim = User.FindFirst("ManagerId")?.Value;
            return int.TryParse(managerIdClaim, out var managerId) ? managerId : 0;
        }
    }

    public class CreateManagerRequest
    {
        public string Account { get; set; } = "";
        public string Password { get; set; } = "";
        public string Name { get; set; } = "";
        public string Email { get; set; } = "";
        public int RoleId { get; set; }
    }

    public class UpdateManagerRequest
    {
        public string Name { get; set; } = "";
        public string Email { get; set; } = "";
        public int RoleId { get; set; }
        public bool IsActive { get; set; } = true;
    }

    public class CreateRoleRequest
    {
        public string Name { get; set; } = "";
        public string Description { get; set; } = "";
        public int[] PermissionIds { get; set; } = Array.Empty<int>();
    }

    public class UpdateRoleRequest
    {
        public string Name { get; set; } = "";
        public string Description { get; set; } = "";
        public int[] PermissionIds { get; set; } = Array.Empty<int>();
    }

    public class CreatePermissionRequest
    {
        public string Name { get; set; } = "";
        public string Code { get; set; } = "";
        public string Description { get; set; } = "";
        public string Category { get; set; } = "";
    }

    public class UpdatePermissionRequest
    {
        public string Name { get; set; } = "";
        public string Description { get; set; } = "";
        public string Category { get; set; } = "";
    }

    public class ChangePasswordRequest
    {
        public string OldPassword { get; set; } = "";
        public string NewPassword { get; set; } = "";
    }
}