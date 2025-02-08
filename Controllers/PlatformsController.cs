using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CommandsServeice.Controllers;

[ApiController, Route("api/commands/[controller]")]
public class PlatformsController : ControllerBase
{
    [HttpPost]
    public IActionResult TestInboundConnection()
    {
        Console.WriteLine("--> Inbound Post # Command Servicу");

        return Ok();
    }
}
