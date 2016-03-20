using SAP.DAL.Tables;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SAP.BOL.Abstract
{
    public interface ITournamentManager
    {
        IEnumerable<Tournament> Tournaments { get; }
        IEnumerable<Phase> Phases { get; }
        IEnumerable<Tasks> Tasks { get; }

        Task<bool> AddTournamnetAsync(Tournament tour, List<Phase> phases, List<Tasks> tasks, int[] taksPerPhaseCount);

        Task<bool> AddTestDataAsync(List<TasksTestData> testData);

        bool ConfigureSet(int Id, bool flag);

        void SetPhaseActiveFlag(int Id, bool flag);

        void SetTaskActiveFlag(int Id, bool flag);

        void SetTournamentActiveFlag(int Id, bool flag);

        List<string> ValidateTournament(int Id);

        void Dispose();
    }
}