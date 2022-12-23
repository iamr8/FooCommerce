using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace FooCommerce.Infrastructure.Mvc.ModelBinders;

public class StringModelBinderProvider : IModelBinderProvider
{
    public IModelBinder GetBinder(ModelBinderProviderContext context)
    {
        return context.Metadata.ModelType == typeof(string)
            ? new StringModelBinder()
            : null;
    }
}