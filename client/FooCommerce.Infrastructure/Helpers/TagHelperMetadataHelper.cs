using System.Reflection;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace FooCommerce.Infrastructure.Helpers;

public static class TagHelperMetadataHelper
{
    public static ModelExpression GetPropertyModelExpression(this ModelExpression model, string propertyName)
    {
        return new ModelExpression($"{model.Name}.{propertyName}", model.ModelExplorer.GetExplorerForProperty(propertyName));
    }

    public static void ApplyNewBindProperty(this ModelMetadata metadata, ref TagHelperOutput output)
    {
        var propName = metadata.PropertyName;

        var containerType = metadata.ContainerType;
        var propInfo = containerType
            .GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .FirstOrDefault(x => x.Name.Equals(propName, StringComparison.CurrentCulture));

        var bindPropertyAttribute = propInfo?.GetCustomAttribute<BindPropertyAttribute>();
        if (bindPropertyAttribute == null)
            return;

        if (!string.IsNullOrEmpty(bindPropertyAttribute.Name))
            output.Attributes.Insert(0, new TagHelperAttribute("name", bindPropertyAttribute.Name));
    }
}