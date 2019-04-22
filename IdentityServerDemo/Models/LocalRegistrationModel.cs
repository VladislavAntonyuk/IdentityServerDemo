using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace IdentityServerDemo.Models
{
    public class LocalRegistrationModel
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string RealName { get; set; }
        [Required]
        public string Email { get; set; }
    }
}