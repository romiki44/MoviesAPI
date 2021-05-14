using Microsoft.Extensions.Logging;
using MoviesAPI.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MoviesAPI.Services
{

    public class InMemoryRepository : IRepository
    {
        private List<Genre> _genres;
        private readonly ILogger<InMemoryRepository> logger;

        public InMemoryRepository(ILogger<InMemoryRepository> logger)
        {
            this.logger = logger;

            _genres = new List<Genre>()
            {
                new Genre() {Id=1, Name="Action"},
                new Genre() {Id=2, Name="Comedy"},
                new Genre() {Id=3, Name="Sci-Fi"},
            };
        }
        
        // umela async uloha!
        public async Task<List<Genre>> GetAllGenres()
        {
            // zobrazi sa informacia, alebo nie?
            // sice metoda je volana v controlleri a conroller je nastaveny na uroven Warning, ale sme v class InMemoryRepository
            // pretoze InMemoryRepository nie je v appsettings.json definovane, vztahuje sa na nu uroven Default
            // a urven Default je nastavena na Information, cize log sa zobrazi!!
            logger.LogInformation("executing metod GetAllGenres()");

            await Task.Delay(300);
            return _genres;
        }

        public Genre GetGenreById(int id)
        {
            return _genres.FirstOrDefault(g => g.Id == id);
        }

        public void AddGenre(Genre genre)
        {
            // akoze databaza!
            genre.Id = _genres.Max(x => x.Id) + 1;
            _genres.Add(genre);
        }
    }
}
