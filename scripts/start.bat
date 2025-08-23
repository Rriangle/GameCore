@echo off
chcp 65001 >nul

REM GameCore 專案啟動腳本 (Windows)
echo 🚀 啟動 GameCore 專案...

REM 檢查 .NET SDK
dotnet --version >nul 2>&1
if %errorlevel% neq 0 (
    echo ❌ 錯誤: 未找到 .NET SDK
    echo 請先安裝 .NET 8.0 SDK: https://dotnet.microsoft.com/download/dotnet/8.0
    pause
    exit /b 1
)

for /f "tokens=*" %%i in ('dotnet --version') do set DOTNET_VERSION=%%i
echo ✅ .NET SDK 版本: %DOTNET_VERSION%

REM 檢查 Docker
docker --version >nul 2>&1
if %errorlevel% neq 0 (
    echo ❌ 錯誤: 未找到 Docker
    echo 請先安裝 Docker: https://docs.docker.com/get-docker/
    pause
    exit /b 1
)

for /f "tokens=*" %%i in ('docker --version') do set DOCKER_VERSION=%%i
echo ✅ Docker 版本: %DOCKER_VERSION%

REM 檢查 Docker Compose
docker-compose --version >nul 2>&1
if %errorlevel% neq 0 (
    echo ❌ 錯誤: 未找到 Docker Compose
    echo 請先安裝 Docker Compose: https://docs.docker.com/compose/install/
    pause
    exit /b 1
)

for /f "tokens=*" %%i in ('docker-compose --version') do set COMPOSE_VERSION=%%i
echo ✅ Docker Compose 版本: %COMPOSE_VERSION%

REM 還原 NuGet 套件
echo 📦 還原 NuGet 套件...
dotnet restore

REM 建置專案
echo 🔨 建置專案...
dotnet build --configuration Release

REM 執行測試
echo 🧪 執行測試...
dotnet test --configuration Release --no-build

REM 啟動 Docker 服務
echo 🐳 啟動 Docker 服務...
docker-compose up -d

REM 等待資料庫啟動
echo ⏳ 等待資料庫啟動...
timeout /t 30 /nobreak >nul

REM 檢查服務狀態
echo 📊 檢查服務狀態...
docker-compose ps

echo 🎉 GameCore 專案啟動完成！
echo 🌐 Web 應用程式: http://localhost:5000
echo 🔌 資料庫: localhost:1433
echo 📝 預設管理員帳號: admin / admin123

echo.
echo 📋 常用命令:
echo   停止服務: docker-compose down
echo   查看日誌: docker-compose logs -f
echo   重新建置: docker-compose up --build
echo   清理資料: docker-compose down -v

pause