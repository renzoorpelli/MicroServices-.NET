using AutoMapper;
using Grpc.Core;
using PlatformService.Data.Repositories;

namespace PlatformService.Services.Grpc;

public class GrpcPlatformService : GrpcPlatform.GrpcPlatformBase
{
    private readonly IPlatformRepository _repository;
    private readonly IMapper _mapper;
    private readonly ILogger<GrpcPlatformService> _logger;

    public GrpcPlatformService(IPlatformRepository repository, IMapper mapper, ILogger<GrpcPlatformService> logger)
    {
        _repository = repository;
        _mapper = mapper;
        _logger = logger;
    }

    public override Task<PlatformResponse> GetAllPlatfoms(GetAllRequest request, ServerCallContext context)
    {
        _logger.LogInformation("Obteniendo todas las plataformas para el servicio gRPC");
        var response = new PlatformResponse();
        
        var platformsFromRepo = _repository.GetAll();
        
        foreach (var platform in platformsFromRepo)
        {
            response.Platform.Add(_mapper.Map<GrpcPlatformModel>(platform));
        }
        
        return  Task.FromResult(response);
    }
}