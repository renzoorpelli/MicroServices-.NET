using System.Text;
using Newtonsoft.Json;
using PlatformService.DTOs.Platform;
using RabbitMQ.Client;

namespace PlatformService.Services.Http.AsyncDataServices;

public class MessageBusClient : IMessageBusClient
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<MessageBusClient> _logger;
    
    private ConnectionFactory _connectionFactory;
    private IConnection _connection;
    private IModel _channel;

    public MessageBusClient(IConfiguration configuration, 
        ILogger<MessageBusClient> logger)
    {
        _configuration = configuration;
        _logger = logger;
        this.BuildConnection();
    }
    
    
    public bool PublishPlatform(PlatformPublishDto publish)
    {
        if (!_connection.IsOpen)
        {
            _logger.LogError("--> La conexion con RabbitMQ se encuentra cerrada, imposible encolar el mensje.");
            return false;
        }
        publish.Event = Event.PlatformPublished;
        var message = JsonConvert.SerializeObject(publish);
        _logger.LogInformation("--> La conexion con RabbitMQ esta abierta, encolando mensaje...");
        EnqueueMessage(message);
        
        return true;
    }

    private void EnqueueMessage(string message)
    {
        var body = Encoding.UTF8.GetBytes(message);
        _channel.BasicPublish(exchange: "trigger",
            routingKey: "",
            basicProperties: null,
            body: body);
        _logger.LogInformation("--> El mensaje se ha publicado correctamente");

    }
    private ConnectionFactory CreateFactory()
    {
        if (_connectionFactory is null)
        {
            _connectionFactory = new ConnectionFactory()
            {
                HostName = _configuration["RabbitMQ:Host"],
                Port = int.Parse(_configuration["RabbitMQ:Port"]!)
            };
        }
        return _connectionFactory;
    }
    
    private void BuildChannel(IConnection connection)
    {
        try
        {
            _logger.LogInformation("--> Creando el Canal de comunicacion con RabbitMQ");
            _channel = _connection.CreateModel();
            _channel.ExchangeDeclare(exchange: "trigger", type: ExchangeType.Fanout);
        }
        catch (Exception)
        {
            _logger.LogError("--> Error al crear el Canal con RabbitMQ");
        }
    }

    private void RabbitMQ_ConnectionShutdown(object sender, EventArgs args)
    {
        _logger.LogInformation("--> La conexion con RabbitMQ se ha cerrado");
    }

    public void Dispose()
    {
        _logger.LogInformation("--> MessageBus Disposed");
        if (_channel.IsOpen)
        {
            _channel.Close();
            _connection.Close();
        }
    }
    
    private void BuildConnection()
    {
        try
        {
            _logger.LogInformation("--> Creando la conexion con RabbitMQ");
            if (_connection is null)
            {
                _connection = CreateFactory().CreateConnection();
            }
            this.BuildChannel(_connection);

            _connection.ConnectionShutdown += RabbitMQ_ConnectionShutdown;
            
            _logger.LogInformation("--> Conexion establecida exitosamente");

        }
        catch (Exception)
        {
            _logger.LogError("--> Error al crear la conexion con RabbitMQ");
        }
    }
}