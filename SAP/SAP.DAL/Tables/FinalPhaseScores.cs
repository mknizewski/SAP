using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAP.DAL.Tables
{
    public class FinalPhaseScores
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("Tournament")]
        public int TournamentId { get; set; }
        [ForeignKey("User")]
        public int UserId { get; set; }
        [ForeignKey("Phase")]
        public int PhaseId { get; set; }
        public decimal TotalScore { get; set; }
        public bool AllowToNextPhase { get; set; }

        public virtual Tournament Tournament { get; set; }
        //TODO: USER!
        public virtual Phase Phase { get; set; }

    }
}
