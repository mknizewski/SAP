using SAP.Web.Areas.Tournament.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SAP.Web.Areas.Tournament.Models
{
    public class HomeTournamentViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int MaxUsers { get; set; }
        public int PhaseCount { get; set; }
        public TournamentStatus Status { get; set; }
    }
}