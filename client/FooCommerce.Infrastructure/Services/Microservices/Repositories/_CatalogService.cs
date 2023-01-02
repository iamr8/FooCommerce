using System.Net;
using System.Text.Json;
using System.Web;
using FooCommerce.Domain.Pagination;
using FooCommerce.Infrastructure.Helpers;
using FooCommerce.Infrastructure.Models;
using Microsoft.AspNetCore.Http;

namespace FooCommerce.Infrastructure.Services.Microservices.Repositories;

public class _CatalogService : ICatalogClient
{
    private readonly HttpClient _httpClient;

    private readonly IHttpContextAccessor _httpContextAccessor;

    public _CatalogService(HttpClient httpClient, IHttpContextAccessor httpContextAccessor)
    {
        _httpClient = httpClient;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<CatalogOverviewModel> GetCatalogOverviewAsync(string slug, CancellationToken cancellationToken = default)
    {
        if (slug == null)
            throw new ArgumentNullException(nameof(slug));

        var uri = new Uri("api/catalogs/overview", UriKind.Relative);
        var query = HttpUtility.ParseQueryString(string.Empty);
        query["slug"] = slug;
        uri = new Uri(uri + "?" + query, UriKind.Relative);

        try
        {
            var response = await _httpClient.GetAsync(uri, cancellationToken);
            response.EnsureSuccessStatusCode();

            var model = await JsonSerializer.DeserializeAsync<CatalogOverviewModel>(await response.Content.ReadAsStreamAsync(cancellationToken));
            return model;
        }
        catch (HttpRequestException e) when (e.StatusCode == HttpStatusCode.NotFound)
        {
            return null;
        }
        catch (Exception e)
        {
            return null;
        }
    }

    private record _ListingOverviewModel
    {
        public uint Id { get; init; }
        public string Name { get; init; }
        public string Image { get; init; }
        public Dictionary<string,string> Specifications { get; init; }
        public int RatingsAverage { get; init; }
        public int RatingsCount { get; init; }
        public int Likes { get; init; }
        public decimal Price { get; set; }
    }

    public async Task<PagedList<ProductOverviewModel>> GetProductsOverviewAsync(int catalogId, CancellationToken cancellationToken = default)
    {
        var uri = new Uri("api/products/list", UriKind.Relative);
        var query = HttpUtility.ParseQueryString(string.Empty);
        query["catalogId"] = catalogId.ToString();
        uri = new Uri(uri + "?" + query, UriKind.Relative);

        try
        {
            var response = await _httpClient.GetAsync(uri, cancellationToken);
            response.EnsureSuccessStatusCode();

            var models = await JsonSerializer.DeserializeAsync<PagedList<_ListingOverviewModel>>(await response.Content.ReadAsStreamAsync(cancellationToken));
            if (models.Items.Any())
            {
                var output = new PagedList<ProductOverviewModel>(1, 1, 10)
                {
                    Items = models.Select(x=> new ProductOverviewModel
                    {
                        Id = (int) x.Id,
                        Name = x.Name,
                        Images = new List<UrlModel> {new(x.Image)},
                        Rating = new RatingModel(x.RatingsAverage, x.RatingsCount) ,
                        Price = new ProductPriceModel(x.Price, "$"),
                        Url = new UrlModel(_httpContextAccessor.HttpContext.GetWebsiteUrl() + "/products/" + x.Id),
                        Properties = new ProductOverviewPropertiesModel(),
                        Colors = new List<ProductColorModel>()
                    }).ToList()
                };

                return output;
            }

            return PagedList<ProductOverviewModel>.Empty;
        }
        catch (HttpRequestException e) when (e.StatusCode == HttpStatusCode.NotFound)
        {
            return PagedList<ProductOverviewModel>.Empty;
        }
        catch (Exception e)
        {
            return PagedList<ProductOverviewModel>.Empty;
        }
    }
}