﻿using SAP.DAL.Tables;
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
        IEnumerable<HistoryTournamentUsers> HistoryTournamentsUsers { get; }

        bool EditTask(Tasks model);

        bool EditTournament(Tournament model);

        void SetPhaseActiveFlag(int Id, bool flag);

        void SetTaskActiveFlag(int Id, bool flag);

        void CountScores(int tournamentId, int phaseId);

        void SetPromotions(int tournamentId, int phaseId);

        void SetTournamentActiveFlag(int Id, bool flag);

        Task<bool> AddTestDataAsync(List<TasksTestData> testData);

        Task<bool> AddTournamentAsync(Tournament tour, List<Phase> phases, List<Tasks> tasks, int[] taskMetadata);

        bool ConfigureSet(int Id, bool flag);

        bool DeleteTestData(int Id);

        bool RegisterToTournament(string userId, int tournamentId);
        bool IsRegistered(string userId, int tournamentId);

        bool AddTaskTestingData(TasksTestData testData);

        bool CourseSaveChanges(int tourId, int phaseId, int taskId);

        bool DeleteTournament(int tournamentId);

        void Dispose();
    }
}