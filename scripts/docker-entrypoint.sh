#!/bin/bash
# GameCore Docker 容器啟動腳本
# 負責初始化和啟動 GameCore 應用程式

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

log_warn() {
    echo -e "${YELLOW}[WARN]${NC} $1"
}

log_error() {
    echo -e "${RED}[ERROR]${NC} $1"
}

log_success() {
    echo -e "${GREEN}[SUCCESS]${NC} $1"
}

# 檢查環境變數
check_environment() {
    log_info "檢查環境配置..."
    
    # 檢查必要的環境變數
    local required_vars=(
        "ASPNETCORE_ENVIRONMENT"
        "ConnectionStrings__DefaultConnection"
    )
    
    for var in "${required_vars[@]}"; do
        if [ -z "${!var}" ]; then
            log_warn "環境變數 $var 未設定"
        else
            log_info "✓ $var 已設定"
        fi
    done
}

# 等待資料庫連線
wait_for_database() {
    if [ -n "$ConnectionStrings__DefaultConnection" ]; then
        log_info "等待資料庫連線..."
        
        # 提取資料庫伺服器資訊
        local server=$(echo "$ConnectionStrings__DefaultConnection" | grep -oP 'Server=\K[^;]+')
        local timeout=60
        local count=0
        
        if [ -n "$server" ]; then
            log_info "嘗試連接到資料庫伺服器: $server"
            
            while [ $count -lt $timeout ]; do
                if nc -z ${server%%,*} ${server##*,} 2>/dev/null || nc -z ${server} 1433 2>/dev/null; then
                    log_success "資料庫連線成功"
                    return 0
                fi
                
                count=$((count + 1))
                log_info "等待資料庫... ($count/$timeout)"
                sleep 1
            done
            
            log_error "資料庫連線超時"
            return 1
        else
            log_warn "無法解析資料庫伺服器資訊"
        fi
    else
        log_info "跳過資料庫連線檢查（未設定連線字串）"
    fi
}

# 等待 Redis 連線
wait_for_redis() {
    if [ -n "$ConnectionStrings__Redis" ]; then
        log_info "等待 Redis 連線..."
        
        local redis_host=$(echo "$ConnectionStrings__Redis" | cut -d':' -f1)
        local redis_port=$(echo "$ConnectionStrings__Redis" | cut -d':' -f2)
        local timeout=30
        local count=0
        
        while [ $count -lt $timeout ]; do
            if nc -z "$redis_host" "$redis_port" 2>/dev/null; then
                log_success "Redis 連線成功"
                return 0
            fi
            
            count=$((count + 1))
            log_info "等待 Redis... ($count/$timeout)"
            sleep 1
        done
        
        log_error "Redis 連線超時"
        return 1
    else
        log_info "跳過 Redis 連線檢查（未設定連線字串）"
    fi
}

# 初始化應用程式目錄
init_directories() {
    log_info "初始化應用程式目錄..."
    
    local dirs=("logs" "uploads" "temp" "cache")
    
    for dir in "${dirs[@]}"; do
        if [ ! -d "/app/$dir" ]; then
            mkdir -p "/app/$dir"
            log_info "創建目錄: /app/$dir"
        fi
    done
    
    # 設定目錄權限
    chmod 755 /app/logs /app/uploads /app/temp 2>/dev/null || true
}

# 檢查應用程式檔案
check_application_files() {
    log_info "檢查應用程式檔案..."
    
    local required_files=(
        "GameCore.Web.dll"
        "appsettings.json"
    )
    
    for file in "${required_files[@]}"; do
        if [ ! -f "/app/$file" ]; then
            log_error "找不到必要檔案: $file"
            return 1
        else
            log_info "✓ $file 存在"
        fi
    done
}

# 設定應用程式配置
setup_configuration() {
    log_info "設定應用程式配置..."
    
    # 如果是開發環境，啟用詳細日誌
    if [ "$ASPNETCORE_ENVIRONMENT" = "Development" ]; then
        export DOTNET_LOGGING__LOGLEVEL__DEFAULT=Debug
        export DOTNET_LOGGING__LOGLEVEL__MICROSOFT=Information
        log_info "開發環境：啟用詳細日誌"
    fi
    
    # 設定文化資訊
    export LC_ALL=zh_TW.UTF-8
    export LANG=zh_TW.UTF-8
    
    # 設定 ASP.NET Core 配置
    export DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false
    export DOTNET_RUNNING_IN_CONTAINER=true
}

# 執行資料庫遷移（如果需要）
run_database_migrations() {
    if [ "$RUN_MIGRATIONS" = "true" ] && [ -n "$ConnectionStrings__DefaultConnection" ]; then
        log_info "執行資料庫遷移..."
        
        # 這裡可以添加 EF Core 遷移命令
        # dotnet ef database update --no-build --verbose || {
        #     log_error "資料庫遷移失敗"
        #     return 1
        # }
        
        log_info "跳過資料庫遷移（未配置）"
    else
        log_info "跳過資料庫遷移"
    fi
}

# 健康檢查
health_check() {
    log_info "執行健康檢查..."
    
    # 檢查磁碟空間
    local disk_usage=$(df / | awk 'NR==2 {print $5}' | sed 's/%//')
    if [ "$disk_usage" -gt 90 ]; then
        log_warn "磁碟使用率過高: $disk_usage%"
    fi
    
    # 檢查記憶體使用
    if command -v free >/dev/null 2>&1; then
        local mem_usage=$(free | grep Mem | awk '{printf "%.1f", $3/$2 * 100.0}')
        log_info "記憶體使用率: $mem_usage%"
    fi
}

# 信號處理
setup_signal_handlers() {
    # 優雅關機處理
    trap 'log_info "收到終止信號，正在優雅關機..."; kill -TERM $PID; wait $PID' TERM INT
}

# 啟動應用程式
start_application() {
    log_info "啟動 GameCore 應用程式..."
    log_info "環境: $ASPNETCORE_ENVIRONMENT"
    log_info "命令: $*"
    
    # 如果是開發環境，使用 dotnet watch
    if [ "$ASPNETCORE_ENVIRONMENT" = "Development" ] && [ "$USE_WATCH" = "true" ]; then
        log_info "使用 dotnet watch 啟動（開發模式）"
        exec dotnet watch run --project GameCore.Web --no-restore
    else
        # 生產環境直接執行
        exec "$@"
    fi
}

# 主函數
main() {
    log_info "=========================================="
    log_info "GameCore 容器啟動中..."
    log_info "時間: $(date)"
    log_info "=========================================="
    
    # 設定信號處理
    setup_signal_handlers
    
    # 執行初始化步驟
    check_environment
    init_directories
    check_application_files
    setup_configuration
    
    # 等待外部服務
    wait_for_database
    wait_for_redis
    
    # 執行資料庫相關操作
    run_database_migrations
    
    # 健康檢查
    health_check
    
    log_success "初始化完成，啟動應用程式..."
    
    # 啟動應用程式
    start_application "$@"
}

# 如果腳本被直接執行
if [ "${BASH_SOURCE[0]}" = "${0}" ]; then
    main "$@"
fi
