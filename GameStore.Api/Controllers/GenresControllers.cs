using GameStore.Api.Infrastructure.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GameStore.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class GenresController(GameStoreDbContext db) : ControllerBase
{
    // GET /api/genres
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var items = await db.Genres
            .OrderBy(g => g.Name)
            .ToListAsync();

        return Ok(items);
    }
}
