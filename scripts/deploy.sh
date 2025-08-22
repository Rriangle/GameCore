#!/bin/bash
# GameCore 部署腳本
# 支援多種部署環境和方式

set -e

# 顏色定義
RED='\033[0;31m'
GREEN='\033[0;32m'
YELLOW='\033[1;33m'
BLUE='\033[0;34m'
PURPLE='\033[0;35m'
CYAN='\033[0;36m'
NC='\033[0m' # No Color

# 預設值
ENVIRONMENT="development"
DEPLOYMENT_TYPE="docker"
BUILD_IMAGE=true
RUN_TESTS=true
BACKUP_DB=false
SKIP_CONFIRM=false
VERSION=""
CONFIG_FILE=""

# 日誌函數
log_info() {
    echo -e "${BLUE}[INFO]${NC} $1"
}

log_warn() {
    echo -e "${YELLOW}[WARN]${NC} $1"
}

log_error() {
    echo -e "${RED}[ERROR]${NC} $1"
}

log_success() {
    echo -e "${GREEN}[SUCCESS]${NC} $1"
}

log_debug() {
    echo -e "${PURPLE}[DEBUG]${NC} $1"
}

# 顯示幫助訊息
show_help() {
    echo -e "${CYAN}GameCore 部署腳本${NC}"
    echo ""
    echo "使用方法: $0 [選項]"
    echo ""
    echo "選項:"
    echo "  -e, --environment ENVIRONMENT  部署環境 (development/staging/production)"
    echo "  -t, --type TYPE                部署類型 (docker/kubernetes/azure/local)"
    echo "  -v, --version VERSION          部署版本標籤"
    echo "  -c, --config CONFIG_FILE       配置檔案路徑"
    echo "  --no-build                     跳過映像建置"
    echo "  --no-tests                     跳過測試"
    echo "  --backup-db                    部署前備份資料庫"
    echo "  --skip-confirm                 跳過確認提示"
    echo "  -h, --help                     顯示此幫助訊息"
    echo ""
    echo "範例:"
    echo "  $0 -e development -t docker"
    echo "  $0 -e production -t kubernetes -v v1.2.3 --backup-db"
    echo "  $0 -e staging -t azure --skip-confirm"
}

# 解析命令列參數
parse_arguments() {
    while [[ $# -gt 0 ]]; do
        case $1 in
            -e|--environment)
                ENVIRONMENT="$2"
                shift 2
                ;;
            -t|--type)
                DEPLOYMENT_TYPE="$2"
                shift 2
                ;;
            -v|--version)
                VERSION="$2"
                shift 2
                ;;
            -c|--config)
                CONFIG_FILE="$2"
                shift 2
                ;;
            --no-build)
                BUILD_IMAGE=false
                shift
                ;;
            --no-tests)
                RUN_TESTS=false
                shift
                ;;
            --backup-db)
                BACKUP_DB=true
                shift
                ;;
            --skip-confirm)
                SKIP_CONFIRM=true
                shift
                ;;
            -h|--help)
                show_help
                exit 0
                ;;
            *)
                log_error "未知參數: $1"
                show_help
                exit 1
                ;;
        esac
    done
}

# 驗證環境
validate_environment() {
    log_info "驗證部署環境..."
    
    case $ENVIRONMENT in
        development|staging|production)
            log_success "環境驗證通過: $ENVIRONMENT"
            ;;
        *)
            log_error "無效的環境: $ENVIRONMENT"
            log_error "支援的環境: development, staging, production"
            exit 1
            ;;
    esac
    
    case $DEPLOYMENT_TYPE in
        docker|kubernetes|azure|local)
            log_success "部署類型驗證通過: $DEPLOYMENT_TYPE"
            ;;
        *)
            log_error "無效的部署類型: $DEPLOYMENT_TYPE"
            log_error "支援的類型: docker, kubernetes, azure, local"
            exit 1
            ;;
    esac
}

# 檢查必要工具
check_prerequisites() {
    log_info "檢查必要工具..."
    
    local tools=()
    
    case $DEPLOYMENT_TYPE in
        docker)
            tools+=("docker" "docker-compose")
            ;;
        kubernetes)
            tools+=("kubectl" "helm")
            ;;
        azure)
            tools+=("az" "docker")
            ;;
        local)
            tools+=("dotnet")
            ;;
    esac
    
    # 通用工具
    tools+=("git" "curl")
    
    for tool in "${tools[@]}"; do
        if command -v "$tool" >/dev/null 2>&1; then
            log_success "✓ $tool 已安裝"
        else
            log_error "✗ $tool 未安裝"
            exit 1
        fi
    done
}

# 獲取版本資訊
get_version() {
    if [ -z "$VERSION" ]; then
        # 嘗試從 Git 標籤獲取版本
        if git rev-parse --git-dir > /dev/null 2>&1; then
            VERSION=$(git describe --tags --abbrev=0 2>/dev/null || echo "latest")
        else
            VERSION="latest"
        fi
    fi
    
    log_info "部署版本: $VERSION"
}

