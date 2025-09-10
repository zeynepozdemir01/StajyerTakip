using MediatR;
using StajyerTakip.Application.Common;
using StajyerTakip.Application.Interfaces;

namespace StajyerTakip.Application.Interns.Commands;

public sealed record DeleteInternCommand(int Id) : IRequest<Result>;

public sealed class DeleteInternCommandHandler
    : IRequestHandler<DeleteInternCommand, Result>
{
    private readonly IInternRepository _repo;

    public DeleteInternCommandHandler(IInternRepository repo) => _repo = repo;

    public async Task<Result> Handle(DeleteInternCommand request, CancellationToken ct)
    {
        try
        {
            await _repo.DeleteAsync(request.Id);
            return Result.Success();
        }
        catch (Exception ex)
        {
            return Result.Failure(ex.Message);
        }
    }
}
