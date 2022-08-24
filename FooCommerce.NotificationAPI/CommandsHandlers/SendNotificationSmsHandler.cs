using FooCommerce.Application.Commands.Membership;
using FooCommerce.Application.Dtos.Notifications;
using FooCommerce.Application.Entities.Membership;
using FooCommerce.Application.Enums.Notifications;
using FooCommerce.NotificationAPI.Commands;

using MediatR;

using Microsoft.Extensions.Logging;

using IMediator = MediatR.IMediator;

namespace FooCommerce.NotificationAPI.CommandsHandlers;

public class SendNotificationSmsHandler : INotificationHandler<SendNotificationSms>
{
    private readonly ILogger<SendNotificationSmsHandler> _logger;
    private readonly IMediator _mediator;

    public SendNotificationSmsHandler(ILogger<SendNotificationSmsHandler> logger, IMediator mediator)
    {
        _logger = logger;
        _mediator = mediator;
    }

    public async Task Handle(SendNotificationSms notification, CancellationToken cancellationToken)
    {
        var renderedTemplate = notification.Options.Factory.CreateSmsModel(
            (NotificationTemplateSmsModel)notification.Options.Template,
            options =>
            {
                options.WebsiteUrl = notification.Options.WebsiteUrl;
            });
        var receiver = notification.Options.Options.Receiver.UserCommunications.Single(x => x.Type == notification.Options.Template.Communication);

        SendNotificationHandlerGuard.Check(renderedTemplate, notification.Options, _logger);

        var smsSent = true;
        if (smsSent)
        {
            var token = notification.Options.Options.Bag.OfType<AuthToken>().FirstOrDefault();
            if (token != null)
            {
                await _mediator.Publish(new UpdateAuthTokenState(token.Id, UserNotificationUpdateState.Sent), cancellationToken);
            }
        }
    }
}