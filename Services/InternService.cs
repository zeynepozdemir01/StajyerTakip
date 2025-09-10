using StajyerTakip.Application.Common;     // PaginatedResult
using StajyerTakip.Application.Interfaces; // IInternRepository
using StajyerTakip.Domain.Entities;

namespace StajyerTakip.Services;

public sealed class InternService : IInternService
{
    private readonly IInternRepository _repo;
    public InternService(IInternRepository repo) => _repo = repo;

    public async Task<PaginatedResult<Intern>> ListAsync(
        string? q, string? status, int page, int pageSize, string sortField, string sortOrder)
    {
        var (items, total) = await _repo.ListAsync(q, status, page, pageSize, sortField, sortOrder);
        // PaginatedResult ctor: (items, totalCount, page, pageSize)
        return new PaginatedResult<Intern>(items, total, page, pageSize);
    }

    public Task<Intern?> GetAsync(int id)
        => _repo.GetByIdAsync(id);

    public async Task<(bool ok, string? error)> CreateAsync(Intern model)
    {
        try
        {
            var id = await _repo.AddAsync(model);   // int döner (eklenen kaydın Id'si)
            return (id > 0, id > 0 ? null : "Kayıt oluşturulamadı.");
        }
        catch (Exception ex)
        {
            return (false, ex.Message);
        }
    }

    public async Task<(bool ok, string? error)> UpdateAsync(Intern model)
    {
        try
        {
            await _repo.UpdateAsync(model);
            return (true, null);
        }
        catch (Exception ex)
        {
            return (false, ex.Message);
        }
    }

    public async Task<bool> DeleteAsync(int id)
    {
        try
        {
            await _repo.DeleteAsync(id);
            return true;
        }
        catch
        {
            return false;
        }
    }
}
