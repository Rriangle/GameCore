using GameCore.Core.Entities;
using GameCore.Core.Interfaces;
using GameCore.Core.DTOs;
using GameCore.Core.Services;
using Microsoft.Extensions.Logging;
using System.Security.Cryptography;
using System.Text;

namespace GameCore.Core.Services
{
    public class ManagerService : IManagerService
    {
        private readonly IManagerRepository _managerRepository;
        private readonly IManagerRolePermissionRepository _managerRolePermissionRepository;
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<ManagerService> _logger;

        public ManagerService(
            IManagerRepository managerRepository,
            IManagerRolePermissionRepository managerRolePermissionRepository,
            IUserRepository userRepository,
            IUnitOfWork unitOfWork,
            ILogger<ManagerService> logger)
        {
            _managerRepository = managerRepository;
            _managerRolePermissionRepository = managerRolePermissionRepository;
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<ManagerLoginResult> LoginAsync(ManagerLoginDto loginDto)
        {
            try
            {
                var manager = await _managerRepository.GetByUsernameAsync(loginDto.Username);
                if (manager == null)
                {
                    return new ManagerLoginResult
                    {
                        Success = false,
                        Message = "用戶名或密碼錯誤"
                    };
                }

                if (!VerifyPassword(loginDto.Password, manager.PasswordHash))
                {
                    return new ManagerLoginResult
                    {
                        Success = false,
                        Message = "用戶名或密碼錯誤"
                    };
                }

                if (!manager.IsActive)
                {
                    return new ManagerLoginResult
                    {
                        Success = false,
                        Message = "帳戶已被停用"
                    };
                }

                // 更新最後登入時間
                manager.LastLoginAt = DateTime.UtcNow;
                _managerRepository.Update(manager);
                await _unitOfWork.SaveChangesAsync();

                // 獲取權限
                var permissions = await GetManagerPermissionsAsync(manager.Id);

                _logger.LogInformation("管理員登入成功: {Username}", manager.Username);

                return new ManagerLoginResult
                {
                    Success = true,
                    Message = "登入成功",
                    Manager = new ManagerDto
                    {
                        Id = manager.Id,
                        Username = manager.Username,
                        Email = manager.Email,
                        Role = manager.Role,
                        IsActive = manager.IsActive,
                        CreatedAt = manager.CreatedAt,
                        LastLoginAt = manager.LastLoginAt,
                        Permissions = permissions
                    }
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "管理員登入失敗: {Username}", loginDto.Username);
                return new ManagerLoginResult
                {
                    Success = false,
                    Message = "登入失敗，請稍後重試"
                };
            }
        }

        public async Task<bool> LogoutAsync(int managerId)
        {
            try
            {
                var manager = await _managerRepository.GetByIdAsync(managerId);
                if (manager != null)
                {
                    manager.LastLoginAt = DateTime.UtcNow;
                    _managerRepository.Update(manager);
                    await _unitOfWork.SaveChangesAsync();
                }

                _logger.LogInformation("管理員登出: {ManagerId}", managerId);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "管理員登出失敗: {ManagerId}", managerId);
                return false;
            }
        }

        public async Task<ManagerProfileResult> GetProfileAsync(int managerId)
        {
            try
            {
                var manager = await _managerRepository.GetByIdAsync(managerId);
                if (manager == null)
                {
                    return new ManagerProfileResult
                    {
                        Success = false,
                        Message = "管理員不存在"
                    };
                }

                var permissions = await GetManagerPermissionsAsync(manager.Id);

                return new ManagerProfileResult
                {
                    Success = true,
                    Manager = new ManagerDto
                    {
                        Id = manager.Id,
                        Username = manager.Username,
                        Email = manager.Email,
                        Role = manager.Role,
                        IsActive = manager.IsActive,
                        CreatedAt = manager.CreatedAt,
                        LastLoginAt = manager.LastLoginAt,
                        Permissions = permissions
                    }
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "獲取管理員資料失敗: {ManagerId}", managerId);
                return new ManagerProfileResult
                {
                    Success = false,
                    Message = "獲取資料失敗"
                };
            }
        }

