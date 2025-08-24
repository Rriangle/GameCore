using GameCore.Core.Entities;
using GameCore.Core.Interfaces;
using Microsoft.Extensions.Logging;

namespace GameCore.Core.Services
{
    /// <summary>
    /// 寵物服務實作
    /// 包含所有寵物相關的業務邏輯，如互動、升級、健康檢查等
    /// </summary>
    public class PetService : IPetService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<PetService> _logger;

        // 寵物系統常數設定
        private const int COLOR_CHANGE_COST = 2000; // 換色費用
        private const int MAX_PET_LEVEL = 250; // 最大等級

        // 每日衰減數值 (按照規格要求)
        private const int DAILY_HUNGER_DECAY = 20;
        private const int DAILY_MOOD_DECAY = 30;
        private const int DAILY_STAMINA_DECAY = 10;
        private const int DAILY_CLEANLINESS_DECAY = 20;
        private const int DAILY_HEALTH_DECAY = 20;

        // 互動增加數值
        private const int INTERACTION_INCREASE = 10;

        // 健康度懲罰閾值
        private const int HEALTH_PENALTY_THRESHOLD = 30;

        public PetService(IUnitOfWork unitOfWork, ILogger<PetService> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        /// <summary>
        /// 取得使用者的寵物資料，如果不存在則建立新寵物
        /// </summary>
        public async Task<Pet> GetOrCreatePetAsync(int userId)
        {
            try
            {
                var pet = await _unitOfWork.PetRepository.GetByUserIdAsync(userId);
                
                if (pet == null)
                {
                    // 建立新寵物，按照規格初始值設為 100
                    pet = new Pet
                    {
                        UserId = userId,
                        Name = "小可愛",
                        Type = "Default",
                        Level = 1,
                        Experience = 0,
                        Hunger = 100,
                        Mood = 100,
                        Stamina = 100,
                        Cleanliness = 100,
                        Health = 100,
                        Color = "Default",
                        IsActive = true,
                        CreateTime = DateTime.UtcNow,
                        UpdateTime = DateTime.UtcNow
                    };

                    await _unitOfWork.PetRepository.AddAsync(pet);
                    await _unitOfWork.SaveChangesAsync();

                    _logger.LogInformation($"為使用者 {userId} 建立新寵物，ID: {pet.Id}");
                }

                return pet;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"取得或建立寵物時發生錯誤，使用者ID: {userId}");
                throw;
            }
        }

