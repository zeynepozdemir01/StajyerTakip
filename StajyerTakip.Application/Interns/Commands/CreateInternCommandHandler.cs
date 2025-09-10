using MediatR;
using StajyerTakip.Application.Common;
using StajyerTakip.Application.Interfaces;
using StajyerTakip.Domain.Entities;

namespace StajyerTakip.Application.Interns.Commands;

public sealed class CreateInternCommandHandler
    : IRequestHandler<CreateInternCommand, Result<int>>
{
    private readonly IInternRepository _repo;

    public CreateInternCommandHandler(IInternRepository repo) => _repo = repo;

    public async Task<Result<int>> Handle(CreateInternCommand request, CancellationToken ct)
    {
        try
        {
            await _repo.AddAsync(request.Model);
            return Result<int>.Success(request.Model.Id);
        }
        catch (Exception ex)
        {
            return Result<int>.Failure(ex.Message);
        }
    }
}
