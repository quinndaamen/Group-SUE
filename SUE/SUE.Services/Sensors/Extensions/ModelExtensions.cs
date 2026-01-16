using Microsoft.Extensions.DependencyInjection;
using SUE.Services.Sensors.Contracts;
using SUE.Services.Users.Contracts;
using SUE.Services.Users.Internals;

namespace SUE.Services.Sensors.Extensions
{
    public static class ModelExtensions
    {
        public static IServiceCollection AddUsersServices(this IServiceCollection services)
        {
            services.AddScoped<ISensorService, SensorService>();
            return services;
        }

    }
}