-- GameCore 資料庫維護腳本
-- 版本: 1.0.0
-- 建立時間: 2025年1月16日
-- 用途: 資料庫日常維護、效能監控、資料清理

USE GameCore;
GO

PRINT '=== GameCore 資料庫維護腳本開始執行 ===';
PRINT '執行時間: ' + CONVERT(NVARCHAR, GETUTCDATE(), 120);
PRINT '';

-- 1. 資料庫健康檢查
PRINT '🔍 執行資料庫健康檢查...';

-- 檢查資料庫狀態
SELECT 
    DB_NAME() AS DatabaseName,
    DATABASEPROPERTYEX(DB_NAME(), 'Status') AS Status,
    DATABASEPROPERTYEX(DB_NAME(), 'Recovery') AS RecoveryModel,
    DATABASEPROPERTYEX(DB_NAME(), 'IsAutoClose') AS IsAutoClose,
    DATABASEPROPERTYEX(DB_NAME(), 'IsAutoShrink') AS IsAutoShrink;

-- 檢查資料庫大小
SELECT 
    DB_NAME() AS DatabaseName,
    SUM(size * 8 / 1024) AS TotalSizeMB,
    SUM(CASE WHEN type = 0 THEN size * 8 / 1024 ELSE 0 END) AS DataSizeMB,
    SUM(CASE WHEN type = 1 THEN size * 8 / 1024 ELSE 0 END) AS LogSizeMB,
    SUM(CASE WHEN type = 2 THEN size * 8 / 1024 ELSE 0 END) AS FileStreamSizeMB
FROM sys.database_files
GROUP BY type;

-- 檢查資料表大小
SELECT 
    t.name AS TableName,
    p.rows AS RowCounts,
    CAST(ROUND((SUM(a.total_pages) * 8) / 1024.00, 2) AS NUMERIC(36, 2)) AS TotalSpaceMB,
    CAST(ROUND((SUM(a.used_pages) * 8) / 1024.00, 2) AS NUMERIC(36, 2)) AS UsedSpaceMB,
    CAST(ROUND(((SUM(a.total_pages) - SUM(a.used_pages)) * 8) / 1024.00, 2) AS NUMERIC(36, 2)) AS UnusedSpaceMB
FROM sys.tables t
INNER JOIN sys.indexes i ON t.object_id = i.object_id
INNER JOIN sys.partitions p ON i.object_id = p.OBJECT_ID AND i.index_id = p.index_id
INNER JOIN sys.allocation_units a ON p.partition_id = a.container_id
WHERE t.name IN ('Users', 'Pets', 'Posts', 'StoreProducts', 'MarketTransactions')
GROUP BY t.name, p.rows
ORDER BY TotalSpaceMB DESC;

PRINT '✅ 資料庫健康檢查完成';
PRINT '';

-- 2. 索引維護
PRINT '🔧 執行索引維護...';

-- 檢查索引碎片
SELECT 
    OBJECT_NAME(ind.OBJECT_ID) AS TableName,
    ind.name AS IndexName,
    indexstats.avg_fragmentation_in_percent AS FragmentationPercent,
    indexstats.page_count AS PageCount
FROM sys.dm_db_index_physical_stats(DB_ID(), NULL, NULL, NULL, NULL) indexstats
INNER JOIN sys.indexes ind ON ind.object_id = indexstats.object_id AND ind.index_id = indexstats.index_id
WHERE indexstats.avg_fragmentation_in_percent > 10
ORDER BY indexstats.avg_fragmentation_in_percent DESC;

-- 重建碎片嚴重的索引
DECLARE @TableName NVARCHAR(128);
DECLARE @IndexName NVARCHAR(128);
DECLARE @SQL NVARCHAR(MAX);

DECLARE IndexCursor CURSOR FOR
SELECT 
    OBJECT_NAME(ind.OBJECT_ID),
    ind.name
FROM sys.dm_db_index_physical_stats(DB_ID(), NULL, NULL, NULL, NULL) indexstats
INNER JOIN sys.indexes ind ON ind.object_id = indexstats.object_id AND ind.index_id = indexstats.index_id
WHERE indexstats.avg_fragmentation_in_percent > 30;

OPEN IndexCursor;
FETCH NEXT FROM IndexCursor INTO @TableName, @IndexName;

WHILE @@FETCH_STATUS = 0
BEGIN
    SET @SQL = 'ALTER INDEX [' + @IndexName + '] ON [' + @TableName + '] REBUILD';
    EXEC sp_executesql @SQL;
    PRINT '✅ 重建索引: ' + @TableName + '.' + @IndexName;
    
    FETCH NEXT FROM IndexCursor INTO @TableName, @IndexName;
