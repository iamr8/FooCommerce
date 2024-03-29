﻿using FluentValidation;

namespace FooCommerce.Infrastructure.Membership.Validators.PropertyValidators;

public class EmailValidator : AbstractValidator<string>
{
    public EmailValidator()
    {
        RuleFor(x => x).NotEmpty().WithMessage("Email is required.");
        RuleFor(x => x).EmailAddress().WithMessage("Email is not valid.");
    }
}