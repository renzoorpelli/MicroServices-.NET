using System.Text;
using CommandService.Services.Http.EventProcessing;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace CommandService.Services.RabbitMQ;

public class MessageBusService : IMessageBusService
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<MessageBusService> _logger;
    private readonly IEventProcessor _eventProcessor;

    private ConnectionFactory? _connectionFactory;
    private IConnection? _connection;

    public MessageBusService(IConfiguration configuration,
        ILogger<MessageBusService> logger,
        IEventProcessor eventProcessor)
    {
        _configuration = configuration;
        _logger = logger;
        _eventProcessor = eventProcessor;
    }

    public IModel Channel { get; set; }
    public string QueueName { get; set; }


    private ConnectionFactory BuildFactory()
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

    public void BuildConnection()
    {
        try
        {
            _logger.LogInformation("--> Creando la conexion con RabbitMQ");
            if (_connection is null)
            {
                _connection = BuildFactory().CreateConnection();
            }

            this.BuildChannel(_connection);

            _connection.ConnectionShutdown += RabbitMQ_ConnectionShutdown;

            _logger.LogInformation("--> Conexion establecida exitosamente");

            BuildQueue(Channel);
        }
        catch (Exception)
        {
            _logger.LogError("--> Error al crear la conexion con RabbitMQ");
        }
    }

    public void NotificationMessageReceiver()
    {
        var consumer = new EventingBasicConsumer(Channel);

        consumer.Received += (ModuleHandle, ea) =>
        {
            _logger.LogInformation("Evento Recibido!");
            var notificationMessage = Encoding.UTF8.GetString(ea.Body.ToArray());

            _eventProcessor.ProcessEvent(notificationMessage);
        };
        Channel.BasicConsume(QueueName, true, consumer);
    }

    private void BuildChannel(IConnection connection)
    {
        try
        {
            _logger.LogInformation("--> Creando el Canal de comunicacion con RabbitMQ");
            Channel = _connection!.CreateModel();
            Channel.ExchangeDeclare(exchange: "trigger", type: ExchangeType.Fanout);
        }
        catch (Exception ex)
        {
            StringBuilder sb = new();
            sb.Append("--> Error al crear el Canal con RabbitMQ");
            sb.AppendLine(ex.Message);
            _logger.LogError(sb.ToString());
        }
    }

    private void BuildQueue(IModel channel)
    {
        QueueName = channel.QueueDeclare().QueueName;

        channel.QueueBind(queue: QueueName,
            exchange: "trigger",
            routingKey: "");
        _logger.LogInformation("--> Escuchando en el Message Bus...");
    }


    private void RabbitMQ_ConnectionShutdown(object sender, EventArgs args)
    {
        _logger.LogInformation("--> La conexion con RabbitMQ se ha cerrado");
    }


    public void Dispose()
    {
        if (Channel.IsOpen)
        {
            Channel.Close();
            _connection?.Close();
        }
    }
}