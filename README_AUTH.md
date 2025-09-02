# GameCore 認證系統使用說明

## 概述

GameCore 認證系統提供完整的用戶認證功能，包括註冊、登入、登出、密碼管理等功能。系統採用 JWT Token 和 Cookie 認證機制，支援多種認證方式。

## 功能特性

### 核心功能
- ✅ 用戶註冊
- ✅ 用戶登入
- ✅ 用戶登出
- ✅ 密碼雜湊 (SHA256)
- ✅ JWT Token 認證
- ✅ Cookie 認證
- ✅ 認證狀態檢查
- ✅ 路由保護

### 進階功能
- ✅ 忘記密碼
- ✅ 重設密碼
- ✅ 變更密碼
- ✅ 個人資料管理
- ✅ 用戶權限管理
- ✅ 管理員認證

## 快速開始

### 1. 啟動專案

```bash
cd GameCore.Web
dotnet run
```

### 2. 測試帳號

系統預設建立以下測試帳號：

#### 測試用戶 001
- **Email:** test001@example.com
- **密碼:** password123
- **權限:** 一般用戶

#### 測試用戶 002
- **Email:** test002@example.com
- **密碼:** password123
- **權限:** 一般用戶

#### 管理員帳號
- **Username:** admin
- **密碼:** admin123
- **權限:** 超級管理員

- **Username:** moderator
- **密碼:** mod123
- **權限:** 內容審核員

### 3. 測試流程

1. 訪問首頁 `/`
2. 點擊「登入」按鈕
3. 使用測試帳號登入
4. 測試各種功能

## API 端點

### 認證 API

| 方法 | 端點 | 描述 |
|------|------|------|
| POST | `/api/auth/register` | 用戶註冊 |
| POST | `/api/auth/login` | 用戶登入 |
| POST | `/api/auth/logout` | 用戶登出 |
| GET | `/api/auth/status` | 檢查認證狀態 |
| POST | `/api/auth/forgot-password` | 忘記密碼 |
| POST | `/api/auth/reset-password` | 重設密碼 |
| POST | `/api/auth/change-password` | 變更密碼 |

### 頁面路由

| 路由 | 控制器 | 描述 |
|------|--------|------|
| `/login` | AuthMvcController | 登入頁面 |
| `/register` | AuthMvcController | 註冊頁面 |
| `/logout` | AuthMvcController | 登出 |
| `/profile` | AuthMvcController | 個人資料 |
| `/change-password` | AuthMvcController | 變更密碼 |
| `/forgot-password` | AuthMvcController | 忘記密碼 |
| `/reset-password` | AuthMvcController | 重設密碼 |

## 資料庫結構

### 主要資料表

#### Users (用戶基本資料)
- UserId: 用戶唯一識別碼
- UserName: 用戶名稱
- Email: 電子郵件
- PasswordHash: 密碼雜湊
- PhoneNumber: 電話號碼
- Gender: 性別
- Birthday: 生日
- RegistrationDate: 註冊日期
- LastLoginDate: 最後登入日期
- IsActive: 是否啟用
- IsEmailVerified: 郵件是否驗證
- IsPhoneVerified: 電話是否驗證

#### UserIntroduces (用戶介紹)
- UserId: 用戶ID
- SelfIntroduction: 自我介紹
- Interests: 興趣
- Hobbies: 嗜好
- FavoriteGames: 喜愛的遊戲
- GamingExperience: 遊戲經驗
- PreferredGameTypes: 偏好的遊戲類型
- GamingGoals: 遊戲目標

#### UserRights (用戶權限)
- UserId: 用戶ID
- CanPost: 可以發文
- CanComment: 可以評論
- CanUpload: 可以上傳
- CanDownload: 可以下載
- CanCreateGame: 可以建立遊戲
- CanModerate: 可以審核
- CanAccessAdminPanel: 可以存取管理面板

#### UserWallets (用戶錢包)
- UserId: 用戶ID
- Balance: 餘額
- TotalEarned: 總收入
- TotalSpent: 總支出
- Currency: 貨幣
- PaymentMethod: 付款方式

#### ManagerData (管理員資料)
- ManagerId: 管理員ID
- Username: 用戶名
- PasswordHash: 密碼雜湊
- Email: 電子郵件
- FullName: 全名
- Role: 角色
- IsActive: 是否啟用

## 認證流程

### 1. 用戶註冊

```http
POST /api/auth/register
Content-Type: application/json

{
  "userName": "新用戶",
  "email": "newuser@example.com",
  "password": "password123",
  "phoneNumber": "0912345678",
  "gender": "男",
  "birthday": "1990-01-01"
}
```

### 2. 用戶登入

```http
POST /api/auth/login
Content-Type: application/json

{
  "email": "test001@example.com",
  "password": "password123"
}
```

### 3. 檢查認證狀態

```http
GET /api/auth/status
Authorization: Bearer {token}
```

### 4. 用戶登出

```http
POST /api/auth/logout
Authorization: Bearer {token}
```

## 安全性

### 密碼安全
- 使用 SHA256 雜湊演算法
- 不儲存明文密碼
- 支援密碼複雜度驗證

### Token 安全
- JWT Token 過期時間設定
- 支援 Token 重新整理
- 安全的 Cookie 設定

### 權限控制
- 基於角色的權限控制 (RBAC)
- 細粒度的功能權限
- 路由級別的權限保護

## 開發指南

### 新增認證需求

1. 在 `IAuthService` 中定義介面
2. 在 `AuthService` 中實作邏輯
3. 在 `AuthController` 中新增 API 端點
4. 在 `AuthMvcController` 中新增頁面路由
5. 建立對應的視圖檔案

### 權限檢查

```csharp
[Authorize] // 需要登入
[Authorize(Roles = "admin")] // 需要管理員權限
[AllowAnonymous] // 允許匿名存取
```

### 自訂認證

```csharp
// 在 Program.cs 中設定
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
});
```

## 故障排除

### 常見問題

#### 1. 登入失敗
- 檢查 Email 和密碼是否正確
- 確認用戶帳號是否啟用
- 檢查資料庫連線

#### 2. Token 過期
- 重新登入取得新 Token
- 檢查 Token 過期時間設定
- 實作 Token 重新整理機制

#### 3. 權限不足
- 檢查用戶角色設定
- 確認功能權限配置
- 查看權限檢查邏輯

### 日誌記錄

系統會記錄以下資訊：
- 認證嘗試
- 登入/登出事件
- 權限檢查結果
- 錯誤和異常

## 更新日誌

### v1.0.0 (2024-01-XX)
- ✅ 基本認證功能
- ✅ 用戶註冊和登入
- ✅ JWT Token 認證
- ✅ 權限管理系統
- ✅ 測試資料初始化

## 聯絡資訊

如有問題或建議，請聯絡開發團隊。

---

**注意：** 這是一個開發版本的認證系統，請勿在生產環境中使用。生產環境請實作更安全的認證機制。 