# GameCore 虛擬寵物系統完整指南

## 📋 系統概述

GameCore虛擬寵物系統是一個完整的史萊姆寵物養成功能，嚴格按照規格實現5維屬性管理、互動行為、等級經驗、換色系統、每日衰減等核心機制。系統設計旨在提供沉浸式的寵物養成體驗，增強使用者黏性和遊戲樂趣。

### 🎯 核心特色

- **一人一寵**: 嚴格執行每位會員僅可擁有一隻史萊姆的業務規則
- **5維屬性系統**: 飢餓值、心情值、體力值、清潔值、健康度的完整管理
- **等級經驗系統**: 分階段升級公式，最高等級250級
- **互動養成**: 餵食、洗澡、玩耍、休息四種基本互動行為
- **換色系統**: 多種顏色選擇，消耗點數進行個性化定制
- **每日衰減**: Asia/Taipei時區的每日自動屬性衰減機制
- **冒險檢查**: 智能判斷寵物是否適合進行冒險活動

## 🏗️ 系統架構

### 三層架構設計

```
┌─────────────────────┐
│   Presentation      │  ← VirtualPetController, Index.cshtml
├─────────────────────┤
│   Business Logic    │  ← PetService, PetDTOs
├─────────────────────┤
│   Data Access       │  ← Pet Entity, DbContext
└─────────────────────┘
```

### 核心元件

1. **VirtualPetController**: RESTful API控制器，提供完整寵物管理端點
2. **PetService**: 業務邏輯服務，實現所有寵物相關功能
3. **PetDTOs**: 資料傳輸物件，涵蓋所有寵物操作的請求和回應
4. **Index.cshtml**: 互動式寵物管理界面，包含實時動畫效果
5. **Pet Entity**: 資料庫實體，對應Pet資料表

## 📊 資料庫設計

### Pet 資料表結構

```sql
CREATE TABLE Pet (
    PetID int IDENTITY(1,1) PRIMARY KEY,
    UserID int NOT NULL,
    PetName nvarchar(50) NOT NULL DEFAULT '小可愛',
    Level int NOT NULL DEFAULT 0,
    LevelUpTime datetime2 NOT NULL DEFAULT SYSUTCDATETIME(),
    Experience int NOT NULL DEFAULT 0,
    Hunger int NOT NULL DEFAULT 0,        -- 飢餓值 (0-100)
    Mood int NOT NULL DEFAULT 0,          -- 心情值 (0-100)  
    Stamina int NOT NULL DEFAULT 0,       -- 體力值 (0-100)
    Cleanliness int NOT NULL DEFAULT 0,   -- 清潔值 (0-100)
    Health int NOT NULL DEFAULT 0,        -- 健康度 (0-100)
    SkinColor nvarchar(50) NOT NULL DEFAULT '#ADD8E6',
    ColorChangedTime datetime2 NOT NULL DEFAULT SYSUTCDATETIME(),
    BackgroundColor nvarchar(50) NOT NULL DEFAULT '粉藍',
    BackgroundColorChangedTime datetime2 NOT NULL DEFAULT SYSUTCDATETIME(),
    PointsChanged int NOT NULL DEFAULT 0,
    PointsChangedTime datetime2 NOT NULL DEFAULT SYSUTCDATETIME(),
    
    FOREIGN KEY (UserID) REFERENCES Users(User_ID)
);
```

### 重要設計原則

- **屬性鉗位**: 所有屬性值嚴格限制在0-100範圍內
- **初始狀態**: 新建寵物時所有5維屬性初始化為100
- **一人一寵**: 業務邏輯層強制執行，資料庫層可考慮唯一約束
- **時間記錄**: 完整記錄各種操作的時間戳記

## 🎮 5維屬性系統

### 屬性定義

