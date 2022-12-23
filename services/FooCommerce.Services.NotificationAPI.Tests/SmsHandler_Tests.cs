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

public class SmsHandler_Tests : FixturePerTest
{
    private readonly IHandler _handler;

    public SmsHandler_Tests(FixturePerTestClass fixture, ITestOutputHelper outputHelper) : base(fixture, outputHelper)
    {
        _handler = ServiceProvider.GetService<IHandler>(typeof(SmsHandler));
    }

    [Fact]
    public async Task Should_Send_Sms()
    {
        // Assert
        var template = new SmsTemplateModel
        {
            Text = new LocalizerValueCollection(new Dictionary<CultureInfo, string>
            {
                {CultureInfo.CurrentCulture, "Welcome to FooCommerce."}
            }),
        };

        // Act
        await _handler.EnqueueAsync(
            template,
            NotificationPurpose.Verification_Request_Email,
            "Arash",
            "+905317251106",
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

        Assert.True(await this.Harness.Published.Any<EnqueueSms>());

        await Task.Delay(500);

        Assert.True(await this.Harness.Consumed.Any<EnqueueSms>());
    }
}