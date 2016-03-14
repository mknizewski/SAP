using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SAP.Web.Areas.Admin.Models
{
    public class TestDataViewModel
    {
        public int TaskId { get; set; }
        public int SystemInputDataId { get; set; }

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