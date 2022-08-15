using Autofac;

using FooCommerce.Application.DbProvider;
using FooCommerce.Application.Enums.Membership;
using FooCommerce.Application.Models.Membership;
using FooCommerce.Application.Services.Listings;
using FooCommerce.Application.Services.Membership;
using FooCommerce.Domain.Enums;
using FooCommerce.Infrastructure.Locations;
using FooCommerce.Infrastructure.Membership;
using FooCommerce.Infrastructure.Protection.Options;

using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using Moq;

using Xunit.Abstractions;

namespace FooCommerce.Infrastructure.Tests.MembershipTests;

public class UserServiceTests : IClassFixture<Fixture>, ITest
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

        var memoryCache = Scope.Resolve<IMemoryCache>();
        var logger = Scope.Resolve<ILogger<IUserService>>();
        var hashingOptions = Scope.Resolve<IOptions<HashingOptions>>();
        var dbConnectionFactory = Scope.Resolve<IDbConnectionFactory>();
        var locationService = new LocationService(dbConnectionFactory, memoryCache, Mock.Of<ILogger<ILocationService>>());
        UserService = new UserService(locationService, dbConnectionFactory, memoryCache, logger);
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
        Assert.Contains(roles, l => l.Type == RoleTypes.Admin);
        Assert.Contains(roles, l => l.Type == RoleTypes.NormalUser);
    }

    [Fact]
    public async Task Should_Return_Role()
    {
        // Arrange
        const RoleTypes role = RoleTypes.NormalUser;

        // Act
        var roleModel = await UserService.GetRoleAsync(role);

        // Assert
        Assert.NotNull(roleModel);
        Assert.NotEqual(Guid.Empty, roleModel.Id);
        Assert.Equal(RoleTypes.NormalUser, roleModel.Type);
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
}