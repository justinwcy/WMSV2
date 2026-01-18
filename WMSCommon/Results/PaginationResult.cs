namespace WMSCommon.Results
{
    public class PaginationResult<T>
    {
        public IReadOnlyList<T> Items { get; init; } = new List<T>();
        public int TotalCount { get; init; }
        public int PageNumber { get; init; }
        public int PageSize { get; init; }
        public int TotalPages => (int)Math.Ceiling(TotalCount / (double)PageSize);
    }
}
