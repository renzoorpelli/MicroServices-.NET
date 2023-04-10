using AutoMapper;
using CommandService.Data.Repositories.Interfaces;
using CommandService.DTOs.Commands;
using CommandService.Models;
using Microsoft.AspNetCore.Mvc;

namespace CommandService.Controllers;

[Route("api/c/platforms/{platformId:guid}/[controller]"), ApiController]
public class CommandController : ControllerBase
{
    private readonly ILogger<CommandController> _logger;
    private readonly ICommandRepository _commandRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CommandController(ILogger<CommandController> logger,
        ICommandRepository commandRepository,
        IUnitOfWork unitOfWork,
        IMapper mapper)
    {
        _logger = logger;
        _commandRepository = commandRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    [HttpGet]
    public ActionResult<IEnumerable<CommandReadDto>> GetAllForPlatform(Guid platformId)
    {
        _logger.LogInformation($"--> Obteniedo los comandos de la plataforma ID: {platformId}");
        _logger.LogInformation("/GET platfoms/platformId/commands");
        if (!_commandRepository.PlatformExist(platformId))
        {
            return NotFound();
        }
        var commandsForPlatform = _commandRepository.GetCommandsForPlatform(platformId);
        return Ok(_mapper.Map<IEnumerable<CommandReadDto>>(commandsForPlatform));
    }
    
    
    [HttpGet("{commandId:guid}")]
    public ActionResult<IEnumerable<CommandReadDto>> GetByIdForPlatform(Guid platformId, Guid commandId)
    {
        _logger.LogInformation($"--> Obteniedo el comando ID: {commandId} de la plataforma ID: {platformId}");
        _logger.LogInformation("/GET platfoms/platformId/commands/commandId");
        if (!_commandRepository.PlatformExist(platformId) || !_commandRepository.CommandExist(commandId))
        {
            return NotFound();
        }
        var commandsForPlatform = _commandRepository.GetById(platformId, commandId);
        return Ok(_mapper.Map<CommandReadDto>(commandsForPlatform));
    }

    [HttpPost]
    public ActionResult<CommandReadDto> CreateCommandForPlatform(Guid platformId, CommandCreateDto command)
    {
        _logger.LogInformation($"--> Creando el comando para la plataforma ID: {platformId}");
        _logger.LogInformation("/POST platfoms/platformId/commands/");
        var commandCreate = _mapper.Map<Command>(command);
        if (_commandRepository.Create(platformId,commandCreate))
        {
            _unitOfWork.Commit();
            return new ObjectResult(commandCreate) {StatusCode = 201};
        }
        return NotFound();
    }

}