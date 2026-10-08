using FluentMigrator;
using Pulse.WebAuthn.Domain.Credentials;
using Pulse.WebAuthn.Domain.Customers;
using Pulse.WebAuthn.Domain.Infrastructure.Migrations;

namespace Pulse.WebAuthn.Domain.Migrations
{
    [Migration(202607280318, "Initial Migration")]
    public class InitialMigration : Migration
    {
        public override void Up()
        {
            Create.Table(nameof(Customer))
                .WithByteIdColumn()
                .WithColumn(nameof(Customer.Name))
                    .AsString()
                    .Unique()
                    .NotNullable()
                .WithColumn(nameof(Customer.DisplayName))
                    .AsString()
                    .NotNullable()
                .WithTimestamp();

            Create.Table(nameof(Credential))
                .WithByteIdColumn()
                .WithColumn(nameof(Credential.PublicKey))
                    .AsBinary()
                .WithColumn(nameof(Credential.SignCount))
                    .AsInt64()
                .WithColumn(nameof(Credential.Transports))
                    .AsString()
                .WithColumn(nameof(Credential.IsBackupEligible))
                    .AsBoolean()
                .WithColumn(nameof(Credential.IsBackedUp))
                    .AsBoolean()
                .WithColumn(nameof(Credential.AttestationObject))
                    .AsBinary()
                .WithColumn(nameof(Credential.AttestationClientDataJson))
                    .AsBinary()
                .WithColumn(nameof(Credential.CustomerId))
                    .AsBinary()
                .WithColumn(nameof(Credential.UserHandle))
                    .AsBinary()
                .WithColumn(nameof(Credential.AttestationFormat))
                    .AsString()
                .WithColumn(nameof(Credential.RegDate))
                    .AsDateTime2()
                .WithColumn(nameof(Credential.AaGuid))
                    .AsGuid()
                .WithTimestamp();

            Create.ForeignKey()
                .FromTable(nameof(Credential)).ForeignColumn(nameof(Credential.CustomerId))
                .ToTable(nameof(Customer)).PrimaryColumn(nameof(Customer.Id));

        }

        public override void Down()
        {
            Delete.ForeignKey()
                .FromTable(nameof(Customer));
            Delete.Table(nameof(Customer))
                .IfExists();
            Delete.Table(nameof(Credential))
                .IfExists();
        }
    }
}
