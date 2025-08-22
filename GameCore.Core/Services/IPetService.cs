using GameCore.Core.Entities;

namespace GameCore.Core.Services
{
    /// <summary>
    /// 寵物服務介面
    /// 定義所有寵物相關的業務邏輯操作
    /// </summary>
    public interface IPetService
    {
        /// <summary>
        /// 取得使用者的寵物資料，如果不存在則建立新寵物
        /// </summary>
        /// <param name="userId">使用者編號</param>
        /// <returns>寵物資料</returns>
        Task<Pet> GetOrCreatePetAsync(int userId);

        /// <summary>
        /// 執行寵物互動 (餵食、洗澡、玩耍、休息)
        /// </summary>
        /// <param name="userId">使用者編號</param>
        /// <param name="interactionType">互動類型</param>
        /// <returns>互動結果</returns>
        Task<PetInteractionResult> InteractWithPetAsync(int userId, PetInteractionType interactionType);

        /// <summary>
        /// 寵物換色 (消耗 2000 點數)
        /// </summary>
        /// <param name="userId">使用者編號</param>
        /// <param name="skinColor">新膚色</param>
        /// <param name="backgroundColor">新背景色</param>
        /// <returns>換色結果</returns>
        Task<PetColorChangeResult> ChangePetColorAsync(int userId, string skinColor, string backgroundColor);

        /// <summary>
        /// 計算寵物升級所需經驗值
        /// </summary>
        /// <param name="currentLevel">當前等級</param>
        /// <returns>升級所需經驗值</returns>
        int CalculateRequiredExperience(int currentLevel);

        /// <summary>
        /// 增加寵物經驗值並處理升級邏輯
        /// </summary>
        /// <param name="pet">寵物實體</param>
        /// <param name="experience">要增加的經驗值</param>
        /// <returns>是否有升級</returns>
        Task<bool> AddExperienceAsync(Pet pet, int experience);

        /// <summary>
        /// 檢查寵物健康狀態並更新健康度
        /// </summary>
        /// <param name="pet">寵物實體</param>
        /// <returns>更新後的健康度</returns>
        Task<int> UpdateHealthStatusAsync(Pet pet);

        /// <summary>
        /// 檢查寵物是否可以進行冒險 (健康度和屬性檢查)
        /// </summary>
        /// <param name="userId">使用者編號</param>
        /// <returns>是否可以冒險</returns>
        Task<bool> CanStartAdventureAsync(int userId);

        /// <summary>
        /// 執行每日寵物屬性衰減 (每日 00:00 執行)
        /// </summary>
        /// <returns>受影響的寵物數量</returns>
        Task<int> ExecuteDailyDecayAsync();

        /// <summary>
        /// 取得寵物互動冷卻時間 (防止頻繁互動)
        /// </summary>
        /// <param name="userId">使用者編號</param>
        /// <param name="interactionType">互動類型</param>
        /// <returns>剩餘冷卻秒數</returns>
        Task<int> GetInteractionCooldownAsync(int userId, PetInteractionType interactionType);

        /// <summary>
        /// 取得寵物狀態描述 (用於前端顯示)
        /// </summary>
        /// <param name="pet">寵物實體</param>
        /// <returns>狀態描述</returns>
        PetStatusDescription GetPetStatusDescription(Pet pet);
    }

    /// <summary>
    /// 寵物互動類型列舉
    /// </summary>
    public enum PetInteractionType
    {
        /// <summary>
        /// 餵食 - 增加飢餓值
        /// </summary>
        Feed = 1,

        /// <summary>
        /// 洗澡 - 增加清潔值
        /// </summary>
        Bath = 2,

        /// <summary>
        /// 玩耍 - 增加心情值
        /// </summary>
        Play = 3,

        /// <summary>
        /// 休息 - 增加體力值
        /// </summary>
        Rest = 4
    }

    /// <summary>
    /// 寵物互動結果
    /// </summary>
    public class PetInteractionResult
    {
        /// <summary>
        /// 是否成功
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// 結果訊息
        /// </summary>
        public string Message { get; set; } = string.Empty;

        /// <summary>
        /// 更新後的寵物資料
        /// </summary>
        public Pet? Pet { get; set; }

        /// <summary>
        /// 是否觸發健康度回復 (四維全滿)
        /// </summary>
        public bool HealthRestored { get; set; }

        /// <summary>
        /// 冷卻時間 (秒)
        /// </summary>
        public int CooldownSeconds { get; set; }
    }

    /// <summary>
    /// 寵物換色結果
    /// </summary>
    public class PetColorChangeResult
    {
        /// <summary>
        /// 是否成功
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// 結果訊息
        /// </summary>
        public string Message { get; set; } = string.Empty;

        /// <summary>
        /// 消耗的點數
        /// </summary>
        public int PointsUsed { get; set; }

        /// <summary>
        /// 更新後的寵物資料
        /// </summary>
        public Pet? Pet { get; set; }

        /// <summary>
        /// 剩餘點數
        /// </summary>
        public int RemainingPoints { get; set; }
    }

    /// <summary>
    /// 寵物狀態描述
    /// </summary>
    public class PetStatusDescription
    {
        /// <summary>
        /// 整體狀態 (優秀/良好/普通/不佳/危險)
        /// </summary>
        public string OverallStatus { get; set; } = string.Empty;

        /// <summary>
        /// 最低的屬性名稱
        /// </summary>
        public string LowestAttribute { get; set; } = string.Empty;

        /// <summary>
        /// 最低的屬性值
        /// </summary>
        public int LowestValue { get; set; }

        /// <summary>
        /// 建議的下一步行動
        /// </summary>
        public string SuggestedAction { get; set; } = string.Empty;

        /// <summary>
        /// 是否可以冒險
        /// </summary>
        public bool CanAdventure { get; set; }

        /// <summary>
        /// 表情狀態 (用於動畫顯示)
        /// </summary>
        public string EmotionState { get; set; } = string.Empty;
    }
}