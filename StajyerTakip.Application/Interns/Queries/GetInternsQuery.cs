using MediatR;
using StajyerTakip.Application.Common;
using StajyerTakip.Domain.Entities;

namespace StajyerTakip.Application.Interns.Queries;

public sealed record GetInternsQuery(
    string? Q,
    string? Status,
    int Page,
    int PageSize,
    string SortField,
    string SortOrder
) : IRequest<Result<PaginatedResult<Intern>>>;
