﻿using FooCommerce.Infrastructure.Shopping.Client;
using FooCommerce.Infrastructure.Shopping.Contracts;

using MassTransit;

using Microsoft.AspNetCore.Mvc;

namespace FooCommerce.Web.Controllers;

[ApiController]
[Route("[controller]")]
public class OrderController : ControllerBase
{
    [HttpGet("{orderId}")]
    [ProducesResponseType(typeof(OrderModel), StatusCodes.Status200OK)]
    public async Task<IActionResult> Get(Guid orderId, CancellationToken cancellationToken, [FromServices] IRequestClient<GetOrder> getOrderClient)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        Response response = await getOrderClient.GetResponse<Order, OrderNotFound>(new
        {
            orderId,
        }, cancellationToken);

        return response switch
        {
            (_, Order x) => Ok(new OrderModel
            {
                OrderId = x.OrderId,
                OrderNumber = x.OrderNumber,
                Status = x.Status
            }),
            (_, OrderNotFound x) => NotFound(new OrderModel
            {
                OrderId = x.OrderId,
            }),
            _ => BadRequest(new OrderModel
            {
                OrderId = orderId,
            })
        };
    }

    [HttpPost]
    [ProducesResponseType(typeof(OrderModel), StatusCodes.Status200OK)]
    public async Task<IActionResult> Submit([FromBody] SubmitOrderModel orderModel, CancellationToken cancellationToken, [FromServices] IPublishEndpoint publishEndpoint)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        await publishEndpoint.Publish<SubmitOrder>(new
        {
            orderModel.OrderId,
            orderModel.OrderNumber
        }, cancellationToken);

        return Accepted(new OrderModel
        {
            OrderId = orderModel.OrderId,
            OrderNumber = orderModel.OrderNumber,
            Status = "Submitted"
        });
    }

    [HttpPost("{orderId}/accept")]
    [ProducesResponseType(typeof(OrderModel), StatusCodes.Status200OK)]
    public async Task<IActionResult> Accept(Guid orderId, CancellationToken cancellationToken, [FromServices] IRequestClient<AcceptOrder> acceptOrderClient)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        Response response = await acceptOrderClient.GetResponse<Order, OrderNotFound>(new
        {
            orderId,
        }, cancellationToken);

        return response switch
        {
            (_, Order x) => Ok(new OrderModel
            {
                OrderId = x.OrderId,
                OrderNumber = x.OrderNumber,
                Status = x.Status
            }),
            (_, OrderNotFound x) => NotFound(new OrderModel
            {
                OrderId = x.OrderId,
            }),
            _ => BadRequest(new OrderModel
            {
                OrderId = orderId,
            })
        };
    }
}