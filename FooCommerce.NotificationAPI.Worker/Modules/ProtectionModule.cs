using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.DataProtection.AuthenticatedEncryption;
using Microsoft.AspNetCore.DataProtection.AuthenticatedEncryption.ConfigurationModel;

namespace FooCommerce.NotificationAPI.Worker.Modules;

public class ProtectionModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        var services = new ServiceCollection();
        services.AddDataProtection()
            .SetApplicationName("FooCommerceProtection_Worker_NotificationAPI")
            .PersistKeysToFileSystem(Directory.CreateDirectory(@"Keys"))
            .UseCryptographicAlgorithms(new AuthenticatedEncryptorConfiguration
            {
                EncryptionAlgorithm = EncryptionAlgorithm.AES_256_CBC,
                ValidationAlgorithm = ValidationAlgorithm.HMACSHA256
            });

        builder.Populate(services);
    }
}