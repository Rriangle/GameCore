# GameCore 官方商城系統完整指南

## 📋 系統概述

GameCore官方商城系統是一個完整的B2C電商平台，嚴格按照規格實現商品瀏覽、購物車管理、訂單流程、排行榜分析等核心電商功能。系統設計旨在提供豐富的購物體驗，支援多種商品類型，整合完整的訂單狀態機，確保交易安全和用戶滿意度。

### 🎯 核心特色

- **多樣商品類型**: 支援遊戲、周邊、點數卡、收藏品等多種商品分類
- **完整購物流程**: 商品瀏覽 → 購物車管理 → 訂單建立 → 付款處理 → 出貨完成
- **智能排行榜**: 依日/月/季/年自動計算商品交易額、交易量排名
- **庫存管理**: 即時庫存檢查、原子性扣減、超賣防護
- **狀態機管理**: 嚴格的訂單和付款狀態轉換控制
- **權限控制**: 使用者購物權限檢查和管理員功能分離

## 🏗️ 系統架構

### 三層架構設計

```
┌─────────────────────┐
│   Presentation      │  ← StoreController, Store Views
├─────────────────────┤
│   Business Logic    │  ← StoreService, StoreDTOs
├─────────────────────┤
│   Data Access       │  ← Store Entities, DbContext
└─────────────────────┘
```

### 核心元件

1. **StoreController**: RESTful API控制器，提供完整商城管理端點
2. **StoreService**: 業務邏輯服務，實現所有商城相關功能
3. **StoreDTOs**: 資料傳輸物件，涵蓋所有商城操作的請求和回應
4. **Store Views**: 商城界面，包含商品展示和購物流程
5. **Store Entities**: 資料庫實體，對應商城相關資料表

## 📊 資料庫設計

### 核心資料表結構

#### Supplier (供應商表)
```sql
CREATE TABLE Supplier (
    supplier_id int IDENTITY(1,1) PRIMARY KEY,
    supplier_name nvarchar(200) NULL
);
```

#### ProductInfo (商品資訊表)
```sql
CREATE TABLE ProductInfo (
    product_id int IDENTITY(1,1) PRIMARY KEY,
    product_name nvarchar(200) NULL,
    product_type nvarchar(50) NULL,
    price decimal(10,2) NOT NULL,
    currency_code nvarchar(10) NULL,
    Shipment_Quantity int NULL,           -- 庫存數量
    product_created_by nvarchar(100) NULL,
    product_created_at datetime2 NULL,
    product_updated_by nvarchar(100) NULL,
    product_updated_at datetime2 NULL,
    user_id int NULL
);
```

#### GameProductDetails (遊戲商品詳細)
```sql
CREATE TABLE GameProductDetails (
    product_id int PRIMARY KEY,           -- FK to ProductInfo
    product_name nvarchar(200) NULL,
    product_description nvarchar(max) NULL,
    supplier_id int NULL,                 -- FK to Supplier
    platform_id int NULL,
    game_id int NULL,
    download_link nvarchar(500) NULL
);
```

#### OtherProductDetails (其他商品詳細)
```sql
CREATE TABLE OtherProductDetails (
    product_id int PRIMARY KEY,           -- FK to ProductInfo
    product_name nvarchar(200) NULL,
    product_description nvarchar(max) NULL,
    supplier_id int NULL,                 -- FK to Supplier
    platform_id int NULL,
    digital_code nvarchar(200) NULL,      -- 數位序號
    size nvarchar(50) NULL,               -- 尺寸
    color nvarchar(50) NULL,              -- 顏色
    material nvarchar(100) NULL,          -- 材質
    stock_quantity nvarchar(50) NULL
);
```

