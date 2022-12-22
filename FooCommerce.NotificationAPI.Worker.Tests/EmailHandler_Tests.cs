using System.Globalization;
using System.Net;

using FooCommerce.Common.Helpers;
using FooCommerce.Domain.ContextRequest;
using FooCommerce.Localization.Models;
using FooCommerce.Services.NotificationAPI.Contracts;
using FooCommerce.Services.NotificationAPI.Dtos;
using FooCommerce.Services.NotificationAPI.Enums;
using FooCommerce.Services.NotificationAPI.Handlers;
using FooCommerce.Services.NotificationAPI.Tests.Setup;

using Xunit.Abstractions;

namespace FooCommerce.Services.NotificationAPI.Tests;

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
                IPAddress = IPAddress.Any,
                Browser = new ContextRequestBrowser("Chrome", "105"),
                Device = new ContextRequestDevice("Desktop"),
                Platform = new ContextRequestPlatform("Windows", "11 22H2"),
                TimezoneId = "Europe/Istanbul",
                UserAgent = "Chrome",
                Engine = new ContextRequestEngine("Chromium"),
                Country = RegionInfo.CurrentRegion,
                Culture = CultureInfo.CurrentCulture
            });

        await Task.Delay(500);

        Assert.True(await this.Harness.Published.Any<EnqueueEmail>());

        await Task.Delay(500);

        Assert.True(await this.Harness.Consumed.Any<EnqueueEmail>());
    }
}