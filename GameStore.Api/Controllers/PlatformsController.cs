using GameStore.Api.Infrastructure.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GameStore.Api.Controllers
{
    // Controlador para gestionar las plataformas de videojuegos
    [ApiController]
    [Route("api/[controller]")]
    public class PlatformsController : ControllerBase
    {
        private readonly GameStoreDbContext _db;
        
        public PlatformsController(GameStoreDbContext db) => _db = db;

        // Obtiene todas las plataformas ordenadas alfabéticamente
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var platforms = await _db.Platforms
                .OrderBy(p => p.Name)
                .ToListAsync();

            return Ok(platforms);
        }
    }
}