using System;
using System.ComponentModel.DataAnnotations;

namespace DepositCalculator.DAL.Interfaces
{
    /// <summary>
    /// A contract for a wrapper around the <see cref="Validator" />.
    /// </summary>
    public interface IValidatorWrapper
    {
        /// <summary>
        /// Throws a <see cref="ValidationException" /> if the given object instance is not valid.
        /// </summary>
        /// <param name="instance">The object instance to test. It can not be null.</param>
        /// <param name="validationContext">
        /// Describes the object being validated and provides services and context for the
        /// validators. It can not be <c>null</c>.
        /// </param>
        /// <param name="validateAllProperties">
        /// If <c>true</c>, also validates all the <paramref name="instance" />'s properties.
        /// </param>
        /// <exception cref="ArgumentNullException">When <paramref name="instance" /> is null.</exception>
        /// <exception cref="ArgumentNullException">When <paramref name="validationContext" /> is null.</exception>
        /// <exception cref="ArgumentException">
        /// When <paramref name="instance" /> does not match the <see cref="ValidationContext.ObjectInstance" />
        /// on <paramref name="validationContext" />.
        /// </exception>
        /// <exception cref="ValidationException">When <paramref name="instance" /> is found to be invalid.</exception>
        void ValidateObject(object instance, ValidationContext validationContext, bool validateAllProperties);
    }
}