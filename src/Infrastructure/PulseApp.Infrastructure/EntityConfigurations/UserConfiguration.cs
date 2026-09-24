using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PulseApp.Domain.Entities;

namespace PulseApp.Infrastructure.EntityConfigurations;

/// <summary>
/// Конфигурация сущности User для EF Core
/// </summary>
public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .IsRequired();

        builder.Property(x => x.KeycloakId)
            .IsRequired()
            .HasMaxLength(255);

        builder.HasIndex(x => x.KeycloakId)
            .IsUnique();

        // Уникальность email обеспечивает Keycloak, у части пользователей email может отсутствовать
        builder.Property(x => x.Email)
            .HasMaxLength(256);

        builder.Property(x => x.DisplayName)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(x => x.CreatedAt)
            .IsRequired();
    }
}
