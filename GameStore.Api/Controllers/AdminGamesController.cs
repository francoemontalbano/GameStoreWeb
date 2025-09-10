using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GameStore.Api.Domain.Entities;
using GameStore.Api.Infrastructure.Persistence;
using GameStore.Api.Domain.DTOs;

namespace GameStore.Api.Controllers
{
    // Controlador CRUD de juegos solo para administradores
    [Authorize(Roles = "Admin")]
    [ApiController]
    [Route("api/admin/[controller]")]
    public class AdminGamesController : ControllerBase
    {
        private readonly GameStoreDbContext _db;

        public AdminGamesController(GameStoreDbContext db)
        {
            _db = db;
        }

        // GET /api/admin/admingames - Obtener todos los juegos con detalles completos
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var games = await _db.Games
                .Include(g => g.GamePlatforms)
                    .ThenInclude(gp => gp.Platform)
                .Include(g => g.GameGenres)
                    .ThenInclude(gg => gg.Genre)
                .Select(g => new
                {
                    g.Id,
                    g.Title,
                    g.Slug,
                    g.Price,
                    g.IsDigital,
                    g.ImageUrl,
                    Platforms = g.GamePlatforms.Select(gp => new { gp.Platform.Id, gp.Platform.Name }),
                    Genres = g.GameGenres.Select(gg => new { gg.Genre.Id, gg.Genre.Name })
                })
                .ToListAsync();

            return Ok(games);
        }

        // GET /api/admin/admingames/{id} - Obtener juego por ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var game = await _db.Games
                .Include(g => g.GamePlatforms)
                    .ThenInclude(gp => gp.Platform)
                .Include(g => g.GameGenres)
                    .ThenInclude(gg => gg.Genre)
                .Where(g => g.Id == id)
                .Select(g => new
                {
                    g.Id,
                    g.Title,
                    g.Slug,
                    g.Price,
                    g.IsDigital,
                    g.ImageUrl,
                    Platforms = g.GamePlatforms.Select(gp => new { gp.Platform.Id, gp.Platform.Name }),
                    Genres = g.GameGenres.Select(gg => new { gg.Genre.Id, gg.Genre.Name })
                })
                .FirstOrDefaultAsync();

            if (game == null)
            {
                return NotFound();
            }

