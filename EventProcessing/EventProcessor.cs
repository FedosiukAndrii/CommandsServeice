using CommandsServeice.Interfaces;
using Microsoft.AspNetCore.Components;
using System.Collections.Concurrent;
using System.Reflection;

namespace CommandsServeice.EventProcessing;

public class EventProcessor(IServiceScopeFactory scopeFactory, IEventParser eventParser) : IEventProcessor
{
    private static readonly ConcurrentDictionary<Type, MethodInfo> MethodCache = new();

    public async Task ProcessEvent(string message)
    {
        var @event = eventParser.Parse(message);

        Console.WriteLine($"--> {@event.Event} event detected");

        await using var scope = scopeFactory.CreateAsyncScope();

        var handlerType = typeof(IAsyncEventHandler<>).MakeGenericType(@event.GetType());
        var handler = scope.ServiceProvider.GetRequiredService(handlerType);

        var method = MethodCache.GetOrAdd(handlerType, t => t.GetMethod("HandleAsync") ?? throw new InvalidOperationException($"No HandleAsync method on {t.Name}"));

        if (method.Invoke(handler, [@event]) is Task task) 
            await task;
    }
}