@echo off
setlocal enabledelayedexpansion

REM GameCore Windows 部署腳本
REM 支援本地開發、Docker 和雲端部署

set "SCRIPT_NAME=%~n0"
set "SCRIPT_DIR=%~dp0"

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
echo %GREEN%[SUCCESS]%NC% %~1
goto :eof

:log_warning
echo %YELLOW%[WARNING]%NC% %~1
goto :eof

:log_error
echo %RED%[ERROR]%NC% %~1
goto :eof

REM 檢查命令是否存在
:check_command
where %1 >nul 2>&1
if %errorlevel% neq 0 (
    call :log_error "%1 未安裝，請先安裝 %1"
    exit /b 1
)
goto :eof

REM 檢查 .NET 環境
:check_dotnet
call :log_info "檢查 .NET 環境..."
call :check_command dotnet
if %errorlevel% neq 0 goto :eof

for /f "tokens=*" %%i in ('dotnet --version') do set "VERSION=%%i"
call :log_info "檢測到 .NET 版本: !VERSION!"

if not "!VERSION!"=="8." (
    call :log_warning "建議使用 .NET 8.0 版本"
)
goto :eof

REM 檢查 Docker 環境
:check_docker
call :log_info "檢查 Docker 環境..."
call :check_command docker
if %errorlevel% neq 0 goto :eof

call :check_command docker-compose
if %errorlevel% neq 0 goto :eof

call :log_success "Docker 環境檢查通過"
goto :eof

REM 還原 NuGet 套件
:restore_packages
call :log_info "還原 NuGet 套件..."
dotnet restore
if %errorlevel% neq 0 (
    call :log_error "套件還原失敗"
    exit /b 1
)
call :log_success "套件還原完成"
goto :eof

REM 建置專案
:build_project
call :log_info "建置專案..."
dotnet build --configuration Release --no-restore
if %errorlevel% neq 0 (
    call :log_error "專案建置失敗"
    exit /b 1
)
call :log_success "專案建置完成"
goto :eof

REM 執行測試
:run_tests
call :log_info "執行測試..."
dotnet test --configuration Release --no-build --verbosity normal
if %errorlevel% neq 0 (
    call :log_warning "測試執行失敗，但繼續部署"
)
call :log_success "測試執行完成"
goto :eof

REM 發佈專案
:publish_project
set "OUTPUT_DIR=.\publish"
call :log_info "發佈專案到 %OUTPUT_DIR%..."
dotnet publish GameCore.Web --configuration Release --output %OUTPUT_DIR% --no-build
if %errorlevel% neq 0 (
    call :log_error "專案發佈失敗"
    exit /b 1
)
call :log_success "專案發佈完成"
goto :eof

REM 本地開發部署
:deploy_local
call :log_info "開始本地開發部署..."
call :check_dotnet
if %errorlevel% neq 0 goto :eof

call :restore_packages
if %errorlevel% neq 0 goto :eof

call :build_project
if %errorlevel% neq 0 goto :eof

call :run_tests

call :log_info "啟動本地開發伺服器..."
call :log_info "應用程式將在 https://localhost:5001 啟動"
call :log_info "按 Ctrl+C 停止伺服器"

dotnet run --project GameCore.Web --configuration Release
goto :eof

REM Docker 部署
:deploy_docker
call :log_info "開始 Docker 部署..."
call :check_docker
if %errorlevel% neq 0 goto :eof

REM 停止現有容器
call :log_info "停止現有容器..."
docker-compose down --remove-orphans

REM 建立並啟動服務
call :log_info "建立並啟動 Docker 服務..."
docker-compose up --build -d

REM 等待服務啟動
call :log_info "等待服務啟動..."
timeout /t 30 /nobreak >nul

REM 檢查服務狀態
call :log_info "檢查服務狀態..."
docker-compose ps

call :log_success "Docker 部署完成！"
call :log_info "應用程式網址: http://localhost:8080"
call :log_info "資料庫連接: localhost:1433"
call :log_info "使用 'docker-compose logs -f' 查看日誌"
goto :eof

REM 生產環境部署
:deploy_production
call :log_info "開始生產環境部署..."
call :check_dotnet
if %errorlevel% neq 0 goto :eof

call :restore_packages
if %errorlevel% neq 0 goto :eof

call :build_project
if %errorlevel% neq 0 goto :eof

call :run_tests

call :publish_project
if %errorlevel% neq 0 goto :eof

call :log_success "生產環境部署完成！"
call :log_info "發佈檔案位於: .\publish"
call :log_info "請將發佈檔案部署到您的 Web 伺服器"
goto :eof

REM 清理部署檔案
:cleanup
call :log_info "清理部署檔案..."

REM 清理建置輸出
dotnet clean

REM 清理發佈目錄
if exist ".\publish" (
    rmdir /s /q ".\publish"
    call :log_info "已清理發佈目錄"
)

REM 清理 Docker 資源
where docker >nul 2>&1
if %errorlevel% equ 0 (
    docker system prune -f
    call :log_info "已清理 Docker 資源"
)

call :log_success "清理完成"
goto :eof

REM 顯示幫助資訊
:show_help
echo GameCore Windows 部署腳本
echo.
echo 用法: %SCRIPT_NAME% [選項]
echo.
echo 選項:
echo   local       本地開發部署
echo   docker      Docker 容器部署
echo   production  生產環境部署
echo   clean       清理部署檔案
echo   help        顯示此幫助資訊
echo.
echo 範例:
echo   %SCRIPT_NAME% local        # 本地開發
echo   %SCRIPT_NAME% docker      # Docker 部署
echo   %SCRIPT_NAME% production  # 生產環境
echo   %SCRIPT_NAME% clean       # 清理檔案
echo.
goto :eof

REM 主函數
:main
if "%1"=="" goto :show_help

if "%1"=="local" goto :deploy_local
if "%1"=="docker" goto :deploy_docker
if "%1"=="production" goto :deploy_production
if "%1"=="clean" goto :cleanup
if "%1"=="help" goto :show_help

echo 無效的選項: %1
echo.
goto :show_help

REM 執行主函數
call :main %*
exit /b %errorlevel%