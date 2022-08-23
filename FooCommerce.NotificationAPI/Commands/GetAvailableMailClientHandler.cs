using FooCommerce.Application.Commands.Notifications;
using FooCommerce.Application.Interfaces.Notifications;
using FooCommerce.NotificationAPI.Models;

using MediatR;

namespace FooCommerce.NotificationAPI.Commands;

public class GetAvailableMailClientHandler : IRequestHandler<GetAvailableMailClient, IEmailClient>
{
    public Task<IEmailClient> Handle(GetAvailableMailClient request, CancellationToken cancellationToken)
    {
        // try to find available mail box according to service bus
        //var client = await EmailClient.GetInstanceAsync(cancellationToken: cancellationToken);
        //return client;
        return Task.FromResult((IEmailClient)EmailClient.Empty);
    }
}