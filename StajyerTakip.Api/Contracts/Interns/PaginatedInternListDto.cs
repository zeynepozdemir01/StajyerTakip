namespace StajyerTakip.Api.Contracts.Interns;

public record PaginatedInternListDto(
    List<InternDto> Items,
    int Page,
    int PageSize,
    int TotalCount
);