        public async Task<ManagerUpdateResult> UpdateProfileAsync(int managerId, ManagerUpdateDto updateDto)
        {
            try
            {
                var manager = await _managerRepository.GetByIdAsync(managerId);
                if (manager == null)
                {
                    return new ManagerUpdateResult
                    {
                        Success = false,
                        Message = "管理員不存在"
                    };
                }

                // 檢查郵箱是否被其他管理員使用
                if (!string.IsNullOrEmpty(updateDto.Email) && updateDto.Email != manager.Email)
                {
                    if (await _managerRepository.ExistsByEmailAsync(updateDto.Email))
                    {
                        return new ManagerUpdateResult
                        {
                            Success = false,
                            Message = "郵箱已被其他管理員使用"
                        };
                    }
                    manager.Email = updateDto.Email;
                }

                if (!string.IsNullOrEmpty(updateDto.Username) && updateDto.Username != manager.Username)
                {
                    if (await _managerRepository.ExistsByUsernameAsync(updateDto.Username))
                    {
                        return new ManagerUpdateResult
                        {
                            Success = false,
                            Message = "用戶名已被使用"
                        };
                    }
                    manager.Username = updateDto.Username;
                }

                _managerRepository.Update(manager);
                await _unitOfWork.SaveChangesAsync();

                _logger.LogInformation("管理員資料更新成功: {ManagerId}", managerId);

                return new ManagerUpdateResult
                {
                    Success = true,
                    Message = "更新成功"
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "管理員資料更新失敗: {ManagerId}", managerId);
                return new ManagerUpdateResult
                {
                    Success = false,
                    Message = "更新失敗"
                };
            }
        }

        public async Task<PasswordChangeResult> ChangePasswordAsync(int managerId, PasswordChangeDto passwordDto)
        {
            try
            {
                var manager = await _managerRepository.GetByIdAsync(managerId);
                if (manager == null)
                {
                    return new PasswordChangeResult
                    {
                        Success = false,
                        Message = "管理員不存在"
                    };
                }

                if (!VerifyPassword(passwordDto.CurrentPassword, manager.PasswordHash))
                {
                    return new PasswordChangeResult
                    {
                        Success = false,
                        Message = "當前密碼錯誤"
                    };
                }

                manager.PasswordHash = HashPassword(passwordDto.NewPassword);
                _managerRepository.Update(manager);
                await _unitOfWork.SaveChangesAsync();

                _logger.LogInformation("管理員密碼修改成功: {ManagerId}", managerId);

                return new PasswordChangeResult
                {
                    Success = true,
                    Message = "密碼修改成功"
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "管理員密碼修改失敗: {ManagerId}", managerId);
                return new PasswordChangeResult
                {
                    Success = false,
                    Message = "密碼修改失敗"
                };
            }
        }

        public async Task<bool> HasPermissionAsync(int managerId, string permission)
        {
            try
            {
                var permissions = await GetManagerPermissionsAsync(managerId);
                return permissions.Contains(permission);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "檢查管理員權限失敗: {ManagerId}, 權限: {Permission}", managerId, permission);
                return false;
            }
        }

