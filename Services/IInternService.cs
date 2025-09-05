using StajyerTakip.Models;
using StajyerTakip.Models.ViewModels;

namespace StajyerTakip.Services
{
    public interface IInternService
    {
        Task<PaginatedResult<Intern>> ListAsync(
            string? q, string? status, int page, int pageSize,
            string sortField, string sortOrder);

        Task<Intern?> GetAsync(int id);

        Task<(bool Ok, string? Error)> CreateAsync(Intern model);

        Task<(bool Ok, string? Error)> UpdateAsync(Intern model);

        Task<bool> DeleteAsync(int id);
    }
}
