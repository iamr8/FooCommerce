using Autofac;
using Autofac.Extensions.DependencyInjection;

using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.DataProtection.AuthenticatedEncryption;
using Microsoft.AspNetCore.DataProtection.AuthenticatedEncryption.ConfigurationModel;
using Microsoft.Extensions.DependencyInjection;

namespace FooCommerce.Infrastructure.Bootstrapper.Modules;

public class ProtectionModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        var services = new ServiceCollection();
        services.AddDataProtection()
            .SetApplicationName("FooCommerceProtection")
            .PersistKeysToFileSystem(Directory.CreateDirectory(@"Keys"))
            .UseCryptographicAlgorithms(new AuthenticatedEncryptorConfiguration
            {
                EncryptionAlgorithm = EncryptionAlgorithm.AES_256_CBC,
                ValidationAlgorithm = ValidationAlgorithm.HMACSHA256
            });

        builder.Populate(services);
    }
}