using GameCore.Core.DTOs;
using GameCore.Core.Entities;
using GameCore.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace GameCore.Core.Services
{
    /// <summary>
    /// 錢包服務實作 - 處理會員點數、交易流水和銷售錢包相關功能
    /// 按照規格要求，從多個來源彙整點數異動記錄，提供完整的錢包管理功能
    /// </summary>
    public class WalletService : IWalletService
    {
        private readonly GameCoreDbContext _context;
        private readonly ILogger<WalletService> _logger;
        private readonly INotificationService _notificationService;

        public WalletService(
            GameCoreDbContext context,
            ILogger<WalletService> logger,
            INotificationService notificationService)
        {
            _context = context;
            _logger = logger;
            _notificationService = notificationService;
        }

        #region 基本錢包功能 (Basic Wallet Functions)

        /// <summary>
        /// 取得使用者錢包資訊
        /// </summary>
        public async Task<WalletDto> GetWalletAsync(int userId)
        {
            try
            {
                _logger.LogInformation($"取得使用者 {userId} 的錢包資訊");

                // 查詢錢包和權限資訊
                var wallet = await _context.UserWallets
                    .Where(w => w.UserId == userId)
                    .FirstOrDefaultAsync();

                var userRights = await _context.UserRights
                    .Where(r => r.UserId == userId)
                    .FirstOrDefaultAsync();

                var salesWallet = await _context.UserSalesInformations
                    .Where(s => s.UserId == userId)
                    .FirstOrDefaultAsync();

                if (wallet == null)
                {
                    _logger.LogWarning($"使用者 {userId} 的錢包不存在，建立新錢包");
                    
                    // 建立新錢包
                    wallet = new UserWallet
                    {
                        UserId = userId,
                        UserPoint = 0,
                        CouponNumber = null
                    };
                    _context.UserWallets.Add(wallet);
                    await _context.SaveChangesAsync();
                }

                return new WalletDto
                {
                    UserId = userId,
                    CurrentPoints = wallet.UserPoint,
                    CouponNumber = wallet.CouponNumber,
                    LastUpdated = DateTime.UtcNow,
                    HasSalesAuthority = userRights?.SalesAuthority == true,
                    SalesWalletBalance = salesWallet?.UserSalesWallet
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"取得使用者 {userId} 錢包資訊時發生錯誤");
                throw;
            }
        }

        /// <summary>
        /// 管理員調整使用者點數
        /// </summary>
        public async Task<WalletDto> AdjustPointsAsync(int userId, int pointsDelta, string reason, int adminId)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                _logger.LogInformation($"管理員 {adminId} 調整使用者 {userId} 點數: {pointsDelta}, 原因: {reason}");

                var wallet = await _context.UserWallets
                    .Where(w => w.UserId == userId)
                    .FirstOrDefaultAsync();

                if (wallet == null)
                {
                    throw new InvalidOperationException($"使用者 {userId} 的錢包不存在");
                }

                var oldBalance = wallet.UserPoint;
                wallet.UserPoint = Math.Max(0, wallet.UserPoint + pointsDelta);
                var actualDelta = wallet.UserPoint - oldBalance;

                await _context.SaveChangesAsync();

                // 建立通知記錄作為稽核證明
                await _notificationService.CreateSystemNotificationAsync(
                    userId: userId,
                    action: "points_adjustment",
                    title: "點數調整通知",
                    message: $"您的點數已由管理員調整 {actualDelta} 點。原因: {reason}",
                    metadata: JsonSerializer.Serialize(new
                    {
                        adminId = adminId,
                        reason = reason,
                        oldBalance = oldBalance,
                        newBalance = wallet.UserPoint,
                        delta = actualDelta,
                        adjustmentTime = DateTime.UtcNow
                    })
                );

                await transaction.CommitAsync();

                _logger.LogInformation($"成功調整使用者 {userId} 點數，從 {oldBalance} 調整至 {wallet.UserPoint}");

                return await GetWalletAsync(userId);
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _logger.LogError(ex, $"調整使用者 {userId} 點數時發生錯誤");
                throw;
            }
        }

        /// <summary>
        /// 檢查使用者是否有足夠點數
        /// </summary>
        public async Task<bool> HasSufficientPointsAsync(int userId, int requiredPoints)
        {
            try
            {
                var wallet = await _context.UserWallets
                    .Where(w => w.UserId == userId)
                    .FirstOrDefaultAsync();

                return wallet != null && wallet.UserPoint >= requiredPoints;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"檢查使用者 {userId} 點數餘額時發生錯誤");
                return false;
            }
        }

        #endregion

        #region 收支明細彙整 (Transaction Ledger Aggregation)

        /// <summary>
        /// 取得使用者點數收支明細彙整
        /// 依據規格從多個來源彙整: 簽到(UserSignInStats)、小遊戲(MiniGame)、寵物換色(Pet)、管理調整(Notifications)
        /// </summary>
        public async Task<PagedResult<LedgerEntryDto>> GetLedgerHistoryAsync(int userId, LedgerQueryDto request)
        {
            try
            {
                _logger.LogInformation($"查詢使用者 {userId} 的點數明細，類型: {request.TransactionType}");

                var allEntries = new List<LedgerEntryDto>();

                // 1. 簽到記錄 (UserSignInStats)
                if (string.IsNullOrEmpty(request.TransactionType) || request.TransactionType == "signin")
                {
                    var signInEntries = await GetSignInLedgerEntriesAsync(userId, request.FromDate, request.ToDate);
                    allEntries.AddRange(signInEntries);
                }

                // 2. 小遊戲記錄 (MiniGame)
                if (string.IsNullOrEmpty(request.TransactionType) || request.TransactionType == "minigame")
                {
                    var gameEntries = await GetMiniGameLedgerEntriesAsync(userId, request.FromDate, request.ToDate);
                    allEntries.AddRange(gameEntries);
                }

                // 3. 寵物換色記錄 (Pet) - 僅最近一次
                if (string.IsNullOrEmpty(request.TransactionType) || request.TransactionType == "pet_color")
                {
                    var petEntries = await GetPetColorLedgerEntriesAsync(userId, request.FromDate, request.ToDate);
                    allEntries.AddRange(petEntries);
                }

                // 4. 管理調整記錄 (Notifications)
                if (string.IsNullOrEmpty(request.TransactionType) || request.TransactionType == "adjustment")
                {
                    var adjustmentEntries = await GetAdjustmentLedgerEntriesAsync(userId, request.FromDate, request.ToDate);
                    allEntries.AddRange(adjustmentEntries);
                }

                // 排序和分頁
                var sortedEntries = request.SortBy.ToLower() switch
                {
                    "date_asc" => allEntries.OrderBy(e => e.Timestamp),
                    "amount_desc" => allEntries.OrderByDescending(e => e.PointsDelta),
                    "amount_asc" => allEntries.OrderBy(e => e.PointsDelta),
                    _ => allEntries.OrderByDescending(e => e.Timestamp)
                };

                var totalCount = sortedEntries.Count();
                var pagedEntries = sortedEntries
                    .Skip((request.Page - 1) * request.PageSize)
                    .Take(request.PageSize)
                    .ToList();

                return new PagedResult<LedgerEntryDto>
                {
                    Items = pagedEntries,
                    TotalCount = totalCount,
                    CurrentPage = request.Page,
                    PageSize = request.PageSize
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"查詢使用者 {userId} 收支明細時發生錯誤");
                throw;
            }
        }

        /// <summary>
        /// 取得簽到記錄的明細項目
        /// </summary>
        private async Task<List<LedgerEntryDto>> GetSignInLedgerEntriesAsync(int userId, DateTime? fromDate, DateTime? toDate)
        {
            var query = _context.UserSignInStats.Where(s => s.UserID == userId && s.PointsChanged != 0);

            if (fromDate.HasValue)
                query = query.Where(s => s.SignTime >= fromDate.Value);
            if (toDate.HasValue)
                query = query.Where(s => s.SignTime <= toDate.Value);

            var signInRecords = await query.ToListAsync();

            return signInRecords.Select(s => new LedgerEntryDto
            {
                Id = $"signin_{s.LogID}",
                Timestamp = s.SignTime,
                Type = "signin",
                PointsDelta = s.PointsChanged,
                Description = $"每日簽到獲得 {s.PointsChanged} 點數",
                SourceRecordId = s.LogID.ToString(),
                Metadata = JsonSerializer.Serialize(new
                {
                    signInDate = s.SignTime.ToString("yyyy-MM-dd"),
                    experienceGained = s.ExpGained,
                    pointsChangedTime = s.PointsChangedTime
                })
            }).ToList();
        }

        /// <summary>
        /// 取得小遊戲記錄的明細項目
        /// </summary>
        private async Task<List<LedgerEntryDto>> GetMiniGameLedgerEntriesAsync(int userId, DateTime? fromDate, DateTime? toDate)
        {
            var query = _context.MiniGames.Where(m => m.UserID == userId && m.PointsChanged != 0);

            if (fromDate.HasValue)
                query = query.Where(m => m.StartTime >= fromDate.Value);
            if (toDate.HasValue)
                query = query.Where(m => m.StartTime <= toDate.Value);

            var gameRecords = await query.ToListAsync();

            return gameRecords.Select(m => new LedgerEntryDto
            {
                Id = $"minigame_{m.PlayID}",
                Timestamp = m.StartTime,
                Type = "minigame",
                PointsDelta = m.PointsChanged,
                Description = m.Result == "Win" 
                    ? $"冒險勝利獲得 {m.PointsChanged} 點數"
                    : $"冒險失敗失去 {Math.Abs(m.PointsChanged)} 點數",
                SourceRecordId = m.PlayID.ToString(),
                Metadata = JsonSerializer.Serialize(new
                {
                    gameLevel = m.Level,
                    result = m.Result,
                    monsterCount = m.MonsterCount,
                    experienceGained = m.ExpGained,
                    aborted = m.Aborted
                })
            }).ToList();
        }

        /// <summary>
        /// 取得寵物換色記錄的明細項目 (僅最近一次)
        /// </summary>
        private async Task<List<LedgerEntryDto>> GetPetColorLedgerEntriesAsync(int userId, DateTime? fromDate, DateTime? toDate)
        {
            var pet = await _context.Pets
                .Where(p => p.UserID == userId && p.PointsChanged != 0)
                .FirstOrDefaultAsync();

            if (pet == null || !pet.PointsChangedTime.HasValue)
                return new List<LedgerEntryDto>();

            var changeTime = pet.PointsChangedTime.Value;

            // 檢查時間範圍
            if (fromDate.HasValue && changeTime < fromDate.Value)
                return new List<LedgerEntryDto>();
            if (toDate.HasValue && changeTime > toDate.Value)
                return new List<LedgerEntryDto>();

            return new List<LedgerEntryDto>
            {
                new LedgerEntryDto
                {
                    Id = $"pet_color_{pet.PetID}",
                    Timestamp = changeTime,
                    Type = "pet_color",
                    PointsDelta = -Math.Abs(pet.PointsChanged), // 換色是消費，所以是負數
                    Description = $"寵物換色消費 {Math.Abs(pet.PointsChanged)} 點數",
                    SourceRecordId = pet.PetID.ToString(),
                    Metadata = JsonSerializer.Serialize(new
                    {
                        skinColor = pet.SkinColor,
                        backgroundColor = pet.BackgroundColor,
                        colorChangedTime = pet.ColorChangedTime
                    })
                }
            };
        }

        /// <summary>
        /// 取得管理調整記錄的明細項目
        /// </summary>
        private async Task<List<LedgerEntryDto>> GetAdjustmentLedgerEntriesAsync(int userId, DateTime? fromDate, DateTime? toDate)
        {
            var query = _context.Notifications
                .Join(_context.NotificationRecipients, 
                    n => n.NotificationId, 
                    nr => nr.NotificationId, 
                    (n, nr) => new { n, nr })
                .Join(_context.NotificationActions,
                    x => x.n.ActionId,
                    na => na.ActionId,
                    (x, na) => new { x.n, x.nr, na })
                .Where(x => x.nr.UserId == userId && x.na.ActionName == "points_adjustment");

            if (fromDate.HasValue)
                query = query.Where(x => x.n.CreatedAt >= fromDate.Value);
            if (toDate.HasValue)
                query = query.Where(x => x.n.CreatedAt <= toDate.Value);

            var adjustmentRecords = await query.ToListAsync();

            return adjustmentRecords.Select(x =>
            {
                var metadata = new { delta = 0, adminId = 0, reason = "" };
                try
                {
                    if (!string.IsNullOrEmpty(x.n.NotificationMessage))
                    {
                        // 從 notification message 中解析點數變化
                        var match = System.Text.RegularExpressions.Regex.Match(
                            x.n.NotificationMessage, @"調整\s*([+-]?\d+)\s*點");
                        if (match.Success)
                        {
                            metadata = new { delta = int.Parse(match.Groups[1].Value), adminId = x.n.SenderManagerId ?? 0, reason = x.n.NotificationMessage };
                        }
                    }
                }
                catch
                {
                    // 解析失敗時使用預設值
                }

                return new LedgerEntryDto
                {
                    Id = $"adjustment_{x.n.NotificationId}",
                    Timestamp = x.n.CreatedAt,
                    Type = "adjustment",
                    PointsDelta = metadata.delta,
                    Description = x.n.NotificationMessage ?? "管理員調整點數",
                    SourceRecordId = x.n.NotificationId.ToString(),
                    Metadata = JsonSerializer.Serialize(metadata)
                };
            }).ToList();
        }

        /// <summary>
        /// 取得使用者點數統計摘要
        /// </summary>
        public async Task<PointsStatisticsDto> GetPointsStatisticsAsync(int userId)
        {
            try
            {
                _logger.LogInformation($"計算使用者 {userId} 的點數統計");

                var wallet = await GetWalletAsync(userId);
                var now = DateTime.UtcNow;
                var today = now.Date;
                var weekStart = today.AddDays(-(int)today.DayOfWeek);
                var monthStart = new DateTime(today.Year, today.Month, 1);

                // 查詢所有交易記錄
                var allLedger = await GetLedgerHistoryAsync(userId, new LedgerQueryDto 
                { 
                    Page = 1, 
                    PageSize = int.MaxValue 
                });

                var entries = allLedger.Items;

                // 計算各時期統計
                var todayEntries = entries.Where(e => e.Timestamp.Date == today);
                var weekEntries = entries.Where(e => e.Timestamp.Date >= weekStart);
                var monthEntries = entries.Where(e => e.Timestamp.Date >= monthStart);

                // 按來源分組統計
                var earningsBySource = entries
                    .Where(e => e.PointsDelta > 0)
                    .GroupBy(e => e.Type)
                    .ToDictionary(g => GetSourceDisplayName(g.Key), g => g.Sum(e => e.PointsDelta));

                var spendingByCategory = entries
                    .Where(e => e.PointsDelta < 0)
                    .GroupBy(e => e.Type)
                    .ToDictionary(g => GetCategoryDisplayName(g.Key), g => Math.Abs(g.Sum(e => e.PointsDelta)));

                return new PointsStatisticsDto
                {
                    TotalPoints = wallet.CurrentPoints,
                    TodayEarned = todayEntries.Where(e => e.PointsDelta > 0).Sum(e => e.PointsDelta),
                    TodaySpent = Math.Abs(todayEntries.Where(e => e.PointsDelta < 0).Sum(e => e.PointsDelta)),
                    WeekEarned = weekEntries.Where(e => e.PointsDelta > 0).Sum(e => e.PointsDelta),
                    WeekSpent = Math.Abs(weekEntries.Where(e => e.PointsDelta < 0).Sum(e => e.PointsDelta)),
                    MonthEarned = monthEntries.Where(e => e.PointsDelta > 0).Sum(e => e.PointsDelta),
                    MonthSpent = Math.Abs(monthEntries.Where(e => e.PointsDelta < 0).Sum(e => e.PointsDelta)),
                    TotalEarned = entries.Where(e => e.PointsDelta > 0).Sum(e => e.PointsDelta),
                    TotalSpent = Math.Abs(entries.Where(e => e.PointsDelta < 0).Sum(e => e.PointsDelta)),
                    EarningsBySource = earningsBySource,
                    SpendingByCategory = spendingByCategory
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"計算使用者 {userId} 點數統計時發生錯誤");
                throw;
            }
        }

        /// <summary>
        /// 取得收入來源顯示名稱
        /// </summary>
        private static string GetSourceDisplayName(string type) => type switch
        {
            "signin" => "每日簽到",
            "minigame" => "小遊戲獎勵",
            "adjustment" => "管理員調整",
            _ => "其他收入"
        };

        /// <summary>
        /// 取得支出類型顯示名稱
        /// </summary>
        private static string GetCategoryDisplayName(string type) => type switch
        {
            "pet_color" => "寵物換色",
            "store_purchase" => "商城購買",
            "adjustment" => "管理員調整",
            _ => "其他支出"
        };

        #endregion

        #region 銷售功能管理 (Sales Management)

        /// <summary>
        /// 申請開通銷售功能
        /// </summary>
        public async Task<ServiceResult<MemberSalesProfileDto>> ApplySalesProfileAsync(int userId, CreateSalesProfileDto salesProfileDto)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                _logger.LogInformation($"使用者 {userId} 申請開通銷售功能");

                // 檢查是否已有銷售檔案
                var existingProfile = await _context.MemberSalesProfiles
                    .Where(p => p.UserId == userId)
                    .FirstOrDefaultAsync();

                if (existingProfile != null)
                {
                    return ServiceResult<MemberSalesProfileDto>.Failure("您已經申請過銷售功能，請等待審核結果");
                }

                // 處理封面照片
                byte[]? photoData = null;
                if (!string.IsNullOrEmpty(salesProfileDto.AccountCoverPhotoBase64))
                {
                    try
                    {
                        photoData = Convert.FromBase64String(salesProfileDto.AccountCoverPhotoBase64);
                    }
                    catch
                    {
                        return ServiceResult<MemberSalesProfileDto>.Failure("封面照片格式錯誤");
                    }
                }

                // 建立銷售檔案
                var salesProfile = new MemberSalesProfile
                {
                    UserId = userId,
                    BankCode = salesProfileDto.BankCode,
                    BankAccountNumber = salesProfileDto.BankAccountNumber,
                    AccountCoverPhoto = photoData
                };

                _context.MemberSalesProfiles.Add(salesProfile);

                // 建立銷售錢包
                var salesWallet = new UserSalesInformation
                {
                    UserId = userId,
                    UserSalesWallet = 0
                };

                _context.UserSalesInformations.Add(salesWallet);

                await _context.SaveChangesAsync();

                // 發送申請通知給管理員
                await _notificationService.CreateSystemNotificationAsync(
                    userId: userId,
                    action: "sales_application",
                    title: "銷售功能申請已提交",
                    message = "您的銷售功能申請已提交，請等待管理員審核。"
                );

                await transaction.CommitAsync();

                var resultDto = new MemberSalesProfileDto
                {
                    UserId = userId,
                    BankCode = salesProfile.BankCode,
                    BankName = GetBankName(salesProfile.BankCode),
                    MaskedBankAccountNumber = MaskBankAccount(salesProfile.BankAccountNumber),
                    SalesAuthorityEnabled = false,
                    CreatedAt = DateTime.UtcNow,
                    ReviewStatus = "pending"
                };

                _logger.LogInformation($"使用者 {userId} 銷售功能申請已提交");
                return ServiceResult<MemberSalesProfileDto>.Success(resultDto, "銷售功能申請已提交，請等待審核");
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _logger.LogError(ex, $"使用者 {userId} 申請銷售功能時發生錯誤");
                return ServiceResult<MemberSalesProfileDto>.Failure("申請過程中發生錯誤，請稍後再試");
            }
        }

        /// <summary>
        /// 取得使用者銷售檔案資訊
        /// </summary>
        public async Task<MemberSalesProfileDto?> GetSalesProfileAsync(int userId)
        {
            try
            {
                var profile = await _context.MemberSalesProfiles
                    .Where(p => p.UserId == userId)
                    .FirstOrDefaultAsync();

                if (profile == null)
                    return null;

                var userRights = await _context.UserRights
                    .Where(r => r.UserId == userId)
                    .FirstOrDefaultAsync();

                return new MemberSalesProfileDto
                {
                    UserId = userId,
                    BankCode = profile.BankCode,
                    BankName = GetBankName(profile.BankCode),
                    MaskedBankAccountNumber = MaskBankAccount(profile.BankAccountNumber),
                    SalesAuthorityEnabled = userRights?.SalesAuthority == true,
                    CreatedAt = DateTime.UtcNow, // 因為原 schema 沒有 created_at 欄位
                    ReviewStatus = userRights?.SalesAuthority == true ? "approved" : "pending"
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"取得使用者 {userId} 銷售檔案時發生錯誤");
                return null;
            }
        }

        /// <summary>
        /// 取得使用者銷售錢包資訊
        /// </summary>
        public async Task<UserSalesInformationDto?> GetSalesWalletAsync(int userId)
        {
            try
            {
                var salesInfo = await _context.UserSalesInformations
                    .Where(s => s.UserId == userId)
                    .FirstOrDefaultAsync();

                if (salesInfo == null)
                    return null;

                // TODO: 計算累計銷售額、手續費等 (需要從訂單系統計算)
                return new UserSalesInformationDto
                {
                    UserId = userId,
                    UserSalesWallet = salesInfo.UserSalesWallet,
                    LastUpdated = DateTime.UtcNow,
                    TotalSalesAmount = 0, // 待實作
                    TotalPlatformFees = 0, // 待實作
                    WithdrawableAmount = salesInfo.UserSalesWallet,
                    MonthlyRevenue = 0 // 待實作
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"取得使用者 {userId} 銷售錢包時發生錯誤");
                return null;
            }
        }

        /// <summary>
        /// 更新銷售錢包餘額
        /// </summary>
        public async Task<ServiceResult<UserSalesInformationDto>> UpdateSalesWalletAsync(int userId, decimal amount, string source)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                _logger.LogInformation($"更新使用者 {userId} 銷售錢包: {amount}, 來源: {source}");

                var salesInfo = await _context.UserSalesInformations
                    .Where(s => s.UserId == userId)
                    .FirstOrDefaultAsync();

                if (salesInfo == null)
                {
                    return ServiceResult<UserSalesInformationDto>.Failure("銷售錢包不存在");
                }

                var oldBalance = salesInfo.UserSalesWallet;
                salesInfo.UserSalesWallet = Math.Max(0, (int)(salesInfo.UserSalesWallet + amount));

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                var resultDto = await GetSalesWalletAsync(userId);
                
                _logger.LogInformation($"成功更新使用者 {userId} 銷售錢包，從 {oldBalance} 更新至 {salesInfo.UserSalesWallet}");
                return ServiceResult<UserSalesInformationDto>.Success(resultDto!, "銷售錢包更新成功");
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _logger.LogError(ex, $"更新使用者 {userId} 銷售錢包時發生錯誤");
                return ServiceResult<UserSalesInformationDto>.Failure("更新銷售錢包時發生錯誤");
            }
        }

        #endregion

        #region 交易處理 (Transaction Processing)

        /// <summary>
        /// 執行點數消費交易
        /// </summary>
        public async Task<ServiceResult<WalletTransactionDto>> SpendPointsAsync(int userId, int points, string purpose, string? referenceId = null)
        {
            if (points <= 0)
                return ServiceResult<WalletTransactionDto>.Failure("消費點數必須大於 0");

            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                _logger.LogInformation($"使用者 {userId} 消費 {points} 點數: {purpose}");

                var wallet = await _context.UserWallets
                    .Where(w => w.UserId == userId)
                    .FirstOrDefaultAsync();

                if (wallet == null)
                {
                    return ServiceResult<WalletTransactionDto>.Failure("錢包不存在");
                }

                if (wallet.UserPoint < points)
                {
                    return ServiceResult<WalletTransactionDto>.Failure($"點數不足，目前餘額: {wallet.UserPoint}，需要: {points}");
                }

                var balanceBefore = wallet.UserPoint;
                wallet.UserPoint -= points;

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                var transactionDto = new WalletTransactionDto
                {
                    TransactionId = Guid.NewGuid().ToString(),
                    UserId = userId,
                    TransactionType = "spend",
                    PointsDelta = -points,
                    BalanceBefore = balanceBefore,
                    BalanceAfter = wallet.UserPoint,
                    Description = purpose,
                    ReferenceId = referenceId,
                    Timestamp = DateTime.UtcNow,
                    Status = "success"
                };

                _logger.LogInformation($"使用者 {userId} 成功消費 {points} 點數，餘額: {wallet.UserPoint}");
                return ServiceResult<WalletTransactionDto>.Success(transactionDto, "點數消費成功");
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _logger.LogError(ex, $"使用者 {userId} 消費點數時發生錯誤");
                return ServiceResult<WalletTransactionDto>.Failure("消費過程中發生錯誤");
            }
        }

        /// <summary>
        /// 執行點數獲得交易
        /// </summary>
        public async Task<ServiceResult<WalletTransactionDto>> EarnPointsAsync(int userId, int points, string source, string? referenceId = null)
        {
            if (points <= 0)
                return ServiceResult<WalletTransactionDto>.Failure("獲得點數必須大於 0");

            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                _logger.LogInformation($"使用者 {userId} 獲得 {points} 點數: {source}");

                var wallet = await _context.UserWallets
                    .Where(w => w.UserId == userId)
                    .FirstOrDefaultAsync();

                if (wallet == null)
                {
                    // 建立新錢包
                    wallet = new UserWallet
                    {
                        UserId = userId,
                        UserPoint = 0
                    };
                    _context.UserWallets.Add(wallet);
                }

                var balanceBefore = wallet.UserPoint;
                wallet.UserPoint += points;

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                var transactionDto = new WalletTransactionDto
                {
                    TransactionId = Guid.NewGuid().ToString(),
                    UserId = userId,
                    TransactionType = "earn",
                    PointsDelta = points,
                    BalanceBefore = balanceBefore,
                    BalanceAfter = wallet.UserPoint,
                    Description = source,
                    ReferenceId = referenceId,
                    Timestamp = DateTime.UtcNow,
                    Status = "success"
                };

                _logger.LogInformation($"使用者 {userId} 成功獲得 {points} 點數，餘額: {wallet.UserPoint}");
                return ServiceResult<WalletTransactionDto>.Success(transactionDto, "點數獲得成功");
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _logger.LogError(ex, $"使用者 {userId} 獲得點數時發生錯誤");
                return ServiceResult<WalletTransactionDto>.Failure("獲得點數過程中發生錯誤");
            }
        }

        #endregion

        #region 輔助方法 (Helper Methods)

        /// <summary>
        /// 取得銀行名稱 (簡化版，實際應從銀行代碼對照表查詢)
        /// </summary>
        private static string GetBankName(int bankCode) => bankCode switch
        {
            1 => "台灣銀行",
            3 => "土地銀行",
            4 => "合作金庫",
            5 => "第一銀行",
            6 => "華南銀行",
            7 => "彰化銀行",
            8 => "上海銀行",
            11 => "台北富邦",
            12 => "台新銀行",
            _ => $"銀行代碼 {bankCode}"
        };

        /// <summary>
        /// 遮蔽銀行帳號 (只顯示後4碼)
        /// </summary>
        private static string MaskBankAccount(string accountNumber)
        {
            if (string.IsNullOrEmpty(accountNumber) || accountNumber.Length <= 4)
                return accountNumber;

            return new string('*', accountNumber.Length - 4) + accountNumber[^4..];
        }

        #endregion
    }
}