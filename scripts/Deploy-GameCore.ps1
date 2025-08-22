# GameCore PowerShell 部署腳本
# 支援多種部署環境和方式

param(
    [Parameter(Mandatory=$false)]
    [ValidateSet("Development", "Staging", "Production")]
    [string]$Environment = "Development",
    
    [Parameter(Mandatory=$false)]
    [ValidateSet("Docker", "Kubernetes", "Azure", "Local")]
    [string]$DeploymentType = "Docker",
    
    [Parameter(Mandatory=$false)]
    [string]$Version = "",
    
    [Parameter(Mandatory=$false)]
    [string]$ConfigFile = "",
    
    [switch]$NoBuild,
    [switch]$NoTests,
    [switch]$BackupDatabase,
    [switch]$SkipConfirm,
    [switch]$Verbose
)

# 顏色定義
$Script:Colors = @{
    Red = "Red"
    Green = "Green"
    Yellow = "Yellow"
    Blue = "Blue"
    Cyan = "Cyan"
    Magenta = "Magenta"
}

# 日誌函數
function Write-LogInfo {
    param([string]$Message)
    Write-Host "[INFO] $Message" -ForegroundColor $Script:Colors.Blue
}

function Write-LogWarn {
    param([string]$Message)
    Write-Host "[WARN] $Message" -ForegroundColor $Script:Colors.Yellow
}

function Write-LogError {
    param([string]$Message)
    Write-Host "[ERROR] $Message" -ForegroundColor $Script:Colors.Red
}

function Write-LogSuccess {
    param([string]$Message)
    Write-Host "[SUCCESS] $Message" -ForegroundColor $Script:Colors.Green
}

function Write-LogDebug {
    param([string]$Message)
    if ($Verbose) {
        Write-Host "[DEBUG] $Message" -ForegroundColor $Script:Colors.Magenta
    }
}

# 顯示橫幅
function Show-Banner {
    Write-Host "========================================" -ForegroundColor $Script:Colors.Cyan
    Write-Host "        GameCore 部署腳本" -ForegroundColor $Script:Colors.Cyan
    Write-Host "========================================" -ForegroundColor $Script:Colors.Cyan
}

# 檢查必要工具
function Test-Prerequisites {
    Write-LogInfo "檢查必要工具..."
    
    $tools = @()
    
    switch ($DeploymentType) {
        "Docker" { $tools += @("docker", "docker-compose") }
        "Kubernetes" { $tools += @("kubectl", "helm") }
        "Azure" { $tools += @("az", "docker") }
        "Local" { $tools += @("dotnet") }
    }
    
    # 通用工具
    $tools += @("git", "curl")
    
    foreach ($tool in $tools) {
        try {
            $null = Get-Command $tool -ErrorAction Stop
            Write-LogSuccess "✓ $tool 已安裝"
        }
        catch {
            Write-LogError "✗ $tool 未安裝或不在 PATH 中"
            throw "缺少必要工具: $tool"
        }
    }
}

# 獲取版本資訊
function Get-DeploymentVersion {
    if ([string]::IsNullOrEmpty($Version)) {
        try {
            # 嘗試從 Git 標籤獲取版本
            if (Test-Path ".git") {
                $gitVersion = git describe --tags --abbrev=0 2>$null
                if ($LASTEXITCODE -eq 0 -and ![string]::IsNullOrEmpty($gitVersion)) {
                    $Script:Version = $gitVersion
                } else {
                    $Script:Version = "latest"
                }
            } else {
                $Script:Version = "latest"
            }
        }
        catch {
            $Script:Version = "latest"
        }
    } else {
        $Script:Version = $Version
    }
    
    Write-LogInfo "部署版本: $Script:Version"
}

# 執行測試
function Invoke-Tests {
    if (-not $NoTests) {
        Write-LogInfo "執行測試套件..."
        
        try {
            dotnet test --configuration Release --verbosity minimal
            if ($LASTEXITCODE -ne 0) {
                throw "測試失敗"
            }
            Write-LogSuccess "測試通過！"
        }
        catch {
            Write-LogError "測試失敗！"
            throw
        }
    } else {
        Write-LogWarn "跳過測試"
    }
}

