using FooCommerce.Products.Domain.Models;
using FooCommerce.Products.Vehicles.Models;

using MediatR;

namespace FooCommerce.Products.Vehicles.Commands;

public class NewVehicleAdHandler : IRequestHandler<NewAdRequestWrapper<NewVehicleAdRequest>, bool>
{
    public Task<bool> Handle(NewAdRequestWrapper<NewVehicleAdRequest> request, CancellationToken cancellationToken)
    {
        return Task.FromResult(true);
    }
}