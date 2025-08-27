namespace GameCore.Core.DTOs
{
    /// <summary>
    /// 批量操作請求
    /// </summary>
    /// <typeparam name="T">數據類型</typeparam>
    public class BulkOperationRequest<T>
    {
        /// <summary>
        /// 操作類型
        /// </summary>
        public string OperationType { get; set; } = string.Empty; // Create, Update, Delete

        /// <summary>
        /// 數據列表
        /// </summary>
        public List<T> Items { get; set; } = new List<T>();

        /// <summary>
        /// 是否在事務中執行
        /// </summary>
        public bool UseTransaction { get; set; } = true;

        /// <summary>
        /// 批次大小
        /// </summary>
        public int BatchSize { get; set; } = 100;

        /// <summary>
        /// 是否繼續執行 (遇到錯誤時)
        /// </summary>
        public bool ContinueOnError { get; set; } = false;
    }

    /// <summary>
    /// 批量操作結果
    /// </summary>
    /// <typeparam name="T">數據類型</typeparam>
    public class BulkOperationResult<T>
    {
        /// <summary>
        /// 操作類型
        /// </summary>
        public string OperationType { get; set; } = string.Empty;

        /// <summary>
        /// 總數量
        /// </summary>
        public int TotalCount { get; set; }

        /// <summary>
        /// 成功數量
        /// </summary>
        public int SuccessCount { get; set; }

        /// <summary>
        /// 失敗數量
        /// </summary>
        public int FailureCount { get; set; }

        /// <summary>
        /// 錯誤列表
        /// </summary>
        public List<BulkOperationError> Errors { get; set; } = new List<BulkOperationError>();

        /// <summary>
        /// 處理的項目
        /// </summary>
        public List<T> ProcessedItems { get; set; } = new List<T>();

        /// <summary>
        /// 執行時間 (毫秒)
        /// </summary>
        public long ExecutionTimeMs { get; set; }

        /// <summary>
        /// 是否全部成功
        /// </summary>
        public bool IsAllSuccess => FailureCount == 0;

        /// <summary>
        /// 成功率
        /// </summary>
        public double SuccessRate => TotalCount > 0 ? (double)SuccessCount / TotalCount * 100 : 0;
    }

    /// <summary>
    /// 批量操作錯誤
    /// </summary>
    public class BulkOperationError
    {
        /// <summary>
        /// 項目索引
        /// </summary>
        public int ItemIndex { get; set; }

        /// <summary>
        /// 錯誤消息
        /// </summary>
        public string ErrorMessage { get; set; } = string.Empty;

        /// <summary>
        /// 錯誤代碼
        /// </summary>
        public string? ErrorCode { get; set; }

        /// <summary>
        /// 項目數據 (JSON 格式)
        /// </summary>
        public string? ItemData { get; set; }

        /// <summary>
        /// 錯誤時間
        /// </summary>
        public DateTime ErrorTime { get; set; } = DateTime.UtcNow;
    }

    /// <summary>
    /// 批量用戶操作請求
    /// </summary>
    public class BulkUserOperationRequest
    {
        /// <summary>
        /// 操作類型
        /// </summary>
        public string OperationType { get; set; } = string.Empty;

        /// <summary>
        /// 用戶 ID 列表
        /// </summary>
        public List<int> UserIds { get; set; } = new List<int>();

        /// <summary>
        /// 更新數據 (用於 Update 操作)
        /// </summary>
        public Dictionary<string, object>? UpdateData { get; set; }

        /// <summary>
        /// 是否在事務中執行
        /// </summary>
        public bool UseTransaction { get; set; } = true;
    }

    /// <summary>
    /// 批量產品操作請求
    /// </summary>
    public class BulkProductOperationRequest
    {
        /// <summary>
        /// 操作類型
        /// </summary>
        public string OperationType { get; set; } = string.Empty;

        /// <summary>
        /// 產品 ID 列表
        /// </summary>
        public List<int> ProductIds { get; set; } = new List<int>();

        /// <summary>
        /// 更新數據 (用於 Update 操作)
        /// </summary>
        public Dictionary<string, object>? UpdateData { get; set; }

        /// <summary>
        /// 是否在事務中執行
        /// </summary>
        public bool UseTransaction { get; set; } = true;
    }

    /// <summary>
    /// 批量訂單操作請求
    /// </summary>
    public class BulkOrderOperationRequest
    {
        /// <summary>
        /// 操作類型
        /// </summary>
        public string OperationType { get; set; } = string.Empty;

        /// <summary>
        /// 訂單 ID 列表
        /// </summary>
        public List<int> OrderIds { get; set; } = new List<int>();

        /// <summary>
        /// 更新數據 (用於 Update 操作)
        /// </summary>
        public Dictionary<string, object>? UpdateData { get; set; }

        /// <summary>
        /// 是否在事務中執行
        /// </summary>
        public bool UseTransaction { get; set; } = true;
    }

    /// <summary>
    /// 批量寵物操作請求
    /// </summary>
    public class BulkPetOperationRequest
    {
        /// <summary>
        /// 操作類型
        /// </summary>
        public string OperationType { get; set; } = string.Empty;

        /// <summary>
        /// 寵物 ID 列表
        /// </summary>
        public List<int> PetIds { get; set; } = new List<int>();

        /// <summary>
        /// 更新數據 (用於 Update 操作)
        /// </summary>
        public Dictionary<string, object>? UpdateData { get; set; }

        /// <summary>
        /// 是否在事務中執行
        /// </summary>
        public bool UseTransaction { get; set; } = true;
    }

    /// <summary>
    /// 批量數據導入請求
    /// </summary>
    /// <typeparam name="T">數據類型</typeparam>
    public class BulkImportRequest<T>
    {
        /// <summary>
        /// 數據列表
        /// </summary>
        public List<T> Items { get; set; } = new List<T>();

        /// <summary>
        /// 導入選項
        /// </summary>
        public ImportOptions Options { get; set; } = new ImportOptions();

        /// <summary>
        /// 驗證規則
        /// </summary>
        public List<ValidationRule> ValidationRules { get; set; } = new List<ValidationRule>();
    }

    /// <summary>
    /// 導入選項
    /// </summary>
    public class ImportOptions
    {
        /// <summary>
        /// 是否跳過重複項
        /// </summary>
        public bool SkipDuplicates { get; set; } = true;

        /// <summary>
        /// 是否更新現有項
        /// </summary>
        public bool UpdateExisting { get; set; } = false;

        /// <summary>
        /// 批次大小
        /// </summary>
        public int BatchSize { get; set; } = 100;

        /// <summary>
        /// 是否在事務中執行
        /// </summary>
        public bool UseTransaction { get; set; } = true;

        /// <summary>
        /// 是否繼續執行 (遇到錯誤時)
        /// </summary>
        public bool ContinueOnError { get; set; } = false;

        /// <summary>
        /// 最大錯誤數量
        /// </summary>
        public int MaxErrorCount { get; set; } = 100;
    }

    /// <summary>
    /// 驗證規則
    /// </summary>
    public class ValidationRule
    {
        /// <summary>
        /// 規則名稱
        /// </summary>
        public string RuleName { get; set; } = string.Empty;

        /// <summary>
        /// 屬性名稱
        /// </summary>
        public string PropertyName { get; set; } = string.Empty;

        /// <summary>
        /// 驗證類型
        /// </summary>
        public string ValidationType { get; set; } = string.Empty; // Required, MinLength, MaxLength, Range, etc.

        /// <summary>
        /// 驗證參數
        /// </summary>
        public Dictionary<string, object> Parameters { get; set; } = new Dictionary<string, object>();

        /// <summary>
        /// 錯誤消息
        /// </summary>
        public string ErrorMessage { get; set; } = string.Empty;
    }

    /// <summary>
    /// 批量數據導出請求
    /// </summary>
    public class BulkExportRequest
    {
        /// <summary>
        /// 導出類型
        /// </summary>
        public string ExportType { get; set; } = string.Empty; // User, Product, Order, Pet, etc.

        /// <summary>
        /// 過濾條件
        /// </summary>
        public Dictionary<string, object>? Filters { get; set; }

        /// <summary>
        /// 排序條件
        /// </summary>
        public List<SortCondition> SortConditions { get; set; } = new List<SortCondition>();

        /// <summary>
        /// 導出格式
        /// </summary>
        public string ExportFormat { get; set; } = "CSV"; // CSV, Excel, JSON, XML

        /// <summary>
        /// 包含的欄位
        /// </summary>
        public List<string> IncludeFields { get; set; } = new List<string>();

        /// <summary>
        /// 排除的欄位
        /// </summary>
        public List<string> ExcludeFields { get; set; } = new List<string>();

        /// <summary>
        /// 最大導出數量
        /// </summary>
        public int? MaxExportCount { get; set; }
    }

    /// <summary>
    /// 排序條件
    /// </summary>
    public class SortCondition
    {
        /// <summary>
        /// 欄位名稱
        /// </summary>
        public string FieldName { get; set; } = string.Empty;

        /// <summary>
        /// 排序方向
        /// </summary>
        public string SortDirection { get; set; } = "Ascending"; // Ascending, Descending
    }

    /// <summary>
    /// 批量操作進度
    /// </summary>
    public class BulkOperationProgress
    {
        /// <summary>
        /// 操作 ID
        /// </summary>
        public string OperationId { get; set; } = string.Empty;

        /// <summary>
        /// 總數量
        /// </summary>
        public int TotalCount { get; set; }

        /// <summary>
        /// 已處理數量
        /// </summary>
        public int ProcessedCount { get; set; }

        /// <summary>
        /// 成功數量
        /// </summary>
        public int SuccessCount { get; set; }

        /// <summary>
        /// 失敗數量
        /// </summary>
        public int FailureCount { get; set; }

        /// <summary>
        /// 進度百分比
        /// </summary>
        public double ProgressPercentage => TotalCount > 0 ? (double)ProcessedCount / TotalCount * 100 : 0;

        /// <summary>
        /// 狀態
        /// </summary>
        public string Status { get; set; } = "Running"; // Running, Completed, Failed, Cancelled

        /// <summary>
        /// 開始時間
        /// </summary>
        public DateTime StartTime { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// 預計完成時間
        /// </summary>
        public DateTime? EstimatedCompletionTime { get; set; }

        /// <summary>
        /// 錯誤消息
        /// </summary>
        public string? ErrorMessage { get; set; }
    }
} 