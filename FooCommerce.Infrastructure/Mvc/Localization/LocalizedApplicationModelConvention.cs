using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace FooCommerce.Infrastructure.Mvc.Localization
{
    public class LocalizedApplicationModelConvention : IApplicationModelConvention
    {
        private readonly AttributeRouteModel _routePrefix;

        public LocalizedApplicationModelConvention()
        {
            _routePrefix = new AttributeRouteModel(new RouteAttribute(Constraints.LanguageKey));
        }

        public void Apply(ApplicationModel application)
        {
            var controllers = application.Controllers
                .Where(x => x.ControllerType.Namespace.StartsWith("Ecohos", StringComparison.InvariantCultureIgnoreCase))
                .ToList();
            foreach (var controller in controllers)
            {
                var controllerSelector = controller.Selectors.SingleOrDefault();
                var controllerRouteModel = controllerSelector?.AttributeRouteModel;
                if (controllerRouteModel == null)
                {
                    ApplyActions(controller);
                    continue;
                }

                var controllerTemplate = controllerRouteModel.Template;
                if (string.IsNullOrEmpty(controllerTemplate))
                    continue;

                if (controller.Filters.All(x => x.GetType() != typeof(MiddlewareFilterAttribute)))
                    controller.Filters.Add(new MiddlewareFilterAttribute(typeof(LocalizationPipeline)));

                if (controllerTemplate.StartsWith("/"))
                {
                    controller.Selectors.Add(new SelectorModel
                    {
                        AttributeRouteModel = new AttributeRouteModel
                        {
                            Order = -1,
                            Template = $"/{{{Constraints.LanguageKey}:{Constraints.LanguageKey}}}{controllerTemplate}",
                            Name = !string.IsNullOrEmpty(controllerRouteModel.Name) ? $"{controllerRouteModel.Name}_Culture" : null,
                            SuppressLinkGeneration = controllerRouteModel.SuppressLinkGeneration,
                            SuppressPathMatching = controllerRouteModel.SuppressPathMatching
                        }
                    });
                }

                ApplyActions(controller);
            }
        }

        private void ApplyActions(ControllerModel controller)
        {
            if (controller.Actions.Any())
            {
                foreach (var action in controller.Actions)
                {
                    var newSelectors = new List<SelectorModel>();
                    foreach (var actionSelector in action.Selectors)
                    {
                        var actionRouteModel = actionSelector?.AttributeRouteModel;
                        if (actionRouteModel == null)
                            continue;

                        var actionTemplate = actionRouteModel.Template;
                        if (string.IsNullOrEmpty(actionTemplate))
                            continue;

                        if (action.Filters.All(x => x.GetType() != typeof(MiddlewareFilterAttribute)))
                            action.Filters.Add(new MiddlewareFilterAttribute(typeof(LocalizationPipeline)));

                        if (actionTemplate.StartsWith("/"))
                        {
                            newSelectors.Add(new SelectorModel
                            {
                                AttributeRouteModel = new AttributeRouteModel
                                {
                                    Order = -1,
                                    Template = $"/{{{Constraints.LanguageKey}:{Constraints.LanguageKey}}}{actionTemplate}",
                                    Name = !string.IsNullOrEmpty(actionRouteModel.Name) ? $"{actionRouteModel.Name}_Culture" : null,
                                    SuppressLinkGeneration = actionRouteModel.SuppressLinkGeneration,
                                    SuppressPathMatching = actionRouteModel.SuppressPathMatching
                                }
                            });
                        }
                    }

                    if (newSelectors.Any())
                        foreach (var selectorModel in newSelectors)
                            action.Selectors.Add(selectorModel);
                }
            }
        }
    }
}