using GameCore.Domain.Entities;

namespace GameCore.Domain.Interfaces
{
    /// <summary>
    /// 管理者角色權限 Repository 介面
    /// </summary>
    public interface IManagerRolePermissionRepository : IRepository<ManagerRolePermission>
    {
        /// <summary>
        /// 根據角色取得權限
        /// </summary>
        Task<IEnumerable<ManagerRolePermission>> GetByRoleAsync(string role);

        /// <summary>
        /// 根據權限取得角色
        /// </summary>
        Task<IEnumerable<ManagerRolePermission>> GetByPermissionAsync(string permission);

        /// <summary>
        /// 新增角色權限
        /// </summary>
        Task<ManagerRolePermission> AddAsync(ManagerRolePermission permission);

        /// <summary>
        /// 更新角色權限
        /// </summary>
        Task UpdateAsync(ManagerRolePermission permission);

        /// <summary>
        /// 刪除角色權限
        /// </summary>
        Task DeleteAsync(ManagerRolePermission permission);
    }
} 
