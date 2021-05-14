using MoviesAPI.Validations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MoviesAPI.Entities
{
    public class Genre: IValidatableObject //druha moznost Model Validation!!!
    {
        public int Id { get; set; }

        // priklady validacie cez atributy
        [Required(ErrorMessage ="Pole {0} je povinné")]
        [StringLength(10)]
        //[FirstLetterUppercase]  //prva moznost - CustomAttribute Validation!!
        public string Name { get; set; }

        [Range(18,60)]
        public int Age { get; set; }
        
        [CreditCard]
        public string CreditCard { get; set; }
        
        [Url]
        public string Url { get; set; }

        // Model Validation: IEnumerable - vraciame tolko validacii, kolko chceme
        // nevyhoda - plati iba pre konkretny model, ak chceme pouzit tu istu validaciu inde,
        // musime ju napisat znova pri novom modeli
        // vyhoda AttributeValidation - vseobecne pravidlo sa napiose raz a pouzije sa lubovolne vela krat!! 
        // okrem toho ModelValidation sa aplikuje AZ PO! AttributeValidation, tzn. 
        // ak mam porusenie attributevalidation, zobrazi sa iba tato chyba, az ked prejde ok attributevalidation
        // az potom sa aplikuje ModelValidation!!!
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if(!string.IsNullOrEmpty(Name))
            {
                var firstLetter = Name[0].ToString();
                if (firstLetter != firstLetter.ToUpper())
                    yield return new ValidationResult("First letter should be uppercase", new string[] { nameof(Name)});

            }
        }
    }

}
