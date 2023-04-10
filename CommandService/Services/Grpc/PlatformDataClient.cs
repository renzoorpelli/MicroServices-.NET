using System.Text;
using AutoMapper;
using CommandService.Data.Repositories.Interfaces;
using CommandService.Models;
using Grpc.Net.Client;
using PlatformService;

namespace CommandService.Services.Grpc;

public class PlatformDataClient : IPlatformDataClient
{
    private readonly IPlatformRepository _platformRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<PlatformDataClient> _logger;
    private readonly IConfiguration _configuration;

    public PlatformDataClient(IPlatformRepository platformRepository,
        IUnitOfWork unitOfWork,
        IMapper mapper,
        ILogger<PlatformDataClient> logger,
        IConfiguration configuration)
    {
        _platformRepository = platformRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
        _configuration = configuration;
    }

    private IEnumerable<Platform> ReturnAllPlatforms()
    {
        _logger.LogInformation("--> Llamada al servicio GRPC... SERVICIO: COMMAND");
        var adress = _configuration["Grpc:Platform"]!;
        var channel = GrpcChannel.ForAddress(adress);
        _logger.LogInformation($"--> Creando canal para la direccion: {adress}");
        var client = new GrpcPlatform.GrpcPlatformClient(channel);
        return CallGrpcSerice(client);
    }

    private IEnumerable<Platform> CallGrpcSerice(GrpcPlatform.GrpcPlatformClient client)
    {
        _logger.LogInformation("Obteniendo todas las plataformas. Servicio: GRPC");
        try
        {
            var request = new GetAllRequest();
            var response = client.GetAllPlatfoms(request);

            return _mapper.Map<IEnumerable<Platform>>(response.Platform);
        }
        catch (Exception e)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("Error al obtener todas las plataformas");
            stringBuilder.AppendLine(e.Message);
            _logger.LogError(stringBuilder.ToString());
            return Enumerable.Empty<Platform>();
        }
    }


    public bool SeedPlatformsToDataBase()
    {
        var platformsGrpc = ReturnAllPlatforms();
        if(platformsGrpc.Any())
        {
            _logger.LogInformation("Se detectaron plataformas en el servicio de Grpc...");
            foreach (var platform in platformsGrpc)
            {
                if (!_platformRepository.ExternalPlatformExist(platform.ExternalId))
                {
                    _platformRepository.CreatePlatform(platform);
                }
            }

            if(_unitOfWork.Commit())
            {
                _logger.LogInformation("Se registraron nuevas plataformas desde el servicio Grpc, por favor verifique");
                return true;
            }
        }
        _logger.LogInformation("No hay plataformas nuevas");
        return false;
    }
}