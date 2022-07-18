﻿using FooCommerce.Domain.DbProvider;
using FooCommerce.Membership.Enums;

namespace FooCommerce.Membership.Entities
{
    public class UserContact : Entity
    {
        public UserContactType Type { get; set; }
        public string Name { get; set; }
        public bool IsVerified { get; set; }
        public Guid UserId { get; set; }
    }
}