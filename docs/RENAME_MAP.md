# GameCore 重命名對照表

## 概述

本文檔記錄了 GameCore 專案架構重設過程中所有重命名的類型、命名空間、類別和方法，以及它們之間的映射關係。這有助於開發人員理解變更並進行相應的更新。

## 命名空間重命名

### 主要命名空間變更

| 舊命名空間 | 新命名空間 | 說明 |
|------------|------------|------|
| `GameCore.Core` | `GameCore.Domain` | 核心業務邏輯移至 Domain 層 |
| `GameCore.Core.Entities` | `GameCore.Domain.Entities` | 實體類別移至 Domain 層 |
| `GameCore.Core.Interfaces` | `GameCore.Domain.Interfaces` | 接口定義移至 Domain 層 |
| `GameCore.Core.Enums` | `GameCore.Domain.Enums` | 枚舉定義移至 Domain 層 |
| `GameCore.Core.ValueObjects` | `GameCore.Domain.ValueObjects` | 值對象移至 Domain 層 |
| `GameCore.Core.Exceptions` | `GameCore.Domain.Exceptions` | 異常類別移至 Domain 層 |

### 層級命名空間對應

```
舊結構:
GameCore.Core.* → 所有核心類型

新結構:
GameCore.Domain.* → 領域層類型
GameCore.Application.* → 應用層類型
GameCore.Infrastructure.* → 基礎設施層類型
GameCore.Web.* → 表現層類型
```

## 類型重命名

### 實體類型重命名

| 舊名稱 | 新名稱 | 說明 | 位置 |
|--------|--------|------|------|
| `User` | `User` | 用戶實體 | `GameCore.Domain.Entities` |
| `UserProfile` | `UserProfile` | 用戶檔案 | `GameCore.Domain.Entities` |
| `UserWallet` | `UserWallet` | 用戶錢包 | `GameCore.Domain.Entities` |
| `Pet` | `Pet` | 寵物實體 | `GameCore.Domain.Entities` |
| `ChatRoom` | `ChatRoom` | 聊天室 | `GameCore.Domain.Entities` |
| `ChatMessage` | `ChatMessage` | 聊天消息 | `GameCore.Domain.Entities` |
| `MiniGame` | `MiniGame` | 小遊戲 | `GameCore.Domain.Entities` |
| `MiniGameRecord` | `MiniGameRecord` | 遊戲記錄 | `GameCore.Domain.Entities` |
| `Store` | `Store` | 商店 | `GameCore.Domain.Entities` |
| `StoreProduct` | `StoreProduct` | 商店商品 | `GameCore.Domain.Entities` |
| `MarketItem` | `MarketItem` | 市場商品 | `GameCore.Domain.Entities` |
| `Notification` | `Notification` | 通知 | `GameCore.Domain.Entities` |
| `Manager` | `Manager` | 管理員 | `GameCore.Domain.Entities` |

### 接口類型重命名

| 舊名稱 | 新名稱 | 說明 | 位置 |
|--------|--------|------|------|
| `IRepository<T>` | `IRepository<T>` | 基礎倉庫接口 | `GameCore.Domain.Interfaces` |
| `IUserRepository` | `IUserRepository` | 用戶倉庫接口 | `GameCore.Domain.Interfaces` |
| `IPetRepository` | `IPetRepository` | 寵物倉庫接口 | `GameCore.Domain.Interfaces` |
| `IChatRepository` | `IChatRepository` | 聊天倉庫接口 | `GameCore.Domain.Interfaces` |
| `IMiniGameRepository` | `IMiniGameRepository` | 小遊戲倉庫接口 | `GameCore.Domain.Interfaces` |
| `IStoreRepository` | `IStoreRepository` | 商店倉庫接口 | `GameCore.Domain.Interfaces` |
| `IPlayerMarketRepository` | `IPlayerMarketRepository` | 玩家市場倉庫接口 | `GameCore.Domain.Interfaces` |
| `INotificationRepository` | `INotificationRepository` | 通知倉庫接口 | `GameCore.Domain.Interfaces` |
| `IManagerRepository` | `IManagerRepository` | 管理員倉庫接口 | `GameCore.Domain.Interfaces` |
| `IUnitOfWork` | `IUnitOfWork` | 工作單元接口 | `GameCore.Domain.Interfaces` |

