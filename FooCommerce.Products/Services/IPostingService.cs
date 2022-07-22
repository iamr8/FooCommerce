using FooCommerce.Products.Domain.Interfaces;
using FooCommerce.Products.Domain.Models;

using MediatR;

namespace FooCommerce.Products.Services
{
    public interface IPostingService
    {
        Task<bool> NewAsync<TRequest>(TRequest request) where TRequest : IAdRequest;
    }

    public class PostingService : IPostingService
    {
        private readonly IMediator _mediator;

        public PostingService(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<bool> NewAsync<TRequest>(TRequest request) where TRequest : IAdRequest
        {
            var type = typeof(NewAdRequestWrapper<>).MakeGenericType(request.GetType());
            var _request = Activator.CreateInstance(type, request);
            var response = (bool)await _mediator.Send(_request);
            return response;
        }
    }
}