#### OrderInfo (訂單資訊表)
```sql
CREATE TABLE OrderInfo (
    order_id int IDENTITY(1,1) PRIMARY KEY,
    user_id int NOT NULL,                 -- FK to Users
    order_date datetime2 NOT NULL,
    order_status nvarchar(50) NOT NULL,   -- Created/ToShip/Shipped/Completed
    payment_status nvarchar(50) NOT NULL, -- Placed/Pending/Paid
    order_total decimal(10,2) NOT NULL,
    payment_at datetime2 NULL,
    shipped_at datetime2 NULL,
    completed_at datetime2 NULL
);
```

#### OrderItems (訂單詳細表)
```sql
CREATE TABLE OrderItems (
    item_id int IDENTITY(1,1) PRIMARY KEY,
    order_id int NOT NULL,                -- FK to OrderInfo
    product_id int NOT NULL,              -- FK to ProductInfo
    line_no int NOT NULL,
    unit_price decimal(10,2) NOT NULL,
    quantity int NOT NULL,
    subtotal decimal(10,2) NOT NULL
);
```

#### Official_Store_Ranking (商城排行榜表)
```sql
CREATE TABLE Official_Store_Ranking (
    ranking_id int IDENTITY(1,1) PRIMARY KEY,
    period_type nvarchar(20) NOT NULL,    -- daily/weekly/monthly/quarterly/yearly
    ranking_date date NOT NULL,
    product_ID int NOT NULL,              -- FK to ProductInfo
    ranking_metric varchar(50) NOT NULL,  -- trading_amount/trading_volume
    ranking_position tinyint NOT NULL,
    trading_amount decimal(15,2) NOT NULL
);
```

### 重要設計原則

- **商品類型區分**: 透過ProductInfo.product_type區分遊戲與非遊戲商品
- **詳細資訊分離**: 遊戲商品存於GameProductDetails，其他商品存於OtherProductDetails
- **訂單狀態機**: order_status和payment_status嚴格按照規格定義的狀態轉換
- **排行榜多維度**: 支援多種期間類型和排名指標的組合
- **原子性操作**: 所有涉及庫存和訂單的操作都在資料庫交易中執行

## 🛒 商城功能

### 商品管理

#### 商品瀏覽與搜尋

```csharp
// 商品列表查詢
var searchDto = new ProductSearchDto
{
    Keyword = "賽博龐克",
    ProductType = "遊戲",
    MinPrice = 500,
    MaxPrice = 2000,
    InStock = true,
    Page = 1,
    PageSize = 20,
    SortBy = "CreatedAt",
    SortDirection = "desc"
};

var products = await storeService.SearchProductsAsync(searchDto);
```

#### 商品詳細資訊

```csharp
// 取得完整商品資訊
var productDetail = await storeService.GetProductDetailAsync(productId);

// 包含基本資訊、供應商、遊戲/其他商品詳細
if (productDetail.ProductType == "遊戲")
{
    var gameDetails = productDetail.GameDetails;
    var downloadLink = gameDetails.DownloadLink;
    var platformId = gameDetails.PlatformId;
}
else
{
    var otherDetails = productDetail.OtherDetails;
    var digitalCode = otherDetails.DigitalCode;
    var size = otherDetails.Size;
    var color = otherDetails.Color;
}
```

### 購物車管理

#### 購物車操作流程

```csharp
// 1. 加入商品到購物車
var addToCartDto = new AddToCartDto 
{ 
    ProductId = 1, 
    Quantity = 2 
};
var result = await storeService.AddToCartAsync(userId, addToCartDto);

// 2. 查看購物車
var cart = await storeService.GetCartAsync(userId);
var totalAmount = cart.Total;
var itemCount = cart.ItemCount;

// 3. 更新商品數量
var updateDto = new UpdateCartItemDto 
{ 
    ProductId = 1, 
    Quantity = 3 
};
await storeService.UpdateCartItemAsync(userId, updateDto);

// 4. 移除商品
await storeService.RemoveFromCartAsync(userId, productId);

// 5. 清空購物車
await storeService.ClearCartAsync(userId);
```

#### 庫存檢查機制

