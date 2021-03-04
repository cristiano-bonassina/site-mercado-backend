using System;
using System.Collections.Generic;
using IdentityServer4;
using IdentityServer4.Models;

namespace LogicArt.SiteMercado.Presentation.Identity
{
    public static class IdentityConfiguration
    {
        public static IEnumerable<IdentityResource> IdentityResources =>
            new[]
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResource("api", "Api", new[] {"api"})
            };

        public static IEnumerable<ApiResource> ApiResources =>
            new[]
            {
                new ApiResource
                {
                    Name = "api",
                    DisplayName = "API",
                    ApiSecrets = new List<Secret> 
                    {
                        new("secret".Sha256())
                    },
                    UserClaims = new[] {"api"}
                }
            };

        public static IEnumerable<Client> Clients =>
            new[]
            {
                new Client
                {
                    AccessTokenLifetime = (int)TimeSpan.FromDays(180).TotalSeconds,
                    AllowOfflineAccess = true,
                    AllowedGrantTypes = new List<string> {"sitemercado-password"},
                    AllowedScopes =
                        new List<string>
                        {
                            "api",
                            IdentityServerConstants.StandardScopes.OpenId,
                            IdentityServerConstants.StandardScopes.OfflineAccess
                        },
                    ClientName = "Web",
                    ClientId = "c46afe5579564d81b755e3358622bddc",
                    ClientSecrets = new List<Secret>
                    {
                        new("KXG8aK/jkoWlsiV9rxQeydotiWJGOqYh/wSdhGV7fjs=".Sha256())
                    },
                    RequireClientSecret = true,
                    RefreshTokenUsage = TokenUsage.ReUse
                }
            };
    }
}
