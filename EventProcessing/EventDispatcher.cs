using CommandsServeice.Interfaces;
using System.Reflection;

namespace CommandsServeice.EventProcessing;

public class EventDispatcher : IEventDispatcher
{
    private readonly Dictionary<Type, Func<IEvent, IServiceProvider, Task>> Handlers = EventHandlerRegistry.BuildHandlerDispatchers(Assembly.GetExecutingAssembly());

    public async Task DispatchAsync(IEvent @event, IServiceProvider provider)
    {
        var eventType = @event.GetType();

        if (!Handlers.TryGetValue(eventType, out var handler))
            Console.WriteLine($"⚠️ No handler found for event type: {eventType.Name}");
        else
            await handler(@event, provider);
    }
}
