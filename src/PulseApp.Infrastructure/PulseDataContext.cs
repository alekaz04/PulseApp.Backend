// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.EntityFrameworkCore;
using PulseApp.Domain.Entities;

namespace PulseApp.Infrastructure;

/// <summary>
/// Контекст доступа к базе данных
/// </summary>
public class PulseDataContext : DbContext
{
    public PulseDataContext(DbContextOptions<PulseDataContext> options) : base(options)
    {

    }

    /// <summary>
    /// Комплименты
    /// </summary>
    public DbSet<Compliments> Compliments { get; set; } = null!;

    /// <summary>
    /// Подписки на push-уведомления
    /// </summary>
    public DbSet<PushSubscription> PushSubscriptions { get; set; } = null!;

    /// <summary>
    /// Настройки приложения
    /// </summary>
    public DbSet<AppSetting> AppSettings { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(PulseDataContext).Assembly);
    }
}
