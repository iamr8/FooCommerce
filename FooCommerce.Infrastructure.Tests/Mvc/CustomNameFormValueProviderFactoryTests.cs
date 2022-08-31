using System.Globalization;
using FooCommerce.Infrastructure.Bootstrapper.Mvc.ModelBinding.CustomProviders;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Primitives;

namespace FooCommerce.Infrastructure.Tests.Mvc
{
    public class CustomNameFormValueProviderFactoryTests
    {
        [Theory]
        [InlineData("input.Htmls", "input.Htmls", "Arash")]
        [InlineData("input.Htmls", "input[htmls]", "Arash")]
        [InlineData("input.Htmls.Html", "input.Htmls.Html", "Arash")]
        [InlineData("input.Htmls.Html", "input[htmls.html]", "Arash")]
        [InlineData("input.Htmls[0].Html", "input.Htmls[0].Html", "Arash")]
        [InlineData("input.Htmls[0].Html", "input[htmls[0].html]", "Arash")]
        public void Normal_Empty(string key, string formKey, string value)
        {
            var form = new FormCollection(new Dictionary<string, StringValues> { { formKey, value } });
            var provider = new CustomNameFormValueProvider(BindingSource.Form, form, CultureInfo.CurrentCulture);

            var providerResult = provider.GetValue(key);
            Assert.Equal(value, providerResult.Values);
        }

        [Fact]
        public void Checkbox()
        {
            var form = new FormCollection(new Dictionary<string, StringValues>
            {
                { "input[showrequestdata]", "true" },
                { "input.ShowRequestData", "false"}
            });
            var provider = new CustomNameFormValueProvider(BindingSource.Form, form, CultureInfo.CurrentCulture);

            var providerResult = provider.GetValue("input.ShowRequestData");
            Assert.Equal("true", providerResult.Values);
        }

        [Fact]
        public void Checkbox2()
        {
            var form = new FormCollection(new Dictionary<string, StringValues>
            {
                { "input.ShowRequestData", "false"},
                { "input[showrequestdata]", "true" }
            });
            var provider = new CustomNameFormValueProvider(BindingSource.Form, form, CultureInfo.CurrentCulture);

            var providerResult = provider.GetValue("input.ShowRequestData");
            Assert.Equal("true", providerResult.Values);
        }
    }
}