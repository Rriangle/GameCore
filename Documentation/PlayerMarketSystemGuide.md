# GameCore 自由市場系統完整指南

## 📋 系統概述

GameCore自由市場系統是一個完整的C2C交易平台，嚴格按照規格實現商品上架、交易頁面、即時訊息、排行榜分析等核心C2C功能。系統設計旨在提供安全可靠的玩家間交易體驗，支援道具交換、帳號轉讓、虛擬貨幣交易等多種交易類型，確保交易安全和平台收益。

### 🎯 核心特色

- **C2C交易模式**: 玩家間直接交易，平台提供安全保障和爭議處理
- **多樣商品類型**: 支援遊戲道具、帳號、虛擬貨幣、稀有裝備、限定商品等
- **安全交易機制**: 買賣雙方確認制度，防止交易糾紛
- **即時通訊系統**: 交易頁面內建聊天功能，方便買賣雙方溝通
- **平台抽成機制**: 自動計算和分配平台手續費
- **權限管理**: 銷售權限審核，確保賣家資質

## 🏗️ 系統架構

### 三層架構設計

```
┌─────────────────────┐
│   Presentation      │  ← PlayerMarketController, PlayerMarket Views
├─────────────────────┤
│   Business Logic    │  ← PlayerMarketService, PlayerMarketDTOs
├─────────────────────┤
│   Data Access       │  ← PlayerMarket Entities, DbContext
└─────────────────────┘
```

### 核心元件

1. **PlayerMarketController**: RESTful API控制器，提供完整C2C交易管理端點
2. **PlayerMarketService**: 業務邏輯服務，實現所有交易相關功能
3. **PlayerMarketDTOs**: 資料傳輸物件，涵蓋所有交易操作的請求和回應
4. **PlayerMarket Views**: 交易界面，包含商品展示和交易流程
5. **PlayerMarket Entities**: 資料庫實體，對應自由市場相關資料表

## 📊 資料庫設計

### 核心資料表結構

#### PlayerMarketProductInfo (自由市場商品資訊表)
```sql
CREATE TABLE PlayerMarketProductInfo (
    p_product_id int IDENTITY(1,1) PRIMARY KEY,
    p_product_type nvarchar(100) NULL,        -- 商品類型
    p_product_title nvarchar(200) NULL,       -- 商品標題(噱頭標語)
    p_product_name nvarchar(200) NULL,        -- 商品名稱
    p_product_description nvarchar(1000) NULL, -- 商品描述
    product_id int NULL,                       -- 關聯官方商品ID (FK)
    seller_id int NOT NULL,                    -- 賣家ID (FK)
    p_status nvarchar(50) NULL,               -- 商品狀態
    price decimal(18,2) NOT NULL,            -- 售價
    p_product_img_id nvarchar(100) NULL,      -- 商品圖片ID
    created_at datetime2 NULL,               -- 建立時間
    updated_at datetime2 NULL                -- 更新時間
);
```

#### PlayerMarketProductImgs (自由市場商品圖片表)
```sql
CREATE TABLE PlayerMarketProductImgs (
    p_product_img_id int IDENTITY(1,1) PRIMARY KEY,
    p_product_id int NOT NULL,               -- 商品ID (FK)
    p_product_img_url varbinary(max) NULL    -- 商品圖片URL (二進位存放)
);
```

#### PlayerMarketOrderInfo (自由市場訂單表)
```sql
CREATE TABLE PlayerMarketOrderInfo (
    p_order_id int IDENTITY(1,1) PRIMARY KEY,
    p_product_id int NOT NULL,               -- 商品ID (FK)
    seller_id int NOT NULL,                  -- 賣家ID (FK)
    buyer_id int NOT NULL,                   -- 買家ID (FK)
    p_order_date datetime2 NOT NULL,         -- 訂單日期
    p_order_status nvarchar(50) NOT NULL,    -- 訂單狀態
    p_payment_status nvarchar(50) NOT NULL,  -- 付款狀態
    p_unit_price decimal(18,2) NOT NULL,     -- 單價
    p_quantity int NOT NULL,                 -- 數量
    p_order_total decimal(18,2) NOT NULL,    -- 總額
    p_order_created_at datetime2 NOT NULL,   -- 建立時間
    p_order_updated_at datetime2 NULL        -- 更新時間
);
```

