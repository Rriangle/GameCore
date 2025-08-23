using GameCore.Core.Entities;

namespace GameCore.Core.Interfaces
{
    /// <summary>
    /// 簽到倉庫介面
    /// </summary>
    public interface ISignInRepository : IRepository<SignInRecord>
    {
        /// <summary>
        /// 檢查使用者是否已在指定日期簽到
        /// </summary>
        /// <param name="userId">使用者ID</param>
        /// <param name="date">簽到日期</param>
        /// <returns>是否已簽到</returns>
        Task<bool> HasSignedInTodayAsync(int userId, DateTime date);

        /// <summary>
        /// 取得使用者的簽到統計
        /// </summary>
        /// <param name="userId">使用者ID</param>
        /// <param name="year">年份</param>
        /// <param name="month">月份</param>
        /// <returns>簽到統計</returns>
        Task<SignInStatistics?> GetStatisticsAsync(int userId, int year, int month);

        /// <summary>
        /// 取得使用者的連續簽到天數
        /// </summary>
        /// <param name="userId">使用者ID</param>
        /// <returns>連續簽到天數</returns>
        Task<int> GetConsecutiveDaysAsync(int userId);

        /// <summary>
        /// 取得使用者的月度簽到記錄
        /// </summary>
        /// <param name="userId">使用者ID</param>
        /// <param name="year">年份</param>
        /// <param name="month">月份</param>
        /// <returns>簽到記錄列表</returns>
        Task<IEnumerable<SignInRecord>> GetMonthlyRecordsAsync(int userId, int year, int month);

        /// <summary>
        /// 建立或更新簽到統計
        /// </summary>
        /// <param name="statistics">簽到統計</param>
        /// <returns>操作結果</returns>
        Task<bool> UpsertStatisticsAsync(SignInStatistics statistics);
    }
}