# GameCore 社交通知系統完整指南

## 📋 系統概述

GameCore社交通知系統是一個完整的即時通信和社群互動平台，嚴格按照規格實現多來源通知投遞、私訊聊天、群組管理、使用者封鎖等核心社交功能。系統設計旨在提供豐富的社群體驗，支援來源/行為字典建置、群組聊天、封鎖機制等功能，建立活躍的社交生態。

### 🎯 核心特色

- **多來源通知系統**: 完整的來源/行為字典建置，支援多種通知來源和行為類型
- **即時私訊聊天**: user→user 或客服 manager_id 的私人訊息系統
- **群組管理**: 建立/加入/退出群組，支援群組管理員和成員權限
- **群組聊天**: 專用群組聊天功能，支援即時群組互動
- **封鎖系統**: 群封鎖機制，同群同人唯一約束
- **實時通知**: 前台收件匣既讀回寫 is_read / read_at

## 🏗️ 系統架構

### 三層架構設計

```
┌─────────────────────┐
│   Presentation      │  ← SocialController, Notification Views
├─────────────────────┤
│   Business Logic    │  ← SocialService, SocialDTOs
├─────────────────────┤
│   Data Access       │  ← Social Entities, DbContext
└─────────────────────┘
```

### 核心元件

1. **SocialController**: RESTful API控制器，提供完整社交管理端點
2. **ISocialService**: 業務邏輯服務介面，定義完整社交功能契約
3. **SocialService**: 業務邏輯實現，包含所有社交相關功能
4. **SocialDTOs**: 資料傳輸物件，涵蓋所有社交操作的請求和回應
5. **Social Views**: 社交界面，包含通知、聊天、群組管理
6. **Social Entities**: 資料庫實體，對應社交相關資料表

## 📊 資料庫設計

### 核心資料表結構

#### Notification_Sources (通知來源表)
```sql
CREATE TABLE Notification_Sources (
    source_id int IDENTITY(1,1) PRIMARY KEY,
    source_name nvarchar(100) NULL              -- 來源名稱
);
```

#### Notification_Actions (通知行為表)
```sql
CREATE TABLE Notification_Actions (
    action_id int IDENTITY(1,1) PRIMARY KEY,
    action_name nvarchar(100) NULL              -- 行為名稱
);
```

#### Notifications (通知主表)
```sql
CREATE TABLE Notifications (
    notification_id int IDENTITY(1,1) PRIMARY KEY,
    source_id int NOT NULL,                     -- 來源類型編號 (FK)
    action_id int NOT NULL,                     -- 行為類型編號 (FK)
    sender_id int NOT NULL,                     -- 發送者編號 (FK)
    sender_manager_id int NULL,                 -- 發送者編號 (管理員, FK)
    notification_title nvarchar(200) NULL,     -- 通知標題
    notification_message nvarchar(1000) NULL,  -- 通知內容
    created_at datetime2 NOT NULL,             -- 建立時間
    group_id int NULL                          -- 群組編號 (若為群組相關, FK)
);
```

#### Notification_Recipients (通知接收者表)
```sql
CREATE TABLE Notification_Recipients (
    recipient_id int IDENTITY(1,1) PRIMARY KEY,
    notification_id int NOT NULL,              -- 通知編號 (FK)
    recipient_user_id int NOT NULL,            -- 接收者編號 (FK)
    is_read bit NOT NULL DEFAULT 0,            -- 是否已讀
    read_at datetime2 NULL,                    -- 已讀時間
    received_at datetime2 NOT NULL            -- 接收時間
);
```

#### Chat_Message (聊天訊息表)
```sql
CREATE TABLE Chat_Message (
    message_id int IDENTITY(1,1) PRIMARY KEY,
    manager_id int NULL,                       -- 管理員編號 (客服, FK)
    sender_id int NOT NULL,                    -- 發送者編號 (FK)
    receiver_id int NULL,                      -- 接收者編號 (FK)
    chat_content nvarchar(1000) NOT NULL,     -- 訊息內容
    sent_at datetime2 NOT NULL,               -- 發送時間
    is_read bit NOT NULL DEFAULT 0,           -- 是否已讀
    is_sent bit NOT NULL DEFAULT 1            -- 是否寄送
);
```

#### Groups (群組表)
```sql
CREATE TABLE Groups (
    group_id int IDENTITY(1,1) PRIMARY KEY,
    group_name nvarchar(100) NULL,            -- 群組名稱
    created_by int NULL,                      -- 建立者編號 (FK)
    created_at datetime2 NULL                 -- 建立時間
);
```

#### Group_Member (群組成員表)
```sql
CREATE TABLE Group_Member (
    group_id int NOT NULL,                    -- 群組編號 (複合主鍵, FK)
    user_id int NOT NULL,                     -- 使用者編號 (複合主鍵, FK)
    joined_at datetime2 NULL,                 -- 加入時間
    is_admin bit NOT NULL DEFAULT 0,          -- 是否為管理員
    
    PRIMARY KEY (group_id, user_id)
);
```

