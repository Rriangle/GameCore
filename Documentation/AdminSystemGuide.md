# GameCore 管理員系統完整指南

## 📋 系統概述

GameCore管理員系統是一個完整的後台管理平台，嚴格按照規格實現管理員認證、角色權限管理、使用者治理、內容管理、商務管理、系統監控等核心功能。系統設計旨在提供全面的業務管理能力，支援多層級權限控制、操作稽核、實時監控等功能，建立科學的管理員權限體系。

### 🎯 核心特色

- **多層級角色權限**: 支援6種核心權限的靈活組合配置
- **完整使用者治理**: 權限調整、點數管理、銷售錢包管理
- **全方位內容管理**: 論壇、貼文、商品、訂單的統一管理
- **實時系統監控**: 系統健康度、效能指標、服務狀態監控
- **操作稽核追蹤**: 完整記錄所有管理操作，支援多維度查詢
- **美觀管理介面**: 現代化響應式設計，支援桌面和行動裝置

## 🏗️ 系統架構

### 權限體系設計

```
ManagerData (管理員) ←→ ManagerRole (角色指派) ←→ ManagerRolePermission (權限定義)
     ↓                           ↓                            ↓
 基本資料管理               多對多關係表                 6大核心權限
 登入追蹤(Admins)           角色組合配置              細粒度權限控制
```

### 六大核心權限模組

1. **AdministratorPrivilegesManagement** - 管理者權限管理
2. **UserStatusManagement** - 使用者狀態管理  
3. **ShoppingPermissionManagement** - 商城權限管理
4. **MessagePermissionManagement** - 論壇權限管理
5. **SalesPermissionManagement** - 銷售權限管理
6. **CustomerService** - 客服權限管理

### 三層架構實現

```
┌─────────────────────┐
│   Presentation      │  ← AdminController, Admin Views, Dashboard
├─────────────────────┤
│   Business Logic    │  ← AdminService, AdminDTOs, Permission Validation
├─────────────────────┤
│   Data Access       │  ← Manager Entities, Role Entities, DbContext
└─────────────────────┘
```

### 核心元件

1. **AdminController**: RESTful API控制器，提供完整管理端點
2. **IAdminService**: 業務邏輯服務介面，定義完整管理功能契約
3. **AdminService**: 業務邏輯實現，包含所有管理相關功能
4. **AdminDTOs**: 資料傳輸物件，涵蓋所有管理操作的請求和回應
5. **Admin Views**: 管理界面，包含儀表板、使用者管理、權限設定
6. **Admin Entities**: 資料庫實體，對應管理相關資料表

## 📊 資料庫設計

### 核心資料表結構

#### ManagerData (管理員主表)
```sql
CREATE TABLE ManagerData (
    Manager_Id int IDENTITY(1,1) PRIMARY KEY,
    Manager_Name nvarchar(100) NULL,         -- 管理員姓名
    Manager_Account varchar(50) NULL,        -- 管理員帳號（唯一）
    Manager_Password nvarchar(255) NULL,     -- 管理員密碼（雜湊）
    Administrator_registration_date datetime2 NULL -- 註冊時間
);
```

#### ManagerRolePermission (角色權限定義表)
```sql
CREATE TABLE ManagerRolePermission (
    ManagerRole_Id int IDENTITY(1,1) PRIMARY KEY,
    role_name nvarchar(100) NOT NULL,        -- 角色名稱
    AdministratorPrivilegesManagement bit NULL, -- 管理者權限管理
    UserStatusManagement bit NULL,           -- 使用者狀態管理
    ShoppingPermissionManagement bit NULL,   -- 商城權限管理
    MessagePermissionManagement bit NULL,    -- 論壇權限管理
    SalesPermissionManagement bit NULL,      -- 銷售權限管理
    customer_service bit NULL                -- 客服權限管理
);
```

