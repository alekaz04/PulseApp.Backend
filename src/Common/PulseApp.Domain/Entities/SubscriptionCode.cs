// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using PulseApp.Domain.Entities;

namespace PulseApp.Domain.Entities;

public class SubscriptionCode
{
    public Guid Id { get; set; }
    public string Code { get; set; } = null!;
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset ExpireAt { get; set; }
    public Guid CreatedCodeUserId { get; set; }

    public bool IsUsed { get; set; }

    public User CreatedCodeUser { get; set; } = null!;

}
