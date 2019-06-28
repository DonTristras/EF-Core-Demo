using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FreeWheel.Models;
using FreeWheel.Filters;

namespace FreeWheel.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private readonly FreeWheelContext _context;

        public MoviesController(FreeWheelContext context)
        {
            _context = context;
        }
        
        // GET: api/Movies/MethodA
        [HttpGet]
        [Route("methodA")]
        public IActionResult methodA([FromBody] MovieSearchFilter filter)
        {
            //Check if there is any filter value set in order to proceed with the search
            if (filter.title != null || filter.year != 0 || (filter.genres != null && filter.genres.Count() > 0))
            {
                //Find all movies where any of the filters match, return movies with average rating
                var foundMovies = (from m in _context.Movies
                                   where (filter.title == null || m.title.Contains(filter.title)) &&
                                   (filter.year == 0 || m.yearOfRelease == filter.year) &&
                                   (filter.genres == null || m.MoviesGenres.Any(g => filter.genres.Contains(g.GenresId)))
                                   select new { m.id, m.title, m.yearOfRelease, m.runningTime, averageRating = Math.Ceiling(m.ratings.Average(a => a.rating) / 0.5) * 0.5 }).ToList();

                if (foundMovies.Count() > 0){
                    return new JsonResult(foundMovies);
                }
                else {
                    return StatusCode(404);
                }
            }
            else {
                return StatusCode(400);
            }
        }

        // GET: api/Movies/MethodA
        [HttpGet]
        [Route("methodB")]
        public IActionResult methodB()
        {

            try
            { 
                //Find top 5 movies who have the biggest average ratings, return movies with average ratings
                var foundMovies = (from m in _context.Movies
                                  orderby m.ratings.Average(a => a.rating) descending, m.title ascending
                                  select new { m.id, m.title, m.yearOfRelease, m.runningTime, averageRating = Math.Ceiling(m.ratings.Average(a => a.rating) / 0.5) * 0.5 }).Take(5).ToList();

                //Return found items or return 404 if not found
                if (foundMovies.Count() > 0)
                {
                    return new JsonResult(foundMovies);
                }
                else
                {
                    return StatusCode(404);
                }
            }
            catch (Exception e)
            {
                return StatusCode(400);
            }
        }

        // GET: api/Movies/MethodC/ID
        [HttpGet]
        [Route("methodC/{id}")]
        public IActionResult methodC([FromRoute] Guid id)
        {
            try
            {
                //Find the 5 top movies ordered by a specific user rating
                var foundMovies = (from m in _context.Ratings
                                   where m.users.id == id
                                   orderby m.rating descending, m.movies.title ascending
                                   select new { m.movies.id, m.movies.title, m.movies.yearOfRelease, m.movies.runningTime, m.rating }).Take(5).ToList();

                //Return found movies as JSON
                if (foundMovies.Count() > 0)
                {
                    return new JsonResult(foundMovies);
                }
                else
                {
                    return StatusCode(404);
                }
            }
            catch (Exception e)
            {
                return StatusCode(400);
            }
        }

        // POST: api/Movies/MethodC/ID
        [HttpPost]
        [Route("methodD/{userId}/{movieId}/{rating}")]
        public IActionResult methodD([FromRoute] Guid userId, [FromRoute] Guid movieId, [FromRoute] int rating)
        {
            try
            {
                //Check if values are correct between 1 = 5// we could also define it on entity framework
                if (rating < 1 && rating > 5)
                {
                    return StatusCode(400);
                }

                //Find the movie and the user and check if both exists, if not return 404
                Movies movie = _context.Movies.Where(m => m.id == movieId).First();
                Users user = _context.Users.Where(u => u.id == userId).First();
                if (movie == null || user == null)
                {
                    return StatusCode(404);
                }

                //Find if there is any rating from a user to a movie and update the value
                var foundRatings = _context.Ratings.Where(x => x.users.id == userId && x.movies.id == movieId);
                if (foundRatings.Count() > 0)
                {
                    Ratings r = foundRatings.FirstOrDefault();
                    r.rating = rating;
                }
                else //If not found, add a new record
                {
                    _context.Ratings.Add(new Ratings { id = Guid.NewGuid(), rating = rating, movies = movie, users = user });
                }

                //Save and return a 200 code
                _context.SaveChanges();
                return StatusCode(200);
            }
            catch (Exception e)
            {
                return StatusCode(400);
            }
        }

        //Uncomment to populate test data
        //[HttpPost]
        //[Route("populate")]
        //public IActionResult Populate()
        //{
        //    Movies m = new Movies() { id = Guid.NewGuid(), runningTime = 130, yearOfRelease = 2011, title = "X men" };
        //    Movies m2 = new Movies() { id = Guid.NewGuid(), runningTime = 180, yearOfRelease = 2013, title = "Pretty woman" };
        //    Movies m3 = new Movies() { id = Guid.NewGuid(), runningTime = 156, yearOfRelease = 2009, title = "Pirates of the caribbean" };
        //    Movies m4 = new Movies() { id = Guid.NewGuid(), runningTime = 180, yearOfRelease = 2016, title = "X men 2" };
        //    Movies m5 = new Movies() { id = Guid.NewGuid(), runningTime = 156, yearOfRelease = 2003, title = "Life of brian" };

        //    Genres g = new Genres() { id = Guid.NewGuid(), title = "Action" };
        //    Genres g2 = new Genres() { id = Guid.NewGuid(), title = "Romantic" };
        //    Users u = new Users() { id = Guid.NewGuid(), name = "Anthony" };
        //    Users u2 = new Users() { id = Guid.NewGuid(), name = "Mike" };

        //    Ratings r = new Ratings() { id = Guid.NewGuid(), rating = 4, movies = m, users = u };
        //    Ratings r2 = new Ratings() { id = Guid.NewGuid(), rating = 5, movies = m, users = u2 };
        //    Ratings r3 = new Ratings() { id = Guid.NewGuid(), rating = 2, movies = m2, users = u };
        //    Ratings r4 = new Ratings() { id = Guid.NewGuid(), rating = 4, movies = m3, users = u };
        //    Ratings r5 = new Ratings() { id = Guid.NewGuid(), rating = 1, movies = m4, users = u };
        //    Ratings r6 = new Ratings() { id = Guid.NewGuid(), rating = 5, movies = m5, users = u };
        //    _context.Movies.Add(m);
        //    _context.Movies.Add(m2);
        //    _context.Movies.Add(m3);
        //    _context.Movies.Add(m4);
        //    _context.Movies.Add(m5);
        //    _context.Genres.Add(g);
        //    _context.Genres.Add(g2);
        //    _context.MoviesGenres.Add(new MoviesGenres() { MoviesId = m.id, GenresId = g.id });
        //    _context.MoviesGenres.Add(new MoviesGenres() { MoviesId = m2.id, GenresId = g2.id });
        //    _context.Users.Add(u);
        //    _context.Users.Add(u2);
        //    _context.Ratings.Add(r);
        //    _context.Ratings.Add(r2);
        //    _context.Ratings.Add(r3);
        //    _context.Ratings.Add(r4);
        //    _context.Ratings.Add(r5);
        //    _context.Ratings.Add(r6);
        //    _context.SaveChanges();
        //    return NoContent();
        //}
        
    }
}