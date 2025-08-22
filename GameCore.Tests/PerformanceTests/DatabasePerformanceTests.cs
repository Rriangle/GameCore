using Xunit;
using FluentAssertions;
using System.Diagnostics;
using GameCore.Infrastructure.Repositories;
using GameCore.Tests.Infrastructure;
using GameCore.Infrastructure.Data;

namespace GameCore.Tests.PerformanceTests
{
    /// <summary>
    /// 資料庫效能測試
    /// 測試各種資料庫操作的效能表現
    /// </summary>
    public class DatabasePerformanceTests : IDisposable
    {
        private readonly GameCoreDbContext _context;
        private readonly UserRepository _userRepository;

        public DatabasePerformanceTests()
        {
            _context = TestDbContextFactory.CreateSqliteDatabase("performance_test.db");
            _userRepository = new UserRepository(_context);
            
            // 預先載入大量測試資料
            TestDataSeeder.SeedUsers(_context, 1000);
            TestDataSeeder.SeedGames(_context, 100);
            TestDataSeeder.SeedProducts(_context, 500);
            _context.SaveChanges();
        }

        public void Dispose()
        {
            TestDbContextFactory.CleanupDatabase(_context);
        }

        [Fact]
        public async Task UserRepository_批量查詢_應該在合理時間內完成()
        {
            // Arrange
            var stopwatch = new Stopwatch();
            const int iterations = 100;

            // Act
            stopwatch.Start();
            
            for (int i = 0; i < iterations; i++)
            {
                var users = await _userRepository.GetAllAsync();
                users.Should().NotBeEmpty();
            }
            
            stopwatch.Stop();

            // Assert
            var averageTime = stopwatch.ElapsedMilliseconds / (double)iterations;
            averageTime.Should().BeLessThan(100, "平均查詢時間應該小於 100ms");
            
            Console.WriteLine($"批量查詢平均時間: {averageTime:F2}ms");
        }

        [Fact]
        public async Task UserRepository_分頁查詢_應該有良好效能()
        {
            // Arrange
            var stopwatch = new Stopwatch();
            const int pageSize = 20;
            const int totalPages = 10;

            // Act
            stopwatch.Start();
            
            for (int page = 1; page <= totalPages; page++)
            {
                var result = await _userRepository.SearchUsersAsync("測試", page, pageSize);
                result.Should().NotBeNull();
            }
            
            stopwatch.Stop();

            // Assert
            var averageTime = stopwatch.ElapsedMilliseconds / (double)totalPages;
            averageTime.Should().BeLessThan(50, "分頁查詢平均時間應該小於 50ms");
            
            Console.WriteLine($"分頁查詢平均時間: {averageTime:F2}ms");
        }

        [Fact]
        public async Task UserRepository_並發讀取_應該支援多執行緒()
        {
            // Arrange
            const int concurrentTasks = 10;
            const int operationsPerTask = 20;
            var stopwatch = new Stopwatch();

            // Act
            stopwatch.Start();
            
            var tasks = Enumerable.Range(0, concurrentTasks)
                .Select(async taskId =>
                {
                    for (int i = 0; i < operationsPerTask; i++)
                    {
                        var userId = (taskId * operationsPerTask + i) % 100 + 1;
                        var user = await _userRepository.GetByIdAsync(userId);
                        // 不需要 Assert，只測試是否會出現並發問題
                    }
                });
            
            await Task.WhenAll(tasks);
            stopwatch.Stop();

            // Assert
            var totalOperations = concurrentTasks * operationsPerTask;
            var averageTime = stopwatch.ElapsedMilliseconds / (double)totalOperations;
            averageTime.Should().BeLessThan(10, "並發讀取平均時間應該小於 10ms");
            
            Console.WriteLine($"並發讀取 ({concurrentTasks} 執行緒, {totalOperations} 操作) 平均時間: {averageTime:F2}ms");
        }

