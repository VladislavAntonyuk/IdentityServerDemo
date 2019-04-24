using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using IdentityServer3.Core.Configuration;
using IdentityServer3.Core.Resources;
using IdentityServer3.Core.Services;
using IdentityServer3.WsFederation.Configuration;
using IdentityServer3.WsFederation.Models;
using IdentityServer3.WsFederation.Services;
using IdentityServerDemo.IdentityServerConfig;
using IdentityServerDemo.Services;
using Microsoft.Owin.Security.WsFederation;
using Owin;

namespace IdentityServerDemo
{
    internal class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            // app.Map("/core", core =>
            //{
            var factory = new IdentityServerServiceFactory()
                .UseInMemoryClients(Config.GetClients())
                .UseInMemoryScopes(Config.GetScopes())
                .AddCustomUserStore();

            var options = new IdentityServerOptions
            {
                SiteName = "IdentityServerDemo",
                SigningCertificate = Config.GetCert(),
                RequireSsl = false,
                Factory = factory,
                PluginConfiguration = ConfigureWsFederation,

                AuthenticationOptions = new AuthenticationOptions
                {
                    IdentityProviders = ConfigureAdditionalIdentityProviders,
                    LoginPageLinks = new LoginPageLink[]
                    {
                            new LoginPageLink
                            {
                                Text = "Register",
                                Href = "localregistration"
                            }
                    }
                },

                EventsOptions = new EventsOptions
                {
                    RaiseSuccessEvents = true,
                    RaiseErrorEvents = true,
                    RaiseFailureEvents = true,
                    RaiseInformationEvents = true
                }
            };

            app.UseIdentityServer(options);
            // });
        }

        public static void ConfigureAdditionalIdentityProviders(IAppBuilder app, string signInAsType)
        {
            app.UseWsFederationAuthentication(
                new WsFederationAuthenticationOptions
                {
                    Wtrealm = "urn:identityServer", //Relying Party Identifiers we configured earlier within ADFS
                    MetadataAddress = "https://login.microsoftonline.com/4341df80-fbe6-41bf-89b0-e6e2379c9c23/federationmetadata/2007-06/federationmetadata.xml?appid=2d29cb0d-2efa-4e79-8708-a44195666796",
                    AuthenticationType = "adfs",
                    Caption = "ADFS",
                    SignInAsAuthenticationType = signInAsType
                });

        }


        private void ConfigureWsFederation(IAppBuilder pluginApp, IdentityServerOptions options)
        {
            var factory = new WsFederationServiceFactory(options.Factory);
            factory.Register(new Registration<IEnumerable<RelyingParty>>(Config.GetRelyingParties()));
            factory.RelyingPartyService =
                new Registration<IRelyingPartyService>(typeof(InMemoryRelyingPartyService));

            pluginApp.UseWsFederationPlugin(new WsFederationPluginOptions
            {
                IdentityServerOptions = options,
                Factory = factory
            });
        }
    }
}
