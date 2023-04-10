using CommandService.Models;

namespace CommandService.Services.Grpc;

public interface IPlatformDataClient
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="platforms"></param>
    /// <returns></returns>
    bool SeedPlatformsToDataBase();
}