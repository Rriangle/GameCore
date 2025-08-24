# 列舉清單與被引用檔案清單

## 建立時間
2025-08-15 23:49 UTC

## 建立的列舉檔案
**GameCore.Core/Enums/CommonEnums.cs**

## 列舉清單

### 1. PostStatus (貼文狀態)
- **值**: Draft(0), Published(1), Hidden(2), Deleted(3)
- **被引用檔案**: 
  - `GameCore.Core/DTOs/CommonDTOs.cs` - ForumPostDto.Status
  - `GameCore.Core/Services/ForumService.cs` - 貼文狀態處理

### 2. PostReplyStatus (貼文回覆狀態)
- **值**: Normal(0), Hidden(1), Deleted(2)
- **被引用檔案**:
  - `GameCore.Core/DTOs/CommonDTOs.cs` - PostReplyDto.Status

### 3. OrderStatus (訂單狀態)
- **值**: Pending(0), Paid(1), Processing(2), Shipped(3), Completed(4), Cancelled(5), Refunded(6)
- **被引用檔案**:
  - `GameCore.Core/Interfaces/IStoreRepository.cs` - 訂單狀態查詢
  - `GameCore.Core/Services/StoreService.cs` - 訂單狀態處理

### 4. MarketItemStatus (市場商品狀態)
- **值**: Active(0), Sold(1), Inactive(2), Expired(3), Deleted(4)
- **被引用檔案**:
  - `GameCore.Core/Interfaces/IPlayerMarketRepository.cs` - 商品狀態查詢
  - `GameCore.Core/Services/PlayerMarketService.cs` - 商品狀態處理

### 5. TransactionStatus (交易狀態)
- **值**: Pending(0), Paid(1), Completed(2), Cancelled(3), Refunded(4)
- **被引用檔案**:
  - `GameCore.Core/Interfaces/IPlayerMarketRepository.cs` - 交易狀態查詢
  - `GameCore.Core/Services/PlayerMarketService.cs` - 交易狀態處理

### 6. NotificationType (通知類型)
- **值**: System(0), Game(1), Transaction(2), Social(3), Event(4)
- **被引用檔案**:
  - `GameCore.Core/Services/NotificationService.cs` - 通知類型處理

### 7. PetMood (寵物心情)
- **值**: VeryHappy(0), Happy(1), Normal(2), Unhappy(3), VeryUnhappy(4)
- **被引用檔案**:
  - `GameCore.Core/Services/PetService.cs` - 寵物心情處理

### 8. PetInteractionType (寵物互動類型)
- **值**: Feed(0), Bath(1), Play(2), Rest(3)
- **被引用檔案**:
  - `GameCore.Core/Services/PetService.cs` - 寵物互動處理

### 9. UserRole (用戶角色)
- **值**: User(0), VIP(1), Admin(2)
- **被引用檔案**:
  - `GameCore.Core/Entities/User.cs` - 用戶角色定義
  - `GameCore.Core/Services/UserService.cs` - 用戶角色處理

### 10. ManagerRole (管理員角色)
- **值**: Manager(0), SeniorManager(1), SuperAdmin(2)
- **被引用檔案**:
  - `GameCore.Core/Services/IManagerService.cs` - 管理員角色處理
  - `GameCore.Core/Services/ManagerService.cs` - 管理員角色處理

## 修正的檔案
1. **GameCore.Core/DTOs/CommonDTOs.cs** - 添加 `using GameCore.Core.Enums;`

## 預期效果
- 解決 CS0246 錯誤：找不到列舉類型
- 統一列舉定義和命名空間
- 為後續的實體類別建立提供基礎

## 下一步
建立缺少的實體類別（Manager, PlayerMarketItem, StoreProduct 等）