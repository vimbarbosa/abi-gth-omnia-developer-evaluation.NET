using System.Text;
using Ambev.DeveloperEvaluation.Domain.Events;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RabbitMQ.Client;

/// <summary>
/// Publishes messages to a RabbitMQ queue using the configuration provided.
/// </summary>
public class RabbitMqEventPublisher : IEventPublisher
{
    private readonly ILogger<RabbitMqEventPublisher> _logger;
    private readonly ConnectionFactory? _factory;
    private readonly bool _isConfigured;

    /// <summary>
    /// Initializes a new instance of the <see cref="RabbitMqEventPublisher"/> class.
    /// </summary>
    /// <param name="configuration">Application configuration for RabbitMQ settings.</param>
    /// <param name="logger">Logger instance.</param>
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

    /// <summary>
    /// Publishes a message to a specific RabbitMQ queue.
    /// </summary>
    /// <typeparam name="T">The type of the message payload.</typeparam>
    /// <param name="message">The message to be sent.</param>
    /// <param name="queueName">The queue to which the message will be published.</param>
    /// <param name="cancellationToken">Cancellation token (unused in this implementation).</param>
    /// <returns>A task representing the asynchronous operation.</returns>
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

            var settings = new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };
            var json = JsonConvert.SerializeObject(message, settings);
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

    /// <summary>
    /// Declares a durable RabbitMQ queue with the specified name.
    /// </summary>
    /// <param name="channel">RabbitMQ channel.</param>
    /// <param name="queueName">Name of the queue to declare.</param>
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

    /// <summary>
    /// Logs a warning for missing or invalid configuration settings.
    /// </summary>
    /// <param name="message">The warning message.</param>
    /// <param name="host">Optional RabbitMQ host value.</param>
    /// <param name="port">Optional RabbitMQ port value.</param>
    /// <param name="user">Optional RabbitMQ user value.</param>
    private void LogConfigWarning(string message, string? host = null, string? port = null, string? user = null)
    {
        _logger.LogWarning("{Message} Host: {Host}, Port: {Port}, User: {User}", message, host ?? "-", port ?? "-", user ?? "-");
    }
}
