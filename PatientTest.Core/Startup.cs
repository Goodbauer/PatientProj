using Microsoft.Extensions.DependencyInjection;
using PatientTest.Core.Interfaces.Services;
using PatientTest.Core.Services;

namespace PatientTest.Core;

public static class Startup
{
    public static void AddCustomService(this IServiceCollection services)
    {
        services.AddScoped(typeof(IPatientService), typeof(PatientService));
    }
}