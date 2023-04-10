using CommandService.Models;

namespace CommandService.Data.Repositories.Interfaces;

public interface IPlatformRepository
{
    
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    IEnumerable<Platform> GetAll();

    /// <summary>
    /// 
    /// </summary>
    /// <param name="platform"></param>
    bool CreatePlatform(Platform platform);

    bool ExternalPlatformExist(Guid externalId);
}