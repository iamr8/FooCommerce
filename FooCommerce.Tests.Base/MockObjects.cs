using System.Globalization;
using System.Net;
using System.Security.Claims;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Routing;
using Microsoft.Net.Http.Headers;

using Moq;

namespace FooCommerce.Tests;

public static class MockObjects
{
    public static IUrlHelper GetUrlHelper(string returnUrl = "http://localhost:8080/")
    {
        var mock = new Mock<IUrlHelper>(MockBehavior.Strict);
        mock
            .Setup(x => x.Action(It.IsAny<UrlActionContext>()))
            .Returns(returnUrl)
            .Verifiable();
        return mock.Object;
    }

    public static IHttpContextAccessor GetHttpContextAccessor(this IServiceProvider serviceProvider)
    {
        return serviceProvider.GetHttpContextAccessor("/", new RouteValueDictionary());
    }

    public static IHttpContextAccessor GetHttpContextAccessor(this IServiceProvider serviceProvider, string reqPath, RouteValueDictionary routeValueDic)
    {
        // var claimMoq = new Mock<ClaimsIdentity>();
        // claimMoq.SetupGet(x => x.IsAuthenticated).Returns(authenticated && user != null);
        // claimMoq.SetupGet(x => x.Claims).Returns(user?.GetClaims() ?? new List<Claim>());
        var nonAuthenticatedClaimIdentity = new ClaimsIdentity();
        var claimsPrincipal = new ClaimsPrincipal(nonAuthenticatedClaimIdentity);

        var defaultHttpContext = new DefaultHttpContext
        {
            RequestServices = serviceProvider
        };

        var mockRepository = new MockRepository(MockBehavior.Strict);

        var sessionMoq = mockRepository.Create<ISession>();

        var headers = new HeaderDictionary
            {
                {
                    HeaderNames.UserAgent,
                    "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/94.0.4606.81 Safari/537.36 Edg/94.0.992.47"
                }
            };

        routeValueDic.TryAdd("culture", CultureInfo.CurrentCulture.TwoLetterISOLanguageName);
        var httpRequestMoq = new Mock<HttpRequest>();
        httpRequestMoq.SetupGet(x => x.Protocol).Returns("HTTP/1.1");
        httpRequestMoq.SetupGet(x => x.Path).Returns(reqPath);
        httpRequestMoq.SetupGet(x => x.PathBase).Returns(new PathString(reqPath));
        httpRequestMoq.SetupGet(x => x.Scheme).Returns("http");
        httpRequestMoq.SetupGet(x => x.Host).Returns(new HostString("localhost"));
        httpRequestMoq.SetupGet(x => x.RouteValues).Returns(routeValueDic);
        httpRequestMoq.SetupGet(x => x.Cookies).Returns(() => new Mock<IRequestCookieCollection>().Object);
        httpRequestMoq.SetupGet(x => x.Headers).Returns(headers);

        var connectionInfoMoq = new Mock<ConnectionInfo>();
        connectionInfoMoq.SetupGet(x => x.RemoteIpAddress).Returns(IPAddress.Parse("78.173.224.233"));
        connectionInfoMoq.SetupGet(x => x.RemotePort).Returns(8080);
        connectionInfoMoq.SetupGet(x => x.LocalIpAddress).Returns(IPAddress.Parse("192.168.1.103"));
        connectionInfoMoq.SetupGet(x => x.LocalPort).Returns(8080);

        var httpContextMoq = new Mock<HttpContext>();
        httpContextMoq.SetupGet(x => x.User).Returns(claimsPrincipal);
        httpContextMoq.SetupGet(x => x.RequestServices).Returns(serviceProvider);
        httpContextMoq.SetupGet(x => x.Connection).Returns(connectionInfoMoq.Object);
        httpContextMoq.SetupGet(x => x.Request).Returns(httpRequestMoq.Object);
        httpContextMoq.SetupGet(x => x.Items).Returns(defaultHttpContext.Items);
        httpContextMoq.SetupGet(x => x.TraceIdentifier).Returns(defaultHttpContext.TraceIdentifier);
        httpContextMoq.SetupGet(x => x.Session).Returns(sessionMoq.Object);
        httpContextMoq.SetupGet(x => x.Features).Returns(defaultHttpContext.Features);
        httpContextMoq.SetupGet(x => x.Response).Returns(defaultHttpContext.Response);
        httpContextMoq.SetupGet(x => x.WebSockets).Returns(defaultHttpContext.WebSockets);

        var moqHttpContextAccessor = new Mock<IHttpContextAccessor>();
        moqHttpContextAccessor.Setup(_ => _.HttpContext).Returns(httpContextMoq.Object);

        return moqHttpContextAccessor.Object;
    }

    public static IHttpContextAccessor GetHttpContextAccessor(this HttpContext currentInstance, string reqPath, RouteValueDictionary routeValueDic)
    {
        routeValueDic.TryAdd("culture", CultureInfo.CurrentCulture.TwoLetterISOLanguageName);
        var httpRequestMoq = new Mock<HttpRequest>();
        httpRequestMoq.SetupGet(x => x.Protocol).Returns("HTTP/1.1");
        httpRequestMoq.SetupGet(x => x.Path).Returns(reqPath);
        httpRequestMoq.SetupGet(x => x.PathBase).Returns(new PathString());
        httpRequestMoq.SetupGet(x => x.Scheme).Returns("http");
        httpRequestMoq.SetupGet(x => x.Host).Returns(new HostString("localhost:8080"));
        httpRequestMoq.SetupGet(x => x.RouteValues).Returns(routeValueDic);
        httpRequestMoq.SetupGet(x => x.Cookies).Returns(It.IsAny<IRequestCookieCollection>());
        httpRequestMoq.SetupGet(x => x.Headers).Returns(currentInstance.Request.Headers);
        httpRequestMoq.SetupGet(x => x.Method).Returns("GET");
        httpRequestMoq.SetupGet(x => x.Query).Returns(new QueryCollection());
        httpRequestMoq.SetupGet(x => x.QueryString).Returns(new QueryString());

        var httpContextMoq = new Mock<HttpContext>();
        httpContextMoq.SetupGet(x => x.User).Returns(currentInstance.User);
        httpContextMoq.SetupGet(x => x.RequestServices).Returns(currentInstance.RequestServices);
        httpContextMoq.SetupGet(x => x.Connection).Returns(currentInstance.Connection);
        httpContextMoq.SetupGet(x => x.Request).Returns(httpRequestMoq.Object);
        httpContextMoq.SetupGet(x => x.Items).Returns(currentInstance.Items);
        httpContextMoq.SetupGet(x => x.TraceIdentifier).Returns(currentInstance.TraceIdentifier);
        httpContextMoq.SetupGet(x => x.Session).Returns(currentInstance.Session);
        httpContextMoq.SetupGet(x => x.Features).Returns(currentInstance.Features);
        httpContextMoq.SetupGet(x => x.Response).Returns(currentInstance.Response);
        httpContextMoq.SetupGet(x => x.WebSockets).Returns(currentInstance.WebSockets);

        var moqHttpContextAccessor = new Mock<IHttpContextAccessor>();
        moqHttpContextAccessor.Setup(_ => _.HttpContext).Returns(httpContextMoq.Object);

        return moqHttpContextAccessor.Object;
    }
}