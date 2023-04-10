using AutoMapper;
using PlatformService.DTOs.Platform;
using PlatformService.Models;

namespace PlatformService.Profiles;

public class PlatformProfile : Profile
{
    public PlatformProfile()
    {
        //(Model) Source -> (Dto) Target 
        CreateMap<Platform, PlatformReadDto>();
        CreateMap<PlatformCreateDto, Platform>();
        CreateMap<PlatformReadDto, PlatformPublishDto>();

        CreateMap<Platform, GrpcPlatformModel>()
            //config for each member of GrpcPlatformModel
            .ForMember(destinationMember: dest=> dest.PlatformId, opt=> opt.MapFrom(from => from.Id))
            .ForMember(destinationMember: dest=> dest.Name, opt=> opt.MapFrom(from => from.Name))
            .ForMember(destinationMember: dest=> dest.Publisher, opt=> opt.MapFrom(from => from.Publisher))
            .ForMember(destinationMember: dest=> dest.Cost, opt=> opt.MapFrom(from => from.Cost));
    }
}