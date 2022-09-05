using Microsoft.Extensions.DependencyInjection;

namespace FooCommerce.Common.Configurations;

public interface Module
{
    void Load(IServiceCollection services);
}