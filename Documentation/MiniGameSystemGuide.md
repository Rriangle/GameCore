# GameCore 小遊戲系統完整指南

## 📋 系統概述

GameCore小遊戲系統是一個完整的出發冒險功能，嚴格按照規格實現每日次數限制、關卡戰鬥、屬性變化、獎勵發放等核心機制。系統設計旨在提供豐富的遊戲體驗，與寵物系統深度整合，增強使用者參與度和平台黏性。

### 🎯 核心特色

- **每日限制**: 嚴格執行每日最多3次遊戲的業務規則，採用Asia/Taipei時區
- **關卡進度**: 勝利升級、失敗保持的關卡系統，最高支援100級
- **寵物整合**: 與寵物健康檢查深度整合，遊戲結果影響寵物5維屬性
- **獎勵系統**: 經驗值和點數獎勵，與錢包系統無縫整合
- **統計分析**: 完整的遊戲記錄、統計分析和排行榜功能
- **中斷處理**: 支援遊戲中斷且不計入每日次數

## 🏗️ 系統架構

### 三層架構設計

```
┌─────────────────────┐
│   Presentation      │  ← MiniGameController, Index.cshtml
├─────────────────────┤
│   Business Logic    │  ← MiniGameService, MiniGameDTOs
├─────────────────────┤
│   Data Access       │  ← MiniGame Entity, DbContext
└─────────────────────┘
```

### 核心元件

1. **MiniGameController**: RESTful API控制器，提供完整遊戲管理端點
2. **MiniGameService**: 業務邏輯服務，實現所有遊戲相關功能
3. **MiniGameDTOs**: 資料傳輸物件，涵蓋所有遊戲操作的請求和回應
4. **Index.cshtml**: 遊戲界面，包含關卡選擇和遊戲進行
5. **MiniGame Entity**: 資料庫實體，對應MiniGame資料表

## 📊 資料庫設計

### MiniGame 資料表結構

```sql
CREATE TABLE MiniGame (
    GameID int IDENTITY(1,1) PRIMARY KEY,
    UserID int NOT NULL,
    Level int NOT NULL,
    MonsterCount int NOT NULL,
    SpeedMultiplier decimal(3,2) NOT NULL,
    StartTime datetime2 NOT NULL,
    EndTime datetime2 NULL,
    DurationMinutes decimal(5,2) NULL,
    Result bit NULL,                    -- NULL=進行中, 1=勝利, 0=失敗
    Aborted bit NOT NULL DEFAULT 0,    -- 是否中斷
    MonstersDefeated int NOT NULL DEFAULT 0,
    FinalScore int NOT NULL DEFAULT 0,
    ExpGained int NOT NULL DEFAULT 0,
    PointsChanged int NOT NULL DEFAULT 0,
    
    FOREIGN KEY (UserID) REFERENCES Users(User_ID)
);
```

### 重要設計原則

- **每日限制檢查**: 透過StartTime和Aborted=0統計當日遊戲次數
- **時區一致性**: 所有時間欄位使用UTC，日界線計算採用Asia/Taipei
- **結果記錄**: Result=NULL表示遊戲進行中，完成後更新為勝負結果
- **中斷處理**: Aborted=1的記錄不計入每日次數限制

## 🎮 遊戲機制

### 每日次數限制

按照規格嚴格實現：

```csharp
// 每日限制：3次 (Asia/Taipei時區)
var today = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TaipeiTimeZone).Date;
var tomorrowUtc = TimeZoneInfo.ConvertTimeToUtc(today.AddDays(1), TaipeiTimeZone);

var todayPlayCount = await _context.MiniGames
    .Where(g => g.UserID == userId && 
                g.StartTime >= TimeZoneInfo.ConvertTimeToUtc(today, TaipeiTimeZone) &&
                g.StartTime < tomorrowUtc &&
                !g.Aborted) // 中斷遊戲不計入
    .CountAsync();
```

### 關卡進度系統

```csharp
// 關卡進度規則
初次冒險: Level = 1
勝利: NextLevel = min(CurrentLevel + 1, 100)
失敗: NextLevel = CurrentLevel (保持不變)
```

### 預設關卡設定

| 關卡 | 怪物數 | 速度倍率 | 勝利獎勵 |
|------|--------|----------|----------|
| 1 | 6 | 1.0x (基礎) | +100 exp, +10 點數 |
| 2 | 8 | 1.2x (加快) | +200 exp, +20 點數 |
| 3 | 10 | 1.5x (再加快) | +300 exp, +30 點數 |
| 4+ | 6+(關卡×2) | 1.0+(關卡×0.1) | +(關卡×100) exp, +(關卡×10) 點數 |

### 寵物屬性影響

按照規格實現的屬性變化：

| 遊戲結果 | 飢餓值 | 心情值 | 體力值 | 清潔值 |
|----------|--------|--------|--------|--------|
| **勝利** | -20 | **+30** | -20 | -20 |
| **失敗** | -20 | **-30** | -20 | -20 |

