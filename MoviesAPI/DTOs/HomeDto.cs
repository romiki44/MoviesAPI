using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MoviesAPI.DTOs
{
    public class HomeDto
    {
        public List<MovieDto> InTheaters { get; set; }
        public List<MovieDto> UpcomingReleases { get; set; }
    }
}
