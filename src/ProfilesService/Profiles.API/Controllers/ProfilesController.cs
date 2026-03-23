using MediatR;
using Microsoft.AspNetCore.Mvc;
using Profiles.Application.Features.Commands.CreateDoctor;
using Profiles.Application.Features.Commands.CreatePatient;
using Profiles.Application.Features.Commands.CreateReceptionist;

namespace Profiles.API.Controllers;

[ApiController]
[Route("api/profiles")]
public class ProfilesController(IMediator _mediator) : ControllerBase
{
    [HttpPost("patients")]
    public async Task<ActionResult> CreatePatient([FromBody] CreatePatientCommand command)
    {
        await _mediator.Send(command);
        return StatusCode(201);
    }

    [HttpPost("doctors")]
    public async Task<IActionResult> CreateDoctor([FromBody] CreateDoctorCommand command)
    {
        await _mediator.Send(command);
        return StatusCode(201);
    }
    [HttpPost("receptionist")]
    public async Task<IActionResult> CreateReceptionist([FromBody] CreateReceptionistCommand command)
    {
        await _mediator.Send(command);
        return StatusCode(201);
    }
}