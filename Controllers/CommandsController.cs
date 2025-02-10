using AutoMapper;
using CommandsServeice.Data;
using CommandsServeice.Dtos;
using CommandsServeice.Models;
using Microsoft.AspNetCore.Mvc;

namespace CommandsServeice.Controllers;

[ApiController, Route("api/commands/platforms/{platformId:int}/[controller]")]
public class CommandsController(ICommandsRepository repository, IMapper mapper) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CreateCommandForPlatform(int platformId, CommandCreateDto commandDto)
    {
        Console.WriteLine($"--> Hit CreateCommandForPlatform {platformId}");

        if (!await repository.PlatformExists(platformId))
            return NotFound();

        var command = mapper.Map<Command>(commandDto);

        await repository.CreateCommand(platformId, command);

        var commandReadDto = mapper.Map<CommandReadDto>(command);

        return CreatedAtRoute(nameof(GetCommandForPlatform), new {platformId, commandId = commandReadDto.Id }, commandReadDto);
    }

    [HttpGet]
    public async Task<IActionResult> GetCommandsForPlatform(int platformId)
    {
        Console.WriteLine($"--> Hit GetCommandsForPlatform {platformId}");

        if(!await repository.PlatformExists(platformId))
            return NotFound();
        
        var commands = await repository.GetCommandsForPlatform(platformId); 

        return Ok(mapper.Map<CommandReadDto>(commands));
    }

    [HttpGet("{commandId:int}", Name = "GetCommandForPlatform")]
    public async Task<IActionResult> GetCommandForPlatform(int platformId, int commandId)
    {
        Console.WriteLine($"--> Hit GetCommandForPlatform {platformId} / {commandId}");

        if (!await repository.PlatformExists(platformId))
            return NotFound();

        var command = await repository.GetCommand(platformId, commandId);

        if(command is null)
            return NotFound();

        return Ok(mapper.Map<CommandReadDto>(command));
    }
}
