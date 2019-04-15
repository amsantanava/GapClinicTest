using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Mocks;
using AppClinic.BusinessLogic;
using AppClinic.Models;
using AppClinic.Controllers;
using System.Web.Http;
using System.Linq;
using System.Collections.Generic;
using System.Web.Http.Results;
using Newtonsoft.Json;

namespace TestAppClinic
{
    [TestClass]
    public class AppointmentTest
    {
        private IBusinessAppointment businessAppointment;

        private void MockBusinessAppointmentRight()
        {
            businessAppointment = MockRepository.GenerateMock<IBusinessAppointment>();
            businessAppointment.Stub(ba => ba.AllowedAppointmentType(null))
                .IgnoreArguments().Return(true);
            businessAppointment.Stub(ba => ba.AllowedCanelAppointment(null))
                .IgnoreArguments().Return(true);

            businessAppointment.Stub(ba => ba.GetAppointments())
                .IgnoreArguments().Return(
                    new List<Appointment>
                    {
                        new Appointment { Id = 1, Date = new DateTime(2019,05,05,12,00,23), PatientId = 1, Status = 1, Type = "Odontología"},
                        new Appointment { Id = 2, Date = new DateTime(2019,05,15,12,00,23), PatientId = 1, Status = 1, Type = "Pediatría"},
                        new Appointment { Id = 3, Date = new DateTime(2019,11,05,12,00,23), PatientId = 1, Status = 1, Type = "Neurología"},
                    }
                );
            Appointment appointment = new Appointment { Id = 1, Date = new DateTime(2019, 05, 05, 12, 00, 23), PatientId = 1, Status = 1, Type = "Odontología" };
            businessAppointment.Stub(ba => ba.GetAppointment(0))
                .IgnoreArguments().Return(appointment);
            businessAppointment.Stub(ba => ba.CreateAppointment(null))
               .IgnoreArguments().Return(appointment);
            businessAppointment.Stub(ba => ba.CancelAppointment(null))
               .IgnoreArguments().Return(appointment);
        }

        private void MockBusinessAppointmentFail()
        {
            businessAppointment = MockRepository.GenerateMock<IBusinessAppointment>();
            businessAppointment.Stub(ba => ba.AllowedAppointmentType(null))
                .IgnoreArguments().Return(false);
            businessAppointment.Stub(ba => ba.AllowedCanelAppointment(null))
                .IgnoreArguments().Return(false);

            businessAppointment.Stub(ba => ba.GetAppointments())
                .IgnoreArguments().Return(null);
            businessAppointment.Stub(ba => ba.GetAppointment(0))
                .IgnoreArguments().Return(null);
            businessAppointment.Stub(ba => ba.CreateAppointment(null))
               .IgnoreArguments().Return(null);
            businessAppointment.Stub(ba => ba.CancelAppointment(null))
               .IgnoreArguments().Return(null);
        }

        [TestMethod]
        public void TestGetAppointmentsRight()
        {
            MockBusinessAppointmentRight();
            AppointmentsController controller = new AppointmentsController(businessAppointment);
            List<Appointment> result = controller.GetAppointments();
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void TestGetAppointmentsFail()
        {
            MockBusinessAppointmentFail();
            AppointmentsController controller = new AppointmentsController(businessAppointment);
            List<Appointment> result = controller.GetAppointments();
            Assert.IsNull(result);
        }

        [TestMethod]
        public async System.Threading.Tasks.Task TestCreateAppointmentsRightAsync()
        {
            MockBusinessAppointmentRight();
            Appointment appointment = new Appointment { Id = 1, Date = new DateTime(2019, 05, 05, 12, 00, 23), PatientId = 1, Status = 1, Type = "Odontología" };
            AppointmentsController controller = new AppointmentsController(businessAppointment);
            IHttpActionResult result = await controller.PostAppointment(appointment);
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public async System.Threading.Tasks.Task TestCreateAppointmentsFailAsync()
        {
            MockBusinessAppointmentFail();
            Appointment appointment = new Appointment { Id = 1, Date = new DateTime(2019, 05, 05, 12, 00, 23), PatientId = 1, Status = 1, Type = "Odontología" };
            AppointmentsController controller = new AppointmentsController(businessAppointment);
            IHttpActionResult result = await controller.PostAppointment(appointment);
            Assert.IsNotNull(result);
        }
    }
}
