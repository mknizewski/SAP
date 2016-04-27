using System;
using System.Collections.Generic;

namespace SAP.Web.Areas.Admin.Models
{
    public class ManageTourViewModel
    {
        public int TournamentId { get; set; }
        public string Title { get; set; }
        public bool IsActive { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public List<ManagePhaseViewModel> Phases { get; set; }
    }

    public class ManagePhaseViewModel
    {
        public int PhaseId { get; set; }
        public bool IsActive { get; set; }
        public string Title { get; set; }
        public List<MTaskViewModel> Tasks { get; set; }
    }

    public class MTaskViewModel
    {
        public int TaskId { get; set; }
        public string Title { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsActive { get; set; }
    }
}