using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using IdentityServer3.AccessTokenValidation;
using IdentityServer3.Core.Configuration;
using IdentityServer3.Core.Resources;
using IdentityServer3.Core.Services;
using IdentityServer3.Core.Services.Default;
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
            app.Map("/core", core =>
           {
               //app.UseIdentityServerBearerTokenAuthentication(new IdentityServerBearerTokenAuthenticationOptions
               //{
               //    Authority = "https://localhost:44331/identity",
               //    RequiredScopes = new[] { "sampleApi" }
               //});
               var viewOptions = new DefaultViewServiceOptions();
               viewOptions.Stylesheets.Add("/Content/Site.css");
               //viewOptions.Scripts.Add("/Content/Site.css");

               var factory = new IdentityServerServiceFactory()
                   .UseInMemoryClients(Config.GetClients())
                   .UseInMemoryScopes(Config.GetScopes())
                   .AddCustomUserStore();

               factory.ConfigureDefaultViewService(viewOptions);

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

               core.UseIdentityServer(options);
           });
        }

        public static void ConfigureAdditionalIdentityProviders(IAppBuilder app, string signInAsType)
        {
            app.UseWsFederationAuthentication(
                new WsFederationAuthenticationOptions
                {
                    Wtrealm = "urn:identityServer", //Relying Party Identifiers we configured earlier within ADFS
                    MetadataAddress = "https://vladdomain.local/federationmetadata/2007-06/federationmetadata.xml",
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
