using GameCore.Core.Entities;

namespace GameCore.Core.Interfaces
{
    /// <summary>
    /// 管理者服務介面
    /// </summary>
    public interface IManagerService
    {
        Task<ManagerData?> GetManagerByAccountAsync(string account);
        Task<IEnumerable<ManagerRolePermission>> GetManagerPermissionsAsync(int managerId);
        Task<bool> ValidateManagerCredentialsAsync(string account, string password);
    }
}