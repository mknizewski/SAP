using System;
using System.ComponentModel.DataAnnotations;

namespace SAP.DAL.Tables
{
    public class Tournament
    {
        [Key]
        public int Id { get; set; }

        public string Title { get; set; }
        public string Description { get; set; }
        public int MaxUsers { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsActive { get; set; }
        public bool IsConfigured { get; set; }
    }
}