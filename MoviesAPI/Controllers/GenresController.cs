using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MoviesAPI.DTOs;
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
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;

        public GenresController(ILogger<GenresController> logger, ApplicationDbContext context, IMapper mapper)
        {
            this.logger = logger;
            this.context = context;
            this.mapper = mapper;
        }

        [HttpGet] 
        public async Task<ActionResult<List<GenreDto>>> Get()
        {
            var genres=await context.Genres.OrderBy(g=>g.Name).ToListAsync();

            // manualne mapovanie na DTO -  nie dobry napad
            /*var genresDTOs = new List<GenreDto>();
            foreach(var genre in genres)
            {
                genresDTOs.Add(new GenreDto() { Id = genre.Id, Name = genre.Name });
            }
            return genresDTOs; */

            // mapovanie cez AutoMapper!
            return mapper.Map<List<GenreDto>>(genres);
        }

        [HttpGet("{Id:int}")]
        public async Task<ActionResult<GenreDto>> Get(int id)
        {
            var genre = await context.Genres.FirstOrDefaultAsync(g => g.Id == id);
            if (genre == null)
                return NotFound();

            return mapper.Map<GenreDto>(genre); 
        }
         
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] GenreCreationDto genreCreationDto)
        {
            var genreDupl=await context.Genres.FirstOrDefaultAsync(g => g.Name == genreCreationDto.Name);
            if (genreDupl != null)
                return BadRequest("Duplicates values not allowed!");

            var genre = mapper.Map<Genre>(genreCreationDto);

            context.Genres.Add(genre);
            await context.SaveChangesAsync();
            return NoContent();
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(int id, [FromBody] GenreCreationDto genreCreationDto)
        {
            var genre = mapper.Map<Genre>(genreCreationDto);
            genre.Id = id;
            context.Entry(genre).State = EntityState.Modified;
            await context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var genre = await context.Genres.FirstOrDefaultAsync(x => x.Id == id);

            if (genre == null)
                return NotFound();

            context.Remove(genre);
            await context.SaveChangesAsync();
            return NoContent();
        }
    }
}



