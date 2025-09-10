namespace GameStore.Api.Domain.Entities;

public class Platform
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
}

public class Genre
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
}

// Tablas de unión (muchos-a-muchos)
public class GamePlatform
{
    public int GameId { get; set; }
    public Game Game { get; set; } = null!;
    public int PlatformId { get; set; }
    public Platform Platform { get; set; } = null!;
}

public class GameGenre
{
    public int GameId { get; set; }
    public Game Game { get; set; } = null!;
    public int GenreId { get; set; }
    public Genre Genre { get; set; } = null!;
}
