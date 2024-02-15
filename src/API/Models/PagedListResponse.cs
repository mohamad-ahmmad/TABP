namespace API.Models;
#pragma warning disable CS1591

public class PagedListResponse<T>
{
    public PagedListResponse(IEnumerable<T> data, int page, int pageSize, int totalCount)
    {
        Data = data;
        Page = page;
        PageSize = pageSize;
        TotalCount = totalCount;
    }
    public IEnumerable<T> Data { get; }
    public int Page { get; }
    public int PageSize { get; }
    public int TotalCount { get; }
    public bool HasNextPage => Page * PageSize < TotalCount;
    public bool HasPreviousPage => Page > 1;
    public List<Link> Links { get; set; } = new();
}

