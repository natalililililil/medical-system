using AuthService.Application.Accounts.Commands.ConfirmEmail;
using AuthService.Application.Accounts.Commands.Login;
using AuthService.Application.Accounts.Commands.RefreshTokenLogic;
using AuthService.Application.Accounts.Commands.RegisterAccount;
using AuthService.Application.Accounts.DTOs;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuthService.Api.Controllers;

[Route("api/auth")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<AuthController> _logger;
    public AuthController(IMediator mediator, ILogger<AuthController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        _logger.LogInformation("Registration attempt for email: {Email}", request.Email);

        await _mediator.Send(new RegisterAccountCommand(request.Email, request.Password, request.ConfirmPassword));

        _logger.LogInformation("Registration successful for email: {Email}", request.Email);

        return Ok(new { message = "Registration completed successfully" });
    }

    [HttpPost("confirm-email")]
    public async Task<IActionResult> ConfirmEmail([FromBody] ConfirmEmailRequest request)
    {
        _logger.LogInformation("Email confirmation attempt");

        await _mediator.Send(new ConfirmEmailCommand(request.Token));

        _logger.LogInformation("Email confirmation successful");

        return Ok(new { message = "Email confirmed successfully" });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginCommand command)
    {
        _logger.LogInformation("Login attempt for email: {Email}", command.Email);

        var result = await _mediator.Send(command);

        _logger.LogInformation("Login successful for email: {Email}", command.Email);

        return Ok(result);
    }

    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh([FromBody] RefreshTokenRequest request)
    {
        _logger.LogInformation("Refresh token attempt");

        var result = await _mediator.Send(new RefreshTokenCommand(request.RefreshToken));

        _logger.LogInformation("Refresh token successful");

        return Ok(result);
    }

    [HttpGet("protected")]
    [Authorize]
    public async Task<IActionResult> TestProtected()
    {
        return Ok(new { message = "eeeeeee!" });
    }
}