# 執行測試
run_tests() {
    if [ "$RUN_TESTS" = true ]; then
        log_info "執行測試套件..."
        
        # 執行單元測試
        if ! dotnet test --configuration Release --verbosity minimal; then
            log_error "測試失敗！"
            exit 1
        fi
        
        log_success "測試通過！"
    else
        log_warn "跳過測試"
    fi
}

# 建置 Docker 映像
build_docker_image() {
    if [ "$BUILD_IMAGE" = true ] && [[ "$DEPLOYMENT_TYPE" =~ ^(docker|kubernetes|azure)$ ]]; then
        log_info "建置 Docker 映像..."
        
        local image_name="gamecore:$VERSION"
        local dockerfile="Dockerfile"
        
        # 根據環境選擇 Dockerfile
        if [ "$ENVIRONMENT" = "development" ]; then
            dockerfile="Dockerfile.advanced"
        fi
        
        if ! docker build -t "$image_name" -f "$dockerfile" .; then
            log_error "Docker 映像建置失敗！"
            exit 1
        fi
        
        log_success "Docker 映像建置完成: $image_name"
    else
        log_info "跳過 Docker 映像建置"
    fi
}

# 備份資料庫
backup_database() {
    if [ "$BACKUP_DB" = true ]; then
        log_info "執行資料庫備份..."
        
        local backup_script="Tools/DataMigrationTool.ps1"
        if [ -f "$backup_script" ]; then
            pwsh "$backup_script" -Action Backup
            log_success "資料庫備份完成"
        else
            log_warn "找不到備份腳本，跳過備份"
        fi
    fi
}

# Docker 部署
deploy_docker() {
    log_info "執行 Docker 部署..."
    
    local compose_file="docker-compose.yml"
    local override_file=""
    
    # 根據環境選擇配置檔案
    case $ENVIRONMENT in
        development)
            override_file="docker-compose.override.yml"
            ;;
        staging)
            override_file="docker-compose.staging.yml"
            ;;
        production)
            override_file="docker-compose.prod.yml"
            ;;
    esac
    
    # 構建 Docker Compose 命令
    local compose_cmd="docker-compose -f $compose_file"
    if [ -f "$override_file" ]; then
        compose_cmd="$compose_cmd -f $override_file"
    fi
    
    # 停止現有服務
    log_info "停止現有服務..."
    $compose_cmd down || true
    
    # 啟動服務
    log_info "啟動服務..."
    if ! $compose_cmd up -d; then
        log_error "Docker 部署失敗！"
        exit 1
    fi
    
    # 等待服務啟動
    log_info "等待服務啟動..."
    sleep 30
    
    # 健康檢查
    if ! curl -f http://localhost:5000/health >/dev/null 2>&1; then
        log_error "服務健康檢查失敗！"
        $compose_cmd logs
        exit 1
    fi
    
    log_success "Docker 部署完成！"
}

# Kubernetes 部署
deploy_kubernetes() {
    log_info "執行 Kubernetes 部署..."
    
    local k8s_dir="kubernetes"
    
    # 檢查 Kubernetes 配置
    if [ ! -d "$k8s_dir" ]; then
        log_error "找不到 Kubernetes 配置目錄: $k8s_dir"
        exit 1
    fi
    
    # 更新映像版本
    log_info "更新 Kubernetes 配置中的映像版本..."
    sed -i.bak "s|ghcr.io/your-org/gamecore:.*|ghcr.io/your-org/gamecore:$VERSION|g" "$k8s_dir/gamecore-deployment.yaml"
    
    # 套用配置
    log_info "套用 Kubernetes 配置..."
    if ! kubectl apply -f "$k8s_dir/"; then
        log_error "Kubernetes 部署失敗！"
        exit 1
    fi
    
    # 等待部署完成
    log_info "等待部署完成..."
    if ! kubectl rollout status deployment/gamecore-web -n gamecore --timeout=300s; then
        log_error "部署超時！"
        exit 1
    fi
    
    log_success "Kubernetes 部署完成！"
}

# Azure 部署
deploy_azure() {
    log_info "執行 Azure 部署..."
    
    local resource_group="gamecore-${ENVIRONMENT}-rg"
    local app_name="gamecore-${ENVIRONMENT}"
    
    # 檢查 Azure 登入狀態
    if ! az account show >/dev/null 2>&1; then
        log_error "請先登入 Azure: az login"
        exit 1
    fi
    
    # 推送映像到 Azure Container Registry
    local acr_name="gamecoreacr"
    local image_name="${acr_name}.azurecr.io/gamecore:$VERSION"
    
    log_info "推送映像到 Azure Container Registry..."
    docker tag "gamecore:$VERSION" "$image_name"
    az acr login --name "$acr_name"
    docker push "$image_name"
    
    # 更新 App Service
    log_info "更新 Azure App Service..."
    az webapp config container set \
        --resource-group "$resource_group" \
        --name "$app_name" \
        --docker-custom-image-name "$image_name"
    
    # 重啟應用程式
    log_info "重啟應用程式..."
    az webapp restart --resource-group "$resource_group" --name "$app_name"
    
    log_success "Azure 部署完成！"
}

