using System.Text.RegularExpressions;

using FluentValidation;

using PhoneNumbers;

namespace FooCommerce.IdentityAPI.Worker.Validators.PropertyValidators;

public class MobileValidator : AbstractValidator<string>
{
    public MobileValidator()
    {
        RuleFor(x => x).NotEmpty().WithMessage("Mobile is required.");
        RuleFor(x => x).Must(ValidatePhone);
    }

    private static bool ValidatePhone(string value)
    {
        var checkMobile = Regex.Matches(value, "\\d+");
        if (checkMobile.Count <= 0)
            return false;

        var mobile = string.Concat(checkMobile.Select(x => x.Value));
        var phoneNumberUtil = PhoneNumberUtil.GetInstance();
        try
        {
            var phoneNumberBuilder = new PhoneNumber.Builder
            {
                CountryCodeSource = PhoneNumber.Types.CountryCodeSource.FROM_NUMBER_WITH_PLUS_SIGN,
            };
            phoneNumberUtil.Parse($"+{mobile}", null, phoneNumberBuilder);
            var phoneNumber = phoneNumberBuilder.Build();
            var valid = phoneNumberUtil.IsValidNumber(phoneNumber);
            if (valid)
            {
                var numberType = phoneNumberUtil.GetNumberType(phoneNumber);
                if (numberType == PhoneNumberType.MOBILE)
                {
                    return true;
                }
            }
        }
        catch (Exception e)
        {
            return false;
        }

        return false;
    }
}