
using CommandsServeice.Interfaces;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace CommandsServeice.AsyncDataServices;

public class MessageBusSubscriber(IConfiguration configuration, IEventProcessor eventProcessor) : BackgroundService
{
    private const string TriggerExchange = "trigger";
    private IConnection _connection;
    private IChannel _channel;
    private string _queueName;

    protected async override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await ConfigureRabbitMQ();

        stoppingToken.ThrowIfCancellationRequested();

        var consumer = new AsyncEventingBasicConsumer(_channel);

        consumer.ReceivedAsync += async (ModuleHandle, ea) =>
        {
            Console.WriteLine("Event Recieved");

            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);

            try
            {
                await eventProcessor.ProcessEvent(message);
                await _channel.BasicAckAsync(ea.DeliveryTag, false);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"--> ❌ Error processing message: {ex.Message}");
                await _channel.BasicNackAsync(ea.DeliveryTag, false, requeue: true);
            }
        };

        await _channel.BasicConsumeAsync(_queueName, autoAck: false, consumer: consumer, cancellationToken: stoppingToken);
    }

    private async Task ConfigureRabbitMQ()
    {
        var factory = new ConnectionFactory()
        {
            HostName = configuration["RabbitMQHost"],
            Port = int.Parse(configuration["RabbitMQPort"])
        };

        _connection = await factory.CreateConnectionAsync();
        _channel = await _connection.CreateChannelAsync();

        await _channel.ExchangeDeclareAsync("trigger", ExchangeType.Fanout);

        _queueName = (await _channel.QueueDeclareAsync()).QueueName;

        await _channel.QueueBindAsync(_queueName, TriggerExchange, "");
        Console.WriteLine("--> Listening on the Message Bus...");

        _connection.ConnectionShutdownAsync += RabbitMQ_ConnectionShutdown;
    }

    private async Task RabbitMQ_ConnectionShutdown(object sender, ShutdownEventArgs e)
    {
        Console.WriteLine("--> RabbitMQ Connection Shutdown. Attempting to reconnect...");

        var maxRetries = 5;
        for (int attempt = 1; attempt <= maxRetries; attempt++)
        {
            try
            {
                var factory = new ConnectionFactory
                {
                    HostName = configuration["RabbitMQHost"],
                    Port = int.Parse(configuration["RabbitMQPort"])
                };

                var newConnection = await factory.CreateConnectionAsync();
                var newChannel = await newConnection.CreateChannelAsync();

                await newChannel.ExchangeDeclareAsync(
                    exchange: TriggerExchange,
                    type: ExchangeType.Fanout
                );

                _connection.ConnectionShutdownAsync -= RabbitMQ_ConnectionShutdown;
                await DisposeAsync();

                _connection = newConnection;
                _channel = newChannel;

                _connection.ConnectionShutdownAsync += RabbitMQ_ConnectionShutdown;

                Console.WriteLine("--> Reconnected to RabbitMQ successfully.");
                return;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"--> Reconnect attempt {attempt} failed: {ex.Message}");
                await Task.Delay(TimeSpan.FromSeconds(5));
            }
        }

        Console.WriteLine("--> Failed to reconnect to RabbitMQ after multiple attempts.");
    }

    public async ValueTask DisposeAsync()
    {
        Console.WriteLine("--> MessageBus Disposed");

        if (_channel is { IsOpen: true })
            await _channel.CloseAsync();

        if (_connection is { IsOpen: true })
            await _connection.CloseAsync();
    }
}