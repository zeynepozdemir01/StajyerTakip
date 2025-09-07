using StajyerTakip.Infrastructure.Data;
using StajyerTakip.Domain.Entities;

namespace StajyerTakip.Data
{
    public static class DbSeeder
    {
        public static async Task SeedAsync(AppDbContext db)
        {
            if (db.Interns.Any()) return;

            var today = DateOnly.FromDateTime(DateTime.Today);

            var list = new List<Intern>
            {
                new Intern {
                    FirstName="Ahmet", LastName="Yılmaz", NationalId="12345678901",
                    Email="ahmet.yilmaz@example.com", Phone="05001112233",
                    School="Orta Doğu Teknik Üniversitesi", Department="Bilgisayar Müh.",
                    StartDate=today.AddDays(-60), EndDate=null, Status="Aktif",
                    CreatedAt=DateTime.UtcNow, UpdatedAt=DateTime.UtcNow
                },
                new Intern {
                    FirstName="Zeynep", LastName="Özdemir", NationalId="12345678902",
                    Email="zeynep.ozdemir@example.com", Phone="05005556677",
                    School="Berlin Teknik Üniversitesi", Department="Yazılım Müh.",
                    StartDate=today.AddDays(-30), EndDate=null, Status="Aktif",
                    CreatedAt=DateTime.UtcNow, UpdatedAt=DateTime.UtcNow
                },
                new Intern {
                    FirstName="Mehmet", LastName="Demir", NationalId="12345678903",
                    Email="mehmet.demir@example.com", Phone="05009998877",
                    School="İTÜ", Department="Elektrik-Elektronik",
                    StartDate=today.AddDays(-120), EndDate=today.AddDays(-10), Status="Pasif",
                    CreatedAt=DateTime.UtcNow, UpdatedAt=DateTime.UtcNow
                },
                new Intern {
                    FirstName="Elif", LastName="Çelik", NationalId="12345678904",
                    Email="elif.celik@example.com",Phone="05005699230",
                    School="Boğaziçi", Department="Yönetim Bilişim Sistemleri",
                    StartDate=today.AddDays(-15), EndDate=null, Status="Aktif",
                    CreatedAt=DateTime.UtcNow, UpdatedAt=DateTime.UtcNow
                }
            };

            db.Interns.AddRange(list);
            await db.SaveChangesAsync();
        }
    }
}
