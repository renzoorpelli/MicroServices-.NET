using AutoMapper;
using CommandService.Data.Repositories.Interfaces;
using CommandService.DTOs.Platforms;
using Microsoft.AspNetCore.Mvc;

namespace CommandService.Controllers;

[Route("api/c/[controller]"), ApiController]
public class PlatformController : ControllerBase
{
    private readonly ILogger<PlatformController> _logger;
    private readonly IPlatformRepository _platformRepository;
    private readonly IMapper _mapper;

    public PlatformController(ILogger<PlatformController> logger,
        IPlatformRepository platformRepository, 
        IMapper mapper)
    {
        _logger = logger;
        _platformRepository = platformRepository;
        _mapper = mapper;
    }

    [HttpGet]
    public ActionResult<IEnumerable<PlatformReadDto>> GetPlatforms()
    {
        _logger.LogInformation("--> Obteniendo todas las plataformas ... /GET: Platforms");
        var platforms = _platformRepository.GetAll();
        return Ok(_mapper.Map<IEnumerable<PlatformReadDto>>(platforms));
    }
}