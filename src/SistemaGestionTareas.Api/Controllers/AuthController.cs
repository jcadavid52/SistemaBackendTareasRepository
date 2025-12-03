using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SistemaGestionTareas.Api.Infrastructure;
using SistemaGestionTareas.ApplicationCore.Dtos.Request;
using SistemaGestionTareas.ApplicationCore.Dtos.Response;
using SistemaGestionTareas.ApplicationCore.Services;

namespace SistemaGestionTareas.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(AuthService authService) : ControllerBase
    {
        [AllowAnonymous]
        [HttpPost("login")]
        [ProducesResponseType(typeof(AuthorizedResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> LoginAsync(
            [FromBody] AuthLoginRequestDto loginRequest,
            CancellationToken cancellationToken = default
            )
        {
            var response = await authService.LoginAsync(loginRequest,cancellationToken);

            return Ok(response);
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] AuthRegisterRequestDto command, CancellationToken cancellationToken = default)
        {
            var response = await authService.RegisterAsync(command, cancellationToken);
            return Created("", response);
        }

        [AllowAnonymous]
        [HttpPost("refresh-token")]
        [ProducesResponseType(typeof(AuthorizedResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> RefreshToken(
            [FromBody] AuthRefreshTokenRequestDto request,
            CancellationToken cancellationToken = default
            )
        {
            var refreshTokenResult = await authService.RefreshTokenAsync(request,cancellationToken);
            return Ok(refreshTokenResult);
        }


    }
}