遊戲完成後執行健康度檢查：
- 飢餓值 < 30 → 健康度 -20
- 清潔值 < 30 → 健康度 -20  
- 體力值 < 30 → 健康度 -20

### 冒險阻止條件

嚴格按照規格實現：
- 健康度 = 0 → 禁止開始遊戲
- 任一屬性 = 0 → 禁止開始遊戲
- 每日次數已達3次 → 禁止開始遊戲

## 🔧 API 文件

### 核心API端點

#### 1. 檢查遊戲資格
```http
GET /api/minigame/eligibility
```

**回應範例:**
```json
{
  "success": true,
  "data": {
    "canPlay": true,
    "message": "可以開始冒險！",
    "todayPlayCount": 1,
    "dailyLimit": 3,
    "remainingPlays": 2,
    "petHealthy": true,
    "blockingReasons": [],
    "suggestedActions": [],
    "nextPlayTime": null
  }
}
```

#### 2. 開始新遊戲
```http
POST /api/minigame/start
```

**回應範例:**
```json
{
  "success": true,
  "data": {
    "gameId": 456,
    "userId": 123,
    "level": 2,
    "monsterCount": 8,
    "speedMultiplier": 1.2,
    "expectedReward": {
      "experience": 200,
      "points": 20
    },
    "startTime": "2024-08-15T10:30:00Z",
    "status": "進行中",
    "gameTips": "速度加快了，保持專注！"
  },
  "message": "冒險開始！關卡 2"
}
```

#### 3. 完成遊戲
```http
POST /api/minigame/finish/{gameId}
Content-Type: application/json

{
  "isVictory": true,
  "durationSeconds": 300,
  "monstersDefeated": 8,
  "finalScore": 1200,
  "gameNotes": "完美通關！"
}
```

**回應範例:**
```json
{
  "success": true,
  "data": {
    "gameId": 456,
    "isVictory": true,
    "resultMessage": "恭喜！成功通過關卡 2！",
    "reward": {
      "experience": 200,
      "points": 20
    },
    "petStatsChange": {
      "hungerChange": -20,
      "moodChange": 30,
      "staminaChange": -20,
      "cleanlinessChange": -20,
      "healthChange": 0
    },
    "nextLevel": 3,
    "canContinue": true,
    "duration": "00:05:00",
    "remainingPlaysToday": 2
  }
}
```

#### 4. 中斷遊戲
```http
POST /api/minigame/abort/{gameId}
Content-Type: application/json

{
  "reason": "網路連線中斷"
}
```

#### 5. 遊戲記錄查詢
```http
GET /api/minigame/records?from=2024-08-01&to=2024-08-15&result=true&page=1&pageSize=20
```

**回應範例:**
```json
{
  "success": true,
  "data": {
    "page": 1,
    "pageSize": 20,
    "totalCount": 45,
    "totalPages": 3,
    "data": [
      {
        "gameId": 123,
        "level": 2,
        "isVictory": true,
        "monstersDefeated": 8,
        "finalScore": 1200,
        "expGained": 200,
        "pointsGained": 20,
        "startTime": "2024-08-15T10:30:00Z",
        "endTime": "2024-08-15T10:35:00Z",
        "duration": "00:05:00"
      }
    ]
  }
}
```

#### 6. 遊戲統計
```http
GET /api/minigame/statistics
```

**回應範例:**
```json
{
  "success": true,
  "data": {
    "totalGames": 25,
    "totalVictories": 18,
    "totalDefeats": 7,
    "winRate": 72.0,
    "highestLevel": 5,
    "currentLevel": 3,
    "totalExperienceEarned": 2500,
    "totalPointsEarned": 250,
    "bestScore": 1800,
    "averageScore": 1200,
    "longestWinStreak": 5,
    "currentWinStreak": 2
  }
}
```

#### 7. 當日遊戲狀態
```http
GET /api/minigame/daily-status
```

**回應範例:**
```json
{
  "success": true,
  "data": {
    "date": "2024-08-15",
    "todayPlayCount": 2,
    "dailyLimit": 3,
    "remainingPlays": 1,
    "canPlay": true,
    "todayTotalExp": 300,
    "todayTotalPoints": 30,
    "todayVictories": 2,
    "nextResetTime": "2024-08-16T00:00:00+08:00"
  }
}
```

### 管理員API

#### 1. 系統設定查詢
```http
GET /api/minigame/admin/config
Authorization: Bearer {admin_token}
```

#### 2. 全域遊戲記錄查詢
```http
GET /api/minigame/admin/records?userId=123&username=player1&page=1
Authorization: Bearer {admin_token}
```

## 🖥️ 前端介面

### UI設計原則

- **Glass Morphism風格**: 與系統整體設計一致的半透明毛玻璃效果
- **關卡選擇**: 清晰的關卡難度展示和選擇界面
- **即時回饋**: 遊戲過程中的即時狀態顯示和結果反饋
- **響應式設計**: 支援桌面和行動裝置
- **統計視覺化**: 豐富的統計圖表和排行榜展示

### 主要元件

