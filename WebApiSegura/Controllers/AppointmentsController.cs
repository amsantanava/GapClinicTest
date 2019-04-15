using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using AppClinic.Models;
using AppClinic.BusinessLogic;

namespace AppClinic.Controllers
{
    [Authorize]
    public class AppointmentsController : ApiController
    {
        IBusinessAppointment businessAppointment;

        public AppointmentsController()
        {
            businessAppointment = new BusinessAppointment();
        }

        public AppointmentsController(IBusinessAppointment bsAppointment)
        {
            businessAppointment = bsAppointment;
        }

        // GET: api/Appointments
        public List<Appointment> GetAppointments()
        {
            return businessAppointment.GetAppointments();
        }

        // GET: api/Appointments/5
        [ActionName("MyAppointments")]
        [Route("api/Appointments/MyAppointments/{patientId}")]
        [ResponseType(typeof(List<Appointment>))]
        public async Task<IHttpActionResult> GetMyAppointments(int patientId)
        {
            List<Appointment> appointments = businessAppointment.GetAppointmentsByPatient(patientId);
            if (appointments == null)
            {
                return NotFound();
            }

            return Ok(appointments);
        }

        
        // POST: api/Appointments
        [ResponseType(typeof(Appointment))]
        public async Task<IHttpActionResult> PostAppointment(Appointment appointment)
        {
            try
            {

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (businessAppointment.AppointmentExistsSameDay(appointment))
                {
                    return Ok(new ErrorResponse(){ErrorCode =  "BF01", ErrorMessage = "Already exists another appointment at the same day" });
                }


                if (!businessAppointment.AllowedAppointmentType(appointment.Type))
                {
                    return Ok(new ErrorResponse() { ErrorCode = "BF03", ErrorMessage = "Appointment type is not allowed" });
                }
                appointment = businessAppointment.CreateAppointment(appointment);

                return CreatedAtRoute("DefaultApi", new { id = appointment.Id }, appointment);
            }
            catch (Exception)
            {
                return Ok(new ErrorResponse() { ErrorCode = "EX00", ErrorMessage = "It was not posible create the appointment"});
            }
        }

        // DELETE: api/Appointments/5
        [HttpPost]
        [ActionName("Cancel")]
        [Route("api/Appointments/Cancel/{Id}")]
        [ResponseType(typeof(Appointment))]
        public async Task<IHttpActionResult> CancelAppointment(int Id)
        {
            Appointment appointment = businessAppointment.GetAppointment(Id);

            if (appointment == null)
            {
                return NotFound();
            }

            if (!businessAppointment.AllowedCanelAppointment(appointment))
            {
                return Ok(new ErrorResponse() { ErrorCode = "BF02", ErrorMessage = "It can't be cancel" });
            }


            appointment = businessAppointment.CancelAppointment(appointment);

            return Ok("Appointment was canceled");
        }



        //protected override void Dispose(bool disposing)
        //{
        //    if (disposing)
        //    {
        //        db.Dispose();
        //    }
        //    base.Dispose(disposing);
        //}

       
    }
}