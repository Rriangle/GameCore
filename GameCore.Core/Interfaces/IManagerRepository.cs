using GameCore.Core.Entities;

namespace GameCore.Core.Interfaces
{
    /// <summary>
    /// 管理員倉庫介面
    /// </summary>
    public interface IManagerRepository : IRepository<ManagerData>
    {
        /// <summary>
        /// 根據帳號取得管理員
        /// </summary>
        /// <param name="account">帳號</param>
        /// <returns>管理員</returns>
        Task<ManagerData?> GetByAccountAsync(string account);

        /// <summary>
        /// 驗證管理員登入
        /// </summary>
        /// <param name="account">帳號</param>
        /// <param name="password">密碼</param>
        /// <returns>管理員</returns>
        Task<ManagerData?> ValidateLoginAsync(string account, string password);

        /// <summary>
        /// 取得管理員權限
        /// </summary>
        /// <param name="managerId">管理員ID</param>
        /// <returns>權限</returns>
        Task<ManagerRolePermission?> GetPermissionsAsync(int managerId);

        /// <summary>
        /// 更新管理員最後登入時間
        /// </summary>
        /// <param name="managerId">管理員ID</param>
        /// <returns>操作結果</returns>
        Task<bool> UpdateLastLoginAsync(int managerId);
    }

    /// <summary>
    /// 管理員角色權限倉庫介面
    /// </summary>
    public interface IManagerRolePermissionRepository : IRepository<ManagerRolePermission>
    {
        /// <summary>
        /// 取得所有角色權限
        /// </summary>
        /// <returns>角色權限列表</returns>
        Task<IEnumerable<ManagerRolePermission>> GetAllRolesAsync();

        /// <summary>
        /// 根據角色名稱取得權限
        /// </summary>
        /// <param name="roleName">角色名稱</param>
        /// <returns>角色權限</returns>
        Task<ManagerRolePermission?> GetByRoleNameAsync(string roleName);

        /// <summary>
        /// 建立新角色
        /// </summary>
        /// <param name="role">角色權限</param>
        /// <returns>操作結果</returns>
        Task<bool> CreateRoleAsync(ManagerRolePermission role);

        /// <summary>
        /// 更新角色權限
        /// </summary>
        /// <param name="roleId">角色ID</param>
        /// <param name="permissions">權限</param>
        /// <returns>操作結果</returns>
        Task<bool> UpdatePermissionsAsync(int roleId, ManagerRolePermission permissions);
    }
}