using System.Linq;
using GameStore.Api.Domain.Entities;

namespace GameStore.Api.Infrastructure.Persistence
{
    public static class DevSeeder
    {
        public static void Seed(GameStoreDbContext db)
        {
            // ====== Catálogos (si faltan, se crean) ======
            string[] platforms = { "PC", "PS5", "Xbox", "Switch" };
            foreach (var name in platforms)
                if (!db.Platforms.Any(p => p.Name == name))
                    db.Platforms.Add(new Platform { Name = name });

            string[] genres = { "Action", "RPG", "Indie", "Sports", "Shooter", "Adventure", "Strategy", "Racing" };
            foreach (var name in genres)
                if (!db.Genres.Any(g => g.Name == name))
                    db.Genres.Add(new Genre { Name = name });

            db.SaveChanges();

            // helpers rápidos
            int P(string n) => db.Platforms.First(p => p.Name == n).Id;
            int G(string n) => db.Genres.First(g => g.Name == n).Id;

            void AddGame(string title, string slug, decimal price, bool digital, string[] pfs, string[] gns)
            {
                if (db.Games.Any(x => x.Slug == slug)) return; // evita duplicados por slug

                var game = new Game { Title = title, Slug = slug, Price = price, IsDigital = digital };
                db.Games.Add(game);
                db.SaveChanges();

                foreach (var pn in pfs.Distinct())
                    db.GamePlatforms.Add(new GamePlatform { GameId = game.Id, PlatformId = P(pn) });

                foreach (var gn in gns.Distinct())
                    db.GameGenres.Add(new GameGenre { GameId = game.Id, GenreId = G(gn) });

                db.SaveChanges();
            }

            // ====== Juegos (precios en ARS) ======
            AddGame("Elden Ring", "elden-ring", 99999m, true, new[] { "PC", "PS5", "Xbox" }, new[] { "RPG", "Action" });
            AddGame("God of War Ragnarök", "god-of-war-ragnarok", 119999m, true, new[] { "PS5" }, new[] { "Action", "Adventure" });
            AddGame("Horizon Forbidden West", "horizon-forbidden-west", 109999m, true, new[] { "PS5" }, new[] { "Action", "Adventure" });
            AddGame("Marvel's Spider-Man 2", "spider-man-2", 129999m, true, new[] { "PS5" }, new[] { "Action" });
            AddGame("The Last of Us Part I", "the-last-of-us-part-1", 119999m, true, new[] { "PS5" }, new[] { "Action", "Adventure" });
            AddGame("Gran Turismo 7", "gran-turismo-7", 94999m, true, new[] { "PS5" }, new[] { "Racing" });

            AddGame("Halo Infinite", "halo-infinite", 79999m, true, new[] { "Xbox", "PC" }, new[] { "Shooter", "Action" });
            AddGame("Forza Horizon 5", "forza-horizon-5", 89999m, true, new[] { "Xbox", "PC" }, new[] { "Racing" });
            AddGame("Gears 5", "gears-5", 59999m, true, new[] { "Xbox", "PC" }, new[] { "Shooter", "Action" });
            AddGame("Starfield", "starfield", 129999m, true, new[] { "Xbox", "PC" }, new[] { "RPG", "Adventure" });
            AddGame("Sea of Thieves", "sea-of-thieves", 49999m, true, new[] { "Xbox", "PC" }, new[] { "Adventure" });

            AddGame("Zelda: Tears of the Kingdom", "zelda-tears-of-the-kingdom", 129999m, true, new[] { "Switch" }, new[] { "Adventure" });
            AddGame("Super Mario Odyssey", "super-mario-odyssey", 79999m, true, new[] { "Switch" }, new[] { "Adventure" });
            AddGame("Mario Kart 8 Deluxe", "mario-kart-8-deluxe", 69999m, true, new[] { "Switch" }, new[] { "Racing" });
            AddGame("Metroid Dread", "metroid-dread", 89999m, true, new[] { "Switch" }, new[] { "Action" });
            AddGame("Animal Crossing: New Horizons", "animal-crossing-new-horizons", 79999m, true, new[] { "Switch" }, new[] { "Strategy" });

            AddGame("Baldur's Gate 3", "baldurs-gate-3", 139999m, true, new[] { "PC", "PS5" }, new[] { "RPG", "Adventure" });
            AddGame("Diablo IV", "diablo-iv", 119999m, true, new[] { "PC", "PS5", "Xbox" }, new[] { "Action", "RPG" });
            AddGame("EA Sports FC 24", "ea-sports-fc-24", 109999m, true, new[] { "PC", "PS5", "Xbox", "Switch" }, new[] { "Sports" });
            AddGame("NBA 2K24", "nba-2k24", 99999m, true, new[] { "PC", "PS5", "Xbox", "Switch" }, new[] { "Sports" });
            AddGame("Call of Duty: Modern Warfare III", "cod-modern-warfare-3", 129999m, true, new[] { "PC", "PS5", "Xbox" }, new[] { "Shooter" });
            AddGame("Apex Legends", "apex-legends", 0m, true, new[] { "PC", "PS5", "Xbox", "Switch" }, new[] { "Shooter" });

            AddGame("Hades", "hades", 49999m, true, new[] { "PC", "PS5", "Xbox", "Switch" }, new[] { "Indie", "Action" });
            AddGame("Celeste", "celeste", 29999m, true, new[] { "PC", "PS5", "Xbox", "Switch" }, new[] { "Indie" });
            AddGame("Stardew Valley", "stardew-valley", 19999m, true, new[] { "PC", "PS5", "Xbox", "Switch" }, new[] { "Indie", "Strategy" });
            AddGame("Cuphead", "cuphead", 29999m, true, new[] { "PC", "Xbox", "Switch", "PS5" }, new[] { "Indie" });
            AddGame("Sekiro: Shadows Die Twice", "sekiro-shadows-die-twice", 69999m, true, new[] { "PC", "PS5", "Xbox" }, new[] { "Action", "Adventure" });
            AddGame("Dark Souls III", "dark-souls-3", 59999m, true, new[] { "PC", "PS5", "Xbox" }, new[] { "RPG", "Action" });
            AddGame("Ghost of Tsushima", "ghost-of-tsushima", 109999m, true, new[] { "PS5" }, new[] { "Action", "Adventure" });
        }
    }
}
