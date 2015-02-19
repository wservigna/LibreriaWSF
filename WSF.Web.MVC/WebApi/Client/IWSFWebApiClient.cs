using System;
using System.Threading.Tasks;

namespace WSF.WebApi.Client
{
    /// <summary>
    /// Used to make requests to WSF based Web APIs.
    /// </summary>
    public interface IWSFWebApiClient
    {
        /// <summary>
        /// Base URL for all request. 
        /// </summary>
        string BaseUrl { get; set; }

        /// <summary>
        /// Timeout value for all requests (used if not supplied in the request method).
        /// Default: 90 seconds.
        /// </summary>
        TimeSpan Timeout { get; set; }

        /// <summary>
        /// Makes post request that does not get or return value.
        /// </summary>
        /// <param name="url">Url</param>
        /// <param name="timeout">Timeout as milliseconds</param>
        Task PostAsync(string url, int? timeout = null);

        /// <summary>
        /// Makes post request that gets input but does not return value.
        /// </summary>
        /// <param name="url">Url</param>
        /// <param name="input">Input</param>
        /// <param name="timeout">Timeout as milliseconds</param>
        Task PostAsync(string url, object input, int? timeout = null);

        /// <summary>
        /// Makes post request that does not get input but returns value.
        /// </summary>
        /// <param name="url">Url</param>
        /// <param name="timeout">Timeout as milliseconds</param>
        Task<TResult> PostAsync<TResult>(string url, int? timeout = null) where TResult : class, new();

        /// <summary>
        /// Makes post request that gets input and returns value.
        /// </summary>
        /// <param name="url">Url</param>
        /// <param name="input">Input</param>
        /// <param name="timeout">Timeout as milliseconds</param>
        Task<TResult> PostAsync<TResult>(string url, object input, int? timeout = null) where TResult : class, new();
    }
}