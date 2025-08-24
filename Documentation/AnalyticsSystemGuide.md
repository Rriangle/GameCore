# GameCore 分析系統完整指南

## 📋 系統概述

GameCore分析系統是一個完整的遊戲熱度監測和數據洞察平台，嚴格按照規格實現每日指標收集、熱度指數計算、排行榜快照、洞察貼文等核心分析功能。系統設計旨在提供全面的遊戲表現監控，支援多來源數據整合、自動化指數計算、定期榜單產製等功能，建立科學的遊戲熱度評估體系。

### 🎯 核心特色

- **多來源數據整合**: 支援Steam、Twitch、YouTube等10+外部平台數據收集
- **自動化指數計算**: 每日自動計算加權熱度指數，反映遊戲真實熱度
- **定期榜單產製**: 自動生成日榜、週榜快照，提供歷史趨勢追蹤
- **智能洞察生成**: 基於數據變化自動生成洞察貼文和分析報告
- **實時監控儀表板**: 綜合展示關鍵指標和趨勢變化
- **靈活配置管理**: 支援指標權重調整和計算參數優化

## 🏗️ 系統架構

### 數據流架構設計

```
外部數據源 → 數據清洗 → 每日指標 → 指數計算 → 榜單快照 → 洞察生成
     ↓           ↓          ↓         ↓         ↓         ↓
   API抓取    格式標準化   UPSERT    加權計算   排序快照   智能分析
```

### 三層架構實現

```
┌─────────────────────┐
│   Presentation      │  ← AnalyticsController, Analytics Views
├─────────────────────┤
│   Business Logic    │  ← AnalyticsService, AnalyticsDTOs
├─────────────────────┤
│   Data Access       │  ← Analytics Entities, DbContext
└─────────────────────┘
```

### 核心元件

1. **AnalyticsController**: RESTful API控制器，提供完整分析管理端點
2. **IAnalyticsService**: 業務邏輯服務介面，定義完整分析功能契約
3. **AnalyticsService**: 業務邏輯實現，包含所有分析相關功能
4. **AnalyticsDTOs**: 資料傳輸物件，涵蓋所有分析操作的請求和回應
5. **Analytics Views**: 分析界面，包含儀表板、排行榜、洞察展示
6. **Analytics Entities**: 資料庫實體，對應分析相關資料表

## 📊 資料庫設計

### 核心資料表結構

#### games (遊戲表)
```sql
CREATE TABLE games (
    game_id int IDENTITY(1,1) PRIMARY KEY,
    game_name nvarchar(100) NOT NULL,        -- 遊戲名稱
    game_description nvarchar(500) NULL,     -- 遊戲描述
    is_active bit NOT NULL DEFAULT 1,        -- 是否啟用
    created_at datetime2 NOT NULL            -- 建立時間
);
```

#### metric_sources (指標來源表)
```sql
CREATE TABLE metric_sources (
    source_id int IDENTITY(1,1) PRIMARY KEY,
    source_name nvarchar(100) NULL,          -- 來源名稱
    api_endpoint nvarchar(500) NULL,         -- API端點
    is_active bit NOT NULL DEFAULT 1,        -- 是否啟用
    created_at datetime2 NOT NULL            -- 建立時間
);
```

#### metrics (指標表)
```sql
CREATE TABLE metrics (
    metric_id int IDENTITY(1,1) PRIMARY KEY,
    source_id int NOT NULL,                  -- 來源ID (FK)
    code varchar(50) NOT NULL,               -- 指標代碼
    unit varchar(20) NULL,                   -- 單位
    description nvarchar(200) NULL,          -- 指標說明
    is_active bit NOT NULL DEFAULT 1,        -- 是否啟用
    created_at datetime2 NOT NULL,           -- 建立時間
    
    UNIQUE (source_id, code)                 -- 同一來源下代碼唯一
);
```

#### game_source_map (遊戲來源對應表)
```sql
CREATE TABLE game_source_map (
    map_id int IDENTITY(1,1) PRIMARY KEY,
    game_id int NOT NULL,                    -- 遊戲ID (FK)
    source_id int NOT NULL,                  -- 來源ID (FK)
    external_id nvarchar(100) NOT NULL,      -- 外部ID
    is_active bit NOT NULL DEFAULT 1,        -- 是否啟用
    created_at datetime2 NOT NULL            -- 建立時間
);
```

