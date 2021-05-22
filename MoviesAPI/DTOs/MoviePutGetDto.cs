using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MoviesAPI.DTOs
{
    public class MoviePutGetDto
    {
        public MovieDto Movie { get; set; }
        public List<GenreDto> SelectedGenres { get; set; }
        public List<GenreDto> NonSelectedGenres { get; set; }
        public List<MovieTheaterDto> SelectedMovieTheaters { get; set; }
        public List<MovieTheaterDto> NonSelectedMovieTheaters { get; set; }
        public List<ActorMovieDto> Actors { get; set; }
    }
}
