# GameCore 部署指南

本文件提供 GameCore 專案的完整部署指南，涵蓋開發、測試、預生產和生產環境的部署流程。

## 📋 目錄

- [環境需求](#環境需求)
- [快速開始](#快速開始)
- [部署方式](#部署方式)
- [環境配置](#環境配置)
- [持續集成/持續部署](#持續集成持續部署)
- [監控和維護](#監控和維護)
- [故障排除](#故障排除)

## 🔧 環境需求

### 基本需求

- **.NET 8.0 SDK** - 應用程式執行環境
- **SQL Server 2022** - 主要資料庫
- **Redis 7.0+** - 快取和 Session 儲存
- **Node.js 18+** - 前端建置工具

### 開發環境額外需求

- **Visual Studio 2022** 或 **VS Code**
- **Docker Desktop** - 容器化開發
- **Git** - 版本控制
- **PowerShell 7+** - 部署腳本執行

### 生產環境需求

- **Azure App Service** 或 **Kubernetes 叢集**
- **Azure SQL Database** 或 **SQL Server 2022**
- **Azure Redis Cache** 或 **Redis 叢集**
- **Azure CDN** - 靜態資源加速

## 🚀 快速開始

### 1. 本地開發環境設定

```bash
# 克隆專案
git clone https://github.com/your-org/gamecore.git
cd gamecore

# 還原依賴項目
dotnet restore

# 設定資料庫
cd Tools
pwsh DataMigrationTool.ps1 -Action Init
pwsh DataMigrationTool.ps1 -Action SeedBasic

# 啟動開發環境
cd ..
dotnet run --project GameCore.Web
```

### 2. Docker 快速啟動

```bash
# 啟動完整開發環境
docker-compose up -d

# 檢查服務狀態
docker-compose ps

# 查看日誌
docker-compose logs -f gamecore-web
```

### 3. 使用部署腳本

```powershell
# 開發環境部署
.\scripts\Deploy-GameCore.ps1 -Environment Development -DeploymentType Docker

# 生產環境部署
.\scripts\Deploy-GameCore.ps1 -Environment Production -DeploymentType Azure -BackupDatabase
```

## 🎯 部署方式

### 1. Docker 容器化部署

適用於開發和測試環境的快速部署。

#### 開發環境

```yaml
# docker-compose.yml
version: '3.8'
services:
  gamecore-web:
    build: .
    ports:
      - "5000:80"
    environment:
      - ASPNETCORE_ENVIRONMENT=Development
      - ConnectionStrings__DefaultConnection=Server=gamecore-db;Database=GameCore;...
    depends_on:
      - gamecore-db
      - gamecore-redis
```

#### 部署指令

```bash
# 建置和啟動
docker-compose up -d --build

# 更新服務
docker-compose pull
docker-compose up -d

# 停止服務
docker-compose down
```

### 2. Kubernetes 叢集部署

適用於大規模生產環境。

#### 部署步驟

```bash
# 1. 建立命名空間
kubectl create namespace gamecore

# 2. 設定機密資料
kubectl create secret generic gamecore-secrets \
  --from-literal=ConnectionStrings__DefaultConnection="..." \
  --from-literal=ApiKeys__GoogleOAuth="..." \
  -n gamecore

# 3. 部署應用程式
kubectl apply -f kubernetes/

# 4. 檢查部署狀態
kubectl get pods -n gamecore
kubectl rollout status deployment/gamecore-web -n gamecore
```

#### 自動擴展配置

```yaml
apiVersion: autoscaling/v2
kind: HorizontalPodAutoscaler
metadata:
  name: gamecore-web-hpa
spec:
  scaleTargetRef:
    apiVersion: apps/v1
    kind: Deployment
    name: gamecore-web
  minReplicas: 3
  maxReplicas: 10
  metrics:
  - type: Resource
    resource:
      name: cpu
      target:
        type: Utilization
        averageUtilization: 70
```

### 3. Azure App Service 部署

適用於中小型應用程式的雲端部署。

#### Azure CLI 部署

```bash
# 1. 登入 Azure
az login

# 2. 建立資源群組
az group create --name gamecore-prod-rg --location eastasia

# 3. 建立 App Service 方案
az appservice plan create \
  --name gamecore-prod-plan \
  --resource-group gamecore-prod-rg \
  --sku P1V2 \
  --is-linux

# 4. 建立 Web App
az webapp create \
  --name gamecore-prod \
  --resource-group gamecore-prod-rg \
  --plan gamecore-prod-plan \
  --deployment-container-image-name gamecore:latest

# 5. 配置應用程式設定
az webapp config appsettings set \
  --name gamecore-prod \
  --resource-group gamecore-prod-rg \
  --settings ASPNETCORE_ENVIRONMENT=Production
```

#### GitHub Actions 自動部署

```yaml
name: Deploy to Azure
on:
  push:
    branches: [main]

jobs:
  deploy:
    runs-on: ubuntu-latest
    steps:
    - uses: actions/checkout@v4
    
    - name: Login to Azure
      uses: azure/login@v1
      with:
        creds: ${{ secrets.AZURE_CREDENTIALS }}
    
    - name: Deploy to Azure Web App
      uses: azure/webapps-deploy@v2
      with:
        app-name: gamecore-prod
        images: ghcr.io/your-org/gamecore:${{ github.sha }}
```

## ⚙️ 環境配置

### 開發環境 (Development)

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\MSSQLLocalDB;Database=GameCore_Dev;Trusted_Connection=true;",
    "Redis": "localhost:6379"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Debug",
      "Microsoft": "Information"
    }
  },
  "AllowedHosts": "*",
  "EnableSwagger": true
}
```

### 測試環境 (Staging)

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=gamecore-staging-db.database.windows.net;Database=GameCore_Staging;User Id=admin;Password=***;",
    "Redis": "gamecore-staging-cache.redis.cache.windows.net:6380,password=***,ssl=True"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft": "Warning"
    }
  },
  "AllowedHosts": "gamecore-staging.azurewebsites.net"
}
```

### 生產環境 (Production)

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=gamecore-prod-db.database.windows.net;Database=GameCore;User Id=admin;Password=***;",
    "Redis": "gamecore-prod-cache.redis.cache.windows.net:6380,password=***,ssl=True"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Warning",
      "Microsoft": "Error"
    }
  },
  "AllowedHosts": "gamecore.com,www.gamecore.com"
}
```

## 🔄 持續集成/持續部署

### GitHub Actions 工作流程

我們的 CI/CD 管道包含以下階段：

1. **程式碼品質檢查**
   - 編譯檢查
   - 單元測試
   - 程式碼覆蓋率
   - SonarCloud 分析

2. **安全性掃描**
   - 依賴項目漏洞掃描
   - CodeQL 分析
   - Docker 映像安全掃描

3. **建置和部署**
   - Docker 映像建置
   - 推送到容器註冊表
   - 部署到目標環境

4. **部署後驗證**
   - 健康檢查
   - 冒煙測試
   - 效能測試

### 分支策略

- **main** - 生產環境，自動部署到 Production
- **develop** - 開發分支，自動部署到 Staging
- **feature/** - 功能分支，觸發測試但不部署
- **hotfix/** - 緊急修復，可直接合併到 main

### 部署觸發條件

```yaml
on:
  push:
    branches: [main, develop]
  pull_request:
    branches: [main]
  release:
    types: [published]
```

## 📊 監控和維護

### 應用程式監控

#### Health Checks

```csharp
// 健康檢查端點
builder.Services.AddHealthChecks()
    .AddSqlServer(connectionString)
    .AddRedis(redisConnectionString)
    .AddApplicationInsights();

app.MapHealthChecks("/health");
app.MapHealthChecks("/health/ready", new HealthCheckOptions
{
    Predicate = check => check.Tags.Contains("ready")
});
```

#### Application Insights

```json
{
  "ApplicationInsights": {
    "InstrumentationKey": "your-instrumentation-key"
  },
  "Logging": {
    "ApplicationInsights": {
      "LogLevel": {
        "Default": "Information"
      }
    }
  }
}
```

### 效能監控

#### Prometheus 指標

```csharp
// 自訂指標
services.AddMetrics()
    .AddPrometheusExporter();

// 中介軟體
app.UseMetrics();
app.MapPrometheusScrapingEndpoint();
```

#### Grafana 儀表板

- **應用程式效能**: 回應時間、輸送量、錯誤率
- **系統資源**: CPU、記憶體、磁碟使用率
- **資料庫效能**: 連線數、查詢時間、死鎖
- **使用者行為**: 活躍使用者數、頁面瀏覽量

### 日誌管理

#### Structured Logging

```csharp
logger.LogInformation("User {UserId} logged in from {IPAddress}", 
    userId, ipAddress);
```

#### ELK Stack

- **Elasticsearch**: 日誌儲存和搜尋
- **Logstash**: 日誌處理和轉換
- **Kibana**: 日誌視覺化和分析

## 🔧 故障排除

### 常見問題

#### 1. 應用程式啟動失敗

**症狀**: 容器啟動後立即退出

**解決方案**:
```bash
# 檢查容器日誌
docker logs gamecore-web

# 檢查配置
docker exec -it gamecore-web cat appsettings.json

# 檢查環境變數
docker exec -it gamecore-web env | grep ASPNETCORE
```

#### 2. 資料庫連線失敗

**症狀**: `SqlException: Cannot open database`

**解決方案**:
```powershell
# 檢查連線字串
$connectionString = "Server=...;Database=...;User Id=...;Password=..."
Test-SqlConnection -ConnectionString $connectionString

# 檢查防火牆規則
# 檢查資料庫伺服器狀態
```

#### 3. Redis 連線問題

**症狀**: `RedisConnectionException: No connection available`

**解決方案**:
```bash
# 檢查 Redis 服務狀態
redis-cli ping

# 檢查網路連線
telnet redis-host 6379

# 檢查認證
redis-cli -h redis-host -p 6379 -a password
```

#### 4. 效能問題

**症狀**: 回應時間緩慢

**診斷步驟**:
1. 檢查 Application Insights 效能資料
2. 分析 SQL 查詢效能
3. 檢查 Redis 快取命中率
4. 檢查系統資源使用率

### 緊急回滾程序

#### Docker 環境

```bash
# 1. 停止當前版本
docker-compose down

# 2. 切換到前一個版本
docker pull gamecore:previous-version
docker tag gamecore:previous-version gamecore:latest

# 3. 重新啟動
docker-compose up -d
```

#### Kubernetes 環境

```bash
# 1. 檢查部署歷史
kubectl rollout history deployment/gamecore-web -n gamecore

# 2. 回滾到前一個版本
kubectl rollout undo deployment/gamecore-web -n gamecore

# 3. 檢查回滾狀態
kubectl rollout status deployment/gamecore-web -n gamecore
```

#### Azure App Service

```bash
# 1. 列出部署歷史
az webapp deployment list --name gamecore-prod --resource-group gamecore-prod-rg

# 2. 回滾到特定版本
az webapp deployment source config-zip \
  --name gamecore-prod \
  --resource-group gamecore-prod-rg \
  --src previous-version.zip
```

### 維護作業

#### 定期備份

```powershell
# 每日資料庫備份
$backupScript = "Tools/DataMigrationTool.ps1"
& $backupScript -Action Backup

# 每週完整備份
& $backupScript -Action Backup -BackupType Full
```

#### 更新作業

```bash
# 1. 維護模式
curl -X POST https://gamecore.com/api/maintenance/enable

# 2. 部署更新
./scripts/deploy.sh -e production -t azure --backup-db

# 3. 驗證部署
curl -f https://gamecore.com/health

# 4. 關閉維護模式
curl -X POST https://gamecore.com/api/maintenance/disable
```

## 📞 支援和聯絡

- **技術支援**: tech-support@gamecore.com
- **DevOps 團隊**: devops@gamecore.com
- **緊急聯絡**: +886-XXX-XXXX

## 📚 相關文件

- [開發環境設定指南](DEVELOPMENT.md)
- [API 文件](API.md)
- [安全性指南](SECURITY.md)
- [效能調整指南](PERFORMANCE.md)

---

**最後更新**: 2025年1月
**文件版本**: 1.0.0
