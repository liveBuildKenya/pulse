using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Pulse.WebAuthn.Application.Assertion.Services;
using Pulse.WebAuthn.Application.Attestation.Services;
using Pulse.WebAuthn.Application.Credentials;
using Pulse.WebAuthn.Application.Customers;
using Pulse.WebAuthn.Domain.Infrastructure;

namespace Pulse.WebAuthn.Application.Infrastructure
{
    /// <summary>
    /// Represents the dependency injection class
    /// </summary>
    public static class Dependencyinjection
    {
        public static void AddMagicAuthApplicationServices(this IServiceCollection serviceCollection, IConfiguration configuration)
        {
            serviceCollection.AddMagicAuthDomainServices(configuration);

            serviceCollection.AddTransient<ICustomerFactory, CustomerFactory>();
            serviceCollection.AddTransient<ICredentialFactory, CredentialFactory>();

            serviceCollection.AddTransient<IAttestationUseCase, AttestationUseCase>();
            serviceCollection.AddTransient<IAssertionUseCase, AssertionUseCase>();

            serviceCollection.AddMigrations(configuration.GetConnectionString("MagicAuth"));
        }
    }
}
