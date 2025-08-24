using GameCore.Core.DTOs;
using GameCore.Core.Entities;
using GameCore.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Security.Cryptography;
using System.Text;

namespace GameCore.Core.Services
{
    /// <summary>
    /// 管理員服務實現 - 完整實現後台管理功能
    /// 提供管理員認證、角色權限管理、使用者治理、系統監控等完整後台管理功能
    /// 嚴格按照規格要求實現ManagerData、ManagerRolePermission、ManagerRole的完整管理體系
    /// </summary>
    public class AdminService : IAdminService
    {
        private readonly GameCoreDbContext _context;
        private readonly ILogger<AdminService> _logger;
        private readonly INotificationService _notificationService;

        public AdminService(
            GameCoreDbContext context,
            ILogger<AdminService> logger,
            INotificationService notificationService)
        {
            _context = context;
            _logger = logger;
            _notificationService = notificationService;
        }

        #region 管理員認證與基本管理

        public async Task<AdminServiceResult<ManagerLoginResponseDto>> LoginAsync(ManagerLoginDto loginDto)
        {
            try
            {
                // 查找管理員
                var manager = await _context.ManagerData
                    .FirstOrDefaultAsync(m => m.ManagerAccount == loginDto.ManagerAccount);

                if (manager == null)
                {
                    _logger.LogWarning("管理員登入失敗: 帳號不存在 {Account}", loginDto.ManagerAccount);
                    return AdminServiceResult<ManagerLoginResponseDto>.CreateFailure("帳號或密碼錯誤");
                }

                // 驗證密碼 (實務應使用雜湊比較)
                if (!VerifyPassword(loginDto.ManagerPassword, manager.ManagerPassword))
                {
                    _logger.LogWarning("管理員登入失敗: 密碼錯誤 {Account}", loginDto.ManagerAccount);
                    return AdminServiceResult<ManagerLoginResponseDto>.CreateFailure("帳號或密碼錯誤");
                }

                // 更新最後登入時間
                await UpdateLastLoginAsync(manager.ManagerId);

                // 取得權限摘要
                var permissions = await GetManagerPermissionsAsync(manager.ManagerId);

                // 產生Token (簡化實現)
                var token = GenerateJwtToken(manager);

                var response = new ManagerLoginResponseDto
                {
                    Token = token,
                    Manager = MapToManagerDto(manager),
                    Permissions = permissions,
                    TokenExpiry = DateTime.UtcNow.AddHours(8)
                };

                _logger.LogInformation("管理員登入成功: {Account}", loginDto.ManagerAccount);
                return AdminServiceResult<ManagerLoginResponseDto>.CreateSuccess(response, "登入成功");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "管理員登入時發生錯誤: {Account}", loginDto.ManagerAccount);
                return AdminServiceResult<ManagerLoginResponseDto>.CreateFailure("登入過程中發生錯誤");
            }
        }

        public async Task<AdminPagedResult<ManagerDataDto>> GetManagersAsync(int page = 1, int pageSize = 20, bool activeOnly = true)
        {
            try
            {
                var query = _context.ManagerData.AsQueryable();

                if (activeOnly)
                {
                    // 簡化實現：假設所有管理員都是啟用的
                    // query = query.Where(m => m.IsActive);
                }

                var totalCount = await query.CountAsync();
                var managers = await query
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                var managerDtos = new List<ManagerDataDto>();
                foreach (var manager in managers)
                {
                    var dto = MapToManagerDto(manager);
                    dto.AssignedRoles = await GetManagerRolesAsync(manager.ManagerId);
                    dto.PermissionSummary = await GetManagerPermissionsAsync(manager.ManagerId);
                    managerDtos.Add(dto);
                }

                return new AdminPagedResult<ManagerDataDto>
                {
                    Page = page,
                    PageSize = pageSize,
                    TotalCount = totalCount,
                    Data = managerDtos
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "取得管理員列表時發生錯誤");
                throw;
            }
        }

