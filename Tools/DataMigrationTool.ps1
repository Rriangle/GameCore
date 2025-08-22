# GameCore 資料遷移和管理工具
# 用於管理資料庫初始化、假資料生成和備份還原

param(
    [Parameter(Mandatory=$true)]
    [ValidateSet("Init", "SeedBasic", "SeedMassive", "Backup", "Restore", "Clean", "Status")]
    [string]$Action,
    
    [string]$ConnectionString = "",
    [string]$BackupPath = "Backups",
    [switch]$Force,
    [switch]$Verbose
)

# 設定變數
$ScriptPath = Split-Path -Parent $MyInvocation.MyCommand.Definition
$DatabasePath = Join-Path (Split-Path -Parent $ScriptPath) "Database"
$LogPath = Join-Path $ScriptPath "Logs"

# 預設連線字串（如果沒有提供）
if ([string]::IsNullOrEmpty($ConnectionString)) {
    $ConnectionString = "Server=(localdb)\MSSQLLocalDB;Database=GameCore;Trusted_Connection=true;TrustServerCertificate=true;"
}

# 確保目錄存在
if (!(Test-Path $LogPath)) {
    New-Item -ItemType Directory -Path $LogPath | Out-Null
}
if (!(Test-Path $BackupPath)) {
    New-Item -ItemType Directory -Path $BackupPath | Out-Null
}

# 日誌函數
function Write-Log {
    param([string]$Message, [string]$Level = "INFO")
    $timestamp = Get-Date -Format "yyyy-MM-dd HH:mm:ss"
    $logMessage = "[$timestamp] [$Level] $Message"
    Write-Host $logMessage
    
    $logFile = Join-Path $LogPath "migration_$(Get-Date -Format 'yyyyMMdd').log"
    Add-Content -Path $logFile -Value $logMessage
}

# SQL 執行函數
function Invoke-SqlScript {
    param(
        [string]$ScriptPath,
        [string]$ConnectionString,
        [string]$Description
    )
    
    try {
        Write-Log "開始執行: $Description" "INFO"
        Write-Log "腳本路徑: $ScriptPath" "DEBUG"
        
        if (!(Test-Path $ScriptPath)) {
            throw "找不到 SQL 腳本檔案: $ScriptPath"
        }
        
        # 使用 sqlcmd 執行 SQL 腳本
        $result = & sqlcmd -S "(localdb)\MSSQLLocalDB" -d "GameCore" -i $ScriptPath -b
        
        if ($LASTEXITCODE -eq 0) {
            Write-Log "$Description 執行成功" "SUCCESS"
            return $true
        } else {
            Write-Log "$Description 執行失敗，退出代碼: $LASTEXITCODE" "ERROR"
            Write-Log "錯誤輸出: $result" "ERROR"
            return $false
        }
    }
    catch {
        Write-Log "執行 $Description 時發生異常: $($_.Exception.Message)" "ERROR"
        return $false
    }
}

# 檢查資料庫狀態
function Get-DatabaseStatus {
    try {
        $query = @"
SELECT 
    (SELECT COUNT(*) FROM sys.tables WHERE name IN ('User', 'Game', 'Pet', 'ProductInfo')) as TableCount,
    (SELECT COUNT(*) FROM [User]) as UserCount,
    (SELECT COUNT(*) FROM [Game]) as GameCount,
    (SELECT COUNT(*) FROM [Pet]) as PetCount,
    (SELECT COUNT(*) FROM [ProductInfo]) as ProductCount
"@
        
        $result = & sqlcmd -S "(localdb)\MSSQLLocalDB" -d "GameCore" -Q $query -h -1
        
        if ($LASTEXITCODE -eq 0) {
            $parts = ($result | Where-Object { $_ -match '\d+' }) -split '\s+'
            return @{
                TablesExist = [int]$parts[0] -eq 4
                UserCount = [int]$parts[1]
                GameCount = [int]$parts[2]
                PetCount = [int]$parts[3]
                ProductCount = [int]$parts[4]
            }
        }
    }
    catch {
        return @{
            TablesExist = $false
            UserCount = 0
            GameCount = 0
            PetCount = 0
            ProductCount = 0
        }
    }
}

