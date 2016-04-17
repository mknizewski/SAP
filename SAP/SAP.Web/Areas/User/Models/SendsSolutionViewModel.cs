using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SAP.Web.Areas.User.Models
{
    public class SendsSolutionViewModel
    {
        public int SolutionId { get; set; }
        public string TournamentTitle { get; set; }
        public string TaskTitle { get; set; }
        public double MemUsage { get; set; }
        public double TimeUsage { get; set; }
        public DateTime InsertTime { get; set; }
        public bool IsAccepted { get; set; }
        public string Lang { get; set; }
        public string Error { get; set; }
    }
}