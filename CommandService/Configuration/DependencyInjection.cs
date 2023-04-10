using CommandService.Data;
using CommandService.Data.Repositories;
using CommandService.Data.Repositories.Interfaces;
using CommandService.Services;
using CommandService.Services.Grpc;
using CommandService.Services.Http.AsyncDataServices;
using CommandService.Services.Http.EventProcessing;
using CommandService.Services.RabbitMQ;
using Microsoft.EntityFrameworkCore;

namespace CommandService.Configuration;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services,
        IWebHostEnvironment environment, IConfiguration configuration)
    {

        services.AddDbContext<ApplicationDbContext>(options =>
        {
            // if (environment.IsProduction())
            // {
                Console.WriteLine("--> ENVIRONMENT: PRODUCTION");
                options.UseSqlServer(configuration.GetConnectionString("CommandsDbConn"));
            // }
            // else
            // {
            //     Console.WriteLine("--> ENVIRONMENT: DEVELOPMENT");
            //     Console.WriteLine("--> Utilizando base de datos en memoria.");
            //     options.UseInMemoryDatabase("MemDb");
            // }
            
        });
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<ICommandRepository, CommandRepository>();
        services.AddScoped<IPlatformRepository, PlatformRepository>();
        services.AddSingleton<IMessageBusService, MessageBusService>();
        services.AddHostedService<PlatformsCheckerHostedService>();
        services.AddScoped<IPlatformDataClient, PlatformDataClient>();
        return services;
    }

    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(Program).Assembly);
        services.AddSingleton<IEventProcessor, EventProcessor>();
        services.AddHostedService<MessageBusSubscriber>();
        services.AddLogging();
        return services;
    }
}