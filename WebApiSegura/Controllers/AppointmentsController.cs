using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using AppClinic.Models;
using WebApiSegura.Models;

namespace WebApiSegura.Controllers
{
    public class AppointmentsController : ApiController
    {
        private ClinicDbContext db = new ClinicDbContext();
        private int ActiveStatus = 1;
        private int CancelStatus = 0;
        private int LimitHoursToCancel = 24;
        private string[] AllowedAppointmentTypes = { "Medicina General", "Odontología", "Pediatría", "Neurología" };

        // GET: api/Appointments
        public IQueryable<Appointment> GetAppointments()
        {
            return db.Appointments;
        }

        // GET: api/Appointments/5
        [ResponseType(typeof(Appointment))]
        public async Task<IHttpActionResult> GetAppointment(int id)
        {
            Appointment appointment = await db.Appointments.FindAsync(id);
            if (appointment == null)
            {
                return NotFound();
            }

            return Ok(appointment);
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

                if (AppointmentExistsSameDay(appointment))
                {
                    return Ok(new ErrorResponse(){ErrorCode =  "BF01", ErrorMessage = "Already exists another appointment at the same day" });
                }


                if (!AllowedAppointmentType(appointment.Type))
                {
                    return Ok(new ErrorResponse() { ErrorCode = "BF03", ErrorMessage = "Appointment type is not allowed" });
                }
                appointment.Status = ActiveStatus; //Default value 1 for Active
                db.Appointments.Add(appointment);
                await db.SaveChangesAsync();

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
            Appointment appointment = await db.Appointments.FindAsync(Id);

            if (appointment == null)
            {
                return NotFound();
            }

            if (!AllowedCanelAppointment(appointment))
            {
                return Ok(new ErrorResponse() { ErrorCode = "BF02", ErrorMessage = "It can't be cancel" });
            }


            appointment.Status = CancelStatus; //Default value 0 for Cancel
            db.Entry(appointment).State = EntityState.Modified;
            await db.SaveChangesAsync();

            return Ok("Appointment was canceled");
        }



        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool AppointmentExistsSameDay(Appointment appointment)
        {
            return db.Appointments.Count(
                a => DbFunctions.TruncateTime(a.Date) == DbFunctions.TruncateTime(appointment.Date)
                && a.PatientId == appointment.PatientId
                && a.Status == ActiveStatus
                ) > 0;
        }

        private bool AllowedCanelAppointment(Appointment appointment)
        {
            DateTime now = DateTime.Now;
            //Calculate limit datetime to cancel
            DateTime MinDateToCancel = now.AddHours(LimitHoursToCancel);
            return appointment.Date > MinDateToCancel;
        }

        private bool AllowedAppointmentType(string type)
        {
            return AllowedAppointmentTypes.Contains(type);
        }
    }
}