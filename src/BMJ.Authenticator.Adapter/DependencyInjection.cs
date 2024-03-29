﻿using BMJ.Authenticator.Adapter.Authentication;
using BMJ.Authenticator.Adapter.Identity;
using BMJ.Authenticator.Application.Common.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace BMJ.Authenticator.Adapter;

public static class DependencyInjection
{
    public static IServiceCollection AddAdapterServices(this IServiceCollection services)
    {
        services.AddTransient<IIdentityAdapter, IdentityAdapter>();
        services.AddTransient<IJwtProvider, JwtProvider>();
        return services;
    }
}
