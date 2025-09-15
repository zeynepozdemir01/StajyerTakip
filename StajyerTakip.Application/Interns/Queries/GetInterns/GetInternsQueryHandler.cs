using System.Linq;
using MediatR;
using StajyerTakip.Application.Common;
using StajyerTakip.Application.Interfaces;

namespace StajyerTakip.Application.Interns.Queries.GetInterns
{
    public sealed class GetInternsQueryHandler
        : IRequestHandler<GetInternsQuery, Result<PagedResult<InternListItemDto>>>
    {
        private readonly IInternRepository _repo;
        public GetInternsQueryHandler(IInternRepository repo) => _repo = repo;

        public async Task<Result<PagedResult<InternListItemDto>>> Handle(GetInternsQuery q, CancellationToken ct)
        {
            var (entities, total) = await _repo.ListAsync(
                q.Search,
                q.Status,
                q.Page,
                q.PageSize,
                q.SortBy ?? string.Empty,
                q.SortDir
            );

            var items = entities.Select(x => new InternListItemDto(
                Id:             x.Id,
                FirstName:      x.FirstName ?? string.Empty,
                LastName:       x.LastName ?? string.Empty,
                IdentityNumber: x.NationalId ?? string.Empty,   
                Email:          x.Email ?? string.Empty,
                Phone:          x.Phone ?? string.Empty,
                School:         x.School ?? string.Empty,
                Department:     x.Department ?? string.Empty,
                StartDate:      x.StartDate.ToDateTime(TimeOnly.MinValue),
                EndDate:        x.EndDate.HasValue 
                                   ? x.EndDate.Value.ToDateTime(TimeOnly.MinValue) 
                                   : (DateTime?)null,
                Status:         x.Status ?? string.Empty
            )).ToList();

            var result = new PagedResult<InternListItemDto>
            {
                Page = q.Page,
                PageSize = q.PageSize,
                Total = total,
                Items = items
            };

            return Result<PagedResult<InternListItemDto>>.Ok(result);
        }
    }
}
