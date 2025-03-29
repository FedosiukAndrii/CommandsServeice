using CommandsServeice.Enums;

namespace CommandsServeice.Interfaces;

public interface IEvent
{
    string Event { get; }
    EventType Type { get; }
}