        /// <summary>
        /// 執行寵物互動 (餵食、洗澡、玩耍、休息)
        /// </summary>
        public async Task<PetInteractionResult> InteractWithPetAsync(int userId, PetInteractionType interactionType)
        {
            try
            {
                var pet = await GetOrCreatePetAsync(userId);

                // 檢查寵物是否可以進行互動 (健康度或任何屬性為0時禁止)
                if (pet.Health == 0 || pet.Hunger == 0 || pet.Mood == 0 || pet.Stamina == 0 || pet.Cleanliness == 0)
                {
                    return new PetInteractionResult
                    {
                        Success = false,
                        Message = "寵物狀態不佳，無法進行互動"
                    };
                }

                var originalHealth = pet.Health;

                // 執行互動邏輯
                switch (interactionType)
                {
                    case PetInteractionType.Feed:
                        pet.Hunger = Math.Min(100, pet.Hunger + INTERACTION_INCREASE);
                        break;
                    case PetInteractionType.Bath:
                        pet.Cleanliness = Math.Min(100, pet.Cleanliness + INTERACTION_INCREASE);
                        break;
                    case PetInteractionType.Play:
                        pet.Mood = Math.Min(100, pet.Mood + INTERACTION_INCREASE);
                        break;
                    case PetInteractionType.Rest:
                        pet.Stamina = Math.Min(100, pet.Stamina + INTERACTION_INCREASE);
                        break;
                    default:
                        return new PetInteractionResult
                        {
                            Success = false,
                            Message = "無效的互動類型"
                        };
                }

                // 檢查健康度回復條件 (四維全滿時回復健康)
                bool healthRestored = false;
                if (pet.Hunger == 100 && pet.Mood == 100 && pet.Stamina == 100 && pet.Cleanliness == 100)
                {
                    pet.Health = 100;
                    healthRestored = true;
                }

                // 更新健康度 (根據低屬性懲罰)
                await UpdateHealthStatusAsync(pet);

                // 記錄互動
                var interaction = new PetInteraction
                {
                    PetId = pet.Id,
                    UserId = userId,
                    InteractionType = interactionType.ToString(),
                    Description = $"執行{GetInteractionActionName(interactionType)}",
                    CreateTime = DateTime.UtcNow
                };

                await _unitOfWork.PetRepository.AddInteractionAsync(interaction);
                await _unitOfWork.PetRepository.UpdateAsync(pet);
                await _unitOfWork.SaveChangesAsync();

                var actionName = GetInteractionActionName(interactionType);
                var message = healthRestored 
                    ? $"執行{actionName}！四維屬性全滿，健康度完全恢復！" 
                    : $"執行{actionName}！{GetAttributeName(interactionType)} +{INTERACTION_INCREASE}";

                _logger.LogInformation($"使用者 {userId} 對寵物執行 {actionName}，健康度恢復: {healthRestored}");

                return new PetInteractionResult
                {
                    Success = true,
                    Message = message,
                    Pet = pet,
                    HealthRestored = healthRestored
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"寵物互動時發生錯誤，使用者ID: {userId}, 互動類型: {interactionType}");
                return new PetInteractionResult
                {
                    Success = false,
                    Message = "互動失敗，請稍後再試"
                };
            }
        }

        /// <summary>
        /// 寵物換色 (消耗 2000 點數)
        /// </summary>
        public async Task<PetColorChangeResult> ChangePetColorAsync(int userId, string color)
        {
            try
            {
                var pet = await GetOrCreatePetAsync(userId);
                var user = await _unitOfWork.UserRepository.GetByIdAsync(userId);

                if (user == null || user.Points < COLOR_CHANGE_COST)
                {
                    return new PetColorChangeResult
                    {
                        Success = false,
                        Message = $"點數不足！需要 {COLOR_CHANGE_COST} 點數，目前只有 {user?.Points ?? 0} 點數",
                        RemainingPoints = user?.Points ?? 0
                    };
                }

                // 扣除點數
                user.Points -= COLOR_CHANGE_COST;
                await _unitOfWork.UserRepository.UpdateAsync(user);

                // 更新寵物外觀
                pet.Color = color;
                pet.UpdateTime = DateTime.UtcNow;

                await _unitOfWork.PetRepository.UpdateAsync(pet);

                // 建立通知記錄
                await _unitOfWork.NotificationRepository.CreatePetColorChangeNotificationAsync(
                    userId, color, COLOR_CHANGE_COST);

                await _unitOfWork.SaveChangesAsync();

                _logger.LogInformation($"使用者 {userId} 為寵物換色，消耗 {COLOR_CHANGE_COST} 點數");

                return new PetColorChangeResult
                {
                    Success = true,
                    Message = "寵物換色成功！",
                    PointsUsed = COLOR_CHANGE_COST,
                    Pet = pet,
                    RemainingPoints = user.Points
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"寵物換色時發生錯誤，使用者ID: {userId}");
                return new PetColorChangeResult
                {
                    Success = false,
                    Message = "換色失敗，請稍後再試"
                };
            }
        }

