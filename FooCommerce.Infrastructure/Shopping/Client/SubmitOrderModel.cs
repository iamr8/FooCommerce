using System.ComponentModel.DataAnnotations;

namespace FooCommerce.Infrastructure.Shopping.Client;

public record SubmitOrderModel([Required] Guid OrderId, [Required][MinLength(6)] string OrderNumber);