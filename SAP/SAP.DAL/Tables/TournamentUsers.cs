using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SAP.DAL.Tables
{
    //TODO: Przeniesc całą tą baze do biznesowej logiki (kontekst bazy danych)
    public class TournamentUsers
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("Tournament")]
        public int TournamentId { get; set; }
        [ForeignKey("User")]
        public string UserId { get; set; }
        [ForeignKey("Phase")]
        public int PhaseId { get; set; }

        public virtual Tournament Tournament { get; set; }
        //TODO: Dodać virtual user
        public virtual Phase Phase { get; set; }
    }
}
