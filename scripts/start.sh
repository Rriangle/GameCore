#!/bin/bash

# GameCore 專案啟動腳本
# 適用於 Linux/macOS

echo "🚀 啟動 GameCore 專案..."

# 檢查 .NET SDK
if ! command -v dotnet &> /dev/null; then
    echo "❌ 錯誤: 未找到 .NET SDK"
    echo "請先安裝 .NET 8.0 SDK: https://dotnet.microsoft.com/download/dotnet/8.0"
    exit 1
fi

echo "✅ .NET SDK 版本: $(dotnet --version)"

# 檢查 Docker
if ! command -v docker &> /dev/null; then
    echo "❌ 錯誤: 未找到 Docker"
    echo "請先安裝 Docker: https://docs.docker.com/get-docker/"
    exit 1
fi

echo "✅ Docker 版本: $(docker --version)"

# 檢查 Docker Compose
if ! command -v docker-compose &> /dev/null; then
    echo "❌ 錯誤: 未找到 Docker Compose"
    echo "請先安裝 Docker Compose: https://docs.docker.com/compose/install/"
    exit 1
fi

echo "✅ Docker Compose 版本: $(docker-compose --version)"

# 還原 NuGet 套件
echo "📦 還原 NuGet 套件..."
dotnet restore

# 建置專案
echo "🔨 建置專案..."
dotnet build --configuration Release

# 執行測試
echo "🧪 執行測試..."
dotnet test --configuration Release --no-build

# 啟動 Docker 服務
echo "🐳 啟動 Docker 服務..."
docker-compose up -d

# 等待資料庫啟動
echo "⏳ 等待資料庫啟動..."
sleep 30

# 檢查服務狀態
echo "📊 檢查服務狀態..."
docker-compose ps

echo "🎉 GameCore 專案啟動完成！"
echo "🌐 Web 應用程式: http://localhost:5000"
echo "🔌 資料庫: localhost:1433"
echo "📝 預設管理員帳號: admin / admin123"

echo ""
echo "📋 常用命令:"
echo "  停止服務: docker-compose down"
echo "  查看日誌: docker-compose logs -f"
echo "  重新建置: docker-compose up --build"
echo "  清理資料: docker-compose down -v"