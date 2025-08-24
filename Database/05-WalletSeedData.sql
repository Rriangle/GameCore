-- =============================================
-- GameCore 錢包系統種子資料
-- 為錢包、銷售功能和交易記錄創建測試資料
-- 確保所有使用者都有完整的錢包系統資料
-- =============================================

USE GameCore;
GO

PRINT '開始插入錢包系統種子資料...';

-- 確保所有現有使用者都有錢包
PRINT '為所有使用者建立錢包...';
INSERT INTO User_wallet (User_Id, User_Point, Coupon_Number)
SELECT u.User_ID, 
       CASE 
           WHEN u.User_ID <= 10 THEN FLOOR(RAND(CHECKSUM(NEWID())) * 5000) + 1000  -- VIP用戶有更多點數
           ELSE FLOOR(RAND(CHECKSUM(NEWID())) * 2000) + 500                         -- 一般用戶
       END as User_Point,
       CASE 
           WHEN FLOOR(RAND(CHECKSUM(NEWID())) * 10) = 0 THEN 'COUPON' + RIGHT('000' + CAST(u.User_ID as VARCHAR), 4)
           ELSE NULL
       END as Coupon_Number
FROM Users u
LEFT JOIN User_wallet uw ON u.User_ID = uw.User_Id
WHERE uw.User_Id IS NULL;

PRINT '已為 ' + CAST(@@ROWCOUNT AS VARCHAR) + ' 位使用者建立錢包';

-- 為部分使用者建立銷售檔案
PRINT '建立銷售檔案資料...';
INSERT INTO MemberSalesProfile (User_Id, BankCode, BankAccountNumber, AccountCoverPhoto)
VALUES 
    (3, 1, '12345678901234', NULL),    -- 台灣銀行
    (5, 4, '98765432109876', NULL),    -- 合作金庫
    (7, 6, '11111111111111', NULL),    -- 華南銀行
    (9, 11, '22222222222222', NULL),   -- 台北富邦
    (11, 12, '33333333333333', NULL),  -- 台新銀行
    (13, 3, '44444444444444', NULL),   -- 土地銀行
    (15, 5, '55555555555555', NULL),   -- 第一銀行
    (17, 7, '66666666666666', NULL),   -- 彰化銀行
    (19, 1, '77777777777777', NULL),   -- 台灣銀行
    (21, 4, '88888888888888', NULL);   -- 合作金庫

PRINT '已建立 ' + CAST(@@ROWCOUNT AS VARCHAR) + ' 筆銷售檔案';

-- 為有銷售檔案的使用者啟用銷售權限
PRINT '啟用銷售權限...';
UPDATE User_Rights 
SET SalesAuthority = 1
WHERE User_Id IN (3, 5, 7, 9, 11, 13, 15, 17, 19, 21);

PRINT '已為 ' + CAST(@@ROWCOUNT AS VARCHAR) + ' 位使用者啟用銷售權限';

-- 建立銷售錢包資料
PRINT '建立銷售錢包資料...';
INSERT INTO User_Sales_Information (User_Id, UserSales_Wallet)
SELECT msp.User_Id, 
       FLOOR(RAND(CHECKSUM(NEWID())) * 50000) + 10000 as UserSales_Wallet
FROM MemberSalesProfile msp;

PRINT '已建立 ' + CAST(@@ROWCOUNT AS VARCHAR) + ' 筆銷售錢包';

-- 建立更多簽到記錄來豐富交易明細
PRINT '建立額外簽到記錄...';
DECLARE @UserId INT = 1;
DECLARE @MaxUserId INT = (SELECT MAX(User_ID) FROM Users);

