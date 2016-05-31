using SAP.BOL.HelperClasses;
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
        IEnumerable<TournamentUsers> TournamentsUsers { get; }
        IEnumerable<HistoryTournamentUsers> HistoryTournamentsUsers { get; }
        IEnumerable<TasksTestData> TasksTestData { get; }

        Task<bool> AddTournamnetAsync(Tournament tour, List<Phase> phases, List<Tasks> tasks, int[] taksPerPhaseCount);

        Task<bool> AddTestDataAsync(List<TasksTestData> testData);

        TournamentsPagination GetTourByPage(int page);

        bool EditTask(Tasks model);

        bool DeleteTestData(int Id);

        bool ConfigureSet(int Id, bool flag);

        bool CourseSaveChanges(int tourId, int phaseId, int taskId);

        void SetPhaseActiveFlag(int Id, bool flag);

        void SetTaskActiveFlag(int Id, bool flag);

        void SetTournamentActiveFlag(int Id, bool flag);

        void CountScores(int tournamentId, int phaseId);

        void SetPromotions(int tournamentId, int phaseId);

        List<string> ValidateTournament(int Id);

        List<Tasks> GetActiveAndEndTask(int tourId);

        List<Phase> GetActiveAndEndPhase(int tourId);

        bool RegisterToTournament(string userId, int tournamentId);

        void Dispose();
    }
}