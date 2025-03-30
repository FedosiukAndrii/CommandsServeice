using CommandsServeice.Interfaces;
using System.Reflection;

namespace CommandsServeice.EventProcessing;

public static class EventHandlerRegistry
{
    public static Dictionary<Type, Func<IEvent, IServiceProvider, Task>> BuildHandlerDispatchers(Assembly assembly)
    {
        var handlerInterfaceType = typeof(IAsyncEventHandler<>);

        var dispatchers = new Dictionary<Type, Func<IEvent, IServiceProvider, Task>>();

        var handlerTypes = assembly
            .GetTypes()
            .Where(t => !t.IsAbstract && !t.IsInterface)
            .SelectMany(t => t.GetInterfaces()
                .Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == handlerInterfaceType)
                .Select(i => new
                {
                    EventType = i.GetGenericArguments()[0]
                }))
            .ToList();

        foreach (var entry in handlerTypes)
        {
            var eventType = entry.EventType;

            var method = typeof(EventHandlerRegistry)
                .GetMethod(nameof(DispatchTyped), BindingFlags.NonPublic | BindingFlags.Static)
                .MakeGenericMethod(eventType);

            var func = (Func<IEvent, IServiceProvider, Task>)Delegate.CreateDelegate(
                typeof(Func<IEvent, IServiceProvider, Task>), method);

            dispatchers[eventType] = func;
        }

        return dispatchers;
    }

    private static async Task DispatchTyped<TEvent>(IEvent @event, IServiceProvider provider) where TEvent : IEvent
    {
        var handler = provider.GetRequiredService<IAsyncEventHandler<TEvent>>();
        await handler.HandleAsync((TEvent)@event);
    }
}
