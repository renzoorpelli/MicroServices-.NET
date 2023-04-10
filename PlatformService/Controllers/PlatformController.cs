using System.Text;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PlatformService.Data.Repositories;
using PlatformService.DTOs.Platform;
using PlatformService.Models;
using PlatformService.Services.Http.AsyncDataServices;
using PlatformService.Services.Http.SyncDataServices;

namespace PlatformService.Controllers;

[Route("api/[controller]"), ApiController]
public class PlatformController : ControllerBase
{
    private readonly IPlatformRepository _repository;
    private readonly IMapper _mapper;
    private readonly ILogger<PlatformController> _logger;
    private readonly ICommandDataClient _commandDataClient;
    private readonly IMessageBusClient _messageBusClient;

    public PlatformController(IPlatformRepository repository,
        IMapper mapper,
        ILogger<PlatformController> logger,
        ICommandDataClient commandDataClient,
        IMessageBusClient messageBusClient)
    {
        _repository = repository;
        _mapper = mapper;
        _logger = logger;
        _commandDataClient = commandDataClient;
        _messageBusClient = messageBusClient;
    }

    [HttpGet(Name = "GetPlatforms")]
    public ActionResult<IEnumerable<PlatformReadDto>> GetAll()
    {
        _logger.LogInformation("--> Obteniendo todas las plataformas...");

        var list = _repository.GetAll();

        return Ok(_mapper.Map<IEnumerable<PlatformReadDto>>(list));
    }

    [HttpGet("{id:guid}")]
    public ActionResult<PlatformReadDto> GetById(Guid id)
    {
        _logger.LogInformation($"--> Obteniendo la plataforma Id:{id} ...");

        var platform = _repository.GetById(id);

        return platform is not null
            ? Ok(_mapper.Map<PlatformReadDto>(platform))
            : NotFound();
    }

    [HttpPost]
    public ActionResult<PlatformReadDto> Create(PlatformCreateDto body)
    {
        _logger.LogInformation($"--> Creando la plataforma ...");

        var bodyRMapped = _mapper.Map<Platform>(body);
        _repository.Create(bodyRMapped);

        var platformReadDto = _mapper.Map<PlatformReadDto>(bodyRMapped);

        //Sync Message Send
        // _ = Task.Run( async () =>
        // {
        //     try
        //     {
        //         await _commandDataClient.SendPlatformToCommand(platformReadDto);
        //         _logger.LogInformation("--> Mensaje enviado sincronamente de manera exitosa.");
        //     }
        //     catch (Exception e)
        //     {
        //         StringBuilder sb = new();
        //         sb.Append("--> No se pudo notificar el mensaje sincronamente");
        //         sb.AppendLine(e.Message);
        //         _logger.LogError(sb.ToString());
        //     }
        // });

        _ = Task.Run(() =>
        {
            try
            {
                var platformPublishedDto = _mapper.Map<PlatformPublishDto>(platformReadDto);
                _messageBusClient.PublishPlatform(platformPublishedDto);
            }
            catch (Exception e)
            {
                StringBuilder sb = new();
                sb.Append("--> ERROR AL NOTIFICAR EL MENSAJE <--");
                sb.AppendLine(e.Message);
                _logger.LogError(sb.ToString());
            }
        });

        return _repository.Commit() ? new ObjectResult(bodyRMapped) { StatusCode = 201 } : BadRequest();
    }
}