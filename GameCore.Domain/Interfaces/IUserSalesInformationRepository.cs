using GameCore.Domain.Entities;

namespace GameCore.Domain.Interfaces
{
    /// <summary>
    /// 用戶銷售資訊 Repository 介面
    /// </summary>
    public interface IUserSalesInformationRepository : IRepository<UserSalesInformation>
    {
        /// <summary>
        /// 根據用戶ID取得銷售資訊
        /// </summary>
        Task<UserSalesInformation?> GetByUserIdAsync(int userId);

        /// <summary>
        /// 新增銷售資訊
        /// </summary>
        Task<UserSalesInformation> AddAsync(UserSalesInformation info);

        /// <summary>
        /// 更新銷售資訊
        /// </summary>
        Task UpdateAsync(UserSalesInformation info);

        /// <summary>
        /// 刪除銷售資訊
        /// </summary>
        Task DeleteAsync(UserSalesInformation info);
    }
} 
