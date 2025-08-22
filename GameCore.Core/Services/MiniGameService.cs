using GameCore.Core.Entities;
using GameCore.Core.Interfaces;

namespace GameCore.Core.Services
{
    /// <summary>
    /// 小冒險遊戲服務實作
    /// </summary>
    public class MiniGameService : IMiniGameService
    {
        private readonly IMiniGameRepository _miniGameRepository;
        private readonly IPetRepository _petRepository;
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly Random _random;

        public MiniGameService(
            IMiniGameRepository miniGameRepository,
            IPetRepository petRepository,
            IUserRepository userRepository,
            IUnitOfWork unitOfWork)
        {
            _miniGameRepository = miniGameRepository;
            _petRepository = petRepository;
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
            _random = new Random();
        }

        /// <summary>
        /// 開始小冒險遊戲
        /// </summary>
        public async Task<MiniGameResult> StartGameAsync(int userId, int petId, int gameLevel)
        {
            try
            {
                // 檢查遊戲資格
                var eligibility = await CheckGameEligibilityAsync(userId, petId);
                if (!eligibility.CanPlay)
                {
                    return new MiniGameResult
                    {
                        IsSuccess = false,
                        Message = eligibility.Message
                    };
                }

                // 取得遊戲設定
                var gameSettings = await GetGameSettingsAsync(gameLevel);
                if (gameSettings == null)
                {
                    return new MiniGameResult
                    {
                        IsSuccess = false,
                        Message = "無效的遊戲等級"
                    };
                }

                // 取得寵物資料
                var pet = await _petRepository.GetByIdAsync(petId);
                if (pet == null || pet.UserId != userId)
                {
                    return new MiniGameResult
                    {
                        IsSuccess = false,
                        Message = "寵物不存在或無權限"
                    };
                }

                // 計算遊戲結果
                var gameResult = CalculateGameResult(gameSettings, pet);
                
                // 建立遊戲記錄
                var gameRecord = new MiniGameRecord
                {
                    UserId = userId,
                    PetId = petId,
                    GameLevel = gameLevel,
                    IsSuccess = gameResult.IsSuccess,
                    PointsEarned = gameResult.PointsEarned,
                    ExperienceEarned = gameResult.ExperienceEarned,
                    HealthChange = gameResult.AttributeChanges.HealthChange,
                    HungerChange = gameResult.AttributeChanges.HungerChange,
                    CleanlinessChange = gameResult.AttributeChanges.CleanlinessChange,
                    HappinessChange = gameResult.AttributeChanges.HappinessChange,
                    EnergyChange = gameResult.AttributeChanges.EnergyChange,
                    GameDate = DateTime.UtcNow,
                    CreatedAt = DateTime.UtcNow
                };

                // 更新寵物屬性
                pet.Health = Math.Max(0, Math.Min(100, pet.Health + gameResult.AttributeChanges.HealthChange));
                pet.Hunger = Math.Max(0, Math.Min(100, pet.Hunger + gameResult.AttributeChanges.HungerChange));
                pet.Cleanliness = Math.Max(0, Math.Min(100, pet.Cleanliness + gameResult.AttributeChanges.CleanlinessChange));
                pet.Happiness = Math.Max(0, Math.Min(100, pet.Happiness + gameResult.AttributeChanges.HappinessChange));
                pet.Energy = Math.Max(0, Math.Min(100, pet.Energy + gameResult.AttributeChanges.EnergyChange));

                // 更新使用者點數和經驗
                var user = await _userRepository.GetByIdAsync(userId);
                if (user != null)
                {
                    user.Points += gameResult.PointsEarned;
                    user.Experience += gameResult.ExperienceEarned;
                    _userRepository.Update(user);
                }

                // 儲存變更
                _miniGameRepository.Add(gameRecord);
                _petRepository.Update(pet);
                await _unitOfWork.SaveChangesAsync();

                return gameResult;
            }
            catch (Exception ex)
            {
                return new MiniGameResult
                {
                    IsSuccess = false,
                    Message = $"遊戲執行失敗: {ex.Message}"
                };
            }
        }

