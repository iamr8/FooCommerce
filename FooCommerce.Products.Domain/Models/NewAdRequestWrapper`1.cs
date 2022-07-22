using FooCommerce.Products.Domain.Interfaces;

using MediatR;

namespace FooCommerce.Products.Domain.Models;

public class NewAdRequestWrapper<TRequest> : IRequest<bool> where TRequest : class, IAdRequest
{
    public TRequest Model;

    public NewAdRequestWrapper(TRequest model)
    {
        this.Model = model;
    }
}