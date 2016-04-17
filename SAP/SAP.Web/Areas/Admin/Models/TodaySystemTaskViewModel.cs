using SAP.BOL.LogicClasses;
using System;

namespace SAP.Web.Areas.Admin.Models
{
    public class TodaySystemTaskViewModel
    {
        public int TournamentId { get; set; }
        public int PhaseId { get; set; }
        public int TaskId { get; set; }
        public DateTime ExecuteTime { get; set; }
        public string TypeOfTask { get; set; }
        public bool IsRealized { get; set; }
        public TaskType TaskType { get; set; }
    }
}