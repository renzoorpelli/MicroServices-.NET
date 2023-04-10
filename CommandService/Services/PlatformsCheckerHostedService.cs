using System.ComponentModel.DataAnnotations;
using CommandService.Services.Grpc;

namespace CommandService.Services;

public class PlatformsCheckerHostedService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;

    public PlatformsCheckerHostedService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }
    
    private Task<bool> DoWork(CancellationToken stoppingToken)
    {
        using (var scope = _serviceProvider.CreateScope())
        {
            var platformDataClient = scope.ServiceProvider.GetService<IPlatformDataClient>()!;
            if (platformDataClient.SeedPlatformsToDataBase())
            {
                return Task.FromResult(true);
            }
        }
        return Task.FromResult(false);
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
       return Task.FromResult(DoWork(stoppingToken));
    }
    

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return base.StopAsync(cancellationToken);
    }
}