| 屬性 | 範圍 | 功能描述 | 互動影響 |
|------|------|----------|----------|
| **飢餓值 (Hunger)** | 0-100 | 寵物的飽食程度 | 餵食 +10 |
| **心情值 (Mood)** | 0-100 | 寵物的情緒狀態 | 玩耍 +10 |
| **體力值 (Stamina)** | 0-100 | 寵物的體力水平 | 休息 +10 |
| **清潔值 (Cleanliness)** | 0-100 | 寵物的衛生狀況 | 洗澡 +10 |
| **健康度 (Health)** | 0-100 | 寵物的整體健康 | 自動計算 |

### 屬性互動規則

```csharp
// 基礎互動效果
餵食: Hunger += 10
洗澡: Cleanliness += 10  
玩耍: Mood += 10
休息: Stamina += 10

// 完美狀態觸發條件
if (Hunger == 100 && Mood == 100 && Stamina == 100 && Cleanliness == 100) {
    Health = 100;
}

// 健康度懲罰規則
if (Hunger < 30) Health -= 20;
if (Cleanliness < 30) Health -= 20;
if (Stamina < 30) Health -= 20;

// 屬性鉗位
所有屬性 = Math.Clamp(value, 0, 100);
```

### 每日衰減機制

每日00:00 (Asia/Taipei時區) 自動執行：

```csharp
Hunger -= 20      // 飢餓值衰減
Mood -= 30        // 心情值衰減  
Stamina -= 10     // 體力值衰減
Cleanliness -= 20 // 清潔值衰減
Health -= 20      // 健康度衰減
```

## 🌟 等級經驗系統

### 升級公式

嚴格按照規格實現的分階段升級需求：

```csharp
// Level 1-10: EXP = 40 × level + 60
public int GetRequiredExp_Level1To10(int level) {
    return 40 * level + 60;
}

// Level 11-100: EXP = 0.8 × level² + 380  
public int GetRequiredExp_Level11To100(int level) {
    return (int)(0.8 * level * level + 380);
}

// Level ≥101: EXP = 285.69 × (1.06^level)
public int GetRequiredExp_Level101Plus(int level) {
    return (int)(285.69 * Math.Pow(1.06, level));
}
```

### 經驗來源

- **互動行為**: 每次互動獲得5經驗值 (可配置)
- **每日簽到**: 週末+200經驗，連續獎勵+300經驗，月度全勤+2000經驗
- **小遊戲**: 根據關卡等級獲得對應經驗值
- **特殊活動**: 系統活動或管理員發放

### 升級獎勵

- **等級上限**: 250級
- **升級獎勵**: 可配置的點數獎勵 (預設關閉)
- **升級通知**: 自動發送升級祝賀通知

## 🎨 換色系統

### 可用顏色選項

| 顏色ID | 顏色名稱 | 膚色值 | 背景色 | 等級需求 | 特殊 |
|--------|----------|--------|--------|----------|------|
| default | 預設淺藍 | #ADD8E6 | 粉藍 | 1 | ✓ |
| pink | 櫻花粉 | #FFB6C1 | 粉紅 | 1 | - |
| green | 薄荷綠 | #98FB98 | 薄荷 | 5 | - |
| yellow | 陽光黃 | #FFFFE0 | 金黃 | 10 | - |
| purple | 夢幻紫 | #DDA0DD | 紫羅蘭 | 15 | - |
| gold | 黃金色 | #FFD700 | 金色 | 50 | ✓ |

### 換色規則

- **費用**: 固定2000點數 (系統可配置)
- **確認機制**: 二次確認防止誤操作
- **即時生效**: 換色成功後立即更新寵物外觀
- **通知記錄**: 自動發送換色成功通知
- **歷史追蹤**: 透過通知系統記錄換色歷史

## 🔧 API 文件

### 核心API端點

#### 1. 取得寵物資訊
```http
GET /api/pet
```

