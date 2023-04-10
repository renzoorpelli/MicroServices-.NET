using CommandService.Data.Repositories.Interfaces;
using CommandService.Models;
using Microsoft.EntityFrameworkCore;

namespace CommandService.Data.Repositories;

public class CommandRepository : ICommandRepository
{
    private readonly IUnitOfWork _unitOfWork;

    public CommandRepository(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    
    public IEnumerable<Command> GetCommandsForPlatform(Guid platformId)
    {
        return _unitOfWork.Context.Commands
            .Where(c => c.PlatformId == platformId);
    }

    public Command? GetById(Guid platoformId, Guid commandId)
    {
        return _unitOfWork.Context.Commands
            .Where(c => c.PlatformId == platoformId && c.Id == commandId)
            .FirstOrDefault();
    }

    public bool Create(Guid platformId, Command? command)
    {
        if (PlatformExist(platformId) && command is not null)
        {
            command.PlatformId = platformId;
            _unitOfWork.Context.Commands.Add(command);
            return true;
        }
        return false;
    }

    public bool PlatformExist(Guid platformId)
    {
        return _unitOfWork.Context.Platforms.Any(p => p.Id == platformId);
    }
    
    public bool CommandExist(Guid commandId)
    {
        return _unitOfWork.Context.Commands.Any(c => c.Id == commandId);
    }
    
}