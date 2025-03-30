using CommandsServeice.Attributes;
using CommandsServeice.Enums;
using CommandsServeice.Interfaces;

namespace CommandsServeice.Dtos.Events;

[EventType(EventType.Unknown)]
public record UnknownEvent(string Event): IEvent
{
    public EventType Type => EventType.Unknown;
}