using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AppClinic.Models;
using Rhino.Mocks;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Collections.Generic;
using AppClinic.Controllers;

namespace AppClinicTest
{
    [TestClass]
    public class AppointmentsControllerTest
    {
        private IClinicDbContext context;

        [TestInitialize]
        public void Initialize()
        {
            context = MockRepository.GenerateMock<IClinicDbContext>();
            MockUser();
            MockPatient();
            MockAppointment();
        }

        [TestMethod]
        public void TestLoginAuthenticate()
        {
            LoginController controller = new LoginController(context);
            var response = controller.Authenticate(new LoginRequest() { Username = "Admin", Password = "123456" });
        }



        private static IDbSet<T> GetDbSetTestDouble<T>(IList<T> data) where T : class
        {
            IQueryable<T> queryable = data.AsQueryable();

            IDbSet<T> dbSet = MockRepository.GenerateMock<IDbSet<T>, IQueryable>();

            dbSet.Stub(m => m.Provider).Return(queryable.Provider);
            dbSet.Stub(m => m.Expression).Return(queryable.Expression);
            dbSet.Stub(m => m.ElementType).Return(queryable.ElementType);
            dbSet.Stub(m => m.GetEnumerator()).Return(queryable.GetEnumerator());

            return dbSet;
        }

        private void MockUser()
        {
            var stubUserData = new List<User>
                            {
                                new User { Id = 1, Username = "Admin", Password = "123456", Role = "Admin" },
                                new User { Id = 2, Username = "Patinet1", Password = "123456", Role = "Patient" }
                            };
            context.Stub(x => x.Users).PropertyBehavior();
            context.Users = (DbSet<User>)GetDbSetTestDouble(stubUserData);
        }
        private void MockPatient()
        {
            var stubPatientData = new List<Patient>
                            {
                                new Patient { Id = 1, Address = "CL 10 CR 85 50", Age = 21, IdentityNumber = "156561615", Nombre = " Juan Carlos Morales"},
                            };
            context.Stub(x => x.Patients).PropertyBehavior();
            context.Patients = (DbSet<Patient>)GetDbSetTestDouble(stubPatientData);
        }

        private void MockAppointment()
        {
            var stubAppointmentData = new List<Appointment>
                            {
                                new Appointment { Id = 1, Date = new DateTime(2019,04,22,15,00,00), PatientId = 1, Status = 1, Type = "Medicina General" },
                                new Appointment { Id = 2, Date = new DateTime(2019,04,23,15,00,00), PatientId = 1, Status = 1, Type = "Medicina General" },
                            };
            context.Stub(x => x.Appointments).PropertyBehavior();
            context.Appointments = (DbSet<Appointment>)GetDbSetTestDouble(stubAppointmentData);
        }
    }
}
