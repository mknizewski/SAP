using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SAP.DAL.Tables
{
    public class HistoryScores
    {
        [Key]
        public int Id { get; set; }

        public int OldId { get; set; }

        [ForeignKey("Tournament")]
        public int TournamentId { get; set; }

        [ForeignKey("Phase")]
        public int PhaseId { get; set; }

        [ForeignKey("User")]
        public string UserId { get; set; }

        public decimal TotalScore { get; set; }

        public virtual Tournament Tournament { get; set; }
        public virtual Phase Phase { get; set; }
        public virtual ApplicationUser User { get; set; }
    }
}