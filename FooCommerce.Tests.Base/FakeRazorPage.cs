using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FooCommerce.Tests.Base;

public class FakeRazorPage<TModel> : Page
{
    public TModel Model { get; set; }

    public override Task ExecuteAsync()
    {
        throw new NotImplementedException();
    }
}