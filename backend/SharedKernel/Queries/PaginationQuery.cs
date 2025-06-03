namespace SharedKernel.Queries;

public record PaginationQuery
{
    public int? PageNumber { get; set; } = 1;
    public int? PageSize { get; set; } = 10;
    
    public void Normalize()
    {
        PageNumber ??= 1;
        PageSize ??= 10;
    }
}
