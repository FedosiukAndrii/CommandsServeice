using AutoMapper;
using CommandsServeice.Dtos;
using CommandsServeice.Dtos.Events;
using CommandsServeice.Models;

namespace CommandsServeice.Profiles;

public class CommandsProfile : Profile
{
    public CommandsProfile()
    {
        CreateMap<Platform, PlatformReadDTO>();
        CreateMap<CommandReadDTO, Command>();
        CreateMap<Command, CommandReadDTO>();

        CreateMap<PlatformPublishedEvent, Platform>()
            .ForMember(dest => dest.ExternalId, opt => opt.MapFrom(src => src.Id));
    }
}
