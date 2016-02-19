using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//TODO: Dorobic historczyne tabele 
namespace SAP.DAL.Tables
{
    public class Scores
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("Tournamnet")]
        public int TournamentId { get; set; }
        [ForeignKey("Task")]
        public int TaskId { get; set; }
        [ForeignKey("User")]
        public int UserId { get; set; }
        [ForeignKey("Compiler")]
        public int CompilerId { get; set; }
        public decimal Score { get; set; }
        public string Program { get; set; }
        public DateTime InsertTime { get; set; }

        public virtual Tournament Tournament { get; set; }
        public virtual Tasks Task { get; set; }
        //TODO: USER!
        public virtual Compilers Compiler { get; set; }
    }
}
