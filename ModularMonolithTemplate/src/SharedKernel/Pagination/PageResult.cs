namespace ModularMonolith.Platform.SharedKernel.Pagination;

public sealed record PageResult<T>(IReadOnlyList<T> Items, long Total, int PageNumber, int PageSize);
