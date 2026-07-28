using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;


namespace Pulse.WebAuthn.Application.Migrations
{
    public static class MigrationRunner
    {
        public static void RunMigrations(this IApplicationBuilder applicationBuilder, string connectionString)
        {
            using var scope = applicationBuilder.ApplicationServices.CreateScope();

            //Runner.ExecuteMigrations(scope.ServiceProvider, connectionString);

        }
    }
}
