using System.ComponentModel.DataAnnotations;

namespace SAP.Web.Models
{
    public class CompilerViewModel
    {
        public string Program { get; set; }

        [Display(Name = "Wybierz kompilator")]
        public int SelectedCompiler { get; set; }
    }
}