using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Pulse.WebAuthn.Domain.Credentials.Services;
using Pulse.WebAuthn.Domain.Customers.Services;
using Pulse.WebAuthn.Domain.Infrastructure.Data;

namespace Pulse.WebAuthn.Domain.Infrastructure
{
    /// <summary>
    /// Represents the dependency injection container for the MagicAuth domain.
    /// </summary>
    public static class DependencyInjection
    {
        public static void AddMagicAuthDomainServices(this IServiceCollection serviceCollection, IConfiguration configuration)
        {
            serviceCollection.AddScoped(typeof(IRepository<>), typeof(EntityRepository<>));

            //serviceCollection.AddDbContext<ILiveDataProvider, LiveDataProvider>(contextOptions => contextOptions
            //.UseNpgsql(configuration.GetConnectionString("MagicAuth")));

            serviceCollection.AddTransient<ICustomerService, CustomerService>();
            serviceCollection.AddTransient<ICredentialService, CredentialService>();
        }
    }
}
