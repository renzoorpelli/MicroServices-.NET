using RabbitMQ.Client;

namespace CommandService.Services.RabbitMQ;

public interface IMessageBusService
{
    IModel Channel { get; set; }
    string QueueName { get; set; }
    
    /// <summary>
    /// 
    /// </summary>
    public void Dispose();
    /// <summary>
    /// 
    /// </summary>
    public void BuildConnection();

    public void NotificationMessageReceiver();
}