        [Fact]
        public async Task UserRepository_大量插入_應該有合理效能()
        {
            // Arrange
            const int batchSize = 100;
            var stopwatch = new Stopwatch();

            // 創建測試使用者
            var testUsers = Enumerable.Range(1, batchSize)
                .Select(i => new GameCore.Core.Entities.User
                {
                    UserAccount = $"perftest_{i}_{Guid.NewGuid():N}",
                    UserName = $"效能測試使用者 {i}",
                    Email = $"perftest{i}@example.com",
                    UserLevel = 1,
                    Points = 0,
                    Experience = 0,
                    DisplayName = $"測試 {i}",
                    RegistrationTime = DateTime.UtcNow,
                    LastLoginTime = DateTime.UtcNow,
                    IsOnline = false,
                    Status = "Active"
                })
                .ToList();

            // Act
            stopwatch.Start();
            
            foreach (var user in testUsers)
            {
                await _userRepository.AddAsync(user);
            }
            await _context.SaveChangesAsync();
            
            stopwatch.Stop();

            // Assert
            var averageTime = stopwatch.ElapsedMilliseconds / (double)batchSize;
            averageTime.Should().BeLessThan(5, "大量插入平均時間應該小於 5ms");
            
            Console.WriteLine($"大量插入 ({batchSize} 筆) 平均時間: {averageTime:F2}ms");
        }

        [Fact]
        public async Task UserRepository_複雜查詢_應該在可接受範圍內()
        {
            // Arrange
            var stopwatch = new Stopwatch();
            const int iterations = 50;

            // Act
            stopwatch.Start();
            
            for (int i = 0; i < iterations; i++)
            {
                // 執行包含 JOIN 的複雜查詢
                var users = await _context.Users
                    .Where(u => u.Points > 100)
                    .OrderByDescending(u => u.Experience)
                    .Take(10)
                    .ToListAsync();
                
                users.Should().NotBeNull();
            }
            
            stopwatch.Stop();

            // Assert
            var averageTime = stopwatch.ElapsedMilliseconds / (double)iterations;
            averageTime.Should().BeLessThan(200, "複雜查詢平均時間應該小於 200ms");
            
            Console.WriteLine($"複雜查詢平均時間: {averageTime:F2}ms");
        }

        [Fact]
        public async Task 記憶體使用量_大量資料操作_應該在合理範圍內()
        {
            // Arrange
            var initialMemory = GC.GetTotalMemory(true);

            // Act
            for (int i = 0; i < 100; i++)
            {
                var users = await _userRepository.GetAllAsync();
                var count = users.Count();
                
                // 強制垃圾回收以獲得更準確的記憶體使用量
                if (i % 10 == 0)
                {
                    GC.Collect();
                    GC.WaitForPendingFinalizers();
                }
            }

            var finalMemory = GC.GetTotalMemory(true);

            // Assert
            var memoryIncrease = (finalMemory - initialMemory) / (1024 * 1024); // MB
            memoryIncrease.Should().BeLessThan(50, "記憶體增長應該小於 50MB");
            
            Console.WriteLine($"記憶體使用增長: {memoryIncrease:F2}MB");
        }

        [Theory]
        [InlineData(10)]
        [InlineData(50)]
        [InlineData(100)]
        [InlineData(500)]
        public async Task 分頁效能測試_不同頁面大小_應該有合理效能(int pageSize)
        {
            // Arrange
            var stopwatch = new Stopwatch();

            // Act
            stopwatch.Start();
            var result = await _userRepository.SearchUsersAsync("測試", 1, pageSize);
            stopwatch.Stop();

            // Assert
            result.Should().NotBeNull();
            stopwatch.ElapsedMilliseconds.Should().BeLessThan(1000, $"頁面大小 {pageSize} 的查詢時間應該小於 1000ms");
            
            Console.WriteLine($"頁面大小 {pageSize}: {stopwatch.ElapsedMilliseconds}ms, 結果數量: {result.Items.Count}");
        }
    }
}
