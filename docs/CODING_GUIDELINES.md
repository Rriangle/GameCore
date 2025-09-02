# GameCore 編碼規範

## 概述

本文檔定義了 GameCore 專案的編碼規範，確保代碼的一致性、可讀性和可維護性。所有開發人員必須遵循這些規範。

## 命名規範

### 1. 命名空間 (Namespace)
```csharp
// ✅ 正確
namespace GameCore.Domain.Entities
namespace GameCore.Application.Services
namespace GameCore.Infrastructure.Repositories

// ❌ 錯誤
namespace GameCore.domain.entities
namespace GameCore.Application.services
```

### 2. 類別 (Class)
```csharp
// ✅ 正確 - PascalCase
public class UserService
public class GameCoreDbContext
public class NotificationRepository

// ❌ 錯誤
public class userService
public class gameCoreDbContext
public class notification_repository
```

### 3. 介面 (Interface)
```csharp
// ✅ 正確 - 以 I 開頭
public interface IUserService
public interface IRepository<T>
public interface INotificationHandler

// ❌ 錯誤
public interface UserService
public interface Repository<T>
```

### 4. 方法 (Method)
```csharp
// ✅ 正確 - PascalCase
public async Task<Result<UserDto>> GetUserByIdAsync(int id)
public void UpdateUserStatus(UserStatus status)
public bool IsUserActive(int userId)

// ❌ 錯誤
public async Task<Result<UserDto>> get_user_by_id_async(int id)
public void updateUserStatus(UserStatus status)
```

### 5. 屬性 (Property)
```csharp
// ✅ 正確 - PascalCase
public string UserName { get; set; }
public int UserId { get; private set; }
public DateTime CreatedAt { get; init; }

// ❌ 錯誤
public string userName { get; set; }
public int user_id { get; set; }
```

### 6. 欄位 (Field)
```csharp
// ✅ 正確 - camelCase 或 _camelCase
private readonly IUserRepository _userRepository;
private string _connectionString;
private int maxRetryCount;

// ❌ 錯誤
private readonly IUserRepository UserRepository;
private string ConnectionString;
```

### 7. 常數 (Constant)
```csharp
// ✅ 正確 - PascalCase
public const int MaxRetryCount = 3;
public const string DefaultConnectionString = "Server=localhost;Database=GameCore;";
public static readonly TimeSpan DefaultTimeout = TimeSpan.FromSeconds(30);

// ❌ 錯誤
public const int maxRetryCount = 3;
public const string default_connection_string = "Server=localhost;";
```

### 8. 枚舉 (Enum)
```csharp
// ✅ 正確 - PascalCase
public enum UserStatus
{
    Active,
    Inactive,
    Suspended
}

public enum GameType
{
    MiniGame,
    CardGame,
    PuzzleGame
}

// ❌ 錯誤
public enum userStatus
{
    active,
    inactive
}
```

## 專案結構

### 1. 檔案組織
```
GameCore.Domain/
├── Entities/
│   ├── User.cs
│   ├── Pet.cs
│   └── Game.cs
├── ValueObjects/
│   ├── Email.cs
│   └── Money.cs
├── Enums/
│   ├── UserStatus.cs
│   └── GameType.cs
├── Events/
│   ├── UserCreatedEvent.cs
│   └── GameCompletedEvent.cs
├── Interfaces/
│   ├── IUserRepository.cs
│   └── IGameRepository.cs
└── Exceptions/
    ├── DomainException.cs
    └── ValidationException.cs
```

### 2. 檔案命名
```csharp
// ✅ 正確 - 與類別名稱相同
User.cs          // 包含 User 類別
UserService.cs   // 包含 UserService 類別
IUserRepository.cs // 包含 IUserRepository 介面

// ❌ 錯誤
user.cs
user_service.cs
IUserRepo.cs
```

### 3. 資料夾命名
```csharp
// ✅ 正確 - PascalCase
Controllers/
Repositories/
Services/
Validators/

// ❌ 錯誤
controllers/
repositories/
services/
```

## 例外處理規範

### 1. 自定義例外類別
```csharp
// ✅ 正確
public class DomainException : Exception
{
    public DomainException(string message) : base(message) { }
    public DomainException(string message, Exception innerException) 
        : base(message, innerException) { }
}

public class ValidationException : Exception
{
    public IReadOnlyList<ValidationError> Errors { get; }
    
    public ValidationException(IEnumerable<ValidationError> errors)
    {
        Errors = errors.ToList().AsReadOnly();
    }
}
```