#### game_metric_daily (每日指標數據表)
```sql
CREATE TABLE game_metric_daily (
    id int IDENTITY(1,1) PRIMARY KEY,
    game_id int NOT NULL,                    -- 遊戲ID (FK)
    metric_id int NOT NULL,                  -- 指標ID (FK)
    date date NOT NULL,                      -- 日期
    value decimal(18,4) NOT NULL,            -- 數值
    agg_method varchar(20) NULL,             -- 聚合方法
    created_at datetime2 NOT NULL,           -- 建立時間
    updated_at datetime2 NOT NULL,           -- 更新時間
    
    UNIQUE (game_id, metric_id, date)        -- UPSERT 防重
);
```

#### popularity_index_daily (每日熱度指數表)
```sql
CREATE TABLE popularity_index_daily (
    id bigint IDENTITY(1,1) PRIMARY KEY,
    game_id int NOT NULL,                    -- 遊戲ID (FK)
    date date NOT NULL,                      -- 日期
    index_value decimal(18,4) NOT NULL,      -- 熱度指數
    created_at datetime2 NOT NULL,           -- 建立時間
    
    UNIQUE (game_id, date)                   -- 每日每遊戲唯一
);
```

#### leaderboard_snapshots (排行榜快照表)
```sql
CREATE TABLE leaderboard_snapshots (
    snapshot_id bigint IDENTITY(1,1) PRIMARY KEY,
    period varchar(20) NOT NULL,             -- 期間類型
    ts datetime2 NOT NULL,                   -- 快照時間
    game_id int NOT NULL,                    -- 遊戲ID (FK)
    rank int NOT NULL,                       -- 排名
    score decimal(18,4) NOT NULL,            -- 分數
    created_at datetime2 NOT NULL,           -- 建立時間
    
    UNIQUE (period, ts, rank, game_id)       -- 防重複快照
);
```

#### posts (洞察貼文表)
```sql
CREATE TABLE posts (
    post_id int IDENTITY(1,1) PRIMARY KEY,
    game_id int NULL,                        -- 遊戲ID (FK)
    title nvarchar(200) NOT NULL,            -- 標題
    content nvarchar(max) NOT NULL,          -- 內容
    status nvarchar(20) NOT NULL,            -- 狀態
    pinned bit NOT NULL DEFAULT 0,           -- 是否置頂
    created_at datetime2 NOT NULL,           -- 建立時間
    published_at datetime2 NULL              -- 發佈時間
);
```

#### post_metric_snapshot (貼文指標快照表)
```sql
CREATE TABLE post_metric_snapshot (
    snapshot_id int IDENTITY(1,1) PRIMARY KEY,
    post_id int NOT NULL,                    -- 貼文ID (FK)
    game_id int NULL,                        -- 遊戲ID (FK)
    date date NOT NULL,                      -- 快照日期
    index_value decimal(18,4) NOT NULL,      -- 當日指數
    details_json nvarchar(max) NULL,         -- 詳細資訊
    created_at datetime2 NOT NULL            -- 建立時間
);
```

#### post_sources (貼文來源表)
```sql
CREATE TABLE post_sources (
    source_id int IDENTITY(1,1) PRIMARY KEY,
    post_id int NOT NULL,                    -- 貼文ID (FK)
    source_url nvarchar(500) NOT NULL,       -- 來源URL
    source_title nvarchar(200) NULL,         -- 來源標題
    source_type nvarchar(50) NULL,           -- 來源類型
    created_at datetime2 NOT NULL            -- 建立時間
);
```

### 重要設計原則

- **數據分層架構**: 原始數據 → 清洗數據 → 計算指數 → 快照存儲
- **UPSERT防重機制**: game_metric_daily使用複合唯一鍵防止重複插入
- **時間序列優化**: 按日期分區，支援高效的時間範圍查詢
- **外部ID對應**: game_source_map管理內部遊戲與外部平台的映射關係

## 📈 分析功能

### 遊戲管理

#### 遊戲基礎資訊管理

```csharp
// 建立遊戲
var createDto = new CreateGameDto
{
    GameName = "原神",
    GameDescription = "開放世界冒險RPG遊戲",
    IsActive = true
};

var result = await analyticsService.CreateGameAsync(createDto);

// 查詢遊戲列表
var games = await analyticsService.GetGamesAsync(activeOnly: true, page: 1, pageSize: 20);

// 遊戲詳細資訊（包含最新熱度）
var gameDetail = await analyticsService.GetGameDetailAsync(gameId: 1);
```

