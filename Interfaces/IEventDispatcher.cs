namespace CommandsServeice.Interfaces;

public interface IEventDispatcher
{
    Task DispatchAsync(IEvent @event, IServiceProvider provider);
}