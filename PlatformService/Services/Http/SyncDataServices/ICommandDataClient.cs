using PlatformService.DTOs.Platform;

namespace PlatformService.Services.Http.SyncDataServices;

public interface ICommandDataClient
{
    Task<bool> SendPlatformToCommand(PlatformReadDto platform);
}