WHILE @UserId <= @MaxUserId
BEGIN
    -- 為每個使用者建立過去30天的隨機簽到記錄
    DECLARE @DaysBack INT = 0;
    WHILE @DaysBack < 30
    BEGIN
        -- 70%機率有簽到記錄
        IF RAND(CHECKSUM(NEWID())) < 0.7
        BEGIN
            DECLARE @SignInDate DATETIME2 = DATEADD(DAY, -@DaysBack, GETUTCDATE());
            DECLARE @IsWeekend BIT = CASE WHEN DATEPART(WEEKDAY, @SignInDate) IN (1, 7) THEN 1 ELSE 0 END;
            DECLARE @BasePoints INT = CASE WHEN @IsWeekend = 1 THEN 30 ELSE 20 END;
            DECLARE @BonusPoints INT = 0;
            DECLARE @ExpGained INT = CASE WHEN @IsWeekend = 1 THEN 200 ELSE 0 END;
            
            -- 隨機連續簽到獎勵
            IF @DaysBack % 7 = 0 AND @DaysBack > 0 AND RAND(CHECKSUM(NEWID())) < 0.3
            BEGIN
                SET @BonusPoints = 40;
                SET @ExpGained = @ExpGained + 300;
            END
            
            -- 插入簽到記錄
            INSERT INTO UserSignInStats (SignTime, UserID, PointsChanged, ExpGained, PointsChangedTime, ExpGainedTime)
            VALUES (
                @SignInDate,
                @UserId,
                @BasePoints + @BonusPoints,
                @ExpGained,
                @SignInDate,
                @SignInDate
            );
        END
        
        SET @DaysBack = @DaysBack + 1;
    END
    
    SET @UserId = @UserId + 1;
END

PRINT '已建立額外簽到記錄';

-- 建立小遊戲記錄
PRINT '建立小遊戲記錄...';
SET @UserId = 1;
WHILE @UserId <= @MaxUserId
BEGIN
    -- 為每個使用者建立隨機小遊戲記錄
    DECLARE @GameCount INT = FLOOR(RAND(CHECKSUM(NEWID())) * 20) + 5; -- 5-25場遊戲
    DECLARE @GameIndex INT = 0;
    
    WHILE @GameIndex < @GameCount
    BEGIN
        DECLARE @GameDate DATETIME2 = DATEADD(
            MINUTE, 
            -FLOOR(RAND(CHECKSUM(NEWID())) * 43200), -- 過去30天隨機時間
            GETUTCDATE()
        );
        
        DECLARE @GameLevel INT = FLOOR(RAND(CHECKSUM(NEWID())) * 3) + 1; -- 1-3級
        DECLARE @MonsterCount INT = CASE @GameLevel WHEN 1 THEN 6 WHEN 2 THEN 8 ELSE 10 END;
        DECLARE @SpeedMultiplier DECIMAL(5,2) = 1.0 + (@GameLevel - 1) * 0.2;
        DECLARE @GameResult NVARCHAR(10) = CASE 
            WHEN RAND(CHECKSUM(NEWID())) < 0.6 THEN 'Win'  -- 60%勝率
            WHEN RAND(CHECKSUM(NEWID())) < 0.9 THEN 'Lose' -- 30%敗率
            ELSE 'Abort' -- 10%中途退出
        END;
        
        DECLARE @PointsChanged INT = 0;
        DECLARE @ExpGained INT = 0;
        
        IF @GameResult = 'Win'
        BEGIN
            SET @PointsChanged = @GameLevel * 10 + FLOOR(RAND(CHECKSUM(NEWID())) * 20);
            SET @ExpGained = @GameLevel * 100 + FLOOR(RAND(CHECKSUM(NEWID())) * 100);
        END
        ELSE IF @GameResult = 'Lose'
        BEGIN
            SET @PointsChanged = -FLOOR(RAND(CHECKSUM(NEWID())) * 10) - 5; -- 輸了扣點數
            SET @ExpGained = FLOOR(RAND(CHECKSUM(NEWID())) * 50); -- 還是有少量經驗
        END
        
        -- 插入遊戲記錄
        INSERT INTO MiniGame (
            UserID, PetID, Level, MonsterCount, SpeedMultiplier, Result,
            ExpGained, ExpGainedTime, PointsChanged, PointsChangedTime,
            HungerDelta, MoodDelta, StaminaDelta, CleanlinessDelta,
            StartTime, EndTime, Aborted
        )
        VALUES (
            @UserId,
            @UserId, -- 假設每個使用者都有對應的寵物
            @GameLevel,
            @MonsterCount,
            @SpeedMultiplier,
            @GameResult,
            @ExpGained,
            CASE WHEN @GameResult != 'Abort' THEN @GameDate ELSE NULL END,
            @PointsChanged,
            CASE WHEN @GameResult != 'Abort' THEN @GameDate ELSE NULL END,
            -20, -- 飢餓值下降
            CASE WHEN @GameResult = 'Win' THEN 30 ELSE -30 END, -- 心情變化
            -20, -- 體力下降
            -20, -- 清潔度下降
            @GameDate,
            CASE WHEN @GameResult != 'Abort' THEN DATEADD(MINUTE, FLOOR(RAND(CHECKSUM(NEWID())) * 15) + 5, @GameDate) ELSE NULL END,
            CASE WHEN @GameResult = 'Abort' THEN 1 ELSE 0 END
        );
        
        SET @GameIndex = @GameIndex + 1;
    END
    
    SET @UserId = @UserId + 1;
