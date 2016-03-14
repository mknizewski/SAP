using SAP.DAL.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        Task<bool> AddTournamentAsync(Tournament tour, List<Phase> phases, List<Tasks> tasks, int[] taskMetadata);
        bool AddTaskTestingData(TasksTestData testData);
        bool DeleteTournament(int tournamentId);
        void Dispose();
    }
}
