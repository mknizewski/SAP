using System;
using System.Collections.Generic;

namespace SAP.Web.Areas.User.Models
{
    public class TournamentsIndexViewModel
    {
        public List<ActualTournamentsViewModel> ActualTour { get; set; }
        public List<HistoryTournamentsViewModel> HistoryTour { get; set; }
    }

    public class ActualTournamentsViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
    }

    public class HistoryTournamentsViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Phase { get; set; }
    }
}