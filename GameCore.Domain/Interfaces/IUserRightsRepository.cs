using GameCore.Domain.Entities;

namespace GameCore.Domain.Interfaces
{
    /// <summary>
    /// 用戶權限 Repository 介面
    /// </summary>
    public interface IUserRightsRepository : IRepository<UserRights>
    {
        /// <summary>
        /// 根據用戶ID取得權限
        /// </summary>
        Task<UserRights?> GetByUserIdAsync(int userId);

        /// <summary>
        /// 根據權限類型取得權限
        /// </summary>
        Task<IEnumerable<UserRights>> GetByRightTypeAsync(string rightType);

        /// <summary>
        /// 新增權限
        /// </summary>
        Task<UserRights> AddAsync(UserRights rights);

        /// <summary>
        /// 更新權限
        /// </summary>
        Task UpdateAsync(UserRights rights);

        /// <summary>
        /// 刪除權限
        /// </summary>
        Task DeleteAsync(UserRights rights);

        /// <summary>
        /// 取得所有權限
        /// </summary>
        Task<IEnumerable<UserRights>> GetAll();
    }
} 
