using AppClinic.Models;
using System.Collections.Generic;
using System.Linq;

namespace AppClinic.BusinessLogic
{
    public interface IBusinessAppointment
    {
        List<Appointment> GetAppointments();
        List<Appointment> GetAppointmentsByPatient(int patientId);
        Appointment CreateAppointment(Appointment appointment);
        Appointment GetAppointment(int Id);
        Appointment CancelAppointment(Appointment appointment);
        bool AllowedAppointmentType(string type);
        bool AllowedCanelAppointment(Appointment appointment);
        bool AppointmentExistsSameDay(Appointment appointment);
    }
}