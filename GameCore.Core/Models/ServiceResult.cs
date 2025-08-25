namespace GameCore.Core.Models
{
    /// <summary>
    /// 通用服務結果模型 - 用於統一 API 回應格式
    /// </summary>
    /// <typeparam name="T">結果資料類型</typeparam>
    public class ServiceResult<T>
    {
        /// <summary>
        /// 是否成功
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// 結果訊息
        /// </summary>
        public string Message { get; set; } = string.Empty;

        /// <summary>
        /// 結果資料
        /// </summary>
        public T? Data { get; set; }

        /// <summary>
        /// 錯誤列表
        /// </summary>
        public List<string> Errors { get; set; } = new List<string>();

        /// <summary>
        /// 錯誤代碼
        /// </summary>
        public string? ErrorCode { get; set; }

        /// <summary>
        /// 建立成功結果
        /// </summary>
        /// <param name="data">結果資料</param>
        /// <param name="message">成功訊息</param>
        /// <returns>成功結果</returns>
        public static ServiceResult<T> SuccessResult(T data, string message = "操作成功")
        {
            return new ServiceResult<T>
            {
                Success = true,
                Data = data,
                Message = message
            };
        }

        /// <summary>
        /// 建立失敗結果
        /// </summary>
        /// <param name="message">失敗訊息</param>
        /// <param name="errors">錯誤列表</param>
        /// <param name="errorCode">錯誤代碼</param>
        /// <returns>失敗結果</returns>
        public static ServiceResult<T> FailureResult(string message, List<string>? errors = null, string? errorCode = null)
        {
            return new ServiceResult<T>
            {
                Success = false,
                Message = message,
                Errors = errors ?? new List<string>(),
                ErrorCode = errorCode
            };
        }

        /// <summary>
        /// 建立失敗結果 (單一錯誤)
        /// </summary>
        /// <param name="message">失敗訊息</param>
        /// <param name="error">錯誤訊息</param>
        /// <param name="errorCode">錯誤代碼</param>
        /// <returns>失敗結果</returns>
        public static ServiceResult<T> FailureResult(string message, string error, string? errorCode = null)
        {
            return new ServiceResult<T>
            {
                Success = false,
                Message = message,
                Errors = new List<string> { error },
                ErrorCode = errorCode
            };
        }

        /// <summary>
        /// 建立驗證失敗結果
        /// </summary>
        /// <param name="errors">驗證錯誤列表</param>
        /// <returns>驗證失敗結果</returns>
        public static ServiceResult<T> ValidationFailureResult(List<string> errors)
        {
            return new ServiceResult<T>
            {
                Success = false,
                Message = "資料驗證失敗",
                Errors = errors,
                ErrorCode = "VALIDATION_ERROR"
            };
        }

        /// <summary>
        /// 建立未授權結果
        /// </summary>
        /// <param name="message">未授權訊息</param>
        /// <returns>未授權結果</returns>
        public static ServiceResult<T> UnauthorizedResult(string message = "未授權存取")
        {
            return new ServiceResult<T>
            {
                Success = false,
                Message = message,
                ErrorCode = "UNAUTHORIZED"
            };
        }

        /// <summary>
        /// 建立禁止存取結果
        /// </summary>
        /// <param name="message">禁止存取訊息</param>
        /// <returns>禁止存取結果</returns>
        public static ServiceResult<T> ForbiddenResult(string message = "禁止存取")
        {
            return new ServiceResult<T>
            {
                Success = false,
                Message = message,
                ErrorCode = "FORBIDDEN"
            };
        }

        /// <summary>
        /// 建立找不到結果
        /// </summary>
        /// <param name="message">找不到訊息</param>
        /// <returns>找不到結果</returns>
        public static ServiceResult<T> NotFoundResult(string message = "找不到指定資源")
        {
            return new ServiceResult<T>
            {
                Success = false,
                Message = message,
                ErrorCode = "NOT_FOUND"
            };
        }

        /// <summary>
        /// 建立衝突結果
        /// </summary>
        /// <param name="message">衝突訊息</param>
        /// <returns>衝突結果</returns>
        public static ServiceResult<T> ConflictResult(string message = "資源衝突")
        {
            return new ServiceResult<T>
            {
                Success = false,
                Message = message,
                ErrorCode = "CONFLICT"
            };
        }

        /// <summary>
        /// 建立伺服器錯誤結果
        /// </summary>
        /// <param name="message">伺服器錯誤訊息</param>
        /// <param name="errorCode">錯誤代碼</param>
        /// <returns>伺服器錯誤結果</returns>
        public static ServiceResult<T> ServerErrorResult(string message = "伺服器內部錯誤", string? errorCode = null)
        {
            return new ServiceResult<T>
            {
                Success = false,
                Message = message,
                ErrorCode = errorCode ?? "INTERNAL_SERVER_ERROR"
            };
        }

        /// <summary>
        /// 建立業務邏輯錯誤結果
        /// </summary>
        /// <param name="message">業務邏輯錯誤訊息</param>
        /// <param name="errorCode">錯誤代碼</param>
        /// <returns>業務邏輯錯誤結果</returns>
        public static ServiceResult<T> BusinessErrorResult(string message, string? errorCode = null)
        {
            return new ServiceResult<T>
            {
                Success = false,
                Message = message,
                ErrorCode = errorCode ?? "BUSINESS_ERROR"
            };
        }

        /// <summary>
        /// 建立超時結果
        /// </summary>
        /// <param name="message">超時訊息</param>
        /// <returns>超時結果</returns>
        public static ServiceResult<T> TimeoutResult(string message = "操作超時")
        {
            return new ServiceResult<T>
            {
                Success = false,
                Message = message,
                ErrorCode = "TIMEOUT"
            };
        }

        /// <summary>
        /// 建立限流結果
        /// </summary>
        /// <param name="message">限流訊息</param>
        /// <returns>限流結果</returns>
        public static ServiceResult<T> RateLimitResult(string message = "請求過於頻繁，請稍後再試")
        {
            return new ServiceResult<T>
            {
                Success = false,
                Message = message,
                ErrorCode = "RATE_LIMIT"
            };
        }

        /// <summary>
        /// 建立維護中結果
        /// </summary>
        /// <param name="message">維護中訊息</param>
        /// <returns>維護中結果</returns>
        public static ServiceResult<T> MaintenanceResult(string message = "系統維護中，請稍後再試")
        {
            return new ServiceResult<T>
            {
                Success = false,
                Message = message,
                ErrorCode = "MAINTENANCE"
            };
        }

        /// <summary>
        /// 建立版本不支援結果
        /// </summary>
        /// <param name="message">版本不支援訊息</param>
        /// <returns>版本不支援結果</returns>
        public static ServiceResult<T> VersionNotSupportedResult(string message = "API 版本不支援")
        {
            return new ServiceResult<T>
            {
                Success = false,
                Message = message,
                ErrorCode = "VERSION_NOT_SUPPORTED"
            };
        }

        /// <summary>
        /// 建立參數錯誤結果
        /// </summary>
        /// <param name="message">參數錯誤訊息</param>
        /// <param name="errors">參數錯誤列表</param>
        /// <returns>參數錯誤結果</returns>
        public static ServiceResult<T> BadRequestResult(string message = "請求參數錯誤", List<string>? errors = null)
        {
            return new ServiceResult<T>
            {
                Success = false,
                Message = message,
                Errors = errors ?? new List<string>(),
                ErrorCode = "BAD_REQUEST"
            };
        }

        /// <summary>
        /// 建立資源已存在結果
        /// </summary>
        /// <param name="message">資源已存在訊息</param>
        /// <returns>資源已存在結果</returns>
        public static ServiceResult<T> AlreadyExistsResult(string message = "資源已存在")
        {
            return new ServiceResult<T>
            {
                Success = false,
                Message = message,
                ErrorCode = "ALREADY_EXISTS"
            };
        }

        /// <summary>
        /// 建立資源不足結果
        /// </summary>
        /// <param name="message">資源不足訊息</param>
        /// <returns>資源不足結果</returns>
        public static ServiceResult<T> InsufficientResourceResult(string message = "資源不足")
        {
            return new ServiceResult<T>
            {
                Success = false,
                Message = message,
                ErrorCode = "INSUFFICIENT_RESOURCE"
            };
        }

        /// <summary>
        /// 建立條件不滿足結果
        /// </summary>
        /// <param name="message">條件不滿足訊息</param>
        /// <returns>條件不滿足結果</returns>
        public static ServiceResult<T> PreconditionFailedResult(string message = "前置條件不滿足")
        {
            return new ServiceResult<T>
            {
                Success = false,
                Message = message,
                ErrorCode = "PRECONDITION_FAILED"
            };
        }

        /// <summary>
        /// 建立請求實體過大結果
        /// </summary>
        /// <param name="message">請求實體過大訊息</param>
        /// <returns>請求實體過大結果</returns>
        public static ServiceResult<T> RequestEntityTooLargeResult(string message = "請求實體過大")
        {
            return new ServiceResult<T>
            {
                Success = false,
                Message = message,
                ErrorCode = "REQUEST_ENTITY_TOO_LARGE"
            };
        }

        /// <summary>
        /// 建立請求 URI 過長結果
        /// </summary>
        /// <param name="message">請求 URI 過長訊息</param>
        /// <returns>請求 URI 過長結果</returns>
        public static ServiceResult<T> RequestUriTooLongResult(string message = "請求 URI 過長")
        {
            return new ServiceResult<T>
            {
                Success = false,
                Message = message,
                ErrorCode = "REQUEST_URI_TOO_LONG"
            };
        }

        /// <summary>
        /// 建立不支援的媒體類型結果
        /// </summary>
        /// <param name="message">不支援的媒體類型訊息</param>
        /// <returns>不支援的媒體類型結果</returns>
        public static ServiceResult<T> UnsupportedMediaTypeResult(string message = "不支援的媒體類型")
        {
            return new ServiceResult<T>
            {
                Success = false,
                Message = message,
                ErrorCode = "UNSUPPORTED_MEDIA_TYPE"
            };
        }

        /// <summary>
        /// 建立期望失敗結果
        /// </summary>
        /// <param name="message">期望失敗訊息</param>
        /// <returns>期望失敗結果</returns>
        public static ServiceResult<T> ExpectationFailedResult(string message = "期望失敗")
        {
            return new ServiceResult<T>
            {
                Success = false,
                Message = message,
                ErrorCode = "EXPECTATION_FAILED"
            };
        }

        /// <summary>
        /// 建立升級要求結果
        /// </summary>
        /// <param name="message">升級要求訊息</param>
        /// <returns>升級要求結果</returns>
        public static ServiceResult<T> UpgradeRequiredResult(string message = "需要升級")
        {
            return new ServiceResult<T>
            {
                Success = false,
                Message = message,
                ErrorCode = "UPGRADE_REQUIRED"
            };
        }

        /// <summary>
        /// 建立預設成功結果 (無資料)
        /// </summary>
        /// <param name="message">成功訊息</param>
        /// <returns>成功結果</returns>
        public static ServiceResult<T> DefaultSuccessResult(string message = "操作成功")
        {
            return new ServiceResult<T>
            {
                Success = true,
                Message = message
            };
        }

        /// <summary>
        /// 建立預設失敗結果 (無資料)
        /// </summary>
        /// <param name="message">失敗訊息</param>
        /// <returns>失敗結果</returns>
        public static ServiceResult<T> DefaultFailureResult(string message = "操作失敗")
        {
            return new ServiceResult<T>
            {
                Success = false,
                Message = message
            };
        }
    }

    /// <summary>
    /// 非泛型服務結果模型 - 用於不需要返回資料的操作
    /// </summary>
    public class ServiceResult : ServiceResult<object>
    {
        /// <summary>
        /// 建立成功結果
        /// </summary>
        /// <param name="message">成功訊息</param>
        /// <returns>成功結果</returns>
        public static ServiceResult SuccessResult(string message = "操作成功")
        {
            return new ServiceResult
            {
                Success = true,
                Message = message
            };
        }

        /// <summary>
        /// 建立失敗結果
        /// </summary>
        /// <param name="message">失敗訊息</param>
        /// <param name="errors">錯誤列表</param>
        /// <param name="errorCode">錯誤代碼</param>
        /// <returns>失敗結果</returns>
        public static ServiceResult FailureResult(string message, List<string>? errors = null, string? errorCode = null)
        {
            return new ServiceResult
            {
                Success = false,
                Message = message,
                Errors = errors ?? new List<string>(),
                ErrorCode = errorCode
            };
        }

        /// <summary>
        /// 建立失敗結果 (單一錯誤)
        /// </summary>
        /// <param name="message">失敗訊息</param>
        /// <param name="error">錯誤訊息</param>
        /// <param name="errorCode">錯誤代碼</param>
        /// <returns>失敗結果</returns>
        public static ServiceResult FailureResult(string message, string error, string? errorCode = null)
        {
            return new ServiceResult
            {
                Success = false,
                Message = message,
                Errors = new List<string> { error },
                ErrorCode = errorCode
            };
        }

        /// <summary>
        /// 建立驗證失敗結果
        /// </summary>
        /// <param name="errors">驗證錯誤列表</param>
        /// <returns>驗證失敗結果</returns>
        public static ServiceResult ValidationFailureResult(List<string> errors)
        {
            return new ServiceResult
            {
                Success = false,
                Message = "資料驗證失敗",
                Errors = errors,
                ErrorCode = "VALIDATION_ERROR"
            };
        }

        /// <summary>
        /// 建立未授權結果
        /// </summary>
        /// <param name="message">未授權訊息</param>
        /// <returns>未授權結果</returns>
        public static ServiceResult UnauthorizedResult(string message = "未授權存取")
        {
            return new ServiceResult
            {
                Success = false,
                Message = message,
                ErrorCode = "UNAUTHORIZED"
            };
        }

        /// <summary>
        /// 建立禁止存取結果
        /// </summary>
        /// <param name="message">禁止存取訊息</param>
        /// <returns>禁止存取結果</returns>
        public static ServiceResult ForbiddenResult(string message = "禁止存取")
        {
            return new ServiceResult
            {
                Success = false,
                Message = message,
                ErrorCode = "FORBIDDEN"
            };
        }

        /// <summary>
        /// 建立找不到結果
        /// </summary>
        /// <param name="message">找不到訊息</param>
        /// <returns>找不到結果</returns>
        public static ServiceResult NotFoundResult(string message = "找不到指定資源")
        {
            return new ServiceResult
            {
                Success = false,
                Message = message,
                ErrorCode = "NOT_FOUND"
            };
        }

        /// <summary>
        /// 建立衝突結果
        /// </summary>
        /// <param name="message">衝突訊息</param>
        /// <returns>衝突結果</returns>
        public static ServiceResult ConflictResult(string message = "資源衝突")
        {
            return new ServiceResult
            {
                Success = false,
                Message = message,
                ErrorCode = "CONFLICT"
            };
        }

        /// <summary>
        /// 建立伺服器錯誤結果
        /// </summary>
        /// <param name="message">伺服器錯誤訊息</param>
        /// <param name="errorCode">錯誤代碼</param>
        /// <returns>伺服器錯誤結果</returns>
        public static ServiceResult ServerErrorResult(string message = "伺服器內部錯誤", string? errorCode = null)
        {
            return new ServiceResult
            {
                Success = false,
                Message = message,
                ErrorCode = errorCode ?? "INTERNAL_SERVER_ERROR"
            };
        }

        /// <summary>
        /// 建立業務邏輯錯誤結果
        /// </summary>
        /// <param name="message">業務邏輯錯誤訊息</param>
        /// <param name="errorCode">錯誤代碼</param>
        /// <returns>業務邏輯錯誤結果</returns>
        public static ServiceResult BusinessErrorResult(string message, string? errorCode = null)
        {
            return new ServiceResult
            {
                Success = false,
                Message = message,
                ErrorCode = errorCode ?? "BUSINESS_ERROR"
            };
        }

        /// <summary>
        /// 建立超時結果
        /// </summary>
        /// <param name="message">超時訊息</param>
        /// <returns>超時結果</returns>
        public static ServiceResult TimeoutResult(string message = "操作超時")
        {
            return new ServiceResult
            {
                Success = false,
                Message = message,
                ErrorCode = "TIMEOUT"
            };
        }

        /// <summary>
        /// 建立限流結果
        /// </summary>
        /// <param name="message">限流訊息</param>
        /// <returns>限流結果</returns>
        public static ServiceResult RateLimitResult(string message = "請求過於頻繁，請稍後再試")
        {
            return new ServiceResult
            {
                Success = false,
                Message = message,
                ErrorCode = "RATE_LIMIT"
            };
        }

        /// <summary>
        /// 建立維護中結果
        /// </summary>
        /// <param name="message">維護中訊息</param>
        /// <returns>維護中結果</returns>
        public static ServiceResult MaintenanceResult(string message = "系統維護中，請稍後再試")
        {
            return new ServiceResult
            {
                Success = false,
                Message = message,
                ErrorCode = "MAINTENANCE"
            };
        }

        /// <summary>
        /// 建立版本不支援結果
        /// </summary>
        /// <param name="message">版本不支援訊息</param>
        /// <returns>版本不支援結果</returns>
        public static ServiceResult VersionNotSupportedResult(string message = "API 版本不支援")
        {
            return new ServiceResult
            {
                Success = false,
                Message = message,
                ErrorCode = "VERSION_NOT_SUPPORTED"
            };
        }

        /// <summary>
        /// 建立參數錯誤結果
        /// </summary>
        /// <param name="message">參數錯誤訊息</param>
        /// <param name="errors">參數錯誤列表</param>
        /// <returns>參數錯誤結果</returns>
        public static ServiceResult BadRequestResult(string message = "請求參數錯誤", List<string>? errors = null)
        {
            return new ServiceResult
            {
                Success = false,
                Message = message,
                Errors = errors ?? new List<string>(),
                ErrorCode = "BAD_REQUEST"
            };
        }

        /// <summary>
        /// 建立資源已存在結果
        /// </summary>
        /// <param name="message">資源已存在訊息</param>
        /// <returns>資源已存在結果</returns>
        public static ServiceResult AlreadyExistsResult(string message = "資源已存在")
        {
            return new ServiceResult
            {
                Success = false,
                Message = message,
                ErrorCode = "ALREADY_EXISTS"
            };
        }

        /// <summary>
        /// 建立資源不足結果
        /// </summary>
        /// <param name="message">資源不足訊息</param>
        /// <returns>資源不足結果</returns>
        public static ServiceResult InsufficientResourceResult(string message = "資源不足")
        {
            return new ServiceResult
            {
                Success = false,
                Message = message,
                ErrorCode = "INSUFFICIENT_RESOURCE"
            };
        }

        /// <summary>
        /// 建立條件不滿足結果
        /// </summary>
        /// <param name="message">條件不滿足訊息</param>
        /// <returns>條件不滿足結果</returns>
        public static ServiceResult PreconditionFailedResult(string message = "前置條件不滿足")
        {
            return new ServiceResult
            {
                Success = false,
                Message = message,
                ErrorCode = "PRECONDITION_FAILED"
            };
        }

        /// <summary>
        /// 建立請求實體過大結果
        /// </summary>
        /// <param name="message">請求實體過大訊息</param>
        /// <returns>請求實體過大結果</returns>
        public static ServiceResult RequestEntityTooLargeResult(string message = "請求實體過大")
        {
            return new ServiceResult
            {
                Success = false,
                Message = message,
                ErrorCode = "REQUEST_ENTITY_TOO_LARGE"
            };
        }

        /// <summary>
        /// 建立請求 URI 過長結果
        /// </summary>
        /// <param name="message">請求 URI 過長訊息</param>
        /// <returns>請求 URI 過長結果</returns>
        public static ServiceResult RequestUriTooLongResult(string message = "請求 URI 過長")
        {
            return new ServiceResult
            {
                Success = false,
                Message = message,
                ErrorCode = "REQUEST_URI_TOO_LONG"
            };
        }

        /// <summary>
        /// 建立不支援的媒體類型結果
        /// </summary>
        /// <param name="message">不支援的媒體類型訊息</param>
        /// <returns>不支援的媒體類型結果</returns>
        public static ServiceResult UnsupportedMediaTypeResult(string message = "不支援的媒體類型")
        {
            return new ServiceResult
            {
                Success = false,
                Message = message,
                ErrorCode = "UNSUPPORTED_MEDIA_TYPE"
            };
        }

        /// <summary>
        /// 建立期望失敗結果
        /// </summary>
        /// <param name="message">期望失敗訊息</param>
        /// <returns>期望失敗結果</returns>
        public static ServiceResult ExpectationFailedResult(string message = "期望失敗")
        {
            return new ServiceResult
            {
                Success = false,
                Message = message,
                ErrorCode = "EXPECTATION_FAILED"
            };
        }

        /// <summary>
        /// 建立升級要求結果
        /// </summary>
        /// <param name="message">升級要求訊息</param>
        /// <returns>升級要求結果</returns>
        public static ServiceResult UpgradeRequiredResult(string message = "需要升級")
        {
            return new ServiceResult
            {
                Success = false,
                Message = message,
                ErrorCode = "UPGRADE_REQUIRED"
            };
        }

        /// <summary>
        /// 建立預設成功結果
        /// </summary>
        /// <param name="message">成功訊息</param>
        /// <returns>成功結果</returns>
        public static ServiceResult DefaultSuccessResult(string message = "操作成功")
        {
            return new ServiceResult
            {
                Success = true,
                Message = message
            };
        }

        /// <summary>
        /// 建立預設失敗結果
        /// </summary>
        /// <param name="message">失敗訊息</param>
        /// <returns>失敗結果</returns>
        public static ServiceResult DefaultFailureResult(string message = "操作失敗")
        {
            return new ServiceResult
            {
                Success = false,
                Message = message
            };
        }
    }
}