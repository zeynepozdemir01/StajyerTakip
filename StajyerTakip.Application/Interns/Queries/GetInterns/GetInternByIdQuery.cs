using MediatR;
using StajyerTakip.Application.Common;
using StajyerTakip.Application.Interfaces;
using StajyerTakip.Domain.Entities;

namespace StajyerTakip.Application.Interns.Queries;

public sealed record GetInternByIdQuery(int Id) : IRequest<Result<Intern?>>;

public sealed class GetInternByIdQueryHandler
    : IRequestHandler<GetInternByIdQuery, Result<Intern?>>
{
    private readonly IInternRepository _repo;
    public GetInternByIdQueryHandler(IInternRepository repo) => _repo = repo;

    public async Task<Result<Intern?>> Handle(GetInternByIdQuery request, CancellationToken ct)
    {
        var entity = await _repo.FindByIdAsync(request.Id);
        return entity is null
            ? Result<Intern?>.Fail("Kayıt bulunamadı.")
            : Result<Intern?>.Ok(entity);
    }
}
