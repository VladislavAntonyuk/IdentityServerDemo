using System.Collections.Generic;
using System.Security.Claims;

namespace IdentityServerDemo.Services
{
    public class CustomUser
    {
        public string Subject { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public bool IsActive { get; set; }
        public string RealName { get; set; }
        public string Email { get; set; }
        public string AccountLevel { get; set; }
    }
}