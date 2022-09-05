using FooCommerce.Common.Configurations;
using FooCommerce.IdentityAPI.Worker.Services;
using FooCommerce.IdentityAPI.Worker.Services.Repositories;

namespace FooCommerce.IdentityAPI.Worker.Modules;

public class ServicesModule : Module
{
    public void Load(IServiceCollection services)
    {
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IVerificationService, VerificationService>();
        services.AddScoped<ICommunicationService, CommunicationService>();
        services.AddScoped<ITokenService, TokenService>();
    }
}