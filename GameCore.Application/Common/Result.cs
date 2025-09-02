namespace GameCore.Application.Common
{
    /// <summary>
    /// 統一結果模式
    /// </summary>
    /// <typeparam name="T">結果資料類型</typeparam>
    public class Result<T>
    {
        /// <summary>
        /// 是否成功
        /// </summary>
        public bool IsSuccess { get; }

        /// <summary>
        /// 結果資料
        /// </summary>
        public T? Data { get; }

        /// <summary>
        /// 錯誤訊息
        /// </summary>
        public string? ErrorMessage { get; }

        /// <summary>
        /// 驗證錯誤列表
        /// </summary>
        public List<string> ValidationErrors { get; }

        /// <summary>
        /// 私有建構函式
        /// </summary>
        private Result(bool isSuccess, T? data, string? errorMessage, List<string>? validationErrors = null)
        {
            IsSuccess = isSuccess;
            Data = data;
            ErrorMessage = errorMessage;
            ValidationErrors = validationErrors ?? new List<string>();
        }

        /// <summary>
        /// 建立成功結果
        /// </summary>
        /// <param name="data">結果資料</param>
        /// <returns>成功結果</returns>
        public static Result<T> Success(T data)
        {
            return new Result<T>(true, data, null);
        }

        /// <summary>
        /// 建立失敗結果
        /// </summary>
        /// <param name="errorMessage">錯誤訊息</param>
        /// <returns>失敗結果</returns>
        public static Result<T> Failure(string errorMessage)
        {
            return new Result<T>(false, default, errorMessage);
        }

        /// <summary>
        /// 建立驗證失敗結果
        /// </summary>
        /// <param name="validationErrors">驗證錯誤列表</param>
        /// <returns>驗證失敗結果</returns>
        public static Result<T> Failure(IEnumerable<string> validationErrors)
        {
            return new Result<T>(false, default, "驗證失敗", validationErrors.ToList());
        }

        /// <summary>
        /// 建立驗證失敗結果
        /// </summary>
        /// <param name="errorMessage">錯誤訊息</param>
        /// <param name="validationErrors">驗證錯誤列表</param>
        /// <returns>驗證失敗結果</returns>
        public static Result<T> Failure(string errorMessage, IEnumerable<string> validationErrors)
        {
            return new Result<T>(false, default, errorMessage, validationErrors.ToList());
        }

        /// <summary>
        /// 隱式轉換運算子
        /// </summary>
        /// <param name="data">資料</param>
        public static implicit operator Result<T>(T data) => Success(data);
    }

    /// <summary>
    /// 操作結果（無資料）
    /// </summary>
    public class OperationResult
    {
        /// <summary>
        /// 是否成功
        /// </summary>
        public bool IsSuccess { get; }

        /// <summary>
        /// 錯誤訊息
        /// </summary>
        public string? ErrorMessage { get; }

        /// <summary>
        /// 驗證錯誤列表
        /// </summary>
        public List<string> ValidationErrors { get; }

        /// <summary>
        /// 私有建構函式
        /// </summary>
        private OperationResult(bool isSuccess, string? errorMessage, List<string>? validationErrors = null)
        {
            IsSuccess = isSuccess;
            ErrorMessage = errorMessage;
            ValidationErrors = validationErrors ?? new List<string>();
        }

        /// <summary>
        /// 建立成功結果
        /// </summary>
        /// <returns>成功結果</returns>
        public static OperationResult Success()
        {
            return new OperationResult(true, null);
        }

        /// <summary>
        /// 建立失敗結果
        /// </summary>
        /// <param name="errorMessage">錯誤訊息</param>
        /// <returns>失敗結果</returns>
        public static OperationResult Failure(string errorMessage)
        {
            return new OperationResult(false, errorMessage);
        }

        /// <summary>
        /// 建立驗證失敗結果
        /// </summary>
        /// <param name="validationErrors">驗證錯誤列表</param>
        /// <returns>驗證失敗結果</returns>
        public static OperationResult Failure(IEnumerable<string> validationErrors)
        {
            return new OperationResult(false, "驗證失敗", validationErrors.ToList());
        }

        /// <summary>
        /// 建立驗證失敗結果
        /// </summary>
        /// <param name="errorMessage">錯誤訊息</param>
        /// <param name="validationErrors">驗證錯誤列表</param>
        /// <returns>驗證失敗結果</returns>
        public static OperationResult Failure(string errorMessage, IEnumerable<string> validationErrors)
        {
            return new OperationResult(false, errorMessage, validationErrors.ToList());
        }
    }

    /// <summary>
    /// 驗證錯誤
    /// </summary>
    public class ValidationError
    {
        /// <summary>
        /// 屬性名稱
        /// </summary>
        public string PropertyName { get; set; } = string.Empty;

        /// <summary>
        /// 錯誤訊息
        /// </summary>
        public string ErrorMessage { get; set; } = string.Empty;

        /// <summary>
        /// 錯誤代碼
        /// </summary>
        public string ErrorCode { get; set; } = string.Empty;

        /// <summary>
        /// 嘗試的值
        /// </summary>
        public object? AttemptedValue { get; set; }

        /// <summary>
        /// 建構函式
        /// </summary>
        public ValidationError()
        {
        }

        /// <summary>
        /// 建構函式
        /// </summary>
        /// <param name="propertyName">屬性名稱</param>
        /// <param name="errorMessage">錯誤訊息</param>
        public ValidationError(string propertyName, string errorMessage)
        {
            PropertyName = propertyName;
            ErrorMessage = errorMessage;
        }

        /// <summary>
        /// 建構函式
        /// </summary>
        /// <param name="propertyName">屬性名稱</param>
        /// <param name="errorMessage">錯誤訊息</param>
        /// <param name="errorCode">錯誤代碼</param>
        public ValidationError(string propertyName, string errorMessage, string errorCode)
        {
            PropertyName = propertyName;
            ErrorMessage = errorMessage;
            ErrorCode = errorCode;
        }
    }
} 