### 指標來源管理

#### 多平台數據源配置

```csharp
// 建立指標來源
var sourceDto = new CreateMetricSourceDto
{
    SourceName = "Steam",
    ApiEndpoint = "https://api.steampowered.com/ISteamUserStats/GetNumberOfCurrentPlayers/v1/",
    IsActive = true
};

await analyticsService.CreateMetricSourceAsync(sourceDto);

// 建立指標定義
var metricDto = new CreateMetricDto
{
    SourceId = 1,
    Code = "concurrent_users",
    Unit = "users",
    Description = "Steam平台同時在線玩家數",
    Weight = 3.0m  // 高權重指標
};

await analyticsService.CreateMetricAsync(metricDto);
```

### 每日數據收集

#### 批量數據導入與UPSERT

```csharp
// 單筆數據創建
var dailyMetric = new CreateGameMetricDailyDto
{
    GameId = 1,
    MetricId = 1,
    Date = DateTime.Today,
    Value = 45000.0m,
    AggMethod = "sum"
};

await analyticsService.CreateGameMetricDailyAsync(dailyMetric);

// 批量數據導入
var batchDto = new BatchCreateGameMetricDailyDto
{
    Metrics = new List<CreateGameMetricDailyDto>
    {
        new() { GameId = 1, MetricId = 1, Date = DateTime.Today, Value = 45000 },
        new() { GameId = 1, MetricId = 2, Date = DateTime.Today, Value = 12000 },
        new() { GameId = 2, MetricId = 1, Date = DateTime.Today, Value = 38000 }
    },
    OverwriteExisting = true
};

await analyticsService.BatchCreateGameMetricDailyAsync(batchDto);
```

### 熱度指數計算

#### 加權指數自動計算

```csharp
// 計算特定日期的熱度指數
var result = await analyticsService.CalculatePopularityIndexAsync(
    date: DateTime.Today, 
    gameIds: null  // null為全部遊戲
);

// 指數計算公式
/*
1. 取得各遊戲當日所有指標數據
2. 對每個指標值進行對數正規化: normalized = log(1 + value)
3. 按指標權重加權求和: weighted_sum = Σ(normalized_value × weight)
4. 計算最終指數: index = weighted_sum / total_weight
5. 存入 popularity_index_daily 表
*/

// 查詢熱度指數數據
var indexData = await analyticsService.GetPopularityIndexDailyAsync(
    gameId: 1, 
    startDate: DateTime.Today.AddDays(-7), 
    endDate: DateTime.Today
);

// 取得即時熱度排行榜
var ranking = await analyticsService.GetPopularityRankingAsync(
    date: DateTime.Today, 
    topN: 10
);
```

### 排行榜快照系統

#### 定期榜單產製

```csharp
// 生成日榜快照
await analyticsService.GenerateLeaderboardSnapshotAsync(
    period: "daily", 
    ts: DateTime.Today
);

// 生成週榜快照
await analyticsService.GenerateLeaderboardSnapshotAsync(
    period: "weekly", 
    ts: DateTime.Today
);

// 查詢排行榜快照
var queryDto = new LeaderboardQueryDto
{
    Period = "daily",
    Date = DateTime.Today,
    TopN = 20,
    IncludeChanges = true
};

var snapshot = await analyticsService.GetLeaderboardSnapshotAsync(queryDto);

// 回補歷史快照
await analyticsService.BackfillLeaderboardSnapshotsAsync(
    period: "daily",
    startDate: DateTime.Today.AddDays(-30),
    endDate: DateTime.Today
);
```

### 洞察貼文管理

#### 數據驅動的洞察生成

```csharp
// 建立洞察貼文
var postDto = new CreateInsightPostDto
{
    GameId = 1,
    Title = "原神熱度飆升！玩家數量創新高",
    Content = "根據最新數據顯示，原神在各大平台的熱度指標都呈現顯著上升趨勢...",
    PublishNow = true,
    SourceUrls = new List<string>
    {
        "https://steamcharts.com/app/100001",
        "https://twitchtracker.com/games/1"
    }
};

var result = await analyticsService.CreateInsightPostAsync(postDto);

// 發佈時自動快照指標數據
await analyticsService.PublishInsightPostAsync(postId: result.Data.PostId);

// 設定貼文置頂
await analyticsService.SetInsightPostPinnedAsync(postId: result.Data.PostId, isPinned: true);
```

