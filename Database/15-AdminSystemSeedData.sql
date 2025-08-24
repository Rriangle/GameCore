-- =============================================
-- GameCore 管理員系統種子資料
-- 建立完整的管理員、角色權限、後台登入追蹤測試資料
-- 嚴格按照規格要求生成符合業務邏輯的完整管理員系統資料
-- =============================================

USE GameCore;
GO

PRINT '開始插入管理員系統種子資料...';

-- 清除現有的管理員相關記錄 (按照外鍵順序)
PRINT '清除現有管理員資料...';
DELETE FROM Admins;
DELETE FROM ManagerRole;
DELETE FROM ManagerRolePermission WHERE ManagerRole_Id > 0;
DELETE FROM ManagerData WHERE Manager_Id > 0;
DELETE FROM Mutes WHERE mute_id > 0;
DELETE FROM Styles WHERE style_id > 0;

-- 重置自增ID
DBCC CHECKIDENT ('ManagerData', RESEED, 0);
DBCC CHECKIDENT ('ManagerRolePermission', RESEED, 0);
DBCC CHECKIDENT ('Mutes', RESEED, 0);
DBCC CHECKIDENT ('Styles', RESEED, 0);

PRINT '開始生成管理員基礎資料...';

-- 建立管理員資料
INSERT INTO ManagerData (Manager_Name, Manager_Account, Manager_Password, Administrator_registration_date) VALUES
('超級管理員', 'admin001', 'Llh7Usg4JL6y7VDaY4KO8Q==', GETUTCDATE()), -- 密碼: admin123
('使用者管理員', 'useradmin', 'Llh7Usg4JL6y7VDaY4KO8Q==', GETUTCDATE()), -- 密碼: admin123
('商城管理員', 'shopadmin', 'Llh7Usg4JL6y7VDaY4KO8Q==', GETUTCDATE()), -- 密碼: admin123
('論壇管理員', 'forumadmin', 'Llh7Usg4JL6y7VDaY4KO8Q==', GETUTCDATE()), -- 密碼: admin123
('客服管理員', 'csadmin', 'Llh7Usg4JL6y7VDaY4KO8Q==', GETUTCDATE()), -- 密碼: admin123
('銷售管理員', 'salesadmin', 'Llh7Usg4JL6y7VDaY4KO8Q==', GETUTCDATE()), -- 密碼: admin123
('系統監控員', 'monitor001', 'Llh7Usg4JL6y7VDaY4KO8Q==', GETUTCDATE()), -- 密碼: admin123
('內容稽核員', 'auditor001', 'Llh7Usg4JL6y7VDaY4KO8Q==', GETUTCDATE()); -- 密碼: admin123

PRINT '管理員資料生成完成！總計: 8 個管理員';

-- 建立角色權限定義
INSERT INTO ManagerRolePermission (
    role_name, 
    AdministratorPrivilegesManagement, 
    UserStatusManagement, 
    ShoppingPermissionManagement, 
    MessagePermissionManagement, 
    SalesPermissionManagement, 
    customer_service
) VALUES
('超級管理員', 1, 1, 1, 1, 1, 1),
('使用者管理員', 0, 1, 0, 0, 0, 1),
('商城管理員', 0, 0, 1, 0, 1, 1),
('論壇管理員', 0, 0, 0, 1, 0, 1),
('客服專員', 0, 0, 0, 0, 0, 1),
('銷售管理員', 0, 0, 1, 0, 1, 1),
('系統監控員', 0, 1, 1, 1, 1, 1),
('內容稽核員', 0, 1, 0, 1, 0, 1);

PRINT '角色權限定義完成！總計: 8 個角色';

-- 建立管理員角色指派 (ManagerRole)
INSERT INTO ManagerRole (Manager_Id, ManagerRole_Id, ManagerRole) VALUES
-- 超級管理員 (ID=1) 指派所有權限角色
(1, 1, '超級管理員'),

-- 使用者管理員 (ID=2) 專責使用者治理
(2, 2, '使用者管理員'),

-- 商城管理員 (ID=3) 專責商城和銷售
(3, 3, '商城管理員'),
(3, 6, '銷售管理員'),

-- 論壇管理員 (ID=4) 專責論壇內容
(4, 4, '論壇管理員'),

-- 客服管理員 (ID=5) 專責客服和溝通
(5, 5, '客服專員'),

-- 銷售管理員 (ID=6) 專責銷售相關
(6, 6, '銷售管理員'),

-- 系統監控員 (ID=7) 多重權限
(7, 7, '系統監控員'),

-- 內容稽核員 (ID=8) 內容管理
(8, 8, '內容稽核員');

PRINT '管理員角色指派完成！總計: 9 個指派關係';

