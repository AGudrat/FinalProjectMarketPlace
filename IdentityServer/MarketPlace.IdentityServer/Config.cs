﻿// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4;
using IdentityServer4.Models;
using System;
using System.Collections.Generic;

namespace MarketPlace.IdentityServer
{
    public static class Config
    {
        public static IEnumerable<ApiResource> ApiResources => new ApiResource[]
        {
            new ApiResource("resource_catalog"){Scopes = { "catalog_fullpermission" } },
            new ApiResource("resource_photostock"){Scopes = { "photostock_fullpermission" } },
            new ApiResource("resource_basket"){Scopes = { "basket_fullpermission" } },
            new ApiResource("resource_discount"){Scopes = { "discount_fullpermission" } },
            new ApiResource("resource_order"){Scopes = { "order_fullpermission" } },
            new ApiResource("resource_payment"){Scopes = { "payment_fullpermission" } },
            new ApiResource("resource_gateway"){Scopes = { "gateway_fullpermission" } },
            new ApiResource(IdentityServerConstants.LocalApi.ScopeName)
        };
        public static IEnumerable<IdentityResource> IdentityResources =>
                   new IdentityResource[]
                   {
                       new IdentityResources.Email(),
                       new IdentityResources.OpenId(),
                       new IdentityResources.Profile(),
                       new IdentityResource(){Name="roles",DisplayName="Roles", Description = "Kullanici Rolleri",UserClaims= new[]{"role"}},
                   };

        public static IEnumerable<ApiScope> ApiScopes =>
            new ApiScope[]
            {
                new ApiScope("catalog_fullpermission","CatalogAPI icin full erisim"),
                new ApiScope("photostock_fullpermission","Photo Stock API icin full erisim"),
                new ApiScope("basket_fullpermission","Basket API icin full erisim"),
                new ApiScope("discount_fullpermission","Basket API icin full erisim"),
                new ApiScope("order_fullpermission","Order API icin full erisim"),
                new ApiScope("payment_fullpermission","Payment API icin full erisim"),
                new ApiScope("gateway_fullpermission","Gateway API icin full erisim"),
                new ApiScope(IdentityServerConstants.LocalApi.ScopeName)
            };

        public static IEnumerable<Client> Clients =>
            new Client[]
            {
                new Client
                {
                    ClientName = "ASP.NET Core MVC",
                    ClientId = "WebMvcClient",
                    ClientSecrets={ new Secret("secret".Sha256())},
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    AllowedScopes = { "gateway_fullpermission", "catalog_fullpermission", "photostock_fullpermission",  IdentityServerConstants.LocalApi.ScopeName }

                },
                new Client
                {
                    ClientName = "ASP.NET Core MVC",
                    ClientId = "WebMvcClientForUser",
                    AllowOfflineAccess = true,
                    ClientSecrets={ new Secret("secret".Sha256())},
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                    AllowedScopes = {"gateway_fullpermission", "basket_fullpermission","order_fullpermission",  IdentityServerConstants.LocalApi.ScopeName, IdentityServerConstants.StandardScopes.Email, IdentityServerConstants.StandardScopes.OpenId,IdentityServerConstants.StandardScopes.Profile, IdentityServerConstants.StandardScopes.OfflineAccess, "roles" },
                    AccessTokenLifetime = 1*60*60,
                    RefreshTokenExpiration = TokenExpiration.Absolute,
                    AbsoluteRefreshTokenLifetime = (int)(DateTime.UtcNow.AddHours(4).AddDays(60) - DateTime.UtcNow.AddHours(4)).TotalSeconds,
                    RefreshTokenUsage = TokenUsage.ReUse
                },
                new Client
                {
                    ClientName = "Token Exchange Client",
                    ClientId = "TokenExchangeClient",
                    ClientSecrets={ new Secret("secret".Sha256())},
                    AllowedGrantTypes =new [] { "urn:ietf:params:oauth:grant-type:token-exchange" },
                    AllowedScopes = { "payment_fullpermission", "discount_fullpermission", IdentityServerConstants.StandardScopes.OpenId }
                },
            };
    }
}