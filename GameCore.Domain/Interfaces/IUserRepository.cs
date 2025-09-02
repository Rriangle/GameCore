using System.Collections.Generic;
using System.Threading.Tasks;
using GameCore.Domain.Entities;

namespace GameCore.Domain.Interfaces
{
    /// <summary>
    /// 用戶倉庫介面
    /// </summary>
    public interface IUserRepository : IRepository<User>
    {
        /// <summary>
        /// 根據電子郵件取得用戶
        /// </summary>
        /// <param name="email">電子郵件</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>用戶</returns>
        Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);

        /// <summary>
        /// 根據用戶名取得用戶
        /// </summary>
        /// <param name="userName">用戶名</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>用戶</returns>
        Task<User?> GetByUserNameAsync(string userName, CancellationToken cancellationToken = default);

        /// <summary>
        /// 檢查電子郵件是否存在
        /// </summary>
        /// <param name="email">電子郵件</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>是否存在</returns>
        Task<bool> ExistsByEmailAsync(string email, CancellationToken cancellationToken = default);

        /// <summary>
        /// 檢查用戶名是否存在
        /// </summary>
        /// <param name="userName">用戶名</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>是否存在</returns>
        Task<bool> ExistsByUserNameAsync(string userName, CancellationToken cancellationToken = default);

        /// <summary>
        /// 取得啟用的用戶
        /// </summary>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>啟用的用戶列表</returns>
        Task<IEnumerable<User>> GetActiveUsersAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// 根據角色取得用戶
        /// </summary>
        /// <param name="role">角色</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>用戶列表</returns>
        Task<IEnumerable<User>> GetUsersByRoleAsync(string role, CancellationToken cancellationToken = default);

        /// <summary>
        /// 搜尋用戶
        /// </summary>
        /// <param name="searchTerm">搜尋關鍵字</param>
        /// <param name="pageNumber">頁碼</param>
        /// <param name="pageSize">每頁大小</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>用戶列表</returns>
        Task<IEnumerable<User>> SearchUsersAsync(string searchTerm, int pageNumber, int pageSize, CancellationToken cancellationToken = default);

        /// <summary>
        /// 取得用戶統計資訊
        /// </summary>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>統計資訊</returns>
        Task<object> GetUserStatsAsync(CancellationToken cancellationToken = default);
    }
} 
