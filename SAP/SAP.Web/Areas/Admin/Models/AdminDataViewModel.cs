using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SAP.Web.Areas.Admin.Models
{
    public class AdminDataViewModel
    {
        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Imię")]
        public string FirstName { get; set; }
        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Nazwisko")]
        public string LastName { get; set; }
        [Required]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email")]
        public string Email { get; set; }
        public string Role { get; set; }
    }
}