END

CLOSE IndexCursor;
DEALLOCATE IndexCursor;

-- 重新組織碎片較輕的索引
DECLARE IndexCursor2 CURSOR FOR
SELECT 
    OBJECT_NAME(ind.OBJECT_ID),
    ind.name
FROM sys.dm_db_index_physical_stats(DB_ID(), NULL, NULL, NULL, NULL) indexstats
INNER JOIN sys.indexes ind ON ind.object_id = indexstats.object_id AND ind.index_id = indexstats.index_id
WHERE indexstats.avg_fragmentation_in_percent BETWEEN 10 AND 30;

OPEN IndexCursor2;
FETCH NEXT FROM IndexCursor2 INTO @TableName, @IndexName;

WHILE @@FETCH_STATUS = 0
BEGIN
    SET @SQL = 'ALTER INDEX [' + @IndexName + '] ON [' + @TableName + '] REORGANIZE';
    EXEC sp_executesql @SQL;
    PRINT '✅ 重新組織索引: ' + @TableName + '.' + @IndexName;
    
    FETCH NEXT FROM IndexCursor2 INTO @TableName, @IndexName;
END

CLOSE IndexCursor2;
DEALLOCATE IndexCursor2;

PRINT '✅ 索引維護完成';
PRINT '';

-- 3. 統計資訊更新
PRINT '📊 更新統計資訊...';

-- 更新所有資料表的統計資訊
UPDATE STATISTICS Users;
UPDATE STATISTICS Pets;
UPDATE STATISTICS SignInRecords;
UPDATE STATISTICS MiniGameRecords;
UPDATE STATISTICS Forums;
UPDATE STATISTICS Posts;
UPDATE STATISTICS PostReplies;
UPDATE STATISTICS PostLikes;
UPDATE STATISTICS PostBookmarks;
UPDATE STATISTICS ChatRooms;
UPDATE STATISTICS ChatRoomMembers;
UPDATE STATISTICS ChatMessages;
UPDATE STATISTICS PrivateChats;
UPDATE STATISTICS PrivateMessages;
UPDATE STATISTICS Notifications;
UPDATE STATISTICS NotificationSources;
UPDATE STATISTICS NotificationActions;
UPDATE STATISTICS StoreProducts;
UPDATE STATISTICS StoreOrders;
UPDATE STATISTICS StoreOrderItems;
UPDATE STATISTICS ShoppingCartItems;
UPDATE STATISTICS MarketTransactions;
UPDATE STATISTICS MarketReviews;
UPDATE STATISTICS Managers;
UPDATE STATISTICS ManagerRoles;
UPDATE STATISTICS Permissions;
UPDATE STATISTICS RolePermissions;
UPDATE STATISTICS PetInteractions;

PRINT '✅ 統計資訊更新完成';
PRINT '';

-- 4. 資料清理
PRINT '🧹 執行資料清理...';

-- 清理過期的通知（30天前）
DECLARE @DeletedNotifications INT;
DELETE FROM Notifications 
WHERE CreateTime < DATEADD(day, -30, GETUTCDATE()) 
AND IsRead = 1;

SET @DeletedNotifications = @@ROWCOUNT;
PRINT '   清理過期通知: ' + CAST(@DeletedNotifications AS NVARCHAR(10)) + ' 筆';

-- 清理過期的聊天訊息（90天前）
DECLARE @DeletedChatMessages INT;
DELETE FROM ChatMessages 
WHERE CreateTime < DATEADD(day, -90, GETUTCDATE());

SET @DeletedChatMessages = @@ROWCOUNT;
PRINT '   清理過期聊天訊息: ' + CAST(@DeletedChatMessages AS NVARCHAR(10)) + ' 筆';

-- 清理過期的私聊訊息（90天前）
DECLARE @DeletedPrivateMessages INT;
DELETE FROM PrivateMessages 
WHERE CreateTime < DATEADD(day, -90, GETUTCDATE());

SET @DeletedPrivateMessages = @@ROWCOUNT;
PRINT '   清理過期私聊訊息: ' + CAST(@DeletedPrivateMessages AS NVARCHAR(10)) + ' 筆';

-- 清理過期的小遊戲記錄（180天前）
DECLARE @DeletedGameRecords INT;
DELETE FROM MiniGameRecords 
WHERE CreateTime < DATEADD(day, -180, GETUTCDATE());

