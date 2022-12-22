using FooCommerce.Domain.Enums;
using FooCommerce.Services.NotificationAPI.Contracts;
using FooCommerce.Services.NotificationAPI.Interfaces;

using MassTransit;

using MimeKit;

namespace FooCommerce.Services.NotificationAPI.Consumers;

public class EnqueueEmailConsumer :
    IConsumer<EnqueueEmail>,
    IConsumer<Fault<EnqueueEmail>>
{
    private readonly IWebHostEnvironment _environment;

    private readonly IEmailClient _emailClient;
    private readonly ILogger<EnqueueEmailConsumer> _logger;

    public EnqueueEmailConsumer(IWebHostEnvironment environment,
        ILogger<EnqueueEmailConsumer> logger, IEmailClient emailClient)
    {
        _logger = logger;
        _emailClient = emailClient;
        _environment = environment;
    }

    public async Task Consume(ConsumeContext<EnqueueEmail> context)
    {
        var mime = new MimeMessage
        {
            Subject = context.Message.Subject,
            Sender = new MailboxAddress(_emailClient.SenderAlias, _emailClient.SenderAddress),
            Importance = context.Message.IsImportant ? MessageImportance.High : MessageImportance.Normal,
            Body = new BodyBuilder { HtmlBody = context.Message.Body }.ToMessageBody(),
        };

        mime.From.Add(mime.Sender);
        mime.To.Add(new MailboxAddress(context.Message.ReceiverName, context.Message.ReceiverAddress));

        var emailSent = true;
        if (!_environment.IsDevelopment())
        {
            await _emailClient.ConnectAsync(context.CancellationToken);
            try
            {
                emailSent = await _emailClient.SendAsync(mime, context.CancellationToken);
            }
            catch (Exception e)
            {
                emailSent = false;
                _logger.LogError("Unable to send {0} to {1}\n{2}", CommType.Email, context.Message.ReceiverName, e.Message);
            }

            await _emailClient.DisposeAsync();
        }
    }

    public Task Consume(ConsumeContext<Fault<EnqueueEmail>> context)
    {
        for (int i = 0; i < context.Message.Exceptions.Length; i++)
        {
            _logger.LogError(context.Message.Exceptions[i].Message);
        }

        return Task.CompletedTask;
    }
}