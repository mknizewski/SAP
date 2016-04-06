using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web;
using System.Web.Mvc;

namespace SAP.Web.Areas.Admin.Models
{
    public class TournamentViewModel
    {
        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Tytuł")]
        public string Title { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name = "Opis turnieju")]
        public string Description { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Start turnieju")]
        public DateTime StartDate { get; set; }

        [Required]
        [DataType(DataType.Time)]
        public TimeSpan StartTime { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Koniec turnieju")]
        public DateTime EndDate { get; set; }

        [Required]
        [DataType(DataType.Time)]
        public TimeSpan EndTime { get; set; }

        [Required]
        [Display(Name = "Max uczestników")]
        public int MaxUsers { get; set; }

        [Required]
        [Display(Name = "Ilość faz")]
        public int PhaseCount { get; set; }
    }

    public class PhaseViewModel
    {
        [Required]
        [Display(Name = "Nazwa")]
        [DataType(DataType.Text)]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Kolejność")]
        public int Order { get; set; }

        [Required]
        [Display(Name = "Max uczestników")]
        public int MaxUsers { get; set; }

        [Required]
        [Display(Name = "Ilość zadań")]
        public int TaskCount { get; set; }
    }

    public class TaskViewModel
    {
        public int PhaseId { get; set; }

        [Required]
        [Display(Name = "Tytuł zadania")]
        [DataType(DataType.Text)]
        public string Title { get; set; }

        [Required]
        [Display(Name = "Opis zadania")]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [Required]
        [Display(Name = "Input")]
        [DataType(DataType.MultilineText)]
        public string Input { get; set; }

        [Required]
        [Display(Name = "Output")]
        [DataType(DataType.MultilineText)]
        public string Output { get; set; }

        [Required]
        [Display(Name = "Przykład")]
        [DataType(DataType.MultilineText)]
        public string Example { get; set; }

        [Required]
        [Display(Name = "Czas programu(s)")]
        public double MaxExecutedTime { get; set; }

        [Required]
        [Display(Name = "Pamięc programu(mb)")]
        public double MaxExecutedMemory { get; set; }

        [Display(Name = "Zadanie w PDF")]
        [RegularExpression(@"^(([a-zA-Z]:)|(\\{2}\w+)\$?)(\\(\w[\w].*))(.pdf|.PDF)$", ErrorMessage = "Nieprawidłowy plik")]
        [DataType(DataType.Upload)]
        public HttpPostedFileBase PDF { get; set; }

        [Required]
        [Display(Name = "Start zadania")]
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }

        [Required]
        [DataType(DataType.Time)]
        public TimeSpan StartTime { get; set; }

        [Required]
        [Display(Name = "Koniec zadania")]
        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; }

        [Required]
        [DataType(DataType.Time)]
        public TimeSpan EndTime { get; set; }

        [Required]
        [Display(Name = "Kolejność")]
        public int Order { get; set; }

        [Required]
        public int InputDataId { get; set; }

        [Display(Name = "Typ wprowadzania danych")]
        public IEnumerable<SelectListItem> InputData { get; set; }
    }

    public class PhaseTaskContainer
    {
        public List<TaskViewModel> Tasks { get; set; }
    }

    public class AddTournamentViewModel
    {
        public TournamentViewModel Tournament { get; set; }
        public List<PhaseViewModel> Phases { get; set; }
        public List<PhaseTaskContainer> TaskContainer { get; set; }
    }
}