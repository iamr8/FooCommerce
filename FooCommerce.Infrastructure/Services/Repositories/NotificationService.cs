using MassTransit;

using Microsoft.AspNetCore.Http;

namespace FooCommerce.Infrastructure.Services.Repositories;

public class NotificationService : INotificationService
{
    private readonly IBus _bus;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public NotificationService(IHttpContextAccessor httpContextAccessor, IBus bus)
    {
        _httpContextAccessor = httpContextAccessor;
        _bus = bus;
    }

    //public async Task EnqueueAsync(NotificationAction action, NotificationReceiverStrategy strategy, Guid id, CancellationToken cancellationToken = default)
    //{
    //    IEnumerable<UserCommunicationModel> communicationModels;
    //    switch (strategy)
    //    {
    //        case NotificationReceiverStrategy.ByUserCommunicationId:
    //            communicationModels = await _communicationService.GetModelsByUserIdAsync(id, cancellationToken);
    //            break;

    //        case NotificationReceiverStrategy.ByUserId:
    //            communicationModels = await _communicationService.GetModelsByUserIdAsync(id, cancellationToken);
    //            break;

    //        default:
    //            throw new ArgumentOutOfRangeException(nameof(strategy), strategy, null);
    //    }
    //    var communicationModel = await _communicationService.GetModelByIdAsync(userCommunicationId, cancellationToken);
    //    await _notificationService.EnqueueAsync(new NotificationOptions
    //    {
    //        Action = action,
    //        // Receiver = new NotificationReceiverProvider(NotificationReceiverStrategy.ByUserCommunicationId, result.CommunicationId!.Value),
    //        Receiver = new NotificationReceiverProvider
    //        {
    //            UserId = communicationModel.
    //        },
    //        Content = Enumerable.Range(0, 1).Select(_ => new NotificationFormatter("authToken", result.Token)),
    //        RequestInfo = (ContextRequestInfo)_httpContextAccessor.HttpContext.GetRequestInfo()
    //    }, cancellationToken);
    //}
}