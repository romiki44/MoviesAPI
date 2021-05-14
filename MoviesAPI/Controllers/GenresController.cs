using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Logging;
using MoviesAPI.Entities;
using MoviesAPI.Filters;
using MoviesAPI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MoviesAPI.Controllers
{
    [Route("api/genres")]
    // ApiController automaticky vrati BadRequest s aktulanym ModelState, ak Model nie je validny!!!!
    // ModelState ma trochu viac informacii, ako keby som vracal BadRequest v kode...
    [ApiController]
    public class GenresController : ControllerBase
    {
        private readonly IRepository repository;
        private readonly ILogger<GenresController> logger;

        public GenresController(IRepository repository, ILogger<GenresController> logger)
        {
            this.repository = repository;
            this.logger = logger;
        }

        [HttpGet] //api/genres
        [HttpGet("list")] //api/genres/list
        [HttpGet("/allgenres")] // overwrite! /allgenres

        // globalny cache filter..cize prvy krat metoda zbehne normalne, potom pri dalsej poziadavke sa vracia odpoved z cache po odbu 60 sek
        // vyhoda, ze napr. sa nedotazujeme stale databazy na aktualny vysledok, ak sa tam nic nememi...ak sa ale meni, bude to pruser! :)
        // pre overenie v Postmane musi sa nastavit NoCache-Header v Settings na Off, inac sa nebude cachovat!!...ale momentalne mi to nefuguje!!
        //[ResponseCache(Duration =60)]  

        // Autorizacny filter
        //[Authorize(AuthenticationSchemes =JwtBearerDefaults.AuthenticationScheme)]

        [ServiceFilter(typeof(MyActionFilter))]
        public async Task<ActionResult<List<Genre>>> Get()
        {
            logger.LogInformation("Getting all genres");
            return await repository.GetAllGenres();
        }

        //[HttpGet("{Id:int}/{param=felipe}")]  // len int, nie string!...plus druhy paramter ako route
        //[HttpGet("examaple")] //api/genres/exmaple?Id=2]
        [HttpGet("{Id:int}")]
        public ActionResult<Genre> Get(int id, [BindRequired] string param)
        //public IActionResult Get(int id, string param)
        {
            // [BindRequired]...polozka povinna, inac BadRequest
            // [BindNever]....prameter bude vzdy null, aj ked sa naplni
            // [FromBody], [FromHeader], [FromQuery], [FromRoute], [FromForm], [FromServices]....odkial pride parameter

            // riesim cez [ApiController]
            //if(!ModelState.IsValid)
            //    return BadRequest(ModelState);

            logger.LogDebug("get by id method excuted...");
            var genre=repository.GetGenreById(id);
            if (genre == null)
            {
                logger.LogWarning($"Genre with Id={id} not found!");
                // priklad chyby osetrenej globalnym filtrom MyExceptionFilter....musi sa nastavit v StartUpe!!!
                throw new ApplicationException("Genre NOT FOUND!!!");
                return NotFound();  // chyba, ak vraciam Genre....musim vracat ActioResult<Genre>...alebo IActionResult
            }

            //return Ok(genre);  //ak IActionResult potom OK()...tiez nie je genericky typ...cize mozem vratit co chcem, nie iba Genre!!!
            //return genre; // ak ActonResult<Genre>, nemozem vrati hocico, iba genericky typ!!! min. Aspnet Core 2.1!
            logger.LogInformation($"Getting Genre with Id={id}");
            return Ok(genre); // mozem aj cez OK(genre)
        }

        [HttpPost]
        public ActionResult Post([FromBody] Genre genre)
        {
            // toto je normalne potrebne pre validaciu
            // druhy sposob, pouzit [APIController]

            // netreba, riesim cez [ApiController]
            //if (ModelState.IsValid)
            //    return BadRequest(ModelState);

            repository.AddGenre(genre);

            return NoContent();
        }

        [HttpPut]
        public ActionResult Put([FromBody] Genre genre)
        {

            return NoContent();
        }

        [HttpDelete]
        public ActionResult Delete()
        {

            return NoContent();
        }
    }
}
