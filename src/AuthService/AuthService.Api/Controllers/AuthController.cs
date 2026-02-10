using AuthService.Api.Contracts.Responses;
using AuthService.Application.Accounts.Commands.ConfirmEmail;
using AuthService.Application.Accounts.Commands.Login;
using AuthService.Application.Accounts.Commands.RefreshTokenLogic;
using AuthService.Application.Accounts.Commands.RegisterAccount;
using AuthService.Application.Accounts.DTOs;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

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

    [EnableRateLimiting("AuthPolicy")]
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        _logger.LogInformation("Registration attempt for email: {Email}", request.Email);

        await _mediator.Send(new RegisterAccountCommand(request.Email, request.Password, request.ConfirmPassword));

        return Ok(new MessageResponse("Registration completed successfully"));
    }

    [EnableRateLimiting("AuthPolicy")]
    [HttpPost("confirm-email")]
    public async Task<IActionResult> ConfirmEmail([FromBody] ConfirmEmailRequest request)
    {
        _logger.LogInformation("Email confirmation attempt");

        await _mediator.Send(new ConfirmEmailCommand(request.Token));

        return Ok(new MessageResponse("Email confirmed successfully"));
    }

    [EnableRateLimiting("AuthPolicy")]
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        _logger.LogInformation("Login attempt for email: {Email}", request.Email);

        var result = await _mediator.Send(new LoginCommand(request.Email, request.Password));

        return Ok(result);
    }

    [EnableRateLimiting("RefreshTokenPolicy")]
    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh([FromBody] RefreshTokenRequest request)
    {
        _logger.LogInformation("Refresh token attempt");

        var result = await _mediator.Send(new RefreshTokenCommand(request.RefreshToken));

        return Ok(result);
    }

    [HttpGet("protected")]
    [Authorize]
    public IActionResult TestProtected()
    {
        return Ok(new MessageResponse("eeeeeee!"));
    }
}