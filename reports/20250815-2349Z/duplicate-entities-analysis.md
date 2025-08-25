# 重複實體定義分析

## 發現的重複定義

### 1. Post 類別
- `GameCore.Core/Entities/Post.cs` - 定義了 Post, PostMetricSnapshot, PostSource
- `GameCore.Core/Entities/Forum.cs` - 定義了 Post, PostReply, PostLike, PostReplyLike, PostBookmark
- 衝突：兩個檔案都定義了 Post 類別

### 2. Forum 類別  
- `GameCore.Core/Entities/Forum.cs` - 定義了 Forum
- `GameCore.Core/Entities/Post.cs` - 定義了 Forum
- 衝突：兩個檔案都定義了 Forum 類別

### 3. ChatMessage 類別
- `GameCore.Core/Entities/Notification.cs` - 定義了 ChatMessage
- `GameCore.Core/Entities/Chat.cs` - 定義了 ChatMessage
- 衝突：兩個檔案都定義了 ChatMessage 類別

## 解決方案

### 方案1：合併檔案（推薦）
將相關的實體類別合併到單一檔案中：

1. **Forum.cs** - 保留 Forum, Post, PostReply, PostLike, PostReplyLike, PostBookmark
2. **Chat.cs** - 保留 ChatRoom, ChatMessage, PrivateChat, PrivateMessage
3. **Post.cs** - 移除重複定義，只保留 PostMetricSnapshot, PostSource

### 方案2：重新命名
為重複的類別重新命名以避免衝突。

## 建議執行順序
1. 備份現有檔案
2. 合併 Forum.cs 和 Post.cs 中的 Forum 相關類別
3. 合併 Chat.cs 和 Notification.cs 中的 Chat 相關類別
4. 更新所有引用這些類別的檔案
5. 重新編譯驗證