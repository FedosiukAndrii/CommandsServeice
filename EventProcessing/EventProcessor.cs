using CommandsServeice.Interfaces;

namespace CommandsServeice.EventProcessing;

public class EventProcessor(IServiceScopeFactory scopeFactory, IEventDispatcher dispatcher) : IEventProcessor
{
    public async Task ProcessEvent(string message)
    {
        var @event = EventParser.Parse(message);
        Console.WriteLine($"--> {@event.Event} event detected");

        await using var scope = scopeFactory.CreateAsyncScope();
        await dispatcher.DispatchAsync(@event, scope.ServiceProvider);
    }
}