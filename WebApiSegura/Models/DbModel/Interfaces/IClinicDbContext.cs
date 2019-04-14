using System.Data.Entity;

namespace AppClinic.Models
{
    public interface IClinicDbContext
    {
        DbSet<Appointment> Appointments { get; set; }
        DbSet<Patient> Patients { get; set; }
        DbSet<User> Users { get; set; }
    }
}