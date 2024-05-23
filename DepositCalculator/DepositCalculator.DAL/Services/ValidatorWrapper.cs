using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using DepositCalculator.DAL.Interfaces;

namespace DepositCalculator.DAL.Services
{
    /// <inheritdoc cref="IValidatorWrapper" />
    [ExcludeFromCodeCoverage]
    public class ValidatorWrapper : IValidatorWrapper
    {
        /// <inheritdoc />
        public void ValidateObject(object instance, ValidationContext validationContext, bool validateAllProperties)
        {
            Validator.ValidateObject(instance, validationContext, validateAllProperties);
        }
    }
}