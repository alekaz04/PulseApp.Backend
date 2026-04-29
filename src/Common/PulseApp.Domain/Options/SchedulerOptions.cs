// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace PulseApp.Domain.Options;

/// <summary>
/// Опции задач hangfire
/// </summary>
public class SchedulerOptions
{
    /// <summary>
    /// Cron выражения для задачи раздачи комплиментов
    /// </summary>
    public string ComplimentCronExpression { get; set; } = "26 8-22 * * *";
}
