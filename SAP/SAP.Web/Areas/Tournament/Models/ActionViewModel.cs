using System;
using System.Collections.Generic;

namespace SAP.Web.Areas.Tournament.Models
{
    public class ActionViewModel
    {
        public string Title { get; set; }
        public List<PhasesViewModel> Phases { get; set; }
    }

    public class PhasesViewModel
    {
        public string Title { get; set; }
        public List<TasksViewModel> Tasks { get; set; }
    }

    public class TasksViewModel
    {
        public int Id { get; set; }
        public bool IsActive { get; set; }
        public string Title { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}