### 2. 例外處理模式
```csharp
// ✅ 正確 - 使用 Result 模式
public async Task<Result<UserDto>> GetUserByIdAsync(int id)
{
    try
    {
        var user = await _userRepository.GetByIdAsync(id);
        if (user == null)
        {
            return Result<UserDto>.Failure("User not found");
        }
        
        return Result<UserDto>.Success(user.ToDto());
    }
    catch (Exception ex)
    {
        _logger.LogError(ex, "Error retrieving user with ID {UserId}", id);
        return Result<UserDto>.Failure("An error occurred while retrieving the user");
    }
}

// ❌ 錯誤 - 直接拋出例外
public async Task<UserDto> GetUserByIdAsync(int id)
{
    var user = await _userRepository.GetByIdAsync(id);
    if (user == null)
    {
        throw new UserNotFoundException($"User with ID {id} not found");
    }
    return user.ToDto();
}
```

### 3. 全域例外處理
```csharp
// ✅ 正確 - 在 Web 層實現
public class GlobalExceptionHandler : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        var (statusCode, message) = exception switch
        {
            ValidationException => (StatusCodes.Status400BadRequest, "Validation failed"),
            DomainException => (StatusCodes.Status400BadRequest, exception.Message),
            _ => (StatusCodes.Status500InternalServerError, "An unexpected error occurred")
        };

        httpContext.Response.StatusCode = statusCode;
        await httpContext.Response.WriteAsJsonAsync(new { error = message }, cancellationToken);
        return true;
    }
}
```

## DTO 寫法規範

### 1. DTO 命名
```csharp
// ✅ 正確 - 以 Dto 結尾
public class UserDto
{
    public int Id { get; set; }
    public string UserName { get; set; }
    public string Email { get; set; }
    public UserStatus Status { get; set; }
}

public class CreateUserDto
{
    public string UserName { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
}

public class UpdateUserDto
{
    public string UserName { get; set; }
    public string Email { get; set; }
}

// ❌ 錯誤
public class User
public class CreateUser
public class UserModel
```

### 2. DTO 驗證
```csharp
// ✅ 正確 - 使用 FluentValidation
public class CreateUserDtoValidator : AbstractValidator<CreateUserDto>
{
    public CreateUserDtoValidator()
    {
        RuleFor(x => x.UserName)
            .NotEmpty()
            .Length(3, 50)
            .Matches(@"^[a-zA-Z0-9_]+$")
            .WithMessage("Username must be 3-50 characters and contain only letters, numbers, and underscores");

        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress()
            .WithMessage("Please provide a valid email address");

        RuleFor(x => x.Password)
            .NotEmpty()
            .MinimumLength(8)
            .Matches(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)")
            .WithMessage("Password must be at least 8 characters and contain at least one uppercase letter, one lowercase letter, and one number");
    }
}
```

### 3. DTO 映射
```csharp
// ✅ 正確 - 使用 AutoMapper 或手動映射
public class UserMappingProfile : Profile
{
    public UserMappingProfile()
    {
        CreateMap<User, UserDto>();
        CreateMap<CreateUserDto, User>();
        CreateMap<UpdateUserDto, User>()
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
    }
}

// 手動映射
public static class UserExtensions
{
    public static UserDto ToDto(this User user)
    {
        return new UserDto
        {
            Id = user.Id,
            UserName = user.UserName,
            Email = user.Email,
            Status = user.Status
        };
    }
}
```

## Async/Await 規範

### 1. 非同步方法命名
```csharp
// ✅ 正確 - 以 Async 結尾
public async Task<Result<UserDto>> GetUserByIdAsync(int id)
public async Task<Result<IEnumerable<UserDto>>> GetAllUsersAsync()
public async Task<Result> CreateUserAsync(CreateUserDto dto)
public async Task<Result> UpdateUserAsync(int id, UpdateUserDto dto)
public async Task<Result> DeleteUserAsync(int id)

// ❌ 錯誤
public async Task<Result<UserDto>> GetUserById(int id)
public async Task<Result<IEnumerable<UserDto>>> GetAllUsers()
```

