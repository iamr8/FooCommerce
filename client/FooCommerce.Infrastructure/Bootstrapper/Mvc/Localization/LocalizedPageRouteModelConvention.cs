using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace FooCommerce.Infrastructure.Bootstrapper.Mvc.Localization;

public class LocalizedPageRouteModelConvention : IPageRouteModelConvention
{
    public void Apply(PageRouteModel model)
    {
#if DEBUG
        var fullPath = model.RelativePath;
        var inFolderPath = model.ViewEnginePath;
        var pageName = fullPath.Split('/')[^1];
        var areaName = model.AreaName;
        var cultures = new[] { "tr", "en" };
#endif

        var selectorCount = model.Selectors.Count;
        for (var i = 0; i < selectorCount; i++)
        {
            var selector = model.Selectors[i];
            var template = selector.AttributeRouteModel.Template;
            model.Selectors.Add(new SelectorModel
            {
                AttributeRouteModel = new AttributeRouteModel
                {
                    Order = -1,
                    Template = $"/{{{LanguageConstraints.LanguageKey}:{LanguageConstraints.LanguageKey}}}/{template}"
                }
            });
        }
    }
}