using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PulseApp.Domain.Entities;

namespace PulseApp.Infrastructure.EntityConfigurations;

/// <summary>
/// Конфигурация сущности PushSubscription для EF Core
/// </summary>
public class PushSubscriptionConfiguration : IEntityTypeConfiguration<Subscription>
{
    public void Configure(EntityTypeBuilder<Subscription> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .IsRequired();

        builder.Property(x => x.Endpoint)
            .IsRequired()
            .HasMaxLength(512);

        builder.HasIndex(x => x.Endpoint)
            .IsUnique();

        builder.Property(x => x.P256dh)
            .IsRequired()
            .HasMaxLength(256);

        builder.Property(x => x.Auth)
            .IsRequired()
            .HasMaxLength(256);

        builder.Property(x => x.CreatedAt)
            .IsRequired();

        builder.Property(x => x.UserAgent)
            .HasMaxLength(512);

        builder.Property(x => x.IsActive)
            .IsRequired()
            .HasDefaultValue(true);
    }
}