#### PlayerMarketOrderTradepage (交易中頁面表)
```sql
CREATE TABLE PlayerMarketOrderTradepage (
    p_order_tradepage_id int IDENTITY(1,1) PRIMARY KEY,
    p_order_id int NOT NULL,                 -- 訂單ID (FK)
    p_product_id int NOT NULL,               -- 商品ID (FK)
    p_order_platform_fee decimal(18,2) NOT NULL, -- 平台抽成
    seller_transferred_at datetime2 NULL,    -- 賣家移交時間
    buyer_received_at datetime2 NULL,        -- 買家接收時間
    completed_at datetime2 NULL              -- 交易完成時間
);
```

#### PlayerMarketTradeMsg (自由市場交易頁面對話表)
```sql
CREATE TABLE PlayerMarketTradeMsg (
    trade_msg_id int IDENTITY(1,1) PRIMARY KEY,
    p_order_tradepage_id int NOT NULL,       -- 交易頁面ID (FK)
    msg_from nvarchar(20) NOT NULL,          -- 訊息來源 (seller/buyer)
    message_text nvarchar(500) NOT NULL,     -- 訊息內容
    created_at datetime2 NOT NULL            -- 傳訊時間
);
```

#### PlayerMarketRanking (自由市場排行榜表)
```sql
CREATE TABLE PlayerMarketRanking (
    p_ranking_id int IDENTITY(1,1) PRIMARY KEY,
    p_period_type varchar(20) NOT NULL,      -- 榜單型態
    p_ranking_date date NOT NULL,            -- 榜單日期
    p_product_id int NOT NULL,               -- 商品ID (FK)
    p_ranking_metric varchar(50) NOT NULL,   -- 排名指標
    p_ranking_position int NOT NULL,         -- 名次
    p_trading_amount decimal(18,2) NOT NULL, -- 交易額
    created_at datetime2 NOT NULL,           -- 建立時間
    updated_at datetime2 NOT NULL            -- 更新時間
);
```

### 重要設計原則

- **商品狀態管理**: active(上架中)/sold(已售出)/removed(已下架)/suspended(已暫停)
- **訂單狀態機**: Created → Trading → Completed/Cancelled
- **雙方確認機制**: seller_transferred_at + buyer_received_at → completed_at
- **平台抽成計算**: 按交易金額和商品類型計算手續費
- **即時通訊**: 每個交易頁面獨立的訊息流

## 🛒 交易功能

### 商品管理

#### 商品上架與管理

```csharp
// 上架商品 (需要銷售權限)
var createDto = new CreatePlayerMarketProductDto
{
    PProductType = "遊戲道具",
    PProductTitle = "【超值包】史詩級武器 +15 限時優惠！",
    PProductName = "史詩級武器 +15",
    PProductDescription = "精心培養的高品質道具，屬性完美，適合各種副本和PVP",
    Price = 150.00m,
    ProductId = 1, // 可選：關聯官方商品
    Images = new List<string> { "base64imagedata1", "base64imagedata2" }
};

var result = await playerMarketService.CreateProductAsync(userId, createDto);
```

#### 商品搜尋與瀏覽

```csharp
// 多維度商品搜尋
var searchDto = new PlayerMarketSearchDto
{
    Keyword = "史詩級",
    PProductType = "遊戲道具",
    MinPrice = 100,
    MaxPrice = 300,
    SellerId = null, // 特定賣家
    PStatus = "active",
    Page = 1,
    PageSize = 20,
    SortBy = "CreatedAt",
    SortDirection = "desc"
};

var products = await playerMarketService.SearchProductsAsync(searchDto);
```