### 2. 非同步方法實現
```csharp
// ✅ 正確
public async Task<Result<UserDto>> GetUserByIdAsync(int id)
{
    var user = await _userRepository.GetByIdAsync(id);
    if (user == null)
    {
        return Result<UserDto>.Failure("User not found");
    }
    
    return Result<UserDto>.Success(user.ToDto());
}

// ❌ 錯誤 - 同步等待
public async Task<Result<UserDto>> GetUserByIdAsync(int id)
{
    var user = _userRepository.GetByIdAsync(id).Result; // 不要使用 .Result
    // ...
}
```

### 3. 非同步方法調用
```csharp
// ✅ 正確
public async Task<Result> ProcessUserAsync(int userId)
{
    var user = await _userRepository.GetByIdAsync(userId);
    await _notificationService.SendWelcomeEmailAsync(user.Email);
    await _auditService.LogUserActionAsync(userId, "User processed");
    
    return Result.Success();
}

// ❌ 錯誤 - 同步調用非同步方法
public async Task<Result> ProcessUserAsync(int userId)
{
    var user = _userRepository.GetByIdAsync(userId).Result;
    _notificationService.SendWelcomeEmailAsync(user.Email).Wait();
    // ...
}
```

### 4. 並行處理
```csharp
// ✅ 正確 - 使用 Task.WhenAll
public async Task<Result> ProcessMultipleUsersAsync(IEnumerable<int> userIds)
{
    var tasks = userIds.Select(id => ProcessUserAsync(id));
    var results = await Task.WhenAll(tasks);
    
    return results.All(r => r.IsSuccess) 
        ? Result.Success() 
        : Result.Failure("Some operations failed");
}

// ❌ 錯誤 - 順序處理
public async Task<Result> ProcessMultipleUsersAsync(IEnumerable<int> userIds)
{
    foreach (var userId in userIds)
    {
        await ProcessUserAsync(userId); // 效率較低
    }
    return Result.Success();
}
```

## 註解規範

### 1. XML 文檔註解
```csharp
// ✅ 正確
/// <summary>
/// 根據用戶 ID 獲取用戶資訊
/// </summary>
/// <param name="id">用戶唯一識別碼</param>
/// <returns>包含用戶資訊的結果物件</returns>
/// <exception cref="ArgumentException">當 id 小於等於 0 時拋出</exception>
public async Task<Result<UserDto>> GetUserByIdAsync(int id)
{
    // 實現...
}
```

### 2. 行內註解
```csharp
// ✅ 正確
public async Task<Result> ProcessPaymentAsync(PaymentDto payment)
{
    // 驗證支付金額
    if (payment.Amount <= 0)
    {
        return Result.Failure("Payment amount must be greater than zero");
    }

    // 檢查用戶餘額
    var userWallet = await _walletRepository.GetByUserIdAsync(payment.UserId);
    if (userWallet.Balance < payment.Amount)
    {
        return Result.Failure("Insufficient balance");
    }

    // 執行支付交易
    var transaction = new WalletTransaction
    {
        UserId = payment.UserId,
        Amount = -payment.Amount, // 負數表示支出
        Type = TransactionType.Payment,
        Description = payment.Description
    };

    await _walletRepository.AddTransactionAsync(transaction);
    return Result.Success();
}
```

## 代碼格式規範

### 1. 縮排
```csharp
// ✅ 正確 - 使用 4 個空格
public class UserService
{
    private readonly IUserRepository _userRepository;
    
    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }
    
    public async Task<Result<UserDto>> GetUserByIdAsync(int id)
    {
        var user = await _userRepository.GetByIdAsync(id);
        return user?.ToDto() ?? null;
    }
}
```

### 2. 空行使用
```csharp
// ✅ 正確 - 適當的空行分隔
public class UserService
{
    private readonly IUserRepository _userRepository;
    private readonly ILogger<UserService> _logger;

    public UserService(IUserRepository userRepository, ILogger<UserService> logger)
    {
        _userRepository = userRepository;
        _logger = logger;
    }

    public async Task<Result<UserDto>> GetUserByIdAsync(int id)
    {
        try
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
            {
                return Result<UserDto>.Failure("User not found");
            }

            return Result<UserDto>.Success(user.ToDto());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving user with ID {UserId}", id);
            return Result<UserDto>.Failure("An error occurred");
        }
    }
}
```

