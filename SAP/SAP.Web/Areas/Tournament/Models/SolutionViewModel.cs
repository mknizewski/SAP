using System.ComponentModel.DataAnnotations;
using System.Web;

namespace SAP.Web.Areas.Tournament.Models
{
    public class SolutionViewModel
    {
        public int TaskId { get; set; }
        public string Program { get; set; }
        public HttpPostedFileBase File { get; set; }

        [Required(ErrorMessage = "Musisz wybrac język rozwiązania")]
        [Display(Name = "Wybierz język")]
        public int SelectedLang { get; set; }

        [Display(Name = "Nazwa klasy main")]
        public string JavaMainClassName { get; set; }
    }
}