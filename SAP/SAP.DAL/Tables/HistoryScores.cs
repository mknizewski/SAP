using System.ComponentModel.DataAnnotations;

namespace SAP.DAL.Tables
{
    public class HistoryScores
    {
        [Key]
        public int Id { get; set; }

        public int OldId { get; set; }
        public int TournamentId { get; set; }
        public int PhaseId { get; set; }
        public string UserId { get; set; }
        public decimal TotalScore { get; set; }
    }
}