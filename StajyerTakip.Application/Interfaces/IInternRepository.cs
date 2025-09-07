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

    Task<Intern?> GetByIdAsync(int id);

    Task AddAsync(Intern entity);
    void  Update(Intern entity);
    Task DeleteAsync(int id);

    Task<int> SaveChangesAsync();
}
