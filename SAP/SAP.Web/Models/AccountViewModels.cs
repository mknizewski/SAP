using SAP.BOL.HelperClasses.Attributes;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SAP.Web.Models
{
    public class ExternalLoginConfirmationViewModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }

    public class ExternalLoginListViewModel
    {
        public string ReturnUrl { get; set; }
    }

    public class SendCodeViewModel
    {
        public string SelectedProvider { get; set; }
        public ICollection<System.Web.Mvc.SelectListItem> Providers { get; set; }
        public string ReturnUrl { get; set; }
        public bool RememberMe { get; set; }
    }

    public class VerifyCodeViewModel
    {
        [Required]
        public string Provider { get; set; }

        [Required]
        [Display(Name = "Kod")]
        public string Code { get; set; }

        public string ReturnUrl { get; set; }

        [Display(Name = "Zapamiętać tą przeglądarkę?")]
        public bool RememberBrowser { get; set; }

        public bool RememberMe { get; set; }
    }

    public class ForgotViewModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }

    public class LoginViewModel
    {
        [Required]
        [Display(Name = "Email")]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Hasło")]
        public string Password { get; set; }

        [Display(Name = "Nie wylogowywuj mnie")]
        public bool RememberMe { get; set; }
    }

    public class RegisterViewModel
    {
        //Dane użytkownika

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Imię")]
        public string Name { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Nazwisko")]
        public string Surname { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "{0} musi mieć przynajmniej {2} znaków.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Hasło")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Potwierdz hasło")]
        [Compare("Password", ErrorMessage = "Wprowadzone hasła różnią się od siebie.")]
        public string ConfirmPassword { get; set; }

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

        //Dane szkoły

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

        //Opiekun

        [Display(Name = "Imię")]
        [DataType(DataType.Text)]
        public string CounselorFirstName { get; set; }

        [Display(Name = "Nazwisko")]
        [DataType(DataType.Text)]
        public string CounselorLastName { get; set; }

        //Zgoda na przetwarzanie danych
        [MustBeTrue(ErrorMessage = "Musisz zakceptować, by móc się zarejestrować!")]
        public bool AcceptProcessingPD { get; set; }
    }

    public class ResetPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "{0} musi mieć przynajmniej {2} znaków.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Hasło")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Potwierdź hasło")]
        [Compare("Password", ErrorMessage = "Wprowadzone hasła różnią się od siebie.")]
        public string ConfirmPassword { get; set; }

        public string Code { get; set; }
    }

    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }
}