```csharp
// 即時庫存檢查
var hasStock = await storeService.CheckStockAsync(productId, quantity);

if (!hasStock)
{
    return BadRequest("庫存不足");
}

// 原子性庫存扣減
var deductResult = await storeService.DeductStockAsync(productId, quantity);
```

### 訂單管理

#### 訂單建立流程

```csharp
// 方法1：從指定商品建立訂單
var createOrderDto = new CreateOrderDto
{
    Items = new List<CreateOrderItemDto>
    {
        new() { ProductId = 1, Quantity = 1 },
        new() { ProductId = 2, Quantity = 2 }
    },
    Notes = "請盡快出貨"
};
var orderResult = await storeService.CreateOrderAsync(userId, createOrderDto);

// 方法2：從購物車建立訂單
var cartOrderResult = await storeService.CreateOrderFromCartAsync(userId);
```

#### 訂單狀態機

按照規格嚴格實現的狀態轉換：

```
訂單狀態 (order_status):
Created → ToShip → Shipped → Completed

付款狀態 (payment_status):
Placed → Pending → Paid
```

```csharp
// 狀態轉換驗證
public bool ValidateStatusTransition(string currentStatus, string newStatus)
{
    var validTransitions = new Dictionary<string, List<string>>
    {
        ["Created"] = new() { "ToShip", "Cancelled" },
        ["ToShip"] = new() { "Shipped", "Cancelled" },
        ["Shipped"] = new() { "Completed" },
        ["Completed"] = new(), // 終止狀態
        ["Cancelled"] = new()  // 終止狀態
    };

    return validTransitions.ContainsKey(currentStatus) && 
           validTransitions[currentStatus].Contains(newStatus);
}
```

#### 訂單處理流程

```csharp
// 1. 建立訂單 (order_status=Created, payment_status=Placed)
var order = await storeService.CreateOrderAsync(userId, createOrderDto);

// 2. 處理付款 (payment_status=Paid, order_status=ToShip)
var paymentResult = await storeService.ProcessPaymentAsync(userId, orderId);

// 3. 管理員安排出貨 (order_status=Shipped, 填入shipped_at)
var shipResult = await storeService.ShipOrderAsync(orderId);

// 4. 完成訂單 (order_status=Completed, 填入completed_at)
var completeResult = await storeService.CompleteOrderAsync(orderId);
```

### 排行榜系統

#### 排行榜計算與更新

```csharp
// 更新日榜
await storeService.UpdateRankingsAsync("daily", DateTime.Today);

// 更新月榜
await storeService.UpdateRankingsAsync("monthly", new DateTime(2024, 8, 1));

// 查詢排行榜
var rankingQuery = new RankingQueryDto
{
    PeriodType = "daily",           // daily/weekly/monthly/quarterly/yearly
    RankingMetric = "trading_amount", // trading_amount/trading_volume
    Date = DateTime.Today,
    Limit = 50
};

var rankings = await storeService.GetRankingsAsync(rankingQuery);
```

#### 排行榜資料結構

```csharp
public class StoreRankingDto
{
    public string PeriodType { get; set; }        // 榜單型態
    public DateTime RankingDate { get; set; }     // 榜單日期
    public int ProductId { get; set; }            // 商品ID
    public string ProductName { get; set; }       // 商品名稱
    public string RankingMetric { get; set; }     // 排名指標
    public int RankingPosition { get; set; }      // 名次
    public decimal TradingAmount { get; set; }    // 交易額
}
```

## 🔧 API 文件

### 核心API端點

#### 1. 商品管理 API

```http
# 取得商品列表
GET /api/store/products?page=1&pageSize=20&category=遊戲&inStock=true

# 取得商品詳細資訊
GET /api/store/products/{id}

# 搜尋商品
POST /api/store/products/search
{
  "keyword": "賽博龐克",
  "productType": "遊戲",
  "minPrice": 500,
  "maxPrice": 2000,
  "page": 1,
  "pageSize": 20
}

# 取得商品分類
GET /api/store/categories

# 取得熱門商品
GET /api/store/products/popular?limit=10
```

