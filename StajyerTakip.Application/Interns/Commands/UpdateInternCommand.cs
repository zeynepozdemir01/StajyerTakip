using MediatR;
using StajyerTakip.Application.Common;       
using StajyerTakip.Application.Interfaces;   
using StajyerTakip.Domain.Entities;

namespace StajyerTakip.Application.Interns.Commands;

public sealed record UpdateInternCommand(Intern Entity) : IRequest<Result>;

public sealed class UpdateInternCommandHandler(IInternRepository repo)
    : IRequestHandler<UpdateInternCommand, Result>
{
    public async Task<Result> Handle(UpdateInternCommand request, CancellationToken cancellationToken)
    {
        var model = request.Entity;
        if (model is null) return Result.Fail("Geçersiz model.");

        try
        {
            await repo.UpdateAsync(model);
            return Result.Ok();
        }
        catch (Exception ex)
        {
            return Result.Fail(ex.Message);
        }
    }
}
