// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.ComponentModel.DataAnnotations;

namespace PulseApp.Domain.Options;

/// <summary>
/// Опции аутентификации через Keycloak
/// </summary>
public class KeycloakOptions
{
    /// <summary>
    /// Адрес realm'а Keycloak. Должен совпадать с издателем токена (iss),
    /// например https://auth.example.com/realms/pulse-app
    /// </summary>
    [Required]
    [Url]
    public string Authority { get; set; } = null!;

    /// <summary>
    /// Адрес OIDC discovery документа, если бэкенд ходит в Keycloak по внутреннему адресу
    /// (например http://keycloak:8080/realms/pulse-app/.well-known/openid-configuration).
    /// Если не задан, используется {Authority}/.well-known/openid-configuration
    /// </summary>
    public string? MetadataAddress { get; set; }

    /// <summary>
    /// Ожидаемая аудитория токена (aud) — client id API в Keycloak
    /// </summary>
    [Required]
    public string Audience { get; set; } = null!;

    /// <summary>
    /// Дополнительные допустимые издатели токена, помимо издателя из discovery документа
    /// </summary>
    public string[] ValidIssuers { get; set; } = [];

    /// <summary>
    /// Требовать HTTPS при получении метаданных. Отключать только для локальной разработки
    /// </summary>
    public bool RequireHttpsMetadata { get; set; } = true;

    /// <summary>
    /// Клейм с именем пользователя
    /// </summary>
    [Required]
    public string NameClaimType { get; set; } = "preferred_username";

    /// <summary>
    /// Допустимое расхождение часов при проверке времени жизни токена
    /// </summary>
    public TimeSpan ClockSkew { get; set; } = TimeSpan.FromMinutes(1);

    /// <summary>
    /// Роль администратора системы: роль realm'а или роль клиента <see cref="Audience"/>
    /// </summary>
    [Required]
    public string AdminRole { get; set; } = "admin";

    /// <summary>
    /// Публичный клиент Keycloak для авторизации из Swagger UI
    /// </summary>
    [Required]
    public string SwaggerClientId { get; set; } = "swagger";
}
