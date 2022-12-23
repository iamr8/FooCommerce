using System.Globalization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace FooCommerce.Infrastructure.Mvc.ModelBinding.CustomProviders;

public class CustomNameFormValueProviderFactory : IValueProviderFactory
{
    public async Task CreateValueProviderAsync(ValueProviderFactoryContext context)
    {
        if (context == null)
        {
            throw new ArgumentNullException(nameof(context));
        }

        var request = context.ActionContext.HttpContext.Request;
        if (request.HasFormContentType)
        {
            // Allocating a Task only when the body is form data.
            IFormCollection form;

            try
            {
                form = await request.ReadFormAsync();
            }
            catch (InvalidDataException ex)
            {
                // ReadFormAsync can throw InvalidDataException if the form content is malformed.
                // Wrap it in a ValueProviderException that the CompositeValueProvider special cases.
                throw new ValueProviderException(ex.Message, ex);
            }
            catch (IOException ex)
            {
                // ReadFormAsync can throw IOException if the client disconnects.
                // Wrap it in a ValueProviderException that the CompositeValueProvider special cases.
                throw new ValueProviderException(ex.Message, ex);
            }

            var valueProvider = new CustomNameFormValueProvider(
                BindingSource.Form,
                form,
                CultureInfo.CurrentCulture);

            context.ValueProviders.Add(valueProvider);
        }
    }
}