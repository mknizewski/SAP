using SAP.BOL.Abstract;
using SAP.BOL.HelperClasses;
using SAP.DAL.Abstract;
using SAP.DAL.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public IEnumerable<HistoryTournamentUsers> HistoryTournamentsUsers
        {
            get
            {
                return _tournamentRepository.HistoryTournamentsUsers;
            }
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

        public IEnumerable<TasksTestData> TasksTestData
        {
            get
            {
                return _tournamentRepository.TasksTestData;
            }
        }

        public IEnumerable<Tournament> Tournaments
        {
            get
            {
                return _tournamentRepository.Tournaments;
            }
        }

        public IEnumerable<TournamentUsers> TournamentsUsers
        {
            get
            {
                return _tournamentRepository.TournamentsUsers;
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

        public bool ConfigureSet(int Id, bool flag)
        {
            bool result = _tournamentRepository.ConfigureSet(Id, flag);

            return result;
        }

        public bool CourseSaveChanges(int tourId, int phaseId, int taskId)
        {
            bool result = _tournamentRepository.CourseSaveChanges(tourId, phaseId, taskId);

            return result;
        }

        public void Dispose()
        {
            _tournamentRepository.Dispose();
        }

        public List<Phase> GetActiveAndEndPhase(int tourId)
        {
            var activePhase = _tournamentRepository.Phase
                .Where(x => x.TournamentId == tourId)
                .Where(x => x.IsActive)
                .FirstOrDefault();

            var endPhase = _tournamentRepository.Phase
                .Where(x => x.TournamentId == tourId)
                .Where(x => x.Order < activePhase.Order)
                .ToList();

            endPhase.Add(activePhase);

            return endPhase;
        }

        public List<Tasks> GetActiveAndEndTask(int tourId)
        {
            var task = _tournamentRepository.Tasks
                .Where(x => x.TournamentId == tourId)
                .Where(x => x.IsActive || x.EndDate < DateTime.Now)
                .ToList();

            return task;
        }

        public TournamentsPagination GetTourByPage(int page)
        {
            int count = _tournamentRepository.Tournaments.Where(x => x.IsConfigured).Count();
            var tour = _tournamentRepository.Tournaments
                .Where(x => x.IsConfigured)
                .Skip((page - 1) * 5)
                .Take(5);

            TournamentsPagination result = new TournamentsPagination
            {
                CurrentItems = page,
                TotalItems = count,
                Tournaments = tour
            };

            return result;
        }

        public bool RegisterToTournament(string userId, int tournamentId)
        {
            throw new NotImplementedException();
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

        public List<string> ValidateTournament(int Id)
        {
            List<string> errorList = new List<string>();
            var tour = _tournamentRepository.Tournaments
                .Where(x => x.Id == Id)
                .FirstOrDefault();
            var phases = _tournamentRepository.Phase
                .Where(x => x.TournamentId == Id)
                .ToList();
            var tasks = _tournamentRepository.Tasks
                .Where(x => x.TournamentId == Id)
                .ToList();

            //sprawdzanie dat turnieju
            if (tour.StartDate > tour.EndDate)
                errorList.Add("Data startowa turnieju jest poźniejsza od daty końcowej.");
            else if (tour.StartDate < DateTime.Now || tour.EndDate < DateTime.Now)
                errorList.Add("Data startowa lub końcowa turnieju już mineła.");

            //fazy i zadania, testowe dane
            phases.ForEach(x =>
            {
                var taskPhase = tasks
                .Where(y => y.PhaseId == x.Id)
                .ToList();

                taskPhase.ForEach(y =>
                {
                    //sprawdzanie dat
                    var startDate = y.StartDate;
                    var endDate = y.EndDate;

                    if (startDate > endDate)
                        errorList.Add("Data startowa zadania: " + y.Title + " jest poźniejsza od daty końcowej.");
                    else if (startDate < DateTime.Now || endDate < DateTime.Now)
                        errorList.Add("Data startowa lub końcowa zadania: " + y.Title + " już mineła.");

                    if (y.Order != 1)
                    {
                        var perTask = taskPhase
                            .Where(z => z.Order == (y.Order - 1))
                            .FirstOrDefault();

                        if (perTask.StartDate > startDate)
                            errorList.Add("Data startowa zadania: " + perTask.Title + " jest poźniejsza od zadania: " + y.Title);
                        else if (perTask.EndDate > endDate)
                            errorList.Add("Data końcowa zadania: " + perTask.Title + " jest poźniejsza od zadania: " + y.Title);
                    }

                    //sprawdzanie danych testowych
                    var testData = _tournamentRepository.TasksTestData
                        .Where(z => z.TaskId == y.Id);

                    if (testData.Count() == 0)
                        errorList.Add("Zadanie: " + y.Title + " nie posiada żadnych danych testowych.");
                });
            });

            return errorList;
        }
    }
}