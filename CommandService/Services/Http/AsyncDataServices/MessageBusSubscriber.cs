using System.Text;
using CommandService.Services.Http.EventProcessing;
using CommandService.Services.RabbitMQ;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace CommandService.Services.Http.AsyncDataServices;

public class MessageBusSubscriber : BackgroundService
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<MessageBusSubscriber> _logger;
    private readonly IMessageBusService _service;
    
    public MessageBusSubscriber(IConfiguration configuration,
        ILogger<MessageBusSubscriber> logger,
        IMessageBusService service)
    {
        _configuration = configuration;
        _logger = logger;
        _service = service;
        service.BuildConnection();
    }
    

    public override void Dispose()
    {
        _service.Dispose();
        base.Dispose();
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        stoppingToken.ThrowIfCancellationRequested();
        _service.NotificationMessageReceiver();
        return Task.CompletedTask;
    }

}