            return Ok(game);
        }

        // POST /api/admin/admingames - Crear nuevo juego
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] GameCreateRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Verificar si el slug ya existe
            if (await _db.Games.AnyAsync(g => g.Slug == request.Slug))
            {
                return BadRequest(new { message = "El slug ya existe" });
            }

            // Crear el juego
            var game = new Game
            {
                Title = request.Title,
                Slug = request.Slug,
                Price = request.Price,
                IsDigital = request.IsDigital,
                ImageUrl = request.ImageUrl ?? ""
            };

            _db.Games.Add(game);
            await _db.SaveChangesAsync();

            // Agregar plataformas
            foreach (var platformId in request.PlatformIds)
            {
                if (await _db.Platforms.AnyAsync(p => p.Id == platformId))
                {
                    _db.GamePlatforms.Add(new GamePlatform
                    {
                        GameId = game.Id,
                        PlatformId = platformId
                    });
                }
            }

            // Agregar géneros
            foreach (var genreId in request.GenreIds)
            {
                if (await _db.Genres.AnyAsync(g => g.Id == genreId))
                {
                    _db.GameGenres.Add(new GameGenre
                    {
                        GameId = game.Id,
                        GenreId = genreId
                    });
                }
            }

            await _db.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = game.Id }, new
            {
                game.Id,
                game.Title,
                game.Slug,
                game.Price,
                game.IsDigital,
                game.ImageUrl
            });
        }

        // PUT /api/admin/admingames/{id} - Actualizar juego
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] GameUpdateRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var game = await _db.Games.FindAsync(id);
            if (game == null)
            {
                return NotFound();
            }

            // Verificar si el slug ya existe en otro juego
            if (await _db.Games.AnyAsync(g => g.Slug == request.Slug && g.Id != id))
            {
                return BadRequest(new { message = "El slug ya existe" });
            }

            // Actualizar propiedades del juego
            game.Title = request.Title;
            game.Slug = request.Slug;
            game.Price = request.Price;
            game.IsDigital = request.IsDigital;
            game.ImageUrl = request.ImageUrl ?? "";

            // Eliminar relaciones existentes
            var existingPlatforms = await _db.GamePlatforms.Where(gp => gp.GameId == id).ToListAsync();
            var existingGenres = await _db.GameGenres.Where(gg => gg.GameId == id).ToListAsync();

            _db.GamePlatforms.RemoveRange(existingPlatforms);
            _db.GameGenres.RemoveRange(existingGenres);

            // Agregar nuevas plataformas
            foreach (var platformId in request.PlatformIds)
            {
                if (await _db.Platforms.AnyAsync(p => p.Id == platformId))
                {
                    _db.GamePlatforms.Add(new GamePlatform
                    {
                        GameId = game.Id,
                        PlatformId = platformId
                    });
                }
            }

            // Agregar nuevos géneros
            foreach (var genreId in request.GenreIds)
            {
                if (await _db.Genres.AnyAsync(g => g.Id == genreId))
                {
                    _db.GameGenres.Add(new GameGenre
                    {
                        GameId = game.Id,
                        GenreId = genreId
                    });
                }
            }

            await _db.SaveChangesAsync();

            return Ok(new
            {
                game.Id,
                game.Title,
                game.Slug,
                game.Price,
                game.IsDigital,
                game.ImageUrl
            });
        }

        // DELETE /api/admin/admingames/{id} - Eliminar juego
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var game = await _db.Games.FindAsync(id);
            if (game == null)
            {
                return NotFound();
            }

            // Eliminar relaciones primero
            var platforms = await _db.GamePlatforms.Where(gp => gp.GameId == id).ToListAsync();
            var genres = await _db.GameGenres.Where(gg => gg.GameId == id).ToListAsync();

            _db.GamePlatforms.RemoveRange(platforms);
            _db.GameGenres.RemoveRange(genres);
            _db.Games.Remove(game);

            await _db.SaveChangesAsync();

            return NoContent();
        }

        // GET /api/admin/admingames/platforms - Obtener todas las plataformas
        [HttpGet("platforms")]
        public async Task<IActionResult> GetPlatforms()
        {
            var platforms = await _db.Platforms
                .OrderBy(p => p.Name)
                .Select(p => new { p.Id, p.Name })
                .ToListAsync();

            return Ok(platforms);
        }

        // GET /api/admin/admingames/genres - Obtener todos los géneros
        [HttpGet("genres")]
        public async Task<IActionResult> GetGenres()
        {
            var genres = await _db.Genres
                .OrderBy(g => g.Name)
                .Select(g => new { g.Id, g.Name })
                .ToListAsync();

            return Ok(genres);
        }

        // Endpoint de prueba para verificar conectividad
        [HttpGet("test-upload")]
        public IActionResult TestUpload()
        {
            return Ok(new { message = "Endpoint de subida funcionando correctamente", timestamp = DateTime.UtcNow });
        }

        // Endpoint de prueba para verificar datos de juegos
        [AllowAnonymous]
        [HttpGet("test-games")]
        public async Task<IActionResult> TestGames()
        {
            var games = await _db.Games
                .Select(g => new
                {
                    g.Id,
                    g.Title,
                    g.ImageUrl,
                    HasImageUrl = !string.IsNullOrEmpty(g.ImageUrl),
                    ImageUrlLength = g.ImageUrl != null ? g.ImageUrl.Length : 0
                })
                .ToListAsync();

            return Ok(new { 
                message = "Datos de juegos desde la base de datos",
                games = games,
                count = games.Count
            });
        }

        // Subir imagen de juego
        [HttpPost("upload-image")]
        public async Task<ActionResult<string>> UploadImage(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("No se ha enviado ningún archivo");

            // Validar tipo de archivo - Ampliar lista de formatos aceptados
            var allowedExtensions = new[] { 
                ".jpg", ".jpeg", ".png", ".gif", ".webp", 
                ".bmp", ".tiff", ".tif", ".svg", ".ico",
                ".jfif", ".pjpeg", ".pjp"
            };
            var fileExtension = Path.GetExtension(file.FileName).ToLowerInvariant();
            
            if (!allowedExtensions.Contains(fileExtension))
                return BadRequest("Tipo de archivo no permitido. Formatos aceptados: JPG, JPEG, PNG, GIF, WEBP, BMP, TIFF, SVG, ICO, JFIF");

            // Validar tamaño (máximo 5MB)
            if (file.Length > 5 * 1024 * 1024)
                return BadRequest("El archivo es demasiado grande. Máximo 5MB");

            try
            {
                // Crear directorio si no existe
                var uploadsPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "games");
                if (!Directory.Exists(uploadsPath))
                {
                    Directory.CreateDirectory(uploadsPath);
                }

                // Generar nombre único para el archivo
                var fileName = $"{Guid.NewGuid()}{fileExtension}";
                var filePath = Path.Combine(uploadsPath, fileName);

                // Guardar archivo
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                // Retornar URL relativa
                var imageUrl = $"/uploads/games/{fileName}";
                return Ok(new { imageUrl });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al subir archivo: {ex.Message}");
            }
        }
    }
}
