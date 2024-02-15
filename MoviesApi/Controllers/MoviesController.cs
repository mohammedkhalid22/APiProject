using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MoviesApi.Dtos;
using MoviesApi.Model;

namespace MoviesApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {

        private readonly ApplicationDbContext _context;
        //start for type photo and size
        private new List<string> _allowedExtentions = new List<string> { ".png" , ".jpg"};
        private long _maxallowedPosterSize = 2097152;
        //end
        public MoviesController(ApplicationDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var movie = await _context.Movies
                .Include(g => g.Genre)
                .OrderByDescending( R => R.Rate)
                .Select( m => new MoviesDetailsDto
                {
                    Id = m.Id,
                    genreId = m.Genre.Id,
                    Genre = m.Genre.Name,
                    Poster = m.Poster,
                    Rate = m.Rate,  
                    Storeline = m.Storeline,    
                    Tittle = m.Tittle,
                    Year = m.Year,
                    

                })
                .ToListAsync();
            return Ok(movie);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            var movie = await _context.Movies.Include(g => g.Genre).SingleOrDefaultAsync(m =>m.Id == id);
            if (movie == null)
                return NotFound();

            var dto = new MoviesDetailsDto
            {
                Id = movie.Id,
                genreId = movie.Genre.Id,
                Genre = movie.Genre.Name,
                Poster = movie.Poster,
                Rate = movie.Rate,
                Storeline = movie.Storeline,
                Tittle = movie.Tittle,
                Year = movie.Year,
            };
            return Ok(dto);
        }
        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromForm] CreateMovieDto dto)
        {
            //Start Cheak Size And Type
            if (!_allowedExtentions.Contains(Path.GetExtension(dto.Poster.FileName).ToLower()))
                return BadRequest("Only .png Or .jpg Image Are Allowed");
            if (dto.Poster.Length > _maxallowedPosterSize)
                return BadRequest("Max Allowed 2 MB");
            //End Cheak Size And Type
            //Start Cheak Fk Is Valid?
            var _isValidGenre = await _context.Genres.AnyAsync(g => g.Id == dto.genreId);
                if (!_isValidGenre)
                return BadRequest("InValid Genre Id");
            //End Cheak Fk Is Valid?
            //start to send file
            using var datastreem = new MemoryStream();
            await dto.Poster.CopyToAsync(datastreem);
            //end 
            var movie = new Movie
            {
                genreId = dto.genreId,
                Tittle = dto.Title,
                Year = dto.Year,
                Rate = dto.Rate,
                Storeline = dto.Storeline,
                Poster = datastreem.ToArray(),

            };
            await _context.AddAsync(movie);
            _context.SaveChanges();
            return Ok(movie);

        }
        [HttpPut ("{id}")]
        public async Task<IActionResult> UpdateAsync (int id , [FromBody] CreateMovieDto dto)
        {
            var movie = await _context.Movies.SingleOrDefaultAsync( m => m.Id == id);
            if (movie == null)
                return BadRequest($"No movie Was Fount With Id: {id}");
            //Start Cheak Fk Is Valid?
            var _isValidGenre = await _context.Genres.AnyAsync(g => g.Id == dto.genreId);
            if (!_isValidGenre)
                return BadRequest("InValid Genre Id");
            //End Cheak Fk Is Valid?
            if(dto.Poster != null)
            {
                //Start Cheak Size And Type
                if (!_allowedExtentions.Contains(Path.GetExtension(dto.Poster.FileName).ToLower()))
                    return BadRequest("Only .png Or .jpg Image Are Allowed");
                if (dto.Poster.Length > _maxallowedPosterSize)
                    return BadRequest("Max Allowed 2 MB");
                //End Cheak Size And Type
                //start to send file
                using var datastreem = new MemoryStream();
                await dto.Poster.CopyToAsync(datastreem);
                //end 
                movie.Poster = datastreem.ToArray();
            }
            movie.Rate = dto.Rate;
            movie.Tittle = dto.Title;
            movie.Year = dto.Year;
            movie.Storeline = dto.Storeline;
            movie.genreId = dto.genreId;
            
            _context.SaveChanges();
            return Ok(movie);
        }
        [HttpDelete ("{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var movie = await _context.Movies.FirstOrDefaultAsync(g => g.Id == id);
            if (movie == null)
                return BadRequest($"No Genre Was Fount With Id: { id}");
            _context.Movies.Remove(movie);
            await _context.SaveChangesAsync();
            return Ok(movie);

        }
        
    }
}
