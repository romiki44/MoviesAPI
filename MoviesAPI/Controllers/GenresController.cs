using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Logging;
using MoviesAPI.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MoviesAPI.Controllers
{
    [Route("api/genres")]
    [ApiController]
    public class GenresController : ControllerBase
    {
        private readonly ILogger<GenresController> logger;

        public GenresController(ILogger<GenresController> logger)
        {
            this.logger = logger;
        }

        [HttpGet] 
        public async Task<ActionResult<List<Genre>>> Get()
        {
            await Task.Delay(300);

            return new List<Genre>
            {
                new Genre() {Id=1, Name="Comedy"},
                new Genre() {Id=2, Name="Action"}
            };
        }

        [HttpGet("{Id:int}")]
        public ActionResult<Genre> Get(int id)
        {

            throw new NotImplementedException();
        }

        [HttpPost]
        public ActionResult Post([FromBody] Genre genre)
        {
            throw new NotImplementedException();
        }

        [HttpPut]
        public ActionResult Put([FromBody] Genre genre)
        {
            throw new NotImplementedException();
        }

        [HttpDelete]
        public ActionResult Delete()
        {
            throw new NotImplementedException();
        }
    }
}
