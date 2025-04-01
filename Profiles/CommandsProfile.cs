using AutoMapper;
using CommandsServeice.Dtos;
using CommandsServeice.Dtos.Events;
using CommandsServeice.Models;
using PlatformService;

namespace CommandsServeice.Profiles;

public class CommandsProfile : Profile
{
    public CommandsProfile()
    {
        CreateMap<Platform, PlatformReadDTO>();

        CreateMap<CommandCreateDto, Command>();

        CreateMap<CommandReadDTO, Command>();

        CreateMap<Command, CommandReadDTO>();

        CreateMap<PlatformPublishedEvent, Platform>()
            .ForMember(dest => dest.ExternalId, opt => opt.MapFrom(src => src.Id));

        CreateMap<GrpcPlatformModel, Platform>()
            .ForMember(dest => dest.ExternalId, opt => opt.MapFrom(src => src.PlatformId))
            .ForMember(dest => dest.Commands, opt => opt.Ignore());
    }
}
