using System;
using DepositCalculator.BLL.Exceptions;
using DepositCalculator.Shared.Errors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace DepositCalculator.API.Filters
{
    /// <summary>
    /// An exception filter to handle <see cref="StrategyException" /> while executing the action.
    /// </summary>
    public class CalculationStrategyExceptionFilterAttribute : ExceptionFilterAttribute
    {
        /// <summary>
        /// Sets <see cref="JsonResult" /> with <see cref="StrategyException.StatusCode" />
        /// into <see cref="ExceptionContext.Result" /> if <see cref="StrategyException" />
        /// was thrown while executing the action.
        /// </summary>
        /// <param name="context">
        /// The <see cref="ExceptionContext" /> provides context with <see cref="Exception" />
        /// caught while executing the action.
        /// </param>
        public override void OnException(ExceptionContext context)
        {
            if (context.Exception is not StrategyException)
            {
                return;
            }

            var strategyException = (StrategyException)context.Exception;

            var error = new CalculationStrategyError(context.Exception.Message);

            context.Result = new JsonResult(error)
            {
                StatusCode = strategyException.StatusCode
            };
        }
    }
}