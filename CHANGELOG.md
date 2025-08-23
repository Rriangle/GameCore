# GameCore 專案變更日誌

所有重要的變更都會記錄在此文件中。

格式基於 [Keep a Changelog](https://keepachangelog.com/zh-TW/1.0.0/)，
並且此專案遵循 [Semantic Versioning](https://semver.org/lang/zh-TW/)。

## [未發布] - 2024-12-20

### 🚀 新增功能
- 創建完整的專案架構 (Core, Infrastructure, Web, Tests)
- 實現完整的資料庫 Schema 和 Entity Framework 映射
- 創建商城系統前端頁面 (Store, Product, Cart, Checkout, OrderConfirmation)
- 創建玩家市場前端頁面 (PlayerMarket)
- 創建論壇系統前端頁面 (Forum, Posts, Post, EditPost, CreatePost)
- 創建聊天系統前端頁面 (Chat)
- 創建通知系統前端頁面 (Notification)
- 創建管理系統前端頁面 (Manager)
- 配置 GitHub Actions CI/CD 流程
- 創建 Docker 容器化配置
- 創建啟動腳本 (Windows 和 Linux/macOS)

### 🔧 修復問題
- 修復重複的 Column 屬性定義
- 修復 Thread 類別名稱衝突
- 修復 HasCheckConstraint 用法錯誤
- 添加缺失的 NuGet 套件引用
- 修復實體類別重複定義問題

### 📚 文件更新
- 創建完整的 README.md 使用說明
- 創建 PROGRESS.md 進度報告
- 創建 ARCH-GAP-ANALYSIS.md 架構差異分析
- 創建 DIFF-PLAN.md 修補計畫
- 創建 PR_DESCRIPTION.md PR 描述

### 🏗 架構改進
- 實現分層架構 (Core, Infrastructure, Web, Tests)
- 配置依賴注入和服務註冊
- 實現基礎 Repository 模式
- 配置 Entity Framework Core 8.0
- 實現基礎認證和授權系統

### 🐳 DevOps
- 配置 GitHub Actions 自動化建置和測試
- 創建多階段 Dockerfile
- 配置 Docker Compose 多服務部署
- 實現健康檢查和監控

---

## [0.1.0] - 2024-12-20

### 🎯 初始版本
- 專案基礎架構建立
- 核心實體類別定義
- 基礎資料庫映射
- 前端頁面框架

---

## 變更類型說明

- **🚀 新增功能**: 新功能或新特性
- **🔧 修復問題**: 錯誤修復
- **📚 文件更新**: 文件或註釋的變更
- **🏗 架構改進**: 程式碼重構或架構改進
- **🐳 DevOps**: 部署、CI/CD 或基礎設施變更
- **⚠️ 破壞性變更**: 可能影響現有功能的變更
- **🔒 安全性**: 安全性相關的修復或改進

---

## 版本號說明

- **主版本號**: 不兼容的 API 變更
- **次版本號**: 向後兼容的功能新增
- **修訂版本號**: 向後兼容的問題修復

---

**維護者**: GameCore 專案團隊  
**最後更新**: 2024-12-20