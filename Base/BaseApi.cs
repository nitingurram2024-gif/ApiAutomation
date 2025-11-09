using RestSharp;
using ApiAutomation.Config;
using System;

namespace ApiAutomation.Base
{
    /// <summary>
    /// BaseApi class provides a shared RestSharp client and a helper method to execute HTTP requests.
    /// All endpoint or service classes should inherit from this base class.
    /// </summary>
    public class BaseApi
    {
        /// <summary>
        /// RestSharp client used for sending HTTP requests.
        /// Initialized with the base URL from config or environment variable.
        /// </summary>
        protected readonly RestClient client;

        /// <summary>
        /// Constructor initializes the RestClient.
        /// Base URL is resolved from:
        /// 1. Environment variable 'BASE_URL' (if provided)
        /// 2. Config/appsettings.json 'BaseUrl'
        /// 3. Default fallback: https://api.restful-api.dev
        /// Throws exception if no valid URL is found.
        /// </summary>
        public BaseApi()
        {
            // Get the BaseUrl from environment variable or appsettings.json
            var baseUrl = ConfigReader.Get("BaseUrl", "BASE_URL") ?? "https://api.restful-api.dev";

            // Validate that the URL is not empty
            if (string.IsNullOrWhiteSpace(baseUrl))
                throw new InvalidOperationException(
                    "BaseUrl is not configured. Set Config/appsettings.json or BASE_URL environment variable.");

            // Initialize RestSharp client with the resolved base URL
            client = new RestClient(baseUrl);
        }

        /// <summary>
        /// Executes an HTTP request for the given resource and method.
        /// Optionally includes a JSON body for POST, PUT, PATCH requests.
        /// </summary>
        /// <param name="resource">API endpoint/resource (e.g., "users/1")</param>
        /// <param name="method">HTTP method (GET, POST, PUT, PATCH, DELETE)</param>
        /// <param name="body">Optional request payload (will be serialized as JSON)</param>
        /// <returns>IRestResponse returned by the API call</returns>
        protected IRestResponse Execute(string resource, Method method, object body = null)
        {
            // Create a new request with the specified resource and method
            var request = new RestRequest(resource, method);

            // If a request body is provided, serialize it as JSON and attach
            if (body != null)
            {
                request.AddJsonBody(body);
            }

            // Execute the request and return the response
            return client.Execute(request);
        }
    }
}
