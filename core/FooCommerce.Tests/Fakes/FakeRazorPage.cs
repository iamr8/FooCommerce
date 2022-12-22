using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FooCommerce.Tests.Fakes;

public class FakeRazorPage<TModel> : Page
{
    public TModel Model { get; set; }

    public override Task ExecuteAsync()
    {
        throw new NotImplementedException();
    }
}