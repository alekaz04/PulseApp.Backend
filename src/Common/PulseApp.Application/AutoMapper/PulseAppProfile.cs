// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using AutoMapper;
using PulseApp.Application.DTOs;
using PulseApp.Domain.Entities;

namespace PulseApp.Application.AutoMapper;

public class PulseAppProfile : Profile
{
    public PulseAppProfile()
    {
        CreateMap<Compliment, ComplimentDto>()
            .ReverseMap();

        CreateMap<CreateComplimentDto, Compliment>()
            .ForMember(x => x.Id, e => e.MapFrom(x => Guid.NewGuid()))
            .ForMember(x => x.CreatedAt, e => e.MapFrom(x => DateTimeOffset.UtcNow))
            .ForMember(x => x.UpdatedAt, e => e.MapFrom(x => DateTimeOffset.UtcNow));

    }
}
