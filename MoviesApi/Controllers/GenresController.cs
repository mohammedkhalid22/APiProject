using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MoviesApi.Dtos;
using MoviesApi.Model;

namespace MoviesApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GenresController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public GenresController(ApplicationDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var Genrs = await _context.Genres.ToListAsync();
            return Ok(Genrs);
        }
        [HttpPost]
        public async Task<IActionResult> CreateAsync(CreateGenreDto dto)
        {
            var genre = new Genre { Name = dto.Name };
            await _context.AddAsync(genre);
           _context.SaveChanges();
            return Ok(genre);   
        }
        //هنا بعرفو اننا هنستقبل اي دي علشان نعدل من خلاله
        [HttpPut ("{id}")]
        public async Task<IActionResult> UpdateAsync(int id ,[FromBody] CreateGenreDto dto)
        {
            var genre = await _context.Genres.SingleOrDefaultAsync(g => g.Id == id );
            if (genre == null)
                return NotFound($"No Genre Was Fount With Id : {id}");
            genre.Name = dto.Name;  
            await _context.SaveChangesAsync();
            return Ok(genre);
        }
        [HttpDelete ("{id}")]
        public async Task<IActionResult> DeleteAsync(int id )
        {
            var genre = await _context.Genres.SingleOrDefaultAsync(g => g.Id == id);
            if (genre == null)
                return NotFound($"No Genre Was Fount With Id : {id}");
            _context.Genres.Remove(genre);
            await _context.SaveChangesAsync();
            return Ok(genre);

        }
    }
}
