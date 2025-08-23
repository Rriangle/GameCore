# GameCore 資料庫部署指南

## 📋 概述

GameCore 是一個完整的遊戲平台資料庫系統，包含用戶管理、寵物系統、論壇、聊天、商城、玩家市場等核心功能。本指南將幫助您完成資料庫的部署和配置。

## 🎯 系統需求

### 硬體需求
- **CPU**: 2核心以上
- **記憶體**: 4GB RAM 以上
- **硬碟**: 20GB 可用空間
- **網路**: 穩定的網路連接

### 軟體需求
- **作業系統**: Windows Server 2016+ / Windows 10+ / Linux (支援 Docker)
- **資料庫**: SQL Server 2019+ / SQL Server Express 2019+
- **工具**: SQL Server Management Studio (SSMS) 18.0+
- **權限**: 資料庫管理員權限

## 🚀 快速部署

### 方法一：使用 SQL Server Management Studio (推薦)

1. **下載並安裝 SQL Server**
   ```bash
   # 下載 SQL Server 2019 Express
   https://www.microsoft.com/zh-tw/sql-server/sql-server-downloads
   ```

2. **安裝 SQL Server Management Studio**
   ```bash
   # 下載 SSMS
   https://docs.microsoft.com/zh-tw/sql/ssms/download-sql-server-management-studio-ssms
   ```

3. **執行資料庫腳本**
   - 開啟 SSMS
   - 連接到 SQL Server 實例
   - 依序執行以下腳本：

### 方法二：使用 Docker (開發環境)

```bash
# 拉取 SQL Server 映像
docker pull mcr.microsoft.com/mssql/server:2019-latest

# 啟動 SQL Server 容器
docker run -e "ACCEPT_EULA=Y" -e "SA_PASSWORD=YourStrong@Passw0rd" \
  -p 1433:1433 --name gamecore-sql \
  -d mcr.microsoft.com/mssql/server:2019-latest

# 連接到容器
docker exec -it gamecore-sql /opt/mssql-tools/bin/sqlcmd \
  -S localhost -U sa -P YourStrong@Passw0rd
```

## 📁 腳本執行順序

### 1. 初始化資料庫環境
```sql
-- 執行 00-Initialize.sql
-- 建立資料庫和基本設定
```

### 2. 建立資料表結構
```sql
-- 執行 01-CreateTables.sql
-- 建立所有資料表和關聯
```

### 3. 插入測試資料
```sql
-- 執行 02-SeedData.sql
-- 插入範例資料和測試帳號
```

### 4. 建立效能索引
```sql
-- 執行 03-CreateIndexes.sql
-- 建立查詢優化索引
```

### 5. 執行維護腳本 (可選)
```sql
-- 執行 04-Maintenance.sql
-- 資料庫維護和效能監控
```

## 🔧 詳細部署步驟

### 步驟 1: 環境準備

1. **檢查 SQL Server 版本**
   ```sql
   SELECT @@VERSION;
   -- 確保版本 >= 15.0.2000.5 (SQL Server 2019)
   ```

2. **檢查權限**
   ```sql
   -- 確保當前用戶有建立資料庫的權限
   SELECT IS_SRVROLEMEMBER('sysadmin');
   ```

3. **檢查磁碟空間**
   ```sql
   -- 檢查可用磁碟空間
   EXEC xp_fixeddrives;
   ```

### 步驟 2: 建立資料庫

1. **執行初始化腳本**
   ```sql
   -- 開啟 00-Initialize.sql 並執行
   -- 腳本會自動檢查環境並建立資料庫
   ```

2. **驗證資料庫建立**
   ```sql
   SELECT name, state_desc, recovery_model_desc
   FROM sys.databases
   WHERE name = 'GameCore';
   ```

### 步驟 3: 建立資料表

1. **執行建表腳本**
   ```sql
   -- 開啟 01-CreateTables.sql 並執行
   -- 腳本會建立所有必要的資料表
   ```

2. **驗證資料表建立**
   ```sql
   SELECT TABLE_NAME, TABLE_TYPE
   FROM INFORMATION_SCHEMA.TABLES
   WHERE TABLE_TYPE = 'BASE TABLE'
   ORDER BY TABLE_NAME;
   ```

### 步驟 4: 插入測試資料

1. **執行種子資料腳本**
   ```sql
   -- 開啟 02-SeedData.sql 並執行
   -- 腳本會插入測試用戶、商品、貼文等資料
   ```

2. **驗證資料插入**
   ```sql
   SELECT 'Users' AS TableName, COUNT(*) AS RecordCount FROM Users
   UNION ALL
   SELECT 'Pets', COUNT(*) FROM Pets
   UNION ALL
   SELECT 'Posts', COUNT(*) FROM Posts
   UNION ALL
   SELECT 'StoreProducts', COUNT(*) FROM StoreProducts;
   ```

