using AppClinic.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AppClinic.BusinessLogic
{
    public class BusinessUser : IBusinessUser
    {
        private ClinicDbContext context = new ClinicDbContext();

        public BusinessUser()
        {
        }

        //This method find the user with the credentials and if exists return the user 
        public User GetUserByCredentials(LoginRequest login)
        {
            return context.Users.Where(
                u => u.Username == login.Username
                && u.Password == login.Password
                ).FirstOrDefault();
        }
    }
}