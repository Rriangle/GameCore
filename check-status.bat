@echo off
setlocal enabledelayedexpansion

REM GameCore 專案狀態檢查腳本 (Windows 版本)
REM 檢查專案的完整性和可運行狀態

set "SCRIPT_NAME=%~n0"
set "SCRIPT_DIR=%~dp0"

REM 計數器
set "TOTAL_CHECKS=0"
set "PASSED_CHECKS=0"
set "FAILED_CHECKS=0"

REM 顏色定義 (Windows 10+ 支援)
if "%TERM%"=="xterm" (
    set "RED=[91m"
    set "GREEN=[92m"
    set "YELLOW=[93m"
    set "BLUE=[94m"
    set "NC=[0m"
) else (
    set "RED="
    set "GREEN="
    set "YELLOW="
    set "BLUE="
    set "NC="
)

REM 日誌函數
:log_info
echo %BLUE%[INFO]%NC% %~1
goto :eof

:log_success
echo %GREEN%[✓]%NC% %~1
set /a PASSED_CHECKS+=1
set /a TOTAL_CHECKS+=1
goto :eof

:log_warning
echo %YELLOW%[!]%NC% %~1
set /a TOTAL_CHECKS+=1
goto :eof

:log_error
echo %RED%[✗]%NC% %~1
set /a FAILED_CHECKS+=1
set /a TOTAL_CHECKS+=1
goto :eof

REM 檢查檔案是否存在
:check_file
set "file_path=%~1"
set "description=%~2"

if exist "!file_path!" (
    call :log_success "!description!: !file_path!"
) else (
    call :log_error "!description!: !file_path! (檔案不存在)"
)
goto :eof

REM 檢查目錄是否存在
:check_directory
set "dir_path=%~1"
set "description=%~2"

if exist "!dir_path!" (
    call :log_success "!description!: !dir_path!"
) else (
    call :log_error "!description!: !dir_path! (目錄不存在)"
)
goto :eof

REM 檢查 .NET 專案檔案
:check_dotnet_project
set "project_file=%~1"
set "description=%~2"

if exist "!project_file!" (
    REM 檢查專案檔案是否有效
    dotnet build "!project_file!" --no-restore --verbosity quiet >nul 2>&1
    if !errorlevel! equ 0 (
        call :log_success "!description!: !project_file!"
    ) else (
        call :log_error "!description!: !project_file! (建置失敗)"
    )
) else (
    call :log_error "!description!: !project_file! (檔案不存在)"
)
goto :eof

REM 檢查專案結構
:check_project_structure
call :log_info "檢查專案結構..."

REM 檢查主要檔案
call :check_file "GameCore.sln" "解決方案檔案"
call :check_file "README.md" "專案說明文件"
call :check_file ".gitignore" "Git 忽略檔案"
call :check_file "Dockerfile" "Docker 配置檔案"
call :check_file "docker-compose.yml" "Docker Compose 配置"
call :check_file "deploy.sh" "Linux 部署腳本"
call :check_file "deploy.bat" "Windows 部署腳本"

REM 檢查專案目錄
call :check_directory "GameCore.Web" "Web 應用程式專案"
call :check_directory "GameCore.Core" "核心業務邏輯專案"
call :check_directory "GameCore.Infrastructure" "基礎設施專案"
call :check_directory "GameCore.Tests" "測試專案"
call :check_directory "Database" "資料庫腳本目錄"
call :check_directory "Documentation" "文件目錄"
call :check_directory ".github\workflows" "GitHub Actions 配置"

REM 檢查 Web 專案結構
if exist "GameCore.Web" (
    call :check_file "GameCore.Web\GameCore.Web.csproj" "Web 專案檔案"
    call :check_file "GameCore.Web\Program.cs" "應用程式入口點"
    call :check_file "GameCore.Web\appsettings.json" "應用程式設定"
    call :check_directory "GameCore.Web\Controllers" "控制器目錄"
    call :check_directory "GameCore.Web\Views" "視圖目錄"
    call :check_directory "GameCore.Web\wwwroot" "靜態資源目錄"
)

