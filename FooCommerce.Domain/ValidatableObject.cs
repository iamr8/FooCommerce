using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace FooCommerce.Domain
{
    /// <summary>
    /// Initializes a <see cref="ValidatableObject"/> to validate object's properties
    /// </summary>
    public abstract class ValidatableObject
    {
        /// <summary>
        /// Validates a property manually, based on Data Annotations.
        /// </summary>
        /// <param name="validationContext">A <see cref="ValidationContext"/></param>
        /// <param name="value">A <see cref="object"/> value that should be set to property info to check if validated.</param>
        /// <param name="validationResult">An <see cref="ICollection{T}"/> object that representing errors found based on Annotations rules.</param>
        /// <returns>A <see cref="bool"/> value</returns>
        /// <remarks>If true property value has been validated, and not validated otherwise.</remarks>
        public static bool TryValidateProperty(ValidationContext validationContext, object value, out ICollection<ValidationResult> validationResult)
        {
            validationResult = null;
            validationResult ??= new List<ValidationResult>();
            var valid = Validator.TryValidateProperty(value, validationContext, validationResult);
            if (validationResult != null && !validationResult.Any())
                validationResult = null;

            return valid;
        }

        /// <summary>
        /// Validates a property manually, based on Data Annotations.
        /// </summary>
        /// <param name="validationContext">A <see cref="ValidationContext"/></param>
        /// <param name="validationResult">An <see cref="ICollection{T}"/> object that representing errors found based on Annotations rules.</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <returns>A <see cref="bool"/> value</returns>
        /// <remarks>If true property value has been validated, and not validated otherwise.</remarks>
        public bool TryValidateProperty([NotNull] ValidationContext validationContext, out ICollection<ValidationResult> validationResult)
        {
            if (validationContext == null)
                throw new ArgumentNullException(nameof(validationContext));

            var prop = validationContext.ObjectType
                .GetProperties(BindingFlags.Instance | BindingFlags.Public)
                .Where(x => x.GetGetMethod() != null)
                .Where(x => x.GetCustomAttributes().Any(c => c is ValidationAttribute))
                .FirstOrDefault(x => x.Name == validationContext.MemberName);
            if (prop == null)
                throw new ArgumentNullException($"Unable to find {validationContext.MemberName} in the given object.");

            validationResult = null;
            var value = prop.GetValue(validationContext.ObjectInstance);
            return TryValidateProperty(validationContext, value, out validationResult);
        }

        /// <summary>
        ///     Tests whether the given object instance is valid.
        /// </summary>
        /// <remarks>
        ///     This method evaluates all <see cref="ValidationAttribute" />s attached to the object instance's type.  It also
        ///     checks to ensure all properties marked with <see cref="RequiredAttribute" /> are set.  It does not validate the
        ///     property values of the object.
        /// </remarks>
        /// <param name="validationContext">Describes the object to validate and provides services and context for the validators.</param>
        /// <returns><c>true</c> if the object is valid, <c>false</c> if any validation errors are encountered.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        public IEnumerable<ValidationResult>? Validate(ValidationContext validationContext)
        {
            var validationResult = new List<ValidationResult>();

            Validator.TryValidateObject(this, validationContext, validationResult, true);

            if (!validationResult.Any())
                validationResult = null;

            return validationResult;
        }

        public bool Validate()
        {
            return !(this.Validate(new ValidationContext(this)) ?? Array.Empty<ValidationResult>()).Any();
        }
    }
}