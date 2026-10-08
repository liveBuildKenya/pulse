using FluentMigrator.Builders.Create.Table;
using FluentMigrator.Runner;
using Microsoft.Extensions.DependencyInjection;

namespace Pulse.WebAuthn.Domain.Infrastructure.Migrations
{
    /// <summary>
    /// Represents migration extensions
    /// </summary>
    public static class MigrationExtensions
    {
        public static ICreateTableColumnOptionOrWithColumnSyntax WithByteIdColumn(this ICreateTableWithColumnSyntax createTableWithColumnSyntax)
        {
            return createTableWithColumnSyntax
                .WithColumn("Id")
                    .AsBinary()
                    .NotNullable()
                    .PrimaryKey();
        }

        public static ICreateTableColumnOptionOrWithColumnSyntax WithTimestamp(this ICreateTableWithColumnSyntax createTableWithColumnSyntax)
        {
            return createTableWithColumnSyntax
                .WithColumn("DateCreated")
                    .AsDateTime2()
                    .NotNullable()
                .WithColumn("DateUpdated")
                    .AsDateTime2()
                    .NotNullable();
        }

        public static void RunMigrations(IServiceProvider serviceProvider)
        {
            var runner = serviceProvider.GetRequiredService<IMigrationRunner>();

            runner.MigrateUp();
        }
    }
}
