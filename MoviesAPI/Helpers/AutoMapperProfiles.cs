using AutoMapper;
using Microsoft.AspNetCore.Identity;
using MoviesAPI.DTOs;
using MoviesAPI.Entities;
using NetTopologySuite.Geometries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MoviesAPI.Helpers
{
    public class AutoMapperProfiles: Profile
    {
        public AutoMapperProfiles(GeometryFactory geometryFactory)
        {
            CreateMap<GenreDto, Genre>().ReverseMap();
            CreateMap<GenreCreationDto, Genre>();

            CreateMap<ActorDto, Actor>().ReverseMap();
            CreateMap<ActorCreationDto, Actor>()
                .ForMember(x => x.Picture, options => options.Ignore());

            CreateMap<MovieTheater, MovieTheaterDto>()
                .ForMember(x => x.Latitude, dto => dto.MapFrom(prop => prop.Location.X))
                .ForMember(x => x.Longitude, dto => dto.MapFrom(prop => prop.Location.Y));

            CreateMap<MovieTheaterCreationDto, MovieTheater>()
                .ForMember(x => x.Location, x => x.MapFrom(dto => geometryFactory.CreatePoint(new Coordinate(dto.Latitude, dto.Longitude))));

            CreateMap<MovieCreationDto, Movie>()
                .ForMember(x => x.Poster, options => options.Ignore())
                .ForMember(x => x.MoviesGenres, options => options.MapFrom(MapMoviesGenres))
                .ForMember(x => x.MovieTheatersMovies, options => options.MapFrom(MapMovieTheatersMovies))
                .ForMember(x => x.MoviesActors, options => options.MapFrom(MapMoviesActors));

            CreateMap<Movie, MovieDto>()
                .ForMember(x => x.Genres, options => options.MapFrom(MapMoviesGenres))
                .ForMember(x => x.MovieTheaters, options => options.MapFrom(MapMovieTheatersMovies))
                .ForMember(x => x.Actors, options => options.MapFrom(MapMoviesActors));

            CreateMap<IdentityUser, UserDto>();
        }

        private List<ActorMovieDto> MapMoviesActors(Movie movie, MovieDto movieDto)
        {
            var result = new List<ActorMovieDto>();

            if(movie.MoviesActors!=null)
            {
                foreach(var movieActor in movie.MoviesActors)
                {
                    result.Add(new ActorMovieDto()
                    {
                        Id=movieActor.ActorId,
                        Name=movieActor.Actor.Name,
                        Character=movieActor.Character,
                        Picture=movieActor.Actor.Picture,
                        Order=movieActor.Order
                    });
                }
            }

            return result;
        }

        private List<MovieTheaterDto> MapMovieTheatersMovies(Movie movie, MovieDto movieDto)
        {
            var result = new List<MovieTheaterDto>();

            if(movie.MovieTheatersMovies!=null)
            {
                foreach(var movieTheaterMovies in movie.MovieTheatersMovies)
                {
                    result.Add(new MovieTheaterDto()
                    {
                        Id=movieTheaterMovies.MovieTheaterId,
                        Name=movieTheaterMovies.MovieTheater.Name,
                        Latitude=movieTheaterMovies.MovieTheater.Location.X,
                        Longitude=movieTheaterMovies.MovieTheater.Location.Y
                    });
                }
            }

            return result;
        }

        private List<GenreDto> MapMoviesGenres(Movie movie, MovieDto movieDto)
        {
            var result = new List<GenreDto>();

            if(movie.MoviesGenres!=null)
            {
                foreach(var genre in movie.MoviesGenres)
                {
                    result.Add(new GenreDto() { Id = genre.GenreId, Name = genre.Genre.Name });
                }
            }

            return result;
        }

        private List<MovieGenre> MapMoviesGenres(MovieCreationDto movieCreationDto, Movie movie)
        {
            var result = new List<MovieGenre>();

            if (movieCreationDto.GenresIds == null)
                return result;

            foreach(var id in movieCreationDto.GenresIds)
            {
                result.Add(new MovieGenre() { GenreId = id });
            }

            return result;
        }

        private List<MovieTheaterMovie> MapMovieTheatersMovies(MovieCreationDto movieCreationDto, Movie movie)
        {
            var result = new List<MovieTheaterMovie>();

            if (movieCreationDto.MovieTheatersIds == null)
                return result;

            foreach (var id in movieCreationDto.MovieTheatersIds)
            {
                result.Add(new MovieTheaterMovie() { MovieTheaterId = id });
            }

            return result;
        }

        private List<MovieActor> MapMoviesActors(MovieCreationDto movieCreationDto, Movie movie)
        {
            var result = new List<MovieActor>();

            if (movieCreationDto.Actors == null)
                return result;

            foreach (var actor in movieCreationDto.Actors)
            {
                result.Add(new MovieActor() { ActorId=actor.Id, Character=actor.Character });
            }

            return result;
        }
    }
}
