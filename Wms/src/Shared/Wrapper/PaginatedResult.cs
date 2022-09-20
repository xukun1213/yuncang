namespace Huayu.Shared.Wrapper;

public class PaginatedResult<T> : Result
{
    public List<T>? Data { get; set; }

    public int CurrentPage { get; set; }

    public int PageSize { get; set; }

    public int TotalPages { get; set; }

    public int TotalCount { get; set; }

    public bool HasPreviousPage => CurrentPage > 1;

    public bool HasNextPage => CurrentPage < TotalPages;


    public PaginatedResult(List<T> data) => Data = data;

    internal PaginatedResult(bool succeeded, List<T>? data = default, List<string>? messages = null, int count = 0, int page = 1, int pageSize = 10)
    {
        Succeeded = succeeded;
        if (messages != null) Messages = messages;
        Data = data;
        CurrentPage = page;
        PageSize = pageSize;
        TotalPages = (int)Math.Ceiling(count / (double)pageSize);
        TotalCount = count;
    }

    public static new PaginatedResult<T> Fail(List<string> messages) => new(false, default, messages);

    public static PaginatedResult<T> Success(List<T> data, int count, int page, int pageSize) => new(true, data, null, count, page, pageSize);

    public static new Task<PaginatedResult<T>> FailAsync(List<string> messages) => Task.FromResult(Fail(messages));

    public static Task<PaginatedResult<T>> SuccessAsync(List<T> data, int count, int page, int pageSize) => Task.FromResult(Success(data, count, page, pageSize));
}
