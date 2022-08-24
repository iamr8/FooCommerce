﻿using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace FooCommerce.Infrastructure.Mvc.ModelBinders;

public class DateTimeUtcModelBinder : IModelBinder
{
    public Task BindModelAsync(ModelBindingContext bindingContext)
    {
        if (bindingContext == null)
            throw new ArgumentNullException(nameof(bindingContext));

        if (bindingContext.ModelType != typeof(DateTime) && bindingContext.ModelType != typeof(DateTime?))
            return Task.FromResult(ModelBindingResult.Failed());

        var key = bindingContext.ModelName;
        if (string.IsNullOrEmpty(key))
            return Task.CompletedTask;

        var getValue = bindingContext.ValueProvider.GetValue(key);
        var value = getValue.FirstValue;
        if (string.IsNullOrEmpty(value))
            return Task.CompletedTask;

        var hasValidDateTime = DateTime.TryParse(value, out var dateTime);
        if (!hasValidDateTime)
            return Task.FromResult(ModelBindingResult.Failed());

        dateTime = DateTime.SpecifyKind(dateTime, DateTimeKind.Utc);
        bindingContext.Result = ModelBindingResult.Success(dateTime);
        return Task.CompletedTask;
    }
}