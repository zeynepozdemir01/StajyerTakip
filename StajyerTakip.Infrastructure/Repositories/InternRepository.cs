using Microsoft.EntityFrameworkCore;
using StajyerTakip.Application.Interfaces;
using StajyerTakip.Domain.Entities;
using StajyerTakip.Infrastructure.Data;

namespace StajyerTakip.Infrastructure.Repositories;

public sealed class InternRepository : IInternRepository
{
    private readonly AppDbContext _db;
    public InternRepository(AppDbContext db) => _db = db;

    public async Task<(IReadOnlyList<Intern> Items, int TotalCount)> ListAsync(
        string? q, string? status, int page, int pageSize, string sortField, string sortOrder)
    {
        var query = _db.Interns.AsQueryable();

        if (!string.IsNullOrWhiteSpace(q))
        {
            var term = q.Trim().ToLower();
            query = query.Where(i =>
                i.FirstName!.ToLower().Contains(term) ||
                i.LastName!.ToLower().Contains(term)  ||
                (i.Email ?? "").ToLower().Contains(term) ||
                (i.Phone ?? "").ToLower().Contains(term) ||
                (i.School ?? "").ToLower().Contains(term) ||
                (i.Department ?? "").ToLower().Contains(term) ||
                i.NationalId.Contains(term)
            );
        }

        if (!string.IsNullOrWhiteSpace(status))
            query = query.Where(i => i.Status == status);

        bool asc = string.Equals(sortOrder, "asc", StringComparison.OrdinalIgnoreCase);
        query = (sortField?.ToLower()) switch
        {
            "firstname"  => asc ? query.OrderBy(i => i.FirstName)  : query.OrderByDescending(i => i.FirstName),
            "lastname"   => asc ? query.OrderBy(i => i.LastName)   : query.OrderByDescending(i => i.LastName),
            "email"      => asc ? query.OrderBy(i => i.Email)      : query.OrderByDescending(i => i.Email),
            "school"     => asc ? query.OrderBy(i => i.School)     : query.OrderByDescending(i => i.School),
            "department" => asc ? query.OrderBy(i => i.Department) : query.OrderByDescending(i => i.Department),
            "status"     => asc ? query.OrderBy(i => i.Status)     : query.OrderByDescending(i => i.Status),
            _            => asc ? query.OrderBy(i => i.LastName).ThenBy(i => i.FirstName)
                                : query.OrderByDescending(i => i.LastName).ThenByDescending(i => i.FirstName)
        };

        var total = await query.CountAsync();

        var items = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (items, total);
    }

    public Task<Intern?> GetByIdAsync(int id)
        => _db.Interns.FirstOrDefaultAsync(i => i.Id == id)!;

    public async Task AddAsync(Intern entity)
        => await _db.Interns.AddAsync(entity);

    public void Update(Intern entity)
        => _db.Interns.Update(entity);

    public async Task DeleteAsync(int id)
    {
        var e = await _db.Interns.FindAsync(id);
        if (e != null) _db.Interns.Remove(e);
    }

    public Task<int> SaveChangesAsync() => _db.SaveChangesAsync();
}
