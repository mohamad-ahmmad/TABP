namespace Application.Dtos;
public class PagedList<T>
{
    public PagedList(IEnumerable<T> data, int page, int pageSize, int totalCount)
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
    public bool HasNextPage => Page*PageSize < TotalCount;
    public bool HasPreviousPage => Page > 1;

}

