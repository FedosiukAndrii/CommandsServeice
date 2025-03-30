using CommandsServeice.Attributes;
using CommandsServeice.Enums;
using CommandsServeice.Interfaces;
using System.Reflection;

namespace CommandsServeice.EventProcessing;

public static class EventTypeRegistry 
{
    private static readonly Dictionary<EventType, Type> EventMap = Assembly.GetExecutingAssembly()
        .GetTypes()
        .Where(t => typeof(IEvent).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract)
        .Select(t => new
        {
            Type = t,
            Attribute = t.GetCustomAttribute<EventTypeAttribute>()
        })
        .Where(x => x.Attribute != null)
        .ToDictionary(x => x.Attribute!.EventType, x => x.Type);


    public static Type Resolve(EventType eventType) => EventMap.TryGetValue(eventType, out var type) ? type : null;
}