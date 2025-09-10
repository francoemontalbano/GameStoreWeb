using Microsoft.EntityFrameworkCore;
using GameStore.Api.Domain.Entities;

namespace GameStore.Api.Infrastructure.Persistence
{
    public class GameStoreDbContext : DbContext
    {
        public GameStoreDbContext(DbContextOptions<GameStoreDbContext> options) : base(options) { }

        public DbSet<Game> Games => Set<Game>();
        public DbSet<Platform> Platforms => Set<Platform>();
        public DbSet<Genre> Genres => Set<Genre>();
        public DbSet<GamePlatform> GamePlatforms => Set<GamePlatform>();
        public DbSet<GameGenre> GameGenres => Set<GameGenre>();

        protected override void OnModelCreating(ModelBuilder b)
        {
            // Índice único para URLs limpias
            b.Entity<Game>()
                .HasIndex(g => g.Slug)
                .IsUnique();

            // Seed de juegos
            b.Entity<Game>().HasData(
                new Game { Id = 1, Title = "The Witcher 3: Wild Hunt", Slug = "the-witcher-3-wild-hunt", Price = 39999m, IsDigital = true },
                new Game { Id = 2, Title = "Cyberpunk 2077", Slug = "cyberpunk-2077", Price = 59999m, IsDigital = true },
                new Game { Id = 3, Title = "Red Dead Redemption 2", Slug = "red-dead-redemption-2", Price = 49999m, IsDigital = false }
            );

            // Claves compuestas en las tablas de unión
            b.Entity<GamePlatform>().HasKey(x => new { x.GameId, x.PlatformId });
            b.Entity<GameGenre>().HasKey(x => new { x.GameId, x.GenreId });

            // Seed de plataformas y géneros
            b.Entity<Platform>().HasData(
                new Platform { Id = 1, Name = "PC" },
                new Platform { Id = 2, Name = "PS5" },
                new Platform { Id = 3, Name = "Xbox" },
                new Platform { Id = 4, Name = "Switch" }
            );

            b.Entity<Genre>().HasData(
                new Genre { Id = 1, Name = "Action" },
                new Genre { Id = 2, Name = "RPG" },
                new Genre { Id = 3, Name = "Indie" },
                new Genre { Id = 4, Name = "Sports" }
            );

            // Relacionar juegos seed a plataformas y géneros
            b.Entity<GamePlatform>().HasData(
                new GamePlatform { GameId = 1, PlatformId = 1 }, // Witcher 3 -> PC
                new GamePlatform { GameId = 2, PlatformId = 1 }, // Cyberpunk -> PC
                new GamePlatform { GameId = 3, PlatformId = 2 }  // RDR2 -> PS5
            );

            b.Entity<GameGenre>().HasData(
                new GameGenre { GameId = 1, GenreId = 2 }, // Witcher 3 -> RPG
                new GameGenre { GameId = 2, GenreId = 2 }, // Cyberpunk -> RPG
                new GameGenre { GameId = 3, GenreId = 1 }  // RDR2 -> Action
            );
        }
    }
}
