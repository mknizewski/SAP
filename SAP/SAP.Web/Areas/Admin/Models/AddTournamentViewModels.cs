using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

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
        [DataType(DataType.DateTime)]
        [Display(Name = "Start turnieju")]
        public DateTime StartDate { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        [Display(Name = "Koniec turnieju")]
        public DateTime EndDate { get; set; }

        [Required]
        [Display(Name = "Max czas programu (s)")]
        public double MaxExecutedTime { get; set; }

        [Required]
        [Display(Name = "Max pamięć programu (mb)")]
        public double MaxExecutedMemory { get; set; }

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
        [Display(Name = "Przykład input")]
        [DataType(DataType.MultilineText)]
        public string ExampleInput { get; set; }

        [Required]
        [Display(Name = "Przykład output")]
        [DataType(DataType.MultilineText)]
        public string ExampleOutput { get; set; }

        [Required]
        [Display(Name = "Start zadania")]
        [DataType(DataType.DateTime)]
        public DateTime StartDate { get; set; }

        [Required]
        [Display(Name = "Koniec zadania")]
        [DataType(DataType.DateTime)]
        public DateTime EndDate { get; set; }

        [Required]
        [Display(Name = "Kolejność")]
        public int Order { get; set; }
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