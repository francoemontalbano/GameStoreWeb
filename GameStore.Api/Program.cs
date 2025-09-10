using GameStore.Api.Infrastructure.Persistence;
using GameStore.Api.Domain.Entities;
using GameStore.Api.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

// ========================================
// CONFIGURACIÓN DE LA APLICACIÓN .NET 8
// ========================================

var builder = WebApplication.CreateBuilder(args);

// ===== REGISTRO DE SERVICIOS =====
// Configuración de controladores para la API REST
builder.Services.AddControllers();

// Configuración de Swagger/OpenAPI para documentación automática de la API
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configuración de Entity Framework Core con SQL Server
builder.Services.AddDbContext<GameStoreDbContext>(opt =>
    opt.UseSqlServer(builder.Configuration.GetConnectionString("Default")));

// ===== CONFIGURACIÓN DE IDENTITY =====
// Configurar Identity con el usuario personalizado
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    // Configuración de contraseñas
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 6;

    // Configuración de usuarios
    options.User.RequireUniqueEmail = true;
    options.SignIn.RequireConfirmedEmail = false;
})
.AddEntityFrameworkStores<GameStoreDbContext>()
.AddDefaultTokenProviders();

// ===== CONFIGURACIÓN DE JWT =====
// Configurar autenticación JWT
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!)
        )
    };
});

// ===== REGISTRO DE SERVICIOS PERSONALIZADOS =====
builder.Services.AddScoped<AuthService>();

// ===== CONSTRUCCIÓN DE LA APLICACIÓN =====
var app = builder.Build();

// ===== CONFIGURACIÓN DEL PIPELINE HTTP =====
// Habilitar Swagger solo en entorno de desarrollo
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Configurar archivos estáticos para servir imágenes subidas
app.UseStaticFiles();

// Redireccionar HTTP a HTTPS para mayor seguridad
app.UseHttpsRedirection();

// Habilitar autenticación y autorización
app.UseAuthentication();
app.UseAuthorization();

// ===== INICIALIZACIÓN DE LA BASE DE DATOS =====
// Crear la base de datos si no existe y cargar datos de prueba
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<GameStoreDbContext>();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    
    db.Database.EnsureCreated();   // Crea la DB si no existe
    
    // Crear roles si no existen
    await CreateRolesAsync(roleManager);
    
    // Crear usuario admin por defecto
    await CreateAdminUserAsync(userManager);
    
    DevSeeder.Seed(db);            // Carga datos de prueba si faltan
}

// ===== CONFIGURACIÓN DE RUTAS =====
// Mapear todos los controladores como endpoints de la API
app.MapControllers();

// ===== MÉTODOS AUXILIARES =====
// Crear roles del sistema
static async Task CreateRolesAsync(RoleManager<IdentityRole> roleManager)
{
    string[] roles = { "Admin", "User" };
    
    foreach (var role in roles)
    {
        if (!await roleManager.RoleExistsAsync(role))
        {
            await roleManager.CreateAsync(new IdentityRole(role));
        }
    }
}

// Crear usuario administrador por defecto
static async Task CreateAdminUserAsync(UserManager<ApplicationUser> userManager)
{
    var adminEmail = "admin@gamestore.com";
    var adminUser = await userManager.FindByEmailAsync(adminEmail);
    
    if (adminUser == null)
    {
        adminUser = new ApplicationUser
        {
            UserName = adminEmail,
            Email = adminEmail,
            FirstName = "Admin",
            LastName = "User",
            CreatedAt = DateTime.UtcNow,
            IsActive = true
        };
        
        var result = await userManager.CreateAsync(adminUser, "Admin123!");
        if (result.Succeeded)
        {
            await userManager.AddToRoleAsync(adminUser, "Admin");
        }
    }
}

// ===== INICIAR LA APLICACIÓN =====
app.Run();