namespace AppClinic.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Appointment
    {
        public int Id { get; set; }

        public int PatientId { get; set; }

        public DateTime Date { get; set; }

        public int Status { get; set; }
        public string Type { get; set; }

    }
}
