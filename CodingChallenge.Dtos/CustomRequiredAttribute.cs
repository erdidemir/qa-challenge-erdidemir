using CodingChallenge.Common.Constants;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace CodingChallenge.Dtos
{
    [ExcludeFromCodeCoverage]
    public class CustomRequiredAttribute : RequiredAttribute
    {
        private readonly Type _type;

        public CustomRequiredAttribute(Type type)
        {
            _type = type;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            bool? isInvalid = null;
            string errorMessage = string.Format(StandardValidationErrorMessageConstants.RequiredErrorMessage, validationContext.MemberName);

            if (value is null)
            {
                isInvalid = true;
            }
            else
            {
                if (typeof(DateTime) == _type && value is DateTime dateTimeValue)
                {
                    isInvalid = dateTimeValue == DateTime.MinValue || dateTimeValue == DateTime.MaxValue;
                }
                else if (typeof(Guid) == _type && value is Guid guidValue)
                {
                    isInvalid = guidValue == Guid.Empty;
                }
                else if (typeof(int) == _type && value is int intValue)
                {
                    isInvalid = intValue == default(int);
                }
                else if (typeof(long) == _type && value is long longValue)
                {
                    isInvalid = longValue == default(long);
                }
                else if (typeof(float) == _type && value is float floatValue)
                {
                    isInvalid = floatValue == default(float);
                }
                else if (typeof(double) == _type && value is double doubleValue)
                {
                    isInvalid = doubleValue == default(double);
                }
                else if (typeof(decimal) == _type && value is decimal decimalValue)
                {
                    isInvalid = decimalValue == default(decimal);
                }
                else if (typeof(string) == _type && value is string stringValue)
                {
                    isInvalid = string.IsNullOrWhiteSpace(stringValue);
                }
                else if (typeof(object) == _type)
                {
                    isInvalid = value is null;
                }
            }

            if (isInvalid.HasValue)
            {
                if (isInvalid.Value)
                {
                    return new ValidationResult(errorMessage, new[] { validationContext.MemberName });
                }
                else
                {
                    return ValidationResult.Success;
                }
            }

            return base.IsValid(value, validationContext);
        }
    }
}
