namespace GameStore.Api.Domain.Entities;

// Entidad que representa una plataforma de videojuegos
public class Platform
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
}

// Entidad que representa un género de videojuegos
public class Genre
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
}

// Tabla de unión para la relación muchos-a-muchos entre Game y Platform
public class GamePlatform
{
    public int GameId { get; set; }
    public Game Game { get; set; } = null!;
    public int PlatformId { get; set; }
    public Platform Platform { get; set; } = null!;
}

// Tabla de unión para la relación muchos-a-muchos entre Game y Genre
public class GameGenre
{
    public int GameId { get; set; }
    public Game Game { get; set; } = null!;
    public int GenreId { get; set; }
    public Genre Genre { get; set; } = null!;
}