namespace Huayu.Wms.Identity.Application.Requests;

public abstract class PagedRequest
{
    public int PageSize { get; set; }
    public int pageNumber { get; set; }
    public string[] OrderBy { get; set; }
}
