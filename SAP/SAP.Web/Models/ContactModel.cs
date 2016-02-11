using System.ComponentModel.DataAnnotations;

namespace SAP.Web.Models
{
    public class ContactModel
    {
        [Required(ErrorMessage = "Imię jest wymagane!")]
        [DataType(DataType.Text)]
        [Display(Name = "Imię")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Nazwisko jest wymagane!")]
        [DataType(DataType.Text)]
        [Display(Name = "Nazwisko")]
        public string Surname { get; set; }

        [EmailAddress]
        [Required(ErrorMessage = "Musisz podać adres, by móc dostać odpowiedź!")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Nie możesz wysłać pustej wiadomosći!")]
        [DataType(DataType.MultilineText)]
        [Display(Name = "Wiadomość")]
        public string Message { get; set; }
    }
}