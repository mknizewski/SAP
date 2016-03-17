using SAP.DAL.Tables;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SAP.DAL.Abstract
{
    public interface ITournamentRepository
    {
        IEnumerable<Tournament> Tournaments { get; }
        IEnumerable<Phase> Phase { get; }
        IEnumerable<Tasks> Tasks { get; }
        IEnumerable<TasksTestData> TasksTestData { get; }
        IEnumerable<TournamentUsers> TournamentsUsers { get; }

        void SetPhaseActiveFlag(int Id, bool flag);

        void SetTaskActiveFlag(int Id, bool flag);

        void SetTournamentActiveFlag(int Id, bool flag);

        Task<bool> AddTestDataAsync(List<TasksTestData> testData);

        Task<bool> AddTournamentAsync(Tournament tour, List<Phase> phases, List<Tasks> tasks, int[] taskMetadata);

        bool AddTaskTestingData(TasksTestData testData);

        bool DeleteTournament(int tournamentId);

        void Dispose();
    }
}