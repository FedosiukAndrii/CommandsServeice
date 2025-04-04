using CommandsServeice.AsyncDataServices;
using CommandsServeice.Data;
using CommandsServeice.Dtos.Events;
using CommandsServeice.EventHandlers;
using CommandsServeice.EventProcessing;
using CommandsServeice.Interfaces;
using CommandsServeice.SyncDataServices.Grpc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddDbContext<AppDbContext>(opt => opt.UseInMemoryDatabase("InMemory"));
builder.Services.AddScoped<ICommandsRepository, CommandRepository>();
builder.Services.AddScoped<IAsyncEventHandler<PlatformPublishedEvent>, PlatformPublishEventHandler>();
builder.Services.AddScoped<IPlatformDataClient, PlatformDataClient>();

builder.Services.AddSingleton<IEventProcessor, EventProcessor>();
builder.Services.AddSingleton<IEventDispatcher, EventDispatcher>();

builder.Services.AddHostedService<MessageBusSubscriber>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

Console.WriteLine($"--> CommandService started ");

await app.PrepPopulation();

app.Run();
