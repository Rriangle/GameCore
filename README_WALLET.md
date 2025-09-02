# GameCore 錢包系統使用說明

## 概述

GameCore 錢包系統提供完整的點數管理功能，包括主錢包、銷售錢包、點數流水記錄、銷售權限申請等功能。系統支援點數轉移、提領、統計分析等操作。

## 功能特性

### 核心功能
- ✅ 主錢包餘額查詢
- ✅ 銷售錢包管理
- ✅ 點數流水記錄查詢
- ✅ 銷售權限申請與審核
- ✅ 點數轉移與提領
- ✅ 銷售統計與排行榜
- ✅ 管理員點數調整

### 權限管理
- **一般用戶**：查看錢包餘額、申請銷售權限、轉移點數
- **銷售用戶**：管理銷售錢包、查看銷售統計
- **管理員**：審核銷售權限申請、調整用戶點數

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

### 3. 測試流程

1. 訪問首頁 `/`
2. 點擊「登入」按鈕
3. 使用測試帳號登入
4. 點擊「我的錢包」或訪問 `/wallet`
5. 測試各種錢包功能

## 錢包功能說明

### 1. 主錢包

主錢包是用戶的主要點數帳戶，用於：
- 日常消費（寵物換色、小遊戲等）
- 轉移到銷售錢包
- 接收各種獎勵點數

**查看餘額：**
```http
GET /api/wallet/balance
Authorization: Bearer {token}
```

**回應範例：**
```json
{
  "userId": 1,
  "userPoint": 1500,
  "couponNumber": null,
  "lastUpdated": "2025-01-15T10:30:00Z"
}
```

### 2. 銷售錢包

銷售錢包專門用於玩家市場交易：
- 接收銷售收入
- 支付平台抽成
- 提領到主錢包

**查看銷售錢包：**
```http
GET /api/wallet/sales
Authorization: Bearer {token}
```

**回應範例：**
```json
{
  "userId": 1,
  "userSalesWallet": 500,
  "hasSalesAuthority": true,
  "lastTransactionTime": "2025-01-15T09:00:00Z",
  "monthlySales": 2000,
  "totalSales": 15000
}
```

### 3. 點數流水記錄

查看點數變動歷史，支援多種篩選條件：

**查詢流水記錄：**
```http
GET /api/wallet/ledger?page=1&pageSize=20&fromDate=2025-01-01&toDate=2025-01-15&transactionType=signin
Authorization: Bearer {token}
```

**支援的交易類型：**
- `signin`：簽到獎勵
- `minigame`：小遊戲
- `pet_color_change`：寵物換色
- `adjustment`：管理員調整

**回應範例：**
```json
{
  "transactions": [
    {
      "transactionId": "signin_1",
      "transactionTime": "2025-01-15T08:00:00Z",
      "transactionType": "signin",
      "pointsChanged": 20,
      "balanceAfter": 1500,
      "description": "每日簽到獎勵",
      "metadata": "{\"LogID\":1,\"ExpGained\":0}"
    }
  ],
  "totalCount": 1,
  "totalPages": 1,
  "currentPage": 1,
  "pageSize": 20
}
```

### 4. 銷售權限申請

申請在玩家市場上架商品的權限：

**提交申請：**
```http
POST /api/wallet/sales/permission
Authorization: Bearer {token}
Content-Type: application/json

{
  "bankCode": 004,
  "bankAccountNumber": "1234567890",
  "accountCoverPhoto": "base64encodedimage",
  "applicationNote": "想要銷售遊戲道具"
}
```

**查看申請狀態：**
```http
GET /api/wallet/sales/permission/status
Authorization: Bearer {token}
```

### 5. 點數轉移與提領

**轉移到銷售錢包：**
```http
POST /api/wallet/sales/transfer
Authorization: Bearer {token}
Content-Type: application/json

{
  "amount": 100
}
```

**從銷售錢包提領：**
```http
POST /api/wallet/sales/withdraw
Authorization: Bearer {token}
Content-Type: application/json

{
  "amount": 50
}
```

### 6. 銷售統計

**取得銷售統計：**
```http
GET /api/sales/statistics
Authorization: Bearer {token}
```

**取得銷售排行榜：**
```http
GET /api/sales/ranking?period=monthly&limit=10
Authorization: Bearer {token}
```

