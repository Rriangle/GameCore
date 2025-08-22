# GameCore 專案檔案編碼轉換腳本
# 將所有相關檔案轉換為 UTF-8 with BOM 編碼以防止錯誤

Write-Host "開始轉換 GameCore 專案檔案編碼為 UTF-8 with BOM..." -ForegroundColor Green

# 定義需要轉換的檔案類型
$fileExtensions = @(
    "*.cs",           # C# 檔案
    "*.cshtml",       # Razor 視圖檔案
    "*.html",         # HTML 檔案
    "*.css",          # CSS 檔案
    "*.js",           # JavaScript 檔案
    "*.json",         # JSON 檔案
    "*.xml",          # XML 檔案
    "*.md",           # Markdown 檔案
    "*.txt",          # 文字檔案
    "*.sql",          # SQL 檔案
    "*.ps1",          # PowerShell 腳本
    "*.yml",          # YAML 檔案
    "*.yaml",         # YAML 檔案
    "*.sh",           # Shell 腳本
    "*.dockerfile",   # Dockerfile
    "Dockerfile*"     # Dockerfile 檔案
)

# 定義需要排除的目錄
$excludeDirectories = @(
    "bin",
    "obj",
    ".git",
    "node_modules",
    "packages"
)

# 轉換檔案編碼的函數
function Convert-FileToUtf8Bom {
    param(
        [string]$FilePath
    )
    
    try {
        # 讀取檔案內容
        $content = Get-Content -Path $FilePath -Raw -Encoding UTF8
        
        # 檢查是否已經有 BOM
        $bytes = [System.IO.File]::ReadAllBytes($FilePath)
        if ($bytes.Length -ge 3 -and $bytes[0] -eq 0xEF -and $bytes[1] -eq 0xBB -and $bytes[2] -eq 0xBF) {
            Write-Host "檔案已為 UTF-8 with BOM: $FilePath" -ForegroundColor Yellow
            return
        }
        
        # 寫入檔案並加上 BOM
        $utf8WithBom = New-Object System.Text.UTF8Encoding($true)
        [System.IO.File]::WriteAllText($FilePath, $content, $utf8WithBom)
        
        Write-Host "已轉換: $FilePath" -ForegroundColor Green
    }
    catch {
        Write-Host "轉換失敗: $FilePath - $($_.Exception.Message)" -ForegroundColor Red
    }
}

# 遞迴搜尋並轉換檔案
function Convert-DirectoryFiles {
    param(
        [string]$Directory
    )
    
    foreach ($extension in $fileExtensions) {
        $files = Get-ChildItem -Path $Directory -Filter $extension -Recurse -File | Where-Object {
            $exclude = $false
            foreach ($excludeDir in $excludeDirectories) {
                if ($_.FullName -like "*\$excludeDir\*") {
                    $exclude = $true
                    break
                }
            }
            -not $exclude
        }
        
        foreach ($file in $files) {
            Convert-FileToUtf8Bom -FilePath $file.FullName
        }
    }
}

# 轉換根目錄檔案
Write-Host "轉換根目錄檔案..." -ForegroundColor Cyan
$rootFiles = Get-ChildItem -Path "." -File | Where-Object {
    $fileExtensions -contains $_.Extension -or 
    $_.Name -like "Dockerfile*" -or
    $_.Name -like "*.md" -or
    $_.Name -like "*.txt" -or
    $_.Name -like "*.sql" -or
    $_.Name -like "*.ps1" -or
    $_.Name -like "*.yml" -or
    $_.Name -like "*.yaml" -or
    $_.Name -like "*.sh"
}

foreach ($file in $rootFiles) {
    Convert-FileToUtf8Bom -FilePath $file.FullName
}

# 轉換主要專案目錄
$projectDirectories = @(
    "GameCore.Web",
    "GameCore.Core", 
    "GameCore.Infrastructure",
    "GameCore.Tests",
    "Database",
    "Tools",
    "scripts",
    ".github",
    "Documentation"
)

foreach ($dir in $projectDirectories) {
    if (Test-Path $dir) {
        Write-Host "轉換目錄: $dir" -ForegroundColor Cyan
        Convert-DirectoryFiles -Directory $dir
    }
}

Write-Host "檔案編碼轉換完成！" -ForegroundColor Green
Write-Host "所有相關檔案已轉換為 UTF-8 with BOM 編碼。" -ForegroundColor Green
