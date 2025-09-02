namespace GameCore.Domain.Enums
{
    /// <summary>
    /// 個性特質
    /// </summary>
    public enum PersonalityTrait
    {
        /// <summary>
        /// 活潑
        /// </summary>
        Energetic = 1,

        /// <summary>
        /// 安靜
        /// </summary>
        Quiet = 2,

        /// <summary>
        /// 好奇
        /// </summary>
        Curious = 3,

        /// <summary>
        /// 害羞
        /// </summary>
        Shy = 4,

        /// <summary>
        /// 勇敢
        /// </summary>
        Brave = 5,

        /// <summary>
        /// 謹慎
        /// </summary>
        Cautious = 6,

        /// <summary>
        /// 友善
        /// </summary>
        Friendly = 7,

        /// <summary>
        /// 獨立
        /// </summary>
        Independent = 8
    }

    /// <summary>
    /// 環境因素
    /// </summary>
    public enum EnvironmentalFactors
    {
        /// <summary>
        /// 溫度
        /// </summary>
        Temperature = 1,

        /// <summary>
        /// 濕度
        /// </summary>
        Humidity = 2,

        /// <summary>
        /// 光照
        /// </summary>
        Light = 3,

        /// <summary>
        /// 噪音
        /// </summary>
        Noise = 4,

        /// <summary>
        /// 空間大小
        /// </summary>
        Space = 5,

        /// <summary>
        /// 清潔度
        /// </summary>
        Cleanliness = 6
    }

    /// <summary>
    /// 交易類型
    /// </summary>
    public enum TransactionType
    {
        /// <summary>
        /// 購買
        /// </summary>
        Purchase = 1,

        /// <summary>
        /// 銷售
        /// </summary>
        Sale = 2,

        /// <summary>
        /// 轉帳
        /// </summary>
        Transfer = 3,

        /// <summary>
        /// 退款
        /// </summary>
        Refund = 4,

        /// <summary>
        /// 手續費
        /// </summary>
        Fee = 5,

        /// <summary>
        /// 獎勵
        /// </summary>
        Reward = 6,

        /// <summary>
        /// 懲罰
        /// </summary>
        Penalty = 7
    }
} 
