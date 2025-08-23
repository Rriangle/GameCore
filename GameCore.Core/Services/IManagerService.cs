using GameCore.Core.Services;

namespace GameCore.Core.Services
{
    public interface IManagerService
    {
        Task<ManagerLoginResult> LoginAsync(ManagerLoginDto loginDto);
        Task<bool> LogoutAsync(int managerId);
        Task<ManagerProfileResult> GetProfileAsync(int managerId);
        Task<ManagerUpdateResult> UpdateProfileAsync(int managerId, ManagerUpdateDto updateDto);
        Task<PasswordChangeResult> ChangePasswordAsync(int managerId, PasswordChangeDto passwordDto);
        Task<bool> HasPermissionAsync(int managerId, string permission);
        Task<IEnumerable<string>> GetManagerPermissionsAsync(int managerId);
        Task<IEnumerable<ManagerDto>> GetAllManagersAsync();
        Task<ManagerCreateResult> CreateManagerAsync(ManagerCreateDto createDto);
        Task<ManagerUpdateResult> UpdateManagerRoleAsync(int managerId, int updaterId, ManagerRole newRole);
        Task<ManagerUpdateResult> ActivateManagerAsync(int managerId, int updaterId);
        Task<ManagerUpdateResult> DeactivateManagerAsync(int managerId, int updaterId);
        Task<IEnumerable<ManagerRolePermissionDto>> GetRolePermissionsAsync(ManagerRole role);
        Task<bool> ExistsAsync(int managerId);
        Task<bool> ExistsByUsernameAsync(string username);
        Task<bool> ExistsByEmailAsync(string email);
        Task<ManagerDto> GetByIdAsync(int managerId);
        Task<ManagerDto> GetByUsernameAsync(string username);
        Task<ManagerDto> GetByEmailAsync(string email);
    }
}