## 🔧 API 文件

### 核心API端點

#### 1. 遊戲管理 API

```http
# 取得遊戲列表
GET /api/analytics/games?activeOnly=true&page=1&pageSize=20

# 取得遊戲詳細資訊
GET /api/analytics/games/{id}

# 建立遊戲 (管理員限定)
POST /api/analytics/games
{
  "gameName": "新遊戲",
  "gameDescription": "遊戲描述",
  "isActive": true
}

# 更新遊戲 (管理員限定)
PUT /api/analytics/games/{id}
{
  "gameName": "更新的遊戲名稱",
  "isActive": true
}
```

#### 2. 指標管理 API

```http
# 取得指標來源列表
GET /api/analytics/metric-sources?activeOnly=true

# 建立指標來源 (管理員限定)
POST /api/analytics/metric-sources
{
  "sourceName": "新平台",
  "apiEndpoint": "https://api.example.com/",
  "isActive": true
}

# 取得指標列表
GET /api/analytics/metrics?sourceId=1&activeOnly=true

# 建立指標 (管理員限定)
POST /api/analytics/metrics
{
  "sourceId": 1,
  "code": "new_metric",
  "unit": "count",
  "description": "新指標描述",
  "weight": 2.0
}
```

#### 3. 每日數據管理 API

```http
# 取得每日指標數據
GET /api/analytics/game-metric-daily?gameId=1&metricId=1&startDate=2024-01-01&endDate=2024-01-31

# 建立每日指標數據
POST /api/analytics/game-metric-daily
{
  "gameId": 1,
  "metricId": 1,
  "date": "2024-01-15",
  "value": 50000,
  "aggMethod": "sum"
}

# 批量建立每日指標數據
POST /api/analytics/game-metric-daily/batch
{
  "metrics": [
    {"gameId": 1, "metricId": 1, "date": "2024-01-15", "value": 50000},
    {"gameId": 1, "metricId": 2, "date": "2024-01-15", "value": 15000}
  ],
  "overwriteExisting": true
}

# 取得時間序列數據
POST /api/analytics/time-series
{
  "startDate": "2024-01-01",
  "endDate": "2024-01-31",
  "gameIds": [1, 2, 3],
  "metricIds": [1, 2],
  "granularity": "daily"
}
```

#### 4. 熱度指數 API

```http
# 計算熱度指數
POST /api/analytics/popularity-index/calculate
{
  "date": "2024-01-15",
  "gameIds": [1, 2, 3]
}

# 取得熱度指數數據
GET /api/analytics/popularity-index?gameId=1&startDate=2024-01-01&endDate=2024-01-31

# 取得熱度排行榜
GET /api/analytics/popularity-ranking?date=2024-01-15&topN=10
```

#### 5. 排行榜快照 API

```http
# 生成排行榜快照
POST /api/analytics/leaderboard-snapshot/generate
{
  "period": "daily",
  "ts": "2024-01-15T00:00:00Z"
}

# 取得排行榜快照
POST /api/analytics/leaderboard-snapshot
{
  "period": "daily",
  "date": "2024-01-15",
  "topN": 20,
  "includeChanges": true
}

# 回補排行榜快照
POST /api/analytics/leaderboard-snapshot/backfill
{
  "period": "daily",
  "startDate": "2024-01-01",
  "endDate": "2024-01-15"
}

# 取得排行榜統計
GET /api/analytics/leaderboard-stats?period=daily&date=2024-01-15
```

#### 6. 洞察貼文 API

```http
# 取得洞察貼文列表
GET /api/analytics/insight-posts?gameId=1&status=published&pinnedOnly=false

# 取得洞察貼文詳細資訊
GET /api/analytics/insight-posts/{id}

# 建立洞察貼文 (管理員限定)
POST /api/analytics/insight-posts
{
  "gameId": 1,
  "title": "遊戲熱度分析",
  "content": "詳細分析內容...",
  "publishNow": true,
  "sourceUrls": ["https://example.com/source1"]
}

# 發佈洞察貼文
POST /api/analytics/insight-posts/{id}/publish

# 設定置頂狀態
PUT /api/analytics/insight-posts/{id}/pinned
{
  "isPinned": true
}
```

#### 7. 儀表板統計 API