REM 檢查 Core 專案結構
if exist "GameCore.Core" (
    call :check_file "GameCore.Core\GameCore.Core.csproj" "Core 專案檔案"
    call :check_directory "GameCore.Core\Entities" "實體類別目錄"
    call :check_directory "GameCore.Core\Services" "業務服務目錄"
    call :check_directory "GameCore.Core\Interfaces" "介面定義目錄"
)

REM 檢查 Infrastructure 專案結構
if exist "GameCore.Infrastructure" (
    call :check_file "GameCore.Infrastructure\GameCore.Infrastructure.csproj" "Infrastructure 專案檔案"
    call :check_directory "GameCore.Infrastructure\Data" "資料存取目錄"
    call :check_directory "GameCore.Infrastructure\Repositories" "倉庫實作目錄"
)

REM 檢查測試專案結構
if exist "GameCore.Tests" (
    call :check_file "GameCore.Tests\GameCore.Tests.csproj" "測試專案檔案"
    call :check_directory "GameCore.Tests\UnitTests" "單元測試目錄"
)
goto :eof

REM 檢查 .NET 專案建置
:check_dotnet_build
call :log_info "檢查 .NET 專案建置..."

REM 還原套件
dotnet restore --verbosity quiet >nul 2>&1
if !errorlevel! equ 0 (
    call :log_success "NuGet 套件還原成功"
) else (
    call :log_error "NuGet 套件還原失敗"
    goto :eof
)

REM 建置專案
dotnet build --configuration Release --no-restore --verbosity quiet >nul 2>&1
if !errorlevel! equ 0 (
    call :log_success "專案建置成功"
) else (
    call :log_error "專案建置失敗"
    goto :eof
)
goto :eof

REM 檢查測試執行
:check_tests
call :log_info "檢查測試執行..."

if exist "GameCore.Tests" (
    dotnet test --configuration Release --no-build --verbosity quiet >nul 2>&1
    if !errorlevel! equ 0 (
        call :log_success "測試執行成功"
    ) else (
        call :log_warning "測試執行失敗"
    )
) else (
    call :log_warning "測試專案不存在"
)
goto :eof

REM 檢查資料庫腳本
:check_database_scripts
call :log_info "檢查資料庫腳本..."

if exist "Database" (
    call :check_file "Database\01-CreateTables.sql" "建立資料表腳本"
    call :check_file "Database\02-InsertMockData.sql" "插入假資料腳本"
    
    REM 檢查 SQL 腳本大小
    if exist "Database\01-CreateTables.sql" (
        for /f %%i in ('type "Database\01-CreateTables.sql" ^| find /c /v ""') do set "size=%%i"
        if !size! gtr 100 (
            call :log_success "建立資料表腳本包含 !size! 行"
        ) else (
            call :log_warning "建立資料表腳本行數較少 (!size! 行)"
        )
    )
    
    if exist "Database\02-InsertMockData.sql" (
        for /f %%i in ('type "Database\02-InsertMockData.sql" ^| find /c /v ""') do set "size=%%i"
        if !size! gtr 100 (
            call :log_success "插入假資料腳本包含 !size! 行"
        ) else (
            call :log_warning "插入假資料腳本行數較少 (!size! 行)"
        )
    )
) else (
    call :log_error "資料庫腳本目錄不存在"
)
goto :eof

REM 檢查 Docker 配置
:check_docker_config
call :log_info "檢查 Docker 配置..."

if exist "Dockerfile" (
    call :log_success "Dockerfile 存在"
    
    REM 檢查 Dockerfile 內容
    findstr /c:"FROM mcr.microsoft.com/dotnet/aspnet:8.0" Dockerfile >nul 2>&1
    if !errorlevel! equ 0 (
        call :log_success "Dockerfile 使用正確的 .NET 8.0 基礎映像"
    ) else (
        call :log_warning "Dockerfile 可能未使用 .NET 8.0 基礎映像"
    )
) else (
    call :log_error "Dockerfile 不存在"
)

if exist "docker-compose.yml" (
    call :log_success "docker-compose.yml 存在"
    
    REM 檢查 docker-compose 配置
    findstr /c:"sqlserver" docker-compose.yml >nul 2>&1
    if !errorlevel! equ 0 (
        call :log_success "Docker Compose 包含 SQL Server 服務"
    ) else (
        call :log_warning "Docker Compose 可能缺少 SQL Server 服務"
    )
) else (
    call :log_error "docker-compose.yml 不存在"
)
goto :eof