END

PRINT '已建立小遊戲記錄';

-- 為部分寵物建立換色記錄
PRINT '建立寵物換色記錄...';
UPDATE Pet 
SET PointsChanged = 2000,
    PointsChangedTime = DATEADD(DAY, -FLOOR(RAND(CHECKSUM(NEWID())) * 15), GETUTCDATE()),
    SkinColor = CASE FLOOR(RAND(CHECKSUM(NEWID())) * 5)
        WHEN 0 THEN '#FF6B6B'  -- 紅色
        WHEN 1 THEN '#4ECDC4'  -- 青綠色
        WHEN 2 THEN '#45B7D1'  -- 藍色
        WHEN 3 THEN '#96CEB4'  -- 淺綠色
        ELSE '#FECA57'         -- 黃色
    END,
    ColorChangedTime = DATEADD(DAY, -FLOOR(RAND(CHECKSUM(NEWID())) * 15), GETUTCDATE())
WHERE UserID % 3 = 0; -- 每三個使用者就有一個換過色

PRINT '已為 ' + CAST(@@ROWCOUNT AS VARCHAR) + ' 隻寵物建立換色記錄';

-- 建立管理員點數調整通知記錄 (模擬管理員調整點數的通知)
PRINT '建立管理員調整記錄...';

-- 先確保有點數調整的行為類型
IF NOT EXISTS (SELECT 1 FROM Notification_Actions WHERE action_name = 'points_adjustment')
BEGIN
    INSERT INTO Notification_Actions (action_name) VALUES ('points_adjustment');
END

-- 建立系統來源
IF NOT EXISTS (SELECT 1 FROM Notification_Sources WHERE source_name = 'system')
BEGIN
    INSERT INTO Notification_Sources (source_name) VALUES ('system');
END

DECLARE @SystemSourceId INT = (SELECT source_id FROM Notification_Sources WHERE source_name = 'system');
DECLARE @AdjustmentActionId INT = (SELECT action_id FROM Notification_Actions WHERE action_name = 'points_adjustment');

-- 為部分使用者建立點數調整通知
DECLARE @AdjustmentUserId INT = 2;
WHILE @AdjustmentUserId <= 20
BEGIN
    DECLARE @AdjustmentAmount INT = (FLOOR(RAND(CHECKSUM(NEWID())) * 2000) - 1000); -- -1000 到 +1000
    DECLARE @AdjustmentReason NVARCHAR(MAX) = CASE 
        WHEN @AdjustmentAmount > 0 THEN '活動獎勵加碼 +' + CAST(@AdjustmentAmount AS VARCHAR) + ' 點'
        ELSE '違規處罰扣點 ' + CAST(@AdjustmentAmount AS VARCHAR) + ' 點'
    END;
    
    -- 建立通知
    INSERT INTO Notifications (
        source_id, action_id, sender_id, sender_manager_id,
        notification_title, notification_message, created_at
    )
    VALUES (
        @SystemSourceId,
        @AdjustmentActionId,
        @AdjustmentUserId,
        1, -- 假設管理員ID為1
        '點數調整通知',
        @AdjustmentReason,
        DATEADD(DAY, -FLOOR(RAND(CHECKSUM(NEWID())) * 10), GETUTCDATE())
    );
    
    -- 建立通知接收者記錄
    INSERT INTO Notification_Recipients (
        notification_id, user_id, is_read, read_at
    )
    VALUES (
        SCOPE_IDENTITY(),
        @AdjustmentUserId,
        CASE WHEN RAND(CHECKSUM(NEWID())) < 0.7 THEN 1 ELSE 0 END, -- 70%已讀
        CASE WHEN RAND(CHECKSUM(NEWID())) < 0.7 THEN DATEADD(HOUR, FLOOR(RAND(CHECKSUM(NEWID())) * 24), GETUTCDATE()) ELSE NULL END
    );
    
    SET @AdjustmentUserId = @AdjustmentUserId + 3; -- 每3個使用者一個調整記錄