### 服務接口重命名

| 舊名稱 | 新名稱 | 說明 | 位置 |
|--------|--------|------|------|
| `IAuthService` | `IAuthService` | 認證服務接口 | `GameCore.Application.Interfaces` |
| `IUserService` | `IUserService` | 用戶服務接口 | `GameCore.Application.Interfaces` |
| `IWalletService` | `IWalletService` | 錢包服務接口 | `GameCore.Application.Interfaces` |
| `IChatService` | `IChatService` | 聊天服務接口 | `GameCore.Application.Interfaces` |
| `IPetService` | `IPetService` | 寵物服務接口 | `GameCore.Application.Interfaces` |
| `IPlayerMarketService` | `IPlayerMarketService` | 玩家市場服務接口 | `GameCore.Application.Interfaces` |
| `IMiniGameService` | `IMiniGameService` | 小遊戲服務接口 | `GameCore.Application.Interfaces` |
| `IStoreService` | `IStoreService` | 商店服務接口 | `GameCore.Application.Interfaces` |
| `INotificationService` | `INotificationService` | 通知服務接口 | `GameCore.Application.Interfaces` |
| `ISalesService` | `ISalesService` | 銷售服務接口 | `GameCore.Application.Interfaces` |
| `IManagerService` | `IManagerService` | 管理員服務接口 | `GameCore.Application.Interfaces` |

### 枚舉類型重命名

| 舊名稱 | 新名稱 | 說明 | 位置 |
|--------|--------|------|------|
| `UserStatus` | `UserStatus` | 用戶狀態 | `GameCore.Domain.Enums` |
| `PetType` | `PetType` | 寵物類型 | `GameCore.Domain.Enums` |
| `ChatRoomType` | `ChatRoomType` | 聊天室類型 | `GameCore.Domain.Enums` |
| `GameType` | `GameType` | 遊戲類型 | `GameCore.Domain.Enums` |
| `TransactionType` | `TransactionType` | 交易類型 | `GameCore.Domain.Enums` |
| `NotificationType` | `NotificationType` | 通知類型 | `GameCore.Domain.Enums` |
| `ManagerRole` | `ManagerRole` | 管理員角色 | `GameCore.Domain.Enums` |
| `Permission` | `Permission` | 權限類型 | `GameCore.Domain.Enums` |

## 方法重命名

### 倉庫方法重命名

| 舊方法名 | 新方法名 | 說明 |
|----------|----------|------|
| `GetByIdAsync` | `GetAsync` | 統一命名規範 |
| `GetByEmailAsync` | `GetByEmailAsync` | 保持不變 |
| `GetByUserNameAsync` | `GetByUserNameAsync` | 保持不變 |
| `GetAllAsync` | `GetAllAsync` | 保持不變 |
| `AddAsync` | `AddAsync` | 保持不變 |
| `UpdateAsync` | `UpdateAsync` | 保持不變 |
| `DeleteAsync` | `DeleteAsync` | 保持不變 |
| `ExistsAsync` | `ExistsAsync` | 保持不變 |
| `CountAsync` | `CountAsync` | 保持不變 |

### 服務方法重命名

| 舊方法名 | 新方法名 | 說明 |
|----------|----------|------|
| `GetUser` | `GetUserAsync` | 添加 Async 後綴 |
| `CreateUser` | `CreateUserAsync` | 添加 Async 後綴 |
| `UpdateUser` | `UpdateUserAsync` | 添加 Async 後綴 |
| `DeleteUser` | `DeleteUserAsync` | 添加 Async 後綴 |
| `GetUsers` | `GetUsersAsync` | 添加 Async 後綴 |
| `GetUserByEmail` | `GetUserByEmailAsync` | 添加 Async 後綴 |
| `GetUserByUserName` | `GetUserByUserNameAsync` | 添加 Async 後綴 |

