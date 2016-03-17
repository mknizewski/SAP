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

        #endregion PrivateFields

        #region PublicFields

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
            if (executeTime <= nowTime)
            {
                ExecuteTask(id, flag);
                return true;
            }
            else
                return false;
        }
    }

    public static class TodoList
    {
        private static List<TodaySystemTask> todayTasks;
        private static ITournamentRepository _tournamentRepo;

        static TodoList()
        {
            _tournamentRepo = new TournamentRepository(ApplicationDbContext.Create());
            GC.KeepAlive(_tournamentRepo);
        }

        public static void InicializeTodayTasks()
        {
            ServerConfig.SynchronizeData = true;
            todayTasks = new List<TodaySystemTask>();
            DateTime today = DateTime.Now;

            //TURNIEJE
            var startTour = _tournamentRepo.Tournaments
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

            var endTour = _tournamentRepo.Tournaments
                .Where(x => x.EndDate.Date == today.Date && x.IsConfigured)
                .Where(x => x.EndDate.TimeOfDay > today.TimeOfDay)
                .Where(x => x.IsActive == true);

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

            ServerConfig.SynchronizeData = false;
        }

        public static void Execute(DateTime now)
        {
            if (todayTasks != null)
            {
                foreach (var item in todayTasks)
                {
                    bool result = item.Execute(now);

                    if (result) // jezeli sie wykonal to usun z tablicy
                        todayTasks.Remove(item);
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