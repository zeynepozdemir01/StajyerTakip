using StajyerTakip.Domain.Entities;

namespace StajyerTakip.Models.ViewModels
{
    public class InternListVm
    {
        public IEnumerable<Intern> Items { get; set; } = Enumerable.Empty<Intern>();
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public int TotalCount { get; set; }
        public string? Query { get; set; }
        public string? Status { get; set; }

        public string? SortField { get; set; }
        public string? SortOrder { get; set; } 

        public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
    }
}
