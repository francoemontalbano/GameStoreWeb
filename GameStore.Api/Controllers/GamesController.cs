using GameStore.Api.Domain.Entities;
using GameStore.Api.Infrastructure.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GameStore.Api.Controllers
{
    // Controlador para gestionar los videojuegos de la tienda
    [Route("api/[controller]")]
    [ApiController]
    public class GamesController : ControllerBase
    {
        private readonly GameStoreDbContext _db;
        
        public GamesController(GameStoreDbContext db) => _db = db;

        // Obtiene todos los juegos con filtros opcionales y paginación
        [HttpGet]
        public async Task<IActionResult> GetAll(
         [FromQuery] string? search,
         [FromQuery] string? platform,
         [FromQuery] string? genre,
         [FromQuery] int page = 1,
         [FromQuery] int pageSize = 10)
        {
            // Consulta base sin tracking para mejor rendimiento
            IQueryable<Game> query = _db.Games.AsNoTracking();

            // Aplicar filtros
            if (!string.IsNullOrWhiteSpace(search))
            {
                var searchTerm = search.Trim().ToLower();
                query = query.Where(g => g.Title.ToLower().Contains(searchTerm));
            }

            if (!string.IsNullOrWhiteSpace(platform))
            {
                query = from g in query
                        join gp in _db.GamePlatforms on g.Id equals gp.GameId
                        join p in _db.Platforms on gp.PlatformId equals p.Id
                        where p.Name == platform
                        select g;
            }

            if (!string.IsNullOrWhiteSpace(genre))
            {
                query = from g in query
                        join gg in _db.GameGenres on g.Id equals gg.GameId
                        join ge in _db.Genres on gg.GenreId equals ge.Id
                        where ge.Name == genre
                        select g;
            }

            // Aplicar ordenamiento y paginación
            query = query.Distinct().OrderBy(g => g.Price);

            var total = await query.CountAsync();
            var items = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(g => new { 
                    g.Id, 
                    g.Title, 
                    g.Slug, 
                    g.Price, 
                    g.IsDigital 
                })
                .ToListAsync();

            return Ok(new { total, items });
        }

        // Obtiene un juego específico por su slug
        [HttpGet("{slug}")]
        public async Task<IActionResult> GetBySlug(string slug)
        {
            var game = await _db.Games
                .Where(g => g.Slug == slug)
                .Select(g => new
                {
                    g.Id,
                    g.Title,
                    g.Slug,
                    g.Price,
                    g.IsDigital,
                    Platforms = _db.GamePlatforms
                        .Where(gp => gp.GameId == g.Id)
                        .Select(gp => gp.Platform.Name)
                        .ToList(),
                    Genres = _db.GameGenres
                        .Where(gg => gg.GameId == g.Id)
                        .Select(gg => gg.Genre.Name)
                        .ToList()
                })
                .FirstOrDefaultAsync();

            return game is null ? NotFound() : Ok(game);
        }
    }
}