## DTO 重命名

### 請求 DTO 重命名

| 舊名稱 | 新名稱 | 說明 |
|--------|--------|------|
| `CreateUserDto` | `CreateUserRequest` | 統一使用 Request 後綴 |
| `UpdateUserDto` | `UpdateUserRequest` | 統一使用 Request 後綴 |
| `UserQueryDto` | `UserQueryParameters` | 統一使用 Parameters 後綴 |
| `LoginDto` | `LoginRequest` | 統一使用 Request 後綴 |
| `RegisterDto` | `RegisterRequest` | 統一使用 Request 後綴 |

### 響應 DTO 重命名

| 舊名稱 | 新名稱 | 說明 |
|--------|--------|------|
| `UserDto` | `UserResponse` | 統一使用 Response 後綴 |
| `UserListDto` | `UserListResponse` | 統一使用 Response 後綴 |
| `LoginResultDto` | `AuthResponse` | 統一使用 Response 後綴 |
| `RegisterResultDto` | `AuthResponse` | 統一使用 Response 後綴 |
| `PetDto` | `PetResponse` | 統一使用 Response 後綴 |

## 結果類型重命名

### 基礎結果類型

| 舊名稱 | 新名稱 | 說明 |
|--------|--------|------|
| `ApiResult<T>` | `Result<T>` | 簡化命名 |
| `ApiResponse<T>` | `Result<T>` | 統一使用 Result |
| `OperationResult` | `OperationResult` | 保持不變 |
| `ValidationResult` | `ValidationResult` | 保持不變 |

### 分頁結果類型

| 舊名稱 | 新名稱 | 說明 |
|--------|--------|------|
| `PagedList<T>` | `PagedResult<T>` | 統一使用 Result |
| `PaginatedResult<T>` | `PagedResult<T>` | 統一使用 Result |
| `PageResult<T>` | `PagedResult<T>` | 統一使用 Result |

## 異常類型重命名

### 領域異常

| 舊名稱 | 新名稱 | 說明 |
|--------|--------|------|
| `CoreException` | `DomainException` | 移至 Domain 層 |
| `BusinessException` | `BusinessException` | 保持不變 |
| `ValidationException` | `ValidationException` | 保持不變 |
| `NotFoundException` | `NotFoundException` | 保持不變 |

### 應用異常

| 舊名稱 | 新名稱 | 說明 |
|--------|--------|------|
| `ApplicationException` | `ApplicationException` | 保持不變 |
| `ServiceException` | `ServiceException` | 保持不變 |
| `RepositoryException` | `RepositoryException` | 保持不變 |

## 配置類型重命名

### 應用配置

| 舊名稱 | 新名稱 | 說明 |
|--------|--------|------|
| `AppSettings` | `AppSettings` | 保持不變 |
| `DatabaseSettings` | `DatabaseSettings` | 保持不變 |
| `JwtSettings` | `JwtSettings` | 保持不變 |
| `CacheSettings` | `CacheSettings` | 保持不變 |

## 遷移指南

### 1. 命名空間遷移

**舊代碼**:
```csharp
using GameCore.Core.Entities;
using GameCore.Core.Interfaces;
using GameCore.Core.Enums;
```

**新代碼**:
```csharp
using GameCore.Domain.Entities;
using GameCore.Domain.Interfaces;
using GameCore.Domain.Enums;
```

### 2. 類型引用遷移

**舊代碼**:
```csharp
public class UserService
{
    private readonly IUserRepository _userRepository;
    
    public async Task<User> GetUserAsync(int id)
    {
        return await _userRepository.GetByIdAsync(id);
    }
}
```