#### Group_Chat (群組專用聊天表)
```sql
CREATE TABLE Group_Chat (
    group_chat_id int IDENTITY(1,1) PRIMARY KEY,
    group_id int NULL,                        -- 群組編號 (FK)
    sender_id int NULL,                       -- 發送者編號 (FK)
    group_chat_content nvarchar(1000) NULL,  -- 訊息內容
    sent_at datetime2 NULL,                   -- 發送時間
    is_sent bit NOT NULL DEFAULT 1           -- 是否寄送
);
```

#### Group_Block (封鎖表)
```sql
CREATE TABLE Group_Block (
    block_id int IDENTITY(1,1) PRIMARY KEY,
    group_id int NOT NULL,                    -- 群組編號 (FK)
    user_id int NOT NULL,                     -- 被封鎖者編號 (FK)
    blocked_by int NOT NULL,                  -- 封鎖者編號 (FK)
    created_at datetime2 NULL,                -- 建立時間
    
    -- 唯一鍵約束：同群同人唯一
    UNIQUE (group_id, user_id)
);
```

### 重要設計原則

- **來源/行為字典**: Notification_Sources 和 Notification_Actions 建立通知類型字典
- **通知投遞機制**: 建立 Notifications → 批次 Notification_Recipients
- **已讀狀態管理**: is_read 初始 0，前台開啟通知列表/點開 → 更新 is_read=1、read_at
- **群組成員管理**: Group_Member 複合主鍵 (group_id, user_id)
- **群組封鎖唯一性**: Group_Block 同群同人唯一約束

## 💬 社交功能

### 通知系統

#### 多來源通知架構

```csharp
// 建立通知並投遞給收件人
var createDto = new CreateNotificationDto
{
    SourceId = 1,        // 來源類型 (系統/論壇/商店等)
    ActionId = 2,        // 行為類型 (公告/回覆/交易等)
    SenderId = 123,      // 發送者 (使用者)
    SenderManagerId = null, // 發送者 (管理員)
    NotificationTitle = "新回覆通知",
    NotificationMessage = "您的論壇主題收到了新的回覆",
    RecipientIds = new List<int> { 456, 789 } // 收件人列表
};

var result = await socialService.CreateNotificationAsync(createDto);
```

#### 通知狀態管理

```csharp
// 標記通知為已讀
var markReadDto = new MarkNotificationReadDto
{
    NotificationIds = new List<int> { 1, 2, 3 }
};

await socialService.MarkNotificationsAsReadAsync(userId, markReadDto);

// 取得通知統計
var stats = await socialService.GetNotificationStatsAsync(userId);
/*
統計資訊包含：
- TotalCount: 總通知數
- UnreadCount: 未讀通知數  
- TodayCount: 今日通知數
- WeekCount: 本週通知數
- SourceStats: 按來源統計
- ActionStats: 按行為統計
*/
```

### 聊天系統

#### 私訊功能

```csharp
// 發送私訊 (user→user)
var sendMessageDto = new SendChatMessageDto
{
    ReceiverId = 456,
    ChatContent = "你好！想和你討論一下遊戲攻略。"
};

var result = await socialService.SendChatMessageAsync(senderId, sendMessageDto);

// 發送客服訊息 (manager→user)
var customerServiceDto = new SendChatMessageDto
{
    ManagerId = 1,       // 客服管理員ID
    ChatContent = "您好，我們已收到您的問題，稍後會為您處理。"
};
```

#### 聊天對話管理

```csharp
// 取得聊天對話列表
var conversations = await socialService.GetChatConversationsAsync(userId, page: 1, pageSize: 20);

// 取得聊天訊息歷史
var messages = await socialService.GetChatMessagesAsync(userId, partnerId, page: 1, pageSize: 50);

// 標記聊天訊息為已讀
var markReadDto = new MarkChatReadDto
{
    PartnerId = 456,
    LastReadMessageId = 100
};

await socialService.MarkChatMessagesAsReadAsync(userId, markReadDto);
```

### 群組管理

#### 群組生命週期

```csharp
// 建立群組
var createGroupDto = new CreateGroupDto
{
    GroupName = "遊戲攻略討論群",
    InitialMemberIds = new List<int> { 456, 789 } // 初始成員
};

var result = await socialService.CreateGroupAsync(creatorId, createGroupDto);

// 加入群組
var joinDto = new JoinGroupDto { GroupId = 1 };
await socialService.JoinGroupAsync(userId, joinDto);

// 退出群組
await socialService.LeaveGroupAsync(userId, groupId);
```

#### 群組成員管理

