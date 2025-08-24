# GameCore 每日簽到系統完整指南

## 📋 系統概述

GameCore每日簽到系統是一個完整的使用者日常互動功能，嚴格按照Asia/Taipei時區運作，提供連續簽到獎勵、週末加成、月度全勤獎勵等豐富功能。系統設計旨在提高使用者黏性和日活躍度。

### 🎯 核心特色

- **時區精準**: 嚴格按照Asia/Taipei時區進行日界線計算
- **獎勵豐富**: 基礎獎勵 + 連續獎勵 + 週末加成 + 月度全勤
- **防作弊**: 每日限一次簽到，原子操作保證資料一致性
- **使用者友好**: 美觀的日曆界面和即時統計
- **擴展性強**: 模組化設計，易於擴展新功能

## 🏗️ 系統架構

### 三層架構設計

```
┌─────────────────────┐
│   Presentation      │  ← DailySignInController, Index.cshtml
├─────────────────────┤
│   Business Logic    │  ← DailySignInService, DTOs
├─────────────────────┤
│   Data Access       │  ← UserSignInStats Entity, DbContext
└─────────────────────┘
```

### 核心元件

1. **DailySignInController**: RESTful API控制器
2. **DailySignInService**: 業務邏輯服務
3. **DailySignInDTOs**: 資料傳輸物件
4. **Index.cshtml**: 使用者介面
5. **UserSignInStats**: 資料庫實體

## 📊 資料庫設計

### UserSignInStats 資料表

```sql
CREATE TABLE UserSignInStats (
    LogID int IDENTITY(1,1) PRIMARY KEY,
    SignTime datetime2 NOT NULL DEFAULT SYSUTCDATETIME(),
    UserID int NOT NULL,
    PointsChanged int NOT NULL DEFAULT 0,
    ExpGained int NOT NULL DEFAULT 0,
    PointsChangedTime datetime2 NOT NULL DEFAULT SYSUTCDATETIME(),
    ExpGainedTime datetime2 NOT NULL DEFAULT SYSUTCDATETIME(),
    
    FOREIGN KEY (UserID) REFERENCES Users(User_ID)
);
```

### 重要設計原則

- **UTC儲存**: 所有時間欄位使用UTC時間儲存
- **時區轉換**: 應用層負責Asia/Taipei時區轉換
- **外鍵約束**: 確保資料完整性
- **預設值**: 合理的預設值設定

## 🎁 獎勵系統

### 基礎獎勵規則

| 簽到類型 | 點數獎勵 | 經驗獎勵 | 說明 |
|---------|---------|---------|------|
| 平日 (一~五) | +20 | 0 | 基礎平日獎勵 |
| 週末 (六、日) | +30 | +200 | 週末加成獎勵 |

### 特殊獎勵規則

| 獎勵類型 | 觸發條件 | 額外獎勵 | 說明 |
|---------|---------|---------|------|
| 連續7天獎勵 | 連續簽到滿7天 | +40點數, +300經驗 | 當日額外發放 |
| 月度全勤獎勵 | 當月每日都簽到 | +200點數, +2000經驗 | 月末最後一日發放 |

### 獎勵計算邏輯

```csharp
// 基礎獎勵
var baseRewards = IsWeekend(date) 
    ? new { Points = 30, Experience = 200 }  // 週末
    : new { Points = 20, Experience = 0 };   // 平日

// 連續獎勵 (第7天)
if (currentStreak + 1 == 7) {
    totalPoints += 40;
    totalExperience += 300;
}

// 月度全勤獎勵 (月末且全勤)
if (IsLastDayOfMonth(date) && IsPerfectAttendance()) {
    totalPoints += 200;
    totalExperience += 2000;
}
```

## 🔧 API 文件

### 核心API端點

#### 1. 取得簽到狀態
```http
GET /api/signin/status
```

**回應範例:**
```json
{
  "success": true,
  "data": {
    "userId": 123,
    "todaySigned": false,
    "currentStreak": 5,
    "taipeiDate": "2024-08-15",
    "taipeiDateTime": "2024-08-15T14:30:00+08:00",
    "isWeekend": false,
    "canSignToday": true,
    "todayPotentialRewards": {
      "points": 20,
      "experience": 0
    }
  }
}
```

#### 2. 執行簽到
```http
POST /api/signin
```

**回應範例:**
```json
{
  "success": true,
  "data": {
    "success": true,
    "message": "簽到成功！",
    "pointsEarned": 60,
    "experienceGained": 300,
    "streakBefore": 6,
    "streakAfter": 7,
    "hasSevenDayBonus": true,
    "hasMonthlyBonus": false,
    "bonusMessages": ["連續簽到7天獎勵：+40點數 +300經驗"]
  }
}
```

#### 3. 取得月度統計
```http
GET /api/signin/monthly?year=2024&month=8
```

**回應範例:**
```json
{
  "success": true,
  "data": {
    "year": 2024,
    "month": 8,
    "totalSignedDays": 20,
    "totalDaysInMonth": 31,
    "attendanceRate": 64.52,
    "totalPointsEarned": 500,
    "totalExperienceGained": 1200,
    "isPerfectAttendance": false
  }
}
```

