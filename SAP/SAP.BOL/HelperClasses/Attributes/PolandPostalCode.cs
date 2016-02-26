using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace SAP.BOL.HelperClasses.Attributes
{
    /// <summary>
    /// Autor:  Mateusz Kniżewski
    /// Opis:   Klasa walidacyjna polski kod pocztowy
    /// Data:   07.11.15
    /// </summary>
    public class PolandPostalCode : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value != null)
            {
                string postalCode = value.ToString();

                if (postalCode.Contains('-'))
                {
                    var split = postalCode.Split(new char[] { '-' });

                    if (split.Length == 2)
                    {
                        if (split[0].Length != 2 || split[1].Length != 3)
                            return new ValidationResult("Nieprawidłowy kod pocztowy!");
                    }
                    else
                        return new ValidationResult("Nieprawidłowy kod pocztowy!");
                }
                else
                    return new ValidationResult("Nieprawidłowy kod pocztowy!");
            }

            return ValidationResult.Success;
        }
    }
}