namespace GameCore.Web.Models
{
    /// <summary>
    /// 統一的 API 回應模型
    /// </summary>
    public class ApiResponse<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public T? Data { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
        public int? TotalCount { get; set; }
        public int? PageNumber { get; set; }
        public int? PageSize { get; set; }

        public static ApiResponse<T> SuccessResult(T data, string message = "操作成功")
        {
            return new ApiResponse<T>
            {
                Success = true,
                Message = message,
                Data = data
            };
        }

        public static ApiResponse<T> ErrorResult(string message, T? data = default)
        {
            return new ApiResponse<T>
            {
                Success = false,
                Message = message,
                Data = data
            };
        }

        public static ApiResponse<T> PaginatedResult(T data, int totalCount, int pageNumber, int pageSize, string message = "查詢成功")
        {
            return new ApiResponse<T>
            {
                Success = true,
                Message = message,
                Data = data,
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }
    }

    /// <summary>
    /// 分頁查詢參數
    /// </summary>
    public class PaginationParameters
    {
        private int _pageNumber = 1;
        private int _pageSize = 20;
        private const int MaxPageSize = 100;

        public int PageNumber
        {
            get => _pageNumber;
            set => _pageNumber = value < 1 ? 1 : value;
        }

        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = value > MaxPageSize ? MaxPageSize : value < 1 ? 1 : value;
        }

        public int Skip => (PageNumber - 1) * PageSize;
        public int Take => PageSize;
    }

    /// <summary>
    /// 排序參數
    /// </summary>
    public class SortParameters
    {
        public string SortBy { get; set; } = "Id";
        public bool IsDescending { get; set; } = true;
    }

    /// <summary>
    /// 搜尋參數
    /// </summary>
    public class SearchParameters : PaginationParameters
    {
        public string? Keyword { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string? Category { get; set; }
        public string? Status { get; set; }
    }
}