#### ManagerRole (管理員角色指派表)
```sql
CREATE TABLE ManagerRole (
    Manager_Id int NOT NULL,                 -- 管理員ID (FK)
    ManagerRole_Id int NOT NULL,            -- 角色ID (FK)
    ManagerRole nvarchar(100) NULL,         -- 角色名稱
    
    PRIMARY KEY (Manager_Id, ManagerRole_Id) -- 複合主鍵防重複
);
```

#### Admins (後台登入追蹤表)
```sql
CREATE TABLE Admins (
    manager_id int PRIMARY KEY,             -- 管理員ID (FK)
    last_login datetime2 NULL               -- 最後登入時間
);
```

#### Mutes (禁言項目表)
```sql
CREATE TABLE Mutes (
    mute_id int IDENTITY(1,1) PRIMARY KEY,
    mute_name nvarchar(100) NULL,           -- 禁言名稱
    created_at datetime2 NULL,              -- 建立時間
    is_active bit NOT NULL DEFAULT 1,       -- 是否啟用
    manager_id int NULL                     -- 設置者ID (FK)
);
```

#### Styles (樣式項目表)
```sql
CREATE TABLE Styles (
    style_id int IDENTITY(1,1) PRIMARY KEY,
    style_name nvarchar(100) NULL,          -- 樣式名稱
    effect_desc nvarchar(500) NULL,         -- 效果說明
    created_at datetime2 NULL,              -- 建立時間
    manager_id int NULL                     -- 設置者ID (FK)
);
```

### 重要設計原則

- **角色權限分離**: 權限定義與指派分離，支援靈活的權限組合
- **多對多關係**: 管理員可指派多個角色，角色可指派給多個管理員
- **操作稽核**: 所有重要操作均記錄到應用層審計日誌
- **登入追蹤**: Admins表專門追蹤管理員登入行為

## 👨‍💼 管理功能

### 管理員認證

#### 登入驗證與Token生成

```csharp
// 管理員登入
var loginDto = new ManagerLoginDto
{
    ManagerAccount = "admin001",
    ManagerPassword = "password123"
};

var result = await adminService.LoginAsync(loginDto);

// 登入成功返回
{
    "success": true,
    "data": {
        "token": "Bearer_1_20240824",
        "manager": {
            "managerId": 1,
            "managerName": "系統管理員",
            "managerAccount": "admin001"
        },
        "permissions": {
            "administratorPrivilegesManagement": true,
            "userStatusManagement": true,
            "shoppingPermissionManagement": true
        },
        "tokenExpiry": "2024-08-24T16:00:00Z"
    }
}
```

#### 權限驗證機制

```csharp
// 檢查管理員權限
var hasPermission = await adminService.CheckManagerPermissionAsync(
    managerId: 1, 
    requiredPermission: "UserStatusManagement"
);

// 控制器權限裝飾器
[Authorize(Policy = "UserStatusManagement")]
public async Task<IActionResult> UpdateUserRights(int id, UpdateUserRightsDto updateDto)
{
    // 僅具備使用者狀態管理權限的管理員可執行
}
```

### 角色權限管理

#### 角色定義與權限配置

```csharp
// 建立角色權限
var roleDto = new CreateManagerRolePermissionDto
{
    RoleName = "使用者管理員",
    UserStatusManagement = true,
    CustomerService = true,
    // 其他權限設為false
};

await adminService.CreateRolePermissionAsync(roleDto);

// 指派角色給管理員
await adminService.AssignRolesToManagerAsync(
    managerId: 2, 
    roleIds: new List<int> { 2, 5 } // 使用者管理員 + 客服專員
);
```

#### 權限繼承與合併

```csharp
// 權限摘要計算（多角色權限合併）
var permissions = await adminService.GetManagerPermissionsAsync(managerId: 2);

// 結果: 如果管理員擁有多個角色，權限採用OR邏輯合併
{
    "administratorPrivilegesManagement": false,
    "userStatusManagement": true,     // 來自使用者管理員角色
    "shoppingPermissionManagement": false,
    "messagePermissionManagement": false,
    "salesPermissionManagement": false,
    "customerService": true           // 來自客服專員角色
}
```

### 使用者治理

