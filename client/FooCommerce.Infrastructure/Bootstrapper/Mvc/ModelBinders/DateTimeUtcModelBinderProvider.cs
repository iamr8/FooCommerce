using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace FooCommerce.Infrastructure.Bootstrapper.Mvc.ModelBinders;

public class DateTimeUtcModelBinderProvider : IModelBinderProvider
{
    public IModelBinder GetBinder(ModelBinderProviderContext context)
    {
        return context.Metadata.ModelType == typeof(DateTime) || context.Metadata.ModelType == typeof(DateTime?)
            ? new DateTimeUtcModelBinder()
            : null;
    }
}