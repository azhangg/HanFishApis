using IdentityModel;
using IdentityServer4.Models;
using Microsoft.OpenApi.Writers;
using static IdentityModel.OidcConstants;

namespace HanFishApis.Infrastructure.Ids4
{
    public class Config
    {
        public static string Secret => "ZTccSi9dZbvvbEIt8X2ytOkYBAGPct5OctyXMf9P";

        public static IEnumerable<ApiResource> apiResoure =>
        new List<ApiResource>
        {
            new ApiResource
            {
                Name = "HanFish",
                UserClaims = new string[]
                {
                    JwtClaimTypes.Id,
                    JwtClaimTypes.Name,
                    JwtClaimTypes.Role
                },
                Scopes = { "client","backend" }
            }
        };

        public static IEnumerable<ApiScope> apiScopes =>
        new List<ApiScope>
        {
            new ApiScope("client"),
            new ApiScope("backend")
        };

        public static IEnumerable<Client> Clients =>
        new List<Client>
        {
            new Client
            {
                ClientId = "client",
                AllowedGrantTypes = { GrantType.ResourceOwnerPassword },
                AccessTokenLifetime = 3600,
                ClientSecrets =
                {
                    new Secret(Secret.ToSha256())
                },
                AllowOfflineAccess = true,
                AllowedScopes = { 
                    "client", 
                    StandardScopes.OfflineAccess 
                }
            },
            new Client
            {
                ClientId = "backend",
                AllowedGrantTypes = { GrantType.ResourceOwnerPassword },
                AccessTokenLifetime = 3600,
                ClientSecrets =
                {
                    new Secret(Secret.ToSha256())
                },
                 AllowOfflineAccess = true,
                AllowedScopes = {
                    "backend", 
                    StandardScopes.OfflineAccess 
                }
            }
        };
    }
}