### 訂單管理

#### 下單購買流程

```csharp
// 1. 建立訂單
var createOrderDto = new CreatePlayerMarketOrderDto
{
    PProductId = 1,
    PQuantity = 1,
    Notes = "請盡快安排交易時間"
};

var orderResult = await playerMarketService.CreateOrderAsync(buyerId, createOrderDto);

// 2. 系統自動建立交易頁面
var tradepageResult = await playerMarketService.CreateTradepageAsync(buyerId, orderId);
```

#### 訂單狀態機

按照規格嚴格實現的狀態轉換：

```
訂單狀態 (p_order_status):
Created → Trading → Completed / Cancelled

付款狀態 (p_payment_status):
Pending → Paid / N/A (道具交換)
```

### 交易頁面管理

#### 安全交易機制

```csharp
// 買賣雙方確認流程
// 1. 賣家確認移交
var confirmTransferDto = new ConfirmTransferDto
{
    Notes = "道具已移交完成，請確認收到"
};
await playerMarketService.ConfirmSellerTransferAsync(sellerId, tradepageId, confirmTransferDto);

// 2. 買家確認接收
var confirmReceiveDto = new ConfirmTransferDto
{
    Notes = "商品已收到，謝謝！"
};
await playerMarketService.ConfirmBuyerReceivedAsync(buyerId, tradepageId, confirmReceiveDto);

// 3. 系統自動完成交易和入帳
// - 賣家獲得: 交易金額 - 平台抽成 → UserSales_Wallet
// - 買家扣除: 交易金額 → User_Point (如用點數)
// - 平台收入: 平台抽成
```

#### 即時通訊系統

```csharp
// 交易頁面內建聊天
var messageDto = new SendTradeMessageDto
{
    MessageText = "請問什麼時候可以在線上交易呢？"
};

var messageResult = await playerMarketService.SendTradeMessageAsync(userId, tradepageId, messageDto);

// 訊息自動標記發送者 (buyer/seller)
// 支援即時通知對方
```

### 平台抽成機制

#### 手續費計算

```csharp
// 平台抽成計算 (可依商品類型調整)
public async Task<decimal> CalculatePlatformFeeAsync(decimal tradeAmount, string productType)
{
    var feeRate = productType switch
    {
        "遊戲道具" => 0.05m,      // 5%
        "遊戲帳號" => 0.08m,      // 8%
        "虛擬貨幣" => 0.03m,      // 3%
        "稀有裝備" => 0.06m,      // 6%
        "限定商品" => 0.10m,      // 10%
        _ => 0.05m               // 預設5%
    };
    
    return tradeAmount * feeRate;
}
```

#### 交易結算

```csharp
// 完成交易時的自動結算
public async Task<PlayerMarketServiceResult> CompleteTradeSettlementAsync(int tradepageId)
{
    using var transaction = await _context.Database.BeginTransactionAsync();
    try
    {
        var tradepage = await GetTradepageWithDetailsAsync(tradepageId);
        var order = tradepage.Order;
        var platformFee = tradepage.POrderPlatformFee;
        var sellerAmount = order.POrderTotal - platformFee;

        // 賣家入帳
        await _walletService.AddToSalesWalletAsync(order.SellerId, sellerAmount, $"銷售收入 - 訂單#{order.POrderId}");
        
        // 買家扣款 (如果使用點數)
        if (order.PPaymentStatus == "Paid")
        {
            await _walletService.DeductPointsAsync(order.BuyerId, order.POrderTotal, $"購買商品 - 訂單#{order.POrderId}");
        }
        
        // 更新商品和訂單狀態
        await UpdateProductStatusAsync(order.PProductId, "sold");
        await UpdateOrderStatusAsync(order.POrderId, "Completed");
        
        // 設定完成時間
        tradepage.CompletedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();
        
        await transaction.CommitAsync();
        
        // 發送完成通知
        await SendTradeNotificationAsync("trade_completed", order.SellerId, order);
        await SendTradeNotificationAsync("trade_completed", order.BuyerId, order);
        
        return PlayerMarketServiceResult.CreateSuccess("交易完成，款項已入帳");
    }
    catch (Exception ex)
    {
        await transaction.RollbackAsync();
        throw;
    }
}
```

