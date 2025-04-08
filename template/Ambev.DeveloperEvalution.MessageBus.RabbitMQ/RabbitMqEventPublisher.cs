using System.Text;
using Ambev.DeveloperEvaluation.Domain.Events;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RabbitMQ.Client;

public class RabbitMqEventPublisher : IEventPublisher
{
    private readonly ILogger<RabbitMqEventPublisher> _logger;
    private readonly ConnectionFactory? _factory;
    private readonly bool _isConfigured;

    public RabbitMqEventPublisher(IConfiguration configuration, ILogger<RabbitMqEventPublisher> logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));

        var config = configuration.GetSection("RabbitMQ");

        if (!config.Exists())
        {
            LogConfigWarning("RabbitMQ section is missing.");
            return;
        }

        var host = config["HostName"];
        var port = config["Port"];
        var user = config["UserName"];
        var pass = config["Password"];

        if (string.IsNullOrWhiteSpace(host) ||
            string.IsNullOrWhiteSpace(port) ||
            string.IsNullOrWhiteSpace(user) ||
            string.IsNullOrWhiteSpace(pass))
        {
            LogConfigWarning("One or more RabbitMQ settings are missing or invalid.", host, port, user);
            return;
        }

        _factory = new ConnectionFactory
        {
            HostName = host,
            Port = int.Parse(port),
            UserName = user,
            Password = pass
        };

        _isConfigured = true;
    }

    public Task PublishAsync<T>(T message, string queueName, CancellationToken cancellationToken = default)
    {
        if (!_isConfigured)
        {
            _logger.LogWarning("RabbitMQ publisher is not configured.");
            return Task.CompletedTask;
        }

        if (string.IsNullOrWhiteSpace(queueName))
        {
            _logger.LogWarning("Queue name is null or empty.");
            return Task.CompletedTask;
        }

        if (message == null)
        {
            _logger.LogWarning("Message is null. Skipping publish to queue '{QueueName}'.", queueName);
            return Task.CompletedTask;
        }

        try
        {
            using var connection = _factory!.CreateConnection();
            using var channel = connection.CreateModel();

            DeclareQueue(channel, queueName);

            var json = JsonConvert.SerializeObject(message);
            var body = Encoding.UTF8.GetBytes(json);

            channel.BasicPublish(
                exchange: "",
                routingKey: queueName,
                basicProperties: null,
                body: body
            );

            _logger.LogInformation("Message published to RabbitMQ queue '{QueueName}': {Payload}", queueName, json);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while publishing message to RabbitMQ.");
        }

        return Task.CompletedTask;
    }

    private void DeclareQueue(IModel channel, string queueName)
    {
        channel.QueueDeclare(
            queue: queueName,
            durable: true,
            exclusive: false,
            autoDelete: false,
            arguments: null
        );
    }

    private void LogConfigWarning(string message, string? host = null, string? port = null, string? user = null)
    {
        _logger.LogWarning("{Message} Host: {Host}, Port: {Port}, User: {User}", message, host ?? "-", port ?? "-", user ?? "-");
    }
}