```csharp
// 邀請加入群組 (群組成員限定)
var inviteDto = new InviteToGroupDto
{
    GroupId = 1,
    UserIds = new List<int> { 100, 101, 102 }
};

await socialService.InviteToGroupAsync(inviterId, inviteDto);

// 移除群組成員 (群組管理員限定)
var removeDto = new RemoveFromGroupDto
{
    GroupId = 1,
    UserId = 456
};

await socialService.RemoveFromGroupAsync(adminId, removeDto);

// 設定群組管理員 (群組管理員限定)
var setAdminDto = new SetGroupAdminDto
{
    GroupId = 1,
    UserId = 456,
    IsAdmin = true
};

await socialService.SetGroupAdminAsync(adminId, setAdminDto);
```

### 群組聊天

#### 群組訊息系統

```csharp
// 發送群組聊天
var sendGroupChatDto = new SendGroupChatDto
{
    GroupId = 1,
    GroupChatContent = "大家好！今天有什麼新的遊戲心得可以分享嗎？"
};

var result = await socialService.SendGroupChatAsync(senderId, sendGroupChatDto);

// 取得群組聊天記錄
var groupMessages = await socialService.GetGroupChatMessagesAsync(
    userId, groupId, page: 1, pageSize: 50);

// 搜尋群組聊天訊息
var searchResults = await socialService.SearchGroupChatMessagesAsync(
    userId, groupId, keyword: "攻略", page: 1, pageSize: 20);
```

### 封鎖系統

#### 群組封鎖機制

```csharp
// 封鎖群組成員 (群組管理員限定)
var blockDto = new BlockUserDto
{
    GroupId = 1,
    UserId = 456,
    BlockReason = "違反群組規則，發送不當內容"
};

await socialService.BlockUserInGroupAsync(blockerId, blockDto);

// 解除群組封鎖
var unblockDto = new UnblockUserDto
{
    GroupId = 1,
    UserId = 456
};

await socialService.UnblockUserInGroupAsync(adminId, unblockDto);

// 檢查是否被封鎖
var isBlocked = await socialService.IsUserBlockedInGroupAsync(groupId, userId);

// 取得群組封鎖列表 (群組管理員限定)
var blocks = await socialService.GetGroupBlocksAsync(
    adminId, groupId, page: 1, pageSize: 20);
```

## 🔧 API 文件

### 核心API端點

#### 1. 通知管理 API

```http
# 建立通知
POST /api/social/notifications
{
  "sourceId": 1,
  "actionId": 2,
  "senderId": 123,
  "notificationTitle": "新回覆通知",
  "notificationMessage": "您的論壇主題收到了新的回覆",
  "recipientIds": [456, 789]
}

# 取得使用者通知列表
GET /api/social/notifications?page=1&pageSize=20&unreadOnly=false&sourceId=1

# 標記通知為已讀
PUT /api/social/notifications/read
{
  "notificationIds": [1, 2, 3]
}

# 標記所有通知為已讀
PUT /api/social/notifications/read-all

# 取得通知統計
GET /api/social/notifications/stats

# 刪除通知
DELETE /api/social/notifications
{
  "notificationIds": [1, 2, 3]
}
```

#### 2. 聊天系統 API

```http
# 發送私訊
POST /api/social/chat/messages
{
  "receiverId": 456,
  "chatContent": "你好！想和你討論一下遊戲攻略。"
}

# 發送客服訊息
POST /api/social/chat/messages
{
  "managerId": 1,
  "chatContent": "您好，我們已收到您的問題。"
}

# 取得聊天對話列表
GET /api/social/chat/conversations?page=1&pageSize=20

# 取得聊天訊息歷史
GET /api/social/chat/messages/{partnerId}?page=1&pageSize=50

# 標記聊天訊息為已讀
PUT /api/social/chat/messages/read
{
  "partnerId": 456,
  "lastReadMessageId": 100
}

# 搜尋聊天訊息
GET /api/social/chat/search?keyword=攻略&partnerId=456&page=1&pageSize=20
```

#### 3. 群組管理 API

```http
# 建立群組
POST /api/social/groups
{
  "groupName": "遊戲攻略討論群",
  "initialMemberIds": [456, 789]
}

# 更新群組資訊
PUT /api/social/groups/{id}
{
  "groupName": "更新的群組名稱"
}

# 加入群組
POST /api/social/groups/join
{
  "groupId": 1
}

# 退出群組
POST /api/social/groups/{id}/leave

# 邀請加入群組
POST /api/social/groups/invite
{
  "groupId": 1,
  "userIds": [100, 101, 102]
}

# 移除群組成員
POST /api/social/groups/remove
{
  "groupId": 1,
  "userId": 456
}

# 設定群組管理員
PUT /api/social/groups/admin
{
  "groupId": 1,
  "userId": 456,
  "isAdmin": true
}

# 取得使用者群組列表
GET /api/social/groups?page=1&pageSize=20

# 取得群組詳細資訊
GET /api/social/groups/{id}

# 取得群組成員列表
GET /api/social/groups/{id}/members?page=1&pageSize=50

# 搜尋群組
GET /api/social/groups/search?keyword=攻略&page=1&pageSize=20
```

