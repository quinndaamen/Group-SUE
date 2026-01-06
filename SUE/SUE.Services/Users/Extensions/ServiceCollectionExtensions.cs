using Microsoft.Extensions.DependencyInjection;
using SUE.Services.Users.Contracts;
using SUE.Services.Users.Internals;

namespace SUE.Services.Users.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddUsersServices(this IServiceCollection services)
        {
            services.AddScoped<IUserOnboardingService, UserOnboardingService>();
            return services;
        }
    }
}