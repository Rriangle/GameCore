using GameCore.Core.Entities;

namespace GameCore.Core.Interfaces
{
    /// <summary>
    /// 簽到 Repository 介面
    /// </summary>
    public interface ISignInRepository : IRepository<UserSignInStats>
    {
        /// <summary>
        /// 根據日期取得簽到記錄
        /// </summary>
        /// <param name="userId">用戶ID</param>
        /// <param name="date">日期</param>
        /// <returns></returns>
        Task<UserSignInStats?> GetSignInByDateAsync(int userId, DateTime date);

        /// <summary>
        /// 取得最近簽到記錄
        /// </summary>
        /// <param name="userId">用戶ID</param>
        /// <param name="count">數量</param>
        /// <returns></returns>
        Task<IEnumerable<UserSignInStats>> GetRecentSignInsAsync(int userId, int count);

        /// <summary>
        /// 取得日期範圍內的簽到記錄
        /// </summary>
        /// <param name="userId">用戶ID</param>
        /// <param name="startDate">開始日期</param>
        /// <param name="endDate">結束日期</param>
        /// <returns></returns>
        Task<IEnumerable<UserSignInStats>> GetSignInsByDateRangeAsync(int userId, DateTime startDate, DateTime endDate);

        /// <summary>
        /// 取得簽到總數
        /// </summary>
        /// <param name="userId">用戶ID</param>
        /// <returns></returns>
        Task<int> GetSignInCountAsync(int userId);

        /// <summary>
        /// 取得簽到歷史（分頁）
        /// </summary>
        /// <param name="userId">用戶ID</param>
        /// <param name="page">頁碼</param>
        /// <param name="pageSize">每頁數量</param>
        /// <returns></returns>
        Task<IEnumerable<UserSignInStats>> GetSignInHistoryAsync(int userId, int page, int pageSize);

        /// <summary>
        /// 取得連續簽到天數
        /// </summary>
        /// <param name="userId">用戶ID</param>
        /// <returns></returns>
        Task<int> GetStreakDaysAsync(int userId);
    }
}
