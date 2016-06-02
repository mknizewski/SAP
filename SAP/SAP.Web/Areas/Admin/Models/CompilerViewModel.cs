using System.ComponentModel.DataAnnotations;

namespace SAP.Web.Areas.Admin.Models
{
    public class CompilerViewModel
    {
        public int CSystemId { get; set; }

        [Display(Name = "C")]
        public string CPath { get; set; }

        public int CppSystemId { get; set; }

        [Display(Name = "C++")]
        public string CppPath { get; set; }

        public int PascalSystemId { get; set; }

        [Display(Name = "Pascal")]
        public string PascalPath { get; set; }

        public int JavaSystemId { get; set; }

        [Display(Name = "Java")]
        public string JavaPath { get; set; }
    }
}