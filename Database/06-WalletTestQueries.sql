-- =============================================
-- GameCore 錢包系統測試查詢
-- 用於驗證錢包功能和資料完整性
-- =============================================

USE GameCore;
GO

PRINT '=== GameCore 錢包系統測試查詢 ===';

-- 1. 測試基本錢包功能
PRINT '1. 基本錢包功能測試';
SELECT TOP 5
    u.User_name as 使用者名稱,
    uw.User_Point as 點數餘額,
    uw.Coupon_Number as 優惠券編號,
    ur.SalesAuthority as 銷售權限
FROM Users u
LEFT JOIN User_wallet uw ON u.User_ID = uw.User_Id
LEFT JOIN User_Rights ur ON u.User_ID = ur.User_Id
ORDER BY uw.User_Point DESC;

-- 2. 測試收支明細彙整 (模擬錢包服務的明細查詢)
PRINT '2. 收支明細彙整測試 - 使用者ID 3';
DECLARE @TestUserId INT = 3;

-- 簽到記錄
SELECT '簽到' as 交易類型, 
       SignTime as 交易時間,
       PointsChanged as 點數變化,
       '每日簽到獲得點數' as 描述
FROM UserSignInStats 
WHERE UserID = @TestUserId AND PointsChanged != 0

UNION ALL

-- 小遊戲記錄
SELECT '小遊戲' as 交易類型,
       StartTime as 交易時間,
       PointsChanged as 點數變化,
       CASE Result 
           WHEN 'Win' THEN '冒險勝利獲得點數'
           WHEN 'Lose' THEN '冒險失敗扣除點數'
           ELSE '冒險中途退出'
       END as 描述
FROM MiniGame 
WHERE UserID = @TestUserId AND PointsChanged != 0

UNION ALL

-- 寵物換色記錄
SELECT '寵物換色' as 交易類型,
       PointsChangedTime as 交易時間,
       -ABS(PointsChanged) as 點數變化, -- 換色是消費，顯示為負數
       '寵物換色消費點數' as 描述
FROM Pet 
WHERE UserID = @TestUserId AND PointsChanged != 0 AND PointsChangedTime IS NOT NULL

ORDER BY 交易時間 DESC;

-- 3. 測試銷售功能
PRINT '3. 銷售功能測試';
SELECT 
    u.User_name as 使用者名稱,
    msp.BankCode as 銀行代號,
    msp.BankAccountNumber as 銀行帳號,
    ur.SalesAuthority as 銷售權限狀態,
    usi.UserSales_Wallet as 銷售錢包餘額
FROM Users u
JOIN MemberSalesProfile msp ON u.User_ID = msp.User_Id
LEFT JOIN User_Rights ur ON u.User_ID = ur.User_Id  
LEFT JOIN User_Sales_Information usi ON u.User_ID = usi.User_Id
ORDER BY usi.UserSales_Wallet DESC;

-- 4. 測試點數統計計算
PRINT '4. 點數統計計算測試 - 使用者ID 5';
SET @TestUserId = 5;

-- 計算各類型收入
SELECT 
    '簽到收入' as 收入類型,
    ISNULL(SUM(PointsChanged), 0) as 總點數
FROM UserSignInStats 
WHERE UserID = @TestUserId AND PointsChanged > 0

UNION ALL

SELECT 
    '遊戲勝利收入' as 收入類型,
    ISNULL(SUM(PointsChanged), 0) as 總點數
FROM MiniGame 
WHERE UserID = @TestUserId AND PointsChanged > 0

UNION ALL

-- 計算各類型支出
SELECT 
    '遊戲失敗支出' as 收入類型,
    ISNULL(SUM(ABS(PointsChanged)), 0) as 總點數
FROM MiniGame 
WHERE UserID = @TestUserId AND PointsChanged < 0

UNION ALL

SELECT 
    '寵物換色支出' as 收入類型,
    ISNULL(SUM(PointsChanged), 0) as 總點數
FROM Pet 
WHERE UserID = @TestUserId AND PointsChanged > 0;

-- 5. 測試錢包餘額計算正確性
PRINT '5. 錢包餘額計算正確性驗證';
WITH UserTransactionSummary AS (
    -- 計算每個使用者的收支總額
    SELECT 
        u.User_ID,
        u.User_name,
        uw.User_Point as 當前餘額,
        ISNULL(income.total_income, 0) as 總收入,
        ISNULL(expense.total_expense, 0) as 總支出,
        ISNULL(income.total_income, 0) - ISNULL(expense.total_expense, 0) as 計算餘額
    FROM Users u
    LEFT JOIN User_wallet uw ON u.User_ID = uw.User_Id
    LEFT JOIN (
        -- 計算總收入 (簽到 + 遊戲勝利)
        SELECT UserID as User_ID, SUM(PointsChanged) as total_income
        FROM (
            SELECT UserID, PointsChanged FROM UserSignInStats WHERE PointsChanged > 0
            UNION ALL
            SELECT UserID, PointsChanged FROM MiniGame WHERE PointsChanged > 0
        ) income_sources
        GROUP BY UserID
    ) income ON u.User_ID = income.User_ID
    LEFT JOIN (
        -- 計算總支出 (寵物換色 + 遊戲失敗)
        SELECT User_ID, SUM(expense_amount) as total_expense
        FROM (
            SELECT UserID as User_ID, ABS(PointsChanged) as expense_amount 
            FROM MiniGame 
            WHERE PointsChanged < 0
            UNION ALL
            SELECT UserID as User_ID, PointsChanged as expense_amount 
            FROM Pet 
            WHERE PointsChanged > 0
        ) expense_sources
        GROUP BY User_ID
    ) expense ON u.User_ID = expense.User_ID
)
SELECT TOP 10
    User_name as 使用者名稱,
    當前餘額,
    總收入,
    總支出,
    計算餘額,
    CASE 
        WHEN ABS(當前餘額 - 計算餘額) <= 1 THEN '正確'
        ELSE '需檢查'
    END as 餘額狀態