# 主要功能實現
switch ($Action) {
    "Init" {
        Write-Log "🚀 開始初始化 GameCore 資料庫..." "INFO"
        
        # 執行建表腳本
        $createScript = Join-Path $DatabasePath "01-CreateTables.sql"
        $success = Invoke-SqlScript -ScriptPath $createScript -ConnectionString $ConnectionString -Description "建立資料庫表格"
        
        if ($success) {
            Write-Log "✅ 資料庫初始化完成" "SUCCESS"
        } else {
            Write-Log "❌ 資料庫初始化失敗" "ERROR"
            exit 1
        }
    }
    
    "SeedBasic" {
        Write-Log "🌱 開始載入基礎假資料..." "INFO"
        
        $status = Get-DatabaseStatus
        if (!$status.TablesExist) {
            Write-Log "⚠️ 資料庫表格不存在，請先執行 Init 動作" "WARNING"
            exit 1
        }
        
        $seedScript = Join-Path $DatabasePath "02-InsertMockData.sql"
        $success = Invoke-SqlScript -ScriptPath $seedScript -ConnectionString $ConnectionString -Description "載入基礎假資料"
        
        if ($success) {
            Write-Log "✅ 基礎假資料載入完成" "SUCCESS"
        } else {
            Write-Log "❌ 基礎假資料載入失敗" "ERROR"
            exit 1
        }
    }
    
    "SeedMassive" {
        Write-Log "🎲 開始生成大量假資料..." "INFO"
        
        $status = Get-DatabaseStatus
        if (!$status.TablesExist) {
            Write-Log "⚠️ 資料庫表格不存在，請先執行 Init 動作" "WARNING"
            exit 1
        }
        
        if ($status.UserCount -eq 0 -and !$Force) {
            $response = Read-Host "資料庫中沒有基礎資料，是否要先載入基礎資料? (y/N)"
            if ($response -eq 'y' -or $response -eq 'Y') {
                & $MyInvocation.MyCommand.Path -Action "SeedBasic" -ConnectionString $ConnectionString
            }
        }
        
        $massiveScript = Join-Path $DatabasePath "03-GenerateMassiveData.sql"
        $success = Invoke-SqlScript -ScriptPath $massiveScript -ConnectionString $ConnectionString -Description "生成大量假資料"
        
        if ($success) {
            Write-Log "✅ 大量假資料生成完成" "SUCCESS"
            
            # 顯示最終統計
            $finalStatus = Get-DatabaseStatus
            Write-Log "📊 最終資料統計:" "INFO"
            Write-Log "  使用者: $($finalStatus.UserCount) 筆" "INFO"
            Write-Log "  遊戲: $($finalStatus.GameCount) 筆" "INFO"
            Write-Log "  寵物: $($finalStatus.PetCount) 筆" "INFO"
            Write-Log "  商品: $($finalStatus.ProductCount) 筆" "INFO"
        } else {
            Write-Log "❌ 大量假資料生成失敗" "ERROR"
            exit 1
        }
    }
    
    "Backup" {
        Write-Log "💾 開始備份資料庫..." "INFO"
        
        $backupFile = Join-Path $BackupPath "GameCore_$(Get-Date -Format 'yyyyMMdd_HHmmss').bak"
        
        $backupQuery = @"
BACKUP DATABASE [GameCore] 
TO DISK = '$backupFile'
WITH FORMAT, INIT, NAME = 'GameCore-Full Database Backup', SKIP, NOREWIND, NOUNLOAD, STATS = 10
"@
        
        try {
            & sqlcmd -S "(localdb)\MSSQLLocalDB" -Q $backupQuery
            
            if ($LASTEXITCODE -eq 0 -and (Test-Path $backupFile)) {
                $fileSize = [math]::Round((Get-Item $backupFile).Length / 1MB, 2)
                Write-Log "✅ 資料庫備份完成: $backupFile ($fileSize MB)" "SUCCESS"
            } else {
                Write-Log "❌ 資料庫備份失敗" "ERROR"
                exit 1
            }
        }
        catch {
            Write-Log "備份過程中發生異常: $($_.Exception.Message)" "ERROR"
            exit 1
        }
    }
    
    "Restore" {
        Write-Log "🔄 開始還原資料庫..." "INFO"
        
        # 列出可用的備份檔案
        $backupFiles = Get-ChildItem -Path $BackupPath -Filter "*.bak" | Sort-Object LastWriteTime -Descending
        
        if ($backupFiles.Count -eq 0) {
            Write-Log "❌ 找不到備份檔案" "ERROR"
            exit 1
        }
        
        Write-Log "可用的備份檔案:" "INFO"
        for ($i = 0; $i -lt $backupFiles.Count; $i++) {
            Write-Log "  [$i] $($backupFiles[$i].Name) ($($backupFiles[$i].LastWriteTime))" "INFO"
        }
        
        if (!$Force) {
            $selection = Read-Host "請選擇要還原的備份檔案編號 (0-$($backupFiles.Count-1))，或按 Enter 選擇最新的"
            if ([string]::IsNullOrEmpty($selection)) {
                $selectedFile = $backupFiles[0]
            } else {
                $selectedFile = $backupFiles[[int]$selection]
            }
        } else {
            $selectedFile = $backupFiles[0]
        }
        
        $restoreQuery = @"
USE master;
ALTER DATABASE [GameCore] SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
RESTORE DATABASE [GameCore] 
FROM DISK = '$($selectedFile.FullName)'
WITH REPLACE, STATS = 10;
ALTER DATABASE [GameCore] SET MULTI_USER;
"@
        
        try {
            & sqlcmd -S "(localdb)\MSSQLLocalDB" -Q $restoreQuery
            
            if ($LASTEXITCODE -eq 0) {
                Write-Log "✅ 資料庫還原完成: $($selectedFile.Name)" "SUCCESS"
            } else {
                Write-Log "❌ 資料庫還原失敗" "ERROR"
                exit 1
            }
        }
        catch {
            Write-Log "還原過程中發生異常: $($_.Exception.Message)" "ERROR"
            exit 1
        }
    }
    
    "Clean" {
        Write-Log "🧹 開始清理資料庫..." "WARNING"
        
        if (!$Force) {
            $confirmation = Read-Host "⚠️ 此操作將刪除所有資料，是否確定繼續? (yes/no)"
            if ($confirmation -ne "yes") {
                Write-Log "操作已取消" "INFO"
                exit 0
            }
        }
        
        $cleanQuery = @"
-- 清理所有使用者資料表
EXEC sp_MSforeachtable 'DELETE FROM ?'
-- 重置身份識別欄位
EXEC sp_MSforeachtable 'IF OBJECTPROPERTY(OBJECT_ID(''?''), ''TableHasIdentity'') = 1 DBCC CHECKIDENT(''?'', RESEED, 0)'
"@
        
        try {
            & sqlcmd -S "(localdb)\MSSQLLocalDB" -d "GameCore" -Q $cleanQuery
            
            if ($LASTEXITCODE -eq 0) {
                Write-Log "✅ 資料庫清理完成" "SUCCESS"
            } else {
                Write-Log "❌ 資料庫清理失敗" "ERROR"
                exit 1
            }
        }
        catch {
            Write-Log "清理過程中發生異常: $($_.Exception.Message)" "ERROR"
            exit 1
        }
    }
    
    "Status" {
        Write-Log "📊 檢查資料庫狀態..." "INFO"
        
        $status = Get-DatabaseStatus
        
        if ($status.TablesExist) {
            Write-Log "✅ 資料庫表格狀態: 正常" "SUCCESS"
            Write-Log "📈 資料統計:" "INFO"
            Write-Log "  👥 使用者: $($status.UserCount) 筆" "INFO"
            Write-Log "  🎮 遊戲: $($status.GameCount) 筆" "INFO"
            Write-Log "  🐱 寵物: $($status.PetCount) 筆" "INFO"
            Write-Log "  🛒 商品: $($status.ProductCount) 筆" "INFO"
            
            # 計算總記錄數
            $totalRecords = $status.UserCount + $status.GameCount + $status.PetCount + $status.ProductCount
            Write-Log "📊 主要表格總記錄數: $totalRecords 筆" "INFO"
            
            # 檢查是否有足夠的測試資料
            if ($totalRecords -gt 1000) {
                Write-Log "✅ 資料量充足，適合展示和測試" "SUCCESS"
            } elseif ($totalRecords -gt 100) {
                Write-Log "⚠️ 資料量適中，建議增加更多測試資料" "WARNING"
            } else {
                Write-Log "⚠️ 資料量較少，建議執行 SeedMassive 生成更多資料" "WARNING"
            }
        } else {
            Write-Log "❌ 資料庫表格不存在，請先執行 Init 動作" "ERROR"
        }
        
        # 檢查備份檔案
        $backupFiles = Get-ChildItem -Path $BackupPath -Filter "*.bak" -ErrorAction SilentlyContinue
        if ($backupFiles) {
            Write-Log "💾 可用備份: $($backupFiles.Count) 個檔案" "INFO"
            $latestBackup = $backupFiles | Sort-Object LastWriteTime -Descending | Select-Object -First 1
            Write-Log "  最新備份: $($latestBackup.Name) ($($latestBackup.LastWriteTime))" "INFO"
        } else {
            Write-Log "💾 沒有找到備份檔案" "INFO"
        }
    }
}

Write-Log "🎯 操作完成: $Action" "SUCCESS"
