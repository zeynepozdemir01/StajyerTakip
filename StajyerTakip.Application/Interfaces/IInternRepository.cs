using StajyerTakip.Domain.Entities;

namespace StajyerTakip.Application.Interfaces;

public interface IInternRepository
{
    Task<(IReadOnlyList<Intern> Items, int TotalCount)> ListAsync(
        string? q,
        string? status,
        int page,
        int pageSize,
        string sortField,
        string sortOrder);

    Task<Intern?> FindByIdAsync(int id);
    Task<Intern?> GetByIdAsync(int id);

    Task<int> AddAsync(Intern entity);
    Task UpdateAsync(Intern entity);
    Task DeleteAsync(int id);

    Task<int> SaveChangesAsync();
}
