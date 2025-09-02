namespace GameCore.Domain.Enums
{
    /// <summary>
    /// 詐騙風險等級
    /// </summary>
    public enum FraudRiskLevel
    {
        /// <summary>
        /// 低風險
        /// </summary>
        Low = 1,

        /// <summary>
        /// 中風險
        /// </summary>
        Medium = 2,

        /// <summary>
        /// 高風險
        /// </summary>
        High = 3,

        /// <summary>
        /// 極高風險
        /// </summary>
        Critical = 4
    }

    /// <summary>
    /// 託管釋放原因
    /// </summary>
    public enum EscrowReleaseReason
    {
        /// <summary>
        /// 交易完成
        /// </summary>
        TransactionCompleted = 1,

        /// <summary>
        /// 買家確認
        /// </summary>
        BuyerConfirmed = 2,

        /// <summary>
        /// 賣家確認
        /// </summary>
        SellerConfirmed = 3,

        /// <summary>
        /// 爭議解決
        /// </summary>
        DisputeResolved = 4,

        /// <summary>
        /// 系統自動
        /// </summary>
        SystemAutomatic = 5
    }

    /// <summary>
        /// 安全警報等級
        /// </summary>
    public enum SecurityAlertLevel
    {
        /// <summary>
        /// 資訊
        /// </summary>
        Info = 1,

        /// <summary>
        /// 警告
        /// </summary>
        Warning = 2,

        /// <summary>
        /// 錯誤
        /// </summary>
        Error = 3,

        /// <summary>
        /// 嚴重
        /// </summary>
        Critical = 4
    }
} 
