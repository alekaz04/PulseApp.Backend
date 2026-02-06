// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PulseApp.Domain.Entities;

namespace PulseApp.Infrastructure.EntityConfigurations;

/// <summary>
/// Конфигурация сущности AppSetting для EF Core
/// </summary>
public class AppSettingConfiguration : IEntityTypeConfiguration<AppSetting>
{
    public void Configure(EntityTypeBuilder<AppSetting> builder)
    {
        builder.ToTable("app_settings");

        builder.HasKey(x => x.Key);

        builder.Property(x => x.Key)
            .HasColumnName("key")
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(x => x.Value)
            .HasColumnName("value")
            .IsRequired()
            .HasMaxLength(2000);

        builder.Property(x => x.CreatedAt)
            .HasColumnName("created_at")
            .IsRequired();

        builder.Property(x => x.UpdatedAt)
            .HasColumnName("updated_at")
            .IsRequired();
    }
}
