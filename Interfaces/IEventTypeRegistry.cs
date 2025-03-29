
namespace CommandsServeice.Interfaces
{
    public interface IEventTypeRegistry
    {
        Type Resolve(string eventName);
    }
}