FROM UserTransactionSummary
WHERE 當前餘額 IS NOT NULL
ORDER BY 總收入 DESC;

-- 6. 測試通知系統整合
PRINT '6. 通知系統整合測試 - 點數調整通知';
SELECT TOP 5
    u.User_name as 使用者名稱,
    n.notification_title as 通知標題,
    n.notification_message as 通知內容,
    n.created_at as 建立時間,
    nr.is_read as 是否已讀
FROM Notifications n
JOIN Notification_Recipients nr ON n.notification_id = nr.notification_id
JOIN Users u ON nr.user_id = u.User_ID
JOIN Notification_Actions na ON n.action_id = na.action_id
WHERE na.action_name = 'points_adjustment'
ORDER BY n.created_at DESC;

-- 7. 測試資料完整性
PRINT '7. 資料完整性檢查';

-- 檢查是否所有使用者都有錢包
SELECT 
    '缺少錢包的使用者數' as 檢查項目,
    COUNT(*) as 數量
FROM Users u
LEFT JOIN User_wallet uw ON u.User_ID = uw.User_Id
WHERE uw.User_Id IS NULL

UNION ALL

-- 檢查是否所有有銷售檔案的使用者都有銷售錢包
SELECT 
    '有銷售檔案但無銷售錢包的使用者數' as 檢查項目,
    COUNT(*) as 數量
FROM MemberSalesProfile msp
LEFT JOIN User_Sales_Information usi ON msp.User_Id = usi.User_Id
WHERE usi.User_Id IS NULL

UNION ALL

-- 檢查錢包餘額為負數的使用者
SELECT 
    '錢包餘額為負數的使用者數' as 檢查項目,
    COUNT(*) as 數量
FROM User_wallet
WHERE User_Point < 0;

-- 8. 效能測試查詢 (模擬API查詢)
PRINT '8. 效能測試 - 模擬收支明細API查詢';
SET STATISTICS TIME ON;

-- 模擬分頁查詢收支明細
DECLARE @PageSize INT = 20;
DECLARE @PageNumber INT = 1;
DECLARE @Offset INT = (@PageNumber - 1) * @PageSize;

WITH CombinedTransactions AS (
    SELECT 
        'signin_' + CAST(LogID AS VARCHAR) as TransactionId,
        SignTime as TransactionTime,
        'signin' as TransactionType,
        PointsChanged as PointsDelta,
        '每日簽到獲得 ' + CAST(PointsChanged AS VARCHAR) + ' 點數' as Description,
        UserID
    FROM UserSignInStats 
    WHERE PointsChanged != 0
    
    UNION ALL
    
    SELECT 
        'minigame_' + CAST(PlayID AS VARCHAR) as TransactionId,
        StartTime as TransactionTime,
        'minigame' as TransactionType,
        PointsChanged as PointsDelta,
        CASE Result 
            WHEN 'Win' THEN '冒險勝利獲得 ' + CAST(PointsChanged AS VARCHAR) + ' 點數'
            WHEN 'Lose' THEN '冒險失敗失去 ' + CAST(ABS(PointsChanged) AS VARCHAR) + ' 點數'
            ELSE '冒險中途退出'
        END as Description,
        UserID
    FROM MiniGame 
    WHERE PointsChanged != 0
    
    UNION ALL
    
    SELECT 
        'pet_color_' + CAST(PetID AS VARCHAR) as TransactionId,
        PointsChangedTime as TransactionTime,
        'pet_color' as TransactionType,
        -ABS(PointsChanged) as PointsDelta,
        '寵物換色消費 ' + CAST(PointsChanged AS VARCHAR) + ' 點數' as Description,
        UserID
    FROM Pet 
    WHERE PointsChanged != 0 AND PointsChangedTime IS NOT NULL
)
SELECT 
    TransactionId,
    TransactionTime,
    TransactionType,
    PointsDelta,
    Description
FROM CombinedTransactions
WHERE UserID = 3 -- 測試使用者ID 3
ORDER BY TransactionTime DESC
OFFSET @Offset ROWS
FETCH NEXT @PageSize ROWS ONLY;

SET STATISTICS TIME OFF;

PRINT '錢包系統測試查詢完成！';

-- 顯示系統總覽統計
PRINT '=== 系統總覽統計 ===';
SELECT 
    '總使用者數' as 統計項目,
    COUNT(*) as 數值
FROM Users

UNION ALL

SELECT 
    '擁有錢包的使用者數' as 統計項目,
    COUNT(*) as 數值
FROM User_wallet

UNION ALL

SELECT 
    '擁有銷售權限的使用者數' as 統計項目,
    COUNT(*) as 數值
FROM User_Rights 
WHERE SalesAuthority = 1

UNION ALL

SELECT 
    '銷售檔案總數' as 統計項目,
    COUNT(*) as 數值
FROM MemberSalesProfile

UNION ALL

SELECT 
    '簽到記錄總數' as 統計項目,
    COUNT(*) as 數值
FROM UserSignInStats

UNION ALL

SELECT 
    '小遊戲記錄總數' as 統計項目,
    COUNT(*) as 數值
FROM MiniGame

UNION ALL

SELECT 
    '總點數發行量' as 統計項目,
    SUM(User_Point) as 數值
FROM User_wallet;

GO