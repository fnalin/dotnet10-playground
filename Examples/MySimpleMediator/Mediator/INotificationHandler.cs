namespace MySimpleMediator.Mediator;

public interface INotificationHandler<TNotification>
    where TNotification : INotification
{
    Task Handle(TNotification notification, CancellationToken ct);
}
