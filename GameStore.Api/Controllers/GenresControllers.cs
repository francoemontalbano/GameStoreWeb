using GameStore.Api.Infrastructure.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GameStore.Api.Controllers
{
    // Controlador para gestionar los géneros de videojuegos
    [ApiController]
    [Route("api/[controller]")]
    public class GenresController : ControllerBase
    {
        private readonly GameStoreDbContext _db;
        
        public GenresController(GameStoreDbContext db) => _db = db;

        // Obtiene todos los géneros ordenados alfabéticamente
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var genres = await _db.Genres
                .OrderBy(g => g.Name)
                .ToListAsync();

            return Ok(genres);
        }
    }
}