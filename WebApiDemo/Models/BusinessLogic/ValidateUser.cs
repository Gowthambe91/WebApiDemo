using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApiDemo.Models.DomainModel;

namespace WebApiDemo.Models.BusinessLogic
{
    public class ValidateUser
    {
        public static bool Login(string UserName, string Password)
        {
            UserBL userBL = new UserBL();
            List<User> usersList = userBL.GetUsers();

            return usersList.Any(user => user.UserName.Equals(UserName, StringComparison.OrdinalIgnoreCase) &&
            user.Password.Equals(Password));
        }

        public static User GetUserDetails(string userName, string passWord)
        {
            UserBL userBL = new UserBL();

            return userBL.GetUsers().FirstOrDefault(user => user.UserName.Equals(userName, StringComparison.OrdinalIgnoreCase) &&
            user.Password == passWord);
        }
    }
}