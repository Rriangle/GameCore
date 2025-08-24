# 實體與資料庫 Schema 對照表

## 建立時間
2025-08-15 23:49 UTC

## 實體清單與 Schema 對照

### 1. Manager 實體
**檔案**: `GameCore.Core/Entities/Manager.cs`
**對應資料表**: `managers`

| 欄位 | 型別 | Nullable | 預設值 | 說明 |
|------|------|----------|--------|------|
| Id | int | No | Identity | 主鍵 |
| Username | varchar(50) | No | - | 用戶名 |
| Email | varchar(100) | No | - | 電子郵件 |
| PasswordHash | varchar(255) | No | - | 密碼雜湊 |
| Role | ManagerRole | No | Manager | 角色 |
| IsActive | bool | No | true | 是否啟用 |
| LastLoginAt | datetime | Yes | null | 最後登入時間 |
| CreatedAt | datetime | No | UTC Now | 建立時間 |
| UpdatedAt | datetime | No | UTC Now | 更新時間 |

### 2. PlayerMarketItem 實體
**檔案**: `GameCore.Core/Entities/PlayerMarketItem.cs`
**對應資料表**: `player_market_items`

| 欄位 | 型別 | Nullable | 預設值 | 說明 |
|------|------|----------|--------|------|
| Id | int | No | Identity | 主鍵 |
| SellerId | int | No | - | 賣家ID |
| Title | varchar(200) | No | - | 商品標題 |
| Description | text | No | - | 商品描述 |
| Price | decimal(18,2) | No | - | 價格 |
| Category | varchar(50) | No | - | 分類 |
| Condition | varchar(20) | No | - | 商品狀況 |
| ImageUrl | varchar(500) | Yes | null | 圖片URL |
| Status | MarketItemStatus | No | Active | 商品狀態 |
| ViewCount | int | No | 0 | 瀏覽次數 |
| FavoriteCount | int | No | 0 | 收藏次數 |
| CreatedAt | datetime | No | UTC Now | 建立時間 |
| UpdatedAt | datetime | No | UTC Now | 更新時間 |
| ExpiresAt | datetime | Yes | null | 過期時間 |

### 3. MarketTransaction 實體
**檔案**: `GameCore.Core/Entities/PlayerMarketItem.cs`
**對應資料表**: `market_transactions`

| 欄位 | 型別 | Nullable | 預設值 | 說明 |
|------|------|----------|--------|------|
| Id | int | No | Identity | 主鍵 |
| MarketItemId | int | No | - | 商品ID |
| BuyerId | int | No | - | 買家ID |
| SellerId | int | No | - | 賣家ID |
| TransactionAmount | decimal(18,2) | No | - | 交易金額 |
| Status | TransactionStatus | No | Pending | 交易狀態 |
| TransactionDate | datetime | No | UTC Now | 交易日期 |
| CompletedAt | datetime | Yes | null | 完成時間 |
| CreatedAt | datetime | No | UTC Now | 建立時間 |
| UpdatedAt | datetime | No | UTC Now | 更新時間 |

### 4. MarketReview 實體
**檔案**: `GameCore.Core/Entities/PlayerMarketItem.cs`
**對應資料表**: `market_reviews`

| 欄位 | 型別 | Nullable | 預設值 | 說明 |
|------|------|----------|--------|------|
| Id | int | No | Identity | 主鍵 |
| MarketItemId | int | No | - | 商品ID |
| ReviewerId | int | No | - | 評價者ID |
| Rating | int | No | - | 評分(1-5) |
| Comment | text | Yes | null | 評價內容 |
| CreatedAt | datetime | No | UTC Now | 建立時間 |
| UpdatedAt | datetime | No | UTC Now | 更新時間 |

### 5. StoreProduct 實體
**檔案**: `GameCore.Core/Entities/StoreProduct.cs`
**對應資料表**: `store_products`

| 欄位 | 型別 | Nullable | 預設值 | 說明 |
|------|------|----------|--------|------|
| Id | int | No | Identity | 主鍵 |
| Name | varchar(200) | No | - | 商品名稱 |
| Description | text | No | - | 商品描述 |
| Price | decimal(18,2) | No | - | 價格 |
| StockQuantity | int | No | 0 | 庫存數量 |
| Category | varchar(50) | No | - | 分類 |
| ImageUrl | varchar(500) | Yes | null | 圖片URL |
| IsActive | bool | No | true | 是否啟用 |
| CreatedAt | datetime | No | UTC Now | 建立時間 |
| UpdatedAt | datetime | No | UTC Now | 更新時間 |

### 6. StoreOrder 實體
**檔案**: `GameCore.Core/Entities/StoreProduct.cs`
**對應資料表**: `store_orders`

| 欄位 | 型別 | Nullable | 預設值 | 說明 |
|------|------|----------|--------|------|
| Id | int | No | Identity | 主鍵 |
| UserId | int | No | - | 用戶ID |
| OrderTotal | decimal(18,2) | No | - | 訂單總額 |
| Status | OrderStatus | No | Pending | 訂單狀態 |
| ShippingAddress | text | Yes | null | 收貨地址 |
| PaymentMethod | varchar(50) | Yes | null | 付款方式 |
| OrderDate | datetime | No | UTC Now | 訂單日期 |
| ShippedAt | datetime | Yes | null | 發貨時間 |
| DeliveredAt | datetime | Yes | null | 送達時間 |
| CreatedAt | datetime | No | UTC Now | 建立時間 |
| UpdatedAt | datetime | No | UTC Now | 更新時間 |

### 7. ShoppingCart 實體
**檔案**: `GameCore.Core/Entities/StoreProduct.cs`
**對應資料表**: `shopping_carts`

| 欄位 | 型別 | Nullable | 預設值 | 說明 |
|------|------|----------|--------|------|
| Id | int | No | Identity | 主鍵 |
| UserId | int | No | - | 用戶ID |
| CreatedAt | datetime | No | UTC Now | 建立時間 |
| UpdatedAt | datetime | No | UTC Now | 更新時間 |

## 導航屬性與關聯

### Manager 關聯
- `ManagerRolePermission` (一對多)

### PlayerMarketItem 關聯
- `User` (多對一，Seller)
- `MarketTransaction` (一對多)
- `MarketReview` (一對多)

### MarketTransaction 關聯
- `PlayerMarketItem` (多對一)
- `User` (多對一，Buyer)
- `User` (多對一，Seller)

### MarketReview 關聯
- `PlayerMarketItem` (多對一)
- `User` (多對一，Reviewer)

### StoreProduct 關聯
- `StoreOrderItem` (一對多)
- `ShoppingCartItem` (一對多)

### StoreOrder 關聯
- `User` (多對一)
- `StoreOrderItem` (一對多)

### ShoppingCart 關聯
- `User` (多對一)
- `ShoppingCartItem` (一對多)

## Schema 差異處理

### 1. 命名規範統一
- 所有資料表名稱使用小寫 + 底線
- 所有欄位名稱使用小寫 + 底線
- 主鍵統一使用 `id`

### 2. 型別對應
- `decimal` 型別統一使用 `decimal(18,2)` 或 `decimal(18,4)`
- `datetime` 型別統一使用 UTC 時間
- `bool` 型別統一使用 `Is` 前綴

### 3. 預設值處理
- 時間欄位預設值為 `DateTime.UtcNow`
- 狀態欄位預設值為對應列舉的第一個值
- 計數欄位預設值為 0

## 下一步
1. 建立 Repository 實作
2. 完成 Service 介面實作
3. 建立測試