using System.Web.Mvc;
using IdentityServer3.Core.Configuration;
using IdentityServer3.Core.Services;
using IdentityServerDemo.Controllers;
using IdentityServerDemo.Services;

namespace IdentityServerDemo.IdentityServerConfig
{
    public static class CustomIdentityServerFactoryExtensions
    {
        public static IdentityServerServiceFactory AddCustomUserStore(this IdentityServerServiceFactory factory)
        {
            factory.Register(new Registration<IUserRepository, UserRepository>());

            factory.UserService = new Registration<IUserService, CustomUserService>();
            return factory;
        }

    }
}