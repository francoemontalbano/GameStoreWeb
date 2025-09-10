using System.Linq;
using GameStore.Api.Domain.Entities;

namespace GameStore.Api.Infrastructure.Persistence
{
    // Clase para cargar datos de prueba en la base de datos
    public static class DevSeeder
    {
        public static void Seed(GameStoreDbContext db)
        {
            // Crear catálogos básicos
            string[] platforms = { "PC", "PS5", "Xbox", "Switch" };
            foreach (var platformName in platforms)
            {
                if (!db.Platforms.Any(p => p.Name == platformName))
                {
                    db.Platforms.Add(new Platform { Name = platformName });
                }
            }

            string[] genres = { "Acción", "RPG", "Indie", "Deportes", "Disparos", "Aventura", "Estrategia", "Carreras", "Lucha", "Plataformas", "Fiesta", "Familiar", "Simulación", "Cooperativo" };
            foreach (var genreName in genres)
            {
                if (!db.Genres.Any(g => g.Name == genreName))
                {
                    db.Genres.Add(new Genre { Name = genreName });
                }
            }

            db.SaveChanges();

            // Funciones auxiliares
            int GetPlatformId(string name) => db.Platforms.First(p => p.Name == name).Id;
            int GetGenreId(string name) => db.Genres.First(g => g.Name == name).Id;

            // Agrega un juego con sus plataformas y géneros
            void AddGame(string title, string slug, decimal price, bool digital, string[] platformNames, string[] genreNames, string imageUrl = "", string releaseDate = "")
            {
                if (db.Games.Any(x => x.Slug == slug)) return;

                var game = new Game 
                { 
                    Title = title, 
                    Slug = slug, 
                    Price = price, 
                    IsDigital = digital,
                    ImageUrl = imageUrl,
                    ReleaseDate = releaseDate
                };
                db.Games.Add(game);
                db.SaveChanges();

                foreach (var platformName in platformNames.Distinct())
                {
                    db.GamePlatforms.Add(new GamePlatform 
                    { 
                        GameId = game.Id, 
                        PlatformId = GetPlatformId(platformName) 
                    });
                }

                foreach (var genreName in genreNames.Distinct())
                {
                    db.GameGenres.Add(new GameGenre 
                    { 
                        GameId = game.Id, 
                        GenreId = GetGenreId(genreName) 
                    });
                }

                db.SaveChanges();
            }

            // Juegos de ejemplo (precios en pesos argentinos)
            AddGame("Elden Ring", "elden-ring", 99999m, true, 
                new[] { "PC", "PS5", "Xbox" }, 
                new[] { "RPG", "Acción" });
            
            AddGame("God of War Ragnarök", "god-of-war-ragnarok", 119999m, true, 
                new[] { "PS5" }, 
                new[] { "Acción", "Aventura" });
            
            AddGame("Horizon Forbidden West", "horizon-forbidden-west", 109999m, true, 
                new[] { "PS5" }, 
                new[] { "Acción", "Aventura" });
            
            AddGame("Marvel's Spider-Man 2", "spider-man-2", 129999m, true, 
                new[] { "PS5" }, 
                new[] { "Acción" });
            
            AddGame("The Last of Us Part I", "the-last-of-us-part-1", 119999m, true, 
                new[] { "PS5" }, 
                new[] { "Acción", "Aventura" });
            
            AddGame("Gran Turismo 7", "gran-turismo-7", 94999m, true, 
                new[] { "PS5" }, 
                new[] { "Carreras" });

            // Juegos físicos para PlayStation 5
            AddGame("Demon's Souls", "demons-souls", 89999m, false, 
                new[] { "PS5" }, 
                new[] { "RPG", "Acción" });
            
            AddGame("Returnal", "returnal", 109999m, false, 
                new[] { "PS5" }, 
                new[] { "Acción", "Disparos" });
            
            AddGame("Ratchet & Clank: Rift Apart", "ratchet-clank-rift-apart", 119999m, false, 
                new[] { "PS5" }, 
                new[] { "Acción", "Aventura" });
            
            AddGame("Sackboy: A Big Adventure", "sackboy-big-adventure", 79999m, false, 
                new[] { "PS5" }, 
                new[] { "Aventura", "Plataformas" });
            
            AddGame("Astro's Playroom", "astros-playroom", 49999m, false, 
                new[] { "PS5" }, 
                new[] { "Aventura", "Plataformas" });
            
            AddGame("Final Fantasy XVI", "final-fantasy-16", 139999m, false, 
                new[] { "PS5" }, 
                new[] { "RPG", "Acción" });
            
            AddGame("Forspoken", "forspoken", 119999m, false, 
                new[] { "PS5" }, 
                new[] { "RPG", "Acción" });
            
            AddGame("Hogwarts Legacy", "hogwarts-legacy", 129999m, false, 
                new[] { "PS5", "PC", "Xbox" }, 
                new[] { "RPG", "Aventura" });

            AddGame("Halo Infinite", "halo-infinite", 79999m, true, 
                new[] { "Xbox", "PC" }, 
                new[] { "Disparos", "Acción" });
            
            AddGame("Forza Horizon 5", "forza-horizon-5", 89999m, true, 
                new[] { "Xbox", "PC" }, 
                new[] { "Carreras" });
            
            AddGame("Gears 5", "gears-5", 59999m, true, 
                new[] { "Xbox", "PC" }, 
                new[] { "Disparos", "Acción" });
            
            AddGame("Starfield", "starfield", 129999m, true, 
                new[] { "Xbox", "PC" }, 
                new[] { "RPG", "Aventura" });
            
            AddGame("Sea of Thieves", "sea-of-thieves", 49999m, true, 
                new[] { "Xbox", "PC" }, 
                new[] { "Aventura" });

            // Juegos físicos para Xbox
            AddGame("Halo: The Master Chief Collection", "halo-master-chief-collection", 69999m, false, 
                new[] { "Xbox" }, 
                new[] { "Disparos", "Acción" });
            
            AddGame("Forza Motorsport 7", "forza-motorsport-7", 79999m, false, 
                new[] { "Xbox" }, 
                new[] { "Carreras" });
            
            AddGame("Gears of War 4", "gears-of-war-4", 59999m, false, 
                new[] { "Xbox" }, 
                new[] { "Disparos", "Acción" });
            
            AddGame("Ori and the Will of the Wisps", "ori-will-of-the-wisps", 69999m, false, 
                new[] { "Xbox" }, 
                new[] { "Aventura", "Plataformas" });
            
            AddGame("Cuphead", "cuphead-xbox", 39999m, false, 
                new[] { "Xbox" }, 
                new[] { "Indie", "Acción" });
            
            AddGame("Psychonauts 2", "psychonauts-2", 89999m, false, 
                new[] { "Xbox", "PC" }, 
                new[] { "Aventura", "Plataformas" });
            
            AddGame("Microsoft Flight Simulator", "microsoft-flight-simulator", 119999m, false, 
                new[] { "Xbox", "PC" }, 
                new[] { "Simulación" });

            AddGame("Zelda: Tears of the Kingdom", "zelda-tears-of-the-kingdom", 129999m, true, 
                new[] { "Switch" }, 
                new[] { "Aventura" });
            
            AddGame("Super Mario Odyssey", "super-mario-odyssey", 79999m, true, 
                new[] { "Switch" }, 
                new[] { "Aventura" });
            
            AddGame("Mario Kart 8 Deluxe", "mario-kart-8-deluxe", 69999m, true, 
                new[] { "Switch" }, 
                new[] { "Carreras" });
            
            AddGame("Metroid Dread", "metroid-dread", 89999m, true, 
                new[] { "Switch" }, 
                new[] { "Acción" });
            
            AddGame("Animal Crossing: New Horizons", "animal-crossing-new-horizons", 79999m, true, 
                new[] { "Switch" }, 
                new[] { "Estrategia" });

            // Juegos físicos para Nintendo Switch
            AddGame("The Legend of Zelda: Breath of the Wild", "zelda-breath-of-the-wild", 119999m, false, 
                new[] { "Switch" }, 
                new[] { "Adventure", "RPG" });
            
            AddGame("Super Smash Bros. Ultimate", "super-smash-bros-ultimate", 109999m, false, 
                new[] { "Switch" }, 
                new[] { "Lucha", "Acción" });
            
            AddGame("Pokémon Scarlet", "pokemon-scarlet", 129999m, false, 
                new[] { "Switch" }, 
                new[] { "RPG", "Aventura" });
            
            AddGame("Pokémon Violet", "pokemon-violet", 129999m, false, 
                new[] { "Switch" }, 
                new[] { "RPG", "Aventura" });
            
            AddGame("Splatoon 3", "splatoon-3", 99999m, false, 
                new[] { "Switch" }, 
                new[] { "Disparos", "Acción" });
            
            AddGame("Fire Emblem: Three Houses", "fire-emblem-three-houses", 109999m, false, 
                new[] { "Switch" }, 
                new[] { "RPG", "Estrategia" });
            
            AddGame("Xenoblade Chronicles 3", "xenoblade-chronicles-3", 119999m, false, 
                new[] { "Switch" }, 
                new[] { "RPG", "Acción" });
            
            AddGame("Bayonetta 3", "bayonetta-3", 109999m, false, 
                new[] { "Switch" }, 
                new[] { "Acción", "Aventura" });
            
            AddGame("Kirby and the Forgotten Land", "kirby-forgotten-land", 99999m, false, 
                new[] { "Switch" }, 
                new[] { "Aventura", "Plataformas" });
            
            AddGame("Luigi's Mansion 3", "luigis-mansion-3", 89999m, false, 
                new[] { "Switch" }, 
                new[] { "Adventure", "Action" });
            
            AddGame("Donkey Kong Country: Tropical Freeze", "donkey-kong-tropical-freeze", 79999m, false, 
                new[] { "Switch" }, 
                new[] { "Platform", "Action" });
            
            AddGame("Mario Party Superstars", "mario-party-superstars", 89999m, false, 
                new[] { "Switch" }, 
                new[] { "Fiesta", "Familiar" });

            AddGame("Baldur's Gate 3", "baldurs-gate-3", 139999m, true, 
                new[] { "PC", "PS5" }, 
                new[] { "RPG", "Aventura" });
            
            AddGame("Diablo IV", "diablo-iv", 119999m, true, 
                new[] { "PC", "PS5", "Xbox" }, 
                new[] { "Acción", "RPG" });
            
            AddGame("EA Sports FC 24", "ea-sports-fc-24", 109999m, true, 
                new[] { "PC", "PS5", "Xbox", "Switch" }, 
                new[] { "Deportes" });
            
            AddGame("NBA 2K24", "nba-2k24", 99999m, true, 
                new[] { "PC", "PS5", "Xbox", "Switch" }, 
                new[] { "Deportes" });
            
            AddGame("Call of Duty: Modern Warfare III", "cod-modern-warfare-3", 129999m, true, 
                new[] { "PC", "PS5", "Xbox" }, 
                new[] { "Disparos" });
            
            AddGame("Apex Legends", "apex-legends", 0m, true, 
                new[] { "PC", "PS5", "Xbox", "Switch" }, 
                new[] { "Disparos" });

            // Juegos físicos multiplataforma
            AddGame("Cyberpunk 2077", "cyberpunk-2077-physical", 89999m, false, 
                new[] { "PC", "PS5", "Xbox" }, 
                new[] { "RPG", "Acción" }, 
                "https://images.igdb.com/igdb/image/upload/t_cover_big/co2rpf.jpg", 
                "2020-12-10");
            
            AddGame("The Witcher 3: Wild Hunt - Complete Edition", "witcher-3-complete-physical", 79999m, false, 
                new[] { "PC", "PS5", "Xbox", "Switch" }, 
                new[] { "RPG", "Acción" });
            
            AddGame("Red Dead Redemption 2", "red-dead-redemption-2-physical", 99999m, false, 
                new[] { "PC", "PS5", "Xbox" }, 
                new[] { "Acción", "Aventura" });
            
            AddGame("Grand Theft Auto V", "gta-v-physical", 69999m, false, 
                new[] { "PC", "PS5", "Xbox" }, 
                new[] { "Acción", "Aventura" });
            
            AddGame("Minecraft", "minecraft-physical", 59999m, false, 
                new[] { "PC", "PS5", "Xbox", "Switch" }, 
                new[] { "Aventura", "Estrategia" });
            
            AddGame("Among Us", "among-us-physical", 29999m, false, 
                new[] { "PC", "PS5", "Xbox", "Switch" }, 
                new[] { "Fiesta", "Indie" });
            
            AddGame("Fall Guys", "fall-guys-physical", 39999m, false, 
                new[] { "PC", "PS5", "Xbox", "Switch" }, 
                new[] { "Fiesta", "Indie" });
            
            AddGame("It Takes Two", "it-takes-two-physical", 79999m, false, 
                new[] { "PC", "PS5", "Xbox" }, 
                new[] { "Aventura", "Cooperativo" });
            
            AddGame("Overcooked! 2", "overcooked-2-physical", 49999m, false, 
                new[] { "PC", "PS5", "Xbox", "Switch" }, 
                new[] { "Fiesta", "Cooperativo" });
            
            AddGame("Rocket League", "rocket-league-physical", 39999m, false, 
                new[] { "PC", "PS5", "Xbox", "Switch" }, 
                new[] { "Deportes", "Carreras" });

            AddGame("Hades", "hades", 49999m, true, 
                new[] { "PC", "PS5", "Xbox", "Switch" }, 
                new[] { "Indie", "Acción" });
            
            AddGame("Celeste", "celeste", 29999m, true, 
                new[] { "PC", "PS5", "Xbox", "Switch" }, 
                new[] { "Indie" });
            
            AddGame("Stardew Valley", "stardew-valley", 19999m, true, 
                new[] { "PC", "PS5", "Xbox", "Switch" }, 
                new[] { "Indie", "Estrategia" });
            
            AddGame("Cuphead", "cuphead", 29999m, true, 
                new[] { "PC", "Xbox", "Switch", "PS5" }, 
                new[] { "Indie" });
            
            AddGame("Sekiro: Shadows Die Twice", "sekiro-shadows-die-twice", 69999m, true, 
                new[] { "PC", "PS5", "Xbox" }, 
                new[] { "Acción", "Aventura" });
            
            AddGame("Dark Souls III", "dark-souls-3", 59999m, true, 
                new[] { "PC", "PS5", "Xbox" }, 
                new[] { "RPG", "Acción" });
            
            AddGame("Ghost of Tsushima", "ghost-of-tsushima", 109999m, true, 
                new[] { "PS5" }, 
                new[] { "Acción", "Aventura" });
        }
    }
}