#!/bin/bash

# GameCore 專案狀態檢查腳本
# 檢查專案的完整性和可運行狀態

set -e

# 顏色定義
RED='\033[0;31m'
GREEN='\033[0;32m'
YELLOW='\033[1;33m'
BLUE='\033[0;34m'
NC='\033[0m' # No Color

# 計數器
TOTAL_CHECKS=0
PASSED_CHECKS=0
FAILED_CHECKS=0

# 日誌函數
log_info() {
    echo -e "${BLUE}[INFO]${NC} $1"
}

log_success() {
    echo -e "${GREEN}[✓]${NC} $1"
    ((PASSED_CHECKS++))
    ((TOTAL_CHECKS++))
}

log_warning() {
    echo -e "${YELLOW}[!]${NC} $1"
    ((TOTAL_CHECKS++))
}

log_error() {
    echo -e "${RED}[✗]${NC} $1"
    ((FAILED_CHECKS++))
    ((TOTAL_CHECKS++))
}

# 檢查檔案是否存在
check_file() {
    local file_path="$1"
    local description="$2"
    
    if [ -f "$file_path" ]; then
        log_success "$description: $file_path"
    else
        log_error "$description: $file_path (檔案不存在)"
    fi
}

# 檢查目錄是否存在
check_directory() {
    local dir_path="$1"
    local description="$2"
    
    if [ -d "$dir_path" ]; then
        log_success "$description: $dir_path"
    else
        log_error "$description: $dir_path (目錄不存在)"
    fi
}

# 檢查 .NET 專案檔案
check_dotnet_project() {
    local project_file="$1"
    local description="$2"
    
    if [ -f "$project_file" ]; then
        # 檢查專案檔案是否有效
        if dotnet build "$project_file" --no-restore --verbosity quiet 2>/dev/null; then
            log_success "$description: $project_file"
        else
            log_error "$description: $project_file (建置失敗)"
        fi
    else
        log_error "$description: $project_file (檔案不存在)"
    fi
}

# 檢查專案結構
check_project_structure() {
    log_info "檢查專案結構..."
    
    # 檢查主要檔案
    check_file "GameCore.sln" "解決方案檔案"
    check_file "README.md" "專案說明文件"
    check_file ".gitignore" "Git 忽略檔案"
    check_file "Dockerfile" "Docker 配置檔案"
    check_file "docker-compose.yml" "Docker Compose 配置"
    check_file "deploy.sh" "Linux 部署腳本"
    check_file "deploy.bat" "Windows 部署腳本"
    
    # 檢查專案目錄
    check_directory "GameCore.Web" "Web 應用程式專案"
    check_directory "GameCore.Core" "核心業務邏輯專案"
    check_directory "GameCore.Infrastructure" "基礎設施專案"
    check_directory "GameCore.Tests" "測試專案"
    check_directory "Database" "資料庫腳本目錄"
    check_directory "Documentation" "文件目錄"
    check_directory ".github/workflows" "GitHub Actions 配置"
    
    # 檢查 Web 專案結構
    if [ -d "GameCore.Web" ]; then
        check_file "GameCore.Web/GameCore.Web.csproj" "Web 專案檔案"
        check_file "GameCore.Web/Program.cs" "應用程式入口點"
        check_file "GameCore.Web/appsettings.json" "應用程式設定"
        check_directory "GameCore.Web/Controllers" "控制器目錄"
        check_directory "GameCore.Web/Views" "視圖目錄"
        check_directory "GameCore.Web/wwwroot" "靜態資源目錄"
    fi
    
    # 檢查 Core 專案結構
    if [ -d "GameCore.Core" ]; then
        check_file "GameCore.Core/GameCore.Core.csproj" "Core 專案檔案"
        check_directory "GameCore.Core/Entities" "實體類別目錄"
        check_directory "GameCore.Core/Services" "業務服務目錄"
        check_directory "GameCore.Core/Interfaces" "介面定義目錄"
    fi
    
    # 檢查 Infrastructure 專案結構
    if [ -d "GameCore.Infrastructure" ]; then
        check_file "GameCore.Infrastructure/GameCore.Infrastructure.csproj" "Infrastructure 專案檔案"
        check_directory "GameCore.Infrastructure/Data" "資料存取目錄"
        check_directory "GameCore.Infrastructure/Repositories" "倉庫實作目錄"
    fi
    
    # 檢查測試專案結構
    if [ -d "GameCore.Tests" ]; then
        check_file "GameCore.Tests/GameCore.Tests.csproj" "測試專案檔案"
        check_directory "GameCore.Tests/UnitTests" "單元測試目錄"
    fi
}

# 檢查 .NET 專案建置
check_dotnet_build() {
    log_info "檢查 .NET 專案建置..."
    
    # 還原套件
    if dotnet restore --verbosity quiet; then
        log_success "NuGet 套件還原成功"
    else
        log_error "NuGet 套件還原失敗"
        return 1
    fi
    
    # 建置專案
    if dotnet build --configuration Release --no-restore --verbosity quiet; then
        log_success "專案建置成功"
    else
        log_error "專案建置失敗"
        return 1
    fi
}

# 檢查測試執行
check_tests() {
    log_info "檢查測試執行..."
    
    if [ -d "GameCore.Tests" ]; then
        if dotnet test --configuration Release --no-build --verbosity quiet; then
            log_success "測試執行成功"
        else
            log_warning "測試執行失敗"
        fi
    else
        log_warning "測試專案不存在"
    fi
}

