﻿using System.ComponentModel.DataAnnotations;

namespace SAP.Web.Areas.Admin.Models
{
    public class TestDataViewModel
    {
        public int TaskId { get; set; }

        public int Id { get; set; }

        [Required]
        [DataType(DataType.MultilineText)]
        [Display(Name = "Dane wyjścia")]
        public string OutputData { get; set; }

        [Required]
        [DataType(DataType.MultilineText)]
        [Display(Name = "Dane wejścia")]
        public string InputData { get; set; }
    }
}