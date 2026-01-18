namespace WMSCommon.Results
{
    public class RepositoryResult<T>
    {
        public bool IsSuccess { get; set; }
        public T? Data { get; set; }
        public string Message { get; set; } = string.Empty;

        public static RepositoryResult<T> Success(T data) => new() { IsSuccess = true, Data = data};
        public static RepositoryResult<T> Failure(string message) => new() { IsSuccess = false, Message = message };
    }
}
