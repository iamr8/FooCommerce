using System.Diagnostics;
using System.Linq.Expressions;
using System.Reflection;
using System.Text.Encodings.Web;

using FooCommerce.Core.Helpers;
using FooCommerce.Tests.Fakes;
using FooCommerce.Tests.Fakes.Models;
using FooCommerce.Tests.Fakes.Providers;
using FooCommerce.Tests.Helpers;

using MassTransit.Internals;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Primitives;

using Moq;

namespace FooCommerce.Tests.Extensions;

public static class HttpContextExtensions
{
    public static ViewContext GetViewContext(this IHtmlGenerator htmlGenerator, object model, IModelMetadataProvider metadataProvider)
    {
        return TestableHtmlGenerator.GetViewContext(model, htmlGenerator, metadataProvider, modelState: new ModelStateDictionary());
    }

    public static FakeRazorPage<TPageModel> GetRazorPage<TPageModel>(this TPageModel pageModel) where TPageModel : PageModel
    {
        var mvcOptions = TestableHtmlGenerator.GetOptions();
        var htmlGenerator = new TestableHtmlGenerator(pageModel.MetadataProvider, mvcOptions, pageModel.Url, null);
        var viewContext = htmlGenerator.GetViewContext(pageModel, pageModel.MetadataProvider);
        var instance = Activator.CreateInstance<FakeRazorPage<TPageModel>>();
        instance.MetadataProvider = pageModel.MetadataProvider;
        instance.PageContext = pageModel.PageContext;
        instance.ViewContext = viewContext;
        instance.DiagnosticSource = new DiagnosticListener("MoqRazor");
        instance.HtmlEncoder = HtmlEncoder.Default;
        instance.Model = pageModel;
        return instance;
    }

    public static TPage GetPage<TPage>(this HttpContext httpContext, string reqPath, object routeValues, params object[] args) where TPage : PageModel
    {
        var routeDic = routeValues == null ? new Dictionary<string, object>() : AnonymousReflection.ToDictionary(routeValues);
        routeDic.TryAdd("page", reqPath);
        var routeValueDictionary = new RouteValueDictionary(routeDic);

        httpContext = httpContext.GetHttpContextAccessor(reqPath, routeValueDictionary).HttpContext;
        var page = (TPage)Activator.CreateInstance(typeof(TPage), args);

        var tempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>());
        var modelState = new ModelStateDictionary();

        var routeData = new RouteData(httpContext.Request.RouteValues);
        var actionContext = new ActionContext(httpContext, routeData, new PageActionDescriptor(), modelState);
        var modelMetadataProvider = new EmptyModelMetadataProvider();
        // var modelMetadataProvider = new TestModelMetadataProvider();
        var viewData = new ViewDataDictionary(modelMetadataProvider, modelState)
        {
            Model = page
        };

        var pageContext = new PageContext(actionContext)
        {
            ViewData = viewData,
            HttpContext = httpContext,
            RouteData = routeData,
        };

        var urlHelper = new UrlHelper(actionContext);
        page.PageContext = pageContext;
        page.Url = urlHelper;
        page.TempData = tempData;