1. **遊戲大廳**: 關卡選擇、每日狀態、統計總覽
2. **遊戲進行**: Canvas遊戲區域、計時器、分數顯示
3. **結果展示**: 勝負結果、獎勵獲得、屬性變化
4. **記錄查詢**: 歷史記錄、篩選功能、分頁顯示
5. **排行榜**: 多種排行類型、即時更新

### 互動流程

```
使用者進入遊戲頁面
     ↓
檢查遊戲資格
     ↓
選擇關卡難度
     ↓
開始遊戲 (建立記錄)
     ↓
遊戲進行 (Canvas互動)
     ↓
完成/中斷遊戲
     ↓
結算獎勵和寵物屬性
     ↓
顯示結果和統計
```

## ⚙️ 設定與部署

### 依賴注入設定

```csharp
// Program.cs
builder.Services.AddScoped<IMiniGameService, MiniGameService>();
```

### 必要相依性

- `IPetService`: 寵物狀態檢查和屬性更新
- `IWalletService`: 點數獎勵發放
- `INotificationService`: 遊戲結果通知
- `GameCoreDbContext`: 資料庫存取
- `ILogger`: 日誌記錄

### 系統設定

```csharp
public class MiniGameSystemConfigDto
{
    public int DailyPlayLimit { get; set; } = 3;           // 每日遊戲次數限制
    public bool EnableDailyReset { get; set; } = true;     // 啟用每日重置
    public List<GameLevelConfigDto> LevelConfigs { get; set; } // 關卡設定
    public PetStatsEffectConfigDto StatsEffectConfig { get; set; } // 寵物屬性影響
    public bool MaintenanceMode { get; set; } = false;     // 維護模式
}
```

## 🧪 測試指南

### 單元測試

```bash
# 執行所有小遊戲測試
dotnet test --filter "MiniGameControllerTests"

# 執行特定功能測試
dotnet test --filter "StartGame_ShouldReturnSuccess"
```

### 測試覆蓋範圍

- ✅ 遊戲資格檢查 (每日限制、寵物健康)
- ✅ 遊戲開始和完成流程
- ✅ 中斷遊戲處理
- ✅ 記錄查詢和分頁
- ✅ 統計計算和排行榜
- ✅ 管理員功能
- ✅ 錯誤處理和邊界條件

### 測試資料

使用 `09-MiniGameSeedData.sql` 生成完整測試資料，包含：

- 50名使用者的遊戲記錄
- 多樣化的關卡分布和勝負結果
- 特殊情況記錄 (VIP高等級、今日遊戲、中斷記錄)
- 完整的統計數據和排行榜資料

## 🔍 疑難排解

### 常見問題

#### 1. 每日次數限制不準確
**問題**: 使用者反映每日次數計算錯誤
**解決**: 檢查Asia/Taipei時區設定和日界線計算邏輯

#### 2. 寵物屬性未正確更新
**問題**: 遊戲完成後寵物屬性變化不符預期
**解決**: 確認屬性變化規則和健康檢查邏輯

#### 3. 關卡進度異常
**問題**: 勝利後關卡未提升或失敗後關卡下降
**解決**: 檢查關卡進度計算邏輯和最後勝利記錄查詢

#### 4. 中斷遊戲計入每日次數
**問題**: 中斷的遊戲被計入每日限制
**解決**: 確認Aborted標記設定和查詢條件

### 監控指標

- 每日遊戲參與率
- 關卡通過率分析
- 平均遊戲時長
- 中斷率統計
- 寵物健康狀態分布
- 獎勵發放統計

## 📈 效能最佳化

### 資料庫最佳化

```sql
-- 建議的索引
CREATE INDEX IX_MiniGame_UserID_StartTime 
ON MiniGame (UserID, StartTime);

CREATE INDEX IX_MiniGame_Level_Result 
ON MiniGame (Level, Result) 
WHERE Aborted = 0;

CREATE INDEX IX_MiniGame_StartTime_Aborted 
ON MiniGame (StartTime, Aborted);
```

### 快取策略

- 關卡設定快取 (1小時)
- 使用者當前關卡快取 (30分鐘)
- 排行榜快取 (15分鐘)
- 系統設定快取 (24小時)

### 批次處理

- 每日重置批次處理
- 統計計算批次更新
- 排行榜定期重建

## 🚀 未來擴展

### 計劃功能

1. **多人模式**: 協作冒險或競技對戰
2. **特殊事件**: 限時關卡和節慶活動
3. **道具系統**: 增益道具和特殊效果
4. **成就系統**: 遊戲成就和里程碑獎勵
5. **公會功能**: 公會內部排行和團體活動

### 技術擴展

- 實作即時多人對戰
- 加入更豐富的遊戲機制
- 支援自訂關卡創建
- 實作遊戲重播功能

## 📚 相關文件

- [虛擬寵物系統指南](./VirtualPetSystemGuide.md)
- [錢包系統指南](./WalletSystemGuide.md)
- [每日簽到系統指南](./DailySignInSystemGuide.md)
- [API規格文件](./APIReference.md)

---

*本文件最後更新: 2024年8月15日*
*版本: 1.0.0*
*維護者: GameCore開發團隊*