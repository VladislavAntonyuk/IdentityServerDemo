using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServerDemo.Services
{
    public class UserRepository : IUserRepository
    {

        private static List<CustomUser> users = new List<CustomUser>();

        static UserRepository()
        {
            users.Add(new CustomUser()
            {
                Password = "admin",
                Subject = "khdfkjghkdfjgh",
                Username = "admin",
                AccountLevel = "admin",
                Email = "admin@email.com",
                IsActive = true,
                RealName = "Administrator"
            });
            users.Add(new CustomUser()
            {
                Password = "user",
                Subject = "lkmlfkgnkjdgnkdj",
                Username = "user",
                AccountLevel = "user",
                Email = "user@email.com",
                IsActive = true,
                RealName = "User"
            });
        }
        public CustomUser Find(string userId)
        {
            return users.FirstOrDefault(x => x.Subject == userId);
        }
        
        public CustomUser ValidateCredentials(string userName, string password)
        {
            return users.SingleOrDefault(x => x.Username == userName && x.Password == password);
        }
    }

    public interface IUserRepository
    {
        CustomUser Find(string userId);
        CustomUser ValidateCredentials(string userName, string password);
    }
}