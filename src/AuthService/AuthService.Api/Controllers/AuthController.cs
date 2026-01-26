using AuthService.Application.Accounts.Commands;
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
            if (request.Password != request.ConfirmPassword)
                return BadRequest("Passwords do not match");

            await _mediator.Send(new RegisterAccountCommand(request.Email, request.Password));
            return Ok();
        }

    }
}
