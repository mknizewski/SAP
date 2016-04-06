using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SAP.DAL.Tables
{
    public class Tasks
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Tournament")]
        public int TournamentId { get; set; }

        [ForeignKey("Phase")]
        public int PhaseId { get; set; }

        public string Title { get; set; }
        public string Description { get; set; }
        public string Input { get; set; }
        public string Output { get; set; }
        public string Example { get; set; }
        public int Order { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsActive { get; set; }
        public double MaxExecuteTime { get; set; }
        public double MaxExecuteMemory { get; set; }
        public int InputDataTypeId { get; set; }
        public byte[] PDF { get; set; }

        public virtual Tournament Tournament { get; set; }
        public virtual Phase Phase { get; set; }
    }
}