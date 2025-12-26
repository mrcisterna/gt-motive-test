using Xunit;

namespace GtMotive.Estimate.Microservice.InfrastructureTests.Infrastructure
{
    [Collection(TestCollections.TestServer)]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Maintainability", "CA1515:Considere la posibilidad de hacer que los tipos públicos sean internos", Justification = "<pendiente>")]
    public abstract class InfrastructureTestBase(GenericInfrastructureTestServerFixture fixture)
    {
        protected GenericInfrastructureTestServerFixture Fixture { get; } = fixture;
    }
}
