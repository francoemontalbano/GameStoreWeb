namespace GameStore.Api.Domain.Entities
{
    // Entidad que representa un videojuego en la tienda
    public class Game
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public bool IsDigital { get; set; }
        public string ReleaseDate { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty; // URL de la imagen/portada del juego

        // Propiedades de navegación para las relaciones muchos-a-muchos
        public ICollection<GamePlatform> GamePlatforms { get; set; } = new List<GamePlatform>();
        public ICollection<GameGenre> GameGenres { get; set; } = new List<GameGenre>();
    }
}