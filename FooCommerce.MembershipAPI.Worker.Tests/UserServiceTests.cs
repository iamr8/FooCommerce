using Autofac;

using EasyCaching.Core;

using FooCommerce.Domain.DbProvider;
using FooCommerce.MembershipAPI.Enums;
using FooCommerce.MembershipAPI.Worker.DbProvider;
using FooCommerce.MembershipAPI.Worker.Services;
using FooCommerce.MembershipAPI.Worker.Services.Repositories;
using FooCommerce.Tests;
using FooCommerce.Tests.Extensions;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using Xunit.Abstractions;

namespace FooCommerce.MembershipAPI.Worker.Tests;

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
        var userServiceLogger = Scope.Resolve<ILogger<IUserService>>();
        var dbConnectionFactory = Scope.Resolve<IDbConnectionFactory>();
        var dbContextFactory = Scope.Resolve<IDbContextFactory<AppDbContext>>();
        easyCaching.Flush();
        UserService = new UserService(dbConnectionFactory, dbContextFactory, easyCaching, userServiceLogger);
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