#### 4. 取得簽到歷史
```http
GET /api/signin/history?page=1&pageSize=20&fromDate=2024-08-01&toDate=2024-08-31
```

#### 5. 取得簽到日曆
```http
GET /api/signin/calendar?year=2024&month=8
```

### 管理員API

#### 調整簽到記錄
```http
POST /api/signin/admin/adjust
Authorization: Bearer {admin_token}
```

**請求範例:**
```json
{
  "userId": 456,
  "adjustmentDate": "2024-08-15",
  "adjustmentType": "add",
  "reason": "系統維護期間補簽",
  "sendNotification": true
}
```

## 🖥️ 前端介面

### UI設計原則

- **Glass Morphism風格**: 半透明毛玻璃效果
- **響應式設計**: 支援桌面和行動裝置
- **直覺操作**: 簡單明瞭的互動流程
- **即時回饋**: 動畫和狀態顯示

### 主要元件

1. **簽到按鈕**: 大型CTA按鈕，狀態明確
2. **日曆檢視**: 月度簽到狀況視覺化
3. **進度條**: 連續簽到進度顯示
4. **統計卡片**: 月度統計資訊
5. **歷史記錄**: 最近簽到記錄

### 互動流程

```
使用者進入頁面
     ↓
載入簽到狀態
     ↓
顯示今日狀態和潛在獎勵
     ↓
使用者點擊簽到
     ↓
顯示成功動畫和獲得獎勵
     ↓
更新界面狀態
```

## ⚙️ 設定與部署

### 依賴注入設定

```csharp
// Program.cs
builder.Services.AddScoped<IDailySignInService, DailySignInService>();
```

### 必要相依性

- `IWalletService`: 點數管理
- `GameCoreDbContext`: 資料庫存取
- `ILogger`: 日誌記錄

### 環境要求

- .NET 8.0+
- SQL Server 2019+
- Asia/Taipei時區支援

## 🧪 測試指南

### 單元測試

```bash
# 執行所有簽到系統測試
dotnet test --filter "DailySignInControllerTests"

# 執行特定測試
dotnet test --filter "PerformSignIn_ShouldReturnSuccess_WhenSignInSucceeds"
```

### 測試覆蓋範圍

- ✅ 簽到狀態查詢
- ✅ 簽到執行邏輯
- ✅ 獎勵計算
- ✅ 月度統計
- ✅ 歷史記錄
- ✅ 錯誤處理
- ✅ 邊界條件

### 測試資料

使用 `07-DailySignInSeedData.sql` 生成完整測試資料，包含：

- 過去90天的簽到記錄
- 連續簽到獎勵示例
- 月度全勤獎勵記錄
- 不同使用者的簽到模式

## 🔍 疑難排解

### 常見問題

#### 1. 時區問題
**問題**: 簽到時間不正確
**解決**: 確認 `TimeZoneInfo.FindSystemTimeZoneById("Asia/Taipei")` 正常運作

#### 2. 重複簽到
**問題**: 使用者可以重複簽到
**解決**: 檢查 `HasSignedTodayAsync` 方法的UTC時間範圍計算

#### 3. 獎勵計算錯誤
**問題**: 連續獎勵或月度獎勵不正確
**解決**: 檢查 `CalculateCurrentStreakAsync` 和月度全勤檢查邏輯

#### 4. 效能問題
**問題**: 查詢速度慢
**解決**: 
- 對 `UserID` 和 `SignTime` 建立複合索引
- 限制歷史查詢範圍
- 使用分頁查詢

### 監控指標

- 每日簽到率
- 連續簽到天數分布
- 獎勵發放統計
- API回應時間
- 錯誤率

## 📈 效能最佳化

### 資料庫最佳化

```sql
-- 建議的索引
CREATE INDEX IX_UserSignInStats_UserID_SignTime 
ON UserSignInStats (UserID, SignTime);

CREATE INDEX IX_UserSignInStats_SignTime 
ON UserSignInStats (SignTime) 
INCLUDE (UserID, PointsChanged, ExpGained);
```

### 快取策略

- 使用者當日簽到狀態快取 (5分鐘)
- 月度統計快取 (1小時)
- 連續簽到天數快取 (30分鐘)

### 批次處理

- 每日自動統計作業
- 月度獎勵批次發放
- 歷史資料歸檔

## 🚀 未來擴展

### 計劃功能

1. **簽到提醒**: Push通知和Email提醒
2. **簽到挑戰**: 特殊活動和限時挑戰
3. **社交功能**: 好友簽到排行榜
4. **個性化**: 自訂簽到界面和獎勵偏好
5. **分析報表**: 詳細的簽到行為分析

### 擴展建議

- 使用Redis提升快取效能
- 實作分散式鎖防止併發問題
- 加入機器學習預測簽到行為
- 支援多時區使用者

## 📚 相關文件

- [錢包系統指南](./WalletSystemGuide.md)
- [使用者認證指南](./AuthenticationGuide.md)
- [API規格文件](./APIReference.md)
- [資料庫設計文件](./DatabaseDesign.md)

---

*本文件最後更新: 2024年8月15日*
*版本: 1.0.0*
*維護者: GameCore開發團隊*