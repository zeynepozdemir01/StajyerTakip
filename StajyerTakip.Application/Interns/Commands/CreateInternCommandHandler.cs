using MediatR;
using StajyerTakip.Application.Common;
using StajyerTakip.Application.Interfaces;

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
            var id = await _repo.AddAsync(request.Model);
            return id > 0 ? Result<int>.Ok(id) : Result<int>.Fail("Kayıt oluşturulamadı.");
        }
        catch (Exception ex)
        {
            return Result<int>.Fail(ex.Message);
        }
    }
}