REM 檢查 GitHub Actions 配置
:check_github_actions
call :log_info "檢查 GitHub Actions 配置..."

if exist ".github\workflows" (
    if exist ".github\workflows\ci-cd.yml" (
        call :log_success "CI/CD 工作流程配置存在"
        
        REM 檢查工作流程內容
        findstr /c:"dotnet" .github\workflows\ci-cd.yml >nul 2>&1
        if !errorlevel! equ 0 (
            call :log_success "CI/CD 工作流程包含 .NET 建置步驟"
        ) else (
            call :log_warning "CI/CD 工作流程可能缺少 .NET 建置步驟"
        )
    ) else (
        call :log_error "CI/CD 工作流程配置檔案不存在"
    )
) else (
    call :log_warning "GitHub Actions 配置目錄不存在"
)
goto :eof

REM 檢查部署腳本
:check_deployment_scripts
call :log_info "檢查部署腳本..."

if exist "deploy.sh" (
    call :log_success "Linux 部署腳本存在"
) else (
    call :log_error "Linux 部署腳本不存在"
)

if exist "deploy.bat" (
    call :log_success "Windows 部署腳本存在"
) else (
    call :log_error "Windows 部署腳本不存在"
)
goto :eof

REM 檢查專案依賴
:check_dependencies
call :log_info "檢查專案依賴..."

REM 檢查 .NET 版本
where dotnet >nul 2>&1
if !errorlevel! equ 0 (
    for /f "tokens=*" %%i in ('dotnet --version') do set "VERSION=%%i"
    call :log_success "檢測到 .NET 版本: !VERSION!"
    
    if "!VERSION!"=="8." (
        call :log_success ".NET 版本符合要求 (8.x)"
    ) else (
        call :log_warning "建議使用 .NET 8.0 版本"
    )
) else (
    call :log_error ".NET SDK 未安裝"
)

REM 檢查 Docker
where docker >nul 2>&1
if !errorlevel! equ 0 (
    call :log_success "Docker 已安裝"
) else (
    call :log_warning "Docker 未安裝 (可選)"
)

REM 檢查 Docker Compose
where docker-compose >nul 2>&1
if !errorlevel! equ 0 (
    call :log_success "Docker Compose 已安裝"
) else (
    call :log_warning "Docker Compose 未安裝 (可選)"
)
goto :eof

REM 顯示檢查結果摘要
:show_summary
echo.
echo ==========================================
echo           專案狀態檢查摘要
echo ==========================================
echo 總檢查項目: !TOTAL_CHECKS!
echo 通過檢查: !PASSED_CHECKS!
echo 失敗檢查: !FAILED_CHECKS!
set /a WARNING_COUNT=!TOTAL_CHECKS!-!PASSED_CHECKS!-!FAILED_CHECKS!
echo 警告項目: !WARNING_COUNT!

if !FAILED_CHECKS! equ 0 (
    echo.
    echo %GREEN%🎉 專案狀態良好！所有必要檢查都通過了。%NC%
) else (
    echo.
    echo %RED%⚠️  專案存在 !FAILED_CHECKS! 個問題需要解決。%NC%
)

if !PASSED_CHECKS! equ !TOTAL_CHECKS! (
    echo %GREEN%🚀 專案已準備就緒，可以立即部署！%NC%
)
goto :eof

REM 主函數
:main
echo ==========================================
echo        GameCore 專案狀態檢查
echo ==========================================
echo.

REM 重置計數器
set "TOTAL_CHECKS=0"
set "PASSED_CHECKS=0"
set "FAILED_CHECKS=0"

REM 執行各項檢查
call :check_project_structure
call :check_dotnet_build
call :check_tests
call :check_database_scripts
call :check_docker_config
call :check_github_actions
call :check_deployment_scripts
call :check_dependencies

REM 顯示摘要
call :show_summary

REM 返回適當的退出碼
if !FAILED_CHECKS! equ 0 (
    exit /b 0
) else (
    exit /b 1
)

REM 執行主函數
call :main
exit /b %errorlevel%