**回應範例:**
```json
{
  "success": true,
  "hasPet": true,
  "data": {
    "petId": 1,
    "userId": 123,
    "petName": "小可愛",
    "level": 15,
    "experience": 2500,
    "hunger": 80,
    "mood": 75,
    "stamina": 90,
    "cleanliness": 85,
    "health": 82,
    "skinColor": "#ADD8E6",
    "backgroundColor": "粉藍",
    "canAdventure": true,
    "petStatus": "快樂",
    "healthStatus": "非常健康",
    "lowStatsWarnings": []
  }
}
```

#### 2. 建立新寵物
```http
POST /api/pet
Content-Type: application/json

{
  "petName": "史萊姆王"
}
```

#### 3. 寵物互動
```http
POST /api/pet/actions/{action}
```

支援的互動類型: `feed`, `bathe`, `play`, `rest`

**回應範例:**
```json
{
  "success": true,
  "data": {
    "success": true,
    "message": "成功餵食寵物！",
    "interactionType": "feed",
    "beforeStats": {
      "hunger": 70,
      "mood": 80,
      "stamina": 85,
      "cleanliness": 75,
      "health": 77
    },
    "afterStats": {
      "hunger": 80,
      "mood": 80,
      "stamina": 85,
      "cleanliness": 75,
      "health": 77
    },
    "statsChange": {
      "hungerChange": 10,
      "moodChange": 0,
      "staminaChange": 0,
      "cleanlinessChange": 0,
      "healthChange": 0,
      "experienceChange": 5
    },
    "leveledUp": false,
    "perfectCondition": false,
    "experienceGained": 5
  }
}
```

#### 4. 寵物換色
```http
POST /api/pet/recolor
Content-Type: application/json

{
  "skinColor": "#FFB6C1",
  "backgroundColor": "粉紅",
  "confirmPayment": true
}
```

#### 5. 檢查冒險準備度
```http
GET /api/pet/adventure/readiness
```

**回應範例:**
```json
{
  "success": true,
  "data": {
    "canAdventure": false,
    "message": "寵物狀態不佳，需要先照料",
    "currentHealth": 25,
    "blockingReasons": [
      "寵物健康度過低",
      "寵物飢餓值為0"
    ],
    "suggestedActions": [
      "與寵物互動提升健康度",
      "餵食寵物"
    ]
  }
}
```

### 管理員API

#### 1. 取得系統設定
```http
GET /api/pet/admin/config
Authorization: Bearer {admin_token}
```

#### 2. 重置寵物狀態
```http
POST /api/pet/admin/reset/{petId}
Authorization: Bearer {admin_token}
Content-Type: application/json

{
  "resetType": "stats",
  "reason": "系統維護重置",
  "sendNotification": true
}
```

## 🖥️ 前端介面

### UI設計原則

- **Glass Morphism風格**: 半透明毛玻璃效果，與系統整體設計一致
- **即時動畫**: 史萊姆寵物的跳躍、眨眼動畫效果
- **直覺操作**: 大型互動按鈕，清晰的狀態顯示
- **響應式設計**: 支援桌面和行動裝置
- **視覺回饋**: 互動後的即時狀態更新和成功提示

### 主要元件

1. **寵物顯示區**: 3D史萊姆動畫，顯示當前顏色和狀態
2. **屬性面板**: 5維屬性的視覺化進度條
3. **互動按鈕**: 餵食、洗澡、玩耍、休息四個主要操作
4. **等級顯示**: 當前等級和經驗進度條
5. **換色面板**: 顏色選擇和換色操作
6. **統計資訊**: 寵物年齡、互動次數等統計

### 互動流程

```
使用者進入寵物頁面
     ↓
載入寵物狀態資料
     ↓
顯示寵物動畫和屬性
     ↓
使用者執行互動操作
     ↓
發送API請求
     ↓
顯示互動結果和動畫
     ↓
更新寵物狀態顯示
```

## ⚙️ 設定與部署

### 依賴注入設定

```csharp
// Program.cs
builder.Services.AddScoped<IPetService, PetService>();
```

### 必要相依性

- `IWalletService`: 點數管理 (換色扣款)
- `INotificationService`: 通知發送
- `GameCoreDbContext`: 資料庫存取
- `ILogger`: 日誌記錄

