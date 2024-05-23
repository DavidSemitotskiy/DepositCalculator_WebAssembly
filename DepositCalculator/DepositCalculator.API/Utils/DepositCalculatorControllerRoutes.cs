using System;
using DepositCalculator.API.Controllers;

namespace DepositCalculator.API.Utils
{
    /// <summary>
    /// The routes for <see cref="DepositCalculatorController" />.
    /// </summary>
    public static class DepositCalculatorControllerRoutes
    {
        /// <summary>
        /// The route for <see cref="DepositCalculatorController" /> itself.
        /// </summary>
        public const string ControllerRoute = "api/[controller]";

        /// <summary>
        /// The route based on action name.
        /// </summary>
        public const string DefaultActionRoute = "[action]";

        /// <summary>
        /// The base route for actions that operates with calculations.
        /// </summary>
        public const string CalculationsRoute = "Calculations";

        /// <summary>
        /// The route for <see cref="DepositCalculatorController.GetCalculationByIdAsync(Guid)" /> endpoint.
        /// </summary>
        public const string CalculationByIdRoute = $"{CalculationsRoute}/{{calculationId:guid}}";
    }
}