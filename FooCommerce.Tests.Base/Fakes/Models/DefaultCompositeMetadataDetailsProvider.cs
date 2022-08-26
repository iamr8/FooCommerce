using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;

namespace FooCommerce.Tests.Fakes.Models;

/// <summary>
/// A default implementation of <see cref="ICompositeMetadataDetailsProvider"/>.
/// </summary>
internal class DefaultCompositeMetadataDetailsProvider : ICompositeMetadataDetailsProvider
{
    private readonly IEnumerable<IMetadataDetailsProvider> _providers;

    /// <summary>
    /// Creates a new <see cref="Microsoft.AspNetCore.Mvc.ModelBinding.Metadata.DefaultCompositeMetadataDetailsProvider"/>.
    /// </summary>
    /// <param name="providers">The set of <see cref="IMetadataDetailsProvider"/> instances.</param>
    public DefaultCompositeMetadataDetailsProvider(IEnumerable<IMetadataDetailsProvider> providers)
    {
        _providers = providers;
    }

    /// <inheritdoc />
    public void CreateBindingMetadata(BindingMetadataProviderContext context)
    {
        if (context == null)
        {
            throw new ArgumentNullException(nameof(context));
        }

        foreach (var provider in _providers.OfType<IBindingMetadataProvider>())
        {
            provider.CreateBindingMetadata(context);
        }
    }

    /// <inheritdoc />
    public void CreateDisplayMetadata(DisplayMetadataProviderContext context)
    {
        if (context == null)
        {
            throw new ArgumentNullException(nameof(context));
        }

        foreach (var provider in _providers.OfType<IDisplayMetadataProvider>())
        {
            provider.CreateDisplayMetadata(context);
        }
    }

    /// <inheritdoc />
    public void CreateValidationMetadata(ValidationMetadataProviderContext context)
    {
        if (context == null)
        {
            throw new ArgumentNullException(nameof(context));
        }

        foreach (var provider in _providers.OfType<IValidationMetadataProvider>())
        {
            provider.CreateValidationMetadata(context);
        }
    }
}