using GameCore.Core.Entities;
using GameCore.Core.Services;

namespace GameCore.Core.Interfaces
{
    /// <summary>
    /// 寵物 Repository 介面
    /// 定義寵物相關的資料存取方法
    /// </summary>
    public interface IPetRepository : IRepository<Pet>
    {
        /// <summary>
        /// 根據使用者 ID 取得寵物
        /// </summary>
        /// <param name="userId">使用者 ID</param>
        /// <returns>寵物實體或 null</returns>
        Task<Pet?> GetByUserIdAsync(int userId);

        /// <summary>
        /// 取得需要每日衰減的所有寵物
        /// </summary>
        /// <returns>寵物列表</returns>
        Task<IEnumerable<Pet>> GetPetsForDailyDecayAsync();

        /// <summary>
        /// 更新寵物最後互動時間 (用於冷卻計算)
        /// </summary>
        /// <param name="userId">使用者 ID</param>
        /// <param name="interactionType">互動類型</param>
        /// <param name="interactionTime">互動時間</param>
        /// <returns>Task</returns>
        Task UpdateLastInteractionAsync(int userId, PetInteractionType interactionType, DateTime interactionTime);

        /// <summary>
        /// 取得最後互動時間
        /// </summary>
        /// <param name="userId">使用者 ID</param>
        /// <param name="interactionType">互動類型</param>
        /// <returns>最後互動時間或 null</returns>
        Task<DateTime?> GetLastInteractionTimeAsync(int userId, PetInteractionType interactionType);

        /// <summary>
        /// 取得寵物等級排行榜
        /// </summary>
        /// <param name="top">取前幾名</param>
        /// <returns>排行榜列表</returns>
        Task<IEnumerable<Pet>> GetLevelLeaderboardAsync(int top = 10);

        /// <summary>
        /// 取得特定等級範圍的寵物數量統計
        /// </summary>
        /// <param name="minLevel">最低等級</param>
        /// <param name="maxLevel">最高等級</param>
        /// <returns>寵物數量</returns>
        Task<int> GetPetCountByLevelRangeAsync(int minLevel, int maxLevel);

        /// <summary>
        /// 取得寵物狀態統計 (健康度分佈)
        /// </summary>
        /// <returns>狀態統計</returns>
        Task<PetHealthStats> GetHealthStatsAsync();

        /// <summary>
        /// 搜尋寵物 (根據名稱或主人暱稱)
        /// </summary>
        /// <param name="searchTerm">搜尋關鍵字</param>
        /// <param name="page">頁數</param>
        /// <param name="pageSize">每頁筆數</param>
        /// <returns>搜尋結果</returns>
        Task<PagedResult<Pet>> SearchPetsAsync(string searchTerm, int page = 1, int pageSize = 20);
    }

    /// <summary>
    /// 寵物健康統計
    /// </summary>
    public class PetHealthStats
    {
        /// <summary>
        /// 健康度優秀的寵物數量 (80-100)
        /// </summary>
        public int ExcellentHealthCount { get; set; }

        /// <summary>
        /// 健康度良好的寵物數量 (60-79)
        /// </summary>
        public int GoodHealthCount { get; set; }

        /// <summary>
        /// 健康度普通的寵物數量 (40-59)
        /// </summary>
        public int AverageHealthCount { get; set; }

        /// <summary>
        /// 健康度不佳的寵物數量 (20-39)
        /// </summary>
        public int PoorHealthCount { get; set; }

        /// <summary>
        /// 健康度危險的寵物數量 (0-19)
        /// </summary>
        public int CriticalHealthCount { get; set; }

        /// <summary>
        /// 總寵物數量
        /// </summary>
        public int TotalPetCount { get; set; }

        /// <summary>
        /// 平均健康度
        /// </summary>
        public double AverageHealth { get; set; }

        /// <summary>
        /// 平均等級
        /// </summary>
        public double AverageLevel { get; set; }
    }
}