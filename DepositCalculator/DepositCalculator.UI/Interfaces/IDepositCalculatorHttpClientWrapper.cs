using System.Net.Http;
using System.Threading.Tasks;

namespace DepositCalculator.UI.Interfaces
{
    /// <summary>
    /// A contract for a wrapper around the <see cref="HttpClient" />.
    /// </summary>
    public interface IDepositCalculatorHttpClientWrapper
    {
        /// <summary>
        /// Sends an HTTP GET request asynchronously to the specified <paramref name="requestUri" />.
        /// </summary>
        /// <param name="requestUri">The Uri of the resource to request.</param>
        /// <returns>
        /// A task that represents the asynchronous get operation.
        /// The result is <see cref="HttpResponseMessage" />.
        /// </returns>
        Task<HttpResponseMessage> GetAsync(string requestUri);

        /// <summary>
        /// Sends an HTTP POST request asynchronously to the specified <paramref name="requestUri"/>
        /// with the <paramref name="data" /> serialized as JSON in the request body.
        /// </summary>
        /// <param name="requestUri">The Uri of the resource to request.</param>
        /// <param name="data">The data to be sent in the request body.</param>
        /// <returns>
        /// A task that represents the asynchronous post operation.
        /// The result is <see cref="HttpResponseMessage" />.
        /// </returns>
        Task<HttpResponseMessage> PostAsync(string requestUri, object data);
    }
}