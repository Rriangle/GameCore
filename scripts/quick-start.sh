#!/bin/bash

# GameCore 快速啟動腳本
# 用於快速啟動開發環境

set -e

# 顏色定義
GREEN='\033[0;32m'
BLUE='\033[0;34m'
YELLOW='\033[1;33m'
NC='\033[0m'

echo -e "${BLUE}🎮 GameCore 快速啟動腳本${NC}"
echo "=================================="

# 檢查 Docker 是否運行
if ! docker info > /dev/null 2>&1; then
    echo -e "${YELLOW}⚠️  Docker 未運行，請先啟動 Docker Desktop${NC}"
    exit 1
fi

echo -e "${GREEN}✅ Docker 正在運行${NC}"

# 檢查是否有現有的容器
if docker ps -a | grep -q "gamecore"; then
    echo -e "${YELLOW}發現現有的 GameCore 容器，正在停止...${NC}"
    docker-compose down
fi

# 啟動服務
echo -e "${BLUE}🚀 啟動 GameCore 服務...${NC}"
docker-compose up -d

echo -e "${BLUE}⏳ 等待服務啟動...${NC}"
sleep 30

# 檢查服務狀態
echo -e "${BLUE}🔍 檢查服務狀態...${NC}"

# 檢查 Web 應用程式
if curl -f http://localhost:5000/health > /dev/null 2>&1; then
    echo -e "${GREEN}✅ Web 應用程式運行正常${NC}"
else
    echo -e "${YELLOW}⚠️  Web 應用程式可能需要更多時間啟動${NC}"
fi

# 檢查資料庫
if curl -f http://localhost:5000/api/health/database > /dev/null 2>&1; then
    echo -e "${GREEN}✅ 資料庫連接正常${NC}"
else
    echo -e "${YELLOW}⚠️  資料庫可能需要更多時間啟動${NC}"
fi

echo ""
echo -e "${GREEN}🎉 GameCore 啟動完成！${NC}"
echo ""
echo "📱 訪問資訊："
echo "  - 主頁: http://localhost:5000"
echo "  - 健康檢查: http://localhost:5000/health"
echo "  - API 端點: http://localhost:5000/api"
echo ""
echo "🗄️ 資料庫資訊："
echo "  - 伺服器: localhost:1433"
echo "  - 資料庫: GameCore"
echo "  - 用戶名: sa"
echo "  - 密碼: GameCore123!"
echo ""
echo "📊 監控命令："
echo "  - 查看日誌: docker-compose logs -f"
echo "  - 停止服務: docker-compose down"
echo "  - 重啟服務: docker-compose restart"
echo ""
echo -e "${BLUE}💡 提示：首次啟動可能需要幾分鐘來初始化資料庫${NC}"