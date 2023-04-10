using PlatformService.DTOs.Platform;

namespace PlatformService.Services.Http.AsyncDataServices;

public interface IMessageBusClient
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="publish"></param>
    /// <returns></returns>
    bool PublishPlatform(PlatformPublishDto publish);
    
    
}