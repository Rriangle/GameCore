# GameCore 官方商城模組

## 概述

GameCore 官方商城是一個完整的電子商務系統，提供遊戲相關商品、硬體設備、配件周邊、收藏品、服飾和書籍等商品的線上購物體驗。系統採用 ASP.NET Core MVC 架構，支援響應式設計，提供完整的購物流程。

## 功能特色

### 🛍️ 商品管理
- **商品分類**：6大主要分類，包含遊戲相關、硬體設備、配件周邊、收藏品、服飾、書籍雜誌
- **商品搜尋**：支援關鍵字搜尋、分類篩選、價格範圍篩選
- **商品詳情**：完整的商品資訊、圖片展示、評價系統
- **相關商品推薦**：基於分類的智慧推薦系統

### 🛒 購物車系統
- **購物車管理**：添加、移除、更新商品數量
- **本地儲存**：使用 localStorage 保存購物車狀態
- **即時更新**：購物車數量即時顯示
- **清空購物車**：一鍵清空所有商品

### 📦 訂單管理
- **訂單建立**：完整的結帳流程
- **訂單追蹤**：訂單狀態管理（待處理、處理中、已出貨、已完成、已取消）
- **訂單歷史**：用戶訂單查詢與管理
- **訂單詳情**：完整的訂單資訊與商品清單

### 💳 結帳系統
- **多種付款方式**：信用卡、銀行轉帳、超商取貨付款、貨到付款、電子支付
- **地址管理**：收貨地址輸入與驗證
- **運費計算**：智慧運費計算（滿1000元免運費）
- **折扣系統**：支援優惠券與折扣碼

### ⭐ 評價系統
- **商品評價**：用戶購買後可對商品進行評價
- **評分系統**：1-5星評分機制
- **評價驗證**：驗證購買用戶的真實評價
- **評價管理**：評價內容審核與管理

### 📊 數據統計
- **熱門商品**：基於瀏覽量、銷量、收藏數的熱門商品排行
- **銷售統計**：商品銷售數據分析
- **用戶行為**：購物車轉換率、商品瀏覽統計

## 技術架構

### 後端架構
```
GameCore.Core/
├── Entities/          # 實體模型
│   ├── StoreProduct.cs
│   ├── StoreCategory.cs
│   ├── StoreCart.cs
│   ├── StoreOrder.cs
│   └── StoreProductReview.cs
├── DTOs/             # 資料傳輸物件
│   ├── ProductDto.cs
│   ├── CartDto.cs
│   ├── OrderDto.cs
│   └── CreateOrderRequestDto.cs
├── Services/         # 業務邏輯層
│   ├── IStoreService.cs
│   └── StoreService.cs
└── Repositories/     # 資料存取層
    ├── IStoreRepository.cs
    ├── ICartRepository.cs
    └── IOrderRepository.cs
```

### 前端架構
```
GameCore.Web/
├── Controllers/      # MVC 控制器
│   └── StoreMvcController.cs
├── Views/           # Razor 視圖
│   └── StoreMvc/
│       ├── Index.cshtml          # 商城首頁
│       ├── Product.cshtml        # 商品詳情
│       ├── Cart.cshtml          # 購物車
│       ├── Checkout.cshtml      # 結帳頁面
│       ├── OrderConfirmation.cshtml  # 訂單確認
│       ├── MyOrders.cshtml      # 我的訂單
│       ├── Category.cshtml      # 分類頁面
│       └── Search.cshtml        # 搜尋結果
├── wwwroot/         # 靜態資源
│   ├── css/
│   │   └── store.css
│   └── js/
│       └── store.js
└── Shared/          # 共享視圖
    └── _StoreLayout.cshtml
```