**新代碼**:
```csharp
public class UserService
{
    private readonly IUserRepository _userRepository;
    
    public async Task<User> GetUserAsync(int id)
    {
        return await _userRepository.GetAsync(id);
    }
}
```

### 3. DTO 遷移

**舊代碼**:
```csharp
public class CreateUserDto
{
    public string UserName { get; set; }
    public string Email { get; set; }
}
```

**新代碼**:
```csharp
public class CreateUserRequest
{
    public string UserName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
}
```

### 4. 結果類型遷移

**舊代碼**:
```csharp
public async Task<ApiResult<User>> GetUserAsync(int id)
{
    var user = await _userRepository.GetByIdAsync(id);
    return ApiResult<User>.Success(user);
}
```

**新代碼**:
```csharp
public async Task<Result<User>> GetUserAsync(int id)
{
    var user = await _userRepository.GetAsync(id);
    return Result<User>.Success(user);
}
```

## 自動化遷移工具

### 1. 批量重命名腳本

```bash
# PowerShell 腳本示例
Get-ChildItem -Recurse -Include "*.cs" | ForEach-Object {
    (Get-Content $_.FullName) | ForEach-Object {
        $_ -replace "GameCore\.Core", "GameCore.Domain" `
           -replace "GetByIdAsync", "GetAsync" `
           -replace "CreateUserDto", "CreateUserRequest" `
           -replace "UserDto", "UserResponse" `
           -replace "ApiResult", "Result"
    } | Set-Content $_.FullName
}
```

### 2. Visual Studio 重構工具

1. 使用 `Ctrl+Shift+H` 打開查找和替換
2. 選擇 "Replace in Files"
3. 設置搜索範圍為整個解決方案
4. 逐個執行重命名操作

### 3. ReSharper 重構

1. 右鍵點擊要重命名的類型
2. 選擇 "Rename"
3. 選擇 "Rename in Solution"
4. 確認所有引用都已更新

## 驗證清單

### 遷移完成後檢查

- [ ] 所有 `GameCore.Core` 引用已更新為 `GameCore.Domain`
- [ ] 所有方法名已添加 `Async` 後綴
- [ ] 所有 DTO 名稱已更新為 `Request`/`Response` 格式
- [ ] 所有結果類型已更新為 `Result<T>` 格式
- [ ] 解決方案可以成功編譯
- [ ] 所有測試可以通過
- [ ] 所有 API 端點可以正常響應

## 注意事項

### 1. 向後兼容性
- 重命名過程中保持 API 契約不變
- 數據庫結構保持不變
- 外部集成點保持不變

### 2. 測試覆蓋
- 重命名後立即運行所有測試
- 確保功能行為沒有改變
- 驗證所有集成點正常工作

### 3. 文檔更新
- 更新所有相關文檔
- 更新 API 文檔
- 更新開發指南

### 4. 團隊協調
- 通知所有團隊成員重命名變更
- 協調重命名時間，避免衝突
- 提供遷移指南和培訓

## 總結

本重命名對照表提供了 GameCore 專案架構重設過程中的完整遷移指南。通過系統性的重命名，我們可以：

1. **提高一致性**: 統一的命名規範
2. **改善可讀性**: 更清晰的類型和方法名稱
3. **增強可維護性**: 更好的代碼組織結構
4. **便於理解**: 符合 Clean Architecture 原則的命名

記住：重命名是一個重要但風險較低的變更，關鍵是要確保所有引用都已正確更新，並且功能行為保持不變。 

## 相容性屬性（暫時適配）

