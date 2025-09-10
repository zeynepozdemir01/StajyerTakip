using StajyerTakip.Domain.Entities;
using StajyerTakip.Application.Common;

namespace StajyerTakip.Services
{
    public interface IInternService
    {
        Task<PaginatedResult<Intern>> ListAsync(
            string? q, string? status, int page, int pageSize, string sortField, string sortOrder);

        Task<Intern?> GetAsync(int id);

        Task<(bool ok, string? error)> CreateAsync(Intern model);

        Task<(bool ok, string? error)> UpdateAsync(Intern model);

        Task<bool> DeleteAsync(int id);
    }
}
