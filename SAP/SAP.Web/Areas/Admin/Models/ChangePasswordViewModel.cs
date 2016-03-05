using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SAP.Web.Areas.Admin.Models
{
    public class ChangePasswordViewModel
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Obecne hasło")]
        public string OldPassword { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "Pole {0} musi mieć przynajmniej {2} znaków długości.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Nowe hasło")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Potwierdź hasło")]
        [Compare("NewPassword", ErrorMessage = "Hasła nie pokrywają się.")]
        public string ConfirmPassword { get; set; }
    }
}