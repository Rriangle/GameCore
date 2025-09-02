namespace GameCore.Domain.Enums
{
    /// <summary>
    /// 文章狀態枚舉
    /// </summary>
    public enum PostStatus
    {
        /// <summary>
        /// 草稿
        /// </summary>
        Draft = 0,

        /// <summary>
        /// 已發布
        /// </summary>
        Published = 1,

        /// <summary>
        /// 待審核
        /// </summary>
        Pending = 2,

        /// <summary>
        /// 已拒絕
        /// </summary>
        Rejected = 3,

        /// <summary>
        /// 已刪除
        /// </summary>
        Deleted = 4,

        /// <summary>
        /// 已封鎖
        /// </summary>
        Blocked = 5,

        /// <summary>
        /// 已置頂
        /// </summary>
        Pinned = 6,

        /// <summary>
        /// 已精華
        /// </summary>
        Featured = 7,

        /// <summary>
        /// 相容性：Active（舊代碼引用）
        /// </summary>
        Active = Published
    }

    /// <summary>
    /// 文章來源枚舉
    /// </summary>
    public enum PostSource
    {
        /// <summary>
        /// 用戶原創
        /// </summary>
        Original = 0,

        /// <summary>
        /// 轉載
        /// </summary>
        Repost = 1,

        /// <summary>
        /// 翻譯
        /// </summary>
        Translation = 2,

        /// <summary>
        /// 官方
        /// </summary>
        Official = 3,

        /// <summary>
        /// 新聞
        /// </summary>
        News = 4,

        /// <summary>
        /// 攻略
        /// </summary>
        Guide = 5,

        /// <summary>
        /// 心得
        /// </summary>
        Experience = 6,

        /// <summary>
        /// 問題
        /// </summary>
        Question = 7,

        /// <summary>
        /// 討論
        /// </summary>
        Discussion = 8
    }
} 