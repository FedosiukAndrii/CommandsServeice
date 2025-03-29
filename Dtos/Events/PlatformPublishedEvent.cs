using CommandsServeice.Attributes;
using CommandsServeice.Enums;
using CommandsServeice.Interfaces;

namespace CommandsServeice.Dtos.Events;

[EventType("Platform_Published")]
public record PlatformPublishedEvent(string Event, int PlatformId, string Name, int Id): IEvent
{
    public EventType Type => EventType.PlatformPublished;
}