        public async Task<IEnumerable<string>> GetManagerPermissionsAsync(int managerId)
        {
            try
            {
                var manager = await _managerRepository.GetByIdAsync(managerId);
                if (manager == null) return Enumerable.Empty<string>();

                var rolePermissions = await _managerRolePermissionRepository.GetByRoleAsync(manager.Role);
                return rolePermissions.Select(rp => rp.Permission);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "獲取管理員權限失敗: {ManagerId}", managerId);
                return Enumerable.Empty<string>();
            }
        }

        public async Task<IEnumerable<ManagerDto>> GetAllManagersAsync()
        {
            try
            {
                var managers = await _managerRepository.GetAllAsync();
                var managerDtos = new List<ManagerDto>();

                foreach (var manager in managers)
                {
                    var permissions = await GetManagerPermissionsAsync(manager.Id);
                    managerDtos.Add(new ManagerDto
                    {
                        Id = manager.Id,
                        Username = manager.Username,
                        Email = manager.Email,
                        Role = manager.Role,
                        IsActive = manager.IsActive,
                        CreatedAt = manager.CreatedAt,
                        LastLoginAt = manager.LastLoginAt,
                        Permissions = permissions
                    });
                }

                return managerDtos;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "獲取所有管理員失敗");
                return Enumerable.Empty<ManagerDto>();
            }
        }

        public async Task<ManagerCreateResult> CreateManagerAsync(ManagerCreateDto createDto)
        {
            try
            {
                // 檢查用戶名是否已存在
                if (await _managerRepository.ExistsByUsernameAsync(createDto.Username))
                {
                    return new ManagerCreateResult
                    {
                        Success = false,
                        Message = "用戶名已存在"
                    };
                }

                // 檢查郵箱是否已存在
                if (await _managerRepository.ExistsByEmailAsync(createDto.Email))
                {
                    return new ManagerCreateResult
                    {
                        Success = false,
                        Message = "郵箱已被註冊"
                    };
                }

                // 創建新管理員
                var manager = new Manager
                {
                    Username = createDto.Username,
                    Email = createDto.Email,
                    PasswordHash = HashPassword(createDto.Password),
                    Role = createDto.Role,
                    CreatedAt = DateTime.UtcNow,
                    LastLoginAt = DateTime.UtcNow,
                    IsActive = true
                };

                _managerRepository.Add(manager);
                await _unitOfWork.SaveChangesAsync();

                _logger.LogInformation("新管理員創建成功: {Username}, 角色: {Role}", manager.Username, manager.Role);

                return new ManagerCreateResult
                {
                    Success = true,
                    Message = "管理員創建成功",
                    ManagerId = manager.Id
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "創建管理員失敗: {Username}", createDto.Username);
                return new ManagerCreateResult
                {
                    Success = false,
                    Message = "創建失敗"
                };
            }
        }

        public async Task<ManagerUpdateResult> UpdateManagerRoleAsync(int managerId, int updaterId, ManagerRole newRole)
        {
            try
            {
                // 檢查更新者權限
                if (!await HasPermissionAsync(updaterId, "MANAGER_ROLE_UPDATE"))
                {
                    return new ManagerUpdateResult
                    {
                        Success = false,
                        Message = "無權限執行此操作"
                    };
                }

                var manager = await _managerRepository.GetByIdAsync(managerId);
                if (manager == null)
                {
                    return new ManagerUpdateResult
                    {
                        Success = false,
                        Message = "管理員不存在"
                    };
                }

                // 不能修改自己的角色
                if (managerId == updaterId)
                {
                    return new ManagerUpdateResult
                    {
                        Success = false,
                        Message = "不能修改自己的角色"
                    };
                }

                manager.Role = newRole;
                _managerRepository.Update(manager);
                await _unitOfWork.SaveChangesAsync();

                _logger.LogInformation("管理員角色更新成功: {ManagerId}, 新角色: {NewRole}", managerId, newRole);

                return new ManagerUpdateResult
                {
                    Success = true,
                    Message = "角色更新成功"
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "更新管理員角色失敗: {ManagerId}", managerId);
                return new ManagerUpdateResult
                {
                    Success = false,
                    Message = "更新失敗"
                };
            }
        }

        public async Task<ManagerUpdateResult> ActivateManagerAsync(int managerId, int updaterId)
        {
            try
            {
                // 檢查更新者權限
                if (!await HasPermissionAsync(updaterId, "MANAGER_STATUS_UPDATE"))
                {
                    return new ManagerUpdateResult
                    {
                        Success = false,
                        Message = "無權限執行此操作"
                    };
                }

                var manager = await _managerRepository.GetByIdAsync(managerId);
                if (manager == null)
                {
                    return new ManagerUpdateResult
                    {
                        Success = false,
                        Message = "管理員不存在"
                    };
                }

                manager.IsActive = true;
                _managerRepository.Update(manager);
                await _unitOfWork.SaveChangesAsync();

                _logger.LogInformation("管理員啟用成功: {ManagerId}", managerId);

                return new ManagerUpdateResult
                {
                    Success = true,
                    Message = "管理員已啟用"
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "啟用管理員失敗: {ManagerId}", managerId);
                return new ManagerUpdateResult
                {
                    Success = false,
                    Message = "啟用失敗"
                };
            }
        }

        public async Task<ManagerUpdateResult> DeactivateManagerAsync(int managerId, int updaterId)
        {
            try
            {
                // 檢查更新者權限
                if (!await HasPermissionAsync(updaterId, "MANAGER_STATUS_UPDATE"))
                {
                    return new ManagerUpdateResult
                    {
                        Success = false,
                        Message = "無權限執行此操作"
                    };
                }

                var manager = await _managerRepository.GetByIdAsync(managerId);
                if (manager == null)
                {
                    return new ManagerUpdateResult
                    {
                        Success = false,
                        Message = "管理員不存在"
                    };
                }

                // 不能停用自己
                if (managerId == updaterId)
                {
                    return new ManagerUpdateResult
                    {
                        Success = false,
                        Message = "不能停用自己"
                    };
                }

                manager.IsActive = false;
                _managerRepository.Update(manager);
                await _unitOfWork.SaveChangesAsync();

                _logger.LogInformation("管理員停用成功: {ManagerId}", managerId);

                return new ManagerUpdateResult
                {
                    Success = true,
                    Message = "管理員已停用"
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "停用管理員失敗: {ManagerId}", managerId);
                return new ManagerUpdateResult
                {
                    Success = false,
                    Message = "停用失敗"
                };
            }
        }

        public async Task<IEnumerable<ManagerRolePermissionDto>> GetRolePermissionsAsync(ManagerRole role)
        {
            try
            {
                var rolePermissions = await _managerRolePermissionRepository.GetByRoleAsync(role);
                return rolePermissions.Select(rp => new ManagerRolePermissionDto
                {
                    Id = rp.Id,
                    Role = rp.Role,
                    Permission = rp.Permission,
                    Description = rp.Description,
                    CreatedAt = rp.CreatedAt
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "獲取角色權限失敗: {Role}", role);
                return Enumerable.Empty<ManagerRolePermissionDto>();
            }
        }

        public async Task<bool> ExistsAsync(int managerId)
        {
            return await _managerRepository.ExistsAsync(managerId);
        }

        public async Task<bool> ExistsByUsernameAsync(string username)
        {
            return await _managerRepository.ExistsByUsernameAsync(username);
        }

        public async Task<bool> ExistsByEmailAsync(string email)
        {
            return await _managerRepository.ExistsByEmailAsync(email);
        }

        public async Task<ManagerDto> GetByIdAsync(int managerId)
        {
            var manager = await _managerRepository.GetByIdAsync(managerId);
            if (manager == null) return null;

            var permissions = await GetManagerPermissionsAsync(manager.Id);

            return new ManagerDto
            {
                Id = manager.Id,
                Username = manager.Username,
                Email = manager.Email,
                Role = manager.Role,
                IsActive = manager.IsActive,
                CreatedAt = manager.CreatedAt,
                LastLoginAt = manager.LastLoginAt,
                Permissions = permissions
            };
        }

        public async Task<ManagerDto> GetByUsernameAsync(string username)
        {
            var manager = await _managerRepository.GetByUsernameAsync(username);
            if (manager == null) return null;

            var permissions = await GetManagerPermissionsAsync(manager.Id);

            return new ManagerDto
            {
                Id = manager.Id,
                Username = manager.Username,
                Email = manager.Email,
                Role = manager.Role,
                IsActive = manager.IsActive,
                CreatedAt = manager.CreatedAt,
                LastLoginAt = manager.LastLoginAt,
                Permissions = permissions
            };
        }

        public async Task<ManagerDto> GetByEmailAsync(string email)
        {
            var manager = await _managerRepository.GetByEmailAsync(email);
            if (manager == null) return null;

            var permissions = await GetManagerPermissionsAsync(manager.Id);

            return new ManagerDto
            {
                Id = manager.Id,
                Username = manager.Username,
                Email = manager.Email,
                Role = manager.Role,
                IsActive = manager.IsActive,
                CreatedAt = manager.CreatedAt,
                LastLoginAt = manager.LastLoginAt,
                Permissions = permissions
            };
        }

        public async Task<ManagerUpdateResult> UpdateManagerRoleAsync(int managerId, int updaterId, string newRole)
        {
            try
            {
                var manager = await _managerRepository.GetByIdAsync(managerId);
                if (manager == null)
                {
                    return new ManagerUpdateResult
                    {
                        Success = false,
                        Message = "管理員不存在"
                    };
                }

                manager.Role = newRole;
                manager.UpdatedAt = DateTime.UtcNow;
                _managerRepository.Update(manager);
                await _unitOfWork.SaveChangesAsync();

                _logger.LogInformation("管理員角色更新成功: {ManagerId} -> {NewRole}", managerId, newRole);

                return new ManagerUpdateResult
                {
                    Success = true,
                    Message = "角色更新成功"
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "更新管理員角色失敗: {ManagerId}", managerId);
                return new ManagerUpdateResult
                {
                    Success = false,
                    Message = "更新失敗"
                };
            }
        }

        public async Task<IEnumerable<ManagerRolePermissionDto>> GetRolePermissionsAsync(string role)
        {
            try
            {
                var permissions = await _managerRolePermissionRepository.GetByRoleAsync(role);
                return permissions.Select(p => new ManagerRolePermissionDto
                {
                    Role = p.Role,
                    Permission = p.Permission,
                    Description = p.Description
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "獲取角色權限失敗: {Role}", role);
                return Enumerable.Empty<ManagerRolePermissionDto>();
            }
        }

        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(hashedBytes);
            }
        }

        private bool VerifyPassword(string password, string hash)
        {
            var passwordHash = HashPassword(password);
            return passwordHash == hash;
        }
    }
}