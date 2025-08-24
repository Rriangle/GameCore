# GameCore 論壇系統完整指南

## 📋 系統概述

GameCore論壇系統是一個完整的遊戲社群討論平台，嚴格按照規格實現版面管理、主題討論、回覆互動、反應收藏等核心論壇功能。系統設計旨在提供豐富的社群互動體驗，支援每遊戲一版面的架構、多層級回覆、表情反應、內容收藏等功能，建立活躍的遊戲社群生態。

### 🎯 核心特色

- **遊戲專屬版面**: 每個遊戲擁有獨立討論區，確保主題聚焦 (`game_id unique`)
- **豐富互動機制**: 支援6種表情反應(like/love/laugh/wow/sad/angry)
- **彈性回覆結構**: 支援一層父回覆，建立有層次的討論
- **內容狀態管理**: 完整的normal/hidden/archived/deleted狀態機
- **收藏系統**: 支援收藏主題、回覆、版面、遊戲等多種目標類型
- **內容審核**: 管理員可進行內容狀態管理和批量審核

## 🏗️ 系統架構

### 三層架構設計

```
┌─────────────────────┐
│   Presentation      │  ← ForumController, Forum Views
├─────────────────────┤
│   Business Logic    │  ← ForumService, ForumDTOs
├─────────────────────┤
│   Data Access       │  ← Forum Entities, DbContext
└─────────────────────┘
```

### 核心元件

1. **ForumController**: RESTful API控制器，提供完整論壇管理端點
2. **IForumService**: 業務邏輯服務介面，定義完整論壇功能契約
3. **ForumService**: 業務邏輯實現，包含所有論壇相關功能
4. **ForumDTOs**: 資料傳輸物件，涵蓋所有論壇操作的請求和回應
5. **Forum Views**: 論壇界面，包含版面瀏覽和討論互動
6. **Forum Entities**: 資料庫實體，對應論壇相關資料表

## 📊 資料庫設計

### 核心資料表結構

#### forums (論壇版面表)
```sql
CREATE TABLE forums (
    forum_id int IDENTITY(1,1) PRIMARY KEY,
    game_id int NOT NULL UNIQUE,          -- 遊戲ID (唯一，每遊戲一版)
    forum_name nvarchar(100) NOT NULL,    -- 版面名稱
    forum_description nvarchar(500) NULL, -- 版面描述
    thread_count int NOT NULL DEFAULT 0,  -- 主題數量
    post_count int NOT NULL DEFAULT 0,    -- 回覆數量
    view_count bigint NOT NULL DEFAULT 0, -- 瀏覽次數
    is_active bit NOT NULL DEFAULT 1,     -- 是否啟用
    sort_order int NOT NULL DEFAULT 0,    -- 排序順序
    created_at datetime2 NOT NULL,        -- 建立時間
    updated_at datetime2 NOT NULL         -- 更新時間
);
```

#### threads (討論主題表)
```sql
CREATE TABLE threads (
    thread_id bigint IDENTITY(1,1) PRIMARY KEY,
    forum_id int NOT NULL,                -- 版面ID (FK)
    author_user_id int NOT NULL,          -- 作者ID (FK)
    title nvarchar(200) NULL,             -- 主題標題
    status nvarchar(20) NULL DEFAULT 'normal', -- 主題狀態
    created_at datetime2 NULL,            -- 建立時間
    updated_at datetime2 NULL             -- 更新時間
);
```

#### thread_posts (回覆表)
```sql
CREATE TABLE thread_posts (
    id bigint IDENTITY(1,1) PRIMARY KEY,
    thread_id bigint NOT NULL,            -- 主題ID (FK)
    author_user_id int NOT NULL,          -- 作者ID (FK)
    content_md nvarchar(max) NULL,        -- 回覆內容 (Markdown)
    parent_post_id bigint NULL,           -- 父回覆ID (支援一層回覆)
    status nvarchar(20) NULL DEFAULT 'normal', -- 回覆狀態
    created_at datetime2 NULL,            -- 建立時間
    updated_at datetime2 NULL             -- 更新時間
);
```

