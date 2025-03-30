using CommandsServeice.Dtos.Events;
using CommandsServeice.Enums;
using CommandsServeice.Interfaces;
using Newtonsoft.Json.Linq;

namespace CommandsServeice.EventProcessing;

public static class EventParser
{
    public static IEvent Parse(string json)
    {
        var jObject = JObject.Parse(json);
        var eventName = jObject["Event"]?.ToString();

        if (!Enum.TryParse<EventType>(eventName, ignoreCase: true, out var eventType))
            return new UnknownEvent(eventName ?? "null");

        var type = EventTypeRegistry.Resolve(eventType);

        if (type is null)
            return new UnknownEvent(eventName ?? "null");

        return (IEvent)jObject.ToObject(type)!;
    }
}
