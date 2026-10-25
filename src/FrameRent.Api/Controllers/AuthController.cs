using FrameRent.Application.DTOs.Auth;
using FrameRent.Application.Interfaces.Auth;
using FrameRent.Infrastructure.Persistence.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FrameRent.Api.Controllers;

[ApiController]
[Route("api/v1/auth")]
public class AuthController : ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;

    public AuthController(
        UserManager<ApplicationUser> userManager,
        IJwtTokenGenerator jwtTokenGenerator)
    {
        _userManager = userManager;
        _jwtTokenGenerator = jwtTokenGenerator;
    }

    [AllowAnonymous]
    [HttpPost("register")]
    public async Task<ActionResult<AuthResponse>> Register([FromBody] RegisterRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.Password))
            return BadRequest("Email y contraseña son obligatorios.");

        var existingUser = await _userManager.FindByEmailAsync(request.Email.Trim());
        if (existingUser is not null)
            return Conflict("Ya existe un usuario con ese correo electrónico.");

        var user = new ApplicationUser
        {
            UserName = request.Email.Trim(),
            Email = request.Email.Trim(),
            FirstName = request.FirstName.Trim(),
            LastName = request.LastName.Trim(),
            PhoneNumber = request.PhoneNumber.Trim()
        };

        var result = await _userManager.CreateAsync(user, request.Password);
        if (!result.Succeeded)
        {
            return BadRequest(string.Join("; ", result.Errors.Select(e => e.Description)));
        }

        await _userManager.AddToRoleAsync(user, "Customer");

        var token = _jwtTokenGenerator.GenerateToken(
            user.Id,
            user.Email!,
            $"{user.FirstName} {user.LastName}".Trim(),
            "Customer");

        return Ok(new AuthResponse(
            token,
            user.Email!,
            $"{user.FirstName} {user.LastName}".Trim(),
            "Customer",
            user.Id));
    }

    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<ActionResult<AuthResponse>> Login([FromBody] LoginRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.Password))
            return BadRequest("Email y contraseña son obligatorios.");

        var user = await _userManager.FindByEmailAsync(request.Email.Trim());
        if (user is null)
            return Unauthorized("Credenciales inválidas.");

        var isPasswordValid = await _userManager.CheckPasswordAsync(user, request.Password);
        if (!isPasswordValid)
            return Unauthorized("Credenciales inválidas.");

        var roles = await _userManager.GetRolesAsync(user);
        var role = roles.FirstOrDefault() ?? "Customer";

        var token = _jwtTokenGenerator.GenerateToken(
            user.Id,
            user.Email!,
            $"{user.FirstName} {user.LastName}".Trim(),
            role);

        return Ok(new AuthResponse(
            token,
            user.Email!,
            $"{user.FirstName} {user.LastName}".Trim(),
            role,
            user.Id));
    }
}
