using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace GtMotive.Estimate.Microservice.FunctionalTests.Infrastructure
{
    /// <summary>
    /// Base class for HTTP integration tests against the running Docker microservice.
    /// </summary>
    [Collection(TestCollections.HttpIntegration)]
    public abstract class HttpIntegrationTestBase(HttpClientTestFixture fixture) : IAsyncLifetime
    {
        protected HttpClientTestFixture Fixture { get; } = fixture;

        protected HttpClient HttpClient => Fixture.HttpClient;

        public async Task InitializeAsync()
        {
            await Task.CompletedTask;
        }

        public async Task DisposeAsync()
        {
            await Task.CompletedTask;
        }
    }
}
