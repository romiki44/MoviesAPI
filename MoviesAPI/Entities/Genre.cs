using Microsoft.EntityFrameworkCore;
using MoviesAPI.Validations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MoviesAPI.Entities
{
    [Index(nameof(Name), IsUnique =true)]
    public class Genre
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        [FirstLetterUppercase]
        public string Name { get; set; }
    }

}
