using Microsoft.EntityFrameworkCore;
using StajyerTakip.Data;
using StajyerTakip.Models;
using StajyerTakip.Models.ViewModels;

namespace StajyerTakip.Services
{
    public class InternService : IInternService
    {
        private readonly AppDbContext _db;
        public InternService(AppDbContext db) => _db = db;

        public async Task<PaginatedResult<Intern>> ListAsync(
            string? q, string? status, int page, int pageSize,
            string sortField, string sortOrder)
        {
            if (page < 1) page = 1;
            if (pageSize < 1) pageSize = 10;

            var query = _db.Interns.AsQueryable();

            if (!string.IsNullOrWhiteSpace(q))
            {
                query = query.Where(i =>
                    i.FirstName.Contains(q) || i.LastName.Contains(q) ||
                    i.Email.Contains(q) ||
                    (i.Phone != null && i.Phone.Contains(q)) ||
                    (i.School != null && i.School.Contains(q)) ||
                    (i.Department != null && i.Department.Contains(q)));
            }

            if (!string.IsNullOrWhiteSpace(status))
                query = query.Where(i => i.Status == status);

            bool asc = sortOrder?.ToLower() != "desc";
            query = sortField switch
            {
                "FirstName"  => asc ? query.OrderBy(i => i.FirstName)   : query.OrderByDescending(i => i.FirstName),
                "LastName"   => asc ? query.OrderBy(i => i.LastName)    : query.OrderByDescending(i => i.LastName),
                "Email"      => asc ? query.OrderBy(i => i.Email)       : query.OrderByDescending(i => i.Email),
                "School"     => asc ? query.OrderBy(i => i.School)      : query.OrderByDescending(i => i.School),
                "Department" => asc ? query.OrderBy(i => i.Department)  : query.OrderByDescending(i => i.Department),
                "Status"     => asc ? query.OrderBy(i => i.Status)      : query.OrderByDescending(i => i.Status),
                _            => asc ? query.OrderBy(i => i.LastName).ThenBy(i => i.FirstName)
                                    : query.OrderByDescending(i => i.LastName).ThenByDescending(i => i.FirstName)
            };

            var total = await query.CountAsync();
            var items = await query.AsNoTracking()
                                   .Skip((page - 1) * pageSize)
                                   .Take(pageSize)
                                   .ToListAsync();

            return new PaginatedResult<Intern>(items, total, page, pageSize);
        }

        public Task<Intern?> GetAsync(int id)
            => _db.Interns.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id)!;

        public async Task<(bool Ok, string? Error)> CreateAsync(Intern model)
        {
            if (model.EndDate.HasValue && model.EndDate.Value < model.StartDate)
                return (false, "Bitiş tarihi başlangıçtan önce olamaz.");

            var existsNat  = await _db.Interns.AnyAsync(i => i.NationalId == model.NationalId);
            if (existsNat) return (false, "TC Kimlik No benzersiz olmalıdır.");

            var existsMail = await _db.Interns.AnyAsync(i => i.Email == model.Email);
            if (existsMail) return (false, "E-posta benzersiz olmalıdır.");

            model.CreatedAt = DateTime.UtcNow;
            model.UpdatedAt = DateTime.UtcNow;

            _db.Interns.Add(model); 
            return await SaveWithCatch();
        }

        public async Task<(bool Ok, string? Error)> UpdateAsync(Intern model)
        {
            if (model.EndDate.HasValue && model.EndDate.Value < model.StartDate)
                return (false, "Bitiş tarihi başlangıçtan önce olamaz.");

            var existsNat  = await _db.Interns.AnyAsync(i => i.NationalId == model.NationalId && i.Id != model.Id);
            if (existsNat) return (false, "TC Kimlik No benzersiz olmalıdır.");

            var existsMail = await _db.Interns.AnyAsync(i => i.Email == model.Email && i.Id != model.Id);
            if (existsMail) return (false, "E-posta benzersiz olmalıdır.");

            model.UpdatedAt = DateTime.UtcNow;
            _db.Entry(model).State = EntityState.Modified;

            return await SaveWithCatch();
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var m = await _db.Interns.FindAsync(id);
            if (m is null) return false;
            _db.Interns.Remove(m);
            await _db.SaveChangesAsync();
            return true;
        }

        private async Task<(bool Ok, string? Error)> SaveWithCatch()
        {
            try
            {
                await _db.SaveChangesAsync();
                return (true, null);
            }
            catch (DbUpdateException)
            {
                return (false, "Veri kaydedilirken bir hata oluştu. (Benzersizlik veya veri kısıtı)");
            }
        }
    }
}