END

PRINT '已建立管理員調整記錄';

-- 更新使用者錢包餘額以反映所有交易
PRINT '重新計算使用者錢包餘額...';
UPDATE uw
SET User_Point = ISNULL(earned.total_earned, 0) - ISNULL(spent.total_spent, 0)
FROM User_wallet uw
LEFT JOIN (
    -- 計算總獲得點數 (簽到 + 遊戲勝利)
    SELECT UserID as User_Id, SUM(PointsChanged) as total_earned
    FROM (
        SELECT UserID, PointsChanged FROM UserSignInStats WHERE PointsChanged > 0
        UNION ALL
        SELECT UserID, PointsChanged FROM MiniGame WHERE PointsChanged > 0
    ) earned_points
    GROUP BY UserID
) earned ON uw.User_Id = earned.User_Id
LEFT JOIN (
    -- 計算總消費點數 (寵物換色 + 遊戲失敗)
    SELECT User_Id, SUM(spent_amount) as total_spent
    FROM (
        SELECT UserID as User_Id, ABS(PointsChanged) as spent_amount 
        FROM MiniGame 
        WHERE PointsChanged < 0
        UNION ALL
        SELECT UserID as User_Id, ABS(PointsChanged) as spent_amount 
        FROM Pet 
        WHERE PointsChanged > 0 -- 寵物換色是消費，但記錄為正數
    ) spent_points
    GROUP BY User_Id
) spent ON uw.User_Id = spent.User_Id;

-- 確保餘額不為負數
UPDATE User_wallet SET User_Point = 0 WHERE User_Point < 0;

PRINT '已重新計算使用者錢包餘額';

-- 統計報告
PRINT '=== 錢包系統種子資料統計報告 ===';
PRINT '總使用者數: ' + CAST((SELECT COUNT(*) FROM Users) AS VARCHAR);
PRINT '擁有錢包的使用者: ' + CAST((SELECT COUNT(*) FROM User_wallet) AS VARCHAR);
PRINT '擁有銷售權限的使用者: ' + CAST((SELECT COUNT(*) FROM User_Rights WHERE SalesAuthority = 1) AS VARCHAR);
PRINT '銷售檔案數量: ' + CAST((SELECT COUNT(*) FROM MemberSalesProfile) AS VARCHAR);
PRINT '銷售錢包數量: ' + CAST((SELECT COUNT(*) FROM User_Sales_Information) AS VARCHAR);
PRINT '簽到記錄總數: ' + CAST((SELECT COUNT(*) FROM UserSignInStats) AS VARCHAR);
PRINT '小遊戲記錄總數: ' + CAST((SELECT COUNT(*) FROM MiniGame) AS VARCHAR);
PRINT '換色記錄數量: ' + CAST((SELECT COUNT(*) FROM Pet WHERE PointsChanged > 0) AS VARCHAR);
PRINT '點數調整通知數量: ' + CAST((SELECT COUNT(*) FROM Notifications WHERE action_id = @AdjustmentActionId) AS VARCHAR);

-- 查看錢包餘額分布
PRINT '錢包餘額分布:';
SELECT 
    CASE 
        WHEN User_Point = 0 THEN '0點'
        WHEN User_Point <= 100 THEN '1-100點'
        WHEN User_Point <= 500 THEN '101-500點'
        WHEN User_Point <= 1000 THEN '501-1000點'
        WHEN User_Point <= 5000 THEN '1001-5000點'
        ELSE '5000點以上'
    END as 餘額範圍,
    COUNT(*) as 使用者數量
FROM User_wallet
GROUP BY 
    CASE 
        WHEN User_Point = 0 THEN '0點'
        WHEN User_Point <= 100 THEN '1-100點'
        WHEN User_Point <= 500 THEN '101-500點'
        WHEN User_Point <= 1000 THEN '501-1000點'
        WHEN User_Point <= 5000 THEN '1001-5000點'
        ELSE '5000點以上'
    END
ORDER BY MIN(User_Point);

PRINT '錢包系統種子資料插入完成！';
GO