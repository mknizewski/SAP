using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

//TODO: Dorobic historczyne tabele
namespace SAP.DAL.Tables
{
    public class Scores
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Tournament")]
        public int TournamentId { get; set; }

        [ForeignKey("Task")]
        public int TaskId { get; set; }

        [ForeignKey("User")]
        public string UserId { get; set; }

        [ForeignKey("Compiler")]
        public int CompilerId { get; set; }

        public decimal Score { get; set; }
        public string Program { get; set; }
        public DateTime InsertTime { get; set; }

        public virtual Tournament Tournament { get; set; }
        public virtual Tasks Task { get; set; }
        public virtual ApplicationUser User { get; set; }
        public virtual Compilers Compiler { get; set; }
    }
}