**取得銷售報表：**
```http
GET /api/sales/report?startDate=2025-01-01&endDate=2025-01-15
Authorization: Bearer {token}
```

## 管理員功能

### 1. 審核銷售權限申請

**查看申請列表：**
```http
GET /api/admin/sales/permissions?status=pending&page=1&pageSize=20
Authorization: Bearer {admin_token}
```

**審核申請：**
```http
POST /api/admin/sales/permissions/review
Authorization: Bearer {admin_token}
Content-Type: application/json

{
  "applicationId": 1,
  "reviewResult": "approved",
  "reviewNote": "資料完整，通過審核",
  "managerId": 1
}
```

### 2. 調整用戶點數

**調整點數：**
```http
POST /api/admin/sales/wallet/adjust
Authorization: Bearer {admin_token}
Content-Type: application/json

{
  "userId": 1,
  "pointsDelta": 100,
  "reason": "活動獎勵",
  "managerId": 1
}
```

## 前端頁面

### 1. 錢包首頁
- **路徑：** `/wallet`
- **功能：** 顯示錢包概覽、快速操作、銷售統計圖表

### 2. 點數流水記錄
- **路徑：** `/wallet/ledger`
- **功能：** 查看點數變動歷史、篩選條件、分頁顯示

### 3. 銷售權限申請
- **路徑：** `/wallet/sales/permission`
- **功能：** 申請銷售權限、查看申請狀態

### 4. 銷售錢包管理
- **路徑：** `/wallet/sales`
- **功能：** 管理銷售錢包、查看銷售統計、轉移提領點數

## 資料庫結構

### 主要資料表

1. **UserWallet** - 用戶錢包
   - UserId (PK)
   - UserPoint (點數餘額)
   - CouponNumber (優惠券編號)

2. **UserSalesInformation** - 銷售錢包
   - UserId (PK)
   - UserSalesWallet (銷售錢包餘額)

3. **MemberSalesProfile** - 銷售權限申請
   - UserId (PK)
   - BankCode (銀行代號)
   - BankAccountNumber (銀行帳號)
   - AccountCoverPhoto (帳戶封面照片)

4. **UserRights** - 用戶權限
   - UserId (PK)
   - SalesAuthority (銷售權限)

### 關聯關係

- `UserWallet.UserId` → `Users.User_ID`
- `UserSalesInformation.UserId` → `Users.User_ID`
- `MemberSalesProfile.UserId` → `Users.User_ID`
- `UserRights.UserId` → `Users.User_ID`

## 測試

### 單元測試

```bash
cd GameCore.Tests
dotnet test --filter "FullyQualifiedName~WalletServiceTests"
```

### 整合測試

```bash
cd GameCore.Tests
dotnet test --filter "FullyQualifiedName~WalletApiTests"
```

## 錯誤處理

### 常見錯誤碼

- **400 Bad Request**：請求參數錯誤
- **401 Unauthorized**：未認證
- **403 Forbidden**：權限不足
- **404 Not Found**：資源不存在
- **500 Internal Server Error**：伺服器內部錯誤

### 錯誤回應格式

```json
{
  "error": "錯誤訊息描述"
}
```

## 安全性考量

1. **認證授權**：所有 API 都需要有效的 JWT Token
2. **權限檢查**：銷售相關操作需要銷售權限
3. **資料驗證**：所有輸入資料都經過驗證
4. **審計日誌**：重要操作都會記錄審計日誌
5. **點數安全**：點數操作使用資料庫交易確保一致性

## 效能優化

1. **資料庫索引**：在常用查詢欄位上建立索引
2. **分頁查詢**：大量資料使用分頁顯示
3. **快取機制**：頻繁查詢的資料使用快取
4. **非同步處理**：耗時操作使用非同步處理

## 部署注意事項

1. **資料庫連線**：確保資料庫連線字串正確
2. **檔案上傳**：設定適當的檔案上傳大小限制
3. **日誌記錄**：配置適當的日誌級別
4. **監控告警**：設定系統監控和告警機制

## 支援與聯絡

如果您在使用過程中遇到問題，請：

1. 查看系統日誌
2. 檢查資料庫連線
3. 確認權限設定
4. 聯絡系統管理員

---

**版本：** 1.0.0  
**更新日期：** 2025-01-15  
**維護者：** GameCore 開發團隊 