using System.ComponentModel.DataAnnotations;

namespace FooCommerce.Ordering.Client;

public record SubmitOrderModel([Required] Guid OrderId, [Required][MinLength(6)] string OrderNumber);