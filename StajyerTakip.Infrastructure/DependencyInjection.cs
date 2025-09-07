using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StajyerTakip.Application.Interfaces;
using StajyerTakip.Infrastructure.Data;
using StajyerTakip.Infrastructure.Repositories;

namespace StajyerTakip.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration cfg)
    {
        services.AddDbContext<AppDbContext>(opt =>
            opt.UseSqlServer(cfg.GetConnectionString("Default")));

        services.AddScoped<IInternRepository, InternRepository>();
        return services;
    }
}