#### 2. 購物車管理 API

```http
# 取得購物車
GET /api/store/cart

# 加入購物車
POST /api/store/cart/add
{
  "productId": 1,
  "quantity": 2
}

# 更新購物車項目
PUT /api/store/cart/update
{
  "productId": 1,
  "quantity": 3
}

# 移除購物車項目
DELETE /api/store/cart/remove/{productId}

# 清空購物車
DELETE /api/store/cart/clear
```

#### 3. 訂單管理 API

```http
# 建立訂單
POST /api/store/orders/create
{
  "items": [
    {"productId": 1, "quantity": 1},
    {"productId": 2, "quantity": 2}
  ],
  "notes": "請盡快出貨"
}

# 從購物車建立訂單
POST /api/store/orders/create-from-cart

# 取得訂單詳細資訊
GET /api/store/orders/{id}

# 取得使用者訂單列表
GET /api/store/orders?orderStatus=Completed&page=1&pageSize=20

# 模擬付款
POST /api/store/orders/{id}/pay
```

#### 4. 排行榜 API

```http
# 取得商城排行榜
GET /api/store/rankings?periodType=daily&rankingMetric=trading_amount&limit=50
```

#### 5. 統計 API

```http
# 取得商城統計
GET /api/store/statistics
```

### API 回應格式

#### 成功回應
```json
{
  "success": true,
  "data": {
    // 具體資料內容
  },
  "message": "操作成功"
}
```

#### 錯誤回應
```json
{
  "success": false,
  "message": "錯誤訊息",
  "errors": ["詳細錯誤1", "詳細錯誤2"]
}
```

#### 分頁回應
```json
{
  "success": true,
  "data": {
    "page": 1,
    "pageSize": 20,
    "totalCount": 100,
    "totalPages": 5,
    "hasNextPage": true,
    "data": [
      // 資料項目
    ]
  }
}
```

## 🖥️ 前端介面

### UI設計原則

- **Glass Morphism風格**: 與系統整體設計一致的半透明毛玻璃效果
- **商品展示**: 清晰的商品資訊展示和分類瀏覽
- **購物流程**: 直觀的購物車管理和結帳流程
- **響應式設計**: 支援桌面和行動裝置
- **即時更新**: 庫存狀態、購物車數量即時反映

### 主要頁面

1. **商品列表頁**: 分類瀏覽、搜尋篩選、排序功能
2. **商品詳細頁**: 完整商品資訊、加入購物車、相關推薦
3. **購物車頁**: 項目管理、數量調整、總計顯示
4. **結帳頁**: 訂單確認、付款方式、收貨資訊
5. **訂單列表頁**: 訂單歷史、狀態追蹤、詳細查看
6. **排行榜頁**: 熱門商品、銷售排名、趨勢分析

### 互動流程

```
使用者瀏覽商品
     ↓
選擇商品加入購物車
     ↓
查看購物車並調整
     ↓
建立訂單
     ↓
確認並付款
     ↓
訂單處理 (出貨/完成)
     ↓
查看訂單狀態
```

## ⚙️ 設定與部署

### 依賴注入設定

```csharp
// Program.cs
builder.Services.AddScoped<IStoreService, StoreService>();
builder.Services.AddScoped<IStoreRepository, StoreRepository>();
```

### 必要相依性

- `IUserService`: 使用者權限檢查
- `INotificationService`: 訂單狀態通知
- `GameCoreDbContext`: 資料庫存取
- `ILogger`: 日誌記錄

### 系統設定

```json
{
  "StoreSettings": {
    "MaxCartItems": 20,
    "MaxOrderItems": 10,
    "DefaultPageSize": 20,
    "MaxPageSize": 100,
    "StockReserveMinutes": 15,
    "OrderTimeoutMinutes": 30
  }
}
```

## 🧪 測試指南

### 單元測試

```bash
# 執行所有商城測試
dotnet test --filter "StoreControllerTests"

# 執行特定功能測試
dotnet test --filter "AddToCart_ShouldReturnSuccess"
```

