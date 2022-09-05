using FooCommerce.Caching;
using FooCommerce.IdentityAPI.Enums;
using FooCommerce.IdentityAPI.Worker.Services;
using FooCommerce.IdentityAPI.Worker.Tests.Setup;

using Microsoft.Extensions.DependencyInjection;

using Xunit.Abstractions;

namespace FooCommerce.IdentityAPI.Worker.Tests;

public class UserServiceTests
{
    public ITestOutputHelper TestConsole { get; }

    public UserServiceTests(ITestOutputHelper outputHelper)
    {
        TestConsole = outputHelper;
    }

    [Fact]
    public async Task Should_Return_Roles_Cached()
    {
        // Arrange
        await using var fixture = new Fixture(TestConsole);
        await fixture.InitializeAsync();
        var userService = fixture.ServiceProvider.GetService<IUserService>();
        var easyCaching = fixture.ServiceProvider.GetService<ICacheProvider>();
        await easyCaching.FlushAsync();

        // Act
        var roles = await userService.GetRolesAsync();

        // Assert
        Assert.NotNull(roles);
        Assert.NotEmpty(roles);
        Assert.Equal(2, roles.Count());
        Assert.Contains(roles, l => l.Type == RoleType.Admin);
        Assert.Contains(roles, l => l.Type == RoleType.NormalUser);
    }

    //[Fact]
    //public async Task Should_Return_True_SignUp()
    //{
    //    // Arrange
    //    var input = new SignUpRequest
    //    {
    //        Country = 0,
    //        Email = "arash.shabbeh@gmail.com",
    //        FirstName = "Arash",
    //        LastName = "Shabbeh",
    //        Password = "12345678"
    //    };

    //    // Act
    //    var response = await UserService.PrepareSignUpAsync(input);

    //    // Assert
    //    Assert.Equal(JobStatus.Success, response.Status);
    //    Assert.Null(response.Errors);
    //}

    //[Fact]
    //public async Task Should_Return_True_SignIn()
    //{
    //    // Arrange
    //    var signUpInput = new SignUpRequest
    //    {
    //        Country = 0,
    //        Email = "arash.shabbeh@gmail.com",
    //        FirstName = "Arash",
    //        LastName = "Shabbeh",
    //        Password = "12345678"
    //    };
    //    var signInInput = new SignInRequest
    //    {
    //        Remember = true,
    //        Username = "arash.shabbeh@gmail.com",
    //        Password = "12345678"
    //    };

    //    // Act
    //    await UserService.PrepareSignUpAsync(signUpInput);
    //    var response = await UserService.PrepareSignInAsync(signInInput);

    //    // Assert
    //    Assert.Equal(JobStatus.Success, response.Status);
    //    Assert.Null(response.Errors);
    //}
}