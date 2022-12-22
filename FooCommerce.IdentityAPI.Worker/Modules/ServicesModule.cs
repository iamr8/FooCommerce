using FooCommerce.Common.Configurations;
using FooCommerce.IdentityAPI.Worker.Services;
using FooCommerce.IdentityAPI.Worker.Services.Repositories;

namespace FooCommerce.IdentityAPI.Worker.Modules;

public class ServicesModule : Module
{
    public void Load(IServiceCollection services)
    {
        services.AddScoped<IUserManagerService, UserManager>();
        services.AddScoped<ICommunicationsManagerService, CommunicationsManager>();
        services.AddSingleton<ITokenStore, TokenStore>();
    }
}