### 資料庫設計
```sql
-- 商品分類表
StoreCategories
- CategoryId (PK)
- Name
- Description
- Icon
- SortOrder
- IsActive
- CreatedAt
- UpdatedAt

-- 商品表
StoreProducts
- ProductId (PK)
- Name
- Description
- Price
- OriginalPrice
- StockQuantity
- Category
- ImageUrl
- IsActive
- IsFeatured
- Rating
- ReviewCount
- CreatedAt
- UpdatedAt

-- 購物車表
StoreCarts
- CartId (PK)
- UserId (FK)
- CreatedAt
- UpdatedAt

-- 購物車項目表
StoreCartItems
- CartItemId (PK)
- CartId (FK)
- ProductId (FK)
- Quantity
- CreatedAt
- UpdatedAt

-- 訂單表
StoreOrders
- OrderId (PK)
- UserId (FK)
- OrderNumber
- Status
- Subtotal
- ShippingFee
- DiscountAmount
- TotalAmount
- ShippingAddress
- PaymentMethod
- Notes
- CreatedAt
- UpdatedAt

-- 訂單項目表
StoreOrderItems
- OrderItemId (PK)
- OrderId (FK)
- ProductId (FK)
- ProductName
- ProductImageUrl
- UnitPrice
- Quantity
- TotalPrice
- CreatedAt
- UpdatedAt

-- 商品評價表
StoreProductReviews
- ReviewId (PK)
- ProductId (FK)
- UserId (FK)
- Rating
- Title
- Content
- IsVerified
- CreatedAt
- UpdatedAt

-- 熱門商品表
StorePopularProducts
- PopularProductId (PK)
- ProductId (FK)
- ViewCount
- SaleCount
- WishlistCount
- Rating
- ReviewCount
- LastUpdated
```

## 安裝與設定

### 1. 資料庫設定
```bash
# 更新資料庫
dotnet ef database update

# 執行種子資料
dotnet run --project GameCore.Web
```

### 2. 種子資料
系統會自動建立以下測試資料：
- **商品分類**：6個主要分類
- **商品資料**：70+ 個測試商品
- **購物車項目**：20個用戶的購物車資料
- **訂單資料**：100個測試訂單
- **商品評價**：每個商品5-19個評價
- **熱門商品統計**：前20個商品的統計資料

### 3. 環境變數
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=GameCoreDb;Trusted_Connection=true;MultipleActiveResultSets=true"
  },
  "StoreSettings": {
    "DefaultShippingFee": 100,
    "FreeShippingThreshold": 1000,
    "MaxCartItems": 99,
    "MaxProductQuantity": 99
  }
}
```

## 使用方式

### 1. 瀏覽商品
```
GET /StoreMvc                    # 商城首頁
GET /StoreMvc/Category/{category} # 分類頁面
GET /StoreMvc/Product/{id}       # 商品詳情
GET /StoreMvc/Search?q={query}   # 搜尋商品
```

### 2. 購物車操作
```
POST /api/Store/cart/add         # 加入購物車
PUT  /api/Store/cart/update      # 更新數量
DELETE /api/Store/cart/remove    # 移除商品
DELETE /api/Store/cart/clear     # 清空購物車
```

### 3. 訂單管理
```
POST /api/Store/orders/create    # 建立訂單
GET  /api/Store/orders/{id}      # 取得訂單詳情
GET  /api/Store/orders/user/{userId} # 取得用戶訂單
PUT  /api/Store/orders/{id}/cancel # 取消訂單
```

### 4. 商品評價
```
POST /api/Store/products/{id}/reviews # 新增評價
GET  /api/Store/products/{id}/reviews # 取得商品評價
```

## 前端功能

### 響應式設計
- **桌面版**：完整功能展示，多欄位佈局
- **平板版**：適中佈局，觸控友善
- **手機版**：單欄佈局，觸控優化

### 互動功能
- **購物車動畫**：加入購物車的視覺回饋
- **數量控制**：增減商品數量的直觀操作
- **圖片畫廊**：商品多角度展示
- **即時搜尋**：搜尋建議與自動完成
- **Toast 通知**：操作結果的即時回饋

### 用戶體驗
- **載入狀態**：操作進行中的視覺指示
- **錯誤處理**：友善的錯誤訊息與處理
- **表單驗證**：即時的表單驗證與提示
- **分頁導航**：大量資料的分頁瀏覽

## 測試

### 單元測試
```bash
# 執行所有測試
dotnet test