#### 4. 群組聊天 API

```http
# 發送群組聊天
POST /api/social/groups/chat
{
  "groupId": 1,
  "groupChatContent": "大家好！今天有什麼新的遊戲心得可以分享嗎？"
}

# 取得群組聊天記錄
GET /api/social/groups/{id}/chat?page=1&pageSize=50

# 搜尋群組聊天訊息
GET /api/social/groups/{id}/chat/search?keyword=攻略&page=1&pageSize=20

# 刪除群組聊天訊息
DELETE /api/social/groups/chat/messages
{
  "messageIds": [1, 2, 3]
}
```

#### 5. 封鎖系統 API

```http
# 封鎖群組成員
POST /api/social/groups/block
{
  "groupId": 1,
  "userId": 456,
  "blockReason": "違反群組規則"
}

# 解除群組封鎖
POST /api/social/groups/unblock
{
  "groupId": 1,
  "userId": 456
}

# 取得群組封鎖列表
GET /api/social/groups/{id}/blocks?page=1&pageSize=20

# 檢查是否被封鎖
GET /api/social/groups/{id}/blocks/check?userId=456
```

## 🧪 測試指南

### 單元測試

```bash
# 執行所有社交測試
dotnet test --filter "SocialControllerTests"

# 執行特定功能測試
dotnet test --filter "CreateNotification_ShouldReturnSuccess"
```

### 測試覆蓋範圍

- ✅ 通知系統 (建立、查詢、已讀、統計、刪除)
- ✅ 聊天系統 (私訊、客服、對話管理、搜尋)
- ✅ 群組管理 (建立、加入、退出、成員管理)
- ✅ 群組聊天 (發送、歷史、搜尋、刪除)
- ✅ 封鎖系統 (封鎖、解封、查詢、檢查)
- ✅ 權限控制 (群組管理員、成員權限驗證)
- ✅ 錯誤處理和邊界條件

### 測試資料

使用 `13-SocialNotificationSeedData.sql` 生成完整測試資料，包含：

- 11個通知來源類型 (系統、論壇、商店等)
- 18個通知行為類型 (公告、回覆、交易等)
- 750+通知記錄 (每使用者10-20個通知)
- 300+聊天訊息 (涵蓋私訊和客服訊息)
- 8-12個群組 (包含成員管理和權限)
- 200+群組聊天記錄
- 封鎖記錄 (示範群組封鎖機制)

## 🔍 疑難排解

### 常見問題

#### 1. 通知重複投遞問題
**問題**: 同一通知被重複發送給相同收件人
**解決**: 檢查 Notification_Recipients 的唯一性約束

#### 2. 群組封鎖衝突問題
**問題**: 同群同人的重複封鎖記錄
**解決**: Group_Block 表的 (group_id, user_id) 唯一約束

#### 3. 聊天訊息順序問題
**問題**: 聊天訊息顯示順序不正確
**解決**: 按照 sent_at 時間戳排序

### 監控指標

- 通知發送量和已讀率
- 聊天活躍度和回應率
- 群組創建和成員活躍度
- 封鎖事件和解封率
- 系統響應時間和錯誤率

## 📈 效能最佳化

### 資料庫最佳化

```sql
-- 建議的索引
CREATE INDEX IX_notification_recipients_user_read 
ON Notification_Recipients (recipient_user_id, is_read, received_at DESC);

CREATE INDEX IX_chat_message_conversation 
ON Chat_Message (sender_id, receiver_id, sent_at DESC);

CREATE INDEX IX_group_member_user_groups 
ON Group_Member (user_id, joined_at DESC);

CREATE INDEX IX_group_chat_messages 
ON Group_Chat (group_id, sent_at DESC);

CREATE INDEX IX_group_block_unique 
ON Group_Block (group_id, user_id);
```

### 快取策略

- 使用者通知未讀數快取 (5分鐘)
- 群組成員列表快取 (15分鐘)
- 聊天對話列表快取 (10分鐘)
- 通知來源/行為字典快取 (1小時)

## 🚀 未來擴展

### 計劃功能

1. **實時通知**: WebSocket/SignalR 即時推送
2. **訊息加密**: 端到端加密聊天
3. **檔案分享**: 聊天中的圖片和檔案
4. **語音訊息**: 音訊聊天功能
5. **群組權限**: 更細緻的群組角色管理

---

*本文件最後更新: 2024年8月15日*
*版本: 1.0.0*
*維護者: GameCore開發團隊*