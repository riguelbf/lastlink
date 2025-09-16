namespace ModularMonolith.Platform.SharedKernel.Pagination;

public sealed record PageRequest(int PageNumber = 1, int PageSize = 50)
{
    public int Skip => (PageNumber < 1 ? 0 : (PageNumber - 1) * Take);
    public int Take => PageSize is <= 0 or > 1000 ? 50 : PageSize;
}
