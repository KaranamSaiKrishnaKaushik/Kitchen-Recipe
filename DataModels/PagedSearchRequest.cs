namespace DataModels;

public class PagedSearchRequest
{
    public string Source { get; set; }
    public List<string> Names { get; set; }
    public int Page { get; set; }
    public int PageSize { get; set; }
}
