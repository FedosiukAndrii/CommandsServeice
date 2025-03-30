using CommandsServeice.Enums;

namespace CommandsServeice.Attributes;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]

public class EventTypeAttribute(EventType eventName) : Attribute
{
    public EventType EventType { get; } = eventName;
}