## 🔧 API 文件

### 核心API端點

#### 1. 商品管理 API

```http
# 取得市場商品列表
GET /api/market/products?page=1&pageSize=20&category=遊戲道具&status=active

# 取得商品詳細資訊
GET /api/market/products/{id}

# 搜尋商品
POST /api/market/products/search
{
  "keyword": "史詩級",
  "pProductType": "遊戲道具",
  "minPrice": 100,
  "maxPrice": 300,
  "page": 1,
  "pageSize": 20
}

# 上架商品 (需要銷售權限)
POST /api/market/products
{
  "pProductType": "遊戲道具",
  "pProductTitle": "【超值包】史詩級武器 限時優惠！",
  "pProductName": "史詩級武器 +15",
  "pProductDescription": "精心培養的高品質道具",
  "price": 150.00,
  "images": ["base64imagedata"]
}

# 更新商品資訊
PUT /api/market/products/{id}
{
  "pProductTitle": "【降價促銷】史詩級武器 +15",
  "price": 120.00,
  "pStatus": "active"
}

# 上傳商品圖片
POST /api/market/products/{id}/images
["base64image1", "base64image2"]

# 取得我的商品
GET /api/market/my-products?status=active&page=1&pageSize=20
```

#### 2. 訂單管理 API

```http
# 下單購買
POST /api/market/orders
{
  "pProductId": 1,
  "pQuantity": 1,
  "notes": "請盡快交易"
}

# 取得訂單詳細資訊
GET /api/market/orders/{id}

# 取得使用者訂單列表
GET /api/market/orders?role=buyer&status=Trading&page=1&pageSize=20
```

#### 3. 交易頁面 API

```http
# 建立交易頁面
POST /api/market/tradepages
100  // orderId

# 取得交易頁面詳細資訊
GET /api/market/tradepages/{id}

# 發送交易訊息
POST /api/market/tradepages/{id}/messages
{
  "messageText": "請問什麼時候可以交易呢？"
}

# 賣家確認移交
POST /api/market/tradepages/{id}/seller-transferred
{
  "notes": "道具已移交完成"
}

# 買家確認接收
POST /api/market/tradepages/{id}/buyer-received
{
  "notes": "商品已收到，謝謝！"
}
```

#### 4. 排行榜 API

```http
# 取得自由市場排行榜
GET /api/market/rankings?periodType=daily&rankingMetric=trading_amount&limit=50
```

#### 5. 統計 API

```http
# 取得自由市場統計
GET /api/market/statistics
```

### API 回應格式

#### 商品詳細資訊回應
```json
{
  "success": true,
  "data": {
    "pProductId": 1,
    "pProductType": "遊戲道具",
    "pProductTitle": "【超值包】史詩級武器 +15 限時優惠！",
    "pProductName": "史詩級武器 +15",
    "pProductDescription": "精心培養的高品質道具",
    "price": 150.00,
    "pStatus": "active",
    "seller": {
      "sellerId": 123,
      "sellerName": "測試賣家",
      "haseSalesAuthority": true,
      "totalSales": 25,
      "rating": 4.8
    },
    "images": [
      {
        "pProductImgId": 1,
        "isMain": true,
        "sortOrder": 1
      }
    ],
    "officialProduct": {
      "productId": 5,
      "productName": "官方商品名稱",
      "officialPrice": 200.00
    },
    "canBuy": true
  }
}
```

