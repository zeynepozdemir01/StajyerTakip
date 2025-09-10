using MediatR;
using StajyerTakip.Application.Common;
using StajyerTakip.Application.Interfaces;
using StajyerTakip.Domain.Entities;

namespace StajyerTakip.Application.Interns.Commands;

public sealed record UpdateInternCommand(Intern Model) : IRequest<Result>;

public sealed class UpdateInternCommandHandler
    : IRequestHandler<UpdateInternCommand, Result>
{
    private readonly IInternRepository _repo;

    public UpdateInternCommandHandler(IInternRepository repo) => _repo = repo;

    public async Task<Result> Handle(UpdateInternCommand request, CancellationToken ct)
    {
        try
        {
            await _repo.UpdateAsync(request.Model);
            return Result.Success();
        }
        catch (Exception ex)
        {
            return Result.Failure(ex.Message);
        }
    }
}
