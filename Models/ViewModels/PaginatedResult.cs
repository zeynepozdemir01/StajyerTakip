namespace StajyerTakip.Models.ViewModels
{
    public record PaginatedResult<T>(IEnumerable<T> Items, int TotalCount, int Page, int PageSize);
}
