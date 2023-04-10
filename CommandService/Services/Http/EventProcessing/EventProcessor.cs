using System.Text.Json.Serialization;
using AutoMapper;
using CommandService.Data.Repositories.Interfaces;
using CommandService.DTOs.Event;
using CommandService.DTOs.Platforms;
using CommandService.Models;
using Newtonsoft.Json;
using Event = CommandService.DTOs.Platforms.Event;

namespace CommandService.Services.Http.EventProcessing;

public class EventProcessor : IEventProcessor
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly IMapper _mapper;
    private readonly ILogger<EventProcessor> _logger;

    public EventProcessor(IServiceScopeFactory scopeFactory,
        IMapper mapper,
        ILogger<EventProcessor> logger)
    {
        _scopeFactory = scopeFactory;
        _mapper = mapper;
        _logger = logger;
    }

    public void ProcessEvent(string message)
    {
        var eventType = DeterminateEvent(message);
        if (eventType == Event.PlatformPublished)
        { 
            _logger.LogInformation("--> Añadiendo la plataforma...");
            AddPlatform(message);
        }
    }

    private void AddPlatform(string platformPublishedMessage)
    {
        using (var serviceScope = _scopeFactory.CreateScope())
        {
            var repoitoryService = serviceScope.ServiceProvider.GetService<IPlatformRepository>()!;
            var unitOfWorkService = serviceScope.ServiceProvider.GetService<IUnitOfWork>()!;
            var messageToPlatform = JsonConvert.DeserializeObject<PlatformPublishDto>(platformPublishedMessage);

            try
            {
                var mapPlatform = _mapper.Map<Platform>(messageToPlatform);
                _logger.LogInformation("--> Verificando si ya existe la plataforma indicada...");
                if (!repoitoryService.ExternalPlatformExist(mapPlatform.ExternalId))
                {
                    _logger.LogInformation("--> Guardando la plataforma en la base de datos...");   
                    repoitoryService.CreatePlatform(mapPlatform);
                    unitOfWorkService.Commit();
                    _logger.LogInformation("--> Plataforma añadida exitosamente a la base de datos");   
                }
            }
            catch (Exception)
            {
               _logger.LogInformation("--> ERROR: intenando agregar la plataforma desde RabbitMQ a la base de datos");
            }
        }
    }


    private Event DeterminateEvent(string notificationMessage)
    {
        _logger.LogInformation("--> Determinando tipo de evento");

        var eventType = JsonConvert.DeserializeObject<GenericEventDto>(notificationMessage)!;

        switch ((int)eventType.Event)
        {
            case 1:
                _logger.LogInformation("--> Evento = Platform Publised ");
                return Event.PlatformPublished;
            default:
                _logger.LogInformation("--> Evento = Indeterminado ");
                _logger.LogInformation("--> Se suspende el flujo de ejeción del servicio"); 
                return Event.Undeterminated;
        }
    }
}