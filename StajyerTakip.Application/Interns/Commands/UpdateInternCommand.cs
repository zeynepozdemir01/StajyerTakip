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
        var entity = await _repo.FindByIdAsync(request.Model.Id);
        if (entity is null)
            return Result.Fail("Kayıt bulunamadı.");

        // İlgili alanları güncelle
        entity.FirstName  = request.Model.FirstName;
        entity.LastName   = request.Model.LastName;
        entity.Email      = request.Model.Email;
        entity.Phone      = request.Model.Phone;
        entity.School     = request.Model.School;
        entity.Department = request.Model.Department;
        entity.NationalId = request.Model.NationalId;
        entity.StartDate  = request.Model.StartDate;
        entity.EndDate    = request.Model.EndDate;
        entity.Status     = request.Model.Status;

        await _repo.UpdateAsync(entity);
        return Result.Ok();
    }
}
