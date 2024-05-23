using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace DepositCalculator.API.Filters
{
    /// <summary>
    /// An action filter to validate model based on <typeparamref name="T" />.
    /// </summary>
    /// <typeparam name="T">The model type to validate.</typeparam>
    public class ModelValidationActionFilterAttribute<T> : IActionFilter where T : class
    {
        private readonly IValidator<T> _validator;

        /// <summary>
        /// Initializes a new instance of the <see cref="ModelValidationActionFilterAttribute{T}" /> class using
        /// <see cref="IValidator{T}" />.
        /// </summary>
        /// <param name="validator">
        /// The <see cref="IValidator{T}" /> that contains rules to validate <typeparamref name="T" />.
        /// </param>
        public ModelValidationActionFilterAttribute(IValidator<T> validator)
        {
            _validator = validator;
        }

        /// <summary>
        /// Sets <see cref="BadRequestObjectResult" /> with validation errors for <typeparamref name="T" /> into
        /// <see cref="ActionExecutingContext.Result" /> before the action executes if <typeparamref name="T" />
        /// is invalid.
        /// </summary>
        /// <param name="context">
        /// The <see cref="ActionExecutingContext" /> context for action filters with information about controller
        /// and action.
        /// </param>
        public void OnActionExecuting(ActionExecutingContext context)
        {
            Type modelType = typeof(T);

            var actionArgument = context.ActionArguments.FirstOrDefault(keyValuePair => keyValuePair.Value != null
                && keyValuePair.Value.GetType() == modelType);

            object? modelToValidate = actionArgument.Value;

            if (modelToValidate == null)
            {
                return;
            }

            ValidationResult result = _validator.Validate((T)modelToValidate);

            if (result.IsValid)
            {
                return;
            }

            context.Result = new BadRequestObjectResult(result.ToDictionary());
        }

        /// <inheritdoc />
        [ExcludeFromCodeCoverage]
        public void OnActionExecuted(ActionExecutedContext context)
        {
        }
    }
}