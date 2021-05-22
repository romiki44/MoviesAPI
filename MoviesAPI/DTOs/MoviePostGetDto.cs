using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MoviesAPI.DTOs
{
    public class MoviePostGetDto
    {
        public List<GenreDto> Genres { get; set; }
        public List<MovieTheaterDto> MovieTheaters { get; set; }
    }
}
