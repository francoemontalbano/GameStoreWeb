using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using GameStore.Api.Domain.Entities;

namespace GameStore.Api.Infrastructure.Persistence
{
    // Contexto de Entity Framework para la base de datos de GameStore con Identity
    public class GameStoreDbContext : IdentityDbContext<ApplicationUser>
    {
        public GameStoreDbContext(DbContextOptions<GameStoreDbContext> options) : base(options) { }

        // Conjuntos de datos (DbSets)
        public DbSet<Game> Games => Set<Game>();
        public DbSet<Platform> Platforms => Set<Platform>();
        public DbSet<Genre> Genres => Set<Genre>();
        public DbSet<GamePlatform> GamePlatforms => Set<GamePlatform>();
        public DbSet<GameGenre> GameGenres => Set<GameGenre>();

        // Configuración del modelo de datos y datos iniciales
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Llamar a la configuración base de Identity
            base.OnModelCreating(modelBuilder);

            // Índice único para URLs amigables (slugs)
            modelBuilder.Entity<Game>()
                .HasIndex(g => g.Slug)
                .IsUnique();

            // Claves compuestas en las tablas de unión para evitar duplicados
            modelBuilder.Entity<GamePlatform>()
                .HasKey(x => new { x.GameId, x.PlatformId });
            
            modelBuilder.Entity<GameGenre>()
                .HasKey(x => new { x.GameId, x.GenreId });

            // Datos iniciales (seed data)
            modelBuilder.Entity<Game>().HasData(
                new Game { Id = 1, Title = "The Witcher 3: Wild Hunt", Slug = "the-witcher-3-wild-hunt", Price = 39999m, IsDigital = true },
                new Game { Id = 2, Title = "Cyberpunk 2077", Slug = "cyberpunk-2077", Price = 59999m, IsDigital = true },
                new Game { Id = 3, Title = "Red Dead Redemption 2", Slug = "red-dead-redemption-2", Price = 49999m, IsDigital = false }
            );

            modelBuilder.Entity<Platform>().HasData(
                new Platform { Id = 1, Name = "PC" },
                new Platform { Id = 2, Name = "PS5" },
                new Platform { Id = 3, Name = "Xbox" },
                new Platform { Id = 4, Name = "Switch" }
            );

            modelBuilder.Entity<Genre>().HasData(
                new Genre { Id = 1, Name = "Acción" },
                new Genre { Id = 2, Name = "RPG" },
                new Genre { Id = 3, Name = "Indie" },
                new Genre { Id = 4, Name = "Deportes" },
                new Genre { Id = 5, Name = "Disparos" },
                new Genre { Id = 6, Name = "Aventura" },
                new Genre { Id = 7, Name = "Estrategia" },
                new Genre { Id = 8, Name = "Carreras" },
                new Genre { Id = 9, Name = "Lucha" },
                new Genre { Id = 10, Name = "Plataformas" },
                new Genre { Id = 11, Name = "Fiesta" },
                new Genre { Id = 12, Name = "Familiar" },
                new Genre { Id = 13, Name = "Simulación" },
                new Genre { Id = 14, Name = "Cooperativo" }
            );

            // Relaciones iniciales entre juegos y plataformas
            modelBuilder.Entity<GamePlatform>().HasData(
                new GamePlatform { GameId = 1, PlatformId = 1 }, // Witcher 3 -> PC
                new GamePlatform { GameId = 2, PlatformId = 1 }, // Cyberpunk -> PC
                new GamePlatform { GameId = 3, PlatformId = 2 }  // RDR2 -> PS5
            );

            // Relaciones iniciales entre juegos y géneros
            modelBuilder.Entity<GameGenre>().HasData(
                new GameGenre { GameId = 1, GenreId = 2 }, // Witcher 3 -> RPG
                new GameGenre { GameId = 2, GenreId = 2 }, // Cyberpunk -> RPG
                new GameGenre { GameId = 3, GenreId = 1 }  // RDR2 -> Action
            );
        }
    }
}