using AutoMapper;
using CommandsServeice.Data;
using CommandsServeice.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace CommandsServeice.Controllers;

[ApiController, Route("api/commands/[controller]")]
public class PlatformsController(ICommandsRepository repository, IMapper mapper) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetPlatforms()
    {
        Console.WriteLine("--> Getting Platforms from CommandsService");

        var platformItems = await repository.GetAllPlatforms();

        return Ok(mapper.Map<IEnumerable<PlatformReadDto>>(platformItems));
    }

    [HttpPost]
    public IActionResult TestInboundConnection()
    {
        Console.WriteLine("--> Inbound Post # Command Servicу");

        return Ok();
    }
}
