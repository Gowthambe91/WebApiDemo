using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApiDemo.Models.DomainModel;

namespace WebApiDemo.Models.BusinessLogic
{
    public class UserBL
    {
        public List<User> GetUsers()
        {
            List<User> usersList = new List<User>();
            usersList.Add(new User()
            {
                ID = 101,
                UserName = "Adminuser",
                Password = "12345678",
                Roles = "Admin"
            });

            usersList.Add(new User()
            {
                ID = 102,
                UserName = "SuperadminUser",
                Password = "abcdef",
                Roles = "Super Admin"
            });

            usersList.Add(new User()
            {
                ID = 102,
                UserName = "BothUser",
                Password = "abcdef",
                Roles = "Admin,Super Admin"
            });

            return usersList;
        }
    }
}