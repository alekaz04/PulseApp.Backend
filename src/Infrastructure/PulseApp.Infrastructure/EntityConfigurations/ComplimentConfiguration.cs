// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PulseApp.Domain.Entities;

namespace PulseApp.Infrastructure.EntityConfigurations;

/// <summary>
/// Конфигурация сущности Compliments для EF Core
/// </summary>
public class ComplimentConfiguration : IEntityTypeConfiguration<Compliment>
{
    public void Configure(EntityTypeBuilder<Compliment> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .IsRequired();

        builder.Property(x => x.Text)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(x => x.Title)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(x => x.IsBeenPushed)
            .IsRequired()
            .HasDefaultValue(false);

        builder.Property(x => x.IsDeleted)
            .IsRequired()
            .HasDefaultValue(false);

        builder.Property(x => x.CreatedAt)
            .IsRequired();

        builder.Property(x => x.UpdatedAt)
            .IsRequired();

        builder.HasIndex(x => x.IsBeenPushed);
    }
}
