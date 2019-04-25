using System.Collections.Generic;
using System.IdentityModel.Claims;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using IdentityServer3.Core;
using IdentityServer3.Core.Models;
using IdentityServer3.WsFederation.Models;
using IdentityModel.Constants;

namespace IdentityServerDemo.IdentityServerConfig
{
    public class Config
    {
            public static X509Certificate2 GetCert()
            {
                var assembly = typeof(Config).Assembly;
                using (var stream = assembly.GetManifestResourceStream("IdentityServerDemo.IdentityServerConfig.idsrv3test.pfx"))
                {
                    return new X509Certificate2(ReadStream(stream), "idsrv3test");
                }
            }

            private static byte[] ReadStream(Stream input)
            {
                byte[] buffer = new byte[16 * 1024];
                using (MemoryStream ms = new MemoryStream())
                {
                    int read;
                    while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        ms.Write(buffer, 0, read);
                    }
                    return ms.ToArray();
                }
            }

        public static IEnumerable<Client> GetClients()
        {
            // Clients credentials.
            return new List<Client>
            {
                // http://docs.identityserver.io/en/release/reference/client.html.
                new Client
                {
                    ClientId = "client",
                    AllowedCustomGrantTypes = new List<string>(){ Constants.GrantTypes.Password}, // Resource Owner Password Credential grant.
                    AccessTokenLifetime = 900,
                    AllowedScopes = {
                        Constants.StandardScopes.OpenId, // For UserInfo endpoint.
                        Constants.StandardScopes.Profile,
                        Constants.StandardScopes.OfflineAccess, // for refresh token
                        "roles"
                    },
                    ClientSecrets = new List<Secret> { (new Secret("secret".Sha256())) },
                    //AllowAccessToAllCustomGrantTypes = true,
                    //AllowAccessToAllScopes = true,
                    RefreshTokenUsage = TokenUsage.OneTimeOnly,
                    AbsoluteRefreshTokenLifetime = 7200,
                    SlidingRefreshTokenLifetime = 900,
                    RefreshTokenExpiration = TokenExpiration.Sliding,
                    Flow = Flows.ResourceOwner
                }
            };
        }

        public static IEnumerable<Scope> GetScopes()
        {
            return new List<Scope>
            {
                new Scope
                {
                    Enabled = true,
                    Name = "roles",
                    Type = ScopeType.Identity,
                    Claims = new List<ScopeClaim>
                    {
                        new ScopeClaim("role")
                    }
                },
                new Scope
                {
                    Enabled = true,
                    DisplayName = "Sample API",
                    Name = "sampleApi",
                    Description = "Access to a sample API",
                    Type = ScopeType.Resource,

                    Claims = new List<ScopeClaim>
                    {
                        new ScopeClaim("role")
                    }
                },
                StandardScopes.OpenId,
                StandardScopes.Profile,
                StandardScopes.OfflineAccess,
            };

        }

            public static IEnumerable<RelyingParty> GetRelyingParties()
            {
                return new List<RelyingParty> {
                    new RelyingParty {
                        Realm = "urn:identityServer",
                        Name = "testclient",
                        Enabled = true,
                        ReplyUrl = "https://localhost:44331/",
                        TokenType = TokenTypes.Saml2TokenProfile11,
                        ClaimMappings =
                            new Dictionary<string, string> {
                                { "sub", ClaimTypes.NameIdentifier },
                                { "name", ClaimTypes.Name },
                                { "given_name", ClaimTypes.GivenName },
                                { "family_name", ClaimTypes.Surname },
                                { "email", ClaimTypes.Email },
                                { "upn", ClaimTypes.Upn }
                            }
                    }
                };
        }
    }
}