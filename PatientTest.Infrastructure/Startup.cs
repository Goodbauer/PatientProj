using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PatientTest.Core.Interfaces.Generic;

namespace PatientTest.Infrastructure;

public static class Startup
{
    public static void AddContext(this IServiceCollection services, string connectionString)
    {
        services.AddDbContext<Context>(opt => opt.UseNpgsql(connectionString));
    }

    public static void AddRepositoryInfrastructure(this IServiceCollection services)
    {
        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
    }
}