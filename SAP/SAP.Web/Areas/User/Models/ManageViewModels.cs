using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SAP.Web.Areas.User.Models
{
    public class IndexViewModel
    {
        public bool HasPassword { get; set; }
        public IList<UserLoginInfo> Logins { get; set; }
        public string PhoneNumber { get; set; }
        public bool TwoFactor { get; set; }
        public bool BrowserRemembered { get; set; }
    }

    public class ManageLoginsViewModel
    {
        public IList<UserLoginInfo> CurrentLogins { get; set; }
        public IList<AuthenticationDescription> OtherLogins { get; set; }
    }

    public class FactorViewModel
    {
        public string Purpose { get; set; }
    }

    public class SetPasswordViewModel
    {
        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }

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

    public class UserChangeEmailViewModel
    {
        [Required]
        [Display(Name = "Email")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
    }


    public class UserDataViewModel
    {
        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Imię")]
        public string Name { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Nazwisko")]
        public string Surname { get; set; }

        [Display(Name = "Miasto")]
        [DataType(DataType.Text)]
        public string UserCity { get; set; }

        [Display(Name = "Ulica")]
        [DataType(DataType.Text)]
        public string UserStreet { get; set; }

        [Display(Name = "Nr d/m")]
        public string UserHouseNumber { get; set; }

        [Display(Name = "Kod pocztowy")]
        [DataType(DataType.PostalCode)]
        public string UserPostalCode { get; set; }

        [Display(Name = "Telefon")]
        [DataType(DataType.PhoneNumber)]
        public string UserPhone { get; set; }
    }

    public class UserSchoolViewModel
    {
        [Required]
        [Display(Name = "Nazwa")]
        [DataType(DataType.Text)]
        public string SchoolName { get; set; }

        [Required]
        [Display(Name = "Klasa")]
        [DataType(DataType.Text)]
        public string SchoolClass { get; set; }

        [Required]
        [Display(Name = "Miasto")]
        [DataType(DataType.Text)]
        public string SchoolCity { get; set; }

        [Display(Name = "Ulica")]
        [DataType(DataType.Text)]
        public string SchoolStreet { get; set; }

        [Display(Name = "Nr budynku")]
        public string SchoolHouseNumber { get; set; }

        [Display(Name = "Kod pocztowy")]
        [DataType(DataType.PostalCode)]
        public string SchoolPostalCode { get; set; }

        [Display(Name = "Telefon")]
        [DataType(DataType.PhoneNumber)]
        public string SchoolPhone { get; set; }
    }

    public class UserCounselorViewModel
    {
        [Display(Name = "Imię")]
        [DataType(DataType.Text)]
        public string CounselorFirstName { get; set; }

        [Display(Name = "Nazwisko")]
        [DataType(DataType.Text)]
        public string CounselorLastName { get; set; }
    }

    public class AddPhoneNumberViewModel
    {
        [Required]
        [Phone]
        [Display(Name = "Phone Number")]
        public string Number { get; set; }
    }

    public class VerifyPhoneNumberViewModel
    {
        [Required]
        [Display(Name = "Code")]
        public string Code { get; set; }

        [Required]
        [Phone]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }
    }

    public class ConfigureTwoFactorViewModel
    {
        public string SelectedProvider { get; set; }
        public ICollection<System.Web.Mvc.SelectListItem> Providers { get; set; }
    }
}