# 本地部署
deploy_local() {
    log_info "執行本地部署..."
    
    # 停止現有程序
    log_info "停止現有程序..."
    pkill -f "GameCore.Web" || true
    
    # 建置應用程式
    log_info "建置應用程式..."
    if ! dotnet build --configuration Release; then
        log_error "建置失敗！"
        exit 1
    fi
    
    # 發布應用程式
    log_info "發布應用程式..."
    local publish_dir="./publish"
    rm -rf "$publish_dir"
    
    if ! dotnet publish GameCore.Web/GameCore.Web.csproj \
        --configuration Release \
        --output "$publish_dir" \
        --self-contained false; then
        log_error "發布失敗！"
        exit 1
    fi
    
    # 啟動應用程式
    log_info "啟動應用程式..."
    cd "$publish_dir"
    nohup dotnet GameCore.Web.dll > ../logs/app.log 2>&1 &
    cd ..
    
    # 等待啟動
    sleep 10
    
    # 健康檢查
    if ! curl -f http://localhost:5000/health >/dev/null 2>&1; then
        log_error "應用程式健康檢查失敗！"
        exit 1
    fi
    
    log_success "本地部署完成！"
}

# 部署後驗證
post_deployment_verification() {
    log_info "執行部署後驗證..."
    
    local base_url=""
    case $DEPLOYMENT_TYPE in
        docker|local)
            base_url="http://localhost:5000"
            ;;
        kubernetes)
            # 獲取 Ingress URL
            base_url=$(kubectl get ingress gamecore-ingress -n gamecore -o jsonpath='{.spec.rules[0].host}' 2>/dev/null || echo "http://localhost")
            ;;
        azure)
            base_url="https://gamecore-${ENVIRONMENT}.azurewebsites.net"
            ;;
    esac
    
    # 健康檢查
    log_info "檢查應用程式健康狀態..."
    for i in {1..5}; do
        if curl -f "${base_url}/health" >/dev/null 2>&1; then
            log_success "應用程式運行正常！"
            break
        else
            log_warn "健康檢查失敗，重試 $i/5..."
            sleep 10
        fi
        
        if [ $i -eq 5 ]; then
            log_error "應用程式健康檢查失敗！"
            exit 1
        fi
    done
    
    # 基本功能測試
    log_info "執行基本功能測試..."
    if curl -f "${base_url}/" >/dev/null 2>&1; then
        log_success "首頁存取正常"
    else
        log_warn "首頁存取失敗"
    fi
    
    log_success "部署驗證完成！"
    log_info "應用程式 URL: $base_url"
}

# 顯示部署摘要
show_deployment_summary() {
    echo ""
    echo -e "${CYAN}========================================${NC}"
    echo -e "${CYAN}           部署完成摘要${NC}"
    echo -e "${CYAN}========================================${NC}"
    echo -e "${GREEN}環境:${NC} $ENVIRONMENT"
    echo -e "${GREEN}部署類型:${NC} $DEPLOYMENT_TYPE"
    echo -e "${GREEN}版本:${NC} $VERSION"
    echo -e "${GREEN}時間:${NC} $(date)"
    echo -e "${CYAN}========================================${NC}"
    echo ""
}

# 主函數
main() {
    echo -e "${CYAN}========================================${NC}"
    echo -e "${CYAN}        GameCore 部署腳本${NC}"
    echo -e "${CYAN}========================================${NC}"
    
    # 解析參數
    parse_arguments "$@"
    
    # 驗證環境
    validate_environment
    
    # 檢查工具
    check_prerequisites
    
    # 獲取版本
    get_version
    
    # 確認部署
    if [ "$SKIP_CONFIRM" != true ]; then
        echo ""
        echo -e "${YELLOW}即將部署 GameCore:${NC}"
        echo -e "  環境: ${GREEN}$ENVIRONMENT${NC}"
        echo -e "  類型: ${GREEN}$DEPLOYMENT_TYPE${NC}"
        echo -e "  版本: ${GREEN}$VERSION${NC}"
        echo ""
        read -p "確定要繼續嗎？ (y/N): " -n 1 -r
        echo ""
        if [[ ! $REPLY =~ ^[Yy]$ ]]; then
            log_info "部署已取消"
            exit 0
        fi
    fi
    
    # 執行部署流程
    log_info "開始部署流程..."
    
    # 1. 備份資料庫
    backup_database
    
    # 2. 執行測試
    run_tests
    
    # 3. 建置映像
    build_docker_image
    
    # 4. 執行部署
    case $DEPLOYMENT_TYPE in
        docker)
            deploy_docker
            ;;
        kubernetes)
            deploy_kubernetes
            ;;
        azure)
            deploy_azure
            ;;
        local)
            deploy_local
            ;;
    esac
    
    # 5. 部署後驗證
    post_deployment_verification
    
    # 6. 顯示摘要
    show_deployment_summary
    
    log_success "🎉 部署成功完成！"
}

# 錯誤處理
trap 'log_error "部署過程中發生錯誤！"; exit 1' ERR

# 執行主函數
main "$@"
