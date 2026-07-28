using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pulse.WebAuthn.Domain.Credentials;

namespace Pulse.WebAuthn.Domain.Customers
{
    /// <summary>
    /// Represents the mapping configuration for a customer.
    /// </summary>
    public class CustomerMap : IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> builder)
        {
            builder.ToTable(nameof(Customer));

            builder.HasKey(customer => customer.Id);

            builder.HasIndex(customer => customer.Name)
                .IsUnique();

            builder.HasMany<Credential>()
                .WithOne()
                .HasForeignKey(credential => credential.CustomerId);
        }
    }
}