# 建置 Docker 映像
function Build-DockerImage {
    if (-not $NoBuild -and $DeploymentType -in @("Docker", "Kubernetes", "Azure")) {
        Write-LogInfo "建置 Docker 映像..."
        
        $imageName = "gamecore:$Script:Version"
        $dockerfile = "Dockerfile"
        
        # 根據環境選擇 Dockerfile
        if ($Environment -eq "Development") {
            $dockerfile = "Dockerfile.advanced"
        }
        
        try {
            docker build -t $imageName -f $dockerfile .
            if ($LASTEXITCODE -ne 0) {
                throw "Docker 映像建置失敗"
            }
            Write-LogSuccess "Docker 映像建置完成: $imageName"
        }
        catch {
            Write-LogError "Docker 映像建置失敗！"
            throw
        }
    } else {
        Write-LogInfo "跳過 Docker 映像建置"
    }
}

# 備份資料庫
function Backup-Database {
    if ($BackupDatabase) {
        Write-LogInfo "執行資料庫備份..."
        
        $backupScript = "Tools/DataMigrationTool.ps1"
        if (Test-Path $backupScript) {
            try {
                & $backupScript -Action Backup
                Write-LogSuccess "資料庫備份完成"
            }
            catch {
                Write-LogWarn "資料庫備份失敗: $($_.Exception.Message)"
            }
        } else {
            Write-LogWarn "找不到備份腳本，跳過備份"
        }
    }
}

# Docker 部署
function Deploy-Docker {
    Write-LogInfo "執行 Docker 部署..."
    
    $composeFile = "docker-compose.yml"
    $overrideFile = ""
    
    # 根據環境選擇配置檔案
    switch ($Environment) {
        "Development" { $overrideFile = "docker-compose.override.yml" }
        "Staging" { $overrideFile = "docker-compose.staging.yml" }
        "Production" { $overrideFile = "docker-compose.prod.yml" }
    }
    
    # 構建 Docker Compose 命令
    $composeArgs = @("-f", $composeFile)
    if ((Test-Path $overrideFile)) {
        $composeArgs += @("-f", $overrideFile)
    }
    
    try {
        # 停止現有服務
        Write-LogInfo "停止現有服務..."
        docker-compose @composeArgs down 2>$null
        
        # 啟動服務
        Write-LogInfo "啟動服務..."
        docker-compose @composeArgs up -d
        if ($LASTEXITCODE -ne 0) {
            throw "Docker Compose 啟動失敗"
        }
        
        # 等待服務啟動
        Write-LogInfo "等待服務啟動..."
        Start-Sleep -Seconds 30
        
        # 健康檢查
        $healthUrl = "http://localhost:5000/health"
        try {
            $response = Invoke-WebRequest -Uri $healthUrl -UseBasicParsing -TimeoutSec 10
            if ($response.StatusCode -eq 200) {
                Write-LogSuccess "服務健康檢查通過"
            } else {
                throw "健康檢查返回狀態碼: $($response.StatusCode)"
            }
        }
        catch {
            Write-LogError "服務健康檢查失敗！"
            docker-compose @composeArgs logs
            throw
        }
        
        Write-LogSuccess "Docker 部署完成！"
    }
    catch {
        Write-LogError "Docker 部署失敗: $($_.Exception.Message)"
        throw
    }
}

# Kubernetes 部署
function Deploy-Kubernetes {
    Write-LogInfo "執行 Kubernetes 部署..."
    
    $k8sDir = "kubernetes"
    
    if (-not (Test-Path $k8sDir)) {
        Write-LogError "找不到 Kubernetes 配置目錄: $k8sDir"
        throw "Kubernetes 配置目錄不存在"
    }
    
    try {
        # 更新映像版本
        Write-LogInfo "更新 Kubernetes 配置中的映像版本..."
        $deploymentFile = "$k8sDir/gamecore-deployment.yaml"
        if (Test-Path $deploymentFile) {
            $content = Get-Content $deploymentFile -Raw
            $content = $content -replace "ghcr\.io/your-org/gamecore:.*", "ghcr.io/your-org/gamecore:$Script:Version"
            Set-Content $deploymentFile -Value $content
        }
        
        # 套用配置
        Write-LogInfo "套用 Kubernetes 配置..."
        kubectl apply -f $k8sDir/
        if ($LASTEXITCODE -ne 0) {
            throw "kubectl apply 失敗"
        }
        
        # 等待部署完成
        Write-LogInfo "等待部署完成..."
        kubectl rollout status deployment/gamecore-web -n gamecore --timeout=300s
        if ($LASTEXITCODE -ne 0) {
            throw "部署超時"
        }
        
        Write-LogSuccess "Kubernetes 部署完成！"
    }
    catch {
        Write-LogError "Kubernetes 部署失敗: $($_.Exception.Message)"
        throw
    }
}

