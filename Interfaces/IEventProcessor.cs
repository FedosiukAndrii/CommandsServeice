namespace CommandsServeice.Interfaces;

public interface IEventProcessor
{
    Task ProcessEvent(string message);
}