### 3. 行長度
```csharp
// ✅ 正確 - 保持行長度在 120 字元以內
public async Task<Result<UserDto>> GetUserByIdAsync(int id)
{
    var user = await _userRepository.GetByIdAsync(id);
    return user?.ToDto() ?? null;
}

// ❌ 錯誤 - 行太長
public async Task<Result<UserDto>> GetUserByIdAsync(int id) { var user = await _userRepository.GetByIdAsync(id); return user?.ToDto() ?? null; }
```

## 測試規範

### 1. 測試命名
```csharp
// ✅ 正確 - 描述性命名
[Test]
public async Task GetUserByIdAsync_WithValidId_ReturnsUser()
{
    // Arrange
    var userId = 1;
    var expectedUser = new User { Id = userId, UserName = "testuser" };
    _userRepository.Setup(x => x.GetByIdAsync(userId))
        .ReturnsAsync(expectedUser);

    // Act
    var result = await _userService.GetUserByIdAsync(userId);

    // Assert
    result.IsSuccess.Should().BeTrue();
    result.Value.UserName.Should().Be("testuser");
}

[Test]
public async Task GetUserByIdAsync_WithInvalidId_ReturnsFailure()
{
    // Arrange
    var userId = 999;
    _userRepository.Setup(x => x.GetByIdAsync(userId))
        .ReturnsAsync((User)null);

    // Act
    var result = await _userService.GetUserByIdAsync(userId);

    // Assert
    result.IsSuccess.Should().BeFalse();
    result.Error.Should().Be("User not found");
}
```

### 2. 測試結構
```csharp
// ✅ 正確 - AAA 模式
[Test]
public async Task CreateUserAsync_WithValidData_CreatesUserSuccessfully()
{
    // Arrange
    var createUserDto = new CreateUserDto
    {
        UserName = "newuser",
        Email = "newuser@example.com",
        Password = "SecurePass123!"
    };

    _userRepository.Setup(x => x.CreateAsync(It.IsAny<User>()))
        .ReturnsAsync(1);

    // Act
    var result = await _userService.CreateUserAsync(createUserDto);

    // Assert
    result.IsSuccess.Should().BeTrue();
    _userRepository.Verify(x => x.CreateAsync(It.IsAny<User>()), Times.Once);
}
```

## 性能考量

### 1. 資料庫查詢
```csharp
// ✅ 正確 - 使用 Include 避免 N+1 問題
public async Task<IEnumerable<UserDto>> GetUsersWithPetsAsync()
{
    var users = await _context.Users
        .Include(u => u.Pets)
        .ToListAsync();
    
    return users.Select(u => u.ToDto());
}

// ❌ 錯誤 - N+1 問題
public async Task<IEnumerable<UserDto>> GetUsersWithPetsAsync()
{
    var users = await _context.Users.ToListAsync();
    foreach (var user in users)
    {
        user.Pets = await _context.Pets.Where(p => p.UserId == user.Id).ToListAsync();
    }
    return users.Select(u => u.ToDto());
}
```

### 2. 記憶體管理
```csharp
// ✅ 正確 - 使用 using 語句
public async Task<Result> ProcessLargeDataSetAsync()
{
    using var stream = new MemoryStream();
    using var writer = new StreamWriter(stream);
    
    // 處理大量數據
    await writer.WriteAsync("Large dataset content");
    
    return Result.Success();
}
```

## 安全規範

### 1. 輸入驗證
```csharp
// ✅ 正確 - 嚴格驗證輸入
public async Task<Result> UpdateUserAsync(int id, UpdateUserDto dto)
{
    if (id <= 0)
    {
        return Result.Failure("Invalid user ID");
    }

    if (string.IsNullOrWhiteSpace(dto.UserName))
    {
        return Result.Failure("Username is required");
    }

    if (dto.UserName.Length > 50)
    {
        return Result.Failure("Username too long");
    }

    // 繼續處理...
}
```

### 2. SQL Injection 防護
```csharp
// ✅ 正確 - 使用參數化查詢
public async Task<User> GetUserByEmailAsync(string email)
{
    return await _context.Users
        .FirstOrDefaultAsync(u => u.Email == email);
}

// ❌ 錯誤 - 容易受到 SQL Injection 攻擊
public async Task<User> GetUserByEmailAsync(string email)
{
    var sql = $"SELECT * FROM Users WHERE Email = '{email}'";
    return await _context.Users.FromSqlRaw(sql).FirstOrDefaultAsync();
}
```

---

*本編碼規範將隨著專案發展持續更新和完善。所有開發人員必須嚴格遵循這些規範。* 