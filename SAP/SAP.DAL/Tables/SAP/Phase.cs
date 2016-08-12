using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SAP.DAL.Tables
{
    public class Phase
    {
        [Key]
        public int Id { get; set; }

        [Key, ForeignKey("Tournament")]
        public int? TournamentId { get; set; }

        public string Name { get; set; }
        public int Order { get; set; }
        public int MaxUsers { get; set; }
        public int MaxTasks { get; set; }
        public bool IsActive { get; set; }

        public virtual Tournament Tournament { get; set; }
    }
}