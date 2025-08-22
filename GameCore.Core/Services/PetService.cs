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
        private const int INTERACTION_COOLDOWN_SECONDS = 3; // 互動冷卻時間
        private const int MAX_PET_LEVEL = 250; // 最大等級

        // 每日衰減數值 (按照規格要求)
        private const int DAILY_HUNGER_DECAY = 20;
        private const int DAILY_MOOD_DECAY = 30;
        private const int DAILY_STAMINA_DECAY = 10;
        private const int DAILY_CLEANLINESS_DECAY = 20;
        private const int DAILY_HEALTH_DECAY = 20;

        // 互動增加數值
        private const int INTERACTION_INCREASE = 10;

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
                        PetName = "小可愛",
                        Level = 1,
                        Experience = 0,
                        Hunger = 100,      // 應用層預設值覆蓋 DB 預設值 0
                        Mood = 100,        // 應用層預設值覆蓋 DB 預設值 0
                        Stamina = 100,     // 應用層預設值覆蓋 DB 預設值 0
                        Cleanliness = 100, // 應用層預設值覆蓋 DB 預設值 0
                        Health = 100,      // 應用層預設值覆蓋 DB 預設值 0
                        SkinColor = "#ADD8E6",
                        BackgroundColor = "粉藍",
                        LevelUpTime = DateTime.UtcNow,
                        ColorChangedTime = DateTime.UtcNow,
                        BackgroundColorChangedTime = DateTime.UtcNow,
                        PointsChangedTime = DateTime.UtcNow
                    };

                    await _unitOfWork.PetRepository.AddAsync(pet);
                    await _unitOfWork.SaveChangesAsync();

                    _logger.LogInformation($"為使用者 {userId} 建立新寵物，ID: {pet.PetId}");
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
                // 檢查冷卻時間
                var cooldownSeconds = await GetInteractionCooldownAsync(userId, interactionType);
                if (cooldownSeconds > 0)
                {
                    return new PetInteractionResult
                    {
                        Success = false,
                        Message = $"請等待 {cooldownSeconds} 秒後再進行互動",
                        CooldownSeconds = cooldownSeconds
                    };
                }

                var pet = await GetOrCreatePetAsync(userId);
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

                // 記錄互動時間 (用於冷卻計算)
                await _unitOfWork.PetRepository.UpdateLastInteractionAsync(userId, interactionType, DateTime.UtcNow);
                
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
                    HealthRestored = healthRestored,
                    CooldownSeconds = INTERACTION_COOLDOWN_SECONDS
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
        public async Task<PetColorChangeResult> ChangePetColorAsync(int userId, string skinColor, string backgroundColor)
        {
            try
            {
                var pet = await GetOrCreatePetAsync(userId);
                var wallet = await _unitOfWork.UserRepository.GetWalletAsync(userId);

                if (wallet == null || wallet.UserPoint < COLOR_CHANGE_COST)
                {
                    return new PetColorChangeResult
                    {
                        Success = false,
                        Message = $"點數不足！需要 {COLOR_CHANGE_COST} 點數，目前只有 {wallet?.UserPoint ?? 0} 點數",
                        RemainingPoints = wallet?.UserPoint ?? 0
                    };
                }

                // 驗證顏色格式
                if (!IsValidColorFormat(skinColor) || !IsValidColorFormat(backgroundColor))
                {
                    return new PetColorChangeResult
                    {
                        Success = false,
                        Message = "顏色格式無效，請使用正確的十六進位色碼 (如: #FF0000)",
                        RemainingPoints = wallet.UserPoint
                    };
                }

                // 扣除點數
                wallet.UserPoint -= COLOR_CHANGE_COST;
                await _unitOfWork.UserRepository.UpdateWalletAsync(wallet);

                // 更新寵物外觀
                pet.SkinColor = skinColor;
                pet.BackgroundColor = backgroundColor;
                pet.ColorChangedTime = DateTime.UtcNow;
                pet.BackgroundColorChangedTime = DateTime.UtcNow;
                pet.PointsChanged = COLOR_CHANGE_COST;
                pet.PointsChangedTime = DateTime.UtcNow;

                await _unitOfWork.PetRepository.UpdateAsync(pet);

                // 建立通知記錄
                await _unitOfWork.NotificationRepository.CreatePetColorChangeNotificationAsync(
                    userId, skinColor, backgroundColor, COLOR_CHANGE_COST);

                await _unitOfWork.SaveChangesAsync();

                _logger.LogInformation($"使用者 {userId} 為寵物換色，消耗 {COLOR_CHANGE_COST} 點數");

                return new PetColorChangeResult
                {
                    Success = true,
                    Message = "寵物換色成功！",
                    PointsUsed = COLOR_CHANGE_COST,
                    Pet = pet,
                    RemainingPoints = wallet.UserPoint
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
                    pet.LevelUpTime = DateTime.UtcNow;
                    leveledUp = true;

                    _logger.LogInformation($"寵物 {pet.PetId} 升級到 {pet.Level} 級");

                    // 升級獎勵 (可在後台設定，這裡先給預設值)
                    var levelUpReward = CalculateLevelUpReward(pet.Level);
                    if (levelUpReward > 0)
                    {
                        await _unitOfWork.UserRepository.AddPointsAsync(pet.UserId, levelUpReward, "寵物升級獎勵");
                    }
                }
                else
                {
                    break;
                }
            }

            if (leveledUp)
            {
                await _unitOfWork.PetRepository.UpdateAsync(pet);
            }

            return leveledUp;
        }

        /// <summary>
        /// 檢查寵物健康狀態並更新健康度
        /// </summary>
        public async Task<int> UpdateHealthStatusAsync(Pet pet)
        {
            var originalHealth = pet.Health;

            // 按照規格：低於 30 的屬性會扣健康度
            if (pet.Hunger < 30) pet.Health = Math.Max(0, pet.Health - 20);
            if (pet.Cleanliness < 30) pet.Health = Math.Max(0, pet.Health - 20);
            if (pet.Stamina < 30) pet.Health = Math.Max(0, pet.Health - 20);

            // 確保健康度不超過 100
            pet.Health = Math.Min(100, pet.Health);

            if (pet.Health != originalHealth)
            {
                await _unitOfWork.PetRepository.UpdateAsync(pet);
                _logger.LogInformation($"寵物 {pet.PetId} 健康度從 {originalHealth} 更新為 {pet.Health}");
            }

            return pet.Health;
        }

        /// <summary>
        /// 檢查寵物是否可以進行冒險
        /// </summary>
        public async Task<bool> CanStartAdventureAsync(int userId)
        {
            var pet = await GetOrCreatePetAsync(userId);
            
            // 按照規格：Health==0 或任一屬性為 0 時禁止冒險
            return pet.Health > 0 && 
                   pet.Hunger > 0 && 
                   pet.Mood > 0 && 
                   pet.Stamina > 0 && 
                   pet.Cleanliness > 0;
        }

        /// <summary>
        /// 執行每日寵物屬性衰減 (每日 00:00 Asia/Taipei 時區執行)
        /// </summary>
        public async Task<int> ExecuteDailyDecayAsync()
        {
            try
            {
                var allPets = await _unitOfWork.PetRepository.GetAllAsync();
                int affectedCount = 0;

                foreach (var pet in allPets)
                {
                    // 執行每日衰減
                    pet.Hunger = Math.Max(0, pet.Hunger - DAILY_HUNGER_DECAY);
                    pet.Mood = Math.Max(0, pet.Mood - DAILY_MOOD_DECAY);
                    pet.Stamina = Math.Max(0, pet.Stamina - DAILY_STAMINA_DECAY);
                    pet.Cleanliness = Math.Max(0, pet.Cleanliness - DAILY_CLEANLINESS_DECAY);
                    pet.Health = Math.Max(0, pet.Health - DAILY_HEALTH_DECAY);

                    await _unitOfWork.PetRepository.UpdateAsync(pet);
                    affectedCount++;
                }

                await _unitOfWork.SaveChangesAsync();

                _logger.LogInformation($"每日寵物屬性衰減完成，影響 {affectedCount} 隻寵物");
                return affectedCount;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "執行每日寵物屬性衰減時發生錯誤");
                throw;
            }
        }

        /// <summary>
        /// 取得寵物互動冷卻時間
        /// </summary>
        public async Task<int> GetInteractionCooldownAsync(int userId, PetInteractionType interactionType)
        {
            var lastInteraction = await _unitOfWork.PetRepository.GetLastInteractionTimeAsync(userId, interactionType);
            
            if (lastInteraction == null) return 0;

            var elapsedSeconds = (DateTime.UtcNow - lastInteraction.Value).TotalSeconds;
            var remainingSeconds = INTERACTION_COOLDOWN_SECONDS - (int)elapsedSeconds;

            return Math.Max(0, remainingSeconds);
        }

        /// <summary>
        /// 取得寵物狀態描述
        /// </summary>
        public PetStatusDescription GetPetStatusDescription(Pet pet)
        {
            // 計算最低屬性
            var attributes = new Dictionary<string, int>
            {
                { "飢餓", pet.Hunger },
                { "心情", pet.Mood },
                { "體力", pet.Stamina },
                { "清潔", pet.Cleanliness },
                { "健康", pet.Health }
            };

            var lowestAttr = attributes.OrderBy(a => a.Value).First();
            var averageStatus = (pet.Hunger + pet.Mood + pet.Stamina + pet.Cleanliness + pet.Health) / 5;

            // 判斷整體狀態
            string overallStatus = averageStatus switch
            {
                >= 80 => "優秀",
                >= 60 => "良好", 
                >= 40 => "普通",
                >= 20 => "不佳",
                _ => "危險"
            };

            // 建議行動
            string suggestedAction = lowestAttr.Key switch
            {
                "飢餓" => "建議餵食",
                "心情" => "建議玩耍",
                "體力" => "建議休息",
                "清潔" => "建議洗澡",
                "健康" => "建議提升其他屬性",
                _ => "繼續照顧"
            };

            // 表情狀態 (用於動畫)
            string emotionState = lowestAttr.Value switch
            {
                <= 20 => "sad",
                <= 40 => "tired",
                <= 60 => "normal",
                <= 80 => "happy",
                _ => "excellent"
            };

            return new PetStatusDescription
            {
                OverallStatus = overallStatus,
                LowestAttribute = lowestAttr.Key,
                LowestValue = lowestAttr.Value,
                SuggestedAction = suggestedAction,
                CanAdventure = pet.Health > 0 && lowestAttr.Value > 0,
                EmotionState = emotionState
            };
        }

        #region 私有輔助方法

        /// <summary>
        /// 取得互動動作名稱
        /// </summary>
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

        /// <summary>
        /// 取得屬性名稱
        /// </summary>
        private string GetAttributeName(PetInteractionType interactionType)
        {
            return interactionType switch
            {
                PetInteractionType.Feed => "飢餓值",
                PetInteractionType.Bath => "清潔值",
                PetInteractionType.Play => "心情值",
                PetInteractionType.Rest => "體力值",
                _ => "屬性值"
            };
        }

        /// <summary>
        /// 驗證顏色格式 (十六進位色碼)
        /// </summary>
        private bool IsValidColorFormat(string color)
        {
            if (string.IsNullOrEmpty(color)) return false;
            
            // 檢查是否為有效的十六進位色碼格式
            return System.Text.RegularExpressions.Regex.IsMatch(color, @"^#[0-9A-Fa-f]{6}$");
        }

        /// <summary>
        /// 計算升級獎勵點數 (可在後台設定)
        /// </summary>
        private int CalculateLevelUpReward(int newLevel)
        {
            // 簡單的升級獎勵公式，可以根據需求調整
            return newLevel switch
            {
                <= 10 => 50,   // 前 10 級每級 50 點
                <= 50 => 100,  // 11-50 級每級 100 點
                <= 100 => 200, // 51-100 級每級 200 點
                _ => 500       // 100 級以上每級 500 點
            };
        }

        #endregion
    }
}