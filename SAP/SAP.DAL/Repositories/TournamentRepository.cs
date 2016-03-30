using SAP.DAL.Abstract;
using SAP.DAL.DbContext;
using SAP.DAL.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SAP.DAL.Repositories
{
    public class TournamentRepository : ITournamentRepository, IDisposable
    {
        private ApplicationDbContext _context;

        public TournamentRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<HistoryTournamentUsers> HistoryTournamentsUsers
        {
            get
            {
                return _context.HistoryTournamentUsers;
            }
        }

        public IEnumerable<Phase> Phase
        {
            get
            {
                return _context.Phase;
            }
        }

        public IEnumerable<Tasks> Tasks
        {
            get
            {
                return _context.Tasks;
            }
        }

        public IEnumerable<TasksTestData> TasksTestData
        {
            get
            {
                return _context.TasksTestData;
            }
        }

        public IEnumerable<Tournament> Tournaments
        {
            get
            {
                return _context.Tournament;
            }
        }

        public IEnumerable<TournamentUsers> TournamentsUsers
        {
            get
            {
                return _context.TournamentUsers;
            }
        }

        public bool AddTaskTestingData(TasksTestData testData)
        {
            try
            {
                _context.TasksTestData.Add(testData);
                _context.SaveChanges();

                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> AddTestDataAsync(List<TasksTestData> testData)
        {
            try
            {
                foreach (var item in testData)
                    _context.TasksTestData.Add(item);

                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> AddTournamentAsync(Tournament tour, List<Phase> phases, List<Tasks> tasks, int[] taskMetadata)
        {
            try
            {
                var tournamentDb = tour;

                _context.Tournament.Add(tournamentDb);
                await _context.SaveChangesAsync();

                phases.ForEach(x =>
                {
                    x.TournamentId = tournamentDb.Id;
                    _context.Phase.Add(x);
                });

                await _context.SaveChangesAsync();

                int pom = 0;
                for (int i = 0; i < taskMetadata.Length; i++)
                {
                    var phase = phases[i];
                    int len = taskMetadata[i];

                    for (int j = 0; j < len; j++)
                    {
                        var task = tasks[pom];
                        task.TournamentId = tournamentDb.Id;
                        task.PhaseId = phase.Id;

                        pom++;
                        _context.Tasks.Add(task);
                    }
                }

                await _context.SaveChangesAsync();
                return true;
            }
            catch { return false; }
        }

        public bool ConfigureSet(int Id, bool flag)
        {
            try
            {
                var tour = _context.Tournament.Find(Id);
                tour.IsConfigured = flag;
                _context.SaveChanges();

                return true;
            }
            catch
            { return false; }
        }

        public bool DeleteTournament(int tournamentId)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        public bool RegisterToTournament(string userId, int tournamentId)
        {
            try
            {
                int maxUsers = _context.Tournament.Find(tournamentId).MaxUsers;
                int registered = _context.TournamentUsers.
                    Where(x => x.TournamentId == tournamentId)
                    .Count();

                if (registered < maxUsers)
                {
                    var tourModel = new TournamentUsers
                    {
                        TournamentId = tournamentId,
                        UserId = userId
                    };

                    _context.TournamentUsers.Add(tourModel);
                    _context.SaveChanges();

                    return true;
                }
                else
                    return false;

            }
            catch { return false; }
        }

        public void SetPhaseActiveFlag(int Id, bool flag)
        {
            var phase = _context.Phase.Find(Id);
            phase.IsActive = flag;

            _context.SaveChanges();
        }

        public void SetTaskActiveFlag(int Id, bool flag)
        {
            var task = _context.Tasks.Find(Id);
            task.IsActive = flag;

            _context.SaveChanges();
        }

        public void SetTournamentActiveFlag(int Id, bool flag)
        {
            var tour = _context.Tournament.Find(Id);
            tour.IsActive = flag;

            _context.SaveChanges();
        }
    }
}