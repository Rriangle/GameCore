# GameCore 測試執行腳本
# 執行完整的測試套件並生成報告

param(
    [string]$TestType = "All",           # All, Unit, Integration, E2E, Performance
    [string]$Configuration = "Release",  # Debug, Release
    [switch]$Coverage,                   # 是否生成覆蓋率報告
    [switch]$Verbose,                    # 詳細輸出
    [string]$Filter = "",                # 測試篩選器
    [string]$Output = "TestResults"      # 輸出目錄
)

Write-Host "🚀 GameCore 測試套件執行器" -ForegroundColor Cyan
Write-Host "測試類型: $TestType" -ForegroundColor Yellow
Write-Host "配置: $Configuration" -ForegroundColor Yellow
Write-Host "輸出目錄: $Output" -ForegroundColor Yellow

# 創建輸出目錄
if (!(Test-Path $Output)) {
    New-Item -ItemType Directory -Path $Output | Out-Null
}

# 清理舊的測試結果
Remove-Item "$Output\*" -Force -Recurse -ErrorAction SilentlyContinue

# 基本 dotnet test 參數
$testArgs = @(
    "test"
    "GameCore.Tests\GameCore.Tests.csproj"
    "--configuration", $Configuration
    "--logger", "trx;LogFileName=$Output\TestResults.trx"
    "--logger", "html;LogFileName=$Output\TestResults.html"
    "--results-directory", $Output
)

# 添加詳細輸出
if ($Verbose) {
    $testArgs += "--verbosity", "detailed"
}

# 添加測試篩選器
if ($TestType -ne "All") {
    switch ($TestType) {
        "Unit" { $testArgs += "--filter", "Category=Unit|FullyQualifiedName~UnitTests" }
        "Integration" { $testArgs += "--filter", "Category=Integration|FullyQualifiedName~IntegrationTests" }
        "E2E" { $testArgs += "--filter", "Category=E2E|FullyQualifiedName~EndToEndTests" }
        "Performance" { $testArgs += "--filter", "Category=Performance|FullyQualifiedName~PerformanceTests" }
    }
}

# 自定義篩選器
if ($Filter) {
    $testArgs += "--filter", $Filter
}

# 覆蓋率設定
if ($Coverage) {
    Write-Host "📊 啟用代碼覆蓋率分析..." -ForegroundColor Green
    $testArgs += "--collect", "XPlat Code Coverage"
    $testArgs += "--settings", "GameCore.Tests\coverlet.runsettings"
}

Write-Host "🔧 建置測試專案..." -ForegroundColor Green
dotnet build GameCore.Tests\GameCore.Tests.csproj --configuration $Configuration

if ($LASTEXITCODE -ne 0) {
    Write-Host "❌ 建置失敗！" -ForegroundColor Red
    exit 1
}

Write-Host "🧪 執行測試..." -ForegroundColor Green
Write-Host "命令: dotnet $($testArgs -join ' ')" -ForegroundColor DarkGray

$stopwatch = [System.Diagnostics.Stopwatch]::StartNew()
& dotnet @testArgs
$testExitCode = $LASTEXITCODE
$stopwatch.Stop()

Write-Host ""
Write-Host "⏱️  測試執行時間: $($stopwatch.Elapsed.ToString('mm\:ss\.fff'))" -ForegroundColor Cyan

# 處理覆蓋率報告
if ($Coverage -and $testExitCode -eq 0) {
    Write-Host "📊 生成覆蓋率報告..." -ForegroundColor Green
    
    # 查找覆蓋率文件
    $coverageFile = Get-ChildItem -Path $Output -Recurse -Filter "coverage.cobertura.xml" | Select-Object -First 1
    
    if ($coverageFile) {
        # 安裝 ReportGenerator 工具（如果尚未安裝）
        $reportGeneratorPath = "$env:USERPROFILE\.dotnet\tools\reportgenerator.exe"
        
        if (!(Test-Path $reportGeneratorPath)) {
            Write-Host "安裝 ReportGenerator..." -ForegroundColor Yellow
            dotnet tool install --global dotnet-reportgenerator-globaltool
        }
        
        # 生成 HTML 覆蓋率報告
        $coverageOutput = "$Output\CoverageReport"
        & dotnet reportgenerator -reports:$coverageFile.FullName -targetdir:$coverageOutput -reporttypes:Html
        
        Write-Host "✅ 覆蓋率報告已生成: $coverageOutput\index.html" -ForegroundColor Green
    }
    else {
        Write-Host "⚠️  未找到覆蓋率文件" -ForegroundColor Yellow
    }
}

# 顯示測試結果摘要
Write-Host ""
Write-Host "📋 測試結果摘要:" -ForegroundColor Cyan

if (Test-Path "$Output\TestResults.trx") {
    # 解析 TRX 文件獲取測試統計
    try {
        [xml]$trxContent = Get-Content "$Output\TestResults.trx"
        $counters = $trxContent.TestRun.ResultSummary.Counters
        
        Write-Host "  ✅ 總測試數: $($counters.total)" -ForegroundColor White
        Write-Host "  ✅ 通過: $($counters.passed)" -ForegroundColor Green
        Write-Host "  ❌ 失敗: $($counters.failed)" -ForegroundColor Red
        Write-Host "  ⏭️  跳過: $($counters.notExecuted)" -ForegroundColor Yellow
        
        if ($counters.failed -gt 0) {
            Write-Host ""
            Write-Host "❌ 有測試失敗！請檢查詳細報告。" -ForegroundColor Red
        }
    }
    catch {
        Write-Host "  無法解析測試結果文件" -ForegroundColor Yellow
    }
}

# 開啟測試報告（可選）
if ($testExitCode -eq 0) {
    Write-Host ""
    Write-Host "🎉 測試執行完成！" -ForegroundColor Green
    
    $response = Read-Host "是否要開啟測試報告？ (y/N)"
    if ($response -eq 'y' -or $response -eq 'Y') {
        if (Test-Path "$Output\TestResults.html") {
            Start-Process "$Output\TestResults.html"
        }
        
        if ($Coverage -and (Test-Path "$Output\CoverageReport\index.html")) {
            Start-Process "$Output\CoverageReport\index.html"
        }
    }
}
else {
    Write-Host ""
    Write-Host "❌ 測試執行失敗！退出代碼: $testExitCode" -ForegroundColor Red
}

# 清理暫存檔案
Write-Host "🧹 清理暫存檔案..." -ForegroundColor Gray
Get-ChildItem -Path . -Recurse -Filter "*.tmp" -ErrorAction SilentlyContinue | Remove-Item -Force
Get-ChildItem -Path . -Recurse -Filter "test_*.db" -ErrorAction SilentlyContinue | Remove-Item -Force

Write-Host "✨ 完成！" -ForegroundColor Cyan

exit $testExitCode
