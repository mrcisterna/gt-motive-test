using Xunit;

namespace GtMotive.Estimate.Microservice.FunctionalTests.Infrastructure
{
    /// <summary>
    /// HTTP Integration test collection definition.
    /// This ensures that all HTTP integration tests share a single HttpClientTestFixture instance.
    /// </summary>
    [CollectionDefinition(TestCollections.HttpIntegration)]
    public class HttpIntegrationFixture : ICollectionFixture<HttpClientTestFixture>
    {
    }
}
