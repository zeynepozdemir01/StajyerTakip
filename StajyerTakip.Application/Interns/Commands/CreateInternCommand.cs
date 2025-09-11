using MediatR;
using StajyerTakip.Application.Common;       
using StajyerTakip.Application.Interfaces;   
using StajyerTakip.Domain.Entities;

namespace StajyerTakip.Application.Interns.Commands;

public sealed record CreateInternCommand(Intern Entity) : IRequest<Result<int>>;

public sealed class CreateInternCommandHandler(IInternRepository repo)
    : IRequestHandler<CreateInternCommand, Result<int>>
{
    public async Task<Result<int>> Handle(CreateInternCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var id = await repo.AddAsync(request.Entity);
            return id > 0
                ? Result<int>.Ok(id)
                : Result<int>.Fail("Kayıt oluşturulamadı.");
        }
        catch (Exception ex)
        {
            return Result<int>.Fail(ex.Message);
        }
    }
}
