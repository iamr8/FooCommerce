using FooCommerce.Common.Configurations;

using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.DataProtection.AuthenticatedEncryption;
using Microsoft.AspNetCore.DataProtection.AuthenticatedEncryption.ConfigurationModel;
using Microsoft.Extensions.DependencyInjection;

namespace FooCommerce.Infrastructure.Modules;

public class ProtectionModule : Module
{
    public void Load(IServiceCollection services)
    {
        services.AddDataProtection()
            .SetApplicationName("FooCommerceProtection")
            .PersistKeysToFileSystem(Directory.CreateDirectory(@"Keys"))
            .UseCryptographicAlgorithms(new AuthenticatedEncryptorConfiguration
            {
                EncryptionAlgorithm = EncryptionAlgorithm.AES_256_CBC,
                ValidationAlgorithm = ValidationAlgorithm.HMACSHA256
            });
    }
}