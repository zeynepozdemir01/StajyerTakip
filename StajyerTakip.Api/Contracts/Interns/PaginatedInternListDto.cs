namespace StajyerTakip.Api.Contracts.Interns;

public sealed record PaginatedInternListDto(
    IReadOnlyList<InternDto> Items,
    int Page,
    int PageSize,
    int TotalCount
);