#### 使用者權限調整

```csharp
// 調整使用者權限 (停權示例)
var updateDto = new UpdateUserRightsDto
{
    AccountStatus = 0,      // 停權
    CommentPermission = 0,  // 禁言
    Reason = "違反社群規範，惡意洗版"
};

await adminService.UpdateUserRightsAsync(
    userId: 123, 
    updateDto: updateDto, 
    operatorId: 1
);

// 自動發送通知給目標使用者
// 自動記錄操作日誌
```

#### 使用者點數管理

```csharp
// 調整使用者點數
var adjustDto = new AdjustUserPointsDto
{
    Delta = 5000,           // 增加5000點
    Reason = "活動獎勵補發"
};

await adminService.AdjustUserPointsAsync(
    userId: 123, 
    adjustDto: adjustDto, 
    operatorId: 1
);

// 執行步驟:
// 1. 更新 User_wallet.User_Point
// 2. 發送 Notifications (action=points_adjustment)
// 3. 記錄操作日誌到應用層審計系統
```

### 內容管理

#### 論壇內容審核

```csharp
// 隱藏不當貼文
await adminService.UpdateForumContentStatusAsync(
    contentType: "thread_post",
    contentId: 12345,
    newStatus: "hidden",
    operatorId: 1,
    reason: "包含不當內容"
);

// 設定洞察貼文置頂
await adminService.SetInsightPostPinnedAsync(
    postId: 67890,
    isPinned: true,
    operatorId: 1
);
```

#### 商品與訂單管理

```csharp
// 審核玩家市場商品
await adminService.ReviewPlayerMarketProductAsync(
    productId: 54321,
    newStatus: "approved",
    operatorId: 1,
    reason: "審核通過"
);

// 更新訂單狀態
await adminService.UpdateOrderStatusAsync(
    orderId: 98765,
    newStatus: "shipped",
    operatorId: 1,
    note: "已發貨，預計3-5個工作日送達"
);
```

## 🔧 API 文件

### 核心API端點

#### 1. 管理員認證 API

```http
# 管理員登入
POST /api/admin/login
{
  "managerAccount": "admin001",
  "managerPassword": "password123"
}

# 取得當前管理員資訊
GET /api/admin/me
Authorization: Bearer {token}

# 管理員登出
POST /api/admin/logout
Authorization: Bearer {token}
```

#### 2. 管理員管理 API

```http
# 取得管理員列表
GET /api/admin/managers?page=1&pageSize=20&activeOnly=true
Authorization: Bearer {token}
Requires: AdministratorPrivilegesManagement

# 取得管理員詳細資訊
GET /api/admin/managers/{id}
Authorization: Bearer {token}
Requires: AdministratorPrivilegesManagement

# 建立管理員
POST /api/admin/managers
Authorization: Bearer {token}
Requires: AdministratorPrivilegesManagement
{
  "managerName": "新管理員",
  "managerAccount": "newadmin",
  "managerPassword": "password123",
  "roleIds": [2, 3]
}

# 更新管理員
PUT /api/admin/managers/{id}
Authorization: Bearer {token}
Requires: AdministratorPrivilegesManagement
{
  "managerName": "更新的管理員名稱",
  "isActive": true,
  "roleIds": [1, 2]
}
```

#### 3. 角色權限管理 API

```http
# 取得角色權限列表
GET /api/admin/roles
Authorization: Bearer {token}
Requires: AdministratorPrivilegesManagement

# 取得角色權限詳細資訊
GET /api/admin/roles/{id}
Authorization: Bearer {token}
Requires: AdministratorPrivilegesManagement

# 建立角色權限
POST /api/admin/roles
Authorization: Bearer {token}
Requires: AdministratorPrivilegesManagement
{
  "roleName": "內容管理員",
  "messagePermissionManagement": true,
  "customerService": true
}

# 更新角色權限
PUT /api/admin/roles/{id}
Authorization: Bearer {token}
Requires: AdministratorPrivilegesManagement
{
  "roleName": "內容管理員",
  "messagePermissionManagement": true,
  "customerService": true,
  "userStatusManagement": false
}
```

