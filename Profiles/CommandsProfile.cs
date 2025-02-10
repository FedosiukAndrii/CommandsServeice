using AutoMapper;
using CommandsServeice.Dtos;
using CommandsServeice.Models;

namespace CommandsServeice.Profiles;

public class CommandsProfile : Profile
{
    public CommandsProfile()
    {
        CreateMap<Platform, PlatformReadDto>();
        CreateMap<CommandReadDto, Command>();
        CreateMap<Command, CommandReadDto>();
    }
}