### 步驟 5: 建立索引

1. **執行索引腳本**
   ```sql
   -- 開啟 03-CreateIndexes.sql 並執行
   -- 腳本會建立查詢優化索引
   ```

2. **驗證索引建立**
   ```sql
   SELECT 
       t.name AS TableName,
       i.name AS IndexName,
       i.type_desc AS IndexType
   FROM sys.indexes i
   INNER JOIN sys.tables t ON i.object_id = t.object_id
   WHERE t.name IN ('Users', 'Pets', 'Posts', 'StoreProducts', 'MarketTransactions')
   ORDER BY t.name, i.type_desc;
   ```

## 🧪 測試驗證

### 測試帳號

腳本執行完成後，系統會建立以下測試帳號：

| 帳號類型 | 用戶名 | 密碼 | 角色 |
|---------|--------|------|------|
| 管理員 | admin | admin | 超級管理員 |
| 管理員 | content | admin | 內容管理員 |
| 管理員 | store | admin | 商城管理員 |
| 管理員 | service | admin | 客服管理員 |
| 管理員 | analyst | admin | 數據分析員 |
| 用戶 | testuser1 | admin | 一般用戶 |
| 用戶 | testuser2 | admin | 一般用戶 |
| 用戶 | testuser3 | admin | 一般用戶 |
| 賣家 | seller1 | admin | 賣家用戶 |
| 買家 | buyer1 | admin | 買家用戶 |

### 功能測試

1. **用戶登入測試**
   ```sql
   -- 測試用戶查詢
   SELECT Username, Nickname, Level, Points, Coins
   FROM Users
   WHERE Username = 'testuser1';
   ```

2. **寵物系統測試**
   ```sql
   -- 測試寵物查詢
   SELECT p.Name, p.Type, p.Level, p.Experience, u.Nickname
   FROM Pets p
   INNER JOIN Users u ON p.UserId = u.Id
   WHERE u.Username = 'testuser1';
   ```

3. **論壇系統測試**
   ```sql
   -- 測試貼文查詢
   SELECT p.Title, p.Content, u.Nickname, p.CreateTime
   FROM Posts p
   INNER JOIN Users u ON p.UserId = u.Id
   ORDER BY p.CreateTime DESC;
   ```

4. **商城系統測試**
   ```sql
   -- 測試商品查詢
   SELECT Name, Description, Price, Category, Stock
   FROM StoreProducts
   WHERE IsActive = 1
   ORDER BY Category, Price;
   ```

## 📊 資料庫結構

### 核心資料表

| 模組 | 資料表 | 說明 |
|------|--------|------|
| 用戶管理 | Users, Managers, ManagerRoles, Permissions, RolePermissions | 用戶帳號、權限管理 |
| 寵物系統 | Pets, PetInteractions | 寵物養成、互動記錄 |
| 簽到系統 | SignInRecords | 每日簽到記錄 |
| 小遊戲 | MiniGameRecords | 遊戲記錄、分數 |
| 論壇系統 | Forums, Posts, PostReplies, PostLikes, PostBookmarks | 論壇、貼文、互動 |
| 聊天系統 | ChatRooms, ChatMessages, ChatRoomMembers, PrivateChats, PrivateMessages | 群聊、私聊 |
| 通知系統 | Notifications, NotificationSources, NotificationActions | 系統通知 |
| 商城系統 | StoreProducts, StoreOrders, StoreOrderItems, ShoppingCartItems | 官方商城 |
| 玩家市場 | MarketTransactions, MarketReviews | 玩家交易市場 |

### 統計視圖

| 視圖名稱 | 說明 | 用途 |
|----------|------|------|
| vw_UserStats | 用戶統計資訊 | 用戶等級、經驗、點數統計 |
| vw_ForumStats | 論壇統計資訊 | 貼文數量、回覆數量統計 |
| vw_MarketStats | 市場統計資訊 | 交易量、平台費用統計 |

## 🔒 安全性設定

### 1. 資料庫權限

```sql
-- 建立專用資料庫用戶
CREATE LOGIN gamecore_user WITH PASSWORD = 'StrongPassword123!';
CREATE USER gamecore_user FOR LOGIN gamecore_user;

-- 授予必要權限
GRANT SELECT, INSERT, UPDATE, DELETE ON SCHEMA::dbo TO gamecore_user;
GRANT EXECUTE ON SCHEMA::dbo TO gamecore_user;
```

### 2. 連線字串設定

```csharp
// appsettings.json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=GameCore;User Id=gamecore_user;Password=StrongPassword123!;TrustServerCertificate=true;"
  }
}
```