-- 建立管理員登入追蹤記錄 (Admins)
INSERT INTO Admins (manager_id, last_login) VALUES
(1, DATEADD(MINUTE, -30, GETUTCDATE())), -- 超級管理員 30分鐘前登入
(2, DATEADD(HOUR, -2, GETUTCDATE())), -- 使用者管理員 2小時前登入
(3, DATEADD(HOUR, -1, GETUTCDATE())), -- 商城管理員 1小時前登入
(4, DATEADD(HOUR, -4, GETUTCDATE())), -- 論壇管理員 4小時前登入
(5, DATEADD(HOUR, -6, GETUTCDATE())), -- 客服管理員 6小時前登入
(6, DATEADD(DAY, -1, GETUTCDATE())), -- 銷售管理員 昨天登入
(7, DATEADD(HOUR, -3, GETUTCDATE())), -- 系統監控員 3小時前登入
(8, DATEADD(HOUR, -8, GETUTCDATE())); -- 內容稽核員 8小時前登入

PRINT '管理員登入追蹤記錄完成！總計: 8 個記錄';

-- 建立禁言項目 (Mutes)
INSERT INTO Mutes (mute_name, created_at, is_active, manager_id) VALUES
('廣告洗版', GETUTCDATE(), 1, 4),
('人身攻擊', GETUTCDATE(), 1, 4),
('政治敏感', GETUTCDATE(), 1, 4),
('色情內容', GETUTCDATE(), 1, 4),
('詐騙行為', GETUTCDATE(), 1, 4),
('惡意謾罵', GETUTCDATE(), 1, 4),
('洗版刷屏', GETUTCDATE(), 1, 4),
('惡意引戰', GETUTCDATE(), 1, 4),
('侵犯版權', GETUTCDATE(), 1, 4),
('未成年不當', GETUTCDATE(), 1, 4),
('已停用項目', DATEADD(DAY, -30, GETUTCDATE()), 0, 4);

PRINT '禁言項目生成完成！總計: 11 個項目';

-- 建立樣式項目 (Styles)
INSERT INTO Styles (style_name, effect_desc, created_at, manager_id) VALUES
('新手標籤', '顯示新手玩家身份，增加親和力', GETUTCDATE(), 4),
('VIP會員', '顯示VIP會員專屬金色邊框', GETUTCDATE(), 4),
('活躍用戶', '顯示社群活躍用戶徽章', GETUTCDATE(), 4),
('版主權限', '顯示版主專屬管理標識', GETUTCDATE(), 4),
('官方認證', '顯示官方認證的藍色勾勾', GETUTCDATE(), 4),
('遊戲達人', '顯示遊戲專精達人稱號', GETUTCDATE(), 4),
('購物達人', '顯示商城購物達人標籤', GETUTCDATE(), 4),
('貢獻者', '顯示社群貢獻者專屬樣式', GETUTCDATE(), 4),
('創作者', '顯示內容創作者特殊邊框', GETUTCDATE(), 4),
('問題解答王', '顯示熱心解答問題的標識', GETUTCDATE(), 4);

PRINT '樣式項目生成完成！總計: 10 個項目';

-- 生成模擬操作日誌資料 (應用層實現，此處僅註解說明)
/*
操作日誌將記錄以下類型的管理操作：
1. CREATE - 建立資源 (管理員、角色、使用者權限調整等)
2. UPDATE - 更新資源 (管理員資料、角色權限、系統設定等)  
3. DELETE - 刪除資源 (停用管理員、移除角色等)
4. LOGIN - 登入操作 (管理員登入追蹤)
5. AUDIT - 審核操作 (內容稽核、使用者權限審核等)
6. CONFIG - 設定變更 (系統參數調整、權重設定等)

每筆日誌包含：
- 操作類型 (operation_type)
- 操作模組 (module)
- 操作者ID (operator_id)
- 目標資源 (target_resource)
- 目標ID (target_id)
- 操作描述 (description)
- 操作詳情JSON (details)
- 操作結果 (result)
- IP位址 (ip_address)
- 用戶代理 (user_agent)
- 操作時間 (created_at)
*/

-- 統計報告
PRINT '=== 管理員系統種子資料統計報告 ===';

-- 管理員統計
SELECT 
    COUNT(*) as 總管理員數,
    COUNT(CASE WHEN Administrator_registration_date >= DATEADD(DAY, -7, GETUTCDATE()) THEN 1 END) as 本週新增管理員
FROM ManagerData;

-- 角色權限統計
SELECT 
    role_name as 角色名稱,
    CASE WHEN AdministratorPrivilegesManagement = 1 THEN '✓' ELSE '✗' END as 管理者權限,
    CASE WHEN UserStatusManagement = 1 THEN '✓' ELSE '✗' END as 用戶管理,
    CASE WHEN ShoppingPermissionManagement = 1 THEN '✓' ELSE '✗' END as 商城管理,
    CASE WHEN MessagePermissionManagement = 1 THEN '✓' ELSE '✗' END as 論壇管理,
    CASE WHEN SalesPermissionManagement = 1 THEN '✓' ELSE '✗' END as 銷售管理,
    CASE WHEN customer_service = 1 THEN '✓' ELSE '✗' END as 客服權限
FROM ManagerRolePermission
ORDER BY ManagerRole_Id;

-- 角色指派統計
SELECT 
    mrp.role_name as 角色名稱,
    COUNT(mr.Manager_Id) as 指派管理員數
