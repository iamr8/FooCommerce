﻿using FluentValidation;

namespace FooCommerce.Application.Membership.Validators;

public class UsernameValidator : AbstractValidator<string>
{
    public UsernameValidator()
    {
        When(username => username.Contains('@'), () =>
        {
            RuleFor(x => x).SetValidator(new EmailValidator());
        }).Otherwise(() =>
        {
            RuleFor(x => x).SetValidator(new MobileValidator());
        });
    }
}