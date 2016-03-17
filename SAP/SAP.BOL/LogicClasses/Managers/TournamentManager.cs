using SAP.BOL.Abstract;
using SAP.DAL.Abstract;
using SAP.DAL.Tables;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SAP.BOL.LogicClasses.Managers
{
    public class TournamentManager : ITournamentManager, IDisposable
    {
        private ITournamentRepository _tournamentRepository;

        public TournamentManager(ITournamentRepository tournamentRepository)
        {
            _tournamentRepository = tournamentRepository;
        }

        public IEnumerable<Phase> Phases
        {
            get
            {
                return _tournamentRepository.Phase;
            }
        }

        public IEnumerable<Tasks> Tasks
        {
            get
            {
                return _tournamentRepository.Tasks;
            }
        }

        public IEnumerable<Tournament> Tournaments
        {
            get
            {
                return _tournamentRepository.Tournaments;
            }
        }

        public async Task<bool> AddTestDataAsync(List<TasksTestData> testData)
        {
            bool result = await _tournamentRepository.AddTestDataAsync(testData);

            return result;
        }

        public async Task<bool> AddTournamnetAsync(Tournament tour, List<Phase> phases, List<Tasks> tasks, int[] taskPerPhaseCount)
        {
            //konfiguracja aktywności
            tour.IsActive = false;
            tour.IsConfigured = false;
            phases.ForEach(x => x.IsActive = false);
            tasks.ForEach(x => x.IsActive = false);

            bool result = await _tournamentRepository.AddTournamentAsync(tour, phases, tasks, taskPerPhaseCount);
            return result;
        }

        public void Dispose()
        {
            _tournamentRepository.Dispose();
        }

        public void SetPhaseActiveFlag(int Id, bool flag)
        {
            _tournamentRepository.SetPhaseActiveFlag(Id, flag);
        }

        public void SetTaskActiveFlag(int Id, bool flag)
        {
            _tournamentRepository.SetTaskActiveFlag(Id, flag);
        }

        public void SetTournamentActiveFlag(int Id, bool flag)
        {
            _tournamentRepository.SetTournamentActiveFlag(Id, flag);
        }
    }
}