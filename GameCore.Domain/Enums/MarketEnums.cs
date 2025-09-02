namespace GameCore.Domain.Enums
{
    /// <summary>
    /// 市場交易狀態枚舉
    /// </summary>
    public enum MarketTransactionStatus
    {
        /// <summary>
        /// 待付款
        /// </summary>
        PendingPayment = 0,

        /// <summary>
        /// 已付款
        /// </summary>
        Paid = 1,

        /// <summary>
        /// 已發貨
        /// </summary>
        Shipped = 2,

        /// <summary>
        /// 已送達
        /// </summary>
        Delivered = 3,

        /// <summary>
        /// 已完成
        /// </summary>
        Completed = 4,

        /// <summary>
        /// 已取消
        /// </summary>
        Cancelled = 5,

        /// <summary>
        /// 已退款
        /// </summary>
        Refunded = 6,

        /// <summary>
        /// 爭議中
        /// </summary>
        Disputed = 7,

        /// <summary>
        /// 已暫停
        /// </summary>
        Suspended = 8,

        /// <summary>
        /// 已上架
        /// </summary>
        Listed = 9
    }

    /// <summary>
    /// 權限枚舉
    /// </summary>
    public enum Permission
    {
        /// <summary>
        /// 無權限
        /// </summary>
        None = 0,

        /// <summary>
        /// 讀取權限
        /// </summary>
        Read = 1,

        /// <summary>
        /// 寫入權限
        /// </summary>
        Write = 2,

        /// <summary>
        /// 刪除權限
        /// </summary>
        Delete = 4,

        /// <summary>
        /// 管理權限
        /// </summary>
        Admin = 8,

        /// <summary>
        /// 超級管理員權限
        /// </summary>
        SuperAdmin = 16,

        /// <summary>
        /// 內容審核權限
        /// </summary>
        ContentModeration = 32,

        /// <summary>
        /// 用戶管理權限
        /// </summary>
        UserManagement = 64,

        /// <summary>
        /// 系統設置權限
        /// </summary>
        SystemSettings = 128
    }

    /// <summary>
    /// 商品狀態枚舉
    /// </summary>
    public enum ProductStatus
    {
        /// <summary>
        /// 草稿
        /// </summary>
        Draft = 0,

        /// <summary>
        /// 上架中
        /// </summary>
        Active = 1,

        /// <summary>
        /// 下架
        /// </summary>
        Inactive = 2,

        /// <summary>
        /// 已售完
        /// </summary>
        SoldOut = 3,

        /// <summary>
        /// 已刪除
        /// </summary>
        Deleted = 4,

        /// <summary>
        /// 待審核
        /// </summary>
        Pending = 5,

        /// <summary>
        /// 已拒絕
        /// </summary>
        Rejected = 6
    }
} 