using StajyerTakip.Application.Interns.Queries.GetInterns;

namespace StajyerTakip.Models.ViewModels
{
    public class InternListVm
    {
        public IReadOnlyList<InternListItemDto> Items { get; set; } = Array.Empty<InternListItemDto>();

        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public int TotalCount { get; set; }
        public string? Query { get; set; }
        public string? Status { get; set; }

        public string? SortField { get; set; }
        public string? SortOrder { get; set; } 

        public int TotalPages => (int)Math.Ceiling((double)(TotalCount > 0 ? TotalCount : 1) / PageSize);
    }
}
