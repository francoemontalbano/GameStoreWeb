namespace GameStore.Api.Domain.DTOs
{
    // DTO para registro de usuario
    public class RegisterRequest
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string ConfirmPassword { get; set; } = string.Empty;
    }

    // DTO para login de usuario
    public class LoginRequest
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }

    // DTO para respuesta de autenticación
    public class AuthResponse
    {
        public string Token { get; set; } = string.Empty;
        public string RefreshToken { get; set; } = string.Empty;
        public DateTime Expiration { get; set; }
        public UserInfo User { get; set; } = new();
    }

    // DTO para información del usuario
    public class UserInfo
    {
        public string Id { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public List<string> Roles { get; set; } = new();
    }

    // DTO para crear/editar juegos (admin)
    public class GameCreateRequest
    {
        public string Title { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public bool IsDigital { get; set; }
        public string ImageUrl { get; set; } = string.Empty;
        public List<int> PlatformIds { get; set; } = new();
        public List<int> GenreIds { get; set; } = new();
    }

    public class GameUpdateRequest
    {
        public string Title { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public bool IsDigital { get; set; }
        public string ImageUrl { get; set; } = string.Empty;
        public List<int> PlatformIds { get; set; } = new();
        public List<int> GenreIds { get; set; } = new();
    }
}
