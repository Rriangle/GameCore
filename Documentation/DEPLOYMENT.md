# GameCore 部署指南

本文檔提供 GameCore 專案的完整部署指南，涵蓋開發、測試、生產環境的部署流程。

## 系統需求

### 最低系統需求
- **作業系統**: Windows 10/11, Ubuntu 20.04+, macOS 12+
- **CPU**: 4核心 2.0GHz 以上
- **記憶體**: 8GB RAM (生產環境建議16GB+)
- **儲存空間**: 50GB 可用空間
- **網路**: 穩定的網路連線

### 軟體依賴
- **.NET 8.0 SDK**
- **SQL Server 2019+** 或 **SQL Server Express**
- **Node.js 20.x** (前端建置)
- **Docker Desktop** (容器部署)
- **Git** (版本控制)

## 開發環境設置

### 1. 克隆專案
```bash
git clone https://github.com/your-org/GameCore.git
cd GameCore
```

### 2. 安裝依賴
```bash
# 還原 .NET 套件
dotnet restore

# 安裝前端依賴 (如果有)
npm install
```

### 3. 資料庫設置
```bash
# 建立資料庫
dotnet ef database update --project GameCore.Infrastructure --startup-project GameCore.Web

# 插入測試資料
dotnet run --project GameCore.Web -- --seed-data
```

### 4. 配置設定
複製 `appsettings.example.json` 為 `appsettings.Development.json`：
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=GameCore_Dev;Trusted_Connection=true;MultipleActiveResultSets=true"
  },
  "Authentication": {
    "Google": {
      "ClientId": "your-google-client-id",
      "ClientSecret": "your-google-client-secret"
    },
    "Facebook": {
      "AppId": "your-facebook-app-id",
      "AppSecret": "your-facebook-app-secret"
    }
  }
}
```

### 5. 啟動應用程式
```bash
dotnet run --project GameCore.Web
```

應用程式將在 `https://localhost:5001` 啟動。

## 測試環境部署

### 使用 Docker Compose
```bash
# 建置並啟動所有服務
docker-compose -f docker-compose.staging.yml up -d

# 查看服務狀態
docker-compose ps

# 查看日誌
docker-compose logs -f gamecore-web
```

### 環境變數配置
建立 `.env.staging` 檔案：
```env
ASPNETCORE_ENVIRONMENT=Staging
CONNECTION_STRING=Server=sql-server;Database=GameCore_Staging;User Id=sa;Password=YourPassword123;TrustServerCertificate=true
GOOGLE_CLIENT_ID=your-staging-google-client-id
GOOGLE_CLIENT_SECRET=your-staging-google-client-secret
```

## 生產環境部署

### Azure App Service 部署

1. **建立 Azure 資源**
```bash
# 登入 Azure
az login

# 建立資源群組
az group create --name GameCore-Production --location "East Asia"

# 建立 App Service 計劃
az appservice plan create --name GameCore-Plan --resource-group GameCore-Production --sku P1V3 --is-linux

# 建立 Web App
az webapp create --resource-group GameCore-Production --plan GameCore-Plan --name gamecore-app --runtime "DOTNETCORE:8.0"
```

2. **設定應用程式設定**
```bash
az webapp config appsettings set --resource-group GameCore-Production --name gamecore-app --settings \
  ASPNETCORE_ENVIRONMENT=Production \
  ConnectionStrings__DefaultConnection="your-production-connection-string"
```

### Kubernetes 部署

使用提供的 Helm Chart：
```bash
# 安裝 Helm Chart
helm install gamecore-production ./helm/gamecore \
  --namespace production \
  --create-namespace \
  --set image.tag=latest \
  --set environment=production \
  --set ingress.hosts[0].host=gamecore.com \
  --set replicaCount=3
```

## 監控與維護

### 健康檢查端點
- **基本健康檢查**: `/health`
- **詳細健康檢查**: `/health/detailed`
- **就緒檢查**: `/health/ready`
- **存活檢查**: `/health/live`

### 日誌管理
日誌檔案位置：
- **開發環境**: `logs/gamecore-{date}.log`
- **生產環境**: Azure Application Insights 或 ELK Stack

### 效能監控
- **Application Insights**: 應用程式效能監控
- **Prometheus + Grafana**: 系統指標監控
- **SignalR 連線監控**: 即時通訊狀態

## 故障排除

### 常見問題

1. **資料庫連線失敗**
   - 檢查連接字串設定
   - 確認 SQL Server 服務運行
   - 驗證防火牆設定

2. **OAuth 登入失敗**
   - 檢查 Client ID 和 Secret
   - 確認回調 URL 設定
   - 驗證 SSL 憑證

3. **效能問題**
   - 檢查資料庫查詢效能
   - 監控記憶體使用量
   - 分析 SignalR 連線數

### 支援聯絡
如遇到部署問題，請聯絡開發團隊或查看專案 Wiki。 