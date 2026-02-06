// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.EntityFrameworkCore;
using PulseApp.Application.Interfaces;
using PulseApp.Application.Models;
using PulseApp.Domain.Entities;
using WebPush;

namespace PulseApp.Infrastructure.Services;

/// <summary>
/// Реализация сервиса для работы с VAPID ключами
/// </summary>
public class VapidService : IVapidService
{
    private readonly PulseDataContext _context;
    private const string PUBLIC_KEY = "VapidPublicKey";
    private const string PRIVATE_KEY = "VapidPrivateKey";

    public VapidService(PulseDataContext context)
    {
        _context = context;
    }

    public async Task<VapidKeys> GetOrGenerateVapidKeysAsync()
    {
        AppSetting? publicKeySetting = await _context.AppSettings
            .FirstOrDefaultAsync(x => x.Key == PUBLIC_KEY);

        AppSetting? privateKeySetting = await _context.AppSettings
            .FirstOrDefaultAsync(x => x.Key == PRIVATE_KEY);

        if (publicKeySetting == null || privateKeySetting == null)
        {
            // Генерируем новые VAPID ключи
            VapidDetails vapidDetails = VapidHelper.GenerateVapidKeys();

            DateTimeOffset now = DateTimeOffset.UtcNow;

            publicKeySetting = new AppSetting
            {
                Key = PUBLIC_KEY,
                Value = vapidDetails.PublicKey,
                CreatedAt = now,
                UpdatedAt = now
            };

            privateKeySetting = new AppSetting
            {
                Key = PRIVATE_KEY,
                Value = vapidDetails.PrivateKey,
                CreatedAt = now,
                UpdatedAt = now
            };

            _context.AppSettings.Add(publicKeySetting);
            _context.AppSettings.Add(privateKeySetting);
            await _context.SaveChangesAsync();

            return new VapidKeys
            {
                PublicKey = vapidDetails.PublicKey,
                PrivateKey = vapidDetails.PrivateKey
            };
        }

        return new VapidKeys
        {
            PublicKey = publicKeySetting.Value,
            PrivateKey = privateKeySetting.Value
        };
    }

    public async Task<string> GetPublicKeyAsync()
    {
        VapidKeys keys = await GetOrGenerateVapidKeysAsync();
        return keys.PublicKey;
    }
}