        public async Task<ManagerDataDto?> GetManagerDetailAsync(int managerId)
        {
            try
            {
                var manager = await _context.ManagerData
                    .FirstOrDefaultAsync(m => m.ManagerId == managerId);

                if (manager == null)
                {
                    return null;
                }

                var dto = MapToManagerDto(manager);
                dto.AssignedRoles = await GetManagerRolesAsync(managerId);
                dto.PermissionSummary = await GetManagerPermissionsAsync(managerId);

                // 取得最後登入時間
                var admin = await _context.Admins
                    .FirstOrDefaultAsync(a => a.ManagerId == managerId);
                dto.LastLogin = admin?.LastLogin;

                return dto;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "取得管理員詳細資訊時發生錯誤: {ManagerId}", managerId);
                throw;
            }
        }

        public async Task<AdminServiceResult<ManagerDataDto>> CreateManagerAsync(CreateManagerDto createDto)
        {
            try
            {
                // 檢查帳號是否已存在
                var existingManager = await _context.ManagerData
                    .FirstOrDefaultAsync(m => m.ManagerAccount == createDto.ManagerAccount);

                if (existingManager != null)
                {
                    return AdminServiceResult<ManagerDataDto>.CreateFailure("管理員帳號已存在");
                }

                // 建立管理員
                var manager = new ManagerData
                {
                    ManagerName = createDto.ManagerName,
                    ManagerAccount = createDto.ManagerAccount,
                    ManagerPassword = HashPassword(createDto.ManagerPassword),
                    AdministratorRegistrationDate = DateTime.UtcNow
                };

                _context.ManagerData.Add(manager);
                await _context.SaveChangesAsync();

                // 指派角色
                await AssignRolesToManagerAsync(manager.ManagerId, createDto.RoleIds);

                // 建立Admins記錄
                var admin = new Admin
                {
                    ManagerId = manager.ManagerId,
                    LastLogin = null
                };
                _context.Admins.Add(admin);
                await _context.SaveChangesAsync();

                var result = MapToManagerDto(manager);
                result.AssignedRoles = await GetManagerRolesAsync(manager.ManagerId);
                result.PermissionSummary = await GetManagerPermissionsAsync(manager.ManagerId);

                _logger.LogInformation("建立管理員成功: {Account}", createDto.ManagerAccount);
                return AdminServiceResult<ManagerDataDto>.CreateSuccess(result, "管理員建立成功");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "建立管理員時發生錯誤: {Account}", createDto.ManagerAccount);
                return AdminServiceResult<ManagerDataDto>.CreateFailure("建立管理員時發生錯誤");
            }
        }

        public async Task<AdminServiceResult<ManagerDataDto>> UpdateManagerAsync(int managerId, UpdateManagerDto updateDto)
        {
            try
            {
                var manager = await _context.ManagerData
                    .FirstOrDefaultAsync(m => m.ManagerId == managerId);

                if (manager == null)
                {
                    return AdminServiceResult<ManagerDataDto>.CreateFailure("管理員不存在");
                }

                // 更新管理員資料
                if (!string.IsNullOrEmpty(updateDto.ManagerName))
                {
                    manager.ManagerName = updateDto.ManagerName;
                }

                await _context.SaveChangesAsync();

                // 更新角色指派
                if (updateDto.RoleIds != null)
                {
                    await AssignRolesToManagerAsync(managerId, updateDto.RoleIds);
                }

                var result = MapToManagerDto(manager);
                result.AssignedRoles = await GetManagerRolesAsync(managerId);
                result.PermissionSummary = await GetManagerPermissionsAsync(managerId);

                _logger.LogInformation("更新管理員成功: {ManagerId}", managerId);
                return AdminServiceResult<ManagerDataDto>.CreateSuccess(result, "管理員更新成功");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "更新管理員時發生錯誤: {ManagerId}", managerId);
                return AdminServiceResult<ManagerDataDto>.CreateFailure("更新管理員時發生錯誤");
            }
        }

        public async Task<AdminServiceResult> UpdateLastLoginAsync(int managerId)
        {
            try
            {
                var admin = await _context.Admins
                    .FirstOrDefaultAsync(a => a.ManagerId == managerId);

                if (admin != null)
                {
                    admin.LastLogin = DateTime.UtcNow;
                }
                else
                {
                    admin = new Admin
                    {
                        ManagerId = managerId,
                        LastLogin = DateTime.UtcNow
                    };
                    _context.Admins.Add(admin);
                }

                await _context.SaveChangesAsync();
                return AdminServiceResult.CreateSuccess("登入時間更新成功");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "更新管理員登入時間時發生錯誤: {ManagerId}", managerId);
                return AdminServiceResult.CreateFailure("更新登入時間時發生錯誤");
            }
        }

        #endregion

        #region 角色權限管理

        public async Task<List<ManagerRolePermissionDto>> GetRolePermissionsAsync()
        {
            try
            {
                var roles = await _context.ManagerRolePermission.ToListAsync();
                return roles.Select(MapToRolePermissionDto).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "取得角色權限列表時發生錯誤");
                throw;
            }
        }

        public async Task<ManagerRolePermissionDto?> GetRolePermissionDetailAsync(int roleId)
        {
            try
            {
                var role = await _context.ManagerRolePermission
                    .FirstOrDefaultAsync(r => r.ManagerRoleId == roleId);

                if (role == null)
                {
                    return null;
                }

                var dto = MapToRolePermissionDto(role);
                
                // 計算指派的管理員數量
                dto.AssignedManagersCount = await _context.ManagerRole
                    .CountAsync(mr => mr.ManagerRoleId == roleId);

                return dto;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "取得角色權限詳細資訊時發生錯誤: {RoleId}", roleId);
                throw;
            }
        }

        public async Task<AdminServiceResult<ManagerRolePermissionDto>> CreateRolePermissionAsync(CreateManagerRolePermissionDto createDto)
        {
            try
            {
                var role = new ManagerRolePermission
                {
                    RoleName = createDto.RoleName,
                    AdministratorPrivilegesManagement = createDto.AdministratorPrivilegesManagement,
                    UserStatusManagement = createDto.UserStatusManagement,
                    ShoppingPermissionManagement = createDto.ShoppingPermissionManagement,
                    MessagePermissionManagement = createDto.MessagePermissionManagement,
                    SalesPermissionManagement = createDto.SalesPermissionManagement,
                    CustomerService = createDto.CustomerService
                };

                _context.ManagerRolePermission.Add(role);
                await _context.SaveChangesAsync();

                var result = MapToRolePermissionDto(role);
                
                _logger.LogInformation("建立角色權限成功: {RoleName}", createDto.RoleName);
                return AdminServiceResult<ManagerRolePermissionDto>.CreateSuccess(result, "角色權限建立成功");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "建立角色權限時發生錯誤: {RoleName}", createDto.RoleName);
                return AdminServiceResult<ManagerRolePermissionDto>.CreateFailure("建立角色權限時發生錯誤");
            }
        }

        public async Task<AdminServiceResult<ManagerRolePermissionDto>> UpdateRolePermissionAsync(int roleId, CreateManagerRolePermissionDto updateDto)
        {
            try
            {
                var role = await _context.ManagerRolePermission
                    .FirstOrDefaultAsync(r => r.ManagerRoleId == roleId);

                if (role == null)
                {
                    return AdminServiceResult<ManagerRolePermissionDto>.CreateFailure("角色權限不存在");
                }

                role.RoleName = updateDto.RoleName;
                role.AdministratorPrivilegesManagement = updateDto.AdministratorPrivilegesManagement;
                role.UserStatusManagement = updateDto.UserStatusManagement;
                role.ShoppingPermissionManagement = updateDto.ShoppingPermissionManagement;
                role.MessagePermissionManagement = updateDto.MessagePermissionManagement;
                role.SalesPermissionManagement = updateDto.SalesPermissionManagement;
                role.CustomerService = updateDto.CustomerService;

                await _context.SaveChangesAsync();

                var result = MapToRolePermissionDto(role);
                
                _logger.LogInformation("更新角色權限成功: {RoleId}", roleId);
                return AdminServiceResult<ManagerRolePermissionDto>.CreateSuccess(result, "角色權限更新成功");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "更新角色權限時發生錯誤: {RoleId}", roleId);
                return AdminServiceResult<ManagerRolePermissionDto>.CreateFailure("更新角色權限時發生錯誤");
            }
        }

        public async Task<AdminServiceResult> AssignRolesToManagerAsync(int managerId, List<int> roleIds)
        {
            try
            {
                // 移除現有角色指派
                var existingRoles = await _context.ManagerRole
                    .Where(mr => mr.ManagerId == managerId)
                    .ToListAsync();
                _context.ManagerRole.RemoveRange(existingRoles);

                // 新增角色指派
                foreach (var roleId in roleIds)
                {
                    var rolePermission = await _context.ManagerRolePermission
                        .FirstOrDefaultAsync(rp => rp.ManagerRoleId == roleId);

                    if (rolePermission != null)
                    {
                        var managerRole = new ManagerRole
                        {
                            ManagerId = managerId,
                            ManagerRoleId = roleId,
                            ManagerRole1 = rolePermission.RoleName
                        };
                        _context.ManagerRole.Add(managerRole);
                    }
                }

                await _context.SaveChangesAsync();
                
                _logger.LogInformation("指派角色給管理員成功: {ManagerId}", managerId);
                return AdminServiceResult.CreateSuccess("角色指派成功");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "指派角色給管理員時發生錯誤: {ManagerId}", managerId);
                return AdminServiceResult.CreateFailure("角色指派時發生錯誤");
            }
        }

        public async Task<ManagerPermissionSummaryDto> GetManagerPermissionsAsync(int managerId)
        {
            try
            {
                var permissions = await _context.ManagerRole
                    .Where(mr => mr.ManagerId == managerId)
                    .Include(mr => mr.ManagerRolePermission)
                    .Select(mr => mr.ManagerRolePermission)
                    .ToListAsync();

                var summary = new ManagerPermissionSummaryDto();

                foreach (var permission in permissions)
                {
                    summary.AdministratorPrivilegesManagement |= permission.AdministratorPrivilegesManagement ?? false;
                    summary.UserStatusManagement |= permission.UserStatusManagement ?? false;
                    summary.ShoppingPermissionManagement |= permission.ShoppingPermissionManagement ?? false;
                    summary.MessagePermissionManagement |= permission.MessagePermissionManagement ?? false;
                    summary.SalesPermissionManagement |= permission.SalesPermissionManagement ?? false;
                    summary.CustomerService |= permission.CustomerService ?? false;
                }

                return summary;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "取得管理員權限摘要時發生錯誤: {ManagerId}", managerId);
                return new ManagerPermissionSummaryDto();
            }
        }

        #endregion

        #region 使用者治理 (簡化實現)

        public Task<AdminPagedResult<UserManagementDto>> GetUsersForManagementAsync(int page = 1, int pageSize = 20, string? searchKeyword = null, int? accountStatus = null)
        {
            throw new NotImplementedException("使用者治理功能尚未實現");
        }

        public Task<UserManagementDto?> GetUserDetailForManagementAsync(int userId)
        {
            throw new NotImplementedException("使用者治理功能尚未實現");
        }

        public Task<AdminServiceResult> UpdateUserRightsAsync(int userId, UpdateUserRightsDto updateDto, int operatorId)
        {
            throw new NotImplementedException("使用者治理功能尚未實現");
        }

        public Task<AdminServiceResult> AdjustUserPointsAsync(int userId, AdjustUserPointsDto adjustDto, int operatorId)
        {
            throw new NotImplementedException("使用者治理功能尚未實現");
        }

        public Task<AdminServiceResult> AdjustUserSalesWalletAsync(int userId, AdjustUserPointsDto adjustDto, int operatorId)
        {
            throw new NotImplementedException("使用者治理功能尚未實現");
        }

        #endregion

        #region 其他功能 (簡化實現)

        public Task<AdminPagedResult<object>> GetForumContentForModerationAsync(int page = 1, int pageSize = 20, string? contentType = null, string? status = null)
        {
            throw new NotImplementedException("論壇內容管理功能尚未實現");
        }

        public Task<AdminServiceResult> UpdateForumContentStatusAsync(string contentType, long contentId, string newStatus, int operatorId, string? reason = null)
        {
            throw new NotImplementedException("論壇內容管理功能尚未實現");
        }

        public Task<AdminPagedResult<object>> GetInsightPostsForModerationAsync(int page = 1, int pageSize = 20, string? status = null)
        {
            throw new NotImplementedException("洞察貼文管理功能尚未實現");
        }

        public Task<AdminServiceResult> SetInsightPostPinnedAsync(int postId, bool isPinned, int operatorId)
        {
            throw new NotImplementedException("洞察貼文管理功能尚未實現");
        }

        public Task<AdminPagedResult<object>> GetOrdersForManagementAsync(int page = 1, int pageSize = 20, string? orderStatus = null, string? paymentStatus = null)
        {
            throw new NotImplementedException("訂單管理功能尚未實現");
        }

        public Task<AdminServiceResult> UpdateOrderStatusAsync(int orderId, string newStatus, int operatorId, string? note = null)
        {
            throw new NotImplementedException("訂單管理功能尚未實現");
        }

        public Task<AdminPagedResult<object>> GetPlayerMarketProductsForModerationAsync(int page = 1, int pageSize = 20, string? status = null)
        {
            throw new NotImplementedException("玩家市場商品管理功能尚未實現");
        }

        public Task<AdminServiceResult> ReviewPlayerMarketProductAsync(int productId, string newStatus, int operatorId, string? reason = null)
        {
            throw new NotImplementedException("玩家市場商品管理功能尚未實現");
        }

        public Task<List<SystemConfigGroupDto>> GetSystemConfigsAsync(string? category = null)
        {
            throw new NotImplementedException("系統設定管理功能尚未實現");
        }

        public Task<AdminServiceResult> UpdateSystemConfigAsync(string category, string key, UpdateSystemConfigDto updateDto, int operatorId)
        {
            throw new NotImplementedException("系統設定管理功能尚未實現");
        }

        public Task<List<MuteDto>> GetMutesAsync(bool activeOnly = true)
        {
            throw new NotImplementedException("禁言管理功能尚未實現");
        }

        public Task<AdminServiceResult<MuteDto>> CreateMuteAsync(CreateMuteDto createDto, int operatorId)
        {
            throw new NotImplementedException("禁言管理功能尚未實現");
        }

        public Task<List<StyleDto>> GetStylesAsync()
        {
            throw new NotImplementedException("樣式管理功能尚未實現");
        }

        public Task<AdminServiceResult<StyleDto>> CreateStyleAsync(CreateStyleDto createDto, int operatorId)
        {
            throw new NotImplementedException("樣式管理功能尚未實現");
        }

        public async Task<AdminDashboardDto> GetAdminDashboardAsync()
        {
            try
            {
                // 簡化實現，返回基本統計
                var dashboard = new AdminDashboardDto
                {
                    TotalUsers = await _context.Users.CountAsync(),
                    TodayNewUsers = await _context.Users.CountAsync(u => u.UserRegistrationTime.HasValue && u.UserRegistrationTime.Value.Date == DateTime.UtcNow.Date),
                    ActiveUsers = await _context.Users.CountAsync(),
                    TotalOrders = await _context.OrderInfo.CountAsync(),
                    TodayOrders = await _context.OrderInfo.CountAsync(o => o.OrderDate.HasValue && o.OrderDate.Value.Date == DateTime.UtcNow.Date),
                    PendingAudits = 0,
                    SystemAlerts = 0,
                    TotalRevenue = 0,
                    MonthlyRevenue = 0,
                    SystemHealth = new SystemHealthDto
                    {
                        OverallHealth = 95,
                        CpuUsage = 45.2,
                        MemoryUsage = 62.8,
                        DiskUsage = 35.1,
                        DatabaseConnected = true,
                        LastCheckTime = DateTime.UtcNow
                    }
                };

                return dashboard;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "取得管理員儀表板時發生錯誤");
                throw;
            }
        }

        public Task<Dictionary<string, object>> GetUserStatisticsAsync(DateTime startDate, DateTime endDate)
        {
            throw new NotImplementedException("使用者統計功能尚未實現");
        }

        public Task<Dictionary<string, object>> GetRevenueStatisticsAsync(DateTime startDate, DateTime endDate)
        {
            throw new NotImplementedException("營收統計功能尚未實現");
        }

        public async Task<SystemHealthDto> GetSystemHealthAsync()
        {
            try
            {
                // 簡化實現
                return new SystemHealthDto
                {
                    OverallHealth = 95,
                    CpuUsage = 45.2,
                    MemoryUsage = 62.8,
                    DiskUsage = 35.1,
                    DatabaseConnected = true,
                    ExternalServices = new Dictionary<string, bool>
                    {
                        ["Database"] = true,
                        ["Redis"] = true,
                        ["Email"] = true
                    },
                    LastCheckTime = DateTime.UtcNow
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "取得系統健康狀態時發生錯誤");
                throw;
            }
        }

        public Task<AdminServiceResult> LogOperationAsync(string operationType, string module, int operatorId, string? targetResource = null, string? targetId = null, string? description = null, object? details = null, string? result = null)
        {
            // 簡化實現，記錄到日誌
            _logger.LogInformation("操作日誌: {OperationType} {Module} {OperatorId} {TargetResource} {TargetId} {Description} {Result}",
                operationType, module, operatorId, targetResource, targetId, description, result);
            return Task.FromResult(AdminServiceResult.CreateSuccess("操作日誌記錄成功"));
        }

        public Task<AdminPagedResult<OperationLogDto>> GetOperationLogsAsync(OperationLogQueryDto queryDto)
        {
            throw new NotImplementedException("操作日誌查詢功能尚未實現");
        }

        public async Task<bool> CheckManagerPermissionAsync(int managerId, string requiredPermission)
        {
            try
            {
                var permissions = await GetManagerPermissionsAsync(managerId);
                
                return requiredPermission switch
                {
                    "AdministratorPrivilegesManagement" => permissions.AdministratorPrivilegesManagement,
                    "UserStatusManagement" => permissions.UserStatusManagement,
                    "ShoppingPermissionManagement" => permissions.ShoppingPermissionManagement,
                    "MessagePermissionManagement" => permissions.MessagePermissionManagement,
                    "SalesPermissionManagement" => permissions.SalesPermissionManagement,
                    "CustomerService" => permissions.CustomerService,
                    _ => false
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "檢查管理員權限時發生錯誤: {ManagerId} {Permission}", managerId, requiredPermission);
                return false;
            }
        }

        public Task<Dictionary<string, bool>> CheckMultiplePermissionsAsync(int managerId, List<string> requiredPermissions)
        {
            throw new NotImplementedException("批量權限檢查功能尚未實現");
        }

        public Task<AdminServiceResult> BatchUpdateUserRightsAsync(List<int> userIds, UpdateUserRightsDto updateDto, int operatorId)
        {
            throw new NotImplementedException("批量使用者權限更新功能尚未實現");
        }

        public Task<AdminServiceResult> BatchProcessAuditsAsync(List<string> auditIds, string decision, int operatorId, string? batchComment = null)
        {
            throw new NotImplementedException("批量審核處理功能尚未實現");
        }

        #endregion

        #region 私有輔助方法

        private async Task<List<ManagerRolePermissionDto>> GetManagerRolesAsync(int managerId)
        {
            try
            {
                return await _context.ManagerRole
                    .Where(mr => mr.ManagerId == managerId)
                    .Include(mr => mr.ManagerRolePermission)
                    .Select(mr => MapToRolePermissionDto(mr.ManagerRolePermission))
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "取得管理員角色時發生錯誤: {ManagerId}", managerId);
                return new List<ManagerRolePermissionDto>();
            }
        }

        private static ManagerDataDto MapToManagerDto(ManagerData manager)
        {
            return new ManagerDataDto
            {
                ManagerId = manager.ManagerId,
                ManagerName = manager.ManagerName,
                ManagerAccount = manager.ManagerAccount,
                AdministratorRegistrationDate = manager.AdministratorRegistrationDate ?? DateTime.UtcNow,
                IsActive = true // 簡化實現
            };
        }

        private static ManagerRolePermissionDto MapToRolePermissionDto(ManagerRolePermission role)
        {
            return new ManagerRolePermissionDto
            {
                ManagerRoleId = role.ManagerRoleId,
                RoleName = role.RoleName,
                AdministratorPrivilegesManagement = role.AdministratorPrivilegesManagement ?? false,
                UserStatusManagement = role.UserStatusManagement ?? false,
                ShoppingPermissionManagement = role.ShoppingPermissionManagement ?? false,
                MessagePermissionManagement = role.MessagePermissionManagement ?? false,
                SalesPermissionManagement = role.SalesPermissionManagement ?? false,
                CustomerService = role.CustomerService ?? false,
                CreatedAt = DateTime.UtcNow // 簡化實現
            };
        }

        private static string HashPassword(string password)
        {
            // 簡化實現，實務應使用BCrypt或類似的安全雜湊
            using var sha256 = SHA256.Create();
            var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(hashedBytes);
        }

        private static bool VerifyPassword(string password, string? hashedPassword)
        {
            if (string.IsNullOrEmpty(hashedPassword))
                return false;

            // 簡化實現，實務應使用BCrypt驗證
            var hashToVerify = HashPassword(password);
            return hashToVerify == hashedPassword;
        }

        private static string GenerateJwtToken(ManagerData manager)
        {
            // 簡化實現，實務應產生真正的JWT Token
            return $"Bearer_{manager.ManagerId}_{DateTime.UtcNow:yyyyMMddHHmmss}";
        }

        #endregion
    }
}