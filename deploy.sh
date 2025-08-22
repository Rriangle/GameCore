#!/bin/bash

# GameCore 部署腳本
# 支援本地開發、Docker 和雲端部署

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

# 檢查命令是否存在
check_command() {
    if ! command -v $1 &> /dev/null; then
        log_error "$1 未安裝，請先安裝 $1"
        exit 1
    fi
}

# 檢查 .NET 環境
check_dotnet() {
    log_info "檢查 .NET 環境..."
    
    if ! command -v dotnet &> /dev/null; then
        log_error ".NET 8.0 SDK 未安裝，請先安裝 .NET 8.0 SDK"
        exit 1
    fi
    
    local version=$(dotnet --version)
    log_info "檢測到 .NET 版本: $version"
    
    if [[ ! $version =~ ^8\. ]]; then
        log_warning "建議使用 .NET 8.0 版本"
    fi
}

# 檢查 Docker 環境
check_docker() {
    log_info "檢查 Docker 環境..."
    
    if ! command -v docker &> /dev/null; then
        log_error "Docker 未安裝，請先安裝 Docker"
        exit 1
    fi
    
    if ! command -v docker-compose &> /dev/null; then
        log_error "Docker Compose 未安裝，請先安裝 Docker Compose"
        exit 1
    fi
    
    log_success "Docker 環境檢查通過"
}

# 還原 NuGet 套件
restore_packages() {
    log_info "還原 NuGet 套件..."
    dotnet restore
    log_success "套件還原完成"
}

# 建置專案
build_project() {
    log_info "建置專案..."
    dotnet build --configuration Release --no-restore
    log_success "專案建置完成"
}

# 執行測試
run_tests() {
    log_info "執行測試..."
    dotnet test --configuration Release --no-build --verbosity normal
    log_success "測試執行完成"
}

# 發佈專案
publish_project() {
    local output_dir="./publish"
    
    log_info "發佈專案到 $output_dir..."
    dotnet publish GameCore.Web --configuration Release --output $output_dir --no-build
    
    log_success "專案發佈完成"
}

# 本地開發部署
deploy_local() {
    log_info "開始本地開發部署..."
    
    check_dotnet
    restore_packages
    build_project
    run_tests
    
    log_info "啟動本地開發伺服器..."
    log_info "應用程式將在 https://localhost:5001 啟動"
    log_info "按 Ctrl+C 停止伺服器"
    
    dotnet run --project GameCore.Web --configuration Release
}

# Docker 部署
deploy_docker() {
    log_info "開始 Docker 部署..."
    
    check_docker
    
    # 停止現有容器
    log_info "停止現有容器..."
    docker-compose down --remove-orphans
    
    # 建立並啟動服務
    log_info "建立並啟動 Docker 服務..."
    docker-compose up --build -d
    
    # 等待服務啟動
    log_info "等待服務啟動..."
    sleep 30
    
    # 檢查服務狀態
    log_info "檢查服務狀態..."
    docker-compose ps
    
    log_success "Docker 部署完成！"
    log_info "應用程式網址: http://localhost:8080"
    log_info "資料庫連接: localhost:1433"
    log_info "使用 'docker-compose logs -f' 查看日誌"
}

# 生產環境部署
deploy_production() {
    log_info "開始生產環境部署..."
    
    check_dotnet
    restore_packages
    build_project
    run_tests
    publish_project
    
    log_info "生產環境部署完成！"
    log_info "發佈檔案位於: ./publish"
    log_info "請將發佈檔案部署到您的 Web 伺服器"
}

# 清理部署檔案
cleanup() {
    log_info "清理部署檔案..."
    
    # 清理建置輸出
    dotnet clean
    
    # 清理發佈目錄
    if [ -d "./publish" ]; then
        rm -rf ./publish
        log_info "已清理發佈目錄"
    fi
    
    # 清理 Docker 資源
    if command -v docker &> /dev/null; then
        docker system prune -f
        log_info "已清理 Docker 資源"
    fi
    
    log_success "清理完成"
}

# 顯示幫助資訊
show_help() {
    echo "GameCore 部署腳本"
    echo ""
    echo "用法: $0 [選項]"
    echo ""
    echo "選項:"
    echo "  local       本地開發部署"
    echo "  docker      Docker 容器部署"
    echo "  production  生產環境部署"
    echo "  clean       清理部署檔案"
    echo "  help        顯示此幫助資訊"
    echo ""
    echo "範例:"
    echo "  $0 local        # 本地開發"
    echo "  $0 docker      # Docker 部署"
    echo "  $0 production  # 生產環境"
    echo "  $0 clean       # 清理檔案"
}

# 主函數
main() {
    case "${1:-help}" in
        "local")
            deploy_local
            ;;
        "docker")
            deploy_docker
            ;;
        "production")
            deploy_production
            ;;
        "clean")
            cleanup
            ;;
        "help"|*)
            show_help
            ;;
    esac
}

# 執行主函數
main "$@"