# Azure 部署
function Deploy-Azure {
    Write-LogInfo "執行 Azure 部署..."
    
    $resourceGroup = "gamecore-$($Environment.ToLower())-rg"
    $appName = "gamecore-$($Environment.ToLower())"
    
    try {
        # 檢查 Azure 登入狀態
        $null = az account show 2>$null
        if ($LASTEXITCODE -ne 0) {
            Write-LogError "請先登入 Azure: az login"
            throw "Azure 未登入"
        }
        
        # 推送映像到 Azure Container Registry
        $acrName = "gamecoreacr"
        $imageName = "$acrName.azurecr.io/gamecore:$Script:Version"
        
        Write-LogInfo "推送映像到 Azure Container Registry..."
        docker tag "gamecore:$Script:Version" $imageName
        az acr login --name $acrName
        docker push $imageName
        
        if ($LASTEXITCODE -ne 0) {
            throw "映像推送失敗"
        }
        
        # 更新 App Service
        Write-LogInfo "更新 Azure App Service..."
        az webapp config container set `
            --resource-group $resourceGroup `
            --name $appName `
            --docker-custom-image-name $imageName
        
        if ($LASTEXITCODE -ne 0) {
            throw "App Service 更新失敗"
        }
        
        # 重啟應用程式
        Write-LogInfo "重啟應用程式..."
        az webapp restart --resource-group $resourceGroup --name $appName
        
        if ($LASTEXITCODE -ne 0) {
            throw "應用程式重啟失敗"
        }
        
        Write-LogSuccess "Azure 部署完成！"
    }
    catch {
        Write-LogError "Azure 部署失敗: $($_.Exception.Message)"
        throw
    }
}

# 本地部署
function Deploy-Local {
    Write-LogInfo "執行本地部署..."
    
    try {
        # 停止現有程序
        Write-LogInfo "停止現有程序..."
        Get-Process -Name "GameCore.Web" -ErrorAction SilentlyContinue | Stop-Process -Force
        
        # 建置應用程式
        Write-LogInfo "建置應用程式..."
        dotnet build --configuration Release
        if ($LASTEXITCODE -ne 0) {
            throw "建置失敗"
        }
        
        # 發布應用程式
        Write-LogInfo "發布應用程式..."
        $publishDir = "./publish"
        if (Test-Path $publishDir) {
            Remove-Item $publishDir -Recurse -Force
        }
        
        dotnet publish GameCore.Web/GameCore.Web.csproj `
            --configuration Release `
            --output $publishDir `
            --self-contained false
        
        if ($LASTEXITCODE -ne 0) {
            throw "發布失敗"
        }
        
        # 啟動應用程式
        Write-LogInfo "啟動應用程式..."
        $env:ASPNETCORE_ENVIRONMENT = $Environment
        
        Push-Location $publishDir
        try {
            Start-Process -FilePath "dotnet" -ArgumentList "GameCore.Web.dll" -NoNewWindow
            Pop-Location
            
            # 等待啟動
            Start-Sleep -Seconds 10
            
            # 健康檢查
            $healthUrl = "http://localhost:5000/health"
            $response = Invoke-WebRequest -Uri $healthUrl -UseBasicParsing -TimeoutSec 10
            if ($response.StatusCode -eq 200) {
                Write-LogSuccess "本地部署完成！"
            } else {
                throw "健康檢查失敗"
            }
        }
        catch {
            Pop-Location
            throw
        }
    }
    catch {
        Write-LogError "本地部署失敗: $($_.Exception.Message)"
        throw
    }
}

