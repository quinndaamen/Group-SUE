using Microsoft.Extensions.DependencyInjection;
using SUE.Services.Sensors.Contracts;
using SUE.Services.Sensors.Internals;
using SUE.Services.Sensors.Models;

namespace SUE.Services.Sensors;

public static class DependencyInjection
{
    public static IServiceCollection AddMqttService(this IServiceCollection services)
    {
        services.AddSingleton<MqttMessageStore>();
        services.AddScoped<ISensorService, SensorService>();
        services.AddScoped<IGraphService, GraphService>();
        services.AddHostedService<MqttBackgroundService>();
        
        return services;
    }
}