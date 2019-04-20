using System.Security.Cryptography.X509Certificates;
using IdentityServer3.Core.Configuration;
using IdentityServer3.Core.Resources;
using IdentityServer3.Core.Services;
using IdentityServerDemo.IdentityServerConfig;
using IdentityServerDemo.Services;
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

        }
    }
}
