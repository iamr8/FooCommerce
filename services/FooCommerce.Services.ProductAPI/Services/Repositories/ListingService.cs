using FooCommerce.CatalogService.DbProvider;
using FooCommerce.CatalogService.Models;
using FooCommerce.Domain.Pagination;
using FooCommerce.EntityFrameworkCore.Extensions.Pagination;
using Microsoft.EntityFrameworkCore;

namespace FooCommerce.CatalogService.Services.Repositories;

public class ListingService : IListingService
{
    private readonly ProductDbContext _dbContext;

    public ListingService(ProductDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<PagedList<ListingOverviewModel>> ToPagedListAsync(ProductSearchModel search,
        CancellationToken cancellationToken = default)
    {
        if (search == null)
            throw new ArgumentNullException(nameof(search));

        var query = _dbContext.Listings
            .AsNoTracking()
            .AsSplitQuery()
            .Where(x => x.Product.Catalog.ExternalId == search.CatalogId);

        var result = await query.ToPagedListAsync(listing => new
        {
            listing.ExternalId,
            listing.Product.Name,
            Image = listing.Product.Medias
                .Where(c => c.IsCover && !c.IsVideo)
                .MaxBy(c => c.Created)
                .Path,
            Specifications = listing.Product.Specifications
                .Select(c => new {c.Specification.Name, c.Value}),
            Ratings = listing.Ratings
                .Select(x => new {x.Rate}),
            Likes = listing.Likes.Count,
            Price = listing.Prices.OrderByDescending(c => c.Created).First().Amount,
        }, search.PageNo, search.PageSize, cancellationToken);
        if (!result.Any())
            return PagedList<ListingOverviewModel>.Empty;

        var output = new PagedList<ListingOverviewModel>(result.PageNo, result.Pages, result.TotalCount, search)
        {
            Items = result.Items.Select(listing => new ListingOverviewModel
            {
                Id = listing.ExternalId,
                Name = listing.Name,
                Image = listing.Image,
                Specifications = listing.Specifications.ToDictionary(x => x.Name, x => x.Value),
                RatingsAverage = (int) listing.Ratings.Average(x => x.Rate),
                RatingsCount = listing.Ratings.Count(),
                Likes = listing.Likes,
                Price = listing.Price
            }).ToList()
        };
        return output;
    }
}