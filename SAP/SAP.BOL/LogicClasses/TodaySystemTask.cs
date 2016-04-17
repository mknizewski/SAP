using SAP.DAL.Abstract;
using SAP.DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SAP.BOL.LogicClasses
{
    public delegate void FlagSwitch(int Id, bool flag);
    public delegate void CountScores(int tournamentId, int phaseId);

    public class TodaySystemTask
    {
        #region PrivateFields

        private DateTime executeTime;
        private TaskType taskType;
        private int taskId;
        private int tournamentId;
        private int phaseId;
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

        public int TaskId
        {
            get { return taskId; }
            set { taskId = value; }
        }

        public int PhaseId
        {
            get { return phaseId; }
            set { phaseId = value; }
        }

        public int TournamentId
        {
            get { return tournamentId; }
            set { tournamentId = value; }
        }

        public bool Flag
        {
            get { return flag; }
            set { flag = value; }
        }

        public FlagSwitch FlagSwitch = null;
        public CountScores CountScores = null;

        #endregion PublicFields

        public bool Execute(DateTime nowTime)
        {
            nowTime = nowTime.AddTicks(-nowTime.Ticks % 10000000); //usuwamy milisekundy
            int compare = DateTime.Compare(executeTime, nowTime);

            if (compare == 0)
            {
                switch(taskType)
                {
                    case TaskType.ScoreCount:
                        CountScores(tournamentId, phaseId);
                        IsRealized = true;
                        break;
                    case TaskType.SetPromotions:
                        CountScores(tournamentId, phaseId);
                        IsRealized = true;
                        break;
                    default:
                        FlagSwitch(taskId, flag);
                        isRealized = true;
                        break;
                }
                return true;
            }
            else
                return false;
        }
    }

    public static class TodoManager
    {
        public static List<TodaySystemTask> todayTasks;
        private static ITournamentRepository _tournamentRepo;
        public static DateTime LastSynchronized;

        static TodoManager()
        {
            _tournamentRepo = new TournamentRepository();
        }

        public static void InicializeTodayTasks()
        {
            _tournamentRepo = new TournamentRepository();
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

                    task.TaskId = item.Id;
                    task.FlagSwitch += SetTournament;
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

                    task.TaskId = item.Id;
                    task.FlagSwitch += SetTournament;
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

                    activeTask.TaskId = item.Id;
                    activeTask.Flag = true;
                    activeTask.ExecuteTime = item.StartDate;
                    activeTask.FlagSwitch += SetTask;
                    activeTask.TaskType = TaskType.StartTask;

                    activePhase.TaskId = item.PhaseId;
                    activePhase.Flag = true;
                    activePhase.ExecuteTime = item.StartDate;
                    activePhase.FlagSwitch += SetPhase;
                    activePhase.TaskType = TaskType.StartPhase;

                    if (item.Phase.Order != 1)
                    {
                        var setPromo = new TodaySystemTask();

                        setPromo.TournamentId = item.TournamentId;
                        setPromo.PhaseId = item.PhaseId;
                        setPromo.ExecuteTime = item.StartDate;
                        setPromo.CountScores += SetPromotions;
                        setPromo.TaskType = TaskType.SetPromotions;

                        todayTasks.Add(setPromo);
                    }

                    todayTasks.Add(activeTask);
                    todayTasks.Add(activePhase);
                }
                else
                {
                    var activeTask = new TodaySystemTask();

                    activeTask.TaskId = item.Id;
                    activeTask.Flag = true;
                    activeTask.ExecuteTime = item.StartDate;
                    activeTask.FlagSwitch += SetTask;
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
                    var countScore = new TodaySystemTask();

                    disableTask.TaskId = item.Id;
                    disableTask.ExecuteTime = item.EndDate;
                    disableTask.Flag = false;
                    disableTask.FlagSwitch += SetTask;
                    disableTask.TaskType = TaskType.EndTask;

                    disablePhase.TaskId = item.PhaseId;
                    disablePhase.ExecuteTime = item.EndDate;
                    disablePhase.Flag = false;
                    disablePhase.FlagSwitch += SetPhase;
                    disablePhase.TaskType = TaskType.EndPhase;

                    countScore.PhaseId = item.PhaseId;
                    countScore.ExecuteTime = item.EndDate;
                    countScore.TournamentId = item.TournamentId;
                    countScore.CountScores += CountScores;
                    countScore.TaskType = TaskType.ScoreCount;

                    todayTasks.Add(disablePhase);
                    todayTasks.Add(disableTask);
                    todayTasks.Add(countScore);
                }
                else
                {
                    var disableTask = new TodaySystemTask();

                    disableTask.TaskId = item.Id;
                    disableTask.ExecuteTime = item.EndDate;
                    disableTask.Flag = false;
                    disableTask.FlagSwitch += SetTask;
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
                    todayTasks[i].Execute(now);
                }
            }
        }

        public static void SetPromotions(int tournamentId, int phaseId)
        {
            _tournamentRepo.SetPromotions(tournamentId, phaseId);
        }

        public static void CountScores(int tournamentId, int phaseId)
        {
            _tournamentRepo.CountScores(tournamentId, phaseId);
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
        EndTask,
        ScoreCount,
        SetPromotions
    }
}