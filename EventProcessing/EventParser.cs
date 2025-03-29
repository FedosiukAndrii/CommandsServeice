using CommandsServeice.Dtos.Events;
using CommandsServeice.Interfaces;
using Newtonsoft.Json.Linq;

namespace CommandsServeice.EventProcessing;

public class EventParser(IEventTypeRegistry registry) : IEventParser
{
    public IEvent Parse(string json)
    {
        var jObject = JObject.Parse(json);
        var eventName = jObject["Event"]?.ToString();

        var type = registry.Resolve(eventName ?? "");

        if (type is null)
            return new UnknownEvent(eventName ?? "null");

        return (IEvent)jObject.ToObject(type)!;
    }
}
