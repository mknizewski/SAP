using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SAP.Web.Areas.Admin.Models
{
    public class ScoresViewModel
    {
        public PhaseScoresViewModel CurrentPhase { get; set; }
        public List<PhaseScoresViewModel> HistoryPhases { get; set; }
    }

    public class PhaseScoresViewModel
    {
        public string PhaseTitle { get; set; }
        public List<PersonScoreViewModel> Scores { get; set; }
    }

    public class PersonScoreViewModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public decimal Score { get; set; }
    }
}