using System.ComponentModel.DataAnnotations;
using StajyerTakip.Application.Interfaces;
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
        return new PaginatedResult<Intern>
        {
            Items = items,
            Page = page,
            PageSize = pageSize,
            TotalCount = total
        };
    }

    public Task<Intern?> GetAsync(int id) => _repo.GetByIdAsync(id);

    public async Task<(bool ok, string? error)> CreateAsync(Intern model)
    {
        var ctx = new ValidationContext(model);
        var results = new List<ValidationResult>();
        if (!Validator.TryValidateObject(model, ctx, results, validateAllProperties: true))
            return (false, string.Join("; ", results.Select(r => r.ErrorMessage)));

        await _repo.AddAsync(model);
        await _repo.SaveChangesAsync();
        return (true, null);
    }

    public async Task<(bool ok, string? error)> UpdateAsync(Intern model)
    {
        var existing = await _repo.GetByIdAsync(model.Id);
        if (existing is null) return (false, "Kayıt bulunamadı.");

        existing.FirstName  = model.FirstName;
        existing.LastName   = model.LastName;
        existing.NationalId = model.NationalId;
        existing.Email      = model.Email;
        existing.Phone      = model.Phone;
        existing.School     = model.School;
        existing.Department = model.Department;
        existing.StartDate  = model.StartDate;
        existing.EndDate    = model.EndDate;
        existing.Status     = model.Status;
        existing.UpdatedAt  = DateTime.UtcNow;

        _repo.Update(existing);
        await _repo.SaveChangesAsync();
        return (true, null);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        await _repo.DeleteAsync(id);
        var affected = await _repo.SaveChangesAsync();
        return affected > 0;
    }
}
