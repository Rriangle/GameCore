namespace GameCore.Domain.Entities
{
    /// <summary>
    /// ?�天訊息類�??��?
    /// </summary>
    public enum ChatMessageType
    {
        /// <summary>
        /// ?��?訊息
        /// </summary>
        Text,

        /// <summary>
        /// ?��?訊息
        /// </summary>
        Image,

        /// <summary>
        /// 檔�?訊息
        /// </summary>
        File,

        /// <summary>
        /// 語音訊息
        /// </summary>
        Voice,

        /// <summary>
        /// 影�?訊息
        /// </summary>
        Video,

        /// <summary>
        /// 系統訊息
        /// </summary>
        System,

        /// <summary>
        /// ?�知訊息
        /// </summary>
        Notification,

        /// <summary>
        /// 表�?符�?
        /// </summary>
        Emoji,

        /// <summary>
        /// 貼�?
        /// </summary>
        Sticker,

        /// <summary>
        /// 位置訊息
        /// </summary>
        Location
    }
} 
