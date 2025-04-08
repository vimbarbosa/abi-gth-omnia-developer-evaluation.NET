namespace Ambev.DeveloperEvaluation.Unit.Domain.Events
{
    public interface IEventPublisher
    {
        Task PublishAsync<T>(T message, string queueName, CancellationToken cancellationToken = default);
    }
}
