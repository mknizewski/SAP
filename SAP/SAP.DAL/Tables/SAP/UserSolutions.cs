using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SAP.DAL.Tables
{
    public class UserSolutions
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Tournament")]
        public int TournamentId { get; set; }

        [ForeignKey("Phase")]
        public int PhaseId { get; set; }

        [ForeignKey("Task")]
        public int TaskId { get; set; }

        [ForeignKey("User")]
        public string UserId { get; set; }


        public decimal Score { get; set; }
        public string Program { get; set; }
        public double MemoryUsage { get; set; }
        public double ExecutedTime { get; set; }
        public DateTime InsertTime { get; set; }
        public string Error { get; set; }

        public virtual Tournament Tournament { get; set; }
        public virtual Phase Phase { get; set; }
        public virtual Tasks Task { get; set; }
        public virtual ApplicationUser User { get; set; }
    }
}