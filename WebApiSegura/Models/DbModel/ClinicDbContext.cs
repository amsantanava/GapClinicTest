namespace AppClinic.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class ClinicDbContext : DbContext, IClinicDbContext
    {
        public ClinicDbContext()
            : base("name=DefaultConnection")
        {
        }


        public virtual DbSet<Appointment> Appointments { get; set; }
        public virtual DbSet<Patient> Patients { get; set; }
        public virtual DbSet<User> Users { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Patient>()
                .Property(e => e.Nombre)
                .IsUnicode(false);

            modelBuilder.Entity<Patient>()
                .Property(e => e.IdentityNumber)
                .IsUnicode(false);

            modelBuilder.Entity<Patient>()
                .Property(e => e.Address)
                .IsUnicode(false);

            modelBuilder.Entity<Patient>()
                .HasMany(e => e.Appointments)
                .WithRequired(e => e.Patient)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .Property(e => e.Username)
                .IsUnicode(false);

            modelBuilder.Entity<User>()
                .Property(e => e.Password)
                .IsUnicode(false);

            modelBuilder.Entity<User>()
                .Property(e => e.Role)
                .IsUnicode(false);
            
        }
    }
}
