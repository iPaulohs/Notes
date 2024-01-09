namespace Notes.Pagination;

public class Parameters<T> 
{
    private const int maxPageSize = 50;
    private int pageSize = 10;

    public int PageNumber { get; set; }
    public int PageSize
    {
        get
        {
            return pageSize;
        }
        set
        {
            pageSize = (value > maxPageSize) ? maxPageSize : value;
        }
    }
}