### 3. 防火牆設定

```bash
# Windows 防火牆
netsh advfirewall firewall add rule name="SQL Server" dir=in action=allow protocol=TCP localport=1433

# Linux 防火牆 (ufw)
sudo ufw allow 1433/tcp
```

## 📈 效能優化

### 1. 索引策略

- **叢集索引**: 主鍵自動建立
- **非叢集索引**: 查詢頻繁的欄位
- **複合索引**: 多欄位查詢優化
- **篩選索引**: 特定條件的查詢優化

### 2. 統計資訊

```sql
-- 手動更新統計資訊
UPDATE STATISTICS Users;
UPDATE STATISTICS Posts;
UPDATE STATISTICS StoreProducts;

-- 自動更新統計資訊
ALTER DATABASE GameCore SET AUTO_UPDATE_STATISTICS ON;
```

### 3. 查詢優化

```sql
-- 使用參數化查詢
-- 避免 SELECT *
-- 適當使用 JOIN 而非子查詢
-- 定期分析執行計劃
```

## 🛠️ 維護作業

### 日常維護

```sql
-- 每日執行
UPDATE STATISTICS Users;
UPDATE STATISTICS Posts;
UPDATE STATISTICS StoreProducts;

-- 檢查資料庫狀態
SELECT 
    DB_NAME() AS DatabaseName,
    DATABASEPROPERTYEX(DB_NAME(), 'Status') AS Status;
```

### 週期維護

```sql
-- 每週執行完整維護腳本
-- 04-Maintenance.sql

-- 每月重建索引
ALTER INDEX ALL ON Users REBUILD;
ALTER INDEX ALL ON Posts REBUILD;
ALTER INDEX ALL ON StoreProducts REBUILD;
```

### 備份策略

```sql
-- 完整備份
BACKUP DATABASE GameCore TO DISK = 'C:\Backup\GameCore_Full.bak'
WITH COMPRESSION, CHECKSUM;

-- 差異備份
BACKUP DATABASE GameCore TO DISK = 'C:\Backup\GameCore_Diff.bak'
WITH DIFFERENTIAL, COMPRESSION, CHECKSUM;

-- 交易記錄備份
BACKUP LOG GameCore TO DISK = 'C:\Backup\GameCore_Log.trn'
WITH COMPRESSION, CHECKSUM;
```

## 🚨 故障排除

### 常見問題

1. **連線失敗**
   ```sql
   -- 檢查 SQL Server 服務狀態
   -- 檢查防火牆設定
   -- 檢查連線字串
   ```

2. **權限不足**
   ```sql
   -- 檢查用戶權限
   SELECT IS_SRVROLEMEMBER('sysadmin');
   
   -- 授予必要權限
   GRANT CREATE DATABASE TO [username];
   ```

3. **磁碟空間不足**
   ```sql
   -- 檢查磁碟空間
   EXEC xp_fixeddrives;
   
   -- 清理日誌檔案
   DBCC SHRINKFILE (GameCore_Log, 10);
   ```

4. **效能問題**
   ```sql
   -- 檢查索引碎片
   SELECT * FROM sys.dm_db_index_physical_stats(DB_ID(), NULL, NULL, NULL, NULL);
   
   -- 重建索引
   ALTER INDEX ALL ON [TableName] REBUILD;
   ```

### 日誌檢查

```sql
-- 檢查 SQL Server 錯誤日誌
EXEC sp_readerrorlog;

-- 檢查資料庫錯誤
SELECT * FROM sys.dm_exec_requests WHERE status = 'running';
```

## 📞 技術支援

### 聯絡資訊

- **專案維護**: GameCore 開發團隊
- **技術文件**: [專案 Wiki](https://github.com/gamecore/docs)
- **問題回報**: [GitHub Issues](https://github.com/gamecore/issues)

### 社群資源

- **開發者論壇**: [GameCore 開發者社群](https://forum.gamecore.dev)
- **技術部落格**: [GameCore 技術部落格](https://blog.gamecore.dev)
- **影片教學**: [YouTube 頻道](https://youtube.com/gamecore)

## 📝 更新日誌

### 版本 1.0.0 (2025-01-16)

- ✅ 初始版本發布
- ✅ 完整的資料庫結構
- ✅ 測試資料和種子資料
- ✅ 效能索引優化
- ✅ 維護腳本
- ✅ 完整部署文件

---

## 🎉 恭喜！

您已成功完成 GameCore 資料庫的部署！現在可以開始使用這個強大的遊戲平台系統。

如果您在部署過程中遇到任何問題，請參考故障排除章節或聯絡技術支援團隊。

**祝您使用愉快！** 🎮✨