# 執行商城相關測試
dotnet test --filter "Category=Store"
```

### 測試覆蓋率
- **服務層測試**：100% 覆蓋率
- **控制器測試**：主要功能測試
- **整合測試**：API 端點測試
- **端對端測試**：完整購物流程測試

## 部署

### 1. 建置專案
```bash
dotnet build -c Release
```

### 2. 發佈
```bash
dotnet publish -c Release -o ./publish
```

### 3. 部署到 IIS
- 將發佈資料夾複製到 IIS 網站目錄
- 設定應用程式集區為 .NET Core
- 設定資料庫連接字串

### 4. 部署到 Azure
```bash
# 使用 Azure CLI 部署
az webapp deployment source config-zip --resource-group <resource-group> --name <app-name> --src <zip-file-path>
```

## 效能優化

### 資料庫優化
- **索引策略**：商品搜尋、分類查詢的索引優化
- **查詢優化**：使用 Entity Framework 的 Include 和 Select 優化
- **分頁查詢**：大量資料的分頁載入

### 快取策略
- **記憶體快取**：熱門商品、分類資料的快取
- **分散式快取**：Redis 快取支援
- **CDN 快取**：靜態資源的 CDN 快取

### 前端優化
- **圖片懶載入**：商品圖片的延遲載入
- **程式碼分割**：JavaScript 的動態載入
- **資源壓縮**：CSS、JavaScript 的壓縮與合併

## 安全性

### 身份驗證
- **JWT Token**：API 存取的身份驗證
- **角色權限**：用戶角色的權限控制
- **CSRF 保護**：表單提交的 CSRF 防護

### 資料驗證
- **輸入驗證**：所有用戶輸入的驗證
- **SQL 注入防護**：參數化查詢的使用
- **XSS 防護**：輸出編碼的實作

### 業務邏輯安全
- **庫存檢查**：訂單建立時的庫存驗證
- **價格驗證**：前後端價格的一致性檢查
- **訂單權限**：用戶只能存取自己的訂單

## 監控與日誌

### 日誌記錄
- **操作日誌**：用戶操作的詳細記錄
- **錯誤日誌**：系統錯誤的記錄與追蹤
- **效能日誌**：API 回應時間的監控

### 監控指標
- **系統健康**：資料庫連接、API 可用性
- **業務指標**：訂單轉換率、商品瀏覽量
- **效能指標**：回應時間、吞吐量

## 未來規劃

### 短期目標 (1-3個月)
- [ ] 商品收藏功能
- [ ] 優惠券系統
- [ ] 會員積分系統
- [ ] 商品比較功能

### 中期目標 (3-6個月)
- [ ] 推薦系統優化
- [ ] 庫存預警系統
- [ ] 銷售報表功能
- [ ] 多語言支援

### 長期目標 (6-12個月)
- [ ] AI 商品推薦
- [ ] 社交電商功能
- [ ] 直播帶貨功能
- [ ] 跨境電商支援

## 貢獻指南

### 開發環境設定
1. 安裝 .NET 8.0 SDK
2. 安裝 SQL Server 或 SQL Server Express
3. 複製專案到本地
4. 執行資料庫遷移
5. 執行種子資料

### 程式碼規範
- 使用 C# 命名慣例
- 添加適當的 XML 註解
- 遵循 SOLID 原則
- 撰寫單元測試

### 提交規範
- 使用清晰的提交訊息
- 包含相關的 Issue 編號
- 確保所有測試通過
- 更新相關文件

## 聯絡資訊

- **專案維護者**：GameCore 開發團隊
- **技術支援**：support@gamecore.com
- **問題回報**：GitHub Issues
- **功能建議**：GitHub Discussions

## 授權

本專案採用 MIT 授權條款，詳見 [LICENSE](LICENSE) 檔案。 