#### reactions (反應表)
```sql
CREATE TABLE reactions (
    id bigint IDENTITY(1,1) PRIMARY KEY,
    user_id int NOT NULL,                 -- 使用者ID (FK)
    target_type nvarchar(50) NULL,        -- 目標類型 (thread/thread_post)
    target_id bigint NOT NULL,            -- 目標ID
    kind nvarchar(50) NULL,               -- 反應類型
    created_at datetime2 NULL,            -- 建立時間
    
    -- 唯一鍵約束：同一使用者對同一目標的同一反應類型只能有一個
    UNIQUE (target_type, target_id, user_id, kind)
);
```

#### bookmarks (收藏表)
```sql
CREATE TABLE bookmarks (
    id bigint IDENTITY(1,1) PRIMARY KEY,
    user_id int NOT NULL,                 -- 使用者ID (FK)
    target_type nvarchar(50) NULL,        -- 目標類型 (thread/thread_post/forum/game)
    target_id bigint NOT NULL,            -- 目標ID
    created_at datetime2 NULL,            -- 建立時間
    
    -- 唯一鍵約束：同一使用者對同一目標只能收藏一次
    UNIQUE (target_type, target_id, user_id)
);
```

### 重要設計原則

- **每遊戲一版面**: forums.game_id為唯一鍵，確保每個遊戲只有一個論壇版面
- **內容狀態管理**: normal/hidden/archived/deleted四種狀態，支援內容審核
- **一層回覆結構**: thread_posts.parent_post_id支援對特定回覆的回應
- **反應去重機制**: reactions表的唯一鍵約束防止重複反應
- **收藏多樣化**: 支援收藏主題、回覆、版面、遊戲等多種目標類型

## 💬 論壇功能

### 版面管理

#### 遊戲專屬版面架構

```csharp
// 每個遊戲自動建立專屬討論區
var forums = await forumService.GetForumsAsync(gameId: null, activeOnly: true);

// 論壇版面資訊
public class ForumDto
{
    public int ForumId { get; set; }
    public int GameId { get; set; }
    public string GameName { get; set; }        // 遊戲名稱
    public string Name { get; set; }            // 版面名稱
    public string Description { get; set; }     // 版面描述
    public int ThreadCount { get; set; }        // 主題數量
    public int PostCount { get; set; }          // 回覆數量
    public int ViewCount { get; set; }          // 瀏覽次數
    public bool IsActive { get; set; }          // 是否啟用
    public List<ModeratorDto> Moderators { get; set; } // 版主列表
}
```

### 主題討論

#### 主題管理功能

```csharp
// 建立主題
var createDto = new CreateThreadDto
{
    ForumId = 1,
    Title = "【攻略分享】新手必看完整指南",
    Content = "詳細的新手攻略內容...",
    Tags = new List<string> { "攻略", "新手", "指南" },
    IsPinned = false
};

var result = await forumService.CreateThreadAsync(userId, createDto);
```

#### 主題狀態機

按照規格嚴格實現的狀態轉換：

```
主題狀態 (threads.status):
normal ↔ hidden ↔ archived → deleted

回覆狀態 (thread_posts.status):
normal ↔ hidden ↔ archived → deleted
```

### 回覆互動

#### 多層級回覆結構

```csharp
// 建立主回覆
var mainReply = new CreateThreadPostDto
{
    ThreadId = 1,
    Content = "很有用的攻略，感謝分享！",
    ParentPostId = null // 主回覆
};

// 建立子回覆
var subReply = new CreateThreadPostDto
{
    ThreadId = 1,
    Content = "我也覺得很實用，特別是第三個技巧。",
    ParentPostId = 5 // 回覆特定樓層
};

var result = await forumService.CreateThreadPostAsync(userId, mainReply);
```

