namespace CommandsServeice.Interfaces;

public interface IAsyncEventHandler<in T> where T : IEvent
{
    Task HandleAsync(T @event);
}

