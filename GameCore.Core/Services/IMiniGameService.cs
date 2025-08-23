using GameCore.Core.Entities;

namespace GameCore.Core.Services
{
    /// <summary>
    /// 小冒險遊戲服務介面
    /// </summary>
    public interface IMiniGameService
    {
        /// <summary>
        /// 開始小冒險遊戲
        /// </summary>
        /// <param name="userId">使用者ID</param>
        /// <param name="petId">寵物ID</param>
        /// <param name="gameLevel">遊戲等級</param>
        /// <returns>遊戲結果</returns>
        Task<MiniGameResult> StartGameAsync(int userId, int petId, int gameLevel);

        /// <summary>
        /// 檢查使用者是否可以進行遊戲
        /// </summary>
        /// <param name="userId">使用者ID</param>
        /// <param name="petId">寵物ID</param>
        /// <returns>遊戲檢查結果</returns>
        Task<GameCheckResult> CheckGameEligibilityAsync(int userId, int petId);

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

    /// <summary>
    /// 遊戲結果模型
    /// </summary>
    public class MiniGameResult
    {
        public bool IsSuccess { get; set; }
        public int PointsEarned { get; set; }
        public int ExperienceEarned { get; set; }
        public PetAttributeChanges AttributeChanges { get; set; } = new();
        public string Message { get; set; } = string.Empty;
    }

    /// <summary>
    /// 寵物屬性變化模型
    /// </summary>
    public class PetAttributeChanges
    {
        public int HealthChange { get; set; }
        public int HungerChange { get; set; }
        public int CleanlinessChange { get; set; }
        public int HappinessChange { get; set; }
        public int EnergyChange { get; set; }
    }

    /// <summary>
    /// 遊戲檢查結果模型
    /// </summary>
    public class GameCheckResult
    {
        public bool CanPlay { get; set; }
        public string Message { get; set; } = string.Empty;
        public int RemainingGames { get; set; }
        public int DailyLimit { get; set; }
    }
}