### 表情反應系統

#### 六種反應類型

```csharp
// 支援的反應類型
public enum ReactionKind
{
    like = "👍",      // 讚
    love = "❤️",      // 愛心
    laugh = "😂",     // 大笑
    wow = "😲",       // 驚訝
    sad = "😢",       // 難過
    angry = "😡"      // 生氣
}

// 新增反應
var reactionDto = new AddReactionDto
{
    TargetType = "thread",      // 目標類型: thread/thread_post
    TargetId = 1,              // 目標ID
    ReactionType = "like"       // 反應類型
};

var result = await forumService.AddReactionAsync(userId, reactionDto);
```

### 收藏系統

#### 多類型收藏支援

```csharp
// 收藏主題
var bookmarkThread = new AddBookmarkDto
{
    TargetType = "thread",
    TargetId = 1,
    Notes = "很有價值的討論主題"
};

// 收藏回覆
var bookmarkPost = new AddBookmarkDto
{
    TargetType = "thread_post",
    TargetId = 5,
    Notes = "有用的回覆內容"
};

// 收藏版面
var bookmarkForum = new AddBookmarkDto
{
    TargetType = "forum",
    TargetId = 1,
    Notes = "喜歡的討論版面"
};

// 收藏遊戲
var bookmarkGame = new AddBookmarkDto
{
    TargetType = "game",
    TargetId = 1,
    Notes = "感興趣的遊戲"
};

var result = await forumService.AddBookmarkAsync(userId, bookmarkThread);
```

## 🔧 API 文件

### 核心API端點

#### 1. 論壇版面管理 API

```http
# 取得所有論壇版面
GET /api/forum?gameId=1&activeOnly=true

# 取得版面詳細資訊
GET /api/forum/{id}

# 建立論壇版面 (管理員限定)
POST /api/forum
{
  "gameId": 1,
  "name": "新討論區",
  "description": "新建立的討論區"
}

# 更新論壇版面 (管理員限定)
PUT /api/forum/{id}
{
  "name": "更新的討論區名稱",
  "description": "更新的描述",
  "isActive": true
}
```

#### 2. 主題管理 API

```http
# 取得論壇主題列表
GET /api/forum/forums/{forumId}/threads?page=1&pageSize=20

# 取得主題詳細資訊
GET /api/forum/threads/{id}

# 建立主題
POST /api/forum/threads
{
  "forumId": 1,
  "title": "【攻略分享】新手必看指南",
  "content": "詳細的攻略內容...",
  "tags": ["攻略", "新手"]
}

# 更新主題
PUT /api/forum/threads/{id}
{
  "title": "更新的主題標題",
  "content": "更新的內容",
  "status": "normal"
}
```

#### 3. 回覆管理 API

```http
# 取得主題回覆列表
GET /api/forum/threads/{threadId}/posts?page=1&pageSize=20

# 建立回覆
POST /api/forum/posts
{
  "threadId": 1,
  "content": "很有用的內容，感謝分享！",
  "parentPostId": null
}

# 更新回覆
PUT /api/forum/posts/{id}
{
  "content": "更新的回覆內容",
  "status": "normal"
}

# 刪除回覆
DELETE /api/forum/posts/{id}
```

#### 4. 互動管理 API

```http
# 新增反應
POST /api/forum/reactions
{
  "targetType": "thread",
  "targetId": 1,
  "reactionType": "like"
}

# 移除反應
DELETE /api/forum/reactions?targetType=thread&targetId=1&reactionType=like

# 取得反應統計
GET /api/forum/reactions/stats?targetType=thread&targetId=1

# 新增收藏
POST /api/forum/bookmarks
{
  "targetType": "thread",
  "targetId": 1,
  "notes": "很有價值的討論主題"
}

# 移除收藏
DELETE /api/forum/bookmarks/{id}

# 取得使用者收藏列表
GET /api/forum/bookmarks?targetType=thread&page=1&pageSize=20
```

