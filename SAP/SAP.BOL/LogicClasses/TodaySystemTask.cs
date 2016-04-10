using SAP.DAL.Abstract;
using SAP.DAL.DbContext;
using SAP.DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SAP.BOL.LogicClasses
{
    public delegate void ExecuteTask(int Id, bool flag);

    public class TodaySystemTask
    {
        #region PrivateFields

        private DateTime executeTime;
        private TaskType taskType;
        private int id;
        private bool flag;
        private bool isRealized = false;

        #endregion PrivateFields

        #region PublicFields

        public bool IsRealized
        {
            get { return isRealized; }
            set { isRealized = value; }
        }

        public DateTime ExecuteTime
        {
            get { return executeTime; }
            set { executeTime = value; }
        }

        public TaskType TaskType
        {
            get { return taskType; }
            set { taskType = value; }
        }

        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        public bool Flag
        {
            get { return flag; }
            set { flag = value; }
        }

        public ExecuteTask ExecuteTask = null;

        #endregion PublicFields

        public bool Execute(DateTime nowTime)
        {
            nowTime = nowTime.AddTicks(-nowTime.Ticks % 10000000); //usuwamy milisekundy
            int compare = DateTime.Compare(executeTime, nowTime);

            if (compare == 0)
            {
                ExecuteTask(id, flag);
                isRealized = true;
                return true;
            }
            else
                return false;
        }
    }

    public static class TodoList
    {
        public static List<TodaySystemTask> todayTasks;
        private static ITournamentRepository _tournamentRepo;
        public static DateTime LastSynchronized;

        static TodoList()
        {
            _tournamentRepo = new TournamentRepository(ApplicationDbContext.Create());
            GC.KeepAlive(_tournamentRepo);
        }

        public static void InicializeTodayTasks()
        {
            _tournamentRepo = new TournamentRepository(ApplicationDbContext.Create());
            ServerConfig.SynchronizeData = true;
            todayTasks = new List<TodaySystemTask>();
            DateTime today = DateTime.Now;
            LastSynchronized = today;

            //TURNIEJE START
            var startTour = _tournamentRepo.Tournaments
                .Where(x => !x.IsActive)
                .Where(x => x.StartDate.Date == today.Date && x.IsConfigured)
                .Where(x => x.StartDate.TimeOfDay > today.TimeOfDay);

            if (startTour != null)
            {
                foreach (var item in startTour)
                {
                    var task = new TodaySystemTask();

                    task.Id = item.Id;
                    task.ExecuteTask += SetTournament;
                    task.ExecuteTime = item.StartDate;
                    task.Flag = true;
                    task.TaskType = TaskType.StartTournament;

                    todayTasks.Add(task);
                }
            }

            //TURNIEJE STOP
            var endTour = _tournamentRepo.Tournaments
                .Where(x => x.EndDate.Date == today.Date && x.IsConfigured)
                .Where(x => x.EndDate.TimeOfDay > today.TimeOfDay)
                .Where(x => x.IsActive);

            if (endTour != null)
            {
                foreach (var item in endTour)
                {
                    var task = new TodaySystemTask();

                    task.Id = item.Id;
                    task.ExecuteTask += SetTournament;
                    task.ExecuteTime = item.EndDate;
                    task.Flag = false;
                    task.TaskType = TaskType.EndTournament;

                    todayTasks.Add(task);
                }
            }

            //ZADANIA START
            var startTasks = _tournamentRepo.Tasks
                .Where(x => x.StartDate.Date == today.Date)
                .Where(x => x.Tournament.IsActive);

            foreach (var item in startTasks)
            {
                if (item.Order == 1)
                {
                    var activePhase = new TodaySystemTask();
                    var activeTask = new TodaySystemTask();

                    activeTask.Id = item.Id;
                    activeTask.Flag = true;
                    activeTask.ExecuteTime = item.StartDate;
                    activeTask.ExecuteTask += SetTask;
                    activeTask.TaskType = TaskType.StartTask;

                    activePhase.Id = item.PhaseId;
                    activePhase.Flag = true;
                    activePhase.ExecuteTime = item.StartDate;
                    activePhase.ExecuteTask += SetPhase;
                    activePhase.TaskType = TaskType.StartPhase;

                    todayTasks.Add(activeTask);
                    todayTasks.Add(activePhase);
                }
                else
                {
                    var activeTask = new TodaySystemTask();

                    activeTask.Id = item.Id;
                    activeTask.Flag = true;
                    activeTask.ExecuteTime = item.StartDate;
                    activeTask.ExecuteTask += SetTask;
                    activeTask.TaskType = TaskType.StartTask;

                    todayTasks.Add(activeTask);
                }
            }

            //ZADANIA STOP
            var endTask = _tournamentRepo.Tasks
                .Where(x => x.EndDate.Date == today.Date)
                .Where(x => x.Tournament.IsActive);

            foreach (var item in endTask)
            {
                if (item.Order == item.Phase.MaxTasks)
                {
                    var disableTask = new TodaySystemTask();
                    var disablePhase = new TodaySystemTask();

                    disableTask.Id = item.Id;
                    disableTask.ExecuteTime = item.EndDate;
                    disableTask.Flag = false;
                    disableTask.ExecuteTask += SetTask;
                    disableTask.TaskType = TaskType.EndTask;

                    disablePhase.Id = item.PhaseId;
                    disablePhase.ExecuteTime = item.EndDate;
                    disablePhase.Flag = false;
                    disablePhase.ExecuteTask += SetPhase;
                    disablePhase.TaskType = TaskType.EndPhase;

                    //TODO: Opracować algorytm zliczania puntków na koniec fazy

                    todayTasks.Add(disablePhase);
                    todayTasks.Add(disableTask);
                }
                else
                {
                    var disableTask = new TodaySystemTask();

                    disableTask.Id = item.Id;
                    disableTask.ExecuteTime = item.EndDate;
                    disableTask.Flag = false;
                    disableTask.ExecuteTask += SetTask;
                    disableTask.TaskType = TaskType.EndTask;

                    todayTasks.Add(disableTask);
                }
            }

            ServerConfig.SynchronizeData = false;
        }

        public static void Execute(DateTime now)
        {
            if (todayTasks != null)
            {
                for (int i = 0; i < todayTasks.Count; i++)
                {
                    bool result = todayTasks[i].Execute(now);
                }
            }
        }

        public static void SetPhase(int id, bool flag)
        {
            _tournamentRepo.SetPhaseActiveFlag(id, flag);
        }

        public static void SetTournament(int id, bool flag)
        {
            _tournamentRepo.SetTournamentActiveFlag(id, flag);
        }

        public static void SetTask(int id, bool flag)
        {
            _tournamentRepo.SetTaskActiveFlag(id, flag);
        }
    }

    public enum TaskType
    {
        StartPhase,
        EndPhase,
        StartTournament,
        EndTournament,
        StartTask,
        EndTask
    }
}