        /// <summary>
        /// 檢查使用者是否可以進行遊戲
        /// </summary>
        public async Task<GameCheckResult> CheckGameEligibilityAsync(int userId, int petId)
        {
            try
            {
                // 檢查寵物是否存在
                var pet = await _petRepository.GetByIdAsync(petId);
                if (pet == null || pet.UserId != userId)
                {
                    return new GameCheckResult
                    {
                        CanPlay = false,
                        Message = "寵物不存在或無權限"
                    };
                }

                // 檢查寵物健康度
                if (pet.Health <= 0)
                {
                    return new GameCheckResult
                    {
                        CanPlay = false,
                        Message = "寵物健康度不足，無法進行遊戲"
                    };
                }

                // 檢查寵物屬性
                if (pet.Hunger <= 0 || pet.Cleanliness <= 0 || pet.Happiness <= 0 || pet.Energy <= 0)
                {
                    return new GameCheckResult
                    {
                        CanPlay = false,
                        Message = "寵物屬性不足，無法進行遊戲"
                    };
                }

                // 檢查每日遊戲次數限制
                var today = DateTime.UtcNow.Date;
                var hasReachedLimit = await _miniGameRepository.HasReachedDailyLimitAsync(userId, today);
                if (hasReachedLimit)
                {
                    return new GameCheckResult
                    {
                        CanPlay = false,
                        Message = "今日遊戲次數已達上限"
                    };
                }

                var todayCount = await _miniGameRepository.GetTodayGameCountAsync(userId, today);
                var remainingGames = 3 - todayCount;

                return new GameCheckResult
                {
                    CanPlay = true,
                    Message = "可以進行遊戲",
                    RemainingGames = remainingGames,
                    DailyLimit = 3
                };
            }
            catch (Exception ex)
            {
                return new GameCheckResult
                {
                    CanPlay = false,
                    Message = $"檢查失敗: {ex.Message}"
                };
            }
        }

        /// <summary>
        /// 取得遊戲設定
        /// </summary>
        public async Task<MiniGameSettings?> GetGameSettingsAsync(int gameLevel)
        {
            return await _miniGameRepository.GetGameSettingsAsync(gameLevel);
        }

        /// <summary>
        /// 取得使用者的遊戲記錄
        /// </summary>
        public async Task<IEnumerable<MiniGameRecord>> GetUserGameRecordsAsync(int userId, int page, int pageSize)
        {
            return await _miniGameRepository.GetUserGameRecordsAsync(userId, page, pageSize);
        }

        /// <summary>
        /// 取得寵物的遊戲記錄
        /// </summary>
        public async Task<IEnumerable<MiniGameRecord>> GetPetGameRecordsAsync(int petId, int page, int pageSize)
        {
            return await _miniGameRepository.GetPetGameRecordsAsync(petId, page, pageSize);
        }

        /// <summary>
        /// 計算遊戲結果
        /// </summary>
        private MiniGameResult CalculateGameResult(MiniGameSettings settings, Pet pet)
        {
            var result = new MiniGameResult();
            var randomValue = _random.Next(1, 101);

            // 根據成功率決定是否成功
            result.IsSuccess = randomValue <= settings.SuccessRate;

            if (result.IsSuccess)
            {
                // 成功時的獎勵
                result.PointsEarned = settings.BasePointsReward + _random.Next(0, 21); // 額外 0-20 點
                result.ExperienceEarned = settings.BaseExperienceReward + _random.Next(0, 51); // 額外 0-50 經驗

                // 屬性變化 (正面)
                result.AttributeChanges.HealthChange = _random.Next(0, 6); // 0-5
                result.AttributeChanges.HungerChange = _random.Next(-3, 1); // -3 到 0
                result.AttributeChanges.CleanlinessChange = _random.Next(-2, 1); // -2 到 0
                result.AttributeChanges.HappinessChange = _random.Next(2, 8); // 2-7
                result.AttributeChanges.EnergyChange = _random.Next(-5, 1); // -5 到 0

                result.Message = "遊戲成功！寵物獲得了獎勵！";
            }
            else
            {
                // 失敗時的獎勵 (較少)
                result.PointsEarned = settings.BasePointsReward / 2;
                result.ExperienceEarned = settings.BaseExperienceReward / 2;

                // 屬性變化 (負面較多)
                result.AttributeChanges.HealthChange = _random.Next(-3, 2); // -3 到 1
                result.AttributeChanges.HungerChange = _random.Next(-5, 1); // -5 到 0
                result.AttributeChanges.CleanlinessChange = _random.Next(-4, 1); // -4 到 0
                result.AttributeChanges.HappinessChange = _random.Next(-2, 3); // -2 到 2
                result.AttributeChanges.EnergyChange = _random.Next(-8, 1); // -8 到 0

                result.Message = "遊戲失敗，但寵物還是學到了一些經驗。";
            }

            return result;
        }
    }
}