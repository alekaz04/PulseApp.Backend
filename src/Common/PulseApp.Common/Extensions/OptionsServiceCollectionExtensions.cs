// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PulseApp.Domain.Options;

namespace PulseApp.Common.Extensions;

public static class OptionsServiceCollectionExtensions
{
    public static IServiceCollection AddPulseOptions(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .Configure<VapidOptions>(configuration.GetSection(nameof(VapidOptions)))
            .Configure<DatabaseOptions>(configuration.GetSection(nameof(DatabaseOptions)))
            .Configure<ApiKeyOption>(configuration.GetSection(nameof(ApiKeyOption)))
            .Configure<SchedulerOptions>(configuration.GetSection(nameof(SchedulerOptions)));

        return services;
    }
}
