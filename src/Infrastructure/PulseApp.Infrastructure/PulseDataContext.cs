// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using PulseApp.Domain.Options;

namespace PulseApp.Infrastructure;

/// <summary>
/// Контекст доступа к базе данных
/// </summary>
public class PulseDataContext : DbContext
{
    public PulseDataContext(DbContextOptions<PulseDataContext> options, IOptions<DatabaseOptions> dbConfiguration) : base(options)
    {
        if (dbConfiguration.Value.Migrate)
        {
            Database.Migrate();
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(PulseDataContext).Assembly);
    }
}