#### 交易頁面回應
```json
{
  "success": true,
  "data": {
    "pOrderTradepageId": 200,
    "pOrderId": 100,
    "pProductId": 1,
    "pOrderPlatformFee": 7.50,
    "sellerTransferredAt": "2024-08-15T14:30:00Z",
    "buyerReceivedAt": null,
    "completedAt": null,
    "tradeStatus": "等待買家確認",
    "canSellerTransfer": false,
    "canBuyerReceive": true,
    "messages": [
      {
        "tradeMsgId": 1,
        "msgFrom": "seller",
        "messageText": "商品已準備好",
        "createdAt": "2024-08-15T14:00:00Z"
      },
      {
        "tradeMsgId": 2,
        "msgFrom": "buyer", 
        "messageText": "我已經在線上了",
        "createdAt": "2024-08-15T14:15:00Z"
      }
    ]
  }
}
```

## 🖥️ 前端介面

### UI設計原則

- **Glass Morphism風格**: 與系統整體設計一致的半透明毛玻璃效果
- **商品展示**: 清晰的商品資訊展示和賣家信用評級
- **交易流程**: 直觀的交易進度顯示和狀態追蹤
- **即時通訊**: 內建聊天界面，支援即時訊息通知
- **響應式設計**: 支援桌面和行動裝置

### 主要頁面

1. **市場首頁**: 熱門商品、分類瀏覽、搜尋功能
2. **商品詳細頁**: 完整商品資訊、賣家評價、購買按鈕
3. **上架頁面**: 商品發布表單、圖片上傳、價格設定
4. **交易頁面**: 雙方確認流程、即時聊天、進度追蹤
5. **訂單管理**: 買家/賣家訂單列表、狀態管理
6. **排行榜頁**: 熱門商品、成功賣家、交易統計

### 交易流程

```
賣家上架商品
     ↓
買家瀏覽並下單
     ↓
系統建立交易頁面
     ↓
雙方在交易頁面溝通
     ↓
賣家確認移交道具
     ↓
買家確認接收道具
     ↓
系統自動完成交易和入帳
```

## ⚙️ 設定與部署

### 依賴注入設定

```csharp
// Program.cs
builder.Services.AddScoped<IPlayerMarketService, PlayerMarketService>();
builder.Services.AddScoped<IPlayerMarketRepository, PlayerMarketRepository>();
```

### 必要相依性

- `IWalletService`: 錢包系統整合，處理交易入帳
- `IUserService`: 使用者權限檢查和資料查詢
- `INotificationService`: 交易通知和狀態更新
- `GameCoreDbContext`: 資料庫存取
- `ILogger`: 日誌記錄

### 系統設定

```json
{
  "PlayerMarketSettings": {
    "DefaultPlatformFeeRate": 0.05,
    "MaxProductImages": 5,
    "MaxProductDescriptionLength": 1000,
    "TradeTimeoutHours": 72,
    "DisputeResolutionHours": 168,
    "ImageMaxSizeBytes": 5242880
  }
}
```

## 🧪 測試指南

### 單元測試

```bash
# 執行所有自由市場測試
dotnet test --filter "PlayerMarketControllerTests"

# 執行特定功能測試
dotnet test --filter "CreateProduct_ShouldReturnSuccess"
```

### 測試覆蓋範圍

- ✅ 商品上架和管理 (建立、更新、下架、圖片上傳)
- ✅ 商品搜尋和瀏覽 (多維度篩選、分頁排序)
- ✅ 訂單管理 (建立、查詢、取消)
- ✅ 交易頁面管理 (建立、訊息、雙方確認)
- ✅ 交易結算和入帳 (平台抽成、錢包更新)
- ✅ 排行榜計算和查詢
- ✅ 統計資料計算
- ✅ 權限控制和安全檢查
- ✅ 錯誤處理和邊界條件

### 測試資料

