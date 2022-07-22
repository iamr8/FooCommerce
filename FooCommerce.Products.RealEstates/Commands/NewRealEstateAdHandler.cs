using FooCommerce.Products.Domain.Models;
using FooCommerce.Products.RealEstates.Application.Models;

using MediatR;

namespace FooCommerce.Products.RealEstates.Commands;

public class NewRealEstateAdHandler : IRequestHandler<NewAdRequestWrapper<NewRealEstateAdRequest>, bool>
{
    public Task<bool> Handle(NewAdRequestWrapper<NewRealEstateAdRequest> request, CancellationToken cancellationToken)
    {
        var model = request.Model;
        return Task.FromResult(true);
    }
}