FROM ManagerRolePermission mrp
LEFT JOIN ManagerRole mr ON mrp.ManagerRole_Id = mr.ManagerRole_Id
GROUP BY mrp.role_name, mrp.ManagerRole_Id
ORDER BY COUNT(mr.Manager_Id) DESC;

-- 登入活躍度統計
SELECT 
    CASE 
        WHEN last_login >= DATEADD(HOUR, -1, GETUTCDATE()) THEN '1小時內'
        WHEN last_login >= DATEADD(HOUR, -24, GETUTCDATE()) THEN '24小時內'
        WHEN last_login >= DATEADD(DAY, -7, GETUTCDATE()) THEN '一週內'
        ELSE '超過一週'
    END as 登入時間範圍,
    COUNT(*) as 管理員數量
FROM Admins
GROUP BY 
    CASE 
        WHEN last_login >= DATEADD(HOUR, -1, GETUTCDATE()) THEN '1小時內'
        WHEN last_login >= DATEADD(HOUR, -24, GETUTCDATE()) THEN '24小時內'
        WHEN last_login >= DATEADD(DAY, -7, GETUTCDATE()) THEN '一週內'
        ELSE '超過一週'
    END
ORDER BY 
    CASE 
        WHEN CASE 
            WHEN last_login >= DATEADD(HOUR, -1, GETUTCDATE()) THEN '1小時內'
            WHEN last_login >= DATEADD(HOUR, -24, GETUTCDATE()) THEN '24小時內'
            WHEN last_login >= DATEADD(DAY, -7, GETUTCDATE()) THEN '一週內'
            ELSE '超過一週'
        END = '1小時內' THEN 1
        WHEN CASE 
            WHEN last_login >= DATEADD(HOUR, -1, GETUTCDATE()) THEN '1小時內'
            WHEN last_login >= DATEADD(HOUR, -24, GETUTCDATE()) THEN '24小時內'
            WHEN last_login >= DATEADD(DAY, -7, GETUTCDATE()) THEN '一週內'
            ELSE '超過一週'
        END = '24小時內' THEN 2
        WHEN CASE 
            WHEN last_login >= DATEADD(HOUR, -1, GETUTCDATE()) THEN '1小時內'
            WHEN last_login >= DATEADD(HOUR, -24, GETUTCDATE()) THEN '24小時內'
            WHEN last_login >= DATEADD(DAY, -7, GETUTCDATE()) THEN '一週內'
            ELSE '超過一週'
        END = '一週內' THEN 3
        ELSE 4
    END;

-- 禁言和樣式統計
SELECT 
    COUNT(CASE WHEN is_active = 1 THEN 1 END) as 啟用禁言項目,
    COUNT(CASE WHEN is_active = 0 THEN 1 END) as 停用禁言項目
FROM Mutes;

SELECT COUNT(*) as 總樣式項目數 FROM Styles;

-- 權限覆蓋度分析
PRINT '權限覆蓋度分析:';
SELECT 
    '管理者權限管理' as 權限類型,
    COUNT(CASE WHEN AdministratorPrivilegesManagement = 1 THEN 1 END) as 具備此權限的角色數
FROM ManagerRolePermission
UNION ALL
SELECT 
    '用戶狀態管理',
    COUNT(CASE WHEN UserStatusManagement = 1 THEN 1 END)
FROM ManagerRolePermission
UNION ALL
SELECT 
    '商城權限管理',
    COUNT(CASE WHEN ShoppingPermissionManagement = 1 THEN 1 END)
FROM ManagerRolePermission
UNION ALL
SELECT 
    '論壇權限管理',
    COUNT(CASE WHEN MessagePermissionManagement = 1 THEN 1 END)
FROM ManagerRolePermission
UNION ALL
SELECT 
    '銷售權限管理',
    COUNT(CASE WHEN SalesPermissionManagement = 1 THEN 1 END)
FROM ManagerRolePermission
UNION ALL
SELECT 
    '客服權限管理',
    COUNT(CASE WHEN customer_service = 1 THEN 1 END)
FROM ManagerRolePermission;

-- 最活躍管理員Top 5
PRINT '最近活躍的管理員 TOP 5:';
SELECT TOP 5
    md.Manager_Name as 管理員名稱,
    md.Manager_Account as 管理員帳號,
    a.last_login as 最後登入時間,
    DATEDIFF(MINUTE, a.last_login, GETUTCDATE()) as 多少分鐘前,
    STRING_AGG(mrp.role_name, ', ') as 角色列表
FROM ManagerData md
INNER JOIN Admins a ON md.Manager_Id = a.manager_id
LEFT JOIN ManagerRole mr ON md.Manager_Id = mr.Manager_Id
LEFT JOIN ManagerRolePermission mrp ON mr.ManagerRole_Id = mrp.ManagerRole_Id
GROUP BY md.Manager_Id, md.Manager_Name, md.Manager_Account, a.last_login
ORDER BY a.last_login DESC;

PRINT '管理員系統種子資料插入完成！';
GO