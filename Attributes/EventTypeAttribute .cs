namespace CommandsServeice.Attributes;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]

public class EventTypeAttribute(string eventName) : Attribute
{
    public string EventName { get; } = eventName;
}
