using System;
using System.ComponentModel.DataAnnotations;

namespace SAP.DAL.Tables
{
    public class HistoryScores
    {
        [Key]
        public int Id { get; set; }
        public int OldId { get; set; }
        public int TournamentId { get; set; }
        public int TaskId { get; set; }
        public int UserId { get; set; }
        public int CompilerId { get; set; }
        public decimal Score { get; set; }
        public string Program { get; set; }
        public DateTime InsertTime { get; set; }

    }
}
