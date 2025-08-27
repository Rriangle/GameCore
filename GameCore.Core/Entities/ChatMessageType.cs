namespace GameCore.Core.Entities
{
    /// <summary>
    /// 聊天訊息類型列舉
    /// </summary>
    public enum ChatMessageType
    {
        /// <summary>
        /// 文字訊息
        /// </summary>
        Text,

        /// <summary>
        /// 圖片訊息
        /// </summary>
        Image,

        /// <summary>
        /// 檔案訊息
        /// </summary>
        File,

        /// <summary>
        /// 語音訊息
        /// </summary>
        Voice,

        /// <summary>
        /// 影片訊息
        /// </summary>
        Video,

        /// <summary>
        /// 系統訊息
        /// </summary>
        System,

        /// <summary>
        /// 通知訊息
        /// </summary>
        Notification,

        /// <summary>
        /// 表情符號
        /// </summary>
        Emoji,

        /// <summary>
        /// 貼圖
        /// </summary>
        Sticker,

        /// <summary>
        /// 位置訊息
        /// </summary>
        Location
    }
} 