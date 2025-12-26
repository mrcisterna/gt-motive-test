#nullable enable

using System;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace GtMotive.Estimate.Microservice.FunctionalTests.Infrastructure
{
    /// <summary>
    /// HTTP Client fixture for integration tests against a running Docker instance.
    /// Uses the microservice running at http://localhost:5000/.
    /// </summary>
    public sealed class HttpClientTestFixture : IAsyncLifetime, IDisposable
    {
        private const string BaseUrl = "http://localhost:5000";
        private const int MaxRetries = 15;
        private const int RetryDelayMilliseconds = 1000;

        public HttpClientTestFixture()
        {
            HttpClient = new HttpClient
            {
                BaseAddress = new Uri(BaseUrl),
                Timeout = TimeSpan.FromSeconds(30)
            };
        }

        public HttpClient HttpClient { get; }

        public async Task InitializeAsync()
        {
            await WaitForServiceReadinessAsync();
        }

        public async Task DisposeAsync()
        {
            HttpClient?.Dispose();
            await Task.CompletedTask;
        }

        public void Dispose()
        {
            HttpClient?.Dispose();
        }

        /// <summary>
        /// Waits for the microservice to be ready by polling an actual API endpoint.
        /// </summary>
        private async Task WaitForServiceReadinessAsync()
        {
            for (var i = 0; i < MaxRetries; i++)
            {
                try
                {
                    using var response = await HttpClient.GetAsync(new Uri(BaseUrl + "/api/vehicles/available"));
                    if (response.IsSuccessStatusCode)
                    {
                        return;
                    }
                }
                catch (HttpRequestException)
                {
                    // Service not ready yet
                }

                if (i < MaxRetries - 1)
                {
                    await Task.Delay(RetryDelayMilliseconds);
                }
            }

            throw new InvalidOperationException(
                $"The microservice at {BaseUrl} did not become ready within {MaxRetries * RetryDelayMilliseconds}ms. " +
                "Ensure Docker container is running with 'docker compose up -d'");
        }
    }
}
