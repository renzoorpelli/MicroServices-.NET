namespace CommandService.Services.Http.EventProcessing;

public interface IEventProcessor
{
    /// <summary>
    /// Procesa el mensaje segun el valor de la propiedad evento que este posee.
    /// 1 -> PlatformPublished, quiere decir que la plataforma fue creada exitosamente
    /// la plataforma se procesara y guardara en la base de datos
    /// </summary>
    /// <param name="message">Mensaje del ServiceBus serializado en Json</param>
    void ProcessEvent(string message);
}