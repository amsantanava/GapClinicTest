using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AppClinic.Models
{
    public class ClinicDbInitializer : System.Data.Entity.DropCreateDatabaseIfModelChanges<ClinicDbContext>
    {
        protected override void Seed(ClinicDbContext context)
        {
            var users = new List<User>
            {
            new User{Id = 1, Username = "Admin", Password = "123456", Role = "Admin"},
            new User{Id = 2, Username = "Patient1", Password = "123456", Role = "Patient"},
            };

            users.ForEach(s => context.Users.Add(s));
            context.SaveChanges();

            var patients = new List<Patient>
            {
            new Patient { Id = 2, Nombre = "Arley Santana", IdentityNumber = "123345678", Address= "CL 10 #34-80", Age = 28 }
            };

            patients.ForEach(s => context.Patients.Add(s));
            context.SaveChanges();

        }
    }
}