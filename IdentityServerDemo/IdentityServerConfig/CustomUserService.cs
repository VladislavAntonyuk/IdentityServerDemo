using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using IdentityServer3.Core.Extensions;
using IdentityServer3.Core.Models;
using IdentityServer3.Core.Services;
using IdentityServer3.Core.Services.Default;
using IdentityServerDemo.Services;
using Microsoft.Owin;

namespace IdentityServerDemo.IdentityServerConfig
{
    public class CustomUserService : UserServiceBase
    {
        private readonly IUserRepository _userRepository;
        OwinContext ctx;

        public CustomUserService(IUserRepository userRepository, OwinEnvironmentService owinEnv)
        {
            ctx = new OwinContext(owinEnv.Environment);
            _userRepository = userRepository;
        }

        public override Task AuthenticateLocalAsync(LocalAuthenticationContext context)
        {
            var user = _userRepository.ValidateCredentials(context.UserName, context.Password);
            if (user != null)
            {
                context.AuthenticateResult = new AuthenticateResult(user.Subject, user.Username);
            }

            return Task.FromResult(0);
        }

        public override Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            var userId = context.Subject.GetSubjectId();
            var user = _userRepository.Find(userId);
            var claims = new List<Claim>
                          {
                new Claim("accountLevel", user.AccountLevel),
                new Claim("email", user.Email),
                new Claim("name", user.RealName)
                            };
            context.IssuedClaims = claims;

            return Task.CompletedTask;
        }

        public override Task IsActiveAsync(IsActiveContext context)
        {
            var userId = context.Subject.GetSubjectId();
            var user = _userRepository.Find(userId);
            context.IsActive = user != null && user.IsActive;

            return Task.CompletedTask;
        }

        public override Task PreAuthenticateAsync(PreAuthenticationContext context)
        {
            var id = ctx.Request.Query.Get("signin");
            context.AuthenticateResult = new AuthenticateResult("~/custom/login?id=" + id, (IEnumerable<Claim>)null);
            return Task.FromResult(0);
        }
    }
}
