using api.Repositories;
using api.Repositories.Interfaces;
using api.Services;
using api.Services.Interfaces;

namespace api.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {

        services
            .AddScoped<IAuthService, AuthService>()
            .AddScoped<ITokenGeneratorService, TokenGeneratorService>()
            .AddScoped<ITaskRepository, TaskRepository>()
            .AddScoped<ITaskService, TaskService>();

        return services;
    }
}

