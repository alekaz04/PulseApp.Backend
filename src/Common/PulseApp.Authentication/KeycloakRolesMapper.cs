// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Security.Claims;
using System.Text.Json;

namespace PulseApp.Authentication;

/// <summary>
/// Перенос ролей из вложенных клеймов Keycloak в плоские клеймы ролей,
/// чтобы работали RequireRole и [Authorize(Roles = ...)]
/// </summary>
internal static class KeycloakRolesMapper
{
    /// <summary>
    /// Добавить в <paramref name="identity"/> роли realm'а и роли клиента <paramref name="clientId"/>
    /// </summary>
    public static void MapRoles(ClaimsIdentity identity, string clientId)
    {
        var roles = new HashSet<string>(StringComparer.Ordinal);

        if (TryParseObject(identity.FindFirst(KeycloakClaimTypes.RealmAccess), out var realmAccess))
        {
            ReadRoles(realmAccess, roles);
        }

        if (TryParseObject(identity.FindFirst(KeycloakClaimTypes.ResourceAccess), out var resourceAccess)
            && resourceAccess.TryGetProperty(clientId, out var clientAccess))
        {
            ReadRoles(clientAccess, roles);
        }

        foreach (string role in roles)
        {
            identity.AddClaim(new Claim(identity.RoleClaimType, role));
        }
    }

    /// <summary>
    /// Разобрать JSON значение клейма
    /// </summary>
    private static bool TryParseObject(Claim? claim, out JsonElement element)
    {
        element = default;
        if (claim is null)
        {
            return false;
        }

        try
        {
            using var document = JsonDocument.Parse(claim.Value);
            element = document.RootElement.Clone();
            return element.ValueKind == JsonValueKind.Object;
        }
        catch (JsonException)
        {
            return false;
        }
    }

    /// <summary>
    /// Прочитать массив roles из объекта доступа
    /// </summary>
    private static void ReadRoles(JsonElement access, HashSet<string> roles)
    {
        if (access.ValueKind != JsonValueKind.Object
            || !access.TryGetProperty("roles", out var array)
            || array.ValueKind != JsonValueKind.Array)
        {
            return;
        }

        foreach (var item in array.EnumerateArray())
        {
            if (item.ValueKind == JsonValueKind.String && item.GetString() is { Length: > 0 } role)
            {
                roles.Add(role);
            }
        }
    }
}
