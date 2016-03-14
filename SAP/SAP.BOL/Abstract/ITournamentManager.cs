using SAP.DAL.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAP.BOL.Abstract
{
    public interface ITournamentManager
    {
        IEnumerable<Tournament> Tournaments { get; }
        IEnumerable<Phase> Phases { get; }
        IEnumerable<Tasks> Tasks { get; }
        Task<bool> AddTournamnetAsync(Tournament tour, List<Phase> phases, List<Tasks> tasks, int[] taksPerPhaseCount);
        void Dispose();
    }
}
