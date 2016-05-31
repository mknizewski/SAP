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

        public TournamentRepository()
        {
            _context = ApplicationDbContext.Create();
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

        public void CountScores(int tournamentId, int phaseId)
        {
            var solutions = _context.UserSolutions
                .Where(x => x.TournamentId == tournamentId)
                .Where(x => x.PhaseId == phaseId)
                .ToList();

            var users = solutions
                .Select(x => x.UserId)
                .Distinct()
                .ToList();

            var tasks = _context.Tasks
                .Where(x => x.PhaseId == phaseId)
                .Where(x => x.TournamentId == tournamentId)
                .Select(x => x.Id)
                .ToList();

            //zapisujemy do bazy wyniki z fazy
            users.ForEach(x =>
            {
                decimal totalScore = 0;

                tasks.ForEach(y =>
                {
                    var userSolution = solutions
                        .Where(z => z.TaskId == y)
                        .Where(z => z.UserId == x)
                        .Where(z => z.Error == null || z.Error.Equals(""))
                        .OrderByDescending(z => z.InsertTime)
                        .FirstOrDefault();

                    if (userSolution != null)
                        totalScore += userSolution.Score;
                });

                var score = new Scores
                {
                    UserId = x,
                    TournamentId = tournamentId,
                    PhaseId = phaseId,
                    TotalScore = totalScore
                };

                _context.Scores.Add(score);
            });

            _context.SaveChanges();
        }

        public bool CourseSaveChanges(int tourId, int phaseId, int taskId)
        {
            try
            {
                var phases = _context.Phase
                 .Where(x => x.TournamentId == tourId)
                 .ToList();

                phases.ForEach(x =>
                {
                    if (x.Id == phaseId)
                        x.IsActive = true;
                    else
                        x.IsActive = false;
                });

                var tasks = _context.Tasks
                    .Where(x => x.TournamentId == tourId)
                    .ToList();

                tasks.ForEach(x =>
                {
                    if (x.Id == taskId)
                        x.IsActive = true;
                    else
                        x.IsActive = false;
                });

                _context.SaveChanges();
                return true;
            }
            catch
            { return false; }
        }

        public bool DeleteTestData(int Id)
        {
            try
            {
                var model = _context.TasksTestData.Find(Id);

                _context.TasksTestData.Remove(model);
                _context.SaveChanges();

                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool DeleteTournament(int tournamentId)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        public bool EditTask(Tasks model)
        {
            try
            {
                var dbModel = _context.Tasks.Find(model.Id);

                dbModel.Title = model.Title;
                dbModel.Description = model.Description;
                dbModel.StartDate = model.StartDate;
                dbModel.EndDate = model.EndDate;
                dbModel.Input = model.Input;
                dbModel.Output = model.Output;
                dbModel.MaxExecuteMemory = model.MaxExecuteMemory;
                dbModel.MaxExecuteTime = model.MaxExecuteTime;
                dbModel.Example = model.Example;
                dbModel.InputDataTypeId = model.InputDataTypeId;

                if (model.PDF != null)
                    dbModel.PDF = model.PDF;

                _context.SaveChanges();
                return true;
            }
            catch { return false; }
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
            var phase = _context.Phase
                .Where(x => x.Id == Id)
                .FirstOrDefault();

            phase.IsActive = flag;

            _context.SaveChanges();
        }

        public void SetPromotions(int tournamentId, int phaseId)
        {
            phaseId++;
            var phase = _context.Phase.Find(phaseId);
            var maxUserInPhase = phase.MaxUsers;

            var pervPhaseId = _context.Phase
                .Where(x => x.TournamentId == tournamentId)
                .Where(x => x.Order == (phase.Order - 1))
                .Select(x => x.Id)
                .FirstOrDefault();

            //sortujemy wyniki
            var allScoresInPervPhase = _context.Scores
                .Where(x => x.PhaseId == pervPhaseId)
                .OrderByDescending(x => x.TotalScore)
                .ToList();

            //lista osob nieawansujacych - pomijamy osoby które awansowały
            var userNotAllowed = allScoresInPervPhase
                .Skip(maxUserInPhase)
                .ToList();

            //osoby nieawansujące przenosimy do tabeli historycznej i usuwamy rekord w aktualnej tabeli
            userNotAllowed.ForEach(x =>
            {
                var tournamentsUserRow = _context.TournamentUsers
                    .Where(y => y.UserId == x.UserId)
                    .FirstOrDefault();

                var historyTournamentUser = new HistoryTournamentUsers
                {
                    OldId = tournamentsUserRow.Id,
                    TournamentId = x.TournamentId,
                    PhaseId = x.PhaseId,
                    UserId = x.UserId
                };

                _context.HistoryTournamentUsers.Add(historyTournamentUser);
                _context.TournamentUsers.Remove(tournamentsUserRow);
            });

            //przenosimy wyniki do tabelki historycznej i uswamy rekody z aktualnej tabelki
            allScoresInPervPhase.ForEach(x =>
            {
                var historyScores = new HistoryScores
                {
                    OldId = x.Id,
                    PhaseId = x.PhaseId,
                    TournamentId = x.TournamentId,
                    UserId = x.UserId,
                    TotalScore = x.TotalScore
                };

                _context.HistoryScores.Add(historyScores);
                _context.Scores.Remove(x);
            });

            _context.SaveChanges();
        }

        public void SetTaskActiveFlag(int Id, bool flag)
        {
            var task = _context.Tasks
                .Where(x => x.Id == Id)
                .FirstOrDefault();

            task.IsActive = flag;

            _context.SaveChanges();
        }

        public void SetTournamentActiveFlag(int Id, bool flag)
        {
            var tour = _context.Tournament
                .Where(x => x.Id == Id)
                .FirstOrDefault();

            tour.IsActive = flag;

            _context.SaveChanges();
        }
    }
}