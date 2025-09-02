using GameCore.Domain.Entities;

namespace GameCore.Domain.Interfaces
{
    /// <summary>
    /// 會員銷售資料 Repository 介面
    /// </summary>
    public interface IMemberSalesProfileRepository : IRepository<MemberSalesProfile>
    {
        /// <summary>
        /// 根據用戶ID取得銷售資料
        /// </summary>
        Task<MemberSalesProfile?> GetByUserIdAsync(int userId);

        /// <summary>
        /// 根據狀態取得銷售資料
        /// </summary>
        Task<IEnumerable<MemberSalesProfile>> GetByStatusAsync(string status);

        /// <summary>
        /// 新增銷售資料
        /// </summary>
        Task<MemberSalesProfile> AddAsync(MemberSalesProfile profile);

        /// <summary>
        /// 更新銷售資料
        /// </summary>
        Task UpdateAsync(MemberSalesProfile profile);

        /// <summary>
        /// 刪除銷售資料
        /// </summary>
        Task DeleteAsync(MemberSalesProfile profile);

        /// <summary>
        /// 取得所有銷售資料
        /// </summary>
        Task<IEnumerable<MemberSalesProfile>> GetAll();
    }
} 