# 檢查資料庫腳本
check_database_scripts() {
    log_info "檢查資料庫腳本..."
    
    if [ -d "Database" ]; then
        check_file "Database/01-CreateTables.sql" "建立資料表腳本"
        check_file "Database/02-InsertMockData.sql" "插入假資料腳本"
        
        # 檢查 SQL 腳本大小
        if [ -f "Database/01-CreateTables.sql" ]; then
            local size=$(wc -l < "Database/01-CreateTables.sql")
            if [ $size -gt 100 ]; then
                log_success "建立資料表腳本包含 $size 行"
            else
                log_warning "建立資料表腳本行數較少 ($size 行)"
            fi
        fi
        
        if [ -f "Database/02-InsertMockData.sql" ]; then
            local size=$(wc -l < "Database/02-InsertMockData.sql")
            if [ $size -gt 100 ]; then
                log_success "插入假資料腳本包含 $size 行"
            else
                log_warning "插入假資料腳本行數較少 ($size 行)"
            fi
        fi
    else
        log_error "資料庫腳本目錄不存在"
    fi
}

# 檢查 Docker 配置
check_docker_config() {
    log_info "檢查 Docker 配置..."
    
    if [ -f "Dockerfile" ]; then
        log_success "Dockerfile 存在"
        
        # 檢查 Dockerfile 內容
        if grep -q "FROM mcr.microsoft.com/dotnet/aspnet:8.0" Dockerfile; then
            log_success "Dockerfile 使用正確的 .NET 8.0 基礎映像"
        else
            log_warning "Dockerfile 可能未使用 .NET 8.0 基礎映像"
        fi
    else
        log_error "Dockerfile 不存在"
    fi
    
    if [ -f "docker-compose.yml" ]; then
        log_success "docker-compose.yml 存在"
        
        # 檢查 docker-compose 配置
        if grep -q "sqlserver" docker-compose.yml; then
            log_success "Docker Compose 包含 SQL Server 服務"
        else
            log_warning "Docker Compose 可能缺少 SQL Server 服務"
        fi
    else
        log_error "docker-compose.yml 不存在"
    fi
}

# 檢查 GitHub Actions 配置
check_github_actions() {
    log_info "檢查 GitHub Actions 配置..."
    
    if [ -d ".github/workflows" ]; then
        if [ -f ".github/workflows/ci-cd.yml" ]; then
            log_success "CI/CD 工作流程配置存在"
            
            # 檢查工作流程內容
            if grep -q "dotnet" .github/workflows/ci-cd.yml; then
                log_success "CI/CD 工作流程包含 .NET 建置步驟"
            else
                log_warning "CI/CD 工作流程可能缺少 .NET 建置步驟"
            fi
        else
            log_error "CI/CD 工作流程配置檔案不存在"
        fi
    else
        log_warning "GitHub Actions 配置目錄不存在"
    fi
}

# 檢查部署腳本
check_deployment_scripts() {
    log_info "檢查部署腳本..."
    
    if [ -f "deploy.sh" ]; then
        if [ -x "deploy.sh" ]; then
            log_success "Linux 部署腳本存在且可執行"
        else
            log_warning "Linux 部署腳本存在但不可執行"
        fi
    else
        log_error "Linux 部署腳本不存在"
    fi
    
    if [ -f "deploy.bat" ]; then
        log_success "Windows 部署腳本存在"
    else
        log_error "Windows 部署腳本不存在"
    fi
}

# 檢查專案依賴
check_dependencies() {
    log_info "檢查專案依賴..."
    
    # 檢查 .NET 版本
    if command -v dotnet &> /dev/null; then
        local version=$(dotnet --version)
        log_success "檢測到 .NET 版本: $version"
        
        if [[ $version =~ ^8\. ]]; then
            log_success ".NET 版本符合要求 (8.x)"
        else
            log_warning "建議使用 .NET 8.0 版本"
        fi
    else
        log_error ".NET SDK 未安裝"
    fi
    
    # 檢查 Docker
    if command -v docker &> /dev/null; then
        log_success "Docker 已安裝"
    else
        log_warning "Docker 未安裝 (可選)"
    fi
    
    # 檢查 Docker Compose
    if command -v docker-compose &> /dev/null; then
        log_success "Docker Compose 已安裝"
    else
        log_warning "Docker Compose 未安裝 (可選)"
    fi
}

# 顯示檢查結果摘要
show_summary() {
    echo ""
    echo "=========================================="
    echo "          專案狀態檢查摘要"
    echo "=========================================="
    echo "總檢查項目: $TOTAL_CHECKS"
    echo "通過檢查: $PASSED_CHECKS"
    echo "失敗檢查: $FAILED_CHECKS"
    echo "警告項目: $((TOTAL_CHECKS - PASSED_CHECKS - FAILED_CHECKS))"
    
    if [ $FAILED_CHECKS -eq 0 ]; then
        echo ""
        echo -e "${GREEN}🎉 專案狀態良好！所有必要檢查都通過了。${NC}"
    else
        echo ""
        echo -e "${RED}⚠️  專案存在 $FAILED_CHECKS 個問題需要解決。${NC}"
    fi
    
    if [ $PASSED_CHECKS -eq $TOTAL_CHECKS ]; then
        echo -e "${GREEN}🚀 專案已準備就緒，可以立即部署！${NC}"
    fi
}

# 主函數
main() {
    echo "=========================================="
    echo "        GameCore 專案狀態檢查"
    echo "=========================================="
    echo ""
    
    # 重置計數器
    TOTAL_CHECKS=0
    PASSED_CHECKS=0
    FAILED_CHECKS=0
    
    # 執行各項檢查
    check_project_structure
    check_dotnet_build
    check_tests
    check_database_scripts
    check_docker_config
    check_github_actions
    check_deployment_scripts
    check_dependencies
    
    # 顯示摘要
    show_summary
    
    # 返回適當的退出碼
    if [ $FAILED_CHECKS -eq 0 ]; then
        exit 0
    else
        exit 1
    fi
}

# 執行主函數
main "$@"