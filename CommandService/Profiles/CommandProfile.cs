using AutoMapper;
using CommandService.DTOs.Commands;
using CommandService.DTOs.Platforms;
using CommandService.Models;
using PlatformService;

namespace CommandService.Profiles;

public class CommandProfile : Profile
{
    public CommandProfile()
    {
        // commands
        CreateMap<Command, CommandReadDto>();
        CreateMap<CommandCreateDto, Command>();
        
        //platforms
        CreateMap<Platform, PlatformReadDto>();
        CreateMap<PlatformPublishDto, Platform>()
            .ForMember(p => p.ExternalId, 
                opt => opt.MapFrom(src => src.Id));
        //GrpcResponse
        CreateMap<GrpcPlatformModel, Platform>()
            .ForMember(p => p.ExternalId, opt => opt.MapFrom(src => src.PlatformId))
            .ForMember(p => p.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(p => p.Publisher, opt => opt.MapFrom(src => src.Publisher))
            .ForMember(p => p.Cost, opt => opt.MapFrom(src => src.Cost))
            .ForMember(p => p.Commands, opt => opt.Ignore());

    }
}