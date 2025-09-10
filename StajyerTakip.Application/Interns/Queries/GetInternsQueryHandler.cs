using MediatR;
using StajyerTakip.Application.Common;
using StajyerTakip.Application.Interfaces;
using StajyerTakip.Domain.Entities;

namespace StajyerTakip.Application.Interns.Queries;

public sealed class GetInternsQueryHandler
    : IRequestHandler<GetInternsQuery, Result<PaginatedResult<Intern>>>
{
    private readonly IInternRepository _repo;

    public GetInternsQueryHandler(IInternRepository repo) => _repo = repo;

    public async Task<Result<PaginatedResult<Intern>>> Handle(
        GetInternsQuery request,
        CancellationToken cancellationToken)
    {
        var (items, total) = await _repo.ListAsync(
            request.Q,
            request.Status,
            request.Page,
            request.PageSize,
            request.SortField,
            request.SortOrder
        );

        var page = new PaginatedResult<Intern>(items, request.Page, request.PageSize, total);
        return Result<PaginatedResult<Intern>>.Success(page);
    }
}
