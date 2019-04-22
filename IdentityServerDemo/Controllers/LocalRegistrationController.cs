using System;
using System.Collections.Generic;
using System.IdentityModel.Claims;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IdentityServer3.Core;
using IdentityServerDemo.Models;
using IdentityServerDemo.Services;

namespace IdentityServerDemo.Controllers
{
    public class LocalRegistrationController : Controller
    {
        private readonly IUserRepository _userRepository;

        public LocalRegistrationController()
        {
            _userRepository = new UserRepository();
        }

        [HttpGet]
        public ActionResult Index(string signin)
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(string signin, LocalRegistrationModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new CustomUser
                {
                    Username = model.Username,
                    Password = model.Password,
                    Subject = Guid.NewGuid().ToString(),
                    Email = model.Email,
                    RealName = model.RealName,
                    AccountLevel = "user",
                    IsActive = true
                };
                _userRepository.Add(user);

                return Redirect("~/" + Constants.RoutePaths.Login + "?signin=" + signin);
            }

            return View();
        }
    }
}