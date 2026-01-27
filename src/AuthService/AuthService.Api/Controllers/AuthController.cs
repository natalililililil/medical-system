using AuthService.Application.Accounts.Commands.ConfirmEmail;
using AuthService.Application.Accounts.Commands.RegisterAccount;
using AuthService.Application.Accounts.DTOs;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AuthService.Api.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;
        public AuthController(IMediator mediator) => _mediator = mediator;

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            await _mediator.Send(new RegisterAccountCommand(request.Email, request.Password, request.ConfirmPassword));
            return Ok();
        }

        [HttpPost("confirm-email")]
        public async Task<IActionResult> ConfirmEmail([FromBody] ConfirmEmailRequest request)
        {
            await _mediator.Send(new ConfirmEmailCommand(request.Token));
            return Ok();
        }
    }
}
