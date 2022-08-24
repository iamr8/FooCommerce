using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace FooCommerce.Infrastructure.Mvc.ModelBinders;

public class StringModelBinder : IModelBinder
{
    public Task BindModelAsync(ModelBindingContext bindingContext)
    {
        if (bindingContext == null)
            throw new ArgumentNullException(nameof(bindingContext));

        if (bindingContext.ModelType != typeof(string))
            return Task.FromResult(ModelBindingResult.Failed());

        var key = bindingContext.ModelName;
        if (string.IsNullOrEmpty(key))
            return Task.CompletedTask;

        var getValue = bindingContext.ValueProvider.GetValue(key);

        var value = getValue.FirstValue;
        if (string.IsNullOrEmpty(value))
            return Task.CompletedTask;

        value = value.Trim();
        bindingContext.Result = ModelBindingResult.Success(value);
        return Task.CompletedTask;
    }
}