#### 4. 使用者治理 API

```http
# 取得使用者管理列表
GET /api/admin/users?page=1&pageSize=20&search=keyword&accountStatus=1
Authorization: Bearer {token}
Requires: UserStatusManagement

# 取得使用者詳細資訊
GET /api/admin/users/{id}
Authorization: Bearer {token}
Requires: UserStatusManagement

# 調整使用者權限
PUT /api/admin/users/{id}/rights
Authorization: Bearer {token}
Requires: UserStatusManagement
{
  "accountStatus": 0,
  "commentPermission": 0,
  "reason": "違反社群規範"
}

# 調整使用者點數
POST /api/admin/users/{id}/points/adjust
Authorization: Bearer {token}
Requires: UserStatusManagement
{
  "delta": 1000,
  "reason": "活動獎勵"
}

# 調整使用者銷售錢包
POST /api/admin/users/{id}/sales-wallet/adjust
Authorization: Bearer {token}
Requires: SalesPermissionManagement
{
  "delta": 500,
  "reason": "銷售獎金"
}
```

#### 5. 內容管理 API

```http
# 取得論壇內容審核列表
GET /api/admin/content/forum?page=1&pageSize=20&contentType=thread&status=reported
Authorization: Bearer {token}
Requires: MessagePermissionManagement

# 更新論壇內容狀態
PUT /api/admin/content/forum/{contentType}/{contentId}/status
Authorization: Bearer {token}
Requires: MessagePermissionManagement
{
  "newStatus": "hidden",
  "reason": "違反社群規範"
}

# 取得洞察貼文管理列表
GET /api/admin/content/insights?page=1&pageSize=20&status=draft
Authorization: Bearer {token}
Requires: MessagePermissionManagement

# 設定洞察貼文置頂
PUT /api/admin/content/insights/{postId}/pinned
Authorization: Bearer {token}
Requires: MessagePermissionManagement
{
  "isPinned": true
}
```

#### 6. 商務管理 API

```http
# 取得訂單管理列表
GET /api/admin/orders?page=1&pageSize=20&orderStatus=pending&paymentStatus=paid
Authorization: Bearer {token}
Requires: ShoppingPermissionManagement

# 更新訂單狀態
PUT /api/admin/orders/{orderId}/status
Authorization: Bearer {token}
Requires: ShoppingPermissionManagement
{
  "newStatus": "shipped",
  "note": "已發貨"
}

# 取得玩家市場商品審核列表
GET /api/admin/market/products?page=1&pageSize=20&status=pending
Authorization: Bearer {token}
Requires: SalesPermissionManagement

# 審核玩家市場商品
PUT /api/admin/market/products/{productId}/review
Authorization: Bearer {token}
Requires: SalesPermissionManagement
{
  "newStatus": "approved",
  "reason": "審核通過"
}
```

#### 7. 系統監控 API

```http
# 取得管理員儀表板
GET /api/admin/dashboard
Authorization: Bearer {token}

# 取得系統健康狀態
GET /api/admin/system/health
Authorization: Bearer {token}

# 取得使用者統計
GET /api/admin/statistics/users?startDate=2024-01-01&endDate=2024-01-31
Authorization: Bearer {token}

# 取得營收統計
GET /api/admin/statistics/revenue?startDate=2024-01-01&endDate=2024-01-31
Authorization: Bearer {token}
Requires: ShoppingPermissionManagement OR SalesPermissionManagement
```

#### 8. 操作日誌 API

```http
# 查詢操作日誌
POST /api/admin/logs/search
Authorization: Bearer {token}
Requires: AdministratorPrivilegesManagement
{
  "startTime": "2024-01-01T00:00:00Z",
  "endTime": "2024-01-31T23:59:59Z",
  "operationType": "UPDATE",
  "module": "User",
  "operatorId": 1,
  "page": 1,
  "pageSize": 50
}

# 取得操作統計
GET /api/admin/logs/statistics?startDate=2024-01-01&endDate=2024-01-31
Authorization: Bearer {token}
Requires: AdministratorPrivilegesManagement
```