        /// <summary>
        /// 計算寵物升級所需經驗值 (按照規格公式)
        /// </summary>
        public int CalculateRequiredExperience(int currentLevel)
        {
            if (currentLevel <= 0) return 100; // 安全檢查
            if (currentLevel >= MAX_PET_LEVEL) return int.MaxValue; // 已達最大等級

            if (currentLevel <= 10)
            {
                // Level 1–10：EXP = 40 × level + 60
                return 40 * currentLevel + 60;
            }
            else if (currentLevel <= 100)
            {
                // Level 11–100：EXP = 0.8 × level² + 380
                return (int)(0.8 * currentLevel * currentLevel + 380);
            }
            else
            {
                // Level ≥ 101：EXP = 285.69 × (1.06^level)
                return (int)(285.69 * Math.Pow(1.06, currentLevel));
            }
        }

        /// <summary>
        /// 增加寵物經驗值並處理升級邏輯
        /// </summary>
        public async Task<bool> AddExperienceAsync(Pet pet, int experience)
        {
            if (experience <= 0 || pet.Level >= MAX_PET_LEVEL) return false;

            pet.Experience += experience;
            bool leveledUp = false;

            // 檢查是否可以升級
            while (pet.Level < MAX_PET_LEVEL)
            {
                var requiredExp = CalculateRequiredExperience(pet.Level);
                if (pet.Experience >= requiredExp)
                {
                    pet.Experience -= requiredExp;
                    pet.Level++;
                    leveledUp = true;

                    _logger.LogInformation($"寵物 {pet.Id} 升級到 {pet.Level} 級");
                }
                else
                {
                    break;
                }
            }

            pet.UpdateTime = DateTime.UtcNow;
            await _unitOfWork.PetRepository.UpdateAsync(pet);
            await _unitOfWork.SaveChangesAsync();

            return leveledUp;
        }

        /// <summary>
        /// 檢查寵物是否可以進行小遊戲
        /// </summary>
        public async Task<bool> CanPlayMiniGameAsync(int userId)
        {
            try
            {
                var pet = await GetOrCreatePetAsync(userId);
                
                // 健康度為0或任何屬性為0時禁止遊戲
                return pet.Health > 0 && pet.Hunger > 0 && pet.Mood > 0 && pet.Stamina > 0 && pet.Cleanliness > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"檢查寵物遊戲狀態時發生錯誤，使用者ID: {userId}");
                return false;
            }
        }

        /// <summary>
        /// 更新健康度狀態 (根據低屬性懲罰)
        /// </summary>
        private async Task UpdateHealthStatusAsync(Pet pet)
        {
            var originalHealth = pet.Health;

            // 健康度懲罰規則
            if (pet.Hunger < HEALTH_PENALTY_THRESHOLD)
            {
                pet.Health = Math.Max(0, pet.Health - 20);
            }

            if (pet.Cleanliness < HEALTH_PENALTY_THRESHOLD)
            {
                pet.Health = Math.Max(0, pet.Health - 20);
            }

            if (pet.Stamina < HEALTH_PENALTY_THRESHOLD)
            {
                pet.Health = Math.Max(0, pet.Health - 20);
            }

            // 確保健康度在0-100範圍內
            pet.Health = Math.Max(0, Math.Min(100, pet.Health));

            if (pet.Health != originalHealth)
            {
                _logger.LogInformation($"寵物 {pet.Id} 健康度從 {originalHealth} 調整為 {pet.Health}");
            }
        }

        private string GetInteractionActionName(PetInteractionType interactionType)
        {
            return interactionType switch
            {
                PetInteractionType.Feed => "餵食",
                PetInteractionType.Bath => "洗澡",
                PetInteractionType.Play => "玩耍",
                PetInteractionType.Rest => "休息",
                _ => "互動"
            };
        }

        private string GetAttributeName(PetInteractionType interactionType)
        {
            return interactionType switch
            {
                PetInteractionType.Feed => "飢餓值",
                PetInteractionType.Bath => "清潔值",
                PetInteractionType.Play => "心情值",
                PetInteractionType.Rest => "體力值",
                _ => "屬性"
            };
        }
    }
}