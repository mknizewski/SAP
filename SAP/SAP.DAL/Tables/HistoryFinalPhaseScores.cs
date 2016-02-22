using System.ComponentModel.DataAnnotations;

namespace SAP.DAL.Tables
{
    public class HistoryFinalPhaseScores
    {
        [Key]
        public int Id { get; set; }

        public int OldId { get; set; }

        public int TournamentId { get; set; }

        public int UserId { get; set; }

        public int PhaseId { get; set; }

        public decimal TotalScore { get; set; }
        public bool AllowToNextPhase { get; set; }
    }
}
