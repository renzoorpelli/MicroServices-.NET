using CommandService.Data.Repositories.Interfaces;
using CommandService.Models;

namespace CommandService.Data.Repositories;

public class PlatformRepository : IPlatformRepository
{
    private readonly IUnitOfWork _unitOfWork;
    
    public PlatformRepository(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
   
    public IEnumerable<Platform> GetAll()
    {
        return _unitOfWork.Context.Platforms.ToList();
    }

    public bool CreatePlatform(Platform? platform)
    {
        if (platform is not null)
        {
            _unitOfWork.Context.Platforms.Add(platform);
            return true;
        }
        return false;
    }

    public bool ExternalPlatformExist(Guid externalId)
    {
        return _unitOfWork.Context.Platforms.Any(p => p.ExternalId == externalId);
    }
}