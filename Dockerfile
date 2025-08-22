# GameCore 遊戲社群平台 Docker 配置
# 使用多階段建置來優化映像大小

# ===== 建置階段 =====
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# 複製專案檔案並還原套件
COPY ["GameCore.Web/GameCore.Web.csproj", "GameCore.Web/"]
COPY ["GameCore.Core/GameCore.Core.csproj", "GameCore.Core/"]
COPY ["GameCore.Infrastructure/GameCore.Infrastructure.csproj", "GameCore.Infrastructure/"]
COPY ["GameCore.Tests/GameCore.Tests.csproj", "GameCore.Tests/"]

# 還原 NuGet 套件
RUN dotnet restore "GameCore.Web/GameCore.Web.csproj"

# 複製所有原始碼
COPY . .

# 建置專案
WORKDIR "/src/GameCore.Web"
RUN dotnet build "GameCore.Web.csproj" -c Release -o /app/build

# ===== 發佈階段 =====
FROM build AS publish
RUN dotnet publish "GameCore.Web.csproj" -c Release -o /app/publish /p:UseAppHost=false

# ===== 執行時階段 =====
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

# 安裝必要的套件
RUN apt-get update && apt-get install -y \
    curl \
    && rm -rf /var/lib/apt/lists/*

# 建立非 root 使用者
RUN adduser --disabled-password --gecos '' appuser && chown -R appuser /app
USER appuser

# 複製發佈檔案
COPY --from=publish /app/publish .

# 設定環境變數
ENV ASPNETCORE_ENVIRONMENT=Production
ENV ASPNETCORE_URLS=http://+:80

# 暴露連接埠
EXPOSE 80

# 健康檢查
HEALTHCHECK --interval=30s --timeout=3s --start-period=5s --retries=3 \
    CMD curl -f http://localhost/health || exit 1

# 設定入口點
ENTRYPOINT ["dotnet", "GameCore.Web.dll"]

# 標籤資訊
LABEL maintainer="GameCore Team <contact@gamecore.com>"
LABEL version="1.0"
LABEL description="GameCore 遊戲社群平台 - 整合寵物養成、論壇社群、商城系統的綜合平台"