## 🧪 測試指南

### 單元測試

```bash
# 執行所有論壇測試
dotnet test --filter "ForumControllerTests"

# 執行特定功能測試
dotnet test --filter "CreateThread_ShouldReturnSuccess"
```

### 測試覆蓋範圍

- ✅ 版面管理 (建立、更新、查詢、狀態管理)
- ✅ 主題管理 (建立、更新、搜尋、狀態轉換)
- ✅ 回覆管理 (建立、更新、刪除、層級結構)
- ✅ 互動功能 (反應新增移除、反應統計、去重機制)
- ✅ 收藏系統 (多類型收藏、收藏管理、去重檢查)
- ✅ 統計分析 (論壇統計、熱門內容、活躍度排名)
- ✅ 權限控制 (使用者權限、管理員功能、內容審核)
- ✅ 錯誤處理和邊界條件

### 測試資料

使用 `12-ForumSeedData.sql` 生成完整測試資料，包含：

- 20個論壇版面 (對應不同遊戲)
- 300+主題記錄 (涵蓋8種主題類型)
- 1200+回覆記錄 (包含層級結構)
- 800+反應記錄 (6種反應類型)
- 400+收藏記錄 (4種收藏類型)

## 🔍 疑難排解

### 常見問題

#### 1. 反應重複新增問題
**問題**: 使用者可以對同一內容重複新增相同反應
**解決**: 使用資料庫唯一鍵約束和應用層檢查

```csharp
// 檢查是否已存在相同反應
var existingReaction = await _context.Reactions
    .FirstOrDefaultAsync(r => 
        r.TargetType == reactionDto.TargetType &&
        r.TargetId == reactionDto.TargetId &&
        r.UserId == userId &&
        r.Kind == reactionDto.ReactionType);

if (existingReaction != null)
{
    return ForumServiceResult<ReactionDto>.CreateFailure("已經對此內容表達過相同反應");
}
```

#### 2. 回覆層級混亂問題
**問題**: 子回覆的parent_post_id指向錯誤的回覆
**解決**: 嚴格驗證父回覆的存在性和歸屬關係

#### 3. 主題統計不同步問題
**問題**: 主題的post_count與實際回覆數不符
**解決**: 在回覆建立/刪除時同步更新主題統計

### 監控指標

- 版面活躍度和主題發表量
- 使用者互動率 (回覆/反應/收藏)
- 內容審核工作量
- 熱門主題和版面排行
- 使用者留存率和參與度

## 📈 效能最佳化

### 資料庫最佳化

```sql
-- 建議的索引
CREATE INDEX IX_threads_forum_status 
ON threads (forum_id, status, updated_at DESC);

CREATE INDEX IX_thread_posts_thread_parent 
ON thread_posts (thread_id, parent_post_id, created_at);

CREATE INDEX IX_reactions_target_user 
ON reactions (target_type, target_id, user_id);

CREATE INDEX IX_bookmarks_user_type 
ON bookmarks (user_id, target_type, created_at DESC);
```

### 快取策略

- 熱門主題列表快取 (30分鐘)
- 版面統計資料快取 (1小時)
- 反應統計快取 (15分鐘)
- 使用者收藏列表快取 (10分鐘)

## 🚀 未來擴展

### 計劃功能

1. **進階搜尋**: 全文檢索、標籤搜尋、進階篩選
2. **內容審核**: AI自動審核、敏感詞過濾、舉報系統
3. **用戶等級**: 根據活躍度設定用戶等級和權限
4. **徽章系統**: 根據貢獻度頒發特殊徽章
5. **私人訊息**: 用戶間私人對話功能

---

*本文件最後更新: 2024年8月15日*
*版本: 1.0.0*
*維護者: GameCore開發團隊*