using WalkerWebApp.Models;

namespace WalkerWebApp.ViewModels;

public class IndexWalkViewModel
{
    public IEnumerable<Walk> Walks { get; set; }
    public int PageSize { get; set; }
    public int Page { get; set; }
    public int TotalPages { get; set; }
    public int TotalWalks { get; set; }
    public int Category { get; set; }
    public bool HasPreviousPage => Page > 1;
    public bool HasNextPage => Page < TotalPages;
}