//using System.Globalization;

//using FooCommerce.IdentityAPI.Contracts.UserContracts;
//using FooCommerce.Infrastructure.Services;
//using FooCommerce.Infrastructure.Tests.Setup;

//using Microsoft.Extensions.DependencyInjection;

//using Xunit.Abstractions;

//namespace FooCommerce.Infrastructure.Tests.Services;

//public class AccountServiceTests
//{
//    public ITestOutputHelper TestConsole { get; }

//    public AccountServiceTests(ITestOutputHelper outputHelper)
//    {
//        TestConsole = outputHelper;
//    }

//    [Fact]
//    public async Task Should_SignUpUser()
//    {
//        // Arrange
//        CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("en");
//        CultureInfo.CurrentUICulture = CultureInfo.GetCultureInfo("en");

//        await using var fixture = new Fixture(TestConsole);
//        await fixture.InitializeAsync();
//        var accountService = fixture.ServiceProvider.GetService<IMembershipService>();

//        var request = new SignUpRequest
//        {
//            Country = 0,
//            Email = "arash.shabbeh@gmail.com",
//            FirstName = "Arash",
//            LastName = "Shabbeh",
//            Password = "12345678"
//        };

//        // Action
//        //var tokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(5));
//        var status = await accountService.SignUpAsync(request);

//        // Assert
//        Assert.Equal(JobStatus.Success, status);
//    }
//}