# 部署後驗證
function Test-DeploymentVerification {
    Write-LogInfo "執行部署後驗證..."
    
    $baseUrl = ""
    switch ($DeploymentType) {
        "Docker" { $baseUrl = "http://localhost:5000" }
        "Local" { $baseUrl = "http://localhost:5000" }
        "Kubernetes" { 
            try {
                $host = kubectl get ingress gamecore-ingress -n gamecore -o jsonpath='{.spec.rules[0].host}' 2>$null
                $baseUrl = if ($host) { "https://$host" } else { "http://localhost" }
            }
            catch {
                $baseUrl = "http://localhost"
            }
        }
        "Azure" { $baseUrl = "https://gamecore-$($Environment.ToLower()).azurewebsites.net" }
    }
    
    # 健康檢查
    Write-LogInfo "檢查應用程式健康狀態..."
    $maxRetries = 5
    for ($i = 1; $i -le $maxRetries; $i++) {
        try {
            $response = Invoke-WebRequest -Uri "$baseUrl/health" -UseBasicParsing -TimeoutSec 10
            if ($response.StatusCode -eq 200) {
                Write-LogSuccess "應用程式運行正常！"
                break
            }
        }
        catch {
            if ($i -eq $maxRetries) {
                Write-LogError "應用程式健康檢查失敗！"
                throw "健康檢查失敗"
            } else {
                Write-LogWarn "健康檢查失敗，重試 $i/$maxRetries..."
                Start-Sleep -Seconds 10
            }
        }
    }
    
    # 基本功能測試
    Write-LogInfo "執行基本功能測試..."
    try {
        $response = Invoke-WebRequest -Uri $baseUrl -UseBasicParsing -TimeoutSec 10
        if ($response.StatusCode -eq 200) {
            Write-LogSuccess "首頁存取正常"
        }
    }
    catch {
        Write-LogWarn "首頁存取失敗: $($_.Exception.Message)"
    }
    
    Write-LogSuccess "部署驗證完成！"
    Write-LogInfo "應用程式 URL: $baseUrl"
}

# 顯示部署摘要
function Show-DeploymentSummary {
    Write-Host ""
    Write-Host "========================================" -ForegroundColor $Script:Colors.Cyan
    Write-Host "           部署完成摘要" -ForegroundColor $Script:Colors.Cyan
    Write-Host "========================================" -ForegroundColor $Script:Colors.Cyan
    Write-Host "環境: " -NoNewline; Write-Host $Environment -ForegroundColor $Script:Colors.Green
    Write-Host "部署類型: " -NoNewline; Write-Host $DeploymentType -ForegroundColor $Script:Colors.Green
    Write-Host "版本: " -NoNewline; Write-Host $Script:Version -ForegroundColor $Script:Colors.Green
    Write-Host "時間: " -NoNewline; Write-Host (Get-Date) -ForegroundColor $Script:Colors.Green
    Write-Host "========================================" -ForegroundColor $Script:Colors.Cyan
    Write-Host ""
}

# 主函數
function Main {
    try {
        Show-Banner
        
        Write-LogInfo "部署參數:"
        Write-LogInfo "  環境: $Environment"
        Write-LogInfo "  類型: $DeploymentType"
        Write-LogInfo "  跳過建置: $NoBuild"
        Write-LogInfo "  跳過測試: $NoTests"
        Write-LogInfo "  備份資料庫: $BackupDatabase"
        
        # 檢查工具
        Test-Prerequisites
        
        # 獲取版本
        Get-DeploymentVersion
        
        # 確認部署
        if (-not $SkipConfirm) {
            Write-Host ""
            Write-Host "即將部署 GameCore:" -ForegroundColor $Script:Colors.Yellow
            Write-Host "  環境: " -NoNewline; Write-Host $Environment -ForegroundColor $Script:Colors.Green
            Write-Host "  類型: " -NoNewline; Write-Host $DeploymentType -ForegroundColor $Script:Colors.Green
            Write-Host "  版本: " -NoNewline; Write-Host $Script:Version -ForegroundColor $Script:Colors.Green
            Write-Host ""
            $confirm = Read-Host "確定要繼續嗎？ (y/N)"
            if ($confirm -ne 'y' -and $confirm -ne 'Y') {
                Write-LogInfo "部署已取消"
                return
            }
        }
        
        # 執行部署流程
        Write-LogInfo "開始部署流程..."
        
        # 1. 備份資料庫
        Backup-Database
        
        # 2. 執行測試
        Invoke-Tests
        
        # 3. 建置映像
        Build-DockerImage
        
        # 4. 執行部署
        switch ($DeploymentType) {
            "Docker" { Deploy-Docker }
            "Kubernetes" { Deploy-Kubernetes }
            "Azure" { Deploy-Azure }
            "Local" { Deploy-Local }
        }
        
        # 5. 部署後驗證
        Test-DeploymentVerification
        
        # 6. 顯示摘要
        Show-DeploymentSummary
        
        Write-LogSuccess "🎉 部署成功完成！"
    }
    catch {
        Write-LogError "部署過程中發生錯誤: $($_.Exception.Message)"
        if ($Verbose) {
            Write-LogError "詳細錯誤: $($_.Exception.StackTrace)"
        }
        exit 1
    }
}

# 執行主函數
Main