### 系統設定

```csharp
public class PetSystemConfigDto
{
    public int RecolorCost { get; set; } = 2000;           // 換色費用
    public int MaxLevel { get; set; } = 250;               // 最大等級
    public bool EnableUpgradeRewards { get; set; } = false; // 升級獎勵
    public PetDailyDecayConfig DailyDecay { get; set; }    // 每日衰減設定
    public PetInteractionConfig InteractionGains { get; set; } // 互動增益設定
}
```

## 🧪 測試指南

### 單元測試

```bash
# 執行所有寵物系統測試
dotnet test --filter "VirtualPetControllerTests"

# 執行特定功能測試
dotnet test --filter "FeedPet_ShouldReturnInteractionResult"
```

### 測試覆蓋範圍

- ✅ 寵物建立和管理
- ✅ 5維屬性互動
- ✅ 等級經驗系統
- ✅ 換色功能
- ✅ 冒險準備度檢查
- ✅ 每日衰減處理
- ✅ 統計和排行
- ✅ 管理員功能
- ✅ 錯誤處理
- ✅ 邊界條件

### 測試資料

使用 `08-PetSystemSeedData.sql` 生成完整測試資料，包含：

- 50隻不同狀態的寵物
- 多種等級和經驗分布
- 不同顏色和換色歷史
- 健康和非健康狀態示例
- 完美狀態寵物範例

## 🔍 疑難排解

### 常見問題

#### 1. 一人一寵規則違反
**問題**: 使用者嘗試建立第二隻寵物
**解決**: 檢查 `CreatePetAsync` 方法的一人一寵檢查邏輯

#### 2. 屬性值超出範圍
**問題**: 屬性值不在0-100範圍內
**解決**: 確認 `ClampPetStats` 方法被正確調用

#### 3. 等級計算錯誤
**問題**: 升級需求經驗計算不正確
**解決**: 檢查 `CalculateRequiredExperience` 方法的公式實現

#### 4. 每日衰減未執行
**問題**: 寵物屬性沒有每日衰減
**解決**: 確認定時作業正確設定Asia/Taipei時區

### 監控指標

- 寵物建立率
- 互動頻率分布
- 等級分布統計
- 換色次數統計
- 健康度分布
- API回應時間

## 📈 效能最佳化

### 資料庫最佳化

```sql
-- 建議的索引
CREATE INDEX IX_Pet_UserID 
ON Pet (UserID);

CREATE INDEX IX_Pet_Level_Experience 
ON Pet (Level, Experience);

CREATE INDEX IX_Pet_Health 
ON Pet (Health) 
WHERE Health < 30; -- 針對不健康寵物的查詢
```

### 快取策略

- 寵物基本資訊快取 (10分鐘)
- 顏色選項快取 (1小時)
- 排行榜快取 (30分鐘)
- 系統設定快取 (24小時)

### 批次處理

- 每日衰減批次處理
- 經驗統計批次計算
- 排行榜定期更新

## 🚀 未來擴展

### 計劃功能

1. **寵物技能系統**: 根據屬性解鎖特殊技能
2. **寵物繁殖**: 兩隻寵物的後代系統
3. **寵物裝備**: 可穿戴道具增強屬性
4. **寵物競賽**: 多人寵物對戰功能
5. **成就系統**: 寵物養成成就和徽章

### 擴展建議

- 實作寵物AI行為模式
- 加入更多互動動畫效果
- 支援自訂寵物外觀素材
- 實作寵物社交功能

## 📚 相關文件

- [每日簽到系統指南](./DailySignInSystemGuide.md)
- [錢包系統指南](./WalletSystemGuide.md)
- [通知系統指南](./NotificationSystemGuide.md)
- [API規格文件](./APIReference.md)

---

*本文件最後更新: 2024年8月15日*
*版本: 1.0.0*
*維護者: GameCore開發團隊*