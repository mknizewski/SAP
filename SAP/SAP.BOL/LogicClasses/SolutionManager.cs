using SAP.BOL.Abstract;
using SAP.DAL.Tables;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SAP.BOL.LogicClasses
{
    /// <summary>
    /// Klasa kompilująca, sprawdzajaca i testująca solucje uzytkownikow
    /// </summary>
    public class SolutionManager
    {
        private string program;
        private string userId;
        private int taskId;
        private CompilerType compilerType;

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

        public void CheckSolution(IProgramManager programManager, IUserManager userManager, ITournamentManager tourManager)
        {
            //Przygotowanie danych
            var _programManager = programManager;
            var _userManager = userManager;
            var _tournamentManager = tourManager;

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

            //Działanie program managera
            try
            {
                //Kompilacja
                _programManager.Compile();

                if (_programManager.HasError) //w przypadku erroru - koniec działania
                {
                    _userManager.AddSolution(task.Id, task.TournamentId, userId, (int)compilerType, 0, program, task.MaxExecuteMemory, task.MaxExecuteTime);
                    return;
                }

                foreach (var item in testData) // uruchamiamy i testujemy program
                {
                    _programManager.InputData = item.InputData;
                    _programManager.Execute();

                    if (!_programManager.HasError)
                    {
                        if (_programManager.OutputData == item.OutputData)
                        {
                            allCpuTime.Add(_programManager.ExecutedTime * 0.001); //zamieniamy ms na s
                            allMemUsage.Add(_programManager.MemoryUsed);
                        }
                        else
                        {
                            _userManager.AddSolution(task.Id, task.TournamentId, userId, (int)compilerType, 0, program, task.MaxExecuteMemory, task.MaxExecuteTime);
                            return;
                        }
                    }
                    else
                    {
                        _userManager.AddSolution(task.Id, task.TournamentId, userId, (int)compilerType, 0, program, task.MaxExecuteMemory, task.MaxExecuteTime);
                        return;
                    }
                }

                double avgCpuTime = 0.0;
                double avgMemUsage = 0.0;

                allCpuTime.ForEach(x => avgCpuTime += x);
                allMemUsage.ForEach(x => avgMemUsage += x);

                avgCpuTime = avgCpuTime / allCpuTime.Count;
                avgMemUsage = avgMemUsage / allMemUsage.Count;

                _userManager.AddSolution(task.Id, task.TournamentId, userId, (int)compilerType, 5, program, avgMemUsage, avgCpuTime);
            }
            catch
            {
                _userManager.AddSolution(task.Id, task.TournamentId, userId, (int)compilerType, 0, program, task.MaxExecuteMemory, task.MaxExecuteTime);
            }
        }
    }
}