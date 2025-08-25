using GameCore.Core.Interfaces;
using GameCore.Core.Models;
using GameCore.Core.Entities;
using GameCore.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Security.Claims;

namespace GameCore.Core.Services
{
    /// <summary>
    /// 錢包服務實作 - 處理用戶點數錢包相關業務邏輯
    /// </summary>
    public class WalletService : IWalletService
    {
        private readonly GameCoreDbContext _context;
        private readonly ILogger<WalletService> _logger;

        public WalletService(GameCoreDbContext context, ILogger<WalletService> logger)
        {
            _context = context;
            _logger = logger;
        }

        /// <summary>
        /// 取得用戶錢包餘額
        /// </summary>
        /// <param name="userId">用戶ID</param>
        /// <returns>錢包餘額資訊</returns>
        public async Task<ServiceResult<WalletBalanceDto>> GetBalanceAsync(int userId)
        {
            try
            {
                _logger.LogInformation("取得用戶 {UserId} 的錢包餘額", userId);

                // 查詢用戶錢包
                var wallet = await _context.UserWallets
                    .FirstOrDefaultAsync(w => w.UserId == userId);

                if (wallet == null)
                {
                    _logger.LogWarning("用戶 {UserId} 的錢包不存在", userId);
                    return ServiceResult<WalletBalanceDto>.NotFoundResult("錢包不存在");
                }

                var balanceDto = new WalletBalanceDto
                {
                    UserId = wallet.UserId,
                    TotalPoints = wallet.TotalPoints,
                    AvailablePoints = wallet.AvailablePoints,
                    FrozenPoints = wallet.FrozenPoints,
                    LastUpdated = wallet.LastUpdated
                };

                _logger.LogInformation("成功取得用戶 {UserId} 的錢包餘額: {AvailablePoints}", userId, wallet.AvailablePoints);
                return ServiceResult<WalletBalanceDto>.SuccessResult(balanceDto, "成功取得錢包餘額");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "取得用戶 {UserId} 錢包餘額時發生錯誤", userId);
                return ServiceResult<WalletBalanceDto>.ServerErrorResult("取得錢包餘額失敗");
            }
        }

