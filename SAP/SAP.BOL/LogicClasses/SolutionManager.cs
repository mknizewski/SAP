using SAP.BOL.Abstract;
using SAP.BOL.LogicClasses.Managers;
using SAP.DAL.Abstract;
using SAP.DAL.Repositories;
using SAP.DAL.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SAP.BOL.LogicClasses
{
    /// <summary>
    /// Klasa kompilująca, sprawdzajaca i testująca solucje uzytkownikow
    /// </summary>
    public class SolutionManager : IDisposable
    {
        private string program;
        private string userId;
        private int taskId;
        private CompilerType compilerType;
        private IProgramManager _programManager;
        private IUserManager _userManager;
        private ITournamentManager _tournamentManager;

        public string JavaMainClass { get; set; }

        public SolutionManager(string program, string userId, int taskId, CompilerType compilerType)
        {
            this.program = program;
            this.userId = userId;
            this.taskId = taskId;
            this.compilerType = compilerType;
        }

        public SolutionManager(byte[] file, string userId, int taskId, CompilerType compilerType)
        {
            program = Encoding.UTF8.GetString(file); //zmiana bitów na string
            this.userId = userId;
            this.taskId = taskId;
            this.compilerType = compilerType;
        }

        public void IniclizeManagers()
        {
            IUserRepository userRepo = new UserRepository();
            ITournamentRepository tourRepo = new TournamentRepository();

            _programManager = new ProgramManager();
            _tournamentManager = new TournamentManager(tourRepo);
            _userManager = new UserManager(userRepo);
        }

        public void CheckSolution()
        {
            List<double> allCpuTime = new List<double>();
            List<double> allMemUsage = new List<double>();

            Tasks task = _tournamentManager.Tasks
                .Where(x => x.Id == taskId)
                .FirstOrDefault();

            List<TasksTestData> testData = _tournamentManager.TasksTestData
                .Where(x => x.TaskId == taskId)
                .ToList();

            //Przygotowanie program managera to działania
            _programManager.Program = program;
            _programManager.Language = compilerType;
            _programManager.InputDataType = (InputDataType)task.InputDataTypeId;
            _programManager.MaxTime = task.MaxExecuteTime;
            _programManager.MaxMemory = task.MaxExecuteMemory;
            _programManager.JavaMainClass = JavaMainClass;

            //Działanie program managera
            try
            {
                //Kompilacja
                _programManager.Compile();

                if (_programManager.HasError) //w przypadku erroru - koniec działania
                {
                    _userManager.AddSolution(task.Id, task.TournamentId, task.PhaseId, userId, (int)compilerType, 0, program, task.MaxExecuteMemory, task.MaxExecuteTime, _programManager.ErrorInfo);
                    return;
                }

                foreach (var item in testData) // uruchamiamy i testujemy program
                {
                    _programManager.InputData = item.InputData;
                    _programManager.Execute();

                    if (!_programManager.HasError)
                    {
                        string output = BlankChars.Remove(_programManager.OutputData);
                        if (output == item.OutputData)
                        {
                            allCpuTime.Add(_programManager.ExecutedTime * 0.001); //zamieniamy ms na s
                            allMemUsage.Add(_programManager.MemoryUsed);
                        }
                        else
                        {
                            _userManager.AddSolution(task.Id, task.TournamentId, task.PhaseId, userId, (int)compilerType, 0, program, task.MaxExecuteMemory, task.MaxExecuteTime, _programManager.ErrorInfo);
                            return;
                        }
                    }
                    else
                    {
                        _userManager.AddSolution(task.Id, task.TournamentId, task.PhaseId, userId, (int)compilerType, 0, program, task.MaxExecuteMemory, task.MaxExecuteTime, _programManager.ErrorInfo);
                        return;
                    }
                }

                double avgCpuTime = 0.0;
                double avgMemUsage = 0.0;

                allCpuTime.ForEach(x => avgCpuTime += x);
                allMemUsage.ForEach(x => avgMemUsage += x);

                avgCpuTime = avgCpuTime / allCpuTime.Count;
                avgMemUsage = avgMemUsage / allMemUsage.Count;

                _userManager.AddSolution(task.Id, task.TournamentId, task.PhaseId, userId, (int)compilerType, 5, program, avgMemUsage, avgCpuTime, _programManager.ErrorInfo);
            }
            catch
            {
                _userManager.AddSolution(task.Id, task.TournamentId, task.PhaseId, userId, (int)compilerType, 0, program, task.MaxExecuteMemory, task.MaxExecuteTime, _programManager.ErrorInfo);
            }

            this.Dispose();
        }

        public void Dispose()
        {
            _userManager.Dispose();
            _userManager = null;

            _programManager.Dispose();
            _programManager = null;

            _tournamentManager.Dispose();
            _tournamentManager = null;
        }
    }
}