## 🧪 測試指南

### 單元測試

```bash
# 執行所有管理員測試
dotnet test --filter "AdminControllerTests"

# 執行特定功能測試
dotnet test --filter "Login_ShouldReturnSuccess"
```

### 測試覆蓋範圍

- ✅ 管理員認證 (登入、權限驗證、Token管理)
- ✅ 角色權限管理 (角色建立、權限指派、權限合併)
- ✅ 使用者治理 (權限調整、點數管理、狀態變更)
- ✅ 內容管理 (論壇審核、貼文管理、狀態控制)
- ✅ 商務管理 (訂單處理、商品審核、狀態更新)
- ✅ 系統監控 (儀表板、健康檢查、統計報表)
- ✅ 操作稽核 (日誌記錄、查詢篩選、統計分析)
- ✅ 錯誤處理和邊界條件

### 測試資料

使用 `15-AdminSystemSeedData.sql` 生成完整測試資料，包含：

- 8個管理員 (超級管理員、使用者管理員、商城管理員等)
- 8個角色權限 (覆蓋6大核心權限的各種組合)
- 9個角色指派關係 (多角色組合測試)
- 8個登入追蹤記錄 (不同時間點的登入狀態)
- 11個禁言項目 (包含啟用和停用狀態)
- 10個樣式項目 (論壇和社群樣式管理)

## 🔍 疑難排解

### 常見問題

#### 1. 管理員無法登入
**問題**: 正確的帳號密碼卻無法登入
**解決**: 
- 檢查ManagerData表中的帳號是否存在
- 確認密碼雜湊方式是否正確
- 檢查Admins表是否有對應記錄

#### 2. 權限檢查失效
**問題**: 管理員無法執行應有權限的操作
**解決**: 
- 檢查ManagerRole表中的角色指派是否正確
- 確認ManagerRolePermission表中的權限設定
- 檢查JWT Token中是否包含正確的權限Claim

#### 3. 操作日誌缺失
**問題**: 重要操作沒有記錄到日誌
**解決**: 
- 確認LogOperationAsync方法是否正確調用
- 檢查應用層審計日誌配置
- 驗證操作結果是否正確傳遞

### 監控指標

- 管理員登入成功率和失敗原因分析
- 各權限模組的使用頻率統計
- 使用者治理操作的影響範圍追蹤
- 內容審核的處理效率和準確性
- 系統健康狀態的異常檢測和告警

## 📈 效能最佳化

### 資料庫最佳化

```sql
-- 建議的索引
CREATE INDEX IX_ManagerData_Account 
ON ManagerData (Manager_Account);

CREATE INDEX IX_ManagerRole_Manager 
ON ManagerRole (Manager_Id);

CREATE INDEX IX_ManagerRole_Role 
ON ManagerRole (ManagerRole_Id);

CREATE INDEX IX_Admins_LastLogin 
ON Admins (last_login DESC);

CREATE INDEX IX_Mutes_Active 
ON Mutes (is_active, manager_id);
```

### 快取策略

- 管理員權限資訊快取 (30分鐘)
- 角色權限定義快取 (2小時)
- 系統健康狀態快取 (5分鐘)
- 操作日誌統計快取 (15分鐘)

### 安全性強化

- 管理員密碼強度要求和定期更新
- 登入失敗次數限制和帳號鎖定機制
- 重要操作的二次確認和審批流程
- 敏感資料的加密存儲和傳輸

## 🚀 未來擴展

### 計劃功能

1. **多租戶支援**: 支援多個獨立的管理域
2. **工作流引擎**: 複雜業務流程的自動化處理
3. **智能推薦**: 基於行為分析的管理建議
4. **移動應用**: 專屬的管理員行動App
5. **API Gateway**: 統一的API管理和監控

---

*本文件最後更新: 2024年8月24日*
*版本: 1.0.0*
*維護者: GameCore開發團隊*