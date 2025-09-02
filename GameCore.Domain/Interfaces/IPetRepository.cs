using GameCore.Domain.Entities;

namespace GameCore.Domain.Interfaces
{
    /// <summary>
    /// 寵物倉庫介面
    /// </summary>
    public interface IPetRepository : IRepository<Pet>
    {
        /// <summary>
        /// 取得用戶的寵物
        /// </summary>
        /// <param name="userId">用戶ID</param>
        /// <returns>寵物</returns>
        Task<Pet?> GetByUserIdAsync(int userId);

        /// <summary>
        /// 檢查用戶是否有寵物
        /// </summary>
        /// <param name="userId">用戶ID</param>
        /// <returns>是否有寵物</returns>
        Task<bool> HasPetAsync(int userId);

        /// <summary>
        /// 更新寵物經驗值
        /// </summary>
        /// <param name="petId">寵物ID</param>
        /// <param name="experience">經驗值</param>
        /// <returns>更新結果</returns>
        Task<bool> UpdateExperienceAsync(int petId, int experience);

        /// <summary>
        /// 更新寵物等級
        /// </summary>
        /// <param name="petId">寵物ID</param>
        /// <param name="level">等級</param>
        /// <returns>更新結果</returns>
        Task<bool> UpdateLevelAsync(int petId, int level);

        /// <summary>
        /// 更新寵物健康度
        /// </summary>
        /// <param name="petId">寵物ID</param>
        /// <param name="health">健康度</param>
        /// <returns>更新結果</returns>
        Task<bool> UpdateHealthAsync(int petId, int health);

        /// <summary>
        /// 更新寵物精力
        /// </summary>
        /// <param name="petId">寵物ID</param>
        /// <param name="energy">精力</param>
        /// <returns>更新結果</returns>
        Task<bool> UpdateEnergyAsync(int petId, int energy);

        /// <summary>
        /// 更新寵物快樂度
        /// </summary>
        /// <param name="petId">寵物ID</param>
        /// <param name="happiness">快樂度</param>
        /// <returns>更新結果</returns>
        Task<bool> UpdateHappinessAsync(int petId, int happiness);

        /// <summary>
        /// 更新寵物飢餓度
        /// </summary>
        /// <param name="petId">寵物ID</param>
        /// <param name="hunger">飢餓度</param>
        /// <returns>更新結果</returns>
        Task<bool> UpdateHungerAsync(int petId, int hunger);

        /// <summary>
        /// 更新寵物心情
        /// </summary>
        /// <param name="petId">寵物ID</param>
        /// <param name="mood">心情</param>
        /// <returns>更新結果</returns>
        Task<bool> UpdateMoodAsync(int petId, int mood);

        /// <summary>
        /// 更新寵物體力
        /// </summary>
        /// <param name="petId">寵物ID</param>
        /// <param name="stamina">體力</param>
        /// <returns>更新結果</returns>
        Task<bool> UpdateStaminaAsync(int petId, int stamina);

        /// <summary>
        /// 更新寵物清潔度
        /// </summary>
        /// <param name="petId">寵物ID</param>
        /// <param name="cleanliness">清潔度</param>
        /// <returns>更新結果</returns>
        Task<bool> UpdateCleanlinessAsync(int petId, int cleanliness);

        /// <summary>
        /// 更新寵物顏色
        /// </summary>
        /// <param name="petId">寵物ID</param>
        /// <param name="skinColor">膚色</param>
        /// <param name="backgroundColor">背景色</param>
        /// <returns>更新結果</returns>
        Task<bool> UpdateColorAsync(int petId, string skinColor, string backgroundColor);

        /// <summary>
        /// 更新最後互動時間
        /// </summary>
        /// <param name="petId">寵物ID</param>
        /// <param name="interactionTime">互動時間</param>
        /// <returns>更新結果</returns>
        Task<bool> UpdateLastInteractionAsync(int petId, DateTime interactionTime);

        /// <summary>
        /// 更新寵物
        /// </summary>
        Task<Pet> Update(Pet pet);

        /// <summary>
        /// 取得最後互動時間
        /// </summary>
        Task<DateTime?> GetLastInteractionTimeAsync(int petId);
    }
} 
