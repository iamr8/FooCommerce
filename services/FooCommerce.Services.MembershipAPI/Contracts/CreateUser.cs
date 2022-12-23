﻿namespace FooCommerce.MembershipService.Contracts;

public interface CreateUser
{
    string FirstName { get; }
    string LastName { get; }
    string Email { get; }
    string Password { get; }
    //uint Country { get; }
}