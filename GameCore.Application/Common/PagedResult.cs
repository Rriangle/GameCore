namespace GameCore.Application.Common
{
    /// <summary>
    /// 分頁結果
    /// </summary>
    /// <typeparam name="T">資料類型</typeparam>
    public class PagedResult<T>
    {
        /// <summary>
        /// 資料項目
        /// </summary>
        public IEnumerable<T> Items { get; set; } = Enumerable.Empty<T>();

        /// <summary>
        /// 總數量
        /// </summary>
        public int TotalCount { get; set; }

        /// <summary>
        /// 當前頁碼
        /// </summary>
        public int PageNumber { get; set; }

        /// <summary>
        /// 當前頁碼 (相容性別名)
        /// </summary>
        public int CurrentPage { get => PageNumber; set => PageNumber = value; }

        /// <summary>
        /// 每頁大小
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// 總頁數
        /// </summary>
        public int TotalPages { get; set; }

        /// <summary>
        /// 是否有上一頁
        /// </summary>
        public bool HasPreviousPage { get; set; }

        /// <summary>
        /// 是否有下一頁
        /// </summary>
        public bool HasNextPage { get; set; }

        /// <summary>
        /// 上一頁頁碼
        /// </summary>
        public int? PreviousPageNumber { get; set; }

        /// <summary>
        /// 下一頁頁碼
        /// </summary>
        public int? NextPageNumber { get; set; }

        /// <summary>
        /// 建構函式
        /// </summary>
        public PagedResult()
        {
        }

        /// <summary>
        /// 建構函式
        /// </summary>
        /// <param name="items">資料項目</param>
        /// <param name="totalCount">總數量</param>
        /// <param name="pageNumber">當前頁碼</param>
        /// <param name="pageSize">每頁大小</param>
        public PagedResult(IEnumerable<T> items, int totalCount, int pageNumber, int pageSize)
        {
            Items = items;
            TotalCount = totalCount;
            PageNumber = pageNumber;
            PageSize = pageSize;
            TotalPages = (int)Math.Ceiling((double)totalCount / pageSize);
            HasPreviousPage = pageNumber > 1;
            HasNextPage = pageNumber < TotalPages;
            PreviousPageNumber = HasPreviousPage ? pageNumber - 1 : null;
            NextPageNumber = HasNextPage ? pageNumber + 1 : null;
        }

        /// <summary>
        /// 建立分頁結果
        /// </summary>
        /// <param name="items">資料項目</param>
        /// <param name="totalCount">總數量</param>
        /// <param name="pageNumber">當前頁碼</param>
        /// <param name="pageSize">每頁大小</param>
        /// <returns>分頁結果</returns>
        public static PagedResult<T> Create(IEnumerable<T> items, int totalCount, int pageNumber, int pageSize)
        {
            return new PagedResult<T>(items, totalCount, pageNumber, pageSize);
        }

        /// <summary>
        /// 建立空的分頁結果
        /// </summary>
        /// <param name="pageNumber">當前頁碼</param>
        /// <param name="pageSize">每頁大小</param>
        /// <returns>空的分頁結果</returns>
        public static PagedResult<T> Empty(int pageNumber = 1, int pageSize = 10)
        {
            return new PagedResult<T>(Enumerable.Empty<T>(), 0, pageNumber, pageSize);
        }
    }

    /// <summary>
    /// 分頁參數
    /// </summary>
    public class PagedParameters
    {
        private int _pageNumber = 1;
        private int _pageSize = 10;

        /// <summary>
        /// 頁碼
        /// </summary>
        public int PageNumber
        {
            get => _pageNumber;
            set => _pageNumber = value < 1 ? 1 : value;
        }

        /// <summary>
        /// 每頁大小
        /// </summary>
        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = value < 1 ? 10 : value > 100 ? 100 : value;
        }

        /// <summary>
        /// 排序欄位
        /// </summary>
        public string? SortBy { get; set; }

        /// <summary>
        /// 是否降序排序
        /// </summary>
        public bool SortDescending { get; set; } = false;

        /// <summary>
        /// 搜尋關鍵字
        /// </summary>
        public string? SearchTerm { get; set; }

        /// <summary>
        /// 建構函式
        /// </summary>
        public PagedParameters()
        {
        }

        /// <summary>
        /// 建構函式
        /// </summary>
        /// <param name="pageNumber">頁碼</param>
        /// <param name="pageSize">每頁大小</param>
        public PagedParameters(int pageNumber, int pageSize)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
        }

        /// <summary>
        /// 取得跳過的數量
        /// </summary>
        /// <returns>跳過的數量</returns>
        public int GetSkip()
        {
            return (PageNumber - 1) * PageSize;
        }

        /// <summary>
        /// 取得取得的數量
        /// </summary>
        /// <returns>取得的數量</returns>
        public int GetTake()
        {
            return PageSize;
        }
    }
} 