        return page;
    }

    public static TPage GetPage<TPage>(this HttpContext httpContext, object routeValues, params object[] args) where TPage : PageModel
    {
        var page = (TPage)Activator.CreateInstance(typeof(TPage), args);

        var tempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>());
        var modelState = new ModelStateDictionary();
        var routeDic = AnonymousReflection.ToDictionary(routeValues);
        routeDic.TryAdd("page", "/index");
        var routeData = new RouteData(new RouteValueDictionary(routeDic));
        var actionContext = new ActionContext(httpContext, routeData, new PageActionDescriptor(), modelState);
        var modelMetadataProvider = new TestModelMetadataProvider();
        var viewData = new ViewDataDictionary(modelMetadataProvider, modelState);

        page.PageContext = new PageContext(actionContext)
        {
            ViewData = viewData,
            HttpContext = httpContext,
            RouteData = routeData,
        };
        // page.Url = MockObjects.GetUrlHelper();
        page.Url = new UrlHelper(actionContext);
        page.TempData = tempData;
        page.PageContext.ViewData.Model = page;

        return page;
    }

    public static TController GetController<TController>(this HttpContext httpContext, params object[] args) where TController : Controller
    {
        // var ctor = typeof(TController).GetConstructors(BindingFlags.Public).ToList();
        var controller = (TController)Activator.CreateInstance(typeof(TController), args);
        controller.ControllerContext = new ControllerContext { HttpContext = httpContext };
        controller.Url = MockObjects.GetUrlHelper();
        return controller;
    }

    public static FormCollection GetFormCollection(this object input, string razorVariable)
    {
        if (input == null) throw new ArgumentNullException(nameof(input));
        if (razorVariable == null) throw new ArgumentNullException(nameof(razorVariable));

        var result = new Dictionary<string, StringValues>();
        var properties = input.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance).ToList()
            .Where(x => x.GetCustomAttribute<BindNeverAttribute>() == null)
            .GroupBy(x => x.Name)
            .Select(x => x.First())
            .ToList();
        if (properties?.Any() == true)
        {
            foreach (var propertyInfo in properties)
            {
                var obj = propertyInfo.GetValue(input);
                if (obj != null)
                    obj = propertyInfo.PropertyType.GetWritableStringValues(obj);

                var key = $"{razorVariable}[{propertyInfo.Name.ToLowerInvariant()}]";
                result.Add(key, obj?.ToString());
            }
        }

        var formCollection = new FormCollection(result);
        return formCollection;
    }

    public static HttpContext GetRequestForm(this HttpContext httpContext, object input = null, string razorVariable = null)
    {
        var form = input != null
            ? input.GetFormCollection(razorVariable)
            : new FormCollection(new Dictionary<string, StringValues>());

        var defaultHttpContext = new DefaultHttpContext
        {
            Request =
            {
                Protocol = "HTTP/1.1",
                Path = "/",
                PathBase = new PathString(),
                Scheme = "http",
                Host = new HostString("localhost"),
                Form = form,
                ContentType = "application/json",
                Method = "POST",
            }
        };

        var moqHttpContext = new Mock<HttpContext>();
        moqHttpContext.SetupGet(x => x.User).Returns(httpContext.User);
        moqHttpContext.SetupGet(x => x.RequestServices).Returns(httpContext.RequestServices);
        moqHttpContext.SetupGet(x => x.Items).Returns(httpContext.Items);
        moqHttpContext.SetupGet(x => x.Request).Returns(defaultHttpContext.Request);
        return moqHttpContext.Object;
    }

    public static Dictionary<string, StringValues> AddRange(this Dictionary<string, StringValues> source,
        Dictionary<string, StringValues> dictionary)
    {
        foreach (var (key, value) in dictionary)
        {
            source.Add(key, value);
        }

        return source;
    }

    public static IHtmlGenerator GetHtmlGenerator<TModel>(this TestModelMetadataProvider metadataProvider, TModel input, Expression<Func<TModel>> containerAccessor, Expression<Func<TModel, object>> modelAccessor, out ModelExpression modelExpression) where TModel : class
    {
        var containerVar = containerAccessor.GetMemberExpression().Member.Name;
        var containerType = containerAccessor.GetType();

        var containerMetadata = metadataProvider.GetMetadataForType(containerType);
        var containerExplorer = metadataProvider.GetModelExplorerForType(containerType, input);

        var memberExpression = modelAccessor.GetMemberExpression();
        var member = memberExpression.Member;
        var propertyInfo = (PropertyInfo)member;

        var propertyMetadata = metadataProvider.GetMetadataForProperty(propertyInfo, propertyInfo.PropertyType);
        var modelExplorer = containerExplorer.GetExplorerForExpression(propertyMetadata, modelAccessor.Compile().Invoke(input));

        modelExpression = new ModelExpression($"{containerVar}.{member.Name}", modelExplorer);

        var htmlGenerator = new TestableHtmlGenerator(metadataProvider);
        return htmlGenerator;
    }

    internal static IHttpContextAccessor AddRequestRouteValues(this IHttpContextAccessor httpContextAccessor)
    {
        if (!httpContextAccessor.HttpContext.Request.RouteValues.ContainsKey("page"))
            httpContextAccessor.HttpContext.Request.RouteValues.Add("page", "/Index");

        return httpContextAccessor;
    }

    public static void SetFormCollection(this HttpContext httpContext, Dictionary<string, StringValues> dictionary)
    {
        var formDictionary = new Dictionary<string, StringValues>();
        formDictionary.AddRange(dictionary);

        httpContext.Request.Form = new FormCollection(formDictionary);
    }

    public static IHttpContextAccessor AddHttpPost(this IHttpContextAccessor httpContextAccessor)
    {
        httpContextAccessor.AddRequestRouteValues();
        httpContextAccessor.HttpContext.Request.ContentType = "application/x-www-form-urlencoded";

        return httpContextAccessor;
    }
}