SET @DeletedGameRecords = @@ROWCOUNT;
PRINT '   清理過期遊戲記錄: ' + CAST(@DeletedGameRecords AS NVARCHAR(10)) + ' 筆';

-- 清理過期的簽到記錄（365天前）
DECLARE @DeletedSignInRecords INT;
DELETE FROM SignInRecords 
WHERE SignInDate < DATEADD(day, -365, GETDATE());

SET @DeletedSignInRecords = @@ROWCOUNT;
PRINT '   清理過期簽到記錄: ' + CAST(@DeletedSignInRecords AS NVARCHAR(10)) + ' 筆';

PRINT '✅ 資料清理完成';
PRINT '';

-- 5. 效能監控
PRINT '📈 執行效能監控...';

-- 檢查慢查詢
SELECT 
    qs.execution_count AS ExecutionCount,
    qs.total_elapsed_time / 1000000 AS TotalElapsedTimeSeconds,
    qs.avg_elapsed_time / 1000000 AS AvgElapsedTimeSeconds,
    qs.total_logical_reads AS TotalLogicalReads,
    qs.avg_logical_reads AS AvgLogicalReads,
    SUBSTRING(qt.text, (qs.statement_start_offset/2)+1, 
        ((CASE qs.statement_end_offset
            WHEN -1 THEN DATALENGTH(qt.text)
            ELSE qs.statement_end_offset
        END - qs.statement_start_offset)/2) + 1) AS QueryText
FROM sys.dm_exec_query_stats qs
CROSS APPLY sys.dm_exec_sql_text(qs.sql_handle) qt
WHERE qs.avg_elapsed_time > 1000000  -- 超過1秒的查詢
ORDER BY qs.avg_elapsed_time DESC;

-- 檢查索引使用情況
SELECT 
    OBJECT_NAME(i.object_id) AS TableName,
    i.name AS IndexName,
    i.type_desc AS IndexType,
    ius.user_seeks AS UserSeeks,
    ius.user_scans AS UserScans,
    ius.user_lookups AS UserLookups,
    ius.user_updates AS UserUpdates,
    ius.last_user_seek AS LastUserSeek,
    ius.last_user_scan AS LastUserScan
FROM sys.dm_db_index_usage_stats ius
INNER JOIN sys.indexes i ON ius.object_id = i.object_id AND ius.index_id = i.index_id
WHERE ius.database_id = DB_ID()
AND OBJECT_NAME(i.object_id) IN ('Users', 'Pets', 'Posts', 'StoreProducts', 'MarketTransactions')
ORDER BY (ius.user_seeks + ius.user_scans + ius.user_lookups) DESC;

-- 檢查等待統計
SELECT 
    wait_type AS WaitType,
    waiting_tasks_count AS WaitingTasksCount,
    wait_time_ms AS WaitTimeMs,
    max_wait_time_ms AS MaxWaitTimeMs,
    signal_wait_time_ms AS SignalWaitTimeMs
FROM sys.dm_os_wait_stats
WHERE wait_type IN ('PAGEIOLATCH_SH', 'PAGEIOLATCH_EX', 'LCK_M_S', 'LCK_M_X')
ORDER BY wait_time_ms DESC;

PRINT '✅ 效能監控完成';
PRINT '';

-- 6. 資料庫備份檢查
PRINT '💾 檢查資料庫備份狀態...';

-- 檢查最後備份時間
SELECT 
    d.name AS DatabaseName,
    MAX(b.backup_finish_date) AS LastBackupDate,
    DATEDIFF(day, MAX(b.backup_finish_date), GETDATE()) AS DaysSinceLastBackup
FROM sys.databases d
LEFT JOIN msdb.dbo.backupset b ON d.name = b.database_name
WHERE d.name = 'GameCore'
GROUP BY d.name;

-- 檢查備份大小
SELECT 
    database_name AS DatabaseName,
    backup_size / 1024 / 1024 AS BackupSizeMB,
    compressed_backup_size / 1024 / 1024 AS CompressedBackupSizeMB,
    backup_finish_date AS BackupFinishDate
FROM msdb.dbo.backupset
WHERE database_name = 'GameCore'
ORDER BY backup_finish_date DESC;

PRINT '✅ 備份狀態檢查完成';
PRINT '';

-- 7. 資料庫一致性檢查
PRINT '🔍 執行資料庫一致性檢查...';

-- 檢查資料庫完整性
DBCC CHECKDB('GameCore') WITH NO_INFOMSGS;

-- 檢查索引完整性
DBCC CHECKCATALOG('GameCore') WITH NO_INFOMSGS;

PRINT '✅ 資料庫一致性檢查完成';
PRINT '';

-- 8. 效能建議
PRINT '💡 生成效能建議...';

-- 檢查未使用的索引
SELECT 
    OBJECT_NAME(i.object_id) AS TableName,
    i.name AS IndexName,
    i.type_desc AS IndexType,
    ius.user_seeks + ius.user_scans + ius.user_lookups AS TotalUsage
FROM sys.dm_db_index_usage_stats ius
INNER JOIN sys.indexes i ON ius.object_id = i.object_id AND ius.index_id = i.index_id
WHERE ius.database_id = DB_ID()
AND ius.user_seeks + ius.user_scans + ius.user_lookups = 0
AND i.is_primary_key = 0
AND i.is_unique = 0
AND OBJECT_NAME(i.object_id) IN ('Users', 'Pets', 'Posts', 'StoreProducts', 'MarketTransactions');

-- 檢查缺少的索引
SELECT 
    dm_mid.database_id AS DatabaseID,
    dm_migs.avg_user_impact AS AvgUserImpact,
    dm_migs.last_user_seek AS LastUserSeek,
    dm_mid.statement AS TableName,
    dm_mid.equality_columns AS EqualityColumns,
    dm_mid.inequality_columns AS InequalityColumns,
    dm_mid.included_columns AS IncludedColumns,
    dm_migs.unique_compiles AS UniqueCompiles,
    dm_migs.user_seeks AS UserSeeks,
    dm_migs.avg_total_user_cost AS AvgTotalUserCost,
    dm_migs.avg_user_impact AS AvgUserImpact
FROM sys.dm_db_missing_index_group_stats dm_migs
INNER JOIN sys.dm_db_missing_index_groups dm_mig ON dm_migs.group_handle = dm_mig.index_group_handle
INNER JOIN sys.dm_db_missing_index_details dm_mid ON dm_mig.index_handle = dm_mid.index_handle
WHERE dm_mid.database_id = DB_ID()
ORDER BY dm_migs.avg_user_impact DESC;

PRINT '✅ 效能建議生成完成';
PRINT '';

-- 9. 維護摘要報告
PRINT '📋 維護摘要報告';
PRINT '================';

-- 計算維護後的統計
DECLARE @TotalTables INT;
DECLARE @TotalIndexes INT;
DECLARE @TotalSizeMB DECIMAL(10,2);

SELECT @TotalTables = COUNT(*) FROM sys.tables;
SELECT @TotalIndexes = COUNT(*) FROM sys.indexes i
INNER JOIN sys.tables t ON i.object_id = t.object_id;
SELECT @TotalSizeMB = SUM(size * 8 / 1024) FROM sys.database_files;

PRINT '資料庫名稱: ' + DB_NAME();
PRINT '資料表數量: ' + CAST(@TotalTables AS NVARCHAR(10));
PRINT '索引數量: ' + CAST(@TotalIndexes AS NVARCHAR(10));
PRINT '資料庫大小: ' + CAST(@TotalSizeMB AS NVARCHAR(20)) + ' MB';
PRINT '維護時間: ' + CONVERT(NVARCHAR, GETUTCDATE(), 120);
PRINT '';

-- 清理統計
PRINT '清理統計:';
PRINT '  - 過期通知: ' + CAST(@DeletedNotifications AS NVARCHAR(10)) + ' 筆';
PRINT '  - 過期聊天訊息: ' + CAST(@DeletedChatMessages AS NVARCHAR(10)) + ' 筆';
PRINT '  - 過期私聊訊息: ' + CAST(@DeletedPrivateMessages AS NVARCHAR(10)) + ' 筆';
PRINT '  - 過期遊戲記錄: ' + CAST(@DeletedGameRecords AS NVARCHAR(10)) + ' 筆';
PRINT '  - 過期簽到記錄: ' + CAST(@DeletedSignInRecords AS NVARCHAR(10)) + ' 筆';
PRINT '';

PRINT '✅ 資料庫維護完成！';
PRINT '✅ 建議每週執行一次完整維護';
PRINT '✅ 建議每日執行統計更新';
PRINT '✅ 建議每月執行索引重建';
PRINT '';
PRINT '💡 下次維護時間: ' + CONVERT(NVARCHAR, DATEADD(week, 1, GETUTCDATE()), 120);
PRINT '';
PRINT '=== GameCore 資料庫維護腳本執行完成 ===';