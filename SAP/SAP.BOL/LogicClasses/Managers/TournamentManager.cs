using SAP.BOL.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SAP.DAL.Tables;
using SAP.DAL.Abstract;

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
    }
}