```http
# 取得分析儀表板
GET /api/analytics/dashboard?date=2024-01-15

# 取得遊戲表現報告
GET /api/analytics/reports/game-performance/{gameId}?startDate=2024-01-01&endDate=2024-01-31

# 取得指標相關性分析
POST /api/analytics/analysis/correlation
{
  "metricIds": [1, 2, 3, 4],
  "startDate": "2024-01-01",
  "endDate": "2024-01-31"
}

# 取得趨勢預測
GET /api/analytics/prediction/trend/{gameId}/{metricId}?predictionDays=7
```

## 🧪 測試指南

### 單元測試

```bash
# 執行所有分析測試
dotnet test --filter "AnalyticsControllerTests"

# 執行特定功能測試
dotnet test --filter "CreateGame_ShouldReturnSuccess"
```

### 測試覆蓋範圍

- ✅ 遊戲管理 (建立、更新、查詢、狀態管理)
- ✅ 指標來源管理 (來源建立、指標定義、權重配置)
- ✅ 每日數據收集 (單筆建立、批量導入、UPSERT邏輯)
- ✅ 熱度指數計算 (加權計算、正規化、排名生成)
- ✅ 排行榜快照 (日榜生成、週榜統計、歷史回補)
- ✅ 洞察貼文 (建立發佈、指標快照、來源管理)
- ✅ 儀表板統計 (綜合指標、趨勢分析、相關性計算)
- ✅ 錯誤處理和邊界條件

### 測試資料

使用 `14-AnalyticsSeedData.sql` 生成完整測試資料，包含：

- 20個熱門遊戲 (原神、英雄聯盟、絕地求生等)
- 10個指標來源 (Steam、Twitch、YouTube等)
- 28個指標定義 (涵蓋所有主要平台指標)
- 60+遊戲來源對應 (每遊戲3-6個數據源)
- 16800+每日指標數據點 (30天×20遊戲×28指標)
- 600+熱度指數記錄 (30天×20遊戲)
- 650+排行榜快照 (包含日榜和週榜)
- 10-15篇洞察貼文 (含指標快照和引用來源)

## 🔍 疑難排解

### 常見問題

#### 1. 熱度指數計算異常
**問題**: 某些遊戲的熱度指數為0或異常值
**解決**: 檢查該遊戲是否有對應的指標數據，驗證權重配置

#### 2. 排行榜快照重複
**問題**: 同一期間的排行榜快照被重複生成
**解決**: 利用唯一鍵約束防重，生成前檢查已存在的快照

#### 3. 外部數據導入失敗
**問題**: 外部API數據無法正常導入
**解決**: 檢查API端點配置、網路連通性、API密鑰有效性

### 監控指標

- 每日數據收集完整性和及時性
- 熱度指數計算準確性和效能
- 排行榜生成成功率和一致性
- 洞察貼文生成質量和頻率
- 系統響應時間和錯誤率

## 📈 效能最佳化

### 資料庫最佳化

```sql
-- 建議的索引
CREATE INDEX IX_game_metric_daily_game_date 
ON game_metric_daily (game_id, date DESC);

CREATE INDEX IX_game_metric_daily_metric_date 
ON game_metric_daily (metric_id, date DESC);

CREATE INDEX IX_popularity_index_daily_date_index 
ON popularity_index_daily (date DESC, index_value DESC);

CREATE INDEX IX_leaderboard_snapshots_period_ts 
ON leaderboard_snapshots (period, ts DESC, rank);

CREATE INDEX IX_posts_status_published 
ON posts (status, published_at DESC) WHERE status = 'published';
```

### 快取策略

- 熱門遊戲排行榜快取 (30分鐘)
- 儀表板統計數據快取 (15分鐘)
- 指標來源和定義快取 (2小時)
- 洞察貼文列表快取 (10分鐘)

### 計算最佳化

- 使用批量操作減少資料庫往返
- 異步處理大量數據導入
- 分頁查詢避免記憶體溢出
- 合理設定指標權重避免偏差

## 🚀 未來擴展

### 計劃功能

1. **機器學習預測**: 基於歷史數據預測遊戲熱度趨勢
2. **實時數據流**: WebSocket推送即時指標變化
3. **自動化洞察**: AI生成個性化數據洞察報告
4. **多維度分析**: 地區、年齡、平台等細分維度分析
5. **異常檢測**: 自動識別數據異常和熱度突變

---

*本文件最後更新: 2024年8月15日*
*版本: 1.0.0*
*維護者: GameCore開發團隊*