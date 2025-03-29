using AutoMapper;
using CommandsServeice.Data;
using CommandsServeice.Dtos.Events;
using CommandsServeice.Interfaces;
using CommandsServeice.Models;

namespace CommandsServeice.EventHandlers;

public class PlatformPublishEventHandler(ICommandsRepository repository, IMapper mapper) : IAsyncEventHandler<PlatformPublishedEvent>
{
    public async Task HandleAsync(PlatformPublishedEvent @event)
    {
		try
		{
            var platform = mapper.Map<Platform>(@event);

            if (!await repository.ExternalPlatformExists(platform.ExternalId))
                await repository.CreatePlatform(platform);
            else
                Console.WriteLine("--> Platform already exists");
        }
		catch (Exception ex)
		{
            Console.WriteLine($"--> Could not add platform to DB {ex.Message}");
		}
    }
}
