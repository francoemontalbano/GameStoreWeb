using GameStore.Api.Domain.Entities;
using GameStore.Api.Infrastructure.Persistence;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GameStore.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GamesController : ControllerBase
    {
        private readonly GameStoreDbContext _db;
        public GamesController(GameStoreDbContext db) => _db = db;


        // GET /api/Games
        [HttpGet]
        public async Task<IActionResult> GetAll(
         [FromQuery] string? search,
         [FromQuery] string? platform,
         [FromQuery] string? genre,
         [FromQuery] int page = 1,
         [FromQuery] int pageSize = 10)
        {
            IQueryable<Game> q = _db.Games.AsNoTracking();

            if (!string.IsNullOrWhiteSpace(search))
            {
                var term = search.Trim().ToLower();
                q = q.Where(g => g.Title.ToLower().Contains(term));
            }
            if (!string.IsNullOrWhiteSpace(platform))
                q = from g in q
                    join gp in _db.GamePlatforms on g.Id equals gp.GameId
                    join p in _db.Platforms on gp.PlatformId equals p.Id
                    where p.Name == platform
                    select g;

            if (!string.IsNullOrWhiteSpace(genre))
                q = from g in q
                    join gg in _db.GameGenres on g.Id equals gg.GameId
                    join ge in _db.Genres on gg.GenreId equals ge.Id
                    where ge.Name == genre
                    select g;

            q = q.Distinct().OrderBy(g => g.Price);

            var total = await q.CountAsync();
            var items = await q
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(g => new { g.Id, g.Title, g.Slug, g.Price, g.IsDigital })
                .ToListAsync();

            return Ok(new { total, items });
        }


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
