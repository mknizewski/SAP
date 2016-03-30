using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SAP.Web.Areas.Tournament.Models
{
    public class SolutionViewModel
    {
        public int TaskId { get; set; }
        public string Program { get; set; }
        public int SelectedLang { get; set; }
    }
}