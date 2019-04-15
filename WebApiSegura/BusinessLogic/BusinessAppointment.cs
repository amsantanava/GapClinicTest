using AppClinic.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace AppClinic.BusinessLogic
{
    public class BusinessAppointment : IBusinessAppointment
    {
        private ClinicDbContext db = new ClinicDbContext();
        private int ActiveStatus = 1;
        private int CancelStatus = 0;
        private int LimitHoursToCancel = 24;
        private string[] AllowedAppointmentTypes = { "Medicina General", "Odontología", "Pediatría", "Neurología" };

        public List<Appointment> GetAppointments()
        {
            return db.Appointments.ToList();
        }

        public List<Appointment> GetAppointmentsByPatient(int patientId)
        {
            return db.Appointments
                    .Where(a => a.PatientId == patientId && a.Status == ActiveStatus)
                    .ToList();
        }

        public Appointment CreateAppointment(Appointment appointment)
        {
            appointment.Status = ActiveStatus; //Default value 1 for Active
            db.Appointments.Add(appointment);
            db.SaveChanges();
            return appointment;
        }

        public Appointment GetAppointment(int Id)
        {
           return db.Appointments.Find(Id);
        }

        public Appointment CancelAppointment(Appointment appointment)
        {
            appointment.Status = CancelStatus; //Default value 0 for Cancel
            db.Entry(appointment).State = EntityState.Modified;
            db.SaveChanges();
            return appointment;
        }

        public bool AppointmentExistsSameDay(Appointment appointment)
        {
            return db.Appointments.Count(
                a => DbFunctions.TruncateTime(a.Date) == DbFunctions.TruncateTime(appointment.Date)
                && a.PatientId == appointment.PatientId
                && a.Status == ActiveStatus
                ) > 0;
        }

        public bool AllowedCanelAppointment(Appointment appointment)
        {
            DateTime now = DateTime.Now;
            //Calculate limit datetime to cancel
            DateTime MinDateToCancel = now.AddHours(LimitHoursToCancel);
            return appointment.Date > MinDateToCancel;
        }

        public bool AllowedAppointmentType(string type)
        {
            return AllowedAppointmentTypes.Contains(type);
        }
    }
}