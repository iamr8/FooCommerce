using System.Globalization;
using System.Net;
using FooCommerce.Common.Helpers;
using FooCommerce.Domain.ContextRequest;
using FooCommerce.Localization.Models;
using FooCommerce.NotificationService.Contracts;
using FooCommerce.NotificationService.Dtos;
using FooCommerce.NotificationService.Enums;
using FooCommerce.NotificationService.Handlers;
using FooCommerce.NotificationService.Tests.Setup;
using Xunit.Abstractions;

namespace FooCommerce.NotificationService.Tests;

public class EmailHandler_Tests : FixturePerTest
{
    private readonly IHandler _handler;

    public EmailHandler_Tests(FixturePerTestClass fixture, ITestOutputHelper outputHelper) : base(fixture, outputHelper)
    {
        _handler = ServiceProvider.GetService<IHandler>(typeof(EmailHandler));
    }

    [Fact]
    public async Task Should_Send_Email()
    {
        // Assert
        var template = new EmailTemplateModel
        {
            Html = new LocalizerValueCollection(new Dictionary<CultureInfo, string>
            {
                {CultureInfo.CurrentCulture, "<p>Hi</p>"}
            }),
            ShowRequestData = true,
        };

        // Act
        await _handler.EnqueueAsync(
            template,
            NotificationPurpose.Verification_Request_Email,
            "Arash",
            "arash.shabbeh@gmail.com",
            new Dictionary<string, string>(),
            new Dictionary<string, string>(),
            "http://localhost:5000",
            new ContextRequestInfo
            {
                IPAddress = IPAddress.Parse("127.0.0.1"),
                Browser = new ContextRequestBrowser { Name = "Chrome", Version = "105" },
                Device = new ContextRequestDevice { Type = "Desktop" },
                Platform = new ContextRequestPlatform { Name = "Windows", Version = "11 22H2" },
                TimezoneId = "Europe/Istanbul",
                UserAgent = "Chrome v105 AppleWebKit",
                Engine = new ContextRequestEngine { Name = "Chromium" },
                Country = RegionInfo.CurrentRegion,
                Culture = CultureInfo.CurrentCulture
            });

        await Task.Delay(500);

        Assert.True(await this.Harness.Published.Any<EnqueueEmail>());

        await Task.Delay(500);

        Assert.True(await this.Harness.Consumed.Any<EnqueueEmail>());
    }
}