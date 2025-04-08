namespace Ambev.DeveloperEvaluation.Domain.Events
{
    public interface IEventPublisher
    {
        Task PublishAsync<T>(T message, string queueName, CancellationToken cancellationToken = default);
    }
}
