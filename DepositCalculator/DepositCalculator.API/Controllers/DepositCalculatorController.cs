using System;
using System.Net.Mime;
using System.Threading.Tasks;
using DepositCalculator.API.Filters;
using DepositCalculator.API.Interfaces;
using DepositCalculator.API.Utils;
using DepositCalculator.BLL.Interfaces;
using DepositCalculator.Shared.Models;
using DepositCalculator.Shared.Utils;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DepositCalculator.API.Controllers
{
    /// <summary>
    /// A controller for handling operations on deposit.
    /// </summary>
    [ApiController]
    [Route(DepositCalculatorControllerRoutes.ControllerRoute)]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public class DepositCalculatorController : ControllerBase
    {
        private readonly IDepositCalculatorOperationsService _depositOperationsService;

        /// <summary>
        /// Initializes a new instance of the <see cref="DepositCalculatorController" /> class using
        /// <see cref="IDepositCalculatorOperationsService" />.
        /// </summary>
        /// <param name="depositOperationsService">
        /// The <see cref="IDepositCalculatorOperationsService" /> for mapping between DTOs and response models.
        /// </param>
        public DepositCalculatorController(
            IDepositCalculatorOperationsService depositOperationsService)
        {
            _depositOperationsService = depositOperationsService;
        }

        /// <summary>
        /// Calculates deposit based on <paramref name="depositInfoRequest" />.
        /// </summary>
        /// <param name="depositInfoRequest">The <see cref="DepositInfoRequestModel" /> to provide data for calculation.</param>
        /// <returns>The <see cref="DepositCalculationResponseModel" /> depending on the calculation method.</returns>
        /// <response code="200">Returns <see cref="DepositCalculationResponseModel" /> depending on the calculation method.</response>
        /// <response code="400">When <paramref name="depositInfoRequest" /> has invalid properties.</response>
        /// <response code="404">
        /// When <see cref="IDepositCalculationStrategy" /> was not found in specific
        /// <see cref="IDepositCalculationStrategyResolver" />.
        /// </response>
        [HttpPost(DepositCalculatorControllerRoutes.DefaultActionRoute)]
        [CalculationStrategyExceptionFilter]
        [TypeFilter(typeof(ModelValidationActionFilterAttribute<DepositInfoRequestModel>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CalculateAsync(DepositInfoRequestModel depositInfoRequest)
        {
            DepositCalculationResponseModel calculation = await _depositOperationsService.CalculateAsync(depositInfoRequest);

            return Ok(calculation);
        }

        /// <summary>
        /// Gets paginated deposit calculations based on <paramref name="paginationRequest" />.
        /// </summary>
        /// <param name="paginationRequest">
        /// The <see cref="PaginationRequestModel" /> to provide number of items and requested page.
        /// </param>
        /// <returns>Paginated <see cref="CalculationDetailsResponseModel" /> by requested page.</returns>
        /// <response code="200">Returns paginated <see cref="CalculationDetailsResponseModel" /> by requested page.</response>
        /// <response code="400">When the <paramref name="paginationRequest" /> has invalid properties.</response>
        /// <response code="404">When there are not deposit calculations by requested page.</response>
        [EnableCors(PolicyNames.SpecificPaginationResponseHeaders)]
        [HttpGet(DepositCalculatorControllerRoutes.CalculationsRoute)]
        [TypeFilter(typeof(ModelValidationActionFilterAttribute<PaginationRequestModel>))]
        public async Task<IActionResult> GetPaginatedCalculationsAsync([FromQuery] PaginationRequestModel paginationRequest)
        {
            PaginatedResponseModel<CalculationDetailsResponseModel> paginatedCalculations = await _depositOperationsService
                .GetPaginatedCalculationsAsync(paginationRequest);

            if (paginatedCalculations.items.Count == 0)
            {
                return NotFound();
            }

            Response.Headers.Add(ResponseHeaderConstants.TotalCount, paginatedCalculations.totalCount.ToString());

            return Ok(paginatedCalculations.items);
        }

        /// <summary>
        /// Gets a specific calculation based on <paramref name="calculationId" />.
        /// </summary>
        /// <param name="calculationId">The <see cref="Guid" /> of existed <see cref="DepositCalculationResponseModel" />.</param>
        /// <returns>The <see cref="DepositCalculationResponseModel" /> by its <see cref="Guid" />.</returns>
        /// <response code="200">Returns <see cref="DepositCalculationResponseModel" /> by its <see cref="Guid" />.</response>
        /// <response code="404">When there are not <see cref="DepositCalculationResponseModel" /> by its <see cref="Guid" />.</response>
        [HttpGet(DepositCalculatorControllerRoutes.CalculationByIdRoute)]
        public async Task<IActionResult> GetCalculationByIdAsync([FromRoute] Guid calculationId)
        {
            DepositCalculationResponseModel? calculation = await _depositOperationsService.GetCalculationByIdAsync(calculationId);

            if (calculation is null)
            {
                return NotFound();
            }

            return Ok(calculation);
        }
    }
}