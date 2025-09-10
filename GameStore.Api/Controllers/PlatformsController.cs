using GameStore.Api.Infrastructure.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GameStore.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PlatformsController(GameStoreDbContext db) : ControllerBase
{
    // GET /api/platforms
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var items = await db.Platforms
            .OrderBy(p => p.Name)
            .ToListAsync();

        return Ok(items);
    }
}
