using System.ComponentModel.DataAnnotations;
using System.Reflection;
using Microsoft.AspNetCore.Mvc;

namespace FooCommerce.CatalogService.Models;

public abstract record PagedListFilter : FooCommerce.Domain.Pagination.PagedListFilter
{
    [FromQuery(Name = "pageNo")]
    [HiddenInput]
    public override int PageNo { get; set; } = 1;

    [FromQuery(Name = "pageSize")]
    [HiddenInput]
    public override int PageSize { get; set; } = 10;

    public virtual IReadOnlyDictionary<string, object> TriggeredAttributes
    {
        get
        {
            var exclusions = new[] { nameof(PageNo), nameof(PageSize), nameof(TriggeredAttributes) };
            var props = this
                .GetType()
                .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(x => exclusions.All(c => x.Name != c))
                .ToArray();

            var dict = new Dictionary<string, object>();
            foreach (var propertyInfo in props)
            {
                var value = propertyInfo.GetValue(this);
                if (value == null || value.Equals(default))
                    continue;

                var key = propertyInfo.Name;
                var displayAttr = propertyInfo.GetCustomAttribute<DisplayAttribute>();
                if (displayAttr != null)
                    key = displayAttr.Name;

                dict.Add(key, value);
            }

            return dict;
        }
    }

    public override IDictionary<string, string> ToDictionary(bool observeEndpoint = false)
    {
        var type = GetType();
        var props = type.GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .Where(propertyInfo => !propertyInfo.Name.Equals(nameof(this.TriggeredAttributes)))
            .ToArray();

        var output = new Dictionary<string, string>();
        foreach (var propertyInfo in props)
        {
            var value = propertyInfo.GetValue(this);

            string key;
            if (observeEndpoint)
            {
                var fromQueryAttr = propertyInfo.GetCustomAttribute<FromQueryAttribute>();
                if (fromQueryAttr != null)
                {
                    key = fromQueryAttr.Name;
                }
                else
                {
                    var fromRouteAttr = propertyInfo.GetCustomAttribute<FromRouteAttribute>();
                    if (fromRouteAttr != null)
                    {
                        key = fromRouteAttr.Name;
                    }
                    else
                    {
                        key = propertyInfo.Name;
                    }
                }
            }
            else
            {
                key = propertyInfo.Name.ToLower();
            }

            if (value != null)
                output.Add(key, value.ToString()?.Trim());
        }

        return output;
    }
}