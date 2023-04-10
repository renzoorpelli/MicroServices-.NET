using Microsoft.EntityFrameworkCore;
using PlatformService.Data;
using PlatformService.Data.Repositories;
using PlatformService.Services.Grpc;
using PlatformService.Services.Http.AsyncDataServices;
using PlatformService.Services.Http.SyncDataServices;

namespace PlatformService.Configuration;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services,
        IConfiguration configuration, IWebHostEnvironment environment)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
        {
            if (environment.IsProduction())
            {
                Console.WriteLine("--> ENVIRONMENT: PRODUCTION");
                options.UseSqlServer(configuration.GetConnectionString("PlatformConn"));
            }
            else
            {
                options.UseInMemoryDatabase("InMemoryDb");
                Console.WriteLine($"--> ENVIRONMENT: {environment.EnvironmentName}");
            }
        });
        services.AddScoped<IPlatformRepository, PlatformRepository>();

        services.AddSingleton<ICommandDataClient, HttpCommandDataClients>();

        services.AddHttpClient("CommandService",
            client => { client.BaseAddress = new Uri(configuration["CommandServiceBaseAddress"]!); });

        services.AddSingleton<IMessageBusClient, MessageBusClient>();

        services.AddGrpc();

        return services;
    }

    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(Program).Assembly);
        services.AddLogging();

        return services;
    }
}