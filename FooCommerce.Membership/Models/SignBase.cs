using System.ComponentModel.DataAnnotations;

using FooCommerce.Domain;

namespace FooCommerce.Membership.Models
{
    public abstract class SignBase : ValidatableObject
    {
        [Required]
        [MinLength(2)]
        public virtual string Username { get; internal set; }

        [Required]
        [MinLength(8)]
        public virtual string Password { get; internal set; }
    }
}