        /// <summary>
        /// 取得用戶點數明細
        /// </summary>
        /// <param name="userId">用戶ID</param>
        /// <param name="page">頁碼</param>
        /// <param name="pageSize">每頁大小</param>
        /// <param name="startDate">開始日期</param>
        /// <param name="endDate">結束日期</param>
        /// <returns>點數明細列表</returns>
        public async Task<ServiceResult<PointLedgerDto>> GetLedgerAsync(int userId, int page = 1, int pageSize = 20, DateTime? startDate = null, DateTime? endDate = null)
        {
            try
            {
                _logger.LogInformation("取得用戶 {UserId} 的點數明細，頁碼: {Page}", userId, page);

                var query = _context.PointLedgers
                    .Where(l => l.UserId == userId);

                // 日期篩選
                if (startDate.HasValue)
                {
                    query = query.Where(l => l.CreatedAt >= startDate.Value);
                }
                if (endDate.HasValue)
                {
                    query = query.Where(l => l.CreatedAt <= endDate.Value);
                }

                // 排序和分頁
                var totalCount = await query.CountAsync();
                var ledgers = await query
                    .OrderByDescending(l => l.CreatedAt)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .Select(l => new PointLedgerItemDto
                    {
                        Id = l.Id,
                        Type = l.Type,
                        Amount = l.Amount,
                        Balance = l.Balance,
                        Description = l.Description,
                        CreatedAt = l.CreatedAt,
                        RelatedId = l.RelatedId,
                        RelatedType = l.RelatedType
                    })
                    .ToListAsync();

                var result = new PointLedgerDto
                {
                    Items = ledgers,
                    TotalCount = totalCount,
                    Page = page,
                    PageSize = pageSize,
                    TotalPages = (int)Math.Ceiling((double)totalCount / pageSize)
                };

                _logger.LogInformation("成功取得用戶 {UserId} 的點數明細，共 {Count} 筆", userId, totalCount);
                return ServiceResult<PointLedgerDto>.SuccessResult(result, "成功取得點數明細");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "取得用戶 {UserId} 點數明細時發生錯誤", userId);
                return ServiceResult<PointLedgerDto>.ServerErrorResult("取得點數明細失敗");
            }
        }

        /// <summary>
        /// 取得用戶點數統計
        /// </summary>
        /// <param name="userId">用戶ID</param>
        /// <param name="period">統計週期</param>
        /// <returns>點數統計資訊</returns>
        public async Task<ServiceResult<PointStatisticsDto>> GetStatisticsAsync(int userId, string period = "month")
        {
            try
            {
                _logger.LogInformation("取得用戶 {UserId} 的點數統計，週期: {Period}", userId, period);

                var now = DateTime.UtcNow;
                DateTime startDate;

                switch (period.ToLower())
                {
                    case "week":
                        startDate = now.AddDays(-7);
                        break;
                    case "month":
                        startDate = now.AddMonths(-1);
                        break;
                    case "quarter":
                        startDate = now.AddMonths(-3);
                        break;
                    case "year":
                        startDate = now.AddYears(-1);
                        break;
                    default:
                        startDate = now.AddMonths(-1);
                        break;
                }

                // 統計收入
                var income = await _context.PointLedgers
                    .Where(l => l.UserId == userId && l.Amount > 0 && l.CreatedAt >= startDate)
                    .SumAsync(l => l.Amount);

                // 統計支出
                var expense = await _context.PointLedgers
                    .Where(l => l.UserId == userId && l.Amount < 0 && l.CreatedAt >= startDate)
                    .SumAsync(l => Math.Abs(l.Amount));

                // 統計各類型收入
                var incomeByType = await _context.PointLedgers
                    .Where(l => l.UserId == userId && l.Amount > 0 && l.CreatedAt >= startDate)
                    .GroupBy(l => l.Type)
                    .Select(g => new PointTypeSummaryDto
                    {
                        Type = g.Key,
                        Amount = g.Sum(l => l.Amount),
                        Count = g.Count()
                    })
                    .ToListAsync();

                // 統計各類型支出
                var expenseByType = await _context.PointLedgers
                    .Where(l => l.UserId == userId && l.Amount < 0 && l.CreatedAt >= startDate)
                    .GroupBy(l => l.Type)
                    .Select(g => new PointTypeSummaryDto
                    {
                        Type = g.Key,
                        Amount = g.Sum(l => Math.Abs(l.Amount)),
                        Count = g.Count()
                    })
                    .ToListAsync();

                var statistics = new PointStatisticsDto
                {
                    Period = period,
                    StartDate = startDate,
                    EndDate = now,
                    TotalIncome = income,
                    TotalExpense = expense,
                    NetChange = income - expense,
                    IncomeByType = incomeByType,
                    ExpenseByType = expenseByType
                };

                _logger.LogInformation("成功取得用戶 {UserId} 的點數統計", userId);
                return ServiceResult<PointStatisticsDto>.SuccessResult(statistics, "成功取得點數統計");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "取得用戶 {UserId} 點數統計時發生錯誤", userId);
                return ServiceResult<PointStatisticsDto>.ServerErrorResult("取得點數統計失敗");
            }
        }

        /// <summary>
        /// 取得用戶優惠券列表
        /// </summary>
        /// <param name="userId">用戶ID</param>
        /// <param name="status">優惠券狀態</param>
        /// <returns>優惠券列表</returns>
        public async Task<ServiceResult<List<CouponDto>>> GetCouponsAsync(int userId, string? status = null)
        {
            try
            {
                _logger.LogInformation("取得用戶 {UserId} 的優惠券列表，狀態: {Status}", userId, status ?? "全部");

                var query = _context.Coupons
                    .Where(c => c.UserId == userId);

                // 狀態篩選
                if (!string.IsNullOrEmpty(status))
                {
                    query = query.Where(c => c.Status == status);
                }

                var coupons = await query
                    .OrderByDescending(c => c.CreatedAt)
                    .Select(c => new CouponDto
                    {
                        Id = c.Id,
                        Code = c.Code,
                        Type = c.Type,
                        Value = c.Value,
                        MinAmount = c.MinAmount,
                        MaxDiscount = c.MaxDiscount,
                        Status = c.Status,
                        ExpiredAt = c.ExpiredAt,
                        UsedAt = c.UsedAt,
                        CreatedAt = c.CreatedAt
                    })
                    .ToListAsync();

                _logger.LogInformation("成功取得用戶 {UserId} 的優惠券列表，共 {Count} 張", userId, coupons.Count);
                return ServiceResult<List<CouponDto>>.SuccessResult(coupons, "成功取得優惠券列表");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "取得用戶 {UserId} 優惠券列表時發生錯誤", userId);
                return ServiceResult<List<CouponDto>>.ServerErrorResult("取得優惠券列表失敗");
            }
        }

        /// <summary>
        /// 使用優惠券
        /// </summary>
        /// <param name="userId">用戶ID</param>
        /// <param name="couponCode">優惠券代碼</param>
        /// <param name="amount">使用金額</param>
        /// <returns>使用結果</returns>
        public async Task<ServiceResult<CouponUsageResultDto>> UseCouponAsync(int userId, string couponCode, decimal amount)
        {
            try
            {
                _logger.LogInformation("用戶 {UserId} 嘗試使用優惠券 {CouponCode}，金額: {Amount}", userId, couponCode, amount);

                // 查詢優惠券
                var coupon = await _context.Coupons
                    .FirstOrDefaultAsync(c => c.UserId == userId && c.Code == couponCode);

                if (coupon == null)
                {
                    _logger.LogWarning("用戶 {UserId} 嘗試使用不存在的優惠券 {CouponCode}", userId, couponCode);
                    return ServiceResult<CouponUsageResultDto>.NotFoundResult("優惠券不存在");
                }

                // 檢查優惠券狀態
                if (coupon.Status != "active")
                {
                    _logger.LogWarning("用戶 {UserId} 嘗試使用狀態為 {Status} 的優惠券 {CouponCode}", userId, coupon.Status, couponCode);
                    return ServiceResult<CouponUsageResultDto>.BusinessErrorResult("優惠券狀態無效");
                }

                // 檢查優惠券是否過期
                if (coupon.ExpiredAt.HasValue && coupon.ExpiredAt.Value < DateTime.UtcNow)
                {
                    _logger.LogWarning("用戶 {UserId} 嘗試使用已過期的優惠券 {CouponCode}", userId, couponCode);
                    return ServiceResult<CouponUsageResultDto>.BusinessErrorResult("優惠券已過期");
                }

                // 檢查使用金額
                if (amount < coupon.MinAmount)
                {
                    _logger.LogWarning("用戶 {UserId} 使用優惠券 {CouponCode} 的金額 {Amount} 低於最低要求 {MinAmount}", 
                        userId, couponCode, amount, coupon.MinAmount);
                    return ServiceResult<CouponUsageResultDto>.BusinessErrorResult($"使用金額需達到 {coupon.MinAmount}");
                }

                // 計算折扣金額
                decimal discountAmount = 0;
                switch (coupon.Type)
                {
                    case "percentage":
                        discountAmount = amount * (coupon.Value / 100);
                        if (coupon.MaxDiscount.HasValue && discountAmount > coupon.MaxDiscount.Value)
                        {
                            discountAmount = coupon.MaxDiscount.Value;
                        }
                        break;
                    case "fixed":
                        discountAmount = coupon.Value;
                        break;
                    default:
                        _logger.LogError("未知的優惠券類型: {Type}", coupon.Type);
                        return ServiceResult<CouponUsageResultDto>.ServerErrorResult("優惠券類型錯誤");
                }

                // 更新優惠券狀態
                coupon.Status = "used";
                coupon.UsedAt = DateTime.UtcNow;

                // 記錄點數變更
                var pointLedger = new PointLedger
                {
                    UserId = userId,
                    Type = "coupon_discount",
                    Amount = discountAmount,
                    Balance = 0, // 稍後更新
                    Description = $"使用優惠券 {couponCode}，折扣 {discountAmount}",
                    RelatedId = coupon.Id,
                    RelatedType = "Coupon",
                    CreatedAt = DateTime.UtcNow
                };

                _context.Coupons.Update(coupon);
                _context.PointLedgers.Add(pointLedger);

                await _context.SaveChangesAsync();

                var result = new CouponUsageResultDto
                {
                    CouponId = coupon.Id,
                    CouponCode = coupon.Code,
                    OriginalAmount = amount,
                    DiscountAmount = discountAmount,
                    FinalAmount = amount - discountAmount,
                    UsedAt = coupon.UsedAt.Value
                };

                _logger.LogInformation("用戶 {UserId} 成功使用優惠券 {CouponCode}，折扣 {DiscountAmount}", userId, couponCode, discountAmount);
                return ServiceResult<CouponUsageResultDto>.SuccessResult(result, "成功使用優惠券");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "用戶 {UserId} 使用優惠券 {CouponCode} 時發生錯誤", userId, couponCode);
                return ServiceResult<CouponUsageResultDto>.ServerErrorResult("使用優惠券失敗");
            }
        }

        /// <summary>
        /// 取得點數排行榜
        /// </summary>
        /// <param name="period">排行榜週期</param>
        /// <param name="limit">排行榜數量限制</param>
        /// <returns>點數排行榜</returns>
        public async Task<ServiceResult<List<PointLeaderboardDto>>> GetLeaderboardAsync(string period = "month", int limit = 100)
        {
            try
            {
                _logger.LogInformation("取得點數排行榜，週期: {Period}，限制: {Limit}", period, limit);

                var now = DateTime.UtcNow;
                DateTime startDate;

                switch (period.ToLower())
                {
                    case "week":
                        startDate = now.AddDays(-7);
                        break;
                    case "month":
                        startDate = now.AddMonths(-1);
                        break;
                    case "quarter":
                        startDate = now.AddMonths(-3);
                        break;
                    case "year":
                        startDate = now.AddYears(-1);
                        break;
                    case "all":
                        startDate = DateTime.MinValue;
                        break;
                    default:
                        startDate = now.AddMonths(-1);
                        break;
                }

                var leaderboard = await _context.PointLedgers
                    .Where(l => l.Amount > 0 && l.CreatedAt >= startDate)
                    .GroupBy(l => l.UserId)
                    .Select(g => new
                    {
                        UserId = g.Key,
                        TotalPoints = g.Sum(l => l.Amount),
                        TransactionCount = g.Count()
                    })
                    .OrderByDescending(x => x.TotalPoints)
                    .Take(limit)
                    .ToListAsync();

                // 取得用戶資訊
                var userIds = leaderboard.Select(x => x.UserId).ToList();
                var users = await _context.Users
                    .Where(u => userIds.Contains(u.Id))
                    .Select(u => new { u.Id, u.Username, u.Email })
                    .ToDictionaryAsync(u => u.Id);

                var result = leaderboard.Select((item, index) => new PointLeaderboardDto
                {
                    Rank = index + 1,
                    UserId = item.UserId,
                    Username = users.ContainsKey(item.UserId) ? users[item.UserId].Username : "未知用戶",
                    Email = users.ContainsKey(item.UserId) ? users[item.UserId].Email : "",
                    TotalPoints = item.TotalPoints,
                    TransactionCount = item.TransactionCount
                }).ToList();

                _logger.LogInformation("成功取得點數排行榜，共 {Count} 名用戶", result.Count);
                return ServiceResult<List<PointLeaderboardDto>>.SuccessResult(result, "成功取得點數排行榜");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "取得點數排行榜時發生錯誤");
                return ServiceResult<List<PointLeaderboardDto>>.ServerErrorResult("取得點數排行榜失敗");
            }
        }

        /// <summary>
        /// 取得點數賺取方式
        /// </summary>
        /// <returns>點數賺取方式列表</returns>
        public async Task<ServiceResult<List<PointEarningMethodDto>>> GetEarningMethodsAsync()
        {
            try
            {
                _logger.LogInformation("取得點數賺取方式列表");

                var methods = new List<PointEarningMethodDto>
                {
                    new PointEarningMethodDto
                    {
                        Method = "daily_signin",
                        Name = "每日簽到",
                        Description = "每日簽到可獲得點數獎勵",
                        BasePoints = 10,
                        BonusPoints = 5,
                        MaxBonusDays = 7,
                        CooldownHours = 24
                    },
                    new PointEarningMethodDto
                    {
                        Method = "minigame",
                        Name = "小遊戲",
                        Description = "完成小遊戲可獲得點數獎勵",
                        BasePoints = 20,
                        BonusPoints = 10,
                        MaxBonusDays = 3,
                        CooldownHours = 24
                    },
                    new PointEarningMethodDto
                    {
                        Method = "pet_care",
                        Name = "寵物照顧",
                        Description = "照顧寵物可獲得點數獎勵",
                        BasePoints = 15,
                        BonusPoints = 8,
                        MaxBonusDays = 1,
                        CooldownHours = 12
                    },
                    new PointEarningMethodDto
                    {
                        Method = "referral",
                        Name = "推薦好友",
                        Description = "推薦好友註冊可獲得點數獎勵",
                        BasePoints = 100,
                        BonusPoints = 50,
                        MaxBonusDays = 0,
                        CooldownHours = 0
                    },
                    new PointEarningMethodDto
                    {
                        Method = "purchase",
                        Name = "購買商品",
                        Description = "購買商品可獲得點數回饋",
                        BasePoints = 0,
                        BonusPoints = 0,
                        MaxBonusDays = 0,
                        CooldownHours = 0
                    }
                };

                _logger.LogInformation("成功取得點數賺取方式列表，共 {Count} 種方式", methods.Count);
                return ServiceResult<List<PointEarningMethodDto>>.SuccessResult(methods, "成功取得點數賺取方式");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "取得點數賺取方式時發生錯誤");
                return ServiceResult<List<PointEarningMethodDto>>.ServerErrorResult("取得點數賺取方式失敗");
            }
        }

        /// <summary>
        /// 取得點數支出歷史
        /// </summary>
        /// <param name="userId">用戶ID</param>
        /// <param name="page">頁碼</param>
        /// <param name="pageSize">每頁大小</param>
        /// <returns>點數支出歷史</returns>
        public async Task<ServiceResult<PointSpendingHistoryDto>> GetSpendingHistoryAsync(int userId, int page = 1, int pageSize = 20)
        {
            try
            {
                _logger.LogInformation("取得用戶 {UserId} 的點數支出歷史，頁碼: {Page}", userId, page);

                var query = _context.PointLedgers
                    .Where(l => l.UserId == userId && l.Amount < 0);

                var totalCount = await query.CountAsync();
                var spendings = await query
                    .OrderByDescending(l => l.CreatedAt)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .Select(l => new PointSpendingItemDto
                    {
                        Id = l.Id,
                        Type = l.Type,
                        Amount = Math.Abs(l.Amount),
                        Description = l.Description,
                        CreatedAt = l.CreatedAt,
                        RelatedId = l.RelatedId,
                        RelatedType = l.RelatedType
                    })
                    .ToListAsync();

                var result = new PointSpendingHistoryDto
                {
                    Items = spendings,
                    TotalCount = totalCount,
                    Page = page,
                    PageSize = pageSize,
                    TotalPages = (int)Math.Ceiling((double)totalCount / pageSize)
                };

                _logger.LogInformation("成功取得用戶 {UserId} 的點數支出歷史，共 {Count} 筆", userId, totalCount);
                return ServiceResult<PointSpendingHistoryDto>.SuccessResult(result, "成功取得點數支出歷史");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "取得用戶 {UserId} 點數支出歷史時發生錯誤", userId);
                return ServiceResult<PointSpendingHistoryDto>.ServerErrorResult("取得點數支出歷史失敗");
            }
        }

        /// <summary>
        /// 取得點數預測
        /// </summary>
        /// <param name="userId">用戶ID</param>
        /// <param name="days">預測天數</param>
        /// <returns>點數預測資訊</returns>
        public async Task<ServiceResult<PointForecastDto>> GetPointsForecastAsync(int userId, int days = 30)
        {
            try
            {
                _logger.LogInformation("取得用戶 {UserId} 的點數預測，預測天數: {Days}", userId, days);

                var now = DateTime.UtcNow;
                var startDate = now.AddDays(-30); // 分析過去30天的數據

                // 分析過去收入模式
                var pastIncome = await _context.PointLedgers
                    .Where(l => l.UserId == userId && l.Amount > 0 && l.CreatedAt >= startDate)
                    .GroupBy(l => l.Type)
                    .Select(g => new
                    {
                        Type = g.Key,
                        TotalAmount = g.Sum(l => l.Amount),
                        Count = g.Count(),
                        AverageAmount = g.Average(l => l.Amount)
                    })
                    .ToListAsync();

                // 分析過去支出模式
                var pastExpense = await _context.PointLedgers
                    .Where(l => l.UserId == userId && l.Amount < 0 && l.CreatedAt >= startDate)
                    .GroupBy(l => l.Type)
                    .Select(g => new
                    {
                        Type = g.Key,
                        TotalAmount = g.Sum(l => Math.Abs(l.Amount)),
                        Count = g.Count(),
                        AverageAmount = g.Average(l => Math.Abs(l.Amount))
                    })
                    .ToListAsync();

                // 計算預測
                var forecast = new PointForecastDto
                {
                    UserId = userId,
                    ForecastDays = days,
                    CurrentBalance = 0, // 稍後更新
                    PredictedIncome = new List<PointForecastItemDto>(),
                    PredictedExpense = new List<PointForecastItemDto>(),
                    NetPrediction = 0
                };

                // 預測收入
                foreach (var income in pastIncome)
                {
                    var dailyAverage = income.TotalAmount / 30.0;
                    var predictedAmount = dailyAverage * days;

                    forecast.PredictedIncome.Add(new PointForecastItemDto
                    {
                        Type = income.Type,
                        PredictedAmount = Math.Round(predictedAmount, 2),
                        Confidence = Math.Min(0.9, income.Count / 30.0) // 信心度基於數據量
                    });

                    forecast.NetPrediction += predictedAmount;
                }

                // 預測支出
                foreach (var expense in pastExpense)
                {
                    var dailyAverage = expense.TotalAmount / 30.0;
                    var predictedAmount = dailyAverage * days;

                    forecast.PredictedExpense.Add(new PointForecastItemDto
                    {
                        Type = expense.Type,
                        PredictedAmount = Math.Round(predictedAmount, 2),
                        Confidence = Math.Min(0.9, expense.Count / 30.0)
                    });

                    forecast.NetPrediction -= predictedAmount;
                }

                // 取得當前餘額
                var wallet = await _context.UserWallets
                    .FirstOrDefaultAsync(w => w.UserId == userId);
                if (wallet != null)
                {
                    forecast.CurrentBalance = wallet.AvailablePoints;
                }

                forecast.NetPrediction = Math.Round(forecast.NetPrediction, 2);

                _logger.LogInformation("成功取得用戶 {UserId} 的點數預測", userId);
                return ServiceResult<PointForecastDto>.SuccessResult(forecast, "成功取得點數預測");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "取得用戶 {UserId} 點數預測時發生錯誤", userId);
                return ServiceResult<PointForecastDto>.ServerErrorResult("取得點數預測失敗");
            }
        }

        // 以下方法為管理員或內部使用

        /// <summary>
        /// 調整用戶點數
        /// </summary>
        /// <param name="userId">用戶ID</param>
        /// <param name="amount">調整金額</param>
        /// <param name="type">調整類型</param>
        /// <param name="description">調整說明</param>
        /// <param name="adminId">管理員ID</param>
        /// <returns>調整結果</returns>
        public async Task<ServiceResult<PointAdjustmentDto>> AdjustPointsAsync(int userId, decimal amount, string type, string description, int adminId)
        {
            try
            {
                _logger.LogInformation("管理員 {AdminId} 調整用戶 {UserId} 點數，金額: {Amount}，類型: {Type}", adminId, userId, amount, type);

                // 檢查用戶是否存在
                var user = await _context.Users.FindAsync(userId);
                if (user == null)
                {
                    _logger.LogWarning("管理員 {AdminId} 嘗試調整不存在的用戶 {UserId} 的點數", adminId, userId);
                    return ServiceResult<PointAdjustmentDto>.NotFoundResult("用戶不存在");
                }

                // 取得或創建錢包
                var wallet = await _context.UserWallets
                    .FirstOrDefaultAsync(w => w.UserId == userId);

                if (wallet == null)
                {
                    wallet = new UserWallet
                    {
                        UserId = userId,
                        TotalPoints = 0,
                        AvailablePoints = 0,
                        FrozenPoints = 0,
                        LastUpdated = DateTime.UtcNow
                    };
                    _context.UserWallets.Add(wallet);
                }

                // 更新錢包餘額
                var oldBalance = wallet.AvailablePoints;
                wallet.AvailablePoints += amount;
                wallet.TotalPoints += amount;
                wallet.LastUpdated = DateTime.UtcNow;

                // 記錄點數變更
                var pointLedger = new PointLedger
                {
                    UserId = userId,
                    Type = type,
                    Amount = amount,
                    Balance = wallet.AvailablePoints,
                    Description = description,
                    RelatedId = adminId,
                    RelatedType = "Admin",
                    CreatedAt = DateTime.UtcNow
                };

                _context.PointLedgers.Add(pointLedger);

                await _context.SaveChangesAsync();

                var result = new PointAdjustmentDto
                {
                    UserId = userId,
                    OldBalance = oldBalance,
                    NewBalance = wallet.AvailablePoints,
                    AdjustmentAmount = amount,
                    Type = type,
                    Description = description,
                    AdminId = adminId,
                    AdjustedAt = DateTime.UtcNow
                };

                _logger.LogInformation("管理員 {AdminId} 成功調整用戶 {UserId} 點數，舊餘額: {OldBalance}，新餘額: {NewBalance}", 
                    adminId, userId, oldBalance, wallet.AvailablePoints);
                return ServiceResult<PointAdjustmentDto>.SuccessResult(result, "成功調整點數");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "管理員 {AdminId} 調整用戶 {UserId} 點數時發生錯誤", adminId, userId);
                return ServiceResult<PointAdjustmentDto>.ServerErrorResult("調整點數失敗");
            }
        }

        /// <summary>
        /// 取得點數變更歷史 (管理員用)
        /// </summary>
        /// <param name="userId">用戶ID</param>
        /// <param name="page">頁碼</param>
        /// <param name="pageSize">每頁大小</param>
        /// <returns>點數變更歷史</returns>
        public async Task<ServiceResult<PointHistoryDto>> GetPointsHistoryAsync(int userId, int page = 1, int pageSize = 20)
        {
            try
            {
                _logger.LogInformation("取得用戶 {UserId} 的點數變更歷史，頁碼: {Page}", userId, page);

                var query = _context.PointLedgers
                    .Where(l => l.UserId == userId);

                var totalCount = await query.CountAsync();
                var history = await query
                    .OrderByDescending(l => l.CreatedAt)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .Select(l => new PointHistoryItemDto
                    {
                        Id = l.Id,
                        Type = l.Type,
                        Amount = l.Amount,
                        Balance = l.Balance,
                        Description = l.Description,
                        CreatedAt = l.CreatedAt,
                        RelatedId = l.RelatedId,
                        RelatedType = l.RelatedType
                    })
                    .ToListAsync();

                var result = new PointHistoryDto
                {
                    Items = history,
                    TotalCount = totalCount,
                    Page = page,
                    PageSize = pageSize,
                    TotalPages = (int)Math.Ceiling((double)totalCount / pageSize)
                };

                _logger.LogInformation("成功取得用戶 {UserId} 的點數變更歷史，共 {Count} 筆", userId, totalCount);
                return ServiceResult<PointHistoryDto>.SuccessResult(result, "成功取得點數變更歷史");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "取得用戶 {UserId} 點數變更歷史時發生錯誤", userId);
                return ServiceResult<PointHistoryDto>.ServerErrorResult("取得點數變更歷史失敗");
            }
        }

        /// <summary>
        /// 檢查用戶點數餘額
        /// </summary>
        /// <param name="userId">用戶ID</param>
        /// <returns>點數餘額檢查結果</returns>
        public async Task<ServiceResult<PointBalanceCheckDto>> CheckBalanceAsync(int userId)
        {
            try
            {
                _logger.LogInformation("檢查用戶 {UserId} 的點數餘額", userId);

                var wallet = await _context.UserWallets
                    .FirstOrDefaultAsync(w => w.UserId == userId);

                if (wallet == null)
                {
                    _logger.LogWarning("用戶 {UserId} 的錢包不存在", userId);
                    return ServiceResult<PointBalanceCheckDto>.NotFoundResult("錢包不存在");
                }

                var result = new PointBalanceCheckDto
                {
                    UserId = userId,
                    TotalPoints = wallet.TotalPoints,
                    AvailablePoints = wallet.AvailablePoints,
                    FrozenPoints = wallet.FrozenPoints,
                    LastUpdated = wallet.LastUpdated,
                    IsSufficient = wallet.AvailablePoints > 0
                };

                _logger.LogInformation("成功檢查用戶 {UserId} 的點數餘額: {AvailablePoints}", userId, wallet.AvailablePoints);
                return ServiceResult<PointBalanceCheckDto>.SuccessResult(result, "成功檢查點數餘額");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "檢查用戶 {UserId} 點數餘額時發生錯誤", userId);
                return ServiceResult<PointBalanceCheckDto>.ServerErrorResult("檢查點數餘額失敗");
            }
        }

        /// <summary>
        /// 凍結用戶點數
        /// </summary>
        /// <param name="userId">用戶ID</param>
        /// <param name="amount">凍結金額</param>
        /// <param name="reason">凍結原因</param>
        /// <param name="adminId">管理員ID</param>
        /// <returns>凍結結果</returns>
        public async Task<ServiceResult<PointFreezeDto>> FreezePointsAsync(int userId, decimal amount, string reason, int adminId)
        {
            try
            {
                _logger.LogInformation("管理員 {AdminId} 凍結用戶 {UserId} 點數，金額: {Amount}，原因: {Reason}", adminId, userId, amount, reason);

                var wallet = await _context.UserWallets
                    .FirstOrDefaultAsync(w => w.UserId == userId);

                if (wallet == null)
                {
                    _logger.LogWarning("管理員 {AdminId} 嘗試凍結不存在的用戶 {UserId} 的點數", adminId, userId);
                    return ServiceResult<PointFreezeDto>.NotFoundResult("錢包不存在");
                }

                if (wallet.AvailablePoints < amount)
                {
                    _logger.LogWarning("管理員 {AdminId} 嘗試凍結超過可用餘額的點數，可用: {Available}，凍結: {Freeze}", 
                        adminId, wallet.AvailablePoints, amount);
                    return ServiceResult<PointFreezeDto>.BusinessErrorResult("可用點數不足");
                }

                // 更新錢包
                wallet.AvailablePoints -= amount;
                wallet.FrozenPoints += amount;
                wallet.LastUpdated = DateTime.UtcNow;

                // 記錄凍結操作
                var pointLedger = new PointLedger
                {
                    UserId = userId,
                    Type = "freeze",
                    Amount = -amount,
                    Balance = wallet.AvailablePoints,
                    Description = $"點數凍結: {reason}",
                    RelatedId = adminId,
                    RelatedType = "Admin",
                    CreatedAt = DateTime.UtcNow
                };

                _context.PointLedgers.Add(pointLedger);

                await _context.SaveChangesAsync();

                var result = new PointFreezeDto
                {
                    UserId = userId,
                    FrozenAmount = amount,
                    AvailablePoints = wallet.AvailablePoints,
                    FrozenPoints = wallet.FrozenPoints,
                    Reason = reason,
                    AdminId = adminId,
                    FrozenAt = DateTime.UtcNow
                };

                _logger.LogInformation("管理員 {AdminId} 成功凍結用戶 {UserId} 點數 {Amount}", adminId, userId, amount);
                return ServiceResult<PointFreezeDto>.SuccessResult(result, "成功凍結點數");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "管理員 {AdminId} 凍結用戶 {UserId} 點數時發生錯誤", adminId, userId);
                return ServiceResult<PointFreezeDto>.ServerErrorResult("凍結點數失敗");
            }
        }

        /// <summary>
        /// 解凍用戶點數
        /// </summary>
        /// <param name="userId">用戶ID</param>
        /// <param name="amount">解凍金額</param>
        /// <param name="reason">解凍原因</param>
        /// <param name="adminId">管理員ID</param>
        /// <returns>解凍結果</returns>
        public async Task<ServiceResult<PointUnfreezeDto>> UnfreezePointsAsync(int userId, decimal amount, string reason, int adminId)
        {
            try
            {
                _logger.LogInformation("管理員 {AdminId} 解凍用戶 {UserId} 點數，金額: {Amount}，原因: {Reason}", adminId, userId, amount, reason);

                var wallet = await _context.UserWallets
                    .FirstOrDefaultAsync(w => w.UserId == userId);

                if (wallet == null)
                {
                    _logger.LogWarning("管理員 {AdminId} 嘗試解凍不存在的用戶 {UserId} 的點數", adminId, userId);
                    return ServiceResult<PointUnfreezeDto>.NotFoundResult("錢包不存在");
                }

                if (wallet.FrozenPoints < amount)
                {
                    _logger.LogWarning("管理員 {AdminId} 嘗試解凍超過凍結餘額的點數，凍結: {Frozen}，解凍: {Unfreeze}", 
                        adminId, wallet.FrozenPoints, amount);
                    return ServiceResult<PointUnfreezeDto>.BusinessErrorResult("凍結點數不足");
                }

                // 更新錢包
                wallet.AvailablePoints += amount;
                wallet.FrozenPoints -= amount;
                wallet.LastUpdated = DateTime.UtcNow;

                // 記錄解凍操作
                var pointLedger = new PointLedger
                {
                    UserId = userId,
                    Type = "unfreeze",
                    Amount = amount,
                    Balance = wallet.AvailablePoints,
                    Description = $"點數解凍: {reason}",
                    RelatedId = adminId,
                    RelatedType = "Admin",
                    CreatedAt = DateTime.UtcNow
                };

                _context.PointLedgers.Add(pointLedger);

                await _context.SaveChangesAsync();

                var result = new PointUnfreezeDto
                {
                    UserId = userId,
                    UnfrozenAmount = amount,
                    AvailablePoints = wallet.AvailablePoints,
                    FrozenPoints = wallet.FrozenPoints,
                    Reason = reason,
                    AdminId = adminId,
                    UnfrozenAt = DateTime.UtcNow
                };

                _logger.LogInformation("管理員 {AdminId} 成功解凍用戶 {UserId} 點數 {Amount}", adminId, userId, amount);
                return ServiceResult<PointUnfreezeDto>.SuccessResult(result, "成功解凍點數");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "管理員 {AdminId} 解凍用戶 {UserId} 點數時發生錯誤", adminId, userId);
                return ServiceResult<PointUnfreezeDto>.ServerErrorResult("解凍點數失敗");
            }
        }

        /// <summary>
        /// 取得凍結點數資訊
        /// </summary>
        /// <param name="userId">用戶ID</param>
        /// <returns>凍結點數資訊</returns>
        public async Task<ServiceResult<FrozenPointsDto>> GetFrozenPointsAsync(int userId)
        {
            try
            {
                _logger.LogInformation("取得用戶 {UserId} 的凍結點數資訊", userId);

                var wallet = await _context.UserWallets
                    .FirstOrDefaultAsync(w => w.UserId == userId);

                if (wallet == null)
                {
                    _logger.LogWarning("用戶 {UserId} 的錢包不存在", userId);
                    return ServiceResult<FrozenPointsDto>.NotFoundResult("錢包不存在");
                }

                var result = new FrozenPointsDto
                {
                    UserId = userId,
                    FrozenPoints = wallet.FrozenPoints,
                    AvailablePoints = wallet.AvailablePoints,
                    TotalPoints = wallet.TotalPoints,
                    LastUpdated = wallet.LastUpdated
                };

                _logger.LogInformation("成功取得用戶 {UserId} 的凍結點數資訊，凍結: {FrozenPoints}", userId, wallet.FrozenPoints);
                return ServiceResult<FrozenPointsDto>.SuccessResult(result, "成功取得凍結點數資訊");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "取得用戶 {UserId} 凍結點數資訊時發生錯誤", userId);
                return ServiceResult<FrozenPointsDto>.ServerErrorResult("取得凍結點數資訊失敗");
            }
        }
    }
}