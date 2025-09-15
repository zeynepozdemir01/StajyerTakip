using MediatR;
using StajyerTakip.Application.Common;

namespace StajyerTakip.Application.Interns.Queries.GetInterns;

public sealed class PagedResult<T>
{
    public int Page { get; init; }
    public int PageSize { get; init; }
    public int Total { get; init; }
    public IReadOnlyList<T> Items { get; init; } = Array.Empty<T>();
}

public sealed class GetInternsQuery : IRequest<Result<PagedResult<InternListItemDto>>>
{
    public int Page { get; init; } = 1;            
    public int PageSize { get; init; } = 10;
    public string? Search { get; init; }
    public string? Status { get; init; }           
    public string? SortBy { get; init; }           
    public string SortDir { get; init; } = "asc";  
}
