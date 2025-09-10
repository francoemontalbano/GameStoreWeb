using Microsoft.AspNetCore.Identity;

namespace GameStore.Api.Domain.Entities
{
    // Usuario personalizado que extiende IdentityUser
    public class ApplicationUser : IdentityUser
    {
        // Propiedades adicionales del usuario
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public bool IsActive { get; set; } = true;
    }
}
