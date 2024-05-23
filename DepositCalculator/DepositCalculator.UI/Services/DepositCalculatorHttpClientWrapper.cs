using System;
using System.Diagnostics.CodeAnalysis;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using DepositCalculator.UI.Interfaces;
using DepositCalculator.UI.Utils;
using Microsoft.Extensions.Options;

namespace DepositCalculator.UI.Services
{
    /// <inheritdoc cref="IDepositCalculatorHttpClientWrapper" />
    [ExcludeFromCodeCoverage]
    public class DepositCalculatorHttpClientWrapper : IDepositCalculatorHttpClientWrapper
    {
        private readonly HttpClient _httpClient;

        private readonly DepositCalculatorApiOptions _depositCalculatorApiOptions;

        /// <summary>
        /// Initializes a new instance of the <see cref="DepositCalculatorHttpClientWrapper" /> class
        /// using <see cref="HttpClient" /> and <see cref="IOptions{DepositCalculatorApiOptions}>" /> of
        /// <see cref="DepositCalculatorApiOptions" />.
        /// </summary>
        /// <param name="httpClient">The <see cref="HttpClient" /> used for making requests.</param>
        /// <param name="depositCalculatorApiOptions">
        /// The <see cref="IOptions{DepositCalculatorApiOptions}>" /> parametrized by
        /// <see cref="DepositCalculatorApiOptions" /> of DepositCalculator Api to send requests to.
        /// </param>
        public DepositCalculatorHttpClientWrapper(
            HttpClient httpClient,
            IOptions<DepositCalculatorApiOptions> depositCalculatorApiOptions)
        {
            _httpClient = httpClient;
            _depositCalculatorApiOptions = depositCalculatorApiOptions.Value;

            _httpClient.BaseAddress = new Uri(_depositCalculatorApiOptions.BaseUri!);
        }

        /// <inheritdoc />
        public Task<HttpResponseMessage> GetAsync(string requestUri)
        {
            return _httpClient.GetAsync(requestUri);
        }

        /// <inheritdoc />
        public Task<HttpResponseMessage> PostAsync(string requestUri, object data)
        {
            return _httpClient.PostAsJsonAsync(requestUri, data);
        }
    }
}