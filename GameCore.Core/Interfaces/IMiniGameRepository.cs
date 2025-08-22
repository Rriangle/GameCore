using GameCore.Core.Entities;

namespace GameCore.Core.Interfaces
{
    /// <summary>
    /// 小冒險遊戲倉庫介面
    /// </summary>
    public interface IMiniGameRepository : IRepository<MiniGameRecord>
    {
        /// <summary>
        /// 檢查使用者今日是否已達到遊戲次數限制
        /// </summary>
        /// <param name="userId">使用者ID</param>
        /// <param name="date">遊戲日期</param>
        /// <returns>是否已達限制</returns>
        Task<bool> HasReachedDailyLimitAsync(int userId, DateTime date);

        /// <summary>
        /// 取得使用者今日的遊戲次數
        /// </summary>
        /// <param name="userId">使用者ID</param>
        /// <param name="date">遊戲日期</param>
        /// <returns>遊戲次數</returns>
        Task<int> GetTodayGameCountAsync(int userId, DateTime date);

        /// <summary>
        /// 取得遊戲設定
        /// </summary>
        /// <param name="gameLevel">遊戲等級</param>
        /// <returns>遊戲設定</returns>
        Task<MiniGameSettings?> GetGameSettingsAsync(int gameLevel);

        /// <summary>
        /// 取得使用者的遊戲記錄
        /// </summary>
        /// <param name="userId">使用者ID</param>
        /// <param name="page">頁碼</param>
        /// <param name="pageSize">每頁大小</param>
        /// <returns>遊戲記錄列表</returns>
        Task<IEnumerable<MiniGameRecord>> GetUserGameRecordsAsync(int userId, int page, int pageSize);

        /// <summary>
        /// 取得寵物的遊戲記錄
        /// </summary>
        /// <param name="petId">寵物ID</param>
        /// <param name="page">頁碼</param>
        /// <param name="pageSize">每頁大小</param>
        /// <returns>遊戲記錄列表</returns>
        Task<IEnumerable<MiniGameRecord>> GetPetGameRecordsAsync(int petId, int page, int pageSize);
    }
}