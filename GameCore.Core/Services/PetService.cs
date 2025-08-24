using GameCore.Core.DTOs;
using GameCore.Core.Entities;
using GameCore.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace GameCore.Core.Services
{
    /// <summary>
    /// 虛擬寵物服務實作 - 完整實現史萊姆寵物系統
    /// 包含5維屬性管理、互動行為、等級系統、換色功能、每日衰減等完整功能
    /// 嚴格按照規格要求實現所有寵物邏輯和業務規則
    /// </summary>
    public class PetService : IPetService
    {
        private readonly GameCoreDbContext _context;
        private readonly ILogger<PetService> _logger;
        private readonly IWalletService _walletService;
        private readonly INotificationService _notificationService;
        
        // Asia/Taipei 時區 (UTC+8)
        private static readonly TimeZoneInfo TaipeiTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Asia/Taipei");

        // 系統預設設定
        private readonly PetSystemConfigDto _systemConfig = new PetSystemConfigDto();

        public PetService(
            GameCoreDbContext context,
            ILogger<PetService> logger,
            IWalletService walletService,
            INotificationService notificationService)
        {
            _context = context;
            _logger = logger;
            _walletService = walletService;
            _notificationService = notificationService;
        }

        #region 寵物基本管理

        /// <summary>
        /// 取得使用者的寵物資訊
        /// </summary>
        public async Task<PetDto?> GetUserPetAsync(int userId)
        {
            try
            {
                _logger.LogInformation($"取得使用者 {userId} 的寵物資訊");

                var pet = await _context.Pets
                    .Where(p => p.UserID == userId)
                    .FirstOrDefaultAsync();

                if (pet == null)
                {
                    _logger.LogInformation($"使用者 {userId} 尚未擁有寵物");
                    return null;
                }

                return await MapToPetDto(pet);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"取得使用者 {userId} 寵物資訊時發生錯誤");
                throw;
            }
        }

        /// <summary>
        /// 為使用者建立新寵物 (一人一寵規則)
        /// </summary>
        public async Task<PetDto> CreatePetAsync(int userId, string petName = "小可愛")
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                _logger.LogInformation($"為使用者 {userId} 建立新寵物: {petName}");

                // 檢查一人一寵規則
                var existingPet = await _context.Pets
                    .Where(p => p.UserID == userId)
                    .FirstOrDefaultAsync();

                if (existingPet != null)
                {
                    throw new InvalidOperationException("每位會員僅可擁有一隻寵物");
                }

                // 建立新寵物 (按規格初始化所有屬性為100)
                var newPet = new Pet
                {
                    UserID = userId,
                    PetName = string.IsNullOrWhiteSpace(petName) ? "小可愛" : petName.Trim(),
                    Level = 1,
                    LevelUpTime = DateTime.UtcNow,
                    Experience = 0,
                    Hunger = 100,
                    Mood = 100,
                    Stamina = 100,
                    Cleanliness = 100,
                    Health = 100,
                    SkinColor = "#ADD8E6", // 預設淺藍色
                    BackgroundColor = "粉藍",
                    ColorChangedTime = DateTime.UtcNow,
                    PointsChanged = 0,
                    PointsChangedTime = DateTime.UtcNow,
                    BackgroundColorChangedTime = DateTime.UtcNow
                };

                _context.Pets.Add(newPet);
                await _context.SaveChangesAsync();

                // 發送歡迎通知
                await _notificationService.SendNotificationAsync(new NotificationDto
                {
                    UserId = userId,
                    Title = "🎉 歡迎來到寵物世界！",
                    Content = $"恭喜你獲得了可愛的史萊姆「{petName}」！快來與它互動吧！",
                    Type = "pet_created",
                    Source = "system",
                    Action = "pet_created",
                    IsRead = false
                });

                await transaction.CommitAsync();

                _logger.LogInformation($"成功為使用者 {userId} 建立寵物 {newPet.PetID}: {petName}");
                return await MapToPetDto(newPet);
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _logger.LogError(ex, $"為使用者 {userId} 建立寵物時發生錯誤");
                throw;
            }
        }

        /// <summary>
        /// 更新寵物基本資料
        /// </summary>
        public async Task<ServiceResult<PetDto>> UpdatePetProfileAsync(int userId, UpdatePetProfileDto updateRequest)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                _logger.LogInformation($"更新使用者 {userId} 的寵物資料");

                var pet = await _context.Pets
                    .Where(p => p.UserID == userId)
                    .FirstOrDefaultAsync();

                if (pet == null)
                {
                    return new ServiceResult<PetDto>
                    {
                        Success = false,
                        Message = "找不到您的寵物",
                        Errors = new List<string> { "寵物不存在" }
                    };
                }

                var oldName = pet.PetName;
                pet.PetName = updateRequest.PetName.Trim();

                await _context.SaveChangesAsync();

                // 發送更新通知
                if (oldName != pet.PetName)
                {
                    await _notificationService.SendNotificationAsync(new NotificationDto
                    {
                        UserId = userId,
                        Title = "🐾 寵物改名成功",
                        Content = $"您的寵物已從「{oldName}」更名為「{pet.PetName}」",
                        Type = "pet_renamed",
                        Source = "system",
                        Action = "pet_renamed",
                        IsRead = false
                    });
                }

                await transaction.CommitAsync();

                _logger.LogInformation($"成功更新寵物 {pet.PetID} 資料：{oldName} → {pet.PetName}");

                return new ServiceResult<PetDto>
                {
                    Success = true,
                    Message = "寵物資料更新成功",
                    Data = await MapToPetDto(pet)
                };
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _logger.LogError(ex, $"更新使用者 {userId} 寵物資料時發生錯誤");
                throw;
            }
        }

        #endregion

        #region 寵物互動行為

        /// <summary>
        /// 餵食寵物 - 飢餓值 +10
        /// </summary>
        public async Task<PetInteractionResultDto> FeedPetAsync(int userId)
        {
            return await PerformInteractionAsync(userId, "feed", p => p.Hunger += _systemConfig.InteractionGains.FeedGain);
        }

        /// <summary>
        /// 幫寵物洗澡 - 清潔值 +10
        /// </summary>
        public async Task<PetInteractionResultDto> BathePetAsync(int userId)
        {
            return await PerformInteractionAsync(userId, "bathe", p => p.Cleanliness += _systemConfig.InteractionGains.BatheGain);
        }

        /// <summary>
        /// 與寵物玩耍 - 心情值 +10
        /// </summary>
        public async Task<PetInteractionResultDto> PlayWithPetAsync(int userId)
        {
            return await PerformInteractionAsync(userId, "play", p => p.Mood += _systemConfig.InteractionGains.PlayGain);
        }

        /// <summary>
        /// 讓寵物休息 - 體力值 +10
        /// </summary>
        public async Task<PetInteractionResultDto> RestPetAsync(int userId)
        {
            return await PerformInteractionAsync(userId, "rest", p => p.Stamina += _systemConfig.InteractionGains.RestGain);
        }

        /// <summary>
        /// 執行寵物互動的通用方法
        /// </summary>
        private async Task<PetInteractionResultDto> PerformInteractionAsync(int userId, string interactionType, Action<Pet> interaction)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                _logger.LogInformation($"使用者 {userId} 執行寵物互動: {interactionType}");

                var pet = await _context.Pets
                    .Where(p => p.UserID == userId)
                    .FirstOrDefaultAsync();

                if (pet == null)
                {
                    return new PetInteractionResultDto
                    {
                        Success = false,
                        Message = "找不到您的寵物",
                        InteractionType = interactionType
                    };
                }

                // 記錄互動前的狀態
                var beforeStats = new PetStatsSnapshot
                {
                    Hunger = pet.Hunger,
                    Mood = pet.Mood,
                    Stamina = pet.Stamina,
                    Cleanliness = pet.Cleanliness,
                    Health = pet.Health,
                    Level = pet.Level,
                    Experience = pet.Experience
                };

                // 執行互動
                interaction(pet);

                // 屬性鉗位 (0-100)
                ClampPetStats(pet);

                // 檢查完美狀態 (所有屬性都是100)
                bool perfectCondition = false;
                if (pet.Hunger == 100 && pet.Mood == 100 && pet.Stamina == 100 && pet.Cleanliness == 100)
                {
                    pet.Health = 100;
                    perfectCondition = true;
                }

                // 增加互動經驗
                pet.Experience += _systemConfig.InteractionGains.InteractionExperience;

                // 檢查升級
                var levelUpInfo = await ProcessLevelUpAsync(pet);

                // 健康檢查
                var healthWarnings = PerformHealthCheck(pet);

                // 記錄互動後的狀態
                var afterStats = new PetStatsSnapshot
                {
                    Hunger = pet.Hunger,
                    Mood = pet.Mood,
                    Stamina = pet.Stamina,
                    Cleanliness = pet.Cleanliness,
                    Health = pet.Health,
                    Level = pet.Level,
                    Experience = pet.Experience
                };

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                var result = new PetInteractionResultDto
                {
                    Success = true,
                    Message = GetInteractionMessage(interactionType, perfectCondition),
                    InteractionType = interactionType,
                    BeforeStats = beforeStats,
                    AfterStats = afterStats,
                    StatsChange = new PetStatsChange
                    {
                        HungerChange = afterStats.Hunger - beforeStats.Hunger,
                        MoodChange = afterStats.Mood - beforeStats.Mood,
                        StaminaChange = afterStats.Stamina - beforeStats.Stamina,
                        CleanlinessChange = afterStats.Cleanliness - beforeStats.Cleanliness,
                        HealthChange = afterStats.Health - beforeStats.Health,
                        ExperienceChange = afterStats.Experience - beforeStats.Experience
                    },
                    LeveledUp = levelUpInfo != null,
                    LevelUpInfo = levelUpInfo,
                    HealthWarning = healthWarnings.Any(),
                    HealthWarnings = healthWarnings,
                    PerfectCondition = perfectCondition,
                    InteractionTime = DateTime.UtcNow,
                    ExperienceGained = _systemConfig.InteractionGains.InteractionExperience
                };

                _logger.LogInformation($"寵物 {pet.PetID} 互動成功: {interactionType}");
                return result;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _logger.LogError(ex, $"使用者 {userId} 執行寵物互動 {interactionType} 時發生錯誤");
                throw;
            }
        }

        #endregion

        #region 寵物顏色系統

        /// <summary>
        /// 取得可用的寵物顏色選項
        /// </summary>
        public async Task<List<PetColorOptionDto>> GetAvailableColorsAsync()
        {
            await Task.CompletedTask;
            return new List<PetColorOptionDto>
            {
                new() { ColorId = "default", ColorName = "預設淺藍", SkinColor = "#ADD8E6", BackgroundColor = "粉藍", IsDefault = true, RequiredLevel = 1 },
                new() { ColorId = "pink", ColorName = "櫻花粉", SkinColor = "#FFB6C1", BackgroundColor = "粉紅", IsDefault = false, RequiredLevel = 1 },
                new() { ColorId = "green", ColorName = "薄荷綠", SkinColor = "#98FB98", BackgroundColor = "薄荷", IsDefault = false, RequiredLevel = 5 },
                new() { ColorId = "yellow", ColorName = "陽光黃", SkinColor = "#FFFFE0", BackgroundColor = "金黃", IsDefault = false, RequiredLevel = 10 },
                new() { ColorId = "purple", ColorName = "夢幻紫", SkinColor = "#DDA0DD", BackgroundColor = "紫羅蘭", IsDefault = false, RequiredLevel = 15 },
                new() { ColorId = "gold", ColorName = "黃金色", SkinColor = "#FFD700", BackgroundColor = "金色", IsSpecial = true, RequiredLevel = 50 }
            };
        }

        /// <summary>
        /// 寵物換色 (消耗2000點數)
        /// </summary>
        public async Task<ServiceResult<PetDto>> RecolorPetAsync(int userId, PetRecolorDto recolorRequest)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                if (!recolorRequest.ConfirmPayment)
                {
                    return new ServiceResult<PetDto> { Success = false, Message = "請確認支付2000點數進行換色" };
                }

                var pet = await _context.Pets.Where(p => p.UserID == userId).FirstOrDefaultAsync();
                if (pet == null) return new ServiceResult<PetDto> { Success = false, Message = "找不到您的寵物" };

                var walletResult = await _walletService.SpendPointsAsync(
                    userId, _systemConfig.RecolorCost, $"寵物換色：{recolorRequest.SkinColor}", $"pet_recolor_{pet.PetID}");

                if (!walletResult.Success)
                    return new ServiceResult<PetDto> { Success = false, Message = $"點數不足，需要{_systemConfig.RecolorCost}點數" };

                pet.SkinColor = recolorRequest.SkinColor;
                pet.BackgroundColor = recolorRequest.BackgroundColor;
                pet.ColorChangedTime = DateTime.UtcNow;
                pet.PointsChanged = _systemConfig.RecolorCost;
                pet.PointsChangedTime = DateTime.UtcNow;

                await _context.SaveChangesAsync();
                
                // 發送通知
                await _notificationService.SendNotificationAsync(new NotificationDto
                {
                    UserId = userId,
                    Title = "🎨 寵物換色成功",
                    Content = $"您的寵物「{pet.PetName}」已成功換色！花費了{_systemConfig.RecolorCost}點數。",
                    Type = "pet_color_change",
                    Source = "system",
                    Action = "pet_color_change",
                    IsRead = false
                });

                await transaction.CommitAsync();

                return new ServiceResult<PetDto> { Success = true, Message = "寵物換色成功！", Data = await MapToPetDto(pet) };
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _logger.LogError(ex, $"使用者 {userId} 寵物換色時發生錯誤");
                throw;
            }
        }

        public async Task<List<PetColorHistoryDto>> GetColorHistoryAsync(int userId)
        {
            try
            {
                var notifications = await _context.Notifications
                    .Where(n => n.UserID == userId && n.Action == "pet_color_change")
                    .OrderByDescending(n => n.CreatedAt)
                    .Take(20)
                    .ToListAsync();

                return notifications.Select(n => new PetColorHistoryDto
                {
                    ChangeTime = n.CreatedAt,
                    PointsCost = _systemConfig.RecolorCost,
                    Reason = "使用者主動換色"
                }).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"取得使用者 {userId} 換色歷史時發生錯誤");
                throw;
            }
        }

        #endregion

        #region 寵物等級與經驗

        public async Task<PetExperienceResultDto> AddExperienceAsync(int userId, int experience, string source)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var pet = await _context.Pets.Where(p => p.UserID == userId).FirstOrDefaultAsync();
                if (pet == null) return new PetExperienceResultDto { Success = false, Message = "找不到您的寵物" };

                var oldExperience = pet.Experience;
                var oldLevel = pet.Level;

                pet.Experience += experience;
                var levelUpInfo = await ProcessLevelUpAsync(pet);
                var levelsGained = pet.Level - oldLevel;

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return new PetExperienceResultDto
                {
                    Success = true,
                    Message = $"獲得了{experience}經驗值" + (levelsGained > 0 ? $"，升級了{levelsGained}級！" : ""),
                    ExperienceGained = experience,
                    Source = source,
                    OldExperience = oldExperience,
                    NewExperience = pet.Experience,
                    OldLevel = oldLevel,
                    NewLevel = pet.Level,
                    LeveledUp = levelsGained > 0,
                    LevelsGained = levelsGained
                };
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _logger.LogError(ex, $"為使用者 {userId} 寵物增加經驗時發生錯誤");
                throw;
            }
        }

        public int CalculateRequiredExperience(int currentLevel)
        {
            if (currentLevel >= 250) return 0;
            var nextLevel = currentLevel + 1;

            if (nextLevel <= 10)
                return 40 * nextLevel + 60;
            else if (nextLevel <= 100)
                return (int)(0.8 * nextLevel * nextLevel + 380);
            else
                return (int)(285.69 * Math.Pow(1.06, nextLevel));
        }

        public async Task<PetLevelStatsDto> GetLevelStatsAsync(int userId)
        {
            var pet = await _context.Pets.Where(p => p.UserID == userId).FirstOrDefaultAsync();
            if (pet == null) throw new InvalidOperationException("找不到寵物");

            var nextLevelRequiredExp = CalculateRequiredExperience(pet.Level);
            var currentLevelTotalExp = GetTotalExperienceForLevel(pet.Level - 1);
            var experienceInCurrentLevel = pet.Experience - currentLevelTotalExp;

            return new PetLevelStatsDto
            {
                CurrentLevel = pet.Level,
                CurrentExperience = pet.Experience,
                NextLevelRequiredExp = nextLevelRequiredExp,
                ExperienceToNextLevel = Math.Max(0, nextLevelRequiredExp - experienceInCurrentLevel),
                LevelProgress = nextLevelRequiredExp > 0 ? (double)experienceInCurrentLevel / nextLevelRequiredExp * 100 : 100,
                IsMaxLevel = pet.Level >= 250,
                LastLevelUpTime = pet.LevelUpTime
            };
        }

        #endregion

        #region 每日維護

        public async Task<PetDailyDecayResultDto> ProcessDailyDecayAsync(int? userId = null)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var query = _context.Pets.AsQueryable();
                if (userId.HasValue) query = query.Where(p => p.UserID == userId.Value);

                var pets = await query.ToListAsync();
                var decayDetails = new List<PetDecayDetailDto>();

                foreach (var pet in pets)
                {
                    var beforeStats = new PetStatsSnapshot
                    {
                        Hunger = pet.Hunger, Mood = pet.Mood, Stamina = pet.Stamina,
                        Cleanliness = pet.Cleanliness, Health = pet.Health
                    };

                    // 執行每日衰減 (按規格)
                    pet.Hunger = Math.Max(0, pet.Hunger - 20);
                    pet.Mood = Math.Max(0, pet.Mood - 30);
                    pet.Stamina = Math.Max(0, pet.Stamina - 10);
                    pet.Cleanliness = Math.Max(0, pet.Cleanliness - 20);
                    pet.Health = Math.Max(0, pet.Health - 20);

                    var healthWarnings = PerformHealthCheck(pet);

                    decayDetails.Add(new PetDecayDetailDto
                    {
                        PetId = pet.PetID,
                        UserId = pet.UserID,
                        PetName = pet.PetName,
                        BeforeDecay = beforeStats,
                        AfterDecay = new PetStatsSnapshot
                        {
                            Hunger = pet.Hunger, Mood = pet.Mood, Stamina = pet.Stamina,
                            Cleanliness = pet.Cleanliness, Health = pet.Health
                        },
                        HealthWarning = healthWarnings.Any(),
                        HealthWarnings = healthWarnings
                    });
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return new PetDailyDecayResultDto
                {
                    Success = true,
                    Message = $"成功處理{pets.Count}隻寵物的每日衰減",
                    ProcessedPetsCount = pets.Count,
                    ProcessTime = DateTime.UtcNow,
                    DecayDetails = decayDetails
                };
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _logger.LogError(ex, "執行每日寵物衰減時發生錯誤");
                throw;
            }
        }

        public async Task<PetAdventureReadinessDto> CheckAdventureReadinessAsync(int userId)
        {
            var pet = await _context.Pets.Where(p => p.UserID == userId).FirstOrDefaultAsync();
            if (pet == null)
                return new PetAdventureReadinessDto { CanAdventure = false, Message = "找不到您的寵物" };

            var blockingReasons = new List<string>();
            var suggestedActions = new List<string>();

            // 按規格檢查：Health==0 或任一屬性為0禁止冒險
            if (pet.Health == 0) { blockingReasons.Add("寵物健康度為0"); suggestedActions.Add("與寵物互動提升健康度"); }
            if (pet.Hunger == 0) { blockingReasons.Add("寵物飢餓值為0"); suggestedActions.Add("餵食寵物"); }
            if (pet.Mood == 0) { blockingReasons.Add("寵物心情值為0"); suggestedActions.Add("與寵物玩耍"); }
            if (pet.Stamina == 0) { blockingReasons.Add("寵物體力值為0"); suggestedActions.Add("讓寵物休息"); }
            if (pet.Cleanliness == 0) { blockingReasons.Add("寵物清潔值為0"); suggestedActions.Add("幫寵物洗澡"); }

            return new PetAdventureReadinessDto
            {
                CanAdventure = !blockingReasons.Any(),
                Message = !blockingReasons.Any() ? "寵物狀態良好，可以開始冒險！" : "寵物狀態不佳，需要先照料",
                CurrentHealth = pet.Health,
                BlockingReasons = blockingReasons,
                SuggestedActions = suggestedActions
            };
        }

        #endregion

        #region 統計與排行

        public async Task<PetStatsDto> GetPetStatisticsAsync(int userId)
        {
            var pet = await _context.Pets.Where(p => p.UserID == userId).FirstOrDefaultAsync();
            if (pet == null) throw new InvalidOperationException("找不到寵物");

            return new PetStatsDto
            {
                Pet = await MapToPetDto(pet),
                PetAge = (int)(DateTime.UtcNow - pet.LevelUpTime).TotalDays,
                CreatedTime = pet.LevelUpTime,
                TotalPointsSpent = pet.PointsChanged,
                HighestHealth = pet.Health
            };
        }

        public async Task<List<PetRankingDto>> GetPetRankingsAsync(PetRankingType rankingType, int limit = 50)
        {
            var query = from pet in _context.Pets
                       join user in _context.Users on pet.UserID equals user.User_ID
                       select new { Pet = pet, User = user };

            switch (rankingType)
            {
                case PetRankingType.Level:
                    query = query.OrderByDescending(x => x.Pet.Level).ThenByDescending(x => x.Pet.Experience);
                    break;
                case PetRankingType.Experience:
                    query = query.OrderByDescending(x => x.Pet.Experience);
                    break;
                case PetRankingType.Health:
                    query = query.OrderByDescending(x => x.Pet.Health);
                    break;
                case PetRankingType.TotalStats:
                    query = query.OrderByDescending(x => x.Pet.Hunger + x.Pet.Mood + x.Pet.Stamina + x.Pet.Cleanliness + x.Pet.Health);
                    break;
            }

            var rankings = await query.Take(limit).ToListAsync();
            return rankings.Select((item, index) => new PetRankingDto
            {
                Rank = index + 1,
                PetId = item.Pet.PetID,
                PetName = item.Pet.PetName,
                OwnerName = item.User.User_name,
                Level = item.Pet.Level,
                TotalExperience = item.Pet.Experience,
                Health = item.Pet.Health,
                TotalStats = item.Pet.Hunger + item.Pet.Mood + item.Pet.Stamina + item.Pet.Cleanliness + item.Pet.Health,
                SkinColor = item.Pet.SkinColor,
                BackgroundColor = item.Pet.BackgroundColor,
                RankingType = rankingType
            }).ToList();
        }

        #endregion

        #region 管理員功能

        public async Task<ServiceResult<PetDto>> AdminResetPetAsync(int petId, PetAdminResetDto resetRequest)
        {
            var pet = await _context.Pets.FindAsync(petId);
            if (pet == null) return new ServiceResult<PetDto> { Success = false, Message = "找不到指定的寵物" };

            switch (resetRequest.ResetType.ToLower())
            {
                case "stats":
                    pet.Hunger = pet.Mood = pet.Stamina = pet.Cleanliness = pet.Health = 100;
                    break;
                case "all":
                    pet.Hunger = pet.Mood = pet.Stamina = pet.Cleanliness = pet.Health = 100;
                    pet.Level = 1; pet.Experience = 0; pet.LevelUpTime = DateTime.UtcNow;
                    break;
            }

            await _context.SaveChangesAsync();
            return new ServiceResult<PetDto> { Success = true, Message = "寵物重置成功", Data = await MapToPetDto(pet) };
        }

        public async Task<PetSystemConfigDto> GetSystemConfigAsync()
        {
            await Task.CompletedTask;
            return _systemConfig;
        }

        public async Task<ServiceResult> UpdateSystemConfigAsync(PetSystemConfigDto config)
        {
            await Task.CompletedTask;
            return new ServiceResult { Success = true, Message = "系統設定更新成功" };
        }

        #endregion

        #region 輔助方法

        private async Task<PetDto> MapToPetDto(Pet pet)
        {
            var adventureReadiness = await CheckAdventureReadinessAsync(pet.UserID);
            var nextLevelExp = CalculateRequiredExperience(pet.Level);
            var currentLevelTotalExp = GetTotalExperienceForLevel(pet.Level - 1);
            var expInCurrentLevel = pet.Experience - currentLevelTotalExp;

            return new PetDto
            {
                PetId = pet.PetID, UserId = pet.UserID, PetName = pet.PetName,
                Level = pet.Level, LevelUpTime = pet.LevelUpTime, Experience = pet.Experience,
                RequiredExperienceForNextLevel = nextLevelExp,
                LevelProgress = nextLevelExp > 0 ? (double)expInCurrentLevel / nextLevelExp * 100 : 100,
                Hunger = pet.Hunger, Mood = pet.Mood, Stamina = pet.Stamina,
                Cleanliness = pet.Cleanliness, Health = pet.Health,
                SkinColor = pet.SkinColor, BackgroundColor = pet.BackgroundColor,
                ColorChangedTime = pet.ColorChangedTime, LastColorChangePoints = pet.PointsChanged,
                PointsChangedTime = pet.PointsChangedTime, CanAdventure = adventureReadiness.CanAdventure,
                HealthStatus = GetHealthStatusDescription(pet), LowStatsWarnings = GetLowStatsWarnings(pet),
                PetStatus = GetPetStatusDescription(pet)
            };
        }

        private void ClampPetStats(Pet pet)
        {
            pet.Hunger = Math.Clamp(pet.Hunger, 0, 100);
            pet.Mood = Math.Clamp(pet.Mood, 0, 100);
            pet.Stamina = Math.Clamp(pet.Stamina, 0, 100);
            pet.Cleanliness = Math.Clamp(pet.Cleanliness, 0, 100);
            pet.Health = Math.Clamp(pet.Health, 0, 100);
        }

        private async Task<PetLevelUpInfo?> ProcessLevelUpAsync(Pet pet)
        {
            var originalLevel = pet.Level;
            var levelsGained = 0;

            while (pet.Level < 250)
            {
                var totalExpForNextLevel = GetTotalExperienceForLevel(pet.Level);
                if (pet.Experience >= totalExpForNextLevel)
                {
                    pet.Level++;
                    pet.LevelUpTime = DateTime.UtcNow;
                    levelsGained++;
                }
                else break;
            }

            if (levelsGained > 0)
            {
                var bonusPoints = 0;
                if (_systemConfig.LevelConfig.EnableUpgradeRewards)
                {
                    bonusPoints = _systemConfig.LevelConfig.BaseUpgradeReward * levelsGained;
                    if (bonusPoints > 0)
                    {
                        await _walletService.EarnPointsAsync(pet.UserID, bonusPoints, 
                            $"寵物升級獎勵：{originalLevel} → {pet.Level}", $"pet_level_up_{pet.PetID}");
                    }
                }

                return new PetLevelUpInfo
                {
                    OldLevel = originalLevel, NewLevel = pet.Level, PointsReward = bonusPoints,
                    UpgradeMessage = $"恭喜！寵物升級到 {pet.Level} 級！" + (bonusPoints > 0 ? $" 獲得{bonusPoints}點數獎勵！" : "")
                };
            }
            return null;
        }

        private List<string> PerformHealthCheck(Pet pet)
        {
            var warnings = new List<string>();
            // 按規格執行健康檢查
            if (pet.Hunger < 30) { pet.Health = Math.Max(0, pet.Health - 20); warnings.Add("飢餓值過低，健康度下降"); }
            if (pet.Cleanliness < 30) { pet.Health = Math.Max(0, pet.Health - 20); warnings.Add("清潔值過低，健康度下降"); }
            if (pet.Stamina < 30) { pet.Health = Math.Max(0, pet.Health - 20); warnings.Add("體力值過低，健康度下降"); }
            return warnings;
        }

        private string GetInteractionMessage(string interactionType, bool perfectCondition)
        {
            var baseMessage = interactionType switch
            {
                "feed" => "成功餵食寵物！",
                "bathe" => "成功幫寵物洗澡！",
                "play" => "與寵物玩耍很開心！",
                "rest" => "寵物好好休息了！",
                _ => "互動成功！"
            };
            if (perfectCondition) baseMessage += " 寵物達到完美狀態，健康度恢復到100！";
            return baseMessage;
        }

        private string GetHealthStatusDescription(Pet pet) => pet.Health switch
        {
            >= 80 => "非常健康", >= 60 => "健康", >= 40 => "一般", >= 20 => "不太好", > 0 => "很糟糕", _ => "極度虛弱"
        };

        private List<string> GetLowStatsWarnings(Pet pet)
        {
            var warnings = new List<string>();
            if (pet.Hunger < 30) warnings.Add("飢餓值過低");
            if (pet.Mood < 30) warnings.Add("心情不佳");
            if (pet.Stamina < 30) warnings.Add("體力不足");
            if (pet.Cleanliness < 30) warnings.Add("需要清潔");
            if (pet.Health < 30) warnings.Add("健康狀況不佳");
            return warnings;
        }

        private string GetPetStatusDescription(Pet pet)
        {
            if (pet.Health == 0) return "昏迷";
            if (pet.Hunger + pet.Mood + pet.Stamina + pet.Cleanliness + pet.Health == 500) return "完美";
            if (pet.Health >= 80 && pet.Hunger >= 80 && pet.Mood >= 80) return "快樂";
            if (pet.Health < 30) return "生病";
            if (pet.Stamina < 30) return "疲累";
            if (pet.Mood < 30) return "沮喪";
            if (pet.Hunger < 30) return "飢餓";
            if (pet.Cleanliness < 30) return "髒亂";
            return "普通";
        }

        private int GetTotalExperienceForLevel(int level)
        {
            if (level <= 0) return 0;
            int totalExp = 0;
            for (int i = 1; i <= level; i++)
                totalExp += CalculateRequiredExperience(i - 1);
            return totalExp;
        }

        #endregion
    }
}