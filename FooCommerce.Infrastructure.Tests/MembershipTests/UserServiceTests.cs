using Autofac;

using EasyCaching.Core;

using FooCommerce.Application.DbProvider;
using FooCommerce.Application.Listings.Services;
using FooCommerce.Application.Membership.Enums;
using FooCommerce.Application.Membership.Models;
using FooCommerce.Application.Membership.Services;
using FooCommerce.Domain.Enums;
using FooCommerce.Infrastructure.Locations;
using FooCommerce.Infrastructure.Membership;
using FooCommerce.Infrastructure.Tests.Setups;
using FooCommerce.Tests.Base;

using Microsoft.Extensions.Logging;

using Xunit.Abstractions;

namespace FooCommerce.Infrastructure.Tests.MembershipTests;

public class UserServiceTests : IClassFixture<Fixture>, ITestScope<Fixture>
{
    public Fixture Fixture { get; }
    public ITestOutputHelper TestConsole { get; }
    public ILifetimeScope Scope { get; }
    private readonly UserService UserService;

    public UserServiceTests(Fixture fixture, ITestOutputHelper outputHelper)
    {
        Fixture = fixture;
        TestConsole = outputHelper;
        Scope = fixture.ConfigureLogging(outputHelper);

        var easyCaching = Scope.Resolve<IEasyCachingProvider>();
        var locationServiceLogger = Scope.Resolve<ILogger<ILocationService>>();
        var userServiceLogger = Scope.Resolve<ILogger<IUserService>>();
        var dbConnectionFactory = Scope.Resolve<IDbConnectionFactory>();
        easyCaching.Flush();
        var locationService = new LocationService(dbConnectionFactory, easyCaching, locationServiceLogger);
        UserService = new UserService(locationService, dbConnectionFactory, easyCaching, userServiceLogger);
    }

    [Fact]
    public async Task Should_Return_Roles_Cached()
    {
        // Act
        var roles = await UserService.GetRolesAsync();

        // Assert
        Assert.NotNull(roles);
        Assert.NotEmpty(roles);
        Assert.Equal(2, roles.Count());
        Assert.Contains(roles, l => l.Type == RoleType.Admin);
        Assert.Contains(roles, l => l.Type == RoleType.NormalUser);
    }

    [Fact]
    public async Task Should_Return_Role()
    {
        // Arrange
        const RoleType role = RoleType.NormalUser;

        // Act
        var roleModel = await UserService.GetRoleAsync(role);

        // Assert
        Assert.NotNull(roleModel);
        Assert.NotEqual(Guid.Empty, roleModel.Id);
        Assert.Equal(RoleType.NormalUser, roleModel.Type);
    }

    [Fact]
    public async Task Should_Return_True_SignUp()
    {
        // Arrange
        var input = new SignUpRequest
        {
            Country = 0,
            Email = "arash.shabbeh@gmail.com",
            FirstName = "Arash",
            LastName = "Shabbeh",
            Password = "12345678"
        };

        // Act
        var response = await UserService.SignUpAsync(input);

        // Assert
        Assert.Equal(JobStatus.Success, response.Status);
        Assert.Null(response.Errors);
    }

    [Fact]
    public async Task Should_Return_True_SignIn()
    {
        // Arrange
        var signUpInput = new SignUpRequest
        {
            Country = 0,
            Email = "arash.shabbeh@gmail.com",
            FirstName = "Arash",
            LastName = "Shabbeh",
            Password = "12345678"
        };
        var signInInput = new SignInRequest
        {
            Remember = true,
            Username = "arash.shabbeh@gmail.com",
            Password = "12345678"
        };

        // Act
        await UserService.SignUpAsync(signUpInput);
        var response = await UserService.SignInAsync(signInInput);

        // Assert
        Assert.Equal(JobStatus.Success, response.Status);
        Assert.Null(response.Errors);
    }
}