using MoviesAPI.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MoviesAPI.Validations
{
    public class FirstLetterUppercaseAttribute: ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            // ValidationContext umoznuje dostat sa k celemu objektu/modelu, v ramci ktoreho chcem validovat
            //var genre=(Genre) validationContext.ObjectInstance;

            // vacsinou ale nie sme zamerani na konkretny objekt
            // kontretne teraz iba chceme iba vseobecne pravislo, aby prve pismeno bolo velke!
            if (value == null || string.IsNullOrEmpty(value.ToString()))
                return ValidationResult.Success;

            var firstLetter = value.ToString()[0].ToString();  // inac by bolo char!
            if (firstLetter != firstLetter.ToUpper())
                return new ValidationResult("First letter should be uppercase");

            return ValidationResult.Success;
        }
    }
}
