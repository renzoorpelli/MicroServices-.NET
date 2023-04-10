using CommandService.Models;

namespace CommandService.Data.Repositories.Interfaces;

public interface ICommandRepository 
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="platformId"></param>
    /// <returns></returns>
    IEnumerable<Command> GetCommandsForPlatform(Guid platformId);
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="platoformId"></param>
    /// <param name="commandId"></param>
    /// <returns></returns>
    Command? GetById(Guid platoformId, Guid commandId);
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="platformId"></param>
    /// <param name="command"></param>
    /// <returns></returns>
    bool Create(Guid platformId, Command command);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="platformId"></param>
    /// <returns></returns>
    bool PlatformExist(Guid platformId);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="commandId"></param>
    /// <returns></returns>
    bool CommandExist(Guid commandId);
}