### 測試覆蓋範圍

- ✅ 商品搜尋和詳細資訊查詢
- ✅ 購物車管理 (新增、更新、移除、清空)
- ✅ 訂單建立和狀態管理
- ✅ 付款處理和庫存扣減
- ✅ 排行榜計算和查詢
- ✅ 統計資料計算
- ✅ 管理員功能
- ✅ 錯誤處理和邊界條件

### 測試資料

使用 `10-OfficialStoreSeedData.sql` 生成完整測試資料，包含：

- 20個供應商
- 50個商品 (涵蓋遊戲、周邊、點數卡、收藏品)
- 100+訂單記錄 (多種狀態分布)
- 完整排行榜資料 (日榜、月榜)
- 豐富的商品詳細資訊

## 🔍 疑難排解

### 常見問題

#### 1. 庫存扣減異常
**問題**: 同時下單導致超賣
**解決**: 使用資料庫交易確保原子性操作

```csharp
using var transaction = await _context.Database.BeginTransactionAsync();
try
{
    // 檢查庫存
    var product = await _context.ProductInfo.FirstOrDefaultAsync(p => p.ProductId == productId);
    if (product.Shipment_Quantity < quantity)
        throw new InvalidOperationException("庫存不足");
    
    // 扣減庫存
    product.Shipment_Quantity -= quantity;
    await _context.SaveChangesAsync();
    
    await transaction.CommitAsync();
}
catch
{
    await transaction.RollbackAsync();
    throw;
}
```

#### 2. 訂單狀態轉換錯誤
**問題**: 非法的狀態轉換
**解決**: 實作狀態轉換驗證邏輯

#### 3. 購物車數據不一致
**問題**: 購物車顯示的庫存與實際不符
**解決**: 即時檢查庫存狀態，提供庫存警告

#### 4. 排行榜計算延遲
**問題**: 排行榜資料更新不及時
**解決**: 設定定時任務或觸發機制更新排行榜

### 監控指標

- 商品瀏覽量
- 購物車轉換率
- 訂單完成率
- 平均訂單金額
- 商品庫存周轉率
- API回應時間

## 📈 效能最佳化

### 資料庫最佳化

```sql
-- 建議的索引
CREATE INDEX IX_ProductInfo_Type_Price 
ON ProductInfo (product_type, price);

CREATE INDEX IX_OrderInfo_UserId_OrderDate 
ON OrderInfo (user_id, order_date);

CREATE INDEX IX_OrderItems_ProductId 
ON OrderItems (product_id);

CREATE INDEX IX_Store_Ranking_Period_Date 
ON Official_Store_Ranking (period_type, ranking_date, ranking_metric);
```

### 快取策略

- 商品分類清單快取 (4小時)
- 熱門商品快取 (1小時)
- 商品詳細資訊快取 (30分鐘)
- 排行榜資料快取 (1小時)

### 批次處理

- 排行榜計算批次處理 (每日凌晨)
- 庫存預警批次檢查
- 訂單狀態自動更新

## 🚀 未來擴展

### 計劃功能

1. **推薦系統**: 基於購買歷史的商品推薦
2. **優惠券系統**: 折扣碼和促銷活動
3. **評價系統**: 商品評分和評論
4. **庫存預警**: 低庫存自動通知
5. **多幣別支援**: 國際化支付

### 技術擴展

- 實作分散式庫存管理
- 加入機器學習推薦演算法
- 支援即時庫存同步
- 實作訂單追蹤系統

## 📚 相關文件

- [錢包系統指南](./WalletSystemGuide.md)
- [每日簽到系統指南](./DailySignInSystemGuide.md)
- [虛擬寵物系統指南](./VirtualPetSystemGuide.md)
- [API規格文件](./APIReference.md)

---

*本文件最後更新: 2024年8月15日*
*版本: 1.0.0*
*維護者: GameCore開發團隊*