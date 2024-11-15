using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Promo.DAL.Data;
using Promo.DAL.Interceptors;
using Promo.DAL.Repositories;
using Promo.Domain.Entities;
using Promo.Domain.Interfaces.Repositories;

namespace Promo.DAL.DependencyInjection;

public static class DependencyInjection
{
    public static void AddDataAccessLayer(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("SQLite");

        services.AddSingleton<AuditingInterceptor>();
        services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseSqlite(connectionString);
        });
        
        services.AddScoped<IDBInitializer, DBInitializer>();
        services.InitRepositories();
    }

    private static void InitRepositories(this IServiceCollection services)
    {
        services.AddScoped<IBaseRepository<User>, BaseRepository<User>>();
        services.AddScoped<IBaseRepository<Country>, BaseRepository<Country>>();
        services.AddScoped<IBaseRepository<Province>, BaseRepository<Province>>();
    }
}