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
            .Configure<DatabaseOptions>(configuration.GetSection(nameof(DatabaseOptions)));

        services.AddOptions<VapidOptions>()
            .Bind(configuration.GetSection(nameof(VapidOptions)))
            .ValidateDataAnnotations();

        services.AddOptions<KeycloakOptions>()
            .Bind(configuration.GetSection(nameof(KeycloakOptions)))
            .ValidateDataAnnotations()
            .ValidateOnStart();

        return services;
    }
}
