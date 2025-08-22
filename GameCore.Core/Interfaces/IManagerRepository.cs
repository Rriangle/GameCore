using GameCore.Core.Entities;

namespace GameCore.Core.Interfaces
{
    /// <summary>
    /// 管理員 Repository 介面
    /// </summary>
    public interface IManagerRepository : IRepository<ManagerData>
    {
        /// <summary>
        /// 取得管理員權限
        /// </summary>
        /// <param name="managerId">管理員ID</param>
        /// <returns></returns>
        Task<IEnumerable<ManagerRolePermission>> GetPermissionsAsync(int managerId);

        /// <summary>
        /// 取得管理員及其角色
        /// </summary>
        /// <param name="managerId">管理員ID</param>
        /// <returns></returns>
        Task<ManagerData?> GetWithRoleAsync(int managerId);

        /// <summary>
        /// 根據用戶ID取得管理員資料
        /// </summary>
        /// <param name="userId">用戶ID</param>
        /// <returns></returns>
        Task<ManagerData?> GetByUserIdAsync(int userId);

        /// <summary>
        /// 檢查用戶是否為管理員
        /// </summary>
        /// <param name="userId">用戶ID</param>
        /// <returns></returns>
        Task<bool> IsManagerAsync(int userId);
    }
}
