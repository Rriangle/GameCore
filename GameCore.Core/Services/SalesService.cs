using GameCore.Core.Interfaces;
using GameCore.Core.Models;
using GameCore.Core.Entities;
using GameCore.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace GameCore.Core.Services
{
    /// <summary>
    /// 銷售服務實作 - 處理用戶銷售相關業務邏輯
    /// </summary>
    public class SalesService : ISalesService
    {
        private readonly GameCoreDbContext _context;
        private readonly ILogger<SalesService> _logger;

        public SalesService(GameCoreDbContext context, ILogger<SalesService> logger)
        {
            _context = context;
            _logger = logger;
        }

        /// <summary>
        /// 申請銷售權限
        /// </summary>
        public async Task<ServiceResult<SalesPermissionDto>> ApplySalesPermissionAsync(int userId, SalesPermissionRequestDto request)
        {
            try
            {
                _logger.LogInformation("用戶 {UserId} 申請銷售權限", userId);

                // 檢查是否已有申請
                var existingApplication = await _context.SalesPermissions
                    .FirstOrDefaultAsync(sp => sp.UserId == userId);

                if (existingApplication != null)
                {
                    return ServiceResult<SalesPermissionDto>.BusinessErrorResult("已有銷售權限申請");
                }

                var permission = new SalesPermission
                {
                    UserId = userId,
                    Status = "pending",
                    AppliedAt = DateTime.UtcNow,
                    BusinessLicense = request.BusinessLicense,
                    TaxId = request.TaxId,
                    BankAccount = request.BankAccount,
                    ContactPhone = request.ContactPhone,
                    BusinessAddress = request.BusinessAddress
                };

                _context.SalesPermissions.Add(permission);
                await _context.SaveChangesAsync();

                var result = new SalesPermissionDto
                {
                    Id = permission.Id,
                    UserId = permission.UserId,
                    Status = permission.Status,
                    AppliedAt = permission.AppliedAt
                };

                return ServiceResult<SalesPermissionDto>.SuccessResult(result, "銷售權限申請已提交");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "用戶 {UserId} 申請銷售權限時發生錯誤", userId);
                return ServiceResult<SalesPermissionDto>.ServerErrorResult("申請銷售權限失敗");
            }
        }

        /// <summary>
        /// 取得申請狀態
        /// </summary>
        public async Task<ServiceResult<SalesPermissionDto>> GetApplicationStatusAsync(int userId)
        {
            try
            {
                var permission = await _context.SalesPermissions
                    .FirstOrDefaultAsync(sp => sp.UserId == userId);

                if (permission == null)
                {
                    return ServiceResult<SalesPermissionDto>.NotFoundResult("未找到銷售權限申請");
                }

                var result = new SalesPermissionDto
                {
                    Id = permission.Id,
                    UserId = permission.UserId,
                    Status = permission.Status,
                    AppliedAt = permission.AppliedAt,
                    ApprovedAt = permission.ApprovedAt,
                    RejectedAt = permission.RejectedAt,
                    RejectionReason = permission.RejectionReason
                };

                return ServiceResult<SalesPermissionDto>.SuccessResult(result, "成功取得申請狀態");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "取得用戶 {UserId} 銷售權限申請狀態時發生錯誤", userId);
                return ServiceResult<SalesPermissionDto>.ServerErrorResult("取得申請狀態失敗");
            }
        }

        /// <summary>
        /// 取得銷售錢包
        /// </summary>
        public async Task<ServiceResult<SalesWalletDto>> GetSalesWalletAsync(int userId)
        {
            try
            {
                var wallet = await _context.SalesWallets
                    .FirstOrDefaultAsync(sw => sw.UserId == userId);

                if (wallet == null)
                {
                    return ServiceResult<SalesWalletDto>.NotFoundResult("銷售錢包不存在");
                }

                var result = new SalesWalletDto
                {
                    UserId = wallet.UserId,
                    TotalEarnings = wallet.TotalEarnings,
                    AvailableBalance = wallet.AvailableBalance,
                    PendingBalance = wallet.PendingBalance,
                    LastUpdated = wallet.LastUpdated
                };

                return ServiceResult<SalesWalletDto>.SuccessResult(result, "成功取得銷售錢包");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "取得用戶 {UserId} 銷售錢包時發生錯誤", userId);
                return ServiceResult<SalesWalletDto>.ServerErrorResult("取得銷售錢包失敗");
            }
        }

        /// <summary>
        /// 申請提現
        /// </summary>
        public async Task<ServiceResult<WithdrawalRequestDto>> RequestWithdrawalAsync(int userId, WithdrawalRequestRequestDto request)
        {
            try
            {
                var wallet = await _context.SalesWallets
                    .FirstOrDefaultAsync(sw => sw.UserId == userId);

                if (wallet == null)
                {
                    return ServiceResult<WithdrawalRequestDto>.NotFoundResult("銷售錢包不存在");
                }

                if (wallet.AvailableBalance < request.Amount)
                {
                    return ServiceResult<WithdrawalRequestDto>.BusinessErrorResult("可用餘額不足");
                }

                var withdrawal = new WithdrawalRequest
                {
                    UserId = userId,
                    Amount = request.Amount,
                    BankAccount = request.BankAccount,
                    Status = "pending",
                    RequestedAt = DateTime.UtcNow
                };

                _context.WithdrawalRequests.Add(withdrawal);

                // 更新錢包餘額
                wallet.AvailableBalance -= request.Amount;
                wallet.PendingBalance += request.Amount;

                await _context.SaveChangesAsync();

                var result = new WithdrawalRequestDto
                {
                    Id = withdrawal.Id,
                    UserId = withdrawal.UserId,
                    Amount = withdrawal.Amount,
                    Status = withdrawal.Status,
                    RequestedAt = withdrawal.RequestedAt
                };

                return ServiceResult<WithdrawalRequestDto>.SuccessResult(result, "提現申請已提交");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "用戶 {UserId} 申請提現時發生錯誤", userId);
                return ServiceResult<WithdrawalRequestDto>.ServerErrorResult("申請提現失敗");
            }
        }

        /// <summary>
        /// 取得提現歷史
        /// </summary>
        public async Task<ServiceResult<List<WithdrawalHistoryDto>>> GetWithdrawalHistoryAsync(int userId)
        {
            try
            {
                var withdrawals = await _context.WithdrawalRequests
                    .Where(wr => wr.UserId == userId)
                    .OrderByDescending(wr => wr.RequestedAt)
                    .Select(wr => new WithdrawalHistoryDto
                    {
                        Id = wr.Id,
                        Amount = wr.Amount,
                        Status = wr.Status,
                        RequestedAt = wr.RequestedAt,
                        ProcessedAt = wr.ProcessedAt
                    })
                    .ToListAsync();

                return ServiceResult<List<WithdrawalHistoryDto>>.SuccessResult(withdrawals, "成功取得提現歷史");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "取得用戶 {UserId} 提現歷史時發生錯誤", userId);
                return ServiceResult<List<WithdrawalHistoryDto>>.ServerErrorResult("取得提現歷史失敗");
            }
        }

        /// <summary>
        /// 取得銷售統計
        /// </summary>
        public async Task<ServiceResult<SalesStatisticsDto>> GetSalesStatisticsAsync(int userId, string period = "month")
        {
            try
            {
                var now = DateTime.UtcNow;
                DateTime startDate = period switch
                {
                    "week" => now.AddDays(-7),
                    "month" => now.AddMonths(-1),
                    "quarter" => now.AddMonths(-3),
                    "year" => now.AddYears(-1),
                    _ => now.AddMonths(-1)
                };

                var orders = await _context.SalesOrders
                    .Where(so => so.UserId == userId && so.CreatedAt >= startDate)
                    .ToListAsync();

                var statistics = new SalesStatisticsDto
                {
                    Period = period,
                    TotalOrders = orders.Count,
                    TotalRevenue = orders.Sum(o => o.TotalAmount),
                    AverageOrderValue = orders.Any() ? orders.Average(o => o.TotalAmount) : 0,
                    StartDate = startDate,
                    EndDate = now
                };

                return ServiceResult<SalesStatisticsDto>.SuccessResult(statistics, "成功取得銷售統計");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "取得用戶 {UserId} 銷售統計時發生錯誤", userId);
                return ServiceResult<SalesStatisticsDto>.ServerErrorResult("取得銷售統計失敗");
            }
        }

        /// <summary>
        /// 取得銷售排行榜
        /// </summary>
        public async Task<ServiceResult<List<SalesLeaderboardDto>>> GetSalesLeaderboardAsync(string period = "month", int limit = 100)
        {
            try
            {
                var now = DateTime.UtcNow;
                DateTime startDate = period switch
                {
                    "week" => now.AddDays(-7),
                    "month" => now.AddMonths(-1),
                    "quarter" => now.AddMonths(-3),
                    "year" => now.AddYears(-1),
                    _ => now.AddMonths(-1)
                };

                var leaderboard = await _context.SalesOrders
                    .Where(so => so.CreatedAt >= startDate)
                    .GroupBy(so => so.UserId)
                    .Select(g => new
                    {
                        UserId = g.Key,
                        TotalRevenue = g.Sum(so => so.TotalAmount),
                        OrderCount = g.Count()
                    })
                    .OrderByDescending(x => x.TotalRevenue)
                    .Take(limit)
                    .ToListAsync();

                var userIds = leaderboard.Select(x => x.UserId).ToList();
                var users = await _context.Users
                    .Where(u => userIds.Contains(u.Id))
                    .Select(u => new { u.Id, u.Username })
                    .ToDictionaryAsync(u => u.Id);

                var result = leaderboard.Select((item, index) => new SalesLeaderboardDto
                {
                    Rank = index + 1,
                    UserId = item.UserId,
                    Username = users.ContainsKey(item.UserId) ? users[item.UserId].Username : "未知用戶",
                    TotalRevenue = item.TotalRevenue,
                    OrderCount = item.OrderCount
                }).ToList();

                return ServiceResult<List<SalesLeaderboardDto>>.SuccessResult(result, "成功取得銷售排行榜");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "取得銷售排行榜時發生錯誤");
                return ServiceResult<List<SalesLeaderboardDto>>.ServerErrorResult("取得銷售排行榜失敗");
            }
        }

        /// <summary>
        /// 取得銷售指南
        /// </summary>
        public async Task<ServiceResult<List<SalesGuideDto>>> GetSalesGuideAsync()
        {
            try
            {
                var guides = new List<SalesGuideDto>
                {
                    new SalesGuideDto
                    {
                        Title = "如何開始銷售",
                        Content = "完成銷售權限申請後，您就可以開始上架商品進行銷售。",
                        Category = "getting_started"
                    },
                    new SalesGuideDto
                    {
                        Title = "商品上架指南",
                        Content = "詳細說明如何上架商品，包括商品描述、價格設定等。",
                        Category = "product_listing"
                    },
                    new SalesGuideDto
                    {
                        Title = "訂單處理流程",
                        Content = "了解訂單處理的完整流程，從接單到發貨。",
                        Category = "order_management"
                    }
                };

                return ServiceResult<List<SalesGuideDto>>.SuccessResult(guides, "成功取得銷售指南");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "取得銷售指南時發生錯誤");
                return ServiceResult<List<SalesGuideDto>>.ServerErrorResult("取得銷售指南失敗");
            }
        }

        /// <summary>
        /// 更新銀行帳戶資訊
        /// </summary>
        public async Task<ServiceResult<BankAccountDto>> UpdateBankAccountAsync(int userId, BankAccountUpdateDto request)
        {
            try
            {
                var permission = await _context.SalesPermissions
                    .FirstOrDefaultAsync(sp => sp.UserId == userId);

                if (permission == null)
                {
                    return ServiceResult<BankAccountDto>.NotFoundResult("未找到銷售權限");
                }

                permission.BankAccount = request.BankAccount;
                permission.UpdatedAt = DateTime.UtcNow;

                await _context.SaveChangesAsync();

                var result = new BankAccountDto
                {
                    UserId = userId,
                    BankAccount = permission.BankAccount,
                    UpdatedAt = permission.UpdatedAt.Value
                };

                return ServiceResult<BankAccountDto>.SuccessResult(result, "銀行帳戶資訊已更新");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "更新用戶 {UserId} 銀行帳戶資訊時發生錯誤", userId);
                return ServiceResult<BankAccountDto>.ServerErrorResult("更新銀行帳戶資訊失敗");
            }
        }

        /// <summary>
        /// 取得銀行帳戶資訊
        /// </summary>
        public async Task<ServiceResult<BankAccountDto>> GetBankAccountAsync(int userId)
        {
            try
            {
                var permission = await _context.SalesPermissions
                    .FirstOrDefaultAsync(sp => sp.UserId == userId);

                if (permission == null)
                {
                    return ServiceResult<BankAccountDto>.NotFoundResult("未找到銷售權限");
                }

                var result = new BankAccountDto
                {
                    UserId = userId,
                    BankAccount = permission.BankAccount,
                    UpdatedAt = permission.UpdatedAt
                };

                return ServiceResult<BankAccountDto>.SuccessResult(result, "成功取得銀行帳戶資訊");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "取得用戶 {UserId} 銀行帳戶資訊時發生錯誤", userId);
                return ServiceResult<BankAccountDto>.ServerErrorResult("取得銀行帳戶資訊失敗");
            }
        }

        // 以下為管理員或內部使用的方法

        /// <summary>
        /// 檢查銷售權限
        /// </summary>
        public async Task<ServiceResult<bool>> CheckSalesPermissionAsync(int userId)
        {
            try
            {
                var permission = await _context.SalesPermissions
                    .FirstOrDefaultAsync(sp => sp.UserId == userId);

                return ServiceResult<bool>.SuccessResult(permission?.Status == "approved", "權限檢查完成");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "檢查用戶 {UserId} 銷售權限時發生錯誤", userId);
                return ServiceResult<bool>.ServerErrorResult("權限檢查失敗");
            }
        }

        /// <summary>
        /// 取得銷售權限詳情
        /// </summary>
        public async Task<ServiceResult<SalesPermissionDetailsDto>> GetSalesPermissionDetailsAsync(int userId)
        {
            try
            {
                var permission = await _context.SalesPermissions
                    .FirstOrDefaultAsync(sp => sp.UserId == userId);

                if (permission == null)
                {
                    return ServiceResult<SalesPermissionDetailsDto>.NotFoundResult("未找到銷售權限");
                }

                var result = new SalesPermissionDetailsDto
                {
                    Id = permission.Id,
                    UserId = permission.UserId,
                    Status = permission.Status,
                    AppliedAt = permission.AppliedAt,
                    ApprovedAt = permission.ApprovedAt,
                    RejectedAt = permission.RejectedAt,
                    RejectionReason = permission.RejectionReason,
                    BusinessLicense = permission.BusinessLicense,
                    TaxId = permission.TaxId,
                    BankAccount = permission.BankAccount,
                    ContactPhone = permission.ContactPhone,
                    BusinessAddress = permission.BusinessAddress
                };

                return ServiceResult<SalesPermissionDetailsDto>.SuccessResult(result, "成功取得銷售權限詳情");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "取得用戶 {UserId} 銷售權限詳情時發生錯誤", userId);
                return ServiceResult<SalesPermissionDetailsDto>.ServerErrorResult("取得銷售權限詳情失敗");
            }
        }

        /// <summary>
        /// 取得銷售商品列表
        /// </summary>
        public async Task<ServiceResult<List<SalesProductDto>>> GetSalesProductsAsync(int userId)
        {
            try
            {
                var products = await _context.SalesProducts
                    .Where(sp => sp.UserId == userId)
                    .Select(sp => new SalesProductDto
                    {
                        Id = sp.Id,
                        Name = sp.Name,
                        Description = sp.Description,
                        Price = sp.Price,
                        Stock = sp.Stock,
                        Status = sp.Status,
                        CreatedAt = sp.CreatedAt
                    })
                    .ToListAsync();

                return ServiceResult<List<SalesProductDto>>.SuccessResult(products, "成功取得銷售商品列表");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "取得用戶 {UserId} 銷售商品列表時發生錯誤", userId);
                return ServiceResult<List<SalesProductDto>>.ServerErrorResult("取得銷售商品列表失敗");
            }
        }

        /// <summary>
        /// 取得銷售訂單列表
        /// </summary>
        public async Task<ServiceResult<List<SalesOrderDto>>> GetSalesOrdersAsync(int userId)
        {
            try
            {
                var orders = await _context.SalesOrders
                    .Where(so => so.UserId == userId)
                    .Select(so => new SalesOrderDto
                    {
                        Id = so.Id,
                        OrderNumber = so.OrderNumber,
                        TotalAmount = so.TotalAmount,
                        Status = so.Status,
                        CreatedAt = so.CreatedAt
                    })
                    .ToListAsync();

                return ServiceResult<List<SalesOrderDto>>.SuccessResult(orders, "成功取得銷售訂單列表");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "取得用戶 {UserId} 銷售訂單列表時發生錯誤", userId);
                return ServiceResult<List<SalesOrderDto>>.ServerErrorResult("取得銷售訂單列表失敗");
            }
        }

        /// <summary>
        /// 取得銷售報表
        /// </summary>
        public async Task<ServiceResult<SalesReportDto>> GetSalesReportAsync(int userId, DateTime startDate, DateTime endDate)
        {
            try
            {
                var orders = await _context.SalesOrders
                    .Where(so => so.UserId == userId && so.CreatedAt >= startDate && so.CreatedAt <= endDate)
                    .ToListAsync();

                var report = new SalesReportDto
                {
                    UserId = userId,
                    StartDate = startDate,
                    EndDate = endDate,
                    TotalOrders = orders.Count,
                    TotalRevenue = orders.Sum(o => o.TotalAmount),
                    AverageOrderValue = orders.Any() ? orders.Average(o => o.TotalAmount) : 0
                };

                return ServiceResult<SalesReportDto>.SuccessResult(report, "成功取得銷售報表");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "取得用戶 {UserId} 銷售報表時發生錯誤", userId);
                return ServiceResult<SalesReportDto>.ServerErrorResult("取得銷售報表失敗");
            }
        }

        /// <summary>
        /// 取得銷售分析
        /// </summary>
        public async Task<ServiceResult<SalesAnalyticsDto>> GetSalesAnalyticsAsync(int userId, string period = "month")
        {
            try
            {
                var now = DateTime.UtcNow;
                DateTime startDate = period switch
                {
                    "week" => now.AddDays(-7),
                    "month" => now.AddMonths(-1),
                    "quarter" => now.AddMonths(-3),
                    "year" => now.AddYears(-1),
                    _ => now.AddMonths(-1)
                };

                var analytics = new SalesAnalyticsDto
                {
                    UserId = userId,
                    Period = period,
                    StartDate = startDate,
                    EndDate = now,
                    GrowthRate = 0, // 需要計算
                    TopProducts = new List<string>(),
                    CustomerRetentionRate = 0
                };

                return ServiceResult<SalesAnalyticsDto>.SuccessResult(analytics, "成功取得銷售分析");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "取得用戶 {UserId} 銷售分析時發生錯誤", userId);
                return ServiceResult<SalesAnalyticsDto>.ServerErrorResult("取得銷售分析失敗");
            }
        }

        /// <summary>
        /// 取得銷售目標
        /// </summary>
        public async Task<ServiceResult<SalesTargetDto>> GetSalesTargetAsync(int userId)
        {
            try
            {
                var target = await _context.SalesTargets
                    .FirstOrDefaultAsync(st => st.UserId == userId);

                if (target == null)
                {
                    return ServiceResult<SalesTargetDto>.NotFoundResult("未設定銷售目標");
                }

                var result = new SalesTargetDto
                {
                    UserId = target.UserId,
                    MonthlyTarget = target.MonthlyTarget,
                    QuarterlyTarget = target.QuarterlyTarget,
                    YearlyTarget = target.YearlyTarget,
                    UpdatedAt = target.UpdatedAt
                };

                return ServiceResult<SalesTargetDto>.SuccessResult(result, "成功取得銷售目標");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "取得用戶 {UserId} 銷售目標時發生錯誤", userId);
                return ServiceResult<SalesTargetDto>.ServerErrorResult("取得銷售目標失敗");
            }
        }

        /// <summary>
        /// 設定銷售目標
        /// </summary>
        public async Task<ServiceResult<SalesTargetDto>> SetSalesTargetAsync(int userId, SalesTargetSetDto request)
        {
            try
            {
                var target = await _context.SalesTargets
                    .FirstOrDefaultAsync(st => st.UserId == userId);

                if (target == null)
                {
                    target = new SalesTarget
                    {
                        UserId = userId,
                        CreatedAt = DateTime.UtcNow
                    };
                    _context.SalesTargets.Add(target);
                }

                target.MonthlyTarget = request.MonthlyTarget;
                target.QuarterlyTarget = request.QuarterlyTarget;
                target.YearlyTarget = request.YearlyTarget;
                target.UpdatedAt = DateTime.UtcNow;

                await _context.SaveChangesAsync();

                var result = new SalesTargetDto
                {
                    UserId = target.UserId,
                    MonthlyTarget = target.MonthlyTarget,
                    QuarterlyTarget = target.QuarterlyTarget,
                    YearlyTarget = target.YearlyTarget,
                    UpdatedAt = target.UpdatedAt
                };

                return ServiceResult<SalesTargetDto>.SuccessResult(result, "銷售目標已設定");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "設定用戶 {UserId} 銷售目標時發生錯誤", userId);
                return ServiceResult<SalesTargetDto>.ServerErrorResult("設定銷售目標失敗");
            }
        }

        /// <summary>
        /// 取得銷售績效
        /// </summary>
        public async Task<ServiceResult<SalesPerformanceDto>> GetSalesPerformanceAsync(int userId)
        {
            try
            {
                var performance = new SalesPerformanceDto
                {
                    UserId = userId,
                    CurrentMonthSales = 0,
                    CurrentQuarterSales = 0,
                    CurrentYearSales = 0,
                    TargetAchievement = 0,
                    PerformanceRating = "A"
                };

                return ServiceResult<SalesPerformanceDto>.SuccessResult(performance, "成功取得銷售績效");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "取得用戶 {UserId} 銷售績效時發生錯誤", userId);
                return ServiceResult<SalesPerformanceDto>.ServerErrorResult("取得銷售績效失敗");
            }
        }
    }
}