使用 `11-PlayerMarketSeedData.sql` 生成完整測試資料，包含：

- 200+商品記錄 (涵蓋6種商品類型)
- 80+訂單記錄 (多種狀態分布)
- 50+交易頁面 (包含完整通訊記錄)
- 300+交易訊息 (買賣雙方對話)
- 完整排行榜資料 (日榜、月榜)

## 🔍 疑難排解

### 常見問題

#### 1. 交易完成後入帳異常
**問題**: 賣家沒有收到款項或買家重複扣款
**解決**: 使用資料庫交易確保結算原子性

```csharp
using var transaction = await _context.Database.BeginTransactionAsync();
try
{
    // 所有錢包操作在同一個交易中
    await _walletService.AddToSalesWalletAsync(sellerId, sellerAmount, reference);
    await _walletService.DeductPointsAsync(buyerId, orderTotal, reference);
    await _context.SaveChangesAsync();
    await transaction.CommitAsync();
}
catch
{
    await transaction.RollbackAsync();
    throw;
}
```

#### 2. 商品狀態同步問題
**問題**: 已售出商品仍然顯示可購買
**解決**: 在訂單完成時同步更新商品狀態

#### 3. 平台抽成計算錯誤
**問題**: 手續費計算不正確
**解決**: 統一手續費計算邏輯，支援商品類型差異化費率

#### 4. 交易頁面權限問題
**問題**: 非交易參與者可以查看交易頁面
**解決**: 嚴格檢查使用者是否為買家或賣家

### 監控指標

- 商品上架量和成交率
- 交易完成時間分布
- 平台手續費收入
- 使用者活躍度 (買家/賣家)
- 交易糾紛率
- 退款和爭議處理時間

## 📈 效能最佳化

### 資料庫最佳化

```sql
-- 建議的索引
CREATE INDEX IX_PlayerMarketProductInfo_Status_Type 
ON PlayerMarketProductInfo (p_status, p_product_type);

CREATE INDEX IX_PlayerMarketProductInfo_Seller_CreatedAt 
ON PlayerMarketProductInfo (seller_id, created_at DESC);

CREATE INDEX IX_PlayerMarketOrderInfo_Buyer_Date 
ON PlayerMarketOrderInfo (buyer_id, p_order_date DESC);

CREATE INDEX IX_PlayerMarketOrderInfo_Seller_Date 
ON PlayerMarketOrderInfo (seller_id, p_order_date DESC);

CREATE INDEX IX_PlayerMarketTradeMsg_Tradepage_CreatedAt 
ON PlayerMarketTradeMsg (p_order_tradepage_id, created_at);
```

### 快取策略

- 熱門商品列表快取 (30分鐘)
- 商品詳細資訊快取 (15分鐘)
- 排行榜資料快取 (1小時)
- 使用者商品列表快取 (10分鐘)

### 即時通訊最佳化

- 使用SignalR實現即時訊息推送
- 訊息分頁載入，避免大量歷史訊息
- 壓縮圖片上傳，減少頻寬使用

## 🚀 未來擴展

### 計劃功能

1. **評價系統**: 買賣雙方互評，建立信用體系
2. **競價機制**: 支援商品競標和最高出價
3. **保險機制**: 交易保險，降低交易風險
4. **多貨幣支援**: 支援多種虛擬貨幣和法幣
5. **API開放**: 提供第三方整合接口

### 技術擴展

- 實作區塊鏈交易記錄
- 加入AI反詐騙檢測
- 支援跨平台道具交易
- 實作智能合約託管

## 📚 相關文件

- [官方商城系統指南](./OfficialStoreSystemGuide.md)
- [錢包系統指南](./WalletSystemGuide.md)
- [通知系統指南](./NotificationSystemGuide.md)
- [API規格文件](./APIReference.md)

---

*本文件最後更新: 2024年8月15日*
*版本: 1.0.0*
*維護者: GameCore開發團隊*