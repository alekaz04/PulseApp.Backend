// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PulseApp.Domain.Entities;

namespace PulseApp.Infrastructure.EntityConfigurations;

/// <summary>
/// Конфигурация сущности PushSubscription для EF Core
/// </summary>
public class PushSubscriptionConfiguration : IEntityTypeConfiguration<PushSubscription>
{
    public void Configure(EntityTypeBuilder<PushSubscription> builder)
    {
        builder.ToTable("push_subscriptions");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasColumnName("id")
            .IsRequired();

        builder.Property(x => x.Endpoint)
            .HasColumnName("endpoint")
            .IsRequired()
            .HasMaxLength(512);

        builder.HasIndex(x => x.Endpoint)
            .IsUnique();

        builder.Property(x => x.P256dh)
            .HasColumnName("p256dh")
            .IsRequired()
            .HasMaxLength(256);

        builder.Property(x => x.Auth)
            .HasColumnName("auth")
            .IsRequired()
            .HasMaxLength(256);

        builder.Property(x => x.CreatedAt)
            .HasColumnName("created_at")
            .IsRequired();

        builder.Property(x => x.UserAgent)
            .HasColumnName("user_agent")
            .HasMaxLength(512);

        builder.Property(x => x.IsActive)
            .HasColumnName("is_active")
            .IsRequired()
            .HasDefaultValue(true);
    }
}