| 舊名稱 | 新名稱 | 類型 | 備註 |
|--------|--------|------|------|
| `User.UserAccount` | `User.User_Account` | 屬性 | 以 NotMapped 提供別名 |
| `User.UserName` | `User.User_name` | 屬性 | 以 NotMapped 提供別名 |
| `User.User_CreatedAt` | `User.CreatedAt` | 屬性 | 以 NotMapped 提供別名 |
| `UserRights.UserId` | `UserRights.User_Id` | 屬性 | 以 NotMapped 提供別名 |
| `UserWallet.UserId` | `UserWallet.User_Id` | 屬性 | 以 NotMapped 提供別名 |
| `UserWallet.UserPoint` | `UserWallet.User_Point` | 屬性 | 以 NotMapped 提供別名 |
| `UserIntroduce.UserId` | `UserIntroduce.User_ID` | 屬性 | 以 NotMapped 提供別名 |
| `UserIntroduce.UserNickName` | `UserIntroduce.User_NickName` | 屬性 | 以 NotMapped 提供別名 |
| `UserIntroduce.CreateAccount` | `UserIntroduce.Create_Account` | 屬性 | 以 NotMapped 提供別名 |
| `ChatMessage.SentAt` | `ChatMessage.CreatedAt` | 屬性 | 以 NotMapped 提供別名 |
| `ChatMessage.ReceiverId` | N/A | 屬性 | 暫以 NotMapped 提供，Repository 需後續對應 |
| `ChatMessage.SenderUser` | `ChatMessage.Sender` | 導航 | 以 NotMapped 別名 |
| `ChatMessage.ReceiverUser` | N/A | 導航 | 暫以 NotMapped 提供 |
| `Forum.Category` | N/A | 屬性 | 暫以 NotMapped 提供 |
| `Post.Id` | `Post.PostId` | 屬性 | 以 NotMapped 提供別名 |
| `Post.Likes` | `Post.LikeCount` | 屬性 | 以 NotMapped 提供別名 |
| `Post.Status` | `PostStatus` | 枚舉 | 以 NotMapped 別名 + `PostStatus.Active` 別名 |
| `Post.LastActivityAt` | `Post.UpdatedAt/LastReplyAt` | 屬性 | 以 NotMapped 提供別名 |
| `Post.PostMetricSnapshot` | `Post.MetricSnapshots` | 導航 | 暫以 NotMapped 提供單一別名 |
| `PostReply.Id` | `PostReply.ReplyId` | 屬性 | 以 NotMapped 提供別名 |
| `PostReply.Status` | N/A | 屬性 | 暫以 NotMapped 提供 |
| `PostReply.LastActivityAt` | `PostReply.UpdatedAt` | 屬性 | 以 NotMapped 提供別名 |
| `PostReply.Path` | N/A | 屬性 | 暫以 NotMapped 提供 |
| `Pet.OwnerId` | `Pet.UserId` | 屬性 | 以 NotMapped 提供別名 |
| `Pet.Owner` | `User` | 導航 | 暫以 NotMapped 提供 |
| `Pet.IsActive` | N/A | 屬性 | 暫以 NotMapped 提供 |
| `Pet.UpdatedAt` | N/A | 屬性 | 暫以 NotMapped 提供 |
| `PlayerMarketProductInfo.PStatus` | `PlayerMarketProductInfo.Status` | 屬性 | 以 NotMapped 提供別名 |
| `MarketItem.Category` | `MarketItem.PProductType` | 屬性 | 以 NotMapped 提供別名 |
| `MarketItem.Status` | `MarketItem.PStatus` | 屬性 | 以 NotMapped 提供別名 |
| `Notification.Content` | `Notification.Message` | 屬性 | 以 NotMapped 提供別名 |
| `Notification.CreateTime` | `Notification.CreatedAt` | 屬性 | 以 NotMapped 提供別名 |
| `Notification.ReadTime` | `Notification.ReadAt` | 屬性 | 以 NotMapped 提供別名 |
| `NotificationSource.Name` | `NotificationSource.ActionName` | 屬性 | 以 NotMapped 提供別名 |
| `NotificationAction.Name` | `NotificationAction.ActionName` | 屬性 | 以 NotMapped 提供別名 | 