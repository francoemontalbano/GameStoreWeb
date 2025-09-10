using Microsoft.AspNetCore.Mvc;
using GameStore.Api.Services;
using GameStore.Api.Domain.DTOs;

namespace GameStore.Api.Controllers
{
    // Controlador para autenticación (login, registro)
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;

        public AuthController(AuthService authService)
        {
            _authService = authService;
        }

        // POST /api/auth/register - Registrar nuevo usuario
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            try
            {
                // Validar datos del formulario
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                // Llamar al servicio de autenticación para registrar usuario
                var result = await _authService.RegisterAsync(request);
                return Ok(result);
            }
            catch (InvalidOperationException ex)
            {
                // Error de validación de negocio (email duplicado, etc.)
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                // Error interno del servidor
                return StatusCode(500, new { message = "Error interno del servidor" });
            }
        }

        // POST /api/auth/login - Iniciar sesión de usuario
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            try
            {
                // Validar datos del formulario
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                // Llamar al servicio de autenticación para iniciar sesión
                var result = await _authService.LoginAsync(request);
                return Ok(result);
            }
            catch (InvalidOperationException ex)
            {
                // Error de credenciales inválidas
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                // Error interno del servidor
                return StatusCode(500, new { message = "Error interno del servidor" });
            }
        }
    }
}
