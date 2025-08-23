using GameCore.Core.Entities;
using GameCore.Core.Interfaces;

namespace GameCore.Core.Services
{
    public class ManagerService : IManagerService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ManagerService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ManagerData?> GetManagerByAccountAsync(string account)
        {
            var managers = await _unitOfWork.ManagerRepository.GetAllAsync();
            return managers.FirstOrDefault(m => m.ManagerAccount == account);
        }

        public async Task<IEnumerable<ManagerRolePermission>> GetManagerPermissionsAsync(int managerId)
        {
            var permissions = await _unitOfWork.ManagerRepository.GetPermissionsAsync(managerId);
            return permissions;
        }

        public async Task<bool> ValidateManagerCredentialsAsync(string account, string password)
        {
            var manager = await GetManagerByAccountAsync(account);
            if (manager == null) return false;

            // 這裡應該使用適當的密碼驗證方法
            // 暫時使用簡單的字符串比較，實際應用中應該使用 BCrypt 等
            return manager.ManagerPassword == password;
        }
    }
}