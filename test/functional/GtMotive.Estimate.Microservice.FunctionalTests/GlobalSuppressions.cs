// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using System.Diagnostics.CodeAnalysis;

[assembly: SuppressMessage("Maintainability", "CA1515:Consider making public types internal", Justification = "For avoid xUnit1027.", Scope = "type", Target = "~T:GtMotive.Estimate.Microservice.FunctionalTests.Infrastructure.CompositionRootCollectionFixture")]
[assembly: SuppressMessage("Maintainability", "CA1515:Consider making public types internal", Justification = "Required for XUnit test discovery.", Scope = "type", Target = "~T:GtMotive.Estimate.Microservice.FunctionalTests.Infrastructure.HttpIntegrationFixture")]
[assembly: SuppressMessage("Maintainability", "CA1515:Consider making public types internal", Justification = "Required for XUnit test discovery.", Scope = "type", Target = "~T:GtMotive.Estimate.Microservice.FunctionalTests.Infrastructure.HttpClientTestFixture")]
[assembly: SuppressMessage("Maintainability", "CA1515:Consider making public types internal", Justification = "Required for XUnit test discovery.", Scope = "type", Target = "~T:GtMotive.Estimate.Microservice.FunctionalTests.Infrastructure.HttpIntegrationTestBase")]
[assembly: SuppressMessage("Naming", "CA1707:Identifiers should not contain underscores", Justification = "XUnit test naming convention for test methods.")]
