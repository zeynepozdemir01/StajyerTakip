using StajyerTakip.Domain.Entities;

namespace StajyerTakip.Services;

public sealed class PaginatedResult<T>
{
    public IReadOnlyList<T> Items { get; init; } = Array.Empty<T>();
    public int Page { get; init; }
    public int PageSize { get; init; }
    public int TotalCount { get; init; }
}

public interface IInternService
{
    Task<PaginatedResult<Intern>> ListAsync(
        string? q, string? status, int page, int pageSize, string sortField, string sortOrder);

    Task<Intern?> GetAsync(int id);

    Task<(bool ok, string? error)> CreateAsync(Intern model);
    Task<(bool ok, string? error)> UpdateAsync(Intern model);
    Task<bool> DeleteAsync(int id);
}
