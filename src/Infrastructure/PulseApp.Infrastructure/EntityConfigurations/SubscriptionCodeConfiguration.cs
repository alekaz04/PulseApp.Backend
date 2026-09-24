// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PulseApp.Domain.Entities;

namespace PulseApp.Infrastructure.EntityConfigurations;

/// <summary>
/// Конфигурация сущности SubscriptionCode для EF Core
/// </summary>
public class SubscriptionCodeConfiguration : IEntityTypeConfiguration<SubscriptionCode>
{
    public void Configure(EntityTypeBuilder<SubscriptionCode> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .IsRequired();

        builder.Property(x => x.Code)
            .IsRequired()
            .HasMaxLength(64);

        builder.HasIndex(x => x.Code)
            .IsUnique();

        builder.Property(x => x.CreatedAt)
            .IsRequired();

        builder.Property(x => x.ExpireAt)
            .IsRequired();

        builder.Property(x => x.CreatedCodeUserId)
            .IsRequired();

        builder.HasOne(x => x.CreatedCodeUser)
            .WithMany()
            .HasForeignKey(x => x.CreatedCodeUserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
