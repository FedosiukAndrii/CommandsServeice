using CommandsServeice.Attributes;
using CommandsServeice.Interfaces;
using System.Reflection;

namespace CommandsServeice.EventProcessing;

public class EventTypeRegistry : IEventTypeRegistry
{
    private readonly Dictionary<string, Type> _eventMap;

    public EventTypeRegistry()
    {
        _eventMap = Assembly.GetExecutingAssembly()
            .GetTypes()
            .Where(t => typeof(IEvent).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract)
            .Select(t => new
            {
                Type = t,
                Attribute = t.GetCustomAttribute<EventTypeAttribute>()
            })
            .Where(x => x.Attribute != null)
            .ToDictionary(x => x.Attribute!.EventName, x => x.Type);
    }

    public Type Resolve(string eventName) => _eventMap.TryGetValue(eventName, out var type) ? type : null;
}