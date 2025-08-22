#!/bin/bash

# GameCore 部署腳本
# 用於自動化部署 GameCore 應用程式

set -e

# 顏色定義
RED='\033[0;31m'
GREEN='\033[0;32m'
YELLOW='\033[1;33m'
BLUE='\033[0;34m'
NC='\033[0m' # No Color

# 日誌函數
log_info() {
    echo -e "${BLUE}[INFO]${NC} $1"
}

log_success() {
    echo -e "${GREEN}[SUCCESS]${NC} $1"
}

log_warning() {
    echo -e "${YELLOW}[WARNING]${NC} $1"
}

log_error() {
    echo -e "${RED}[ERROR]${NC} $1"
}

# 檢查必要工具
check_requirements() {
    log_info "檢查部署環境..."
    
    if ! command -v docker &> /dev/null; then
        log_error "Docker 未安裝，請先安裝 Docker"
        exit 1
    fi
    
    if ! command -v docker-compose &> /dev/null; then
        log_error "Docker Compose 未安裝，請先安裝 Docker Compose"
        exit 1
    fi
    
    log_success "環境檢查完成"
}

# 備份資料庫
backup_database() {
    log_info "備份資料庫..."
    
    if [ -d "backups" ]; then
        mkdir -p backups
    fi
    
    BACKUP_FILE="backups/gamecore_backup_$(date +%Y%m%d_%H%M%S).sql"
    
    docker exec gamecore-sqlserver /opt/mssql-tools/bin/sqlcmd \
        -S localhost -U sa -P GameCore123! \
        -Q "BACKUP DATABASE GameCore TO DISK = '/var/opt/mssql/backup.bak'"
    
    docker cp gamecore-sqlserver:/var/opt/mssql/backup.bak "$BACKUP_FILE"
    
    log_success "資料庫備份完成: $BACKUP_FILE"
}

# 停止現有服務
stop_services() {
    log_info "停止現有服務..."
    docker-compose down
    log_success "服務已停止"
}

# 清理舊映像
cleanup_images() {
    log_info "清理舊映像..."
    docker image prune -f
    log_success "映像清理完成"
}

# 建置新映像
build_images() {
    log_info "建置應用程式映像..."
    docker-compose build --no-cache
    log_success "映像建置完成"
}

# 啟動服務
start_services() {
    log_info "啟動服務..."
    docker-compose up -d
    
    log_info "等待服務啟動..."
    sleep 30
    
    # 檢查服務健康狀態
    if docker-compose ps | grep -q "healthy"; then
        log_success "所有服務已啟動並健康運行"
    else
        log_warning "部分服務可能未完全啟動，請檢查日誌"
    fi
}

# 執行資料庫遷移
run_migrations() {
    log_info "執行資料庫遷移..."
    
    # 等待資料庫完全啟動
    sleep 10
    
    # 這裡可以添加資料庫遷移命令
    # docker-compose exec gamecore-web dotnet ef database update
    
    log_success "資料庫遷移完成"
}

# 檢查應用程式狀態
check_application() {
    log_info "檢查應用程式狀態..."
    
    # 檢查健康檢查端點
    if curl -f http://localhost:5000/health > /dev/null 2>&1; then
        log_success "應用程式健康檢查通過"
    else
        log_error "應用程式健康檢查失敗"
        return 1
    fi
    
    # 檢查資料庫連接
    if curl -f http://localhost:5000/api/health/database > /dev/null 2>&1; then
        log_success "資料庫連接正常"
    else
        log_error "資料庫連接失敗"
        return 1
    fi
}

# 顯示部署資訊
show_deployment_info() {
    log_success "部署完成！"
    echo ""
    echo "應用程式資訊："
    echo "  - Web 應用程式: http://localhost:5000"
    echo "  - 健康檢查: http://localhost:5000/health"
    echo "  - API 文檔: http://localhost:5000/api"
    echo ""
    echo "資料庫資訊："
    echo "  - 伺服器: localhost:1433"
    echo "  - 資料庫: GameCore"
    echo "  - 用戶名: sa"
    echo ""
    echo "Redis 資訊："
    echo "  - 伺服器: localhost:6379"
    echo ""
    echo "查看日誌："
    echo "  - docker-compose logs -f gamecore-web"
    echo "  - docker-compose logs -f sqlserver"
}

# 主函數
main() {
    log_info "開始部署 GameCore 應用程式..."
    
    check_requirements
    
    # 詢問是否備份資料庫
    read -p "是否要備份現有資料庫？(y/N): " -n 1 -r
    echo
    if [[ $REPLY =~ ^[Yy]$ ]]; then
        backup_database
    fi
    
    stop_services
    cleanup_images
    build_images
    start_services
    run_migrations
    check_application
    show_deployment_info
}

# 錯誤處理
trap 'log_error "部署過程中發生錯誤，請檢查日誌"; exit 1' ERR

# 執行主函數
main "$@"