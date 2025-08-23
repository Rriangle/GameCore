-- GameCore 資料庫初始化腳本
-- 版本: 1.0.0
-- 建立時間: 2025年1月16日
-- 用途: 初始化資料庫環境，建立資料庫和基本設定

USE master;
GO

-- 檢查 SQL Server 版本
DECLARE @Version NVARCHAR(128);
SELECT @Version = @@VERSION;
PRINT 'SQL Server 版本: ' + @Version;

-- 檢查是否支援 SQL Server 2019 或更新版本
IF CAST(REPLACE(SUBSTRING(@@VERSION, CHARINDEX('SQL Server', @@VERSION) + 11, 4), '.', '') AS INT) < 150
BEGIN
    PRINT '❌ 警告: 建議使用 SQL Server 2019 或更新版本';
    PRINT '   當前版本可能不支援某些功能';
END
ELSE
BEGIN
    PRINT '✅ SQL Server 版本符合要求';
END

-- 檢查資料庫是否存在
IF EXISTS (SELECT name FROM sys.databases WHERE name = 'GameCore')
BEGIN
    PRINT '⚠️  GameCore 資料庫已存在';
    PRINT '   如需重新建立，請先執行清理腳本或手動刪除';
    
    -- 詢問是否要刪除現有資料庫
    PRINT '';
    PRINT '請選擇操作:';
    PRINT '1. 使用現有資料庫 (建議)';
    PRINT '2. 刪除並重新建立資料庫 (會遺失所有資料)';
    PRINT '';
    PRINT '請在 SQL Server Management Studio 中選擇，或修改此腳本';
    
    -- 預設使用現有資料庫
    PRINT '✅ 使用現有 GameCore 資料庫';
    USE GameCore;
    GO
    
    -- 檢查資料表是否存在
    IF EXISTS (SELECT * FROM sys.tables WHERE name = 'Users')
    BEGIN
        PRINT '✅ 資料表結構完整';
        
        -- 檢查是否有資料
        DECLARE @UserCount INT;
        SELECT @UserCount = COUNT(*) FROM Users;
        PRINT '   用戶數量: ' + CAST(@UserCount AS NVARCHAR(10));
        
        IF @UserCount > 0
        BEGIN
            PRINT '✅ 資料庫已包含測試資料';
            PRINT '   可以直接使用，無需重新初始化';
        END
        ELSE
        BEGIN
            PRINT '⚠️  資料庫結構完整但無測試資料';
            PRINT '   請執行 02-SeedData.sql 插入測試資料';
        END
    END
    ELSE
    BEGIN
        PRINT '❌ 資料表結構不完整';
        PRINT '   請執行 01-CreateTables.sql 建立資料表';
    END
END
ELSE
BEGIN
    PRINT '✅ GameCore 資料庫不存在，準備建立';
    
    -- 檢查磁碟空間
    DECLARE @Drive NVARCHAR(1);
    DECLARE @FreeSpaceMB BIGINT;
    
    SELECT @Drive = LEFT(physical_name, 1)
    FROM sys.master_files
    WHERE database_id = 1 AND file_id = 1;
    
    EXEC xp_fixeddrives;
    
    -- 建立資料庫
    CREATE DATABASE GameCore
    ON PRIMARY (
        NAME = 'GameCore',
        FILENAME = @Drive + ':\Data\GameCore.mdf',
        SIZE = 100MB,
        MAXSIZE = UNLIMITED,
        FILEGROWTH = 10MB
    )
    LOG ON (
        NAME = 'GameCore_Log',
        FILENAME = @Drive + ':\Logs\GameCore.ldf',
        SIZE = 50MB,
        MAXSIZE = UNLIMITED,
        FILEGROWTH = 10MB
    );
    
    PRINT '✅ GameCore 資料庫建立成功';
    PRINT '   資料檔案: ' + @Drive + ':\Data\GameCore.mdf';
    PRINT '   日誌檔案: ' + @Drive + ':\Logs\GameCore.ldf';
    
    -- 設定資料庫選項
    ALTER DATABASE GameCore SET RECOVERY SIMPLE;
    ALTER DATABASE GameCore SET AUTO_CLOSE OFF;
    ALTER DATABASE GameCore SET AUTO_SHRINK OFF;
    ALTER DATABASE GameCore SET PAGE_VERIFY CHECKSUM;
    
    PRINT '✅ 資料庫選項設定完成';
    
    -- 切換到新建立的資料庫
    USE GameCore;
    GO
    
    PRINT '';
    PRINT '📋 下一步操作:';
    PRINT '1. 執行 01-CreateTables.sql 建立資料表結構';
    PRINT '2. 執行 02-SeedData.sql 插入測試資料';
    PRINT '3. 執行 03-CreateIndexes.sql 建立效能索引 (可選)';
    PRINT '';
    PRINT '⚠️  注意: 請確保有足夠的磁碟空間和權限';
END

GO

-- 檢查當前資料庫
SELECT 
    DB_NAME() AS CurrentDatabase,
    DATABASEPROPERTYEX(DB_NAME(), 'Status') AS DatabaseStatus,
    DATABASEPROPERTYEX(DB_NAME(), 'Recovery') AS RecoveryModel,
    DATABASEPROPERTYEX(DB_NAME(), 'IsAutoClose') AS IsAutoClose,
    DATABASEPROPERTYEX(DB_NAME(), 'IsAutoShrink') AS IsAutoShrink;

-- 檢查資料庫大小
SELECT 
    DB_NAME() AS DatabaseName,
    SUM(size * 8 / 1024) AS SizeMB,
    SUM(CASE WHEN type = 0 THEN size * 8 / 1024 ELSE 0 END) AS DataSizeMB,
    SUM(CASE WHEN type = 1 THEN size * 8 / 1024 ELSE 0 END) AS LogSizeMB
FROM sys.database_files
GROUP BY type;

PRINT '';
PRINT '=== GameCore 資料庫初始化完成 ===';
PRINT '建立時間: ' + CONVERT(NVARCHAR, GETUTCDATE(), 120);
PRINT '資料庫: ' + DB_NAME();
PRINT '狀態: ' + CAST(DATABASEPROPERTYEX(DB_NAME(), 'Status') AS NVARCHAR(50));
PRINT '';
PRINT '🎉 資料庫環境已準備就緒！';
PRINT '請繼續執行後續的資料表建立和資料插入腳本。';