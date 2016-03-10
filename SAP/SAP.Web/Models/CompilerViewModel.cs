using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SAP.Web.Models
{
    public class CompilerViewModel
    {
        public string Program { get; set; }
        [Display(Name